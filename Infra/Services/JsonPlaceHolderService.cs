using System.Text;
using System.Text.Json;
using System.Reflection;
using IntegrationsJsonPlaceHolder.Application.Interfaces;
using IntegrationsJsonPlaceHolder.Domain.Entities;
using IntegrationsJsonPlaceHolder.Helpers;
using IntegrationsJsonPlaceHolder.Infra.Config;
using IntegrationsJsonPlaceHolder.Shared.Constants;

namespace IntegrationsJsonPlaceHolder.Infra.Services
{
    public class JsonPlaceHolderService : IJsonPlaceHolderService
    {
        private readonly HttpClient _httpClient;
        private readonly int _keyUrl;

        public JsonPlaceHolderService(HttpClient httpClient, int keyURL)
        {
            _httpClient = httpClient;
            _keyUrl = keyURL;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = Array.Find(
                assembly.GetManifestResourceNames(),
                name => name.EndsWith("appsettings.json", StringComparison.OrdinalIgnoreCase)
            );

            if (resourceName == null)
                throw new InvalidOperationException("Embedded resource 'appsettings.json' not found.");

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream!);
            var json = reader.ReadToEnd();

            var config = JsonSerializer.Deserialize<AppSettings>(json);
            if (config == null)
                throw new InvalidOperationException("Could not deserialize appsettings.json.");

            var urlOne = config.JsonPlaceHolderURL?.UrlOne;
            var urlTwo = config.JsonPlaceHolderURL?.UrlTwo;

            _httpClient.BaseAddress = keyURL switch
            {
                KeyConstants.ONE => new Uri(urlOne ?? throw new Exception("UrlOne not found")),
                KeyConstants.TWO => new Uri(urlTwo ?? throw new Exception("UrlTwo not found")),
                _ => throw new ArgumentException("Invalid KeyURL: 0 or 1 was expected.")
            };
        }

        // ========== POSTS ==========
        public async Task<IEnumerable<Post>?> GetPostsAsync()
        {
            var response = await _httpClient.GetAsync("/posts");
            response.EnsureSuccessStatusCode();
            return await HttpClientHelper.DeserializeExternalApiResponse<IEnumerable<Post>>(response) ?? [];
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/posts/{id}");
            response.EnsureSuccessStatusCode();
            return await HttpClientHelper.DeserializeExternalApiResponse<Post>(response) ?? new Post();
        }

        public async Task<Post?> CreatePostAsync(Post post)
        {
            var json = JsonSerializer.Serialize(post);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/posts", content);
            response.EnsureSuccessStatusCode();
            return await HttpClientHelper.DeserializeExternalApiResponse<Post>(response) ?? new Post();
        }

        public async Task<Post?> UpdatePostAsync(int id, Post post)
        {
            var json = JsonSerializer.Serialize(post);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/posts/{id}", content);
            response.EnsureSuccessStatusCode();
            return await HttpClientHelper.DeserializeExternalApiResponse<Post>(response) ?? new Post();
        }

        public async Task DeletePostAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/posts/{id}");
            response.EnsureSuccessStatusCode();
        }

        // ========== COMMENTS ==========
        public async Task<IEnumerable<Comment>?> GetCommentsAsync()
        {
            var response = await _httpClient.GetAsync($"/comments");
            return await HttpClientHelper.DeserializeExternalApiResponse<IEnumerable<Comment>>(response) ?? [];
        }
        public async Task<Comment?> GetCommentsByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/comments/{id}");
            return await HttpClientHelper.DeserializeExternalApiResponse<Comment>(response) ?? new Comment();
        }

        public async Task<IEnumerable<Comment>?> GetCommentsByPostIdAsync(int postId)
        {
            var response = await _httpClient.GetAsync($"/posts/{postId}/comments");
            response.EnsureSuccessStatusCode();
            return await HttpClientHelper.DeserializeExternalApiResponse<IEnumerable<Comment>>(response) ?? [];
        }

        public async Task<Comment?> CreateCommentAsync(Comment comment)
        {
            var json = JsonSerializer.Serialize(comment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/comments", content);
            response.EnsureSuccessStatusCode();
            return await HttpClientHelper.DeserializeExternalApiResponse<Comment>(response) ?? new Comment();
        }

        public async Task<Comment?> UpdateCommentAsync(int id, Comment comment)
        {
            var json = JsonSerializer.Serialize(comment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/comments/{id}", content);
            response.EnsureSuccessStatusCode();
            return await HttpClientHelper.DeserializeExternalApiResponse<Comment>(response) ?? new Comment();
        }

        public async Task DeleteCommentAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/comments/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
