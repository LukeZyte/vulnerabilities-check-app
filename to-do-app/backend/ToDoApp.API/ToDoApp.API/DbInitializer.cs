using Dapper;
using Microsoft.Data.SqlClient;

public static class DatabaseInitializer
{
    public static void Initialize(string connectionString)
    {
        try
        {
            InitializeDatabase(connectionString);
            InitializeTables(connectionString);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private static void InitializeDatabase(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var databaseName = builder.InitialCatalog;

        builder.InitialCatalog = "master";
        var masterConnectionString = builder.ConnectionString;

        using var connection = new SqlConnection(masterConnectionString);
        connection.Open();

        var exists = connection.QueryFirstOrDefault<int>(
            "SELECT COUNT(*) FROM sys.databases WHERE name = @name",
            new { name = databaseName }
        ) > 0;

        if (!exists)
        {
            connection.Execute($"CREATE DATABASE [{databaseName}]");
        }
    }

    private static void InitializeTables(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        connection.Execute(@"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
            BEGIN
                CREATE TABLE Users (
                    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                    Username NVARCHAR(32) NOT NULL,
                    Password NVARCHAR(128) NOT NULL,
                    CreatedAt DATETIME2 DEFAULT GETDATE() NOT NULL
                );
            END
        ");

        connection.Execute(@"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tasks')
            BEGIN
                CREATE TABLE Tasks (
                    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                    Title NVARCHAR(200) NOT NULL,
                    Description NVARCHAR(MAX),
                    IsCompleted BIT DEFAULT 0 NOT NULL,
                    CreatedAt DATETIME2 DEFAULT GETDATE() NOT NULL
                );
            END
        ");
    }
}
