using System.Text.Json.Serialization;

namespace blog.Models;

public class Post
{
    public int Id { get; set; }
    public int BlogId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    [JsonIgnore]
    public Blog Blog { get; set; } = null!;
}
