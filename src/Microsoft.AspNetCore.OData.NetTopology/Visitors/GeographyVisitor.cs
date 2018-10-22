using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNet.OData.Query.Expressions;

namespace Microsoft.AspNetCore.OData.NetTopology.Visitors
{
    internal class GeographyVisitor : ExpressionVisitor
    {
        private GeogaphyMemberVisitor MemberVisitor { get; } = new GeogaphyMemberVisitor();

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method != ClrCanonicalFunctions.GeoDistance)
            {
                return base.VisitMethodCall(node);
            }

            var args = ExpressionBinderBase.ExtractValueFromNullableArguments(node.Arguments).ToArray();
            var methodCallExpression = Expression.Call(
                MemberVisitor.Visit(args.First()),
                GeographyMethods.GeoDistanceEf,
                MemberVisitor.Visit(args.Skip(1).First()));
            return methodCallExpression;

        }
    }
}