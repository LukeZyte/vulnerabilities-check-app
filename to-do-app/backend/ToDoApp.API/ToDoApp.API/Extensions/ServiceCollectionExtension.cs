using System.Data;
using Microsoft.Data.SqlClient;
using ToDoApp.API.Repositories.Implementations;
using ToDoApp.API.Repositories.Interfaces;
using ToDoApp.API.Services;

namespace ToDoApp.API.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<ITasksRepository, TasksRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IJwtService, JwtService>();


        return services;
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidDataException("Connection string is empty");

        services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));

        services.AddCors(options =>
        {
            options.AddPolicy("SecureCors", policy =>
            {
                policy
                    .WithOrigins(
                    "http://localhost:5173",
                    "https://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        try
        {
            DatabaseInitializer.Initialize(connectionString);
        }
        catch (Exception ex)
        {
        }

        return services;
    }
}
