using System.Linq;
using AspNetCoreODataSample.Web.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Spatial;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreODataSample.Web.Controllers
{
    public class PlacesController : ODataController
    {
        private MovieContext _context;

        public PlacesController(MovieContext context)
        {
            _context = context;
            if (!_context.Places.Any())
            {
                _context.Places.AddRange(
                    new[]
                    {
                        new Place
                        {
                            Name = "New York",
                            Location = GeoHelper.CreatePoint(40.6976684, -74.2605664)
                        },
                        new Place
                        {
                            Name = "Droitwich",
                            Location = GeoHelper.CreatePoint(52.2596183, -2.1707715)
                        },
                        new Place
                        {
                            Name = "Brighton",
                            Location = GeoHelper.CreatePoint(50.8375053, -0.1764017)
                        },
                        new Place
                        {
                            Name = "London",
                            Location = GeoHelper.CreatePoint(51.5287336, -0.3824757)
                        },
                        new Place
                        {
                            Name = "Paris",
                            Location = GeoHelper.CreatePoint(48.8589506, 2.2768478)
                        }
                    });
                _context.SaveChanges();
            }
        }


        [EnableQuery]
        public IActionResult Get()
        {
            return Request.Path.Value.Contains("efcore")
                ? Ok(_context.Places)
                : Ok(new Place[] { }.AsQueryable());
        }

        [HttpPut]
        [HttpPatch]
        public IActionResult Patch([FromODataUri]int key, [FromBody]Delta<Place> delta)
        {
            var place = _context.Places.Single(_ => _.ID == key);
            delta.Patch(place);
            _context.SaveChanges();
            return Ok(place);
        }
    }
}