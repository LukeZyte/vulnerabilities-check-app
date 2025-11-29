using Dapper;
using Microsoft.Data.SqlClient;
using ToDoApp.API.Models.Domain;

public static class DatabaseInitializer
{
    public static void Initialize(string connectionString)
    {
        try
        {
            InitializeDatabase(connectionString);
            InitializeTables(connectionString);
            SeedTables(connectionString); // USE IT ONLY ONE TIME ON FIRST RUN
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private static void SeedTables(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        var usersToInsert = new List<User>
        {
            new User { Id = Guid.NewGuid(), Username = "A", Password = "Bardzo" },
        };

        string sql = "INSERT INTO Users (Id, Username, Password) VALUES (@Id, @Username, @Password);";

        connection.Execute(sql, usersToInsert);
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
                    Text NVARCHAR(MAX) NOT NULL,
                    IsCompleted BIT DEFAULT 0 NOT NULL,
                    CreatedAt DATETIME2 DEFAULT GETDATE() NOT NULL,
                    OwnerId UNIQUEIDENTIFIER NOT NULL
                );
            END
        ");
    }
}
