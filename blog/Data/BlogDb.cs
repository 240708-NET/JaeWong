namespace blog.Data;

using blog.Models;
using Microsoft.EntityFrameworkCore;

public class BlogDb(string connectionString) : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }
}
