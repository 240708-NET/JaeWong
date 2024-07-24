namespace blog.Data;

using blog.Models;

public class Repository : IRepository
{
    public IEnumerable<Blog> GetBlogs()
    {
        using var context = new DataContext();
        return [.. context.Blogs];
    }
    public void AddBlog(Blog blog)
    {
        using var context = new DataContext();
        context.Blogs.Add(blog);
        context.SaveChanges();
    }
    public void UpdateBlog(Blog blog)
    {
        using var context = new DataContext();
        context.Update(blog);
        context.SaveChanges();
    }
    public void RemoveBlog(Blog blog)
    {
        using var context = new DataContext();
        context.Remove(blog);
        context.SaveChanges();
    }
    public IEnumerable<Post> GetPosts(Blog blog)
    {
        using var context = new DataContext();
        return [.. context.Posts.Where(p => p.Blog == blog)];
    }
    public void AddPost(Post post)
    {
        using var context = new DataContext();
        context.Add(post);
        context.SaveChanges();
    }
    public void UpdatePost(Post post)
    {
        using var context = new DataContext();
        context.Update(post);
        context.SaveChanges();
    }
    public void RemovePosts(Post post)
    {
        using var context = new DataContext();
        context.Remove(post);
        context.SaveChanges();
    }
}
