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
            if (node.Method == ClrCanonicalFunctions.GeoDistance)
            {
                var args = ExpressionBinderBase.ExtractValueFromNullableArguments(node.Arguments).ToArray();
                var methodCallExpression = Expression.Call(
                    MemberVisitor.Visit(args.First()),
                    GeographyMethods.GeoDistanceEf,
                    MemberVisitor.Visit(args.Skip(1).First()));
                return methodCallExpression;
            }

            if (node.Method == ClrCanonicalFunctions.GeoIntersects)
            {
                var args = ExpressionBinderBase.ExtractValueFromNullableArguments(node.Arguments).ToArray();
                var methodCallExpression = Expression.Call(
                    MemberVisitor.Visit(args.First()),
                    GeographyMethods.GeoIntersectsEf,
                    MemberVisitor.Visit(args.Skip(1).First()));
                return methodCallExpression;
            }

            if (node.Method == ClrCanonicalFunctions.GeoLength)
            {
                var args = ExpressionBinderBase.ExtractValueFromNullableArguments(node.Arguments).ToArray();
                var arg = MemberVisitor.Visit(args.First());
                var methodCallExpression = Expression.Property(
                    arg,
                    GeographyMethods.GeoLengthEf.Name);
                return methodCallExpression;
            }

            return base.VisitMethodCall(node);

        }
    }
}