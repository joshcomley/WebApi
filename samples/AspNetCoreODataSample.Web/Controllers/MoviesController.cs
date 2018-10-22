// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreODataSample.Web.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Spatial;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

namespace AspNetCoreODataSample.Web.Controllers
{
    public class MoviesController : ODataController
    {
        private readonly MovieContext _context;

        private readonly IList<Movie> _inMemoryMovies;

        public MoviesController(MovieContext context)
        {
            _context = context;

            if (!_context.Movies.Any())
            {
                var m = new Movie
                {
                    Title = "Conan",
                    ReleaseDate = new DateTimeOffset(new DateTime(2017, 3, 3)),
                    Genre = Genre.Comedy,
                    Price = 1.99m,
                    Point = (Point)GeoHelper.CreatePoint(52.2670551, -2.137972)
                };
                _context.Movies.Add(m);
                _context.SaveChanges();
                _context.People.Add(
                    new Person
                    {
                        FirstName = "Idris",
                        LastName = "Elba",
                        FavoriteMovieId = m.ID,
                        DynamicProperties = new Dictionary<string, object>
                        {
                            {"abc", "abcValue"}
                        },
                        MyLevel = Level.High
                    });
                _context.SaveChanges();
            }

            _inMemoryMovies = new List<Movie>
            {
                new Movie
                {
                    ID = 1,
                    Title = "Conan",
                    ReleaseDate = new DateTimeOffset(new DateTime(2018, 3, 3)),
                    Genre = Genre.Comedy,
                    Price = 1.99m
                },
                new Movie
                {
                    ID = 2,
                    Title = "James",
                    ReleaseDate = new DateTimeOffset(new DateTime(2017, 3, 3)),
                    Genre = Genre.Adult,
                    Price = 91.99m
                }
            };
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Request.Path.Value.Contains("efcore") 
                ? Ok(_context.Movies) 
                : Ok(_inMemoryMovies);
        }

        [EnableQuery]
        public IActionResult Get2(int key)
        {
            Movie m;
            m = Request.Path.Value.Contains("efcore") 
                ? _context.Movies.FirstOrDefault(c => c.ID == key)
                : _inMemoryMovies.FirstOrDefault(c => c.ID == key);

            if (m == null)
            {
                return NotFound();
            }

            return Ok(m);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            var m = Request.Path.Value.Contains("efcore") 
                ? _context.Movies.Where(c => c.ID == key) 
                : _inMemoryMovies.Where(c => c.ID == key).AsQueryable();

            if (!m.Any())
            {
                return NotFound();
            }

            return Ok(new SingleResult<Movie>(m));
        }

        [HttpPost]
        public IActionResult Patch(int key, [FromBody]Delta<Movie> delta)
        {
            var existing = _context.Movies.Find(key);
            delta.Patch(existing);
            _context.SaveChanges();
            return Ok();
        }
    }
}
