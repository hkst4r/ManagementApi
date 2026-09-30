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
            //qedin natu dik il config li default dbcontext, (kif ukoll f appdbcontext ghidna li ha nkunu qedin nuzaw object of type Machine)


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



            MachineService service = new MachineService(context);//Create MachineService using AppDbContext, 




            //act

            Machine? result = await service.RetrieveByIdAsync(5);


            //assert


            Assert.NotNull(result);
            Assert.Equal(5, result.id);
            Assert.Equal("Test Machine", result.Name);
            Assert.Equal("Running", result.Status);


            

        }
        [Fact]

        public async Task RetrieveById_MachineDoesntExist_ReturnsNull()
        {//arrange

            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("RetrieveMachineDoesntExistTest").Options;


            AppDbContext context = new AppDbContext(options);

            Machine machine = new Machine { id = 5, Name = "Test Machine", Status = "Running" };

            context.Add(machine);

            await context.SaveChangesAsync();// tibda l async save, pauses THIS method until it finishes, frees the thread, tista taghmel xoghol iehor, ikollok hafna affarijiet taghmel differenza kbira

            MachineService service = new MachineService(context);
            //act
            Machine? result = await service.RetrieveByIdAsync(2);// async allows this method to use await and return its eventual result through a Task<Machine?>
            //assert
            Assert.Null(result);


            /*
             * async → this method can perform asynchronous work using await

                await →  start/wait for the Task; THIS METHOD cannot continue
                         to the next line until it finishes, but the THREAD can be
                         freed to handle other work while waiting*/

        }


        [Fact]

        public async Task CreateAsync_ValidMachine_AddsMachine()
        {//arrange
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("CreateMachineAddsMachineTest").Options;

            await using AppDbContext context = new AppDbContext(options);//await using makes sure the AppDbContext object is asynchronously disposed when we leave the scope even if an assertion fails

            MachineService service = new MachineService(context);
            //act
            Machine toCreate = new Machine { id=5, Name="Test", Status = "OK" };

            Machine result = await service.CreateAsync(toCreate);

            //assert

            Assert.Equal(5, result.id);
            Assert.Equal("Test", result.Name);
            Assert.Equal("OK", result.Status);

            var machineContext = await context.Machines.FirstOrDefaultAsync(mach=> mach.id == 5);

            Assert.NotNull(machineContext);
            Assert.Equal("Test", machineContext.Name);
            Assert.Equal("OK", machineContext.Status);
            Assert.Equal(5, machineContext.id);





        } //disposeasync is called as we left the scope
        //we await its completion
        //test is finished




        [Fact]

        public async Task UpdateAsync_ExistingMachine_UpdatesMachine()
        {
            //arrange
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("UpdateExistingMachineWorks").Options;

            await using (AppDbContext context = new AppDbContext(options))//a context is just an EF Core working session, a temporary EF Core object used to work with that database
                                                                          //Another context can connect to the same database later using the same options

            { 



                Machine machine = new Machine { id = 5, Name = "machineToBeUpdated", Status = "Running" };
                Machine updatedMachine = new Machine { id = 5, Name = "updatedMachine", Status = "Stopped" };
                context.Add(machine);
                await context.SaveChangesAsync();//method ma jkomplix qabel ma titlesta SaveChangesAsync(), it thread mhux qeda blukkata sakemm qed titlesta, tista taghmel xoghol iehor
                                                 //The method waits; the thread doesn't have to.

                MachineService service = new MachineService(context);


                /*

                      DbContextOptions
                     = HOW/WHERE should EF work?
                     "Use this provider/database"


                    AppDbContext
                    = CURRENT EF WORK SESSION
                      queries entities
                      tracks entities
                      saves changes


                    Database
                    = WHERE THE DATA LIVES

                 */

                //we are creating context1 and context2 to have seperate work sessions, the context1 tracking history cannot intervene with the new context2



                

                await service.UpdateAsync(5, updatedMachine);

            }

            await using (AppDbContext context2 = new AppDbContext(options))// Use a fresh context so previous EF tracking doesn't affect the database check.

            { 
            //assert


            Machine? updatedInDb = await context2.Machines.FirstOrDefaultAsync(s => s.id == 5);

                
            Assert.NotNull(updatedInDb);
            Assert.Single(context2.Machines);//one value in test db

            Assert.Equal(5, updatedInDb.id);//checking that the machine was updated successfully in the database
            Assert.Equal("updatedMachine", updatedInDb.Name);
            Assert.Equal("Stopped", updatedInDb.Status);

        }
        }


        [Fact]

        public async Task DeleteAsync_ExistingMachine_RemovesMachine()
        {
            //arrange

            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("DeleteMachineWorks").Options;
            await using AppDbContext context1 = new AppDbContext(options);

            MachineService service = new MachineService(context1);

            Machine machine = new Machine { id = 5, Name = "machineToBeDeleted", Status = "Stopped" };

            context1.Add(machine);
            await context1.SaveChangesAsync();

            //act

            Machine? deleted = await service.DeleteAsync(5);

            //assert

            await using AppDbContext context2 = new AppDbContext(options);

            Assert.NotNull(deleted);

            Assert.Empty(context2.Machines);

            Assert.Equal(deleted.id, machine.id);
            Assert.Equal(deleted.Name, machine.Name);
            Assert.Equal(deleted.Status, machine.Status);







        }


        [Fact]

        public async Task GetAllAsync_MultipleMachines_ReturnsAllMachines()
        {
            //arrange
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("GetAllReturnsAll").Options;
            await using AppDbContext context1 = new AppDbContext(options);

            MachineService service = new MachineService(context1);

            Machine machine1 = new Machine
            {
                id = 1,
                Name = "machine1",
                Status = "Running"

            };

            Machine machine2 = new Machine
            {
                id = 2,
                Name = "machine2",
                Status = "Stopped"

            };

            Machine machine3 = new Machine
            {
                id = 3,
                Name = "machine3",
                Status = "Error"

            };

            context1.Add(machine1);
            context1.Add(machine2);
            context1.Add(machine3);
            await context1.SaveChangesAsync();

            //act


            List<Machine>? result = await service.GetAllAsync();

            //assert
            
            await using AppDbContext context2 = new AppDbContext(options);                                                      /*When the variable goes out of scope, the await using statement calls and awaits 
                                                                                                                                 * the object's DisposeAsync() method, ensuring that asynchronous cleanup 
                                                                                                                                 * operations (like closing database connections or flushing buffers) complete 
                                                                                                                                 * without blocking the thread. */

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            //Assert.Equal(1, result[0].id);
            //Assert.Equal("machine1",result[0].Name);

            //Assert.Equal(2, result[1].id);
            //Assert.Equal("machine2", result[1].Name);

            //Assert.Equal(3, result[2].id);
            //Assert.Equal("machine3", result[2].Name);
            //if the service does not order this will fail

            Assert.Contains(result, m => m.id == 1 && m.Name == "machine1");//therefore we are searching them by id and checking that they exist
            Assert.Contains(result, m => m.id == 2 && m.Name == "machine2");
            Assert.Contains(result, m => m.id == 3 && m.Name == "machine3");





        }

    }



}