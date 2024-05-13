using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskWebApi.Repositories;
using Serilog;
using TaskWebApi.CoreHelper;
using TaskWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskWebApi.Repositories.EF;

namespace TaskWebApi
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(WebApplicationBuilder builder, IWebHostEnvironment env)
        {
            _configuration = builder.Configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskWebApi", Version = "v1" });
            });

            services.Configure<RouteOptions>(options =>
            {
                options.AppendTrailingSlash = false;
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = false;
            });
            services.AddDbContext<TaskDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("TaskWebApiDB"));
            });
            //var connectionString = _configuration.GetConnectionString("TaskWebApiDB");
            AddDI(services);
            //services.AddSingleton<ApiKeyAuthorizationFilter>();
            //services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddAuthentication("Bearer").AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeyAuthentication"))
                };
            });
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var isUserSwagger = _configuration.GetValue<bool>("UseSwagger", false);
            if (isUserSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DefaultModelsExpandDepth(-1);
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskWebApi v1");
                });
            }

            //app.UseMiddleware<ApiKeyAuthenExtension>();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();
            app.MapControllers();
            app.UseRouting();
            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void AddDI(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
        }
    }
}
