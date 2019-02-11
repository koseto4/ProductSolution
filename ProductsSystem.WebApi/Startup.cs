using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProductsSystem.Data.Core;
using ProductsSystem.Services;
using ProductsSystem.Services.Services.Interfaces;
using ProductsSystem.WebApi.Helpers;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProductsSystem.Services.Services;

namespace ProductsSystem.WebApi
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
            var connection = @"Server=USER-PC\SQLEXPRESS;Database=ProductSolutionDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";
            services.AddDbContext<ProductsSystemDbContext>(options =>
               options.UseSqlServer(connection));

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.Events = new JwtBearerEvents
               {
                   OnTokenValidated = context =>
                   {
                       var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                       var userId = int.Parse(context.Principal.Identity.Name);
                       var user = userService.GetById(userId);
                       if (user == null)
                       {
                           // return unauthorized if user no longer exists
                           context.Fail("Unauthorized");
                       }
                       return Task.CompletedTask;
                   }
               };
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Products System Api",
                    Description = "Products Solution Web API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Kostadin Ivanov",
                        Email = string.Empty,
                        Url = "https://www.facebook.com/kostadin.ivanov.79069"
                    }
                });
                c.AddSecurityDefinition("Bearer", new OAuth2Scheme
                {
                    Flow = "password",
                    TokenUrl = "/authenticate"
                });

                // It must be here so the Swagger UI works correctly (Swashbuckle.AspNetCore.SwaggerUI, Version=4.0.1.0)
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
       {
           {"Bearer", new string[] { }}
       });
            });

            services.AddMvc();
            services.AddAutoMapper();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ISubCategoryService, SubCategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products V1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
