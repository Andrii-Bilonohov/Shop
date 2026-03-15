using Application.Abstractions.Logger;
using Application.Abstractions.Repositories;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Logger;
using Infrastructure.Middlewars;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Abstractions.Services.Authorization;
using Application.Abstractions.Services.JWT;
using Application.Abstractions.Services.UnitOfWork;
using Application.Options;
using Infrastructure.Services.Authorization;
using Infrastructure.Services.UnitOfWork;

namespace Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserContext>(options =>
            {
                var connectionString =
                    Environment.GetEnvironmentVariable("ConnectionStrings__UserDb")
                    ?? configuration.GetConnectionString("UserDb");
                options.UseNpgsql(connectionString);
            });
            
            var jwtSettings = configuration.GetSection("JWT").Get<JwtSettings>()
                              ?? throw new ArgumentNullException("JWT settings not set");;

            services.AddSingleton(jwtSettings);
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });
            
            services.AddSingleton<IExceptionLogger, ExceptionLogger>();
            
            services.Configure<GatewayOptions>(configuration.GetSection(GatewayOptions.SectionName));
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseMiddleware<ListenToOnlyGatewayApiMiddleware>();
            return app;
        }
    }
}