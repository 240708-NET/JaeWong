using blog.Data;
using blog.Models;
using Microsoft.VisualBasic;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var repo = new BlogRepo();
var api = app.MapGroup("/api");
api.MapGet("/blog", () =>
{
    return repo.GetBlogs();
});
api.MapPost("/blog", (Blog blog) =>
{
    repo.AddBlog(blog);
    return Results.Created("", blog);
});
api.MapPut("/blog/{id}", (int id, Blog blog) =>
{
    blog.Id = id;
    repo.UpdateBlog(blog);
    return Results.NoContent();
});
api.MapDelete("/blog/{id}", (int id) =>
{
    var blog = new Blog { Id = id, Name = "" };
    repo.RemoveBlog(blog);
    return Results.NoContent();
});
api.MapGet("/blog/{id}/posts", (int id) =>
{
    var blog = new Blog { Id = id, Name = "" };
    return repo.GetPosts(blog);
});
api.MapPost("/post", (Post post) =>
{
    repo.AddPost(post);
    return Results.Created("", post);
});
api.MapPut("/post/{id}", (int id, Post post) =>
{
    post.Id = id;
    repo.UpdatePost(post);
    return Results.NoContent();
});
api.MapDelete("/post/{id}", (int id) =>
{
    var post = new Post { Id = id, Title = "", Content = "" };
    repo.RemovePost(post);
    return Results.NoContent();
});

app.Run();
