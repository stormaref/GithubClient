using System;
using GithubClient.Models;
using Microsoft.EntityFrameworkCore;

namespace GithubClient.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Repository> Repositories { get; set; }
    }
}
