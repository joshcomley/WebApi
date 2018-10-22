// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;

namespace AspNetCoreODataSample.Web.Models
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasOne(p => p.FavoriteMovie).WithMany(m => m.People);
            modelBuilder.Entity<Movie>().Property(m => m.Price).HasColumnType("NUMERIC");
            base.OnModelCreating(modelBuilder);
        }
    }
}
