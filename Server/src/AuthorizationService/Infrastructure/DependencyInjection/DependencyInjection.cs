using Application.Abstractions.Logger;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Logger;
using Infrastructure.Middlewars;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Abstractions.Services.Authorization;
using Application.Abstractions.Services.JWT;
using Application.Abstractions.Services.UnitOfWork;
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
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                options.UseSqlServer(connectionString);
            });

            services.Configure<JwtSettings>(
                configuration.GetSection("Jwt")
            );
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var sp = services.BuildServiceProvider();
                    var jwtSettings = sp.GetRequiredService<IOptions<JwtSettings>>().Value;
                    
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

            services.Configure<GatewayOptions>(
                configuration.GetSection(GatewayOptions.SectionName));

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
