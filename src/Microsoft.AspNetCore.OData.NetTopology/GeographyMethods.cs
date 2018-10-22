using System.Reflection;
using GeoAPI.Geometries;
using Microsoft.AspNet.OData.Query.Expressions;

namespace Microsoft.AspNetCore.OData.NetTopology
{
    internal class GeographyMethods : ClrCanonicalFunctions
    {
        private static readonly IPoint DefaultPoint = default(IPoint);
        private static readonly IGeometry DefaultGeometry = default(IGeometry);

        public static readonly MethodInfo GeoIntersectsEf =
            MethodOf(_ => DefaultGeometry.Intersects(default(IGeometry)));

        public static readonly MethodInfo GeoDistanceEf = 
            MethodOf(_ => DefaultPoint.Distance(default(IGeometry)));
    }
}