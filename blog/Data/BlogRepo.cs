namespace blog.Data;

using System.Data;
using blog.Models;

public class BlogRepo(BlogDb context) : IBlogRepo
{
    public IEnumerable<Blog> GetBlogs()
    {
        return [.. context.Blogs];
    }
    public void AddBlog(Blog blog)
    {
        context.Blogs.Add(blog);
        context.SaveChanges();
    }
    public void UpdateBlog(Blog blog)
    {
        context.Update(blog);
        context.SaveChanges();
    }
    public void RemoveBlog(Blog blog)
    {
        context.Remove(blog);
        context.SaveChanges();
    }
    public IEnumerable<Post> GetPosts(Blog blog)
    {
        return [.. context.Posts.Where(p => p.Blog == blog)];
    }
    public void AddPost(Post post)
    {
        context.Add(post);
        context.SaveChanges();
    }
    public void UpdatePost(Post post)
    {
        context.Update(post);
        context.SaveChanges();
    }
    public void RemovePost(Post post)
    {
        context.Remove(post);
        context.SaveChanges();
    }
}
