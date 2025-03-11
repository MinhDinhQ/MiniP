using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
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
        _baseAPI = _configuration["base_api"] ?? "http://localhost:5283/api/"; 
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

            HttpResponseMessage response = await _http.GetAsync(url);

            // Tjek hvad der er i responsen
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);  // Her kan du tjekke hvad API'en sender tilbage

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response into Post[] if successful
                return JsonSerializer.Deserialize<Post[]>(responseContent);
            }

            // If not successful, return an empty array or handle the error
            Console.WriteLine($"Error fetching posts. Status code: {response.StatusCode}");
            return Array.Empty<Post>();
        }
        catch (Exception ex)
        {
            LogError("Error getting posts", ex);
            throw new ApplicationException($"Error getting posts: {ex.Message}", ex);
        }
    }

    // Get a specific post by ID
    public async Task<Post> GetPost(int id)
    {
        try
        {
            string url = $"{_baseAPI}posts/{id}"; // URL for a specific post
            return await _http.GetFromJsonAsync<Post>(url);
        }
        catch (Exception ex)
        {
            LogError($"Error getting post {id}", ex);
            throw new ApplicationException($"Error getting post {id}: {ex.Message}", ex);
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

            // Read the JSON string from the response
            string json = await msg.Content.ReadAsStringAsync();

            // Deserialize the response into a Comment object
            Comment? newComment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true  // Ignore case when matching JSON properties to C# properties
            });

            return newComment;
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
            string url = $"{_baseAPI}posts/{id}/upvote";  // URL for upvoting a post

            // Send a POST request to upvote the post
            HttpResponseMessage msg = await _http.PostAsJsonAsync(url, "");

            // Read the JSON string from the response
            string json = await msg.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a Post object
            Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
            });

            return updatedPost;
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
            string url = $"{_baseAPI}posts/{id}/downvote";  // URL for downvoting a post

            // Send a POST request to downvote the post
            HttpResponseMessage msg = await _http.PostAsJsonAsync(url, "");

            // Read the JSON string from the response
            string json = await msg.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a Post object
            Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
            });

            return updatedPost;
        }
        catch (Exception ex)
        {
            LogError($"Error downvoting post {id}", ex);
            throw new ApplicationException($"Error downvoting post {id}: {ex.Message}", ex);
        }
    }

    // Upvote a comment
    public async Task<Comment> UpvoteComment(int id)
    {
        try
        {
            string url = $"{_baseAPI}comments/{id}/upvote";  // URL for upvoting a comment

            // Send a POST request to upvote the comment
            HttpResponseMessage msg = await _http.PostAsJsonAsync(url, "");

            // Read the JSON string from the response
            string json = await msg.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a Comment object
            Comment? updatedComment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return updatedComment;
        }
        catch (Exception ex)
        {
            LogError($"Error upvoting comment {id}", ex);
            throw new ApplicationException($"Error upvoting comment {id}: {ex.Message}", ex);
        }
    }

    // Downvote a comment
    public async Task<Comment> DownvoteComment(int id)
    {
        try
        {
            string url = $"{_baseAPI}comments/{id}/downvote";  // URL for downvoting a comment

            // Send a POST request to downvote the comment
            HttpResponseMessage msg = await _http.PostAsJsonAsync(url, "");

            // Read the JSON string from the response
            string json = await msg.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a Comment object
            Comment? updatedComment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return updatedComment;
        }
        catch (Exception ex)
        {
            LogError($"Error downvoting comment {id}", ex);
            throw new ApplicationException($"Error downvoting comment {id}: {ex.Message}", ex);
        }
    }
}
