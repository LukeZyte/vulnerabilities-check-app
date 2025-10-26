using Microsoft.Data.SqlClient;
using System.Data;
using ToDoApp.API.Repositories.Implementations;
using ToDoApp.API.Repositories.Interfaces;

namespace ToDoApp.API.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<ITasksRepository, TasksRepository>();

        return services;
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidDataException("Connection string is empty");

        services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));

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
