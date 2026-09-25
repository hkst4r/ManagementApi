using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;



namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachinesController : ControllerBase
    {

        private readonly IMachineService _machineService;
        
        //nikkrejaw id dependency, qed nghidlu, isma ghandi bzonn oggett li jimxi mal kuntratt IMachineService,
        //jista jkun li jkun jaqwa li jimxi mieghu, 

        public MachinesController(IMachineService machineService)
         //f program.cs, diga ktibnilu, isma, meta f kwalunkwe post ha nsaqsik ghal xi haga IMachineService, irridek ittini MachineService.cs.

        {
            _machineService = machineService;//itfa l parameter(MachineService) god dependency li kreajna
        }

       
        //private static readonly List<Machine> Machines = new()
        //{
        //    new Machine {id = 1, Name = "Server-01", Status = "Running"},
        //    new Machine {id = 2, Name = "Server-02", Status = "Stopped"},
        //    new Machine {id = 3, Name = "Server-03", Status = "Running"},

        //};


        [HttpGet]

        public async Task <List<Machine>> GetAllAsync()//controller awaits machine service, machine service awaits ef to handle sql
        {

           
                return await _machineService.GetAllAsync();

            
        }


        [HttpGet("{id}")]

        public async Task <IActionResult> RetrieveByIdAsync(int id)
        {
                Task <Machine?> task = _machineService.RetrieveByIdAsync(id);
            Machine? result = await task;

            if (result != null)
            {
                
                return Ok(result);

            }

            else
            {
                return NotFound();
            }


        }


        [HttpPost]

        public async Task<ActionResult> CreateAsync(Machine machine)

        {
            await _machineService.CreateAsync(machine);
            return StatusCode(201, machine);


        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            Machine? toDelete = await _machineService.DeleteAsync(id);

            if (toDelete != null)
            {
                return NoContent();
            }


            
            
            return NotFound(id);

            

        }

        [HttpPut("{id}")]


        public async Task<IActionResult> Update(int id, Machine updatedMachine)
        {
            //Task<Machine?> task = _machineService.UpdateAsync(id, updatedMachine);
            //Machine? result = await task;

            Machine? result = await _machineService.UpdateAsync(id, updatedMachine);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    
    }
}
