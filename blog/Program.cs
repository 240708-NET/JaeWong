using blog.Data;
using blog.Models;


if (args.Length < 1)
{
    Console.WriteLine("""
        Usage:
        Server    dotnet run server --launch-profile https
        Client    dotnet run client <server_address>:<port>
        """);
}
else if (args[0] == "server")
{
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

    var repo = new BlogRepo(builder.Configuration["ConnectionString"] ?? "");
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
}
else if (args[0] == "client")
{
    var client = new HttpClient();

    if (args.Length < 2)
    {
        Console.WriteLine("Usage: dotnet run client <server_address>");
        return;
    }
    var apiAddress = $"https://{args[1]}/api";

    Console.WriteLine("Enter 'help' to list all available commands.");
    while (true)
    {
        try
        {
            Console.Write("> ");
            var splitCommand = (Console.ReadLine() ?? "").Split();

            if (splitCommand[0] == "help")
            {
                Console.WriteLine("""
                Available Commands:
                blog ls        List all blogs
                blog create    Create a new blog
                blog rename    Rename an existing blog
                blog remove    Delete a blog
                post ls        List all posts of a blog
                post create    Add a new post to a blog
                post remove    Delete a post
                exit           Exit the client
                """);
                continue;
            }
            else if (splitCommand[0] == "blog" && splitCommand.Length >= 2)
            {
                if (splitCommand[1] == "ls")
                {
                    var blogs = client.GetFromJsonAsAsyncEnumerable<Blog>($"{apiAddress}/blog");
                    await foreach (var blog in blogs)
                    {
                        if (blog is not null)
                        {
                            Console.WriteLine($"[{blog.Id}] {blog.Name}");
                        }
                    }
                    continue;
                }
                else if (splitCommand[1] == "create")
                {
                    Console.Write("Name: ");
                    var name = Console.ReadLine() ?? "(Empty Name)";
                    var response = await client.PostAsJsonAsync(
                        $"{apiAddress}/blog",
                        new Blog { Name = name }
                    );
                    response.EnsureSuccessStatusCode();
                    continue;
                }
                else if (splitCommand[1] == "rename")
                {
                    if (splitCommand.Length < 3)
                    {
                        Console.WriteLine("Usage: blog rename <blog_id>");
                        continue;
                    }
                    var id = int.Parse(splitCommand[2]);
                    Console.Write("Enter the new name: ");
                    var name = Console.ReadLine() ?? "<Empty Name>";
                    var response = await client.PutAsJsonAsync(
                        $"{apiAddress}/blog/{id}",
                        new Blog { Name = name }
                    );
                    response.EnsureSuccessStatusCode();
                    continue;
                }
                else if (splitCommand[1] == "remove")
                {
                    if (splitCommand.Length < 3)
                    {
                        Console.WriteLine("Usage: blog remove <blog_id>");
                        continue;
                    }
                    var id = int.Parse(splitCommand[2]);
                    var response = await client.DeleteAsync($"{apiAddress}/blog/{id}");
                    response.EnsureSuccessStatusCode();
                    continue;
                }
            }
            else if (splitCommand[0] == "post" && splitCommand.Length >= 2)
            {
                if (splitCommand[1] == "ls")
                {
                    if (splitCommand.Length < 3)
                    {
                        Console.WriteLine("Usage: post ls <blog_id>");
                        continue;
                    }
                    var blogId = int.Parse(splitCommand[2]);
                    var posts = client.GetFromJsonAsAsyncEnumerable<Post>($"{apiAddress}/blog/{blogId}/posts");
                    await foreach (var post in posts)
                    {
                        if (post is not null)
                        {
                            Console.WriteLine($"""
                            [{post.Id}] {post.Title}
                            {post.Content}
                            """);
                        }
                    }
                    continue;
                }
                else if (splitCommand[1] == "create")
                {
                    if (splitCommand.Length < 3)
                    {
                        Console.WriteLine("Usage: post create <blog_id>");
                        continue;
                    }
                    var blogId = int.Parse(splitCommand[2]);
                    Console.Write("Title: ");
                    var title = Console.ReadLine() ?? "(Empty Title)";
                    Console.WriteLine("Content: (end with a single-line 'END')");
                    var content = "";
                    while (true)
                    {
                        var line = Console.ReadLine();
                        if (line == "END") break;
                        else content += line + '\n';
                    }
                    var response = await client.PostAsJsonAsync(
                        $"{apiAddress}/post",
                        new Post { BlogId = blogId, Title = title, Content = content }
                    );
                    response.EnsureSuccessStatusCode();
                    continue;
                }
                else if (splitCommand[1] == "remove")
                {
                    if (splitCommand.Length < 3)
                    {
                        Console.WriteLine("Usage: post remove <post_id>");
                        continue;
                    }
                    var id = int.Parse(splitCommand[2]);
                    var response = await client.DeleteAsync($"{apiAddress}/post/{id}");
                    response.EnsureSuccessStatusCode();
                    continue;
                }
            }
            else if (splitCommand[0] == "exit")
            {
                break;
            }
            Console.WriteLine("Unknown command.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

