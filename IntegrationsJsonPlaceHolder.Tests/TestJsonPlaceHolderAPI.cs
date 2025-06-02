using IntegrationsJsonPlaceHolder.Application.Interfaces;
using IntegrationsJsonPlaceHolder.Domain.Entities;
using IntegrationsJsonPlaceHolder.Infra.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationsJsonPlaceHolder.Tests
{
    public class TestJsonPlaceHolderAPI
    {
        private readonly IJsonPlaceHolderService _service;
        private readonly ITestOutputHelper _output;

        public TestJsonPlaceHolderAPI(ITestOutputHelper output)
        {
            _output = output;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Get KeyURL value
            var keyUrlString = configuration["JsonPlaceHolder:KeyURL"];
            if (!int.TryParse(keyUrlString, out int keyUrl))
                throw new Exception("'JsonPlaceHolder:KeyURL' value is not valid in appsettings.json");

            // Register service
            var httpClient = new HttpClient();
            _service = new JsonPlaceHolderService(httpClient, keyUrl);
        }

        [Fact]
        public async Task GetPosts_ReturnsData()
        {
            var posts = await _service.GetPostsAsync();

            Assert.NotNull(posts);
            Assert.NotEmpty(posts);

            // output with this command: dotnet test --logger "console;verbosity=detailed"
            _output.WriteLine("Posts:");
            _output.WriteLine(JsonSerializer.Serialize(posts, new JsonSerializerOptions { WriteIndented = true }));
        }

        [Fact]
        public async Task GetComments_ReturnsData()
        {
            var comments = await _service.GetCommentsAsync();

            Assert.NotNull(comments);
            Assert.NotEmpty(comments);

            // output with this command: dotnet test --logger "console;verbosity=detailed"
            _output.WriteLine("Comments:");
            _output.WriteLine(JsonSerializer.Serialize(comments, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
