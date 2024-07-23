using blog.Data;
using blog.Models;
using Microsoft.EntityFrameworkCore;

using (var context = new DataContext())
{
    context.Blogs.RemoveRange(context.Blogs);
    context.Posts.RemoveRange(context.Posts);
    context.SaveChanges();
}

using (var context = new DataContext())
{
    var blog = new Blog { Name = "MyBlog" };
    context.Add(blog);
    context.SaveChanges();
    blog.Posts.Add(new Post
    {
        Title = "Hello World Example",
        Content = """Console.WriteLine("Hello, World!");""",
        CreatedAt = DateTime.Now,
    });
    context.SaveChanges();
}

using (var context = new DataContext())
{
    var posts = from p in context.Posts where p.Blog.Name == "MyBlog" select p;
    foreach (var post in posts)
    {
        Console.WriteLine(post.Title + "\n----\n" + post.Content + "\n\n\n");
    }
}
