using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace Microsoft.AspNet.OData.Spatial
{
    internal class GeoHelper
    {
        public static Point CreatePoint(double lat, double lng)
        {
            var coord = new Coordinate(lng, lat);
            var geomFactory = new GeometryFactory(new PrecisionModel(), 4326);
            return (Point)geomFactory.CreatePoint(coord);
        }
    }
}