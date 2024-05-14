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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskWebApi.Model;
using Microsoft.AspNetCore.Identity;
using TaskWebApi.Repositories.Entities;

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
            services.AddIdentity<UserEntity, IdentityRole>().AddEntityFrameworkStores<TaskDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
                };
            });
            AddDI(services);
            //services.AddSingleton<ApiKeyAuthorizationFilter>();
            //services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.Configure<AppSetting>(_configuration.GetSection("AppSettings"));

            var secretKey = _configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(opt =>
            //    {
            //        opt.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            //tự cấp token
            //            ValidateIssuer = false,
            //            ValidateAudience = false,

            //            //ký vào token
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

            //            ClockSkew = TimeSpan.Zero
            //        };
            //    });

            //services.AddAuthentication("Bearer").AddJwtBearer(o =>
            //{
            //    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //    {
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        ValidateLifetime = false,
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeyAuthentication"))
            //    };
            //});
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
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();
            app.MapControllers();
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
