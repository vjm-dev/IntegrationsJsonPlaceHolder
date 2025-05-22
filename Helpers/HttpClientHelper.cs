using System.Text.Json;

namespace IntegrationsJsonPlaceHolder.Helpers
{
    public static class HttpClientHelper
    {
        public static async Task<T?> DeserializeExternalApiResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) return default;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content);
        }
    }
}

