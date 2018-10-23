using Microsoft.AspNet.OData.Interfaces;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Builder;
using Microsoft.OData.Edm;

namespace Microsoft.AspNet.OData
{
    /// <summary>
    /// ODataBuilder extensions
    /// </summary>
    public static class ODataNetTopologySuiteExtensions
    {
        /// <summary>
        /// Use OData route with default route name and route prefix.
        /// </summary>
        /// <param name="odata"></param>
        /// <param name="model">The <see cref="IEdmModel"/> to use.</param>
        /// <returns>The <see cref="IApplicationBuilder "/>.</returns>
        public static IODataBuilder UseODataNetTopologySuite(this IODataBuilder odata, IEdmModel model)
        {
            ExpressionHookRegistrar.Instance.Register(model, new NetTopologySuiteExpressionHook());
            return odata;
        }
    }
}