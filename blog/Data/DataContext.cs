namespace blog.Data;

using blog.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

class DatabaseConfig
{
    public required string ConnectionString { get; set; }
}

public class DataContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        DatabaseConfig? databaseConfig = JsonSerializer.Deserialize<DatabaseConfig>(File.ReadAllText("config/database.json"));
        if (databaseConfig is not null)
        {
            optionsBuilder.UseSqlServer(databaseConfig.ConnectionString);
        }
        else
        {
            optionsBuilder.UseSqlite("Data Source=blog.db");
        }
    }
}
