using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers;
using WebApplication1.Models;
using Moq;
using WebApplication1.Services;



namespace WebApplication1.Tests
{
    public class MachineControllerTests
    {
        [Fact]
        public async Task RetrievedById_MachineExists_ReturnsOk()
        {
            //arrange
            FakeMachineService fakeService = new FakeMachineService();
            MachinesController controller = new MachinesController(fakeService);


            //act
            IActionResult result = await controller.RetrieveByIdAsync(5);


            //assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);

            Machine machine = Assert.IsType<Machine>(okResult.Value);

            Assert.Equal(5, machine.id);
            Assert.Equal("machine", machine.Name);
            Assert.Equal("Running", machine.Status);
            //we are not testing if the sql works, or if entity framework works, or if the database contains id 5, we are just testing the controller logic,
            //FAKESERVICE GIVES CONTROLLER MACHINE, CONTROLLER SHOULD RETURN 200 OK and put that machine in the response


        }
    


    [Fact]

        public async Task RetrieveById_MachineNonExistent_ReturnsNotFound()
        {
            FakeMachineService fakeService = new FakeMachineService();
            MachinesController controller = new MachinesController(fakeService);


            IActionResult result = await controller.RetrieveByIdAsync(2);

            NotFoundResult nullResult = Assert.IsType<NotFoundResult>(result);





        }



        [Fact]

        public async Task RetrieveById_ReturnsOk_UsingMoq()
        {
            //arrange
            Mock<IMachineService> mockService = new Mock<IMachineService>();//give me a fake object that follows the IMachineServiceContract we no longer need to create a whole class with methods to be implemented

            Machine machine = new Machine { id = 5, Name = "Test Machine", Status = "Running" };

            mockService.Setup(service => service.RetrieveByIdAsync(5)).ReturnsAsync(machine);//If the controller calls RetrieveByIdAsync(5), give it this machine.
                                                                                             //On this mock service, set it up so that when RetrieveByIdAsync(5) is called, return machine asynchronously.

            /*instead of having to write this in a seperate class:
             
             if (id == 5)
{
             return Task.FromResult<Machine?>(machine);}*/


            MachinesController controller = new MachinesController(mockService.Object);//we cannot call MachinesController controller = new MachinesController(mockService); as mockservice is Mock<IMachineService>, the controller wants IMachineService
            //act

            IActionResult result = await controller.RetrieveByIdAsync(5);
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);

            Machine finalMachine = Assert.IsType<Machine>(okResult.Value);

            Assert.Equal(5, finalMachine.id);
            Assert.Equal("Test Machine", finalMachine.Name);
            Assert.Equal("Running", finalMachine.Status);

            //verify


            //"Did the controller interact with its dependency correctly?"

            mockService.Verify(
                service => service.RetrieveByIdAsync(5),
                Times.Exactly(1));//verify that RetrieveByIdAsync(5) was called only once
                            //suppose someone completely broke the controller and it didn't call the service correctly.
                            //Moq can also check whether a call actually happened.

        }
    }
}
    
