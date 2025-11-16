using Microsoft.Data.SqlClient;
using System.Data;
using VulnerableToDoApp.API.Repositories.Implementations;
using VulnerableToDoApp.API.Repositories.Interfaces;

namespace VulnerableToDoApp.API.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<ITasksRepository, TasksRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        return services;
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidDataException("Connection string is empty");

        services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
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
