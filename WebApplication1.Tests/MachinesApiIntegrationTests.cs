using Azure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Writers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Security.AccessControl;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Models;

//Integration testing, ha nitestjaw lapplication kollox mhux certu bicciet minnha, f Controller Unit Tests kreajna controller u servizz falz biex naraw li l logic tal controller wahdu jahdem
//f Service/Ef tests kreajna service u database temporary apparti tal sql biex naraw li s service logic jahdem kif ukoll l EF jikkomunika kif suppost mal inline memory database. muzajnix sql
//F integration testing ha nkunu qed nitestjaw kollox, (Do these componenet actually work together over http)

//KOLLHA GHANDNA BZONNOM
namespace WebApplication1.Tests
{
    //The compiler generates the Program class behind the scenes. Declaring:

    public class MachinesApiIntegrationTests
    {
        [Fact]

        public async Task GetMachine_MachineExists_ReturnsOk()
        {
            //Arrange

            CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            //WebApplicationFactory<Program> factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            //{

            //    builder.ConfigureServices(services =>
            //    {
            //        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            //        if (descriptor != null)
            //        {
            //            services.Remove(descriptor);
            //        }


            //        services.AddDbContext<AppDbContext>(options =>
            //        {
            //            options.UseInMemoryDatabase("IntegrationTestDb");
            //        });

            //        //bazikament li qed nghidulu, sib id dependency injection t AppDbContext, nehhija u minflok ha nuzaw temporary databse (InMemoryDatabase) jisima "IntegratonTestDb

            //        //xorta qed nuzaw il machine service, machine controller ta veru, kemm biddilna d database ax ma rridux it tests jithalltu mat database vera.

            //    });

            //    //nuzaw program ghax lapp tibda min program.cs u fih kollox li ha nuzaw, li qed jaghmel WebApplicationFactory, ASP.NET, ibda l application,
            //    //ikkrea l controller id dependencies u processa r routers, kull ma ha naghmel jien nibatlek l http requests
            //    //flok qed nafsu get fuq swagger, naghmlu await client.GetAsync("api/machines/5") imbaghad min hemm naraw xjigri.






            //});

            using (IServiceScope scope = factory.Services.CreateScope())//temporarily(using) creates a DI scope ourselves., ef working session
            {
                AppDbContext context =
                    scope.ServiceProvider.GetRequiredService<AppDbContext>();//"DI container, give me the AppDbContext you've configured for this test application."
                                                                             //because we have just replaced its configuration from configuring services in program.cs, we are using ef in memory not the sql db
                context.Machines.Add(new Machine{id = 5, Name = "Integration Machine", Status = "Running"});

                await context.SaveChangesAsync();
            }//scope is disposed, so this AppDbContext and its tracked state are gone.
             //the machine remains stored in the InMemory database.
             //the HTTP request will use a new scope and a new AppDbContext to retrieve it.

            HttpClient client = factory.CreateClient();
            //act
            HttpResponseMessage response = await client.GetAsync("/api/machines/5");//automatically created a scope
            //instead of doing "controller.RetrieveByIdAsync(5);":

            //assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            //If a real HTTP GET request is sent for an existing machine does the API return HTTP 200 OK?

            Machine? returnedMachine = await response.Content.ReadFromJsonAsync<Machine>();//http returns a json machine, we need it as a c# object

            //we are ALSO testing that the machine it returned is the correct one.

            Assert.NotNull(returnedMachine);
            Assert.Equal(5, returnedMachine.id);
            Assert.Equal("Integration Machine", returnedMachine.Name);
            Assert.Equal("Running", returnedMachine.Status);


        }


        [Fact]

        public async Task GetMachine_MachineDoesntExists_ReturnsNotFound()
        {
            //Arrange

            CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            //WebApplicationFactory<Program> factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            //builder.ConfigureServices(services =>
            //{
            //    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            //    if (descriptor != null)
            //    {
            //        services.Remove(descriptor);
            //    }


            //    services.AddDbContext<AppDbContext>(options =>
            //    {
            //        options.UseInMemoryDatabase("IntegrationTestDb");
            //    });



            //}));


            using (IServiceScope scope = factory.Services.CreateScope())
            {
                AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Add(new Machine { id = 1, Name = "Test", Status = "Running" });

                await context.SaveChangesAsync();
            }


            HttpClient client = factory.CreateClient();


            HttpResponseMessage response = await client.GetAsync("/api/machines/5");

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);


        }


        [Fact]


        public async Task CreateMachine_ValidMachine_ReturnsCreated()
        {
            //arrange

            CustomWebApplicationFactory factory = new CustomWebApplicationFactory();
            HttpClient client = factory.CreateClient();

            Machine machine = new Machine { id = 100, Name = "magna", Status = "Running" };
            //act
            HttpResponseMessage response = await client.PostAsJsonAsync("api/machines", machine);//postasjsonasync, .net method converts c# post to json

            //// Simplified idea of what ASP.NET does internally b httpresponsemessage...:

            //using (IServiceScope requestScope = CreateScope())
            //{
            //    // create controller
            //    // create MachineService
            //    // create AppDbContext

            //    // process POST /api/machines

            //} // request finished → scope disposed


            //assert
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);//status code should be 'created'

            //niccekjaw jekk il machine it tajba giet imdahhla
            Machine? resultMachine = await response.Content.ReadFromJsonAsync<Machine>();

            Assert.NotNull(resultMachine);
            Assert.Equal(100, resultMachine.id);
            Assert.Equal("magna", resultMachine.Name);
            Assert.Equal("Running", resultMachine.Status);

            //GHADNA MA NAFUX JEKK DAHALX FID DATABASE


            using (IServiceScope scope = factory.Services.CreateScope())//using kull mqed taghmel, tikkontrolla meta ha nwaqqfu liscope u narmu l context
            {
                AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                Machine? machineInDb = await context.Machines.FirstOrDefaultAsync(m => m.id == 100);

                Assert.NotNull(machineInDb);
                Assert.Equal("magna", machineInDb.Name);
                Assert.Equal("Running", machineInDb.Status); 

            }



        }


        [Fact]

        public async Task UpdateMachine_ExistingMachine_ReturnsOk()
        {
            CustomWebApplicationFactory factory = new CustomWebApplicationFactory();//qed intuh webapp bdawk il bidliet li ghamilna

            using(IServiceScope scope = factory.Services.CreateScope())
            {
                AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Add(new Machine { id = 22, Name = "toBeUpdated", Status = "broken" });

                await context.SaveChangesAsync();
            }

            HttpClient client = factory.CreateClient();

            Machine updated = new Machine { id = 22, Name = "updated", Status = "Running" };
            //act
            HttpResponseMessage response = await client.PutAsJsonAsync("api/machines/22", updated);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Machine? check = await response.Content.ReadFromJsonAsync<Machine>();

            Assert.NotNull(check);
            Assert.Equal("updated", check.Name);
            Assert.Equal("Running", check.Status);


            using(IServiceScope scope = factory.Services.CreateScope())
            {
                AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                Machine? machineInDb = await context.Machines.FirstOrDefaultAsync(m => m.id == 22);

                Assert.NotNull(machineInDb);
                Assert.Equal("updated", machineInDb.Name);
                Assert.Equal("Running", machineInDb.Status);

            }
        }


        [Fact]


        public async Task DeleteMachine_ExistingMachine_ReturnsOk()
        {
            //arrange
            CustomWebApplicationFactory factory = new CustomWebApplicationFactory();

            using (IServiceScope scope = factory.Services.CreateScope())
            {
                AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Add(new Machine { id = 5, Name = "SeTitnehha", Status = "Running" });

                await context.SaveChangesAsync();
            }
            HttpClient client = factory.CreateClient();
            //act

            HttpResponseMessage result = await client.DeleteAsync("api/machines/5");

            //assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, result.StatusCode);

            using (IServiceScope scope = factory.Services.CreateScope())
            {
                AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                Machine? deleted = await context.Machines.FirstOrDefaultAsync(m => m.id == 5);
                Assert.Null(deleted);


            }
        }
    }
}
