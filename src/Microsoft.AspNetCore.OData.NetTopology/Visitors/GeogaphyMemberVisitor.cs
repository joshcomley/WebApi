using System.Linq.Expressions;
using System.Reflection;
using GeoAPI.Geometries;
using Microsoft.AspNet.OData.Query.Expressions;
using Microsoft.AspNetCore.OData.NetTopology.Conversion;
using Microsoft.AspNetCore.OData.NetTopology.Mapping;
using Microsoft.Spatial;
using NetTopologySuite.Geometries;

namespace Microsoft.AspNetCore.OData.NetTopology.Visitors
{
    internal class GeogaphyMemberVisitor : ExpressionVisitor
    {
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member is PropertyInfo info && node.Type == typeof(GeographyPoint) &&
                !typeof(LinqParameterContainer).IsAssignableFrom(node.Expression.Type))
            {
                var mapping = GeographyMapping.Instance.GetMappedProperty(info);
                if (mapping != null)
                {
                    return Expression.Property(node.Expression, mapping.Name);
                }
            }

            switch (node.Expression)
            {
                case ConstantExpression constantExpression
                    when constantExpression.Type ==
                         typeof(LinqParameterContainer.TypedLinqParameterContainer<GeographyPoint>) &&
                         constantExpression.Value is LinqParameterContainer.TypedLinqParameterContainer<GeographyPoint>
                             container:
                {
                    if (container.Property is GeographyPoint geographyPoint)
                    {
                        var result = Expression.Property(
                            VisitConstant(
                                Expression.Constant(
                                    new LinqParameterContainer.TypedLinqParameterContainer<IPoint>(
                                        geographyPoint.ToNtsPoint()))),
                            node.Member.Name);
                        return result;
                    }
                    break;
                }
                case ConstantExpression expression
                    when expression.Type ==
                         typeof(LinqParameterContainer.TypedLinqParameterContainer<GeographyPolygon>) &&
                         expression.Value is LinqParameterContainer.TypedLinqParameterContainer<GeographyPolygon>
                             container:
                {
                    var geographyPolygon = container.Property as GeographyPolygon;
                    var ntsPolygon = geographyPolygon.ToNtsPolygon();
                    var result = Expression.Property(
                        VisitConstant(
                            Expression.Constant(
                                new LinqParameterContainer.TypedLinqParameterContainer<Polygon>(ntsPolygon))),
                        node.Member.Name);
                    return result;
                }
            }

            return base.VisitMember(node);
        }
    }
}