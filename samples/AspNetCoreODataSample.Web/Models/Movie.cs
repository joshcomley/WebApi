// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.OData.NetTopology.Conversion;
using Microsoft.Spatial;
using NetTopologySuite.Geometries;

namespace AspNetCoreODataSample.Web.Models
{
    public class Movie
    {
        public int ID { get; set; }

        public List<MovieStar> Stars { get; set; }

        private PointWrapper _point;
        public Point Point
        {
            get => _point;
            set => _point = value;
        }
        [NotMapped]
        public GeographyPoint LocationPoint
        {
            get => _point;
            set => _point = value;
        }

        private PolygonWrapper _polygonWrapper;
        public Polygon PolygonDb
        {
            get => _polygonWrapper;
            set => _polygonWrapper = value;
        }

        [NotMapped]
        public GeographyPolygon Polygon
        {
            get => _polygonWrapper;
            set => _polygonWrapper = value;
        }


        private LineStringWrapper _lineStringWrapper;
        public LineString LineString
        {
            get => _lineStringWrapper;
            set => _lineStringWrapper = value;
        }

        [NotMapped]
        public GeographyLineString LineStringEdm
        {
            get => _lineStringWrapper;
            set => _lineStringWrapper = value;
        }

        public string Title { get; set; }

        public DateTimeOffset ReleaseDate { get; set; }

        public Genre Genre { get; set; }

        public decimal Price { get; set; }
    }
}
