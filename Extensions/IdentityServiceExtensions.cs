using System;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace banking.Extensions;

public static class IdentityServiceExtensions
{
    public static  IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(opt =>
		{
			opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options => {
            var secretkey = config.GetSection("JwtSettings")["secretKey"] ?? throw new Exception("Tokenkey not found");
            options.TokenValidationParameters = new TokenValidationParameters {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretkey)),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });

        return services;
    }
}
