using System.Collections.Generic;
using System.Linq;
using AspNetCoreODataSample.Web.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreODataSample.Web.Controllers
{
    public class MovieLanguagesController : ODataController
    {
        private readonly MovieContext _context;

        public MovieLanguagesController(MovieContext context)
        {
            _context = context;
            if (!_context.Languages.Any())
            {
                var lang1 = new Language
                {
                    Name = "English"
                };
                _context.Languages.Add(lang1);
                _context.SaveChanges();
            }
            if (!_context.MovieLanguages.Any())
            {
                var lang1 = new MovieLanguage
                {
                    LanguageId = 1,
                    MovieId = 1
                };
                _context.MovieLanguages.Add(lang1);
                _context.SaveChanges();
            }
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_context.MovieLanguages);
        }

        [EnableQuery]
        public IActionResult Get([ModelBinder(typeof(KeyValueBinder))]KeyValuePair<string, object>[] key)
        {
            return Ok(_context.MovieLanguages.SingleOrDefault());
        }
    }
}