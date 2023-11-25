using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Vocula.Server.Settings;

namespace Vocula;

/// <summary>
/// Startup class
/// </summary>
public class Startup(
    IConfiguration configuration
    )
{

    /// <summary>
    /// Configuration
    /// </summary>
    /// <value></value>
    public IConfiguration Configuration { get; } = configuration;

    /// <summary>
    /// Configuratiopn of services
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        // configure strongly typed settings objects
        var voculaSettings = Configuration.GetSection("Vocula");
        services.Configure<VoculaSettings>(voculaSettings);

        services.AddCors(options =>
        {
            if (voculaSettings.Get<VoculaSettings>().Server.Cors.AllowOrigins.Count > 0)
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(voculaSettings.Get<VoculaSettings>().Server.Cors.AllowOrigins.ToArray())
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                );
            }
            else
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                );
            }
        });
        services.AddControllers()
            .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Vocula", Version = "v1" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            option.IncludeXmlComments(xmlPath);
        });
    }

    /// <summary>
    /// Configuration of the HTTP request pipeline and others
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
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

