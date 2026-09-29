using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{//AppDbContext; c#'s gateway to our database
    public class AppDbContext : DbContext//we are inheriting from dbcontext, default microsoft class containing querying, tracking objects, saving changes, talking to databases etc..
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)//we are extending it with info specific to our application, to createan appdbcontext, give me a configuration method meant for app db context
            : base(options)//Pass those options up to my parent class, DbContext, because DbContext is the class that actually knows what to do with them."
        {
        }


        //dbcontextoptions kull mhu configuration, provider: sql server, database

        public DbSet<Machine> Machines { get; set; }//kull ma ghidna, id database tieghi se zzomm objects of type machine. b'id, name u status.

        //igifiri nistaw niktbu: context.Machines.Add(machine); jew await context.Machines.FirstOrDefaultAsync(m => m.id == 5   etc....




    }
}