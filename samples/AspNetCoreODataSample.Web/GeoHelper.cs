using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace Microsoft.AspNet.OData.Spatial
{
    internal class GeoHelper
    {
        public static IPoint CreatePoint(double lat, double lng)
        {
            var coord = new Coordinate(lat, lng);
            var geomFactory = new GeometryFactory(new PrecisionModel(), 4326);
            return geomFactory.CreatePoint(coord);
        }
    }
}