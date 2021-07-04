using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BookDBAPI.Models;
using Book_EF;
using System.Reflection;
using System.IO;
using Book_API.Services.Interfaces;
using Book_API.Services;
using Microsoft.AspNetCore.Http;
using Book_EF.EntityModels;

namespace BookDBAPI
{
  public class Startup
  {
    private const string CONNECTION_STRING_KEY = "DefaultConnection";
    private const string MY_ALLOW_SPECIFIC_ORIGINS = "EnableCORS";

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      AppSettings.Init(configuration.GetConnectionString(CONNECTION_STRING_KEY));
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      //services.AddMvc();
      services.AddControllers();

      services.AddCors(options =>
      {
          options.AddPolicy(MY_ALLOW_SPECIFIC_ORIGINS, builder =>
          {
              builder
                  .WithOrigins("http://localhost:4200", "https://localhost:4200")
                  .WithHeaders("content-type")
                  .AllowAnyMethod()
                  .AllowCredentials();
          });
      });

      services.AddSwaggerGen(c =>
      {
        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
      });

      services.AddDbContext<BookContext>(options => options.UseMySQL(Configuration.GetConnectionString(CONNECTION_STRING_KEY)));
      services.AddTransient<IBookService, BookService>();
      services.AddTransient<IAuthorService, AuthorService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("../swagger/v1/swagger.json", "Book API V1");
      });

      app.UseCors(MY_ALLOW_SPECIFIC_ORIGINS);

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
