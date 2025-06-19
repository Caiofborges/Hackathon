using System.Reflection;
using HackathonAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HackathonAPI.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        
        public DbSet<Colaborador> Colaboradores { get; set; }
    }
}