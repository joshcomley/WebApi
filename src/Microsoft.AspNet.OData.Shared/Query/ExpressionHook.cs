using System.Linq.Expressions;
using Microsoft.OData.UriParser;

namespace Microsoft.AspNet.OData.Query
{
    internal abstract class ExpressionHook
    {
        public abstract Expression Hook(Expression expression, QueryNode node);
    }
}
