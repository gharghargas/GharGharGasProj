using Microsoft.EntityFrameworkCore;
using GharGharGas.API.Models;

// DB CONTEXT
namespace GharGharGas.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Otp> Otps { get; set; }
}