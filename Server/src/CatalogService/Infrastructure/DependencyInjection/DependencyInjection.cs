using Application.Abstractions.Logger;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Abstractions.Services.UnitOfWork;
using Application.Profiles;
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
            
            services.AddDbContext<ItemContext>(options =>
            {
                var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__CatalogDb");
                options.UseNpgsql(connectionString);
            });

            services.AddSingleton<IExceptionLogger, ExceptionLogger>();

            services
                .AddOptions<GatewayOptions>()
                .Bind(configuration.GetSection(GatewayOptions.SectionName))
                .Validate(x => !string.IsNullOrWhiteSpace(x.Secret), "Gateway:Secret is required")
                .ValidateOnStart();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddMemoryCache();
            
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            
            services.AddScoped<IItemRepository, ItemRepository>();

            services.AddAutoMapper(cfg => { }, typeof(ItemProfile).Assembly);
            
            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseMiddleware<ListenToOnlyGatewayApiMiddleware>();
            return app;
        }
    }
}
