namespace blog.Models;

using System.Text.Json.Serialization;

public class Blog
{
    public int Id { get; set; }
    public required string Name { get; set; }
    [JsonIgnore]
    public ICollection<Post> Posts { get; } = [];
}
