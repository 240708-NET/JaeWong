namespace blog.Data;

using blog.Models;
using Microsoft.EntityFrameworkCore;

public class BlogDb : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public BlogDb(DbContextOptions<BlogDb> options) : base(options) { }
}
