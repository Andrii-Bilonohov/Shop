using Application.Abstractions.Logger;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services.Authorization;
using Application.Abstractions.Services.JWT;
using Application.Abstractions.Services.UnitOfWork;
using Application.Options;
using Infrastructure.Data;
using Infrastructure.Logger;
using Infrastructure.Middlewars;
using Infrastructure.Repositories;
using Infrastructure.Services.Authorization;
using Infrastructure.Services.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Services;

namespace Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseNpgsql(connectionString);
            });
            
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            var jwt = configuration.GetSection("Jwt").Get<JwtSettings>()
                      ?? throw new Exception("Jwt settings not configured");
            
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

                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt.Secret))
                };
            });
            
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddSingleton<IExceptionLogger, ExceptionLogger>();
            
            services.Configure<GatewayOptions>(
                configuration.GetSection(GatewayOptions.SectionName));

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseMiddleware<ListenToOnlyGatewayApiMiddleware>();
            return app;
        }
    }
}