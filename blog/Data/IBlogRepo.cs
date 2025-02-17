namespace blog.Data;

using blog.Models;

public interface IBlogRepo
{
    IEnumerable<Blog> GetBlogs();
    void AddBlog(Blog blog);
    void UpdateBlog(Blog blog);
    void RemoveBlog(Blog blog);
    IEnumerable<Post> GetPosts(Blog blog);
    void AddPost(Post post);
    void UpdatePost(Post post);
    void RemovePost(Post post);
}
