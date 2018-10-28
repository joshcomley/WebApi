using System.Collections.Generic;
using Microsoft.AspNet.OData.Common;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace Microsoft.AspNet.OData.Query
{
    /// <summary>
    /// Represents an order by <see cref="IEdmProperty"/> expression.
    /// </summary>
    public class OrderBySingleValueFunctionNode : OrderByNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderByPropertyNode"/> class.
        /// </summary>
        /// <param name="orderByClause">The orderby clause representing property access.</param>
        public OrderBySingleValueFunctionNode(OrderByClause orderByClause)
            : base(orderByClause)
        {
            if (orderByClause == null)
            {
                throw Error.ArgumentNull("orderByClause");
            }

            OrderByClause = orderByClause;
            Direction = orderByClause.Direction;

            SingleValueFunctionCallNode propertyExpression = orderByClause.Expression as SingleValueFunctionCallNode;
            if (propertyExpression == null)
            {
                throw new ODataException(SRResources.OrderByClauseNotSupported);
            }
        }
        
        /// <summary>
        /// Gets the <see cref="OrderByClause"/> of this node.
        /// </summary>
        public OrderByClause OrderByClause { get; private set; }
    }
}