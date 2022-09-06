using System.Net;
using System.Text;
using System.Text.Json;
using BackendTestCommon;

namespace BackendCommon;

public static class HttpClientExtensions
{
    public static JsonSerializerOptions options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async static Task<HttpStatusCode> RegisterUserAsync(this HttpClient client, object user)
    {
        var json = JsonSerializer.Serialize(user, options);
        var body = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PutAsync(string.Format(PathConstants.RegisterUser), body);
        return response.StatusCode;
    }

    public async static Task<HttpStatusCode> LoginAsync(this HttpClient client, object user)
    {
        var json = JsonSerializer.Serialize(user, options);
        var body = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(string.Format(PathConstants.Login), body);
        return response.StatusCode;
    }

    public async static Task<bool> DeleteUserAsync(this HttpClient client, CreateUserModel user)
    {
        HttpResponseMessage response = await client.DeleteAsync(string.Format(PathConstants.GetUser, user.Username));
        return response.IsSuccessStatusCode;
    }

    public async static Task<bool> DeleteAllUsersAsync(this HttpClient client)
    {
        HttpResponseMessage response = await client.DeleteAsync(string.Format(PathConstants.DeleteAllUsers));
        return response.IsSuccessStatusCode;
    }

    public async static Task<HttpResponseMessage> GetUserAsync(this HttpClient client, CreateUserModel user)
    {
        HttpResponseMessage response = await client.GetAsync(string.Format(PathConstants.GetUser, user.Username));
        response.EnsureSuccessStatusCode();
        return response;
    }
}

