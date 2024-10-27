using SDD.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SDD.Services.OrderAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
     
    }
}
