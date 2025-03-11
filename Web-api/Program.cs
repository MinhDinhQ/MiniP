using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using shared.Model;
using API.Server;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<RedditDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); 


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:7228")  
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();


app.UseCors("AllowBlazorOrigin");

app.Use(async (context, next) =>
{
    context.Response.ContentType = "application/json; charset=utf-8";
    await next(context);
});


app.MapGet("/posts", async (RedditDbContext db) =>
{
    var posts = await db.Posts
        .Include(p => p.User)
        .Include(p => p.Comments)
        .ToListAsync();
    return Results.Ok(posts);
});

app.MapGet("/posts/{id}", async (int id, RedditDbContext db) =>
{
    var post = await db.Posts
        .Include(p => p.User)
        .Include(p => p.Comments) 
        .ThenInclude(c => c.User)
        .FirstOrDefaultAsync(p => p.Id == id);

    if (post == null)
        return Results.NotFound();

    return Results.Ok(post);
});


app.MapPost("/posts", async (Post post, RedditDbContext db) =>
{
    db.Posts.Add(post);
    await db.SaveChangesAsync();
    return Results.Created($"/posts/{post.Id}", post);
});

app.MapPost("/posts/{postId}/comments", async (Comment comment, int postId, RedditDbContext db) =>
{
    var post = await db.Posts.FindAsync(postId);
    if (post == null)
        return Results.NotFound();

    post.Comments.Add(comment);
    await db.SaveChangesAsync();
    return Results.Created($"/posts/{postId}/comments/{comment.Id}", comment);
});

app.MapGet("/posts/{postId}/comments", async (int postId, RedditDbContext db) =>
{
    var post = await db.Posts
        .Include(p => p.Comments)  
        .FirstOrDefaultAsync(p => p.Id == postId);

    if (post == null)
        return Results.NotFound();

    
    return Results.Ok(post.Comments);
});



app.MapPost("/posts/{id}/upvote", async (int id, RedditDbContext db) =>
{
    var post = await db.Posts.FindAsync(id);
    if (post == null)
        return Results.NotFound();

    post.Upvotes++;
    await db.SaveChangesAsync();
    return Results.Ok(post);
});

app.MapPost("/posts/{id}/downvote", async (int id, RedditDbContext db) =>
{
    var post = await db.Posts.FindAsync(id);
    if (post == null)
        return Results.NotFound();

    post.Downvotes++;
    await db.SaveChangesAsync();
    return Results.Ok(post);
});

app.MapPost("/comments/{id}/upvote", async (int id, RedditDbContext db) =>
{
    var comment = await db.Comments.FindAsync(id);
    if (comment == null)
        return Results.NotFound();

    comment.Upvotes++;
    await db.SaveChangesAsync();
    return Results.Ok(comment);
});

app.MapPost("/comments/{id}/downvote", async (int id, RedditDbContext db) =>
{
    var comment = await db.Comments.FindAsync(id);
    if (comment == null)
        return Results.NotFound();

    comment.Downvotes++;
    await db.SaveChangesAsync();
    return Results.Ok(comment);
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();