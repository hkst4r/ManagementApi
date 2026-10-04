using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using WebApplication1.Data;

//Integration testing, ha nitestjaw lapplication kollox mhux certu bicciet minnha, f Controller Unit Tests kreajna controller u servizz falz biex naraw li l logic tal controller wahdu jahdem
//f Service/Ef tests kreajna service u fake database biex naraw li s service logic jahdem kif ukoll l EF jikkomunika kif suppost mal inline memory database. muzajnix sql
//F integration testing ha nkunu qed nitestjaw kollox, (Do these componenet actually work together over http)

//KOLLHA GHANDNA BZONNOM
namespace WebApplication1.Tests
{
    //The compiler generates the Program class behind the scenes. Declaring:

    public partial class Program() { }

    //makes that generated type accessible to our test project.
    public class MachinesApiIntegrationTests
    {
        [Task]

        public async Task GetMachine_MachineExists_ReturnsOk()
        {
            //Arrange


            WebApplicationFactory <Program> factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {

                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }


                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("IntegrationTestDb");
                    });

                    //bazikament li qed nghidulu, sib id dependency injection t AppDbContext, nehhija u minflok ha nuzaw temporary databse (InMemoryDatabase) jisima "IntegratonTestDb

                    //xorta qed nuzaw il machine service, machine controller ta veru, kemm biddilna d database ax ma rridux it tests jithalltu mat database vera.

                });

                //nuzaw program ghax lapp tibda min program.cs u fih kollox li ha nuzaw, li qed jaghmel WebApplicationFactory, ASP.NET, ibda l application,
                //ikkrea l controller id dependencies u processa r routers, kull ma ha naghmel jien nibatlek l http requests
                //flok qed nafsu get fuq swagger, naghmlu await client.GetAsync("api/machines/5") imbaghad min hemm naraw xjigri.

                HttpClient client = factory.CreateClient(); //client will act as swagger

                }
            }   
        }
        
    }
}
