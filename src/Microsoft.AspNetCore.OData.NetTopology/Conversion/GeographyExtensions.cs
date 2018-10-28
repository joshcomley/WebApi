using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GeoAPI.Geometries;
using Microsoft.Spatial;
using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Implementation;
using Geometry = NetTopologySuite.Geometries.Geometry;

namespace Microsoft.AspNetCore.OData.NetTopology.Conversion
{
    /// <summary>
    ///     Geography extensions for conversion between Microsoft.Spatial and NTS types
    /// </summary>
    public static class GeographyExtensions
    {
        private const int Srid = 4326;
        private static GeometryFactory GeographyFactory { get; } 
            = new GeometryFactory(new PrecisionModel(), Srid);
        /// <summary>
        ///     Converts an NTS LineString to a Microsoft.Spatial GeogaphyLineString.
        /// </summary>
        /// <param name="lineString">The NTS LineString.</param>
        /// <returns></returns>
        public static GeographyLineString ToGeographyLineString(this Geometry lineString)
        {
            if (lineString == null)
            {
                return null;
            }

            Debug.Assert(lineString.GeometryType == "LineString");
            var builder = SpatialBuilder.Create();
            var pipeLine = builder.GeographyPipeline;
            pipeLine.SetCoordinateSystem(CoordinateSystem.DefaultGeography);
            pipeLine.BeginGeography(SpatialType.LineString);

            var numPionts = lineString.NumPoints;
            for (var n = 0; n < numPionts; n++)
            {
                var pointN = lineString.GetGeometryN(n + 1);
                var lat = pointN.Coordinate.Y;
                var lon = pointN.Coordinate.X;
                var alt = pointN.Coordinate.Z;
                var m = pointN.Length;
                var position = new GeographyPosition(lat, lon, alt, m);
                if (n == 0)
                {
                    pipeLine.BeginFigure(position);
                }
                else
                {
                    pipeLine.LineTo(position);
                }
            }

            pipeLine.EndFigure();
            pipeLine.EndGeography();
            return (GeographyLineString)builder.ConstructedGeography;
        }

        /// <summary>
        ///     Converts a Microsoft.Spatial GeographyLineString to an NTS LineString.
        /// </summary>
        /// <param name="lineString">The Microsoft.Spatial GeographyLineString.</param>
        /// <returns></returns>
        public static LineString ToNtsLineString(this GeographyLineString lineString)
        {
            var coords = new List<Coordinate>();
            foreach (var coord in lineString.Points)
            {
                coords.Add(new Coordinate(coord.Longitude, coord.Latitude, coord.Z ?? 0));
            }
            var ntsLineString = GeographyFactory.CreateLineString(coords.ToArray());
            return (LineString) ntsLineString;
        }

        /// <summary>
        ///     Converts an NTS Point to a Microsoft.Spatial GeogaphyPoint.
        /// </summary>
        /// <param name="point">The NTS Point.</param>
        /// <returns></returns>
        public static GeographyPoint ToGeographyPoint(this Point point)
        {
            if (point == null)
            {
                return null;
            }

            Debug.Assert(point.GeometryType == "Point");
            var lat = point.Y;
            var lon = point.X;
            var alt = point.Z;
            var m = point.Length;
            return GeographyPoint.Create(lat, lon, alt, m);
        }

        /// <summary>
        ///     Converts a Microsoft.Spatial GeographyPoint to an NTS Point.
        /// </summary>
        /// <param name="geographyPoint">The Microsoft.Spatial GeographyPoint.</param>
        /// <returns></returns>
        public static Point ToNtsPoint(this GeographyPoint geographyPoint)
        {
            var lat = geographyPoint.Latitude;
            var lon = geographyPoint.Longitude;
            var coord = new Coordinate(lon, lat);
            return (Point)GeographyFactory.CreatePoint(coord);
        }

        /// <summary>
        ///     Converts a Microsoft.Spatial GeographyPolygon to a Polygon.
        /// </summary>
        /// <param name="geographyPolygon">The Microsoft.Spatial GeographyPolygon.</param>
        /// <returns></returns>
        public static Polygon ToNtsPolygon(this GeographyPolygon geographyPolygon)
        {
            var coords = new List<Coordinate>();
            foreach (var ring in geographyPolygon.Rings)
            {
                foreach (var coord in ring.Points)
                {
                    coords.Add(new Coordinate(coord.Longitude, coord.Latitude, coord.Z ?? 0));
                }
            }

            var geomFactory = new GeometryFactory(new PrecisionModel(), 4326);
            coords.RemoveAt(coords.Count - 1);
            coords.Sort(new CoordinateComparer(CalculateCentre(coords)));
            var first = coords.First();
            coords.Add(new Coordinate(first.X, first.Y, first.Z));
            var poly = new Polygon(
                new LinearRing(new CoordinateArraySequence(coords.ToArray()), geomFactory),
                geomFactory);
            return poly;
        }

        /// <summary>
        ///     Converts an NTS Polygon to a Microsoft.Spatial GeographyPolygon
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static GeographyPolygon ToGeographyPolygon(this Polygon polygon)
        {
            var builder = SpatialImplementation.CurrentImplementation.CreateBuilder();
            builder.GeographyPipeline.SetCoordinateSystem(CoordinateSystem.DefaultGeography);
            builder.GeographyPipeline.BeginGeography(SpatialType.Polygon);
            BuildRing(polygon.ExteriorRing, builder);
            foreach (var ring in polygon.Holes)
            {
                BuildRing(ring, builder);
            }

            builder.GeographyPipeline.EndGeography();
            return (GeographyPolygon) builder.ConstructedGeography;
        }

        private static void BuildRing(IGeometry ring, SpatialPipeline builder)
        {
            var coords = ring.Coordinates.ToList();
            coords.RemoveAt(coords.Count - 1);
            coords.Sort(new CoordinateComparer(CalculateCentre(coords)));
            var first = coords.First();
            builder.GeographyPipeline.BeginFigure(new GeographyPosition(first.Y, first.X, first.Z, null));
            for (var i = 1; i < coords.Count; i++)
            {
                var next = coords[i];
                builder.GeographyPipeline.LineTo(new GeographyPosition(next.Y, next.X, next.Z, null));
            }

            builder.GeographyPipeline.LineTo(new GeographyPosition(first.Y, first.X, first.Z, null));
            builder.GeographyPipeline.EndFigure();
        }

        private static Coordinate CalculateCentre(ICollection<Coordinate> points)
        {
            double totalX = 0, totalY = 0;
            foreach (var p in points)
            {
                totalX += p.X;
                totalY += p.Y;
            }

            var centerX = totalX / points.Count;
            var centerY = totalY / points.Count;
            return new Coordinate(centerX, centerY);
        }

        private class CoordinateComparer : IComparer<Coordinate>
        {
            private const double FloatingPointTolerance = 0.00001;

            public CoordinateComparer(Coordinate center)
            {
                Center = center;
            }

            private Coordinate Center { get; }

            public int Compare(Coordinate a, Coordinate b)
            {
                Debug.Assert(a != null);
                Debug.Assert(b != null);
                if (a.X - Center.X >= 0 && b.X - Center.X < 0)
                {
                    return 1;
                }

                if (a.X - Center.X < 0 && b.X - Center.X >= 0)
                {
                    return 1;
                }

                if (Math.Abs(a.X - Center.X) < FloatingPointTolerance &&
                    Math.Abs(b.X - Center.X) < FloatingPointTolerance)
                {
                    if (a.Y - Center.Y >= 0 || b.Y - Center.Y >= 0)
                    {
                        return a.Y > b.Y ? 1 : -1;
                    }

                    return b.Y > a.Y ? 1 : -1;
                }

                // compute the cross product of vectors (center -> a) x (center -> b)
                var det = (a.X - Center.X) * (b.Y - Center.Y) - (b.X - Center.X) * (a.Y - Center.Y);
                if (det < 0)
                {
                    return 1;
                }

                if (det > 0)
                {
                    return -1;
                }

                // Points a and b are on the same line from the center
                // check which Point is closer to the center
                var d1 = (a.X - Center.X) * (a.X - Center.X) + (a.Y - Center.Y) * (a.Y - Center.Y);
                var d2 = (b.X - Center.X) * (b.X - Center.X) + (b.Y - Center.Y) * (b.Y - Center.Y);
                return d1 > d2 ? 1 : -1;
            }
        }
    }
}