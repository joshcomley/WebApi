using System.Linq;
using AspNetCoreODataSample.Web.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Spatial;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreODataSample.Web.Controllers
{
    public class BooksController : ODataController
    {
        private MovieContext _context;

        public BooksController(MovieContext context)
        {
            _context = context;
            //if (!_context.Books.Any())
            //{
            //    _context.Books.AddRange(
            //        new[]
            //        {
            //            new Book
            //            {
            //                Name = "My Book"
            //            },
            //            new Book
            //            {
            //                Name = "Your Book",
            //            },
            //        });
            //    _context.SaveChanges();
            //}
        }


        [EnableQuery]
        public IActionResult Get()
        {
            return Request.Path.Value.Contains("efcore")
                ? Ok(_context.Places)
                : Ok(new Place[] { }.AsQueryable());
        }

        [HttpPost]
        [EnableQuery]
        public IActionResult Post([FromBody]Book place)
        {
            return Ok(place);
        }
    }
}