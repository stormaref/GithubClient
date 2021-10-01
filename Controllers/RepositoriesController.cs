using System;
using System.Threading.Tasks;
using GithubClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace GithubClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RepositoriesController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IDatabaseService _databaseService;

        public RepositoriesController(IClientService clientService, IDatabaseService databaseService)
        {
            _clientService = clientService;
            _databaseService = databaseService;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> All([FromRoute] string username)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest();
            var repos = await _clientService.GetUserRepositories(username);
            return Ok(repos);
        }

        [HttpGet]
        public async Task<IActionResult> AllFromDb()
        {
            var repos = await _databaseService.GetAllRepositories();
            return Ok(repos);
        }
    }
}
