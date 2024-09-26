using System;
using banking.Data;
using banking.Interfaces;
using banking.Repository;
using banking.Services;
using Microsoft.EntityFrameworkCore;

namespace banking.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        // Add services to the container.
        services.AddCors(opt => {
            opt.AddPolicy("CorsPolicy", builder => builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin());
        });

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Database Context
        services.AddDbContext<DataContext>(options => {
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        // AutoMapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Add New Services and Repsitories here
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IParamsHistoryService, ParamsHistoryService>();

        return services;
    }
}
