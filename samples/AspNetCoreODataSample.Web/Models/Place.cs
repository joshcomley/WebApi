using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.OData.NetTopology.Conversion;
using Microsoft.Spatial;
using NetTopologySuite.Geometries;

namespace AspNetCoreODataSample.Web.Models
{
    public class Place
    {
        public int ID { get; set; }

        public string Name { get; set; }

        private PointWrapper _point;
        [ValidateNever]
        public Point Location
        {
            get => _point;
            set => _point = value;
        }
        [NotMapped]
        public GeographyPoint EdmLocation
        {
            get => _point;
            set => _point = value;
        }
    }
}