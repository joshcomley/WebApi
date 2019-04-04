// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;

namespace AspNetCoreODataSample.Web.Models
{
    public class MovieContext : DbContext
    {
        public MovieContext()
        {
            
        }

        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(
                @"Server=.;Database=AspNetCoreODataSample.Web;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true",
                sql => sql.UseNetTopologySuite());
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieStar> MovieStars { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieStar>().HasKey(_ => new
            {
                _.FirstName,
                _.LastName
            });
            modelBuilder.Entity<Movie>().Property(_ => _.Price).HasColumnType("decimal(18,2)");
            base.OnModelCreating(modelBuilder);
        }
    }
}
