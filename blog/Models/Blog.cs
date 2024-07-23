namespace blog.Models;

public class Blog
{
    public int Id { get; set; }
    public required string Name { get; set; }
    //
    public ICollection<Post> Posts { get; } = [];
}
