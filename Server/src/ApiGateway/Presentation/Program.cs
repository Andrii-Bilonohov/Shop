using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Presentation.Middlewares;
using Presentation.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
                       
                       builder.Services.AddOptions<GatewayOptions>()
                           .Bind(builder.Configuration.GetSection(GatewayOptions.SectionName))
                           .Validate(x => !string.IsNullOrWhiteSpace(x.Secret), "Gateway:Secret is required")
                           .ValidateOnStart();

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

            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret)),

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