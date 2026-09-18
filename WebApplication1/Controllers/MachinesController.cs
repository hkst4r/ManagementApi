using Microsoft.AspNetCore.Mvc;
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

        public IEnumerable<Machine> GetAll()
        {

           
                return _machineService.GetAll();

            
        }


        [HttpGet("{id}")]

        public IActionResult RetrieveById(int id)
        {
            Machine? idMachine = _machineService.RetrieveById(id);

            if (idMachine != null)
            {
                return Ok(idMachine);

            }

            else
            {
                return NotFound();
            }


        }


        [HttpPost]

        public IActionResult Create(Machine machine)

        {
            _machineService.Create(machine);
            return StatusCode(201, machine);


        }


        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            Machine? toDelete = _machineService.Delete(id);
            if (toDelete != null)
            {
                return NoContent();
            }


            
            
            return NotFound(id);

            

        }

        [HttpPut("{id}")]


        public IActionResult Update(int id, Machine updatedMachine)
        {
            Machine? toUpdate = _machineService.Update(id, updatedMachine);

            if (toUpdate == null)
            {
                return NotFound();
            }

            return Ok(updatedMachine);
        }
    
    }
}
