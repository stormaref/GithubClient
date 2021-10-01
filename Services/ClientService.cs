using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GithubClient.Data;
using GithubClient.Models;
using Newtonsoft.Json;

namespace GithubClient.Services
{
    public interface IClientService
    {
        Task<List<Repository>> GetUserRepositories(string username);
    }

    public class ClientService : IClientService
    {
        private readonly HttpClient _client;
        private readonly IDatabaseService _databaseService;

        public ClientService(HttpClient client, IDatabaseService databaseService)
        {
            _client = client;
            _client.DefaultRequestHeaders.Add("User-Agent", "IDK");
            _databaseService = databaseService;
        }

        public async Task<List<Repository>> GetUserRepositories(string username)
        {
            var url = $"https://api.github.com/users/{username}/repos";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;
            var content = await response.Content.ReadAsStringAsync();
            var repositories = JsonConvert.DeserializeObject<List<Repository>>(content);
            repositories.ForEach(async r => await AddRepoToDb(r));
            return repositories;
        }

        private async Task AddRepoToDb(Repository repo)
        {
            await _databaseService.AddOrUpdateRepository(repo);
        }
    }
}
