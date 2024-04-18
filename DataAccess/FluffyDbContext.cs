using DataAccess.Entities;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

public class FluffyDbContext : DbContext
{
	public DbSet<Users> Users { get; set; }
	public DbSet<Tarrifs> Tarrifs { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.SetBasePath(Directory.GetCurrentDirectory())
			.Build();

		optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection"),
			new MySqlServerVersion(new Version(8, 0, 30)));
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Users>().HasIndex(u => u.Key).IsUnique();
	}
}