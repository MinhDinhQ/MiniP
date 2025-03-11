using System.Net.Http.Json;
using System.Text.Json;
using shared.Model;

namespace kreddit_app.Data;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _configuration;
    private readonly string _baseAPI;

    // Constructor to initialize HttpClient and configuration
    public ApiService(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        _configuration = configuration;
        _baseAPI = _configuration["base_api"] ?? "http://localhost:5283/"; 
    }

    private void LogError(string message, Exception ex)
    {
        Console.WriteLine($"{message}: {ex.Message}");
    }

    // Get all posts
    public async Task<Post[]> GetPosts()
    {
        try
        {
            string url = $"{_baseAPI}posts/";
            var response = await _http.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Post[]>();
            }
            else
            {
                throw new Exception("Error fetching posts.");
            }
        }
        catch (Exception ex)
        {
            LogError("Error getting posts", ex);
            throw new ApplicationException($"Error getting posts: {ex.Message}", ex);
        }
    }

    // Get a specific post by ID
    public async Task<Post?> GetPost(int id)
    {
        try
        {
            string url = $"{_baseAPI}posts/{id}"; 
            return await _http.GetFromJsonAsync<Post>(url);
        }
        catch (Exception ex)
        {
            LogError($"Error getting post {id}", ex);
            throw new ApplicationException($"Error getting post {id}: {ex.Message}", ex);
        }
    }

    // Create a post
    public async Task<Post> CreatePost(Post newPost)
{
    try
    {
        string url = $"{_baseAPI}posts";  

        // Send data som JSON til API'et
        var response = await _http.PostAsJsonAsync(url, newPost);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Post>();
        }
        else
        {
            throw new Exception("Error creating post.");
        }
    }
    catch (Exception ex)
    {
        LogError("Error creating post", ex);
        throw new ApplicationException($"Error creating post: {ex.Message}", ex);
    }
}


    // Create a new comment for a post
    public async Task<Comment> CreateComment(string content, int postId, string username)
    {
        try
        {
            string url = $"{_baseAPI}comments";  // URL to create a new comment

            // Send the comment data as JSON
            HttpResponseMessage msg = await _http.PostAsJsonAsync(url, new { Content = content, PostId = postId, Username = username });

            // Check response
            if (msg.IsSuccessStatusCode)
            {
                return await msg.Content.ReadFromJsonAsync<Comment>();
            }
            else
            {
                throw new Exception("Error creating comment");
            }
        }
        catch (Exception ex)
        {
            LogError("Error creating comment", ex);
            throw new ApplicationException($"Error creating comment: {ex.Message}", ex);
        }
    }

    // Upvote a post
    public async Task<Post> UpvotePost(int id)
    {
        try
        {
            string url = $"{_baseAPI}posts/{id}/upvote";  
            HttpResponseMessage msg = await _http.PostAsJsonAsync(url, "");

            if (msg.IsSuccessStatusCode)
            {
                return await msg.Content.ReadFromJsonAsync<Post>();
            }
            else
            {
                throw new Exception($"Error upvoting post {id}");
            }
        }
        catch (Exception ex)
        {
            LogError($"Error upvoting post {id}", ex);
            throw new ApplicationException($"Error upvoting post {id}: {ex.Message}", ex);
        }
    }

    // Downvote a post
    public async Task<Post> DownvotePost(int id)
    {
        try
        {
            string url = $"{_baseAPI}posts/{id}/downvote";  
            var response = await _http.PostAsJsonAsync(url, "");
            return await response.Content.ReadFromJsonAsync<Post>();
        }
        catch (Exception ex)
        {
            LogError($"Error downvoting post {id}", ex);
            throw new ApplicationException($"Error downvoting post {id}: {ex.Message}", ex);
        }
    }


}
