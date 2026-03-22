using Application.Abstractions.Logger;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Abstractions.Services.UnitOfWork;
using Application.Mappers;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Logger;
using Infrastructure.Middlewares;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<OrderContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseNpgsql(connectionString);
            });

            services.AddSingleton<IExceptionLogger, ExceptionLogger>();

            services.Configure<GatewayOptions>(
                configuration.GetSection(GatewayOptions.SectionName));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddAutoMapper(cfg => { }, typeof(OrderMapper).Assembly);

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseMiddleware<ListenToOnlyGatewayApiMiddleware>();
            return app;
        }
    }
}
