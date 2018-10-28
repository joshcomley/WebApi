using System;
using AspNetCoreODataSample.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreODataSample.Web
{
    //public class DesignTimeAppDbContextBuilder : IDesignTimeDbContextFactory<MovieContext>
    //{
    //    private readonly IServiceProvider _serviceProvider;

    //    public DesignTimeAppDbContextBuilder(IServiceProvider serviceProvider)
    //    {
    //        _serviceProvider = serviceProvider;
    //    }
    //    public MovieContext CreateDbContext(string[] args)
    //    {
    //        return new MovieContext(_serviceProvider.GetService<DbContextOptions<MovieContext>>());
    //    }
    //}
}