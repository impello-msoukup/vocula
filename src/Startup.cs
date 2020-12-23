using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Vocula.Server.Settings;

namespace Vocula
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
            // configure strongly typed settings objects
            var voculaSettings = Configuration.GetSection("Vocula");
            services.Configure<VoculaSettings>(voculaSettings);

            services.AddCors(options => {
                if (voculaSettings.Get<VoculaSettings>().Server.Cors.AllowOrigins.Count > 0) {
                    options.AddDefaultPolicy(
                        builder => {
                            builder.WithOrigins(voculaSettings.Get<VoculaSettings>().Server.Cors.AllowOrigins.ToArray())
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        }
                    );
                } else {
                    options.AddDefaultPolicy(
                        builder => {
                            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        }
                    );
                }
            });
            services.AddControllers()
                .AddJsonOptions(options => 
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vocula", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vocula v1"));
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
