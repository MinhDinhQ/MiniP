using System.Net.Http.Json;
using System.Text.Json;
using shared.Model;

namespace kreddit_app.Data;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _configuration;
    private readonly string _baseAPI;

  
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

   
    public async Task<Post> CreatePost(Post newPost)
{
    try
    {
        string url = $"{_baseAPI}posts";  

        
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


 
public async Task<Comment> CreateComment(string content, int postId, string username)
{
    try
    {
      
        string url = $"{_baseAPI}posts/{postId}/comments";  

       
        User user = new User { Username = username };

        HttpResponseMessage msg = await _http.PostAsJsonAsync(url, new { Content = content, User = user });

   
        if (msg.IsSuccessStatusCode)
        {
           
            return await msg.Content.ReadFromJsonAsync<Comment>();
        }
        else
        {
            
            string errorContent = await msg.Content.ReadAsStringAsync();
            throw new Exception($"Error creating comment: {errorContent}");
        }
    }
    catch (Exception ex)
    {
       
        LogError("Error creating comment", ex);
        throw new ApplicationException($"Error creating comment: {ex.Message}", ex);
    }
}

public async Task<List<Comment>> GetCommentsForPost(int postId)
{
    try
    {
        string url = $"{_baseAPI}posts/{postId}/comments"; 

        HttpResponseMessage msg = await _http.GetAsync(url);

        if (msg.IsSuccessStatusCode)
        {
            return await msg.Content.ReadFromJsonAsync<List<Comment>>(); 
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

 
   public async Task<Post> DownvotePost(int id)
{
    try
    {
        string url = $"{_baseAPI}posts/{id}/downvote";    
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