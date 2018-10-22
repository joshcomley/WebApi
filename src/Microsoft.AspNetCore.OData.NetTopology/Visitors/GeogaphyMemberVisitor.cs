using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNet.OData.Query.Expressions;
using Microsoft.AspNetCore.OData.NetTopology.Conversion;
using Microsoft.AspNetCore.OData.NetTopology.Mapping;
using Microsoft.Spatial;

namespace Microsoft.AspNetCore.OData.NetTopology.Visitors
{
    internal class GeogaphyMemberVisitor : ExpressionVisitor
    {
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member is PropertyInfo info && 
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
                    return Convert(node, container, p => p.ToNtsPoint());
                }
                case ConstantExpression expression
                    when expression.Type ==
                         typeof(LinqParameterContainer.TypedLinqParameterContainer<GeographyPolygon>) &&
                         expression.Value is LinqParameterContainer.TypedLinqParameterContainer<GeographyPolygon>
                             container:
                {
                    return Convert(node, container, p => p.ToNtsPolygon());
                }
                case ConstantExpression expression
                    when expression.Type ==
                         typeof(LinqParameterContainer.TypedLinqParameterContainer<GeographyLineString>) &&
                         expression.Value is LinqParameterContainer.TypedLinqParameterContainer<GeographyLineString>
                             container:
                {
                    return Convert(node, container, p => p.ToNtsLineString());
                }
            }

            return base.VisitMember(node);
        }

        private Expression Convert<TEdm, TNts>(
            MemberExpression node,
            LinqParameterContainer.TypedLinqParameterContainer<TEdm> container,
            Func<TEdm, TNts> convert)
        {
            if (container.Property is TEdm geographyPoint)
            {
                var ntsPoint = convert(geographyPoint);
                var result = Expression.Property(
                    VisitConstant(
                        Expression.Constant(
                            new LinqParameterContainer.TypedLinqParameterContainer<TNts>(
                                ntsPoint))),
                    node.Member.Name);
                return result;
            }
            return null;
        }
    }
}