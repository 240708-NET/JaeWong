namespace blog.Data;

using blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class DataContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"""Data Source={Path.Join(Environment.CurrentDirectory, "blog.db")}""");
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }
}
