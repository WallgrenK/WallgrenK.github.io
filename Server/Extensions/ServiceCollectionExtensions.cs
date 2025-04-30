using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Server.Models;
using Server.Models.Interface;
using Server.Models.SeatingModels.Validation;
using Server.Models.Services;
using Server.Models.UserModels;
using Server.Models.UserModels.Validation;
using Server.Security.Authorization.Handlers;
using Server.Security.Jwt;
using System.Text;
using System.Text.Json.Serialization;

namespace Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddDefaultIdentity<User>(
                options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

            services.AddScoped<IAuthorizationHandler, UserIsOwnerAuthorizationHandler>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<JwtHelperService>();

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
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = issuer,
                            ValidAudience = audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                            ClockSkew = TimeSpan.FromMinutes(20),
                        };
                    }
                });
            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<RegisterDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<BookSeatRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<CancelSeatRequestValidator>();

            return services;
        }
    }
}
