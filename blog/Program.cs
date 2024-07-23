using blog.Data;
using blog.Models;

IRepository repo = new Repository();

// reset
foreach (Blog b in repo.GetBlogs())
{
    repo.RemoveBlog(b);
}

// create a blog and add a post
var blog = new Blog { Name = "MyBlog" };
repo.AddBlog(blog);
repo.AddPost(new Post { BlogId = blog.Id, Title = "First Post", Content = "This is the first post!" });
repo.AddPost(new Post { BlogId = blog.Id, Title = "Second Post", Content = "This is the second post." });

// print all blogs and their posts
foreach (Blog b in repo.GetBlogs())
{
    Console.WriteLine(b.Name + "\n====\n");
    foreach (Post p in repo.GetPosts(b))
    {
        Console.WriteLine(p.Title + "\n----\n" + p.Content + "\n");
    }
}
