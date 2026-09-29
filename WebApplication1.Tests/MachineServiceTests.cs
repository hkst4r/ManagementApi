using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Tests
{
    public class MachineServiceTests
    {
        [Fact]


        public async Task RetrieveById_MachineExists_ReturnsMachine()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()//creates a config builder object called options meant for app dbcontext
                .UseInMemoryDatabase("RetrieveMachineTest")//we are NOT using sql, we are creating an memory database managed by EF Core's InMemory provider whose data is temporary
                .Options;//.options extracts the finished config object
                         //we did not create a dbcontext, we created the INSTURCTIONS/CONFIGURATION for one, "When an AppDbContext uses these options, use EF Core's InMemory provider,
                         //with a temporary MEMORY test database named RetrieveMachineTest as we are not testing our real database.

            //so now the config builder object "options" contains, provider: InMemory, database name: RetriveMachineTest

            AppDbContext context = new AppDbContext(options);
            //we are calling: public AppDbContext(DbContextOptions<AppDbContext> options)

            //breakdown: kreajna config options all app db context, we created an object appdbcontext context, qed jiehu l config li ghadna kemm ghamilna (var options....), imbaghad permezz ta base(options) go appdbcontex.cs
            //qedin natu dik il config li default dbcontext, (kif ukoll f appdbcontext ghinda li ha nkunu qedin nuzaw object of type Machine)


            /*          EF Core
               
              (Our test) db     (Normal db we have in sql)
                /                    \
               /                      \
      InMemory Provider             SQL Server Provider
             ↓                                ↓
       .NET memory                            SQL
                                                ↓
                                             SQL Server*/



            Machine machine = new Machine { id = 5, Name = "Test Machine", Status = "Running" };


            context.Add(machine);

            await context.SaveChangesAsync();//No SQL Server involved.



            MachineService service = new MachineService(context);//Create MachineService using AppDbContext




            //act

            Machine? result = await service.RetrieveByIdAsync(5);


            //assert


            Assert.NotNull(result);
            Assert.Equal(5, result.id);
            Assert.Equal("Test Machine", result.Name);
            Assert.Equal("Running", result.Status);


            

        }

    }
}