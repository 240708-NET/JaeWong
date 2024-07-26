namespace blog.Data;

using System.Data;
using blog.Models;

public class BlogRepo(string connectionString) : IBlogRepo
{
    public IEnumerable<Blog> GetBlogs()
    {
        using var context = new BlogDb(connectionString);
        return [.. context.Blogs];
    }
    public void AddBlog(Blog blog)
    {
        using var context = new BlogDb(connectionString);
        context.Blogs.Add(blog);
        context.SaveChanges();
    }
    public void UpdateBlog(Blog blog)
    {
        using var context = new BlogDb(connectionString);
        context.Update(blog);
        context.SaveChanges();
    }
    public void RemoveBlog(Blog blog)
    {
        using var context = new BlogDb(connectionString);
        context.Remove(blog);
        context.SaveChanges();
    }
    public IEnumerable<Post> GetPosts(Blog blog)
    {
        using var context = new BlogDb(connectionString);
        return [.. context.Posts.Where(p => p.Blog == blog)];
    }
    public void AddPost(Post post)
    {
        using var context = new BlogDb(connectionString);
        context.Add(post);
        context.SaveChanges();
    }
    public void UpdatePost(Post post)
    {
        using var context = new BlogDb(connectionString);
        context.Update(post);
        context.SaveChanges();
    }
    public void RemovePost(Post post)
    {
        using var context = new BlogDb(connectionString);
        context.Remove(post);
        context.SaveChanges();
    }
}
