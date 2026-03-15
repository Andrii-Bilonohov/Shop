using Application.Abstractions.Logger;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Abstractions.Services.Payments;
using Application.Abstractions.Services.UnitOfWork;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Logger;
using Infrastructure.Middlewares;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.LiqPay;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<PaymentContext>(options =>
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PaymentDb");
            options.UseNpgsql(connectionString);
        });

        services.AddSingleton<IExceptionLogger, ExceptionLogger>();

        services.Configure<GatewayOptions>(
            configuration.GetSection(GatewayOptions.SectionName));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
            
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        
        services.AddScoped<ILiqPayClient, LiqPayClient>();
        
        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseMiddleware<ListenToOnlyGatewayApiMiddleware>();
        return app;
    }
}