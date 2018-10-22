using System.Linq.Expressions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Query.Expressions;
using Microsoft.AspNetCore.OData.NetTopology.Visitors;
using Microsoft.OData.UriParser;

namespace Microsoft.AspNet.OData
{
    internal class NetTopologySuiteExpressionHook : ExpressionHook
    {
        private GeographyVisitor GeographyVisitor { get; } = new GeographyVisitor();

        public override Expression Hook(Expression expression, QueryNode node)
        {
            if (node is SingleValueFunctionCallNode single)
            {
                if (single.Name == ClrCanonicalFunctions.GeoDistanceFunctionName ||
                    single.Name == ClrCanonicalFunctions.GeoIntersectsFunctionName ||
                    single.Name == ClrCanonicalFunctions.GeoLengthFunctionName)
                {
                    var visit = GeographyVisitor.Visit(expression);
                    return visit;
                }
            }
            return expression;
        }
    }
}