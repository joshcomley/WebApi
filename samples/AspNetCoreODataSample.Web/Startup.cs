// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetCoreODataSample.Web.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Interfaces;

namespace AspNetCoreODataSample.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MovieContext>(
                opt =>
                    opt.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=AspNetCoreODataSample.Web;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true", sql => sql.UseNetTopologySuite()));
            OData = services.AddOData();
            services.AddMvc();
        }

        public IODataBuilder OData { get; set; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var model = EdmModelBuilder.GetEdmModel();

            app.UseMvc(builder =>
            {
                builder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();

                builder.MapODataServiceRoute("odata1", "efcore", model);

                builder.MapODataServiceRoute("odata2", "inmem", model);

                builder.MapODataServiceRoute("odata3", "composite", EdmModelBuilder.GetCompositeModel());
            });

            OData.UseODataNetTopologySuite(model);

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MovieContext>();
                //await context.Database.EnsureCreatedAsync(cancellationToken);
                //context.Database.EnsureCreated();
                context.Database.Migrate();
            }
        }
    }
}
