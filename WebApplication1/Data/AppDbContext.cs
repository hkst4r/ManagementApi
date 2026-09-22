using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{//AppDbContext; c#'s gateway to our database
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Machine> Machines { get; set; }//i want to store machine objects in the database
                                                    
    }
}