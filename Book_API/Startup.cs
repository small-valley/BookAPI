using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

using Amazon.CognitoIdentityProvider;

using Book_API.Middlewares;
using Book_API.Services;
using Book_API.Services.Interfaces;

using Book_EF;
using Book_EF.EntityModels;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace BookDBAPI
{
  public class Startup
  {
    private const string CONNECTION_STRING_KEY = "DefaultConnection";
    private const string MY_ALLOW_SPECIFIC_ORIGINS = "EnableCORS";
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
      AppSettings.Init(_configuration.GetConnectionString(CONNECTION_STRING_KEY));
    }


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      //services.AddMvc();
      services.AddControllers();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
          {
            options.TokenValidationParameters = new TokenValidationParameters
            {
              IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
              {
                // get JsonWebKeySet from AWS Cognito
                var json = new HttpClient().GetStringAsync($"https://cognito-idp.{_configuration["AWS:Region"]}.amazonaws.com/{_configuration["AWS:UserPoolId"]}/.well-known/jwks.json").GetAwaiter().GetResult();
                // serialize the result and return the keys
                return JsonSerializer.Deserialize<JsonWebKeySet>(json).Keys;
              },
              ValidateIssuerSigningKey = true,
              ValidateIssuer = true,
              ValidIssuer = $"https://cognito-idp.{_configuration["AWS:Region"]}.amazonaws.com/{_configuration["AWS:UserPoolId"]}",
              ValidateAudience = false,
              ValidateLifetime = true,
            };

            options.Events = new JwtBearerEvents
            {
              OnAuthenticationFailed = context =>
              {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                  context.Response.Headers.Append("Token-Expired", "true");
                  context.Response.Redirect($"{_configuration["Frontend:AuthFailRedirectUri"]}");
                }
                return Task.CompletedTask;
              }
            };
          });

      services.AddDefaultAWSOptions(_configuration.GetAWSOptions());
      services.AddAWSService<IAmazonCognitoIdentityProvider>();

      services.AddCors(options =>
      {
        options.AddPolicy(MY_ALLOW_SPECIFIC_ORIGINS, builder =>
        {
          builder
                .WithOrigins("http://localhost:4200", "https://localhost:4200")
                .AllowAnyHeader()
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

      services.AddDbContext<BookContext>(options => options.UseMySQL(_configuration.GetConnectionString(CONNECTION_STRING_KEY)));
      services.AddTransient<IAuthService, AuthService>();
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
      app.UseMiddleware<TokenMiddleware>();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
