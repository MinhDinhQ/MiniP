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
        // Skift URL'en til at matche serverens rute med postId som en del af URL'en
        string url = $"{_baseAPI}posts/{postId}/comments";  // Tilføj postId til URL'en

        // Opret User-objekt baseret på det username, der sendes med
        User user = new User { Username = username };

        // Send kommentarens data som JSON, herunder User og Content
        HttpResponseMessage msg = await _http.PostAsJsonAsync(url, new { Content = content, User = user });

        // Tjek responsen
        if (msg.IsSuccessStatusCode)
        {
            // Hvis responsen er succesfuld, returner den oprettede kommentar
            return await msg.Content.ReadFromJsonAsync<Comment>();
        }
        else
        {
            // Hvis noget gik galt, læs fejlmeddelelsen og kast en undtagelse
            string errorContent = await msg.Content.ReadAsStringAsync();
            throw new Exception($"Error creating comment: {errorContent}");
        }
    }
    catch (Exception ex)
    {
        // Log fejl og kast en applikationsfejl
        LogError("Error creating comment", ex);
        throw new ApplicationException($"Error creating comment: {ex.Message}", ex);
    }
}

// Get comments for a specific post
public async Task<List<Comment>> GetCommentsForPost(int postId)
{
    try
    {
        string url = $"{_baseAPI}posts/{postId}/comments";  // URL for at hente kommentarer

        HttpResponseMessage msg = await _http.GetAsync(url);

        if (msg.IsSuccessStatusCode)
        {
            return await msg.Content.ReadFromJsonAsync<List<Comment>>();  // Hent kommentarer som en liste
        }
        else
        {
            string errorContent = await msg.Content.ReadAsStringAsync();
            throw new Exception($"Error fetching comments: {errorContent}");
        }
    }
    catch (Exception ex)
    {
        LogError("Error fetching comments", ex);
        throw new ApplicationException($"Error fetching comments: {ex.Message}", ex);
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
        string url = $"{_baseAPI}posts/{id}/downvote";    // Adjusted URL if needed
       var response = await _http.PostAsJsonAsync(url, "");


        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Post>() ?? new Post();
        }
        else
        {
            throw new Exception($"Error downvoting post {id}: {response.ReasonPhrase}");
        }
    }
    catch (Exception ex)
    {
        LogError($"Error downvoting post {id}", ex);
        throw new ApplicationException($"Error downvoting post {id}: {ex.Message}", ex);
    }
}

public async Task<Comment> UpvoteComment(int id)
{
    try
    {
        string url = $"{_baseAPI}comments/{id}/upvote";  
        HttpResponseMessage response = await _http.PostAsJsonAsync(url, "");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Comment>();
        }
        else
        {
            throw new Exception($"Error upvoting comment {id}");
        }
    }
    catch (Exception ex)
    {
        LogError($"Error upvoting comment {id}", ex);
        throw new ApplicationException($"Error upvoting comment {id}: {ex.Message}", ex);
    }
}

public async Task<Comment> DownvoteComment(int id)
{
    try
    {
        string url = $"{_baseAPI}comments/{id}/downvote";  
        HttpResponseMessage response = await _http.PostAsJsonAsync(url, "");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Comment>();
        }
        else
        {
            throw new Exception($"Error downvoting comment {id}");
        }
    }
    catch (Exception ex)
    {
        LogError($"Error downvoting comment {id}", ex);
        throw new ApplicationException($"Error downvoting comment {id}: {ex.Message}", ex);
    }
}



}