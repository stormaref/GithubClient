using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GithubClient.Data;
using GithubClient.Models;
using Microsoft.EntityFrameworkCore;

namespace GithubClient.Services
{
    public interface IDatabaseService
    {
        Task AddOrUpdateRepository(Repository repository);
        Task<List<Repository>> GetAllRepositories();
    }

    public class DatabaseService : IDatabaseService
    {
        private readonly DatabaseContext _context;

        public DatabaseService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddOrUpdateRepository(Repository repository)
        {
            var b = _context.Repositories.ToListAsync();
            var repo = await _context.Repositories.FindAsync(repository.Id);
            if (repo == null)
            {
                await AddRepository(repository);
                return;
            }
            repo = repository;
            await _context.SaveChangesAsync();
        }

        private async Task AddRepository(Repository repository)
        {
            _context.Repositories.Add(repository);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Repository>> GetAllRepositories() => await _context.Repositories.ToListAsync();
    }
}
