using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;
using Presentation.Middlewares;
using Presentation.Options;

var builder = WebApplication.CreateBuilder(args);

Env.Load("C:/Asp.net/Shop/Server/.env");
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddOptions<GatewayOptions>()
    .Bind(builder.Configuration.GetSection(GatewayOptions.SectionName))
    .Validate(x => !string.IsNullOrWhiteSpace(x.Secret), "Gateway:Secret is required")
    .ValidateOnStart();

var jwtSecret = Environment.GetEnvironmentVariable("JWT__SECRET") 
                ?? throw new ArgumentNullException("JWT__SECRET not set");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT__ISSUER") ?? "Authorization";
var jwtAudience = Environment.GetEnvironmentVariable("JWT__AUDIENCE") ?? "ApiGateway";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),

            ClockSkew = TimeSpan.FromSeconds(30),

            NameClaimType = "sub",
            RoleClaimType = "role",
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Public", p => p.RequireAssertion(_ => true));
    options.AddPolicy("Authenticated", p => p.RequireAuthenticatedUser());
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseAddUserHeaders();
app.MapReverseProxy();
app.Run();