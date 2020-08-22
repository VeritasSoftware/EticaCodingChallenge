using Etica.Business;
using Etica.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Globalization;

namespace Etica.Api
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
            services.AddSingleton<IMappingService, MappingService>();
            services.AddScoped<ICarParkManager, CarParkManager>();
            services.AddScoped<CarParkContext>();
            services.AddScoped<ICarParkRepository, CarParkRepository>();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                var dateConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter
                {
                    DateTimeFormat = "dd'/'MM'/'yyyy'T'HH':'mm':'ss"
                };

                options.SerializerSettings.Converters.Add(dateConverter);
                options.SerializerSettings.Culture = new CultureInfo("en-AU");
                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            });

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Park API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //Exception handler for Prod
                app.UseExceptionHandler(a => a.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature.Error;

                    var result = JsonConvert.SerializeObject(new { error = exception.Message });
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result);
                }));
            }            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Seed sqlite database
            app.Seed().ConfigureAwait(true);
        }
    }
}
