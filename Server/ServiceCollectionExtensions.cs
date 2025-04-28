using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Server.Models.Interface;
using Server.Models.Services;
using Server.Models.UserModels.Validation;
using Server.Security.Jwt;
using System.Text;

namespace Server
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddSingleton<JwtHelperService>();

            return services;
        }

        public static IServiceCollection ConfigureJWT(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var secretKey = builder.Configuration["JwtSettings:SecretKey"];
                    var issuer = builder.Configuration["JwtSettings:Issuer"];
                    var audience = builder.Configuration["JwtSettings:Audience"];

                    if (secretKey != null)
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = issuer,
                            ValidateAudience = true,
                            ValidAudience = audience,
                            ValidateLifetime = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                            ClockSkew = TimeSpan.FromMinutes(5),
                        };
                    }
                });
            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<RegisterDTOValidator>();

            return services;
        }
    }
}
