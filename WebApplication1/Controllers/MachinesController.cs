using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Services;



namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachinesController : ControllerBase
    {
        private readonly ILogger<MachinesController> _logger;

        private readonly IMachineService _machineService;
        
        //nikkrejaw id dependency, qed nghidlu, isma ghandi bzonn oggett li jimxi mal kuntratt IMachineService,
        //jista jkun li jkun jaqwa li jimxi mieghu, 

        public MachinesController(IMachineService machineService, ILogger<MachinesController> logger)
         //f program.cs, diga ktibnilu, isma, meta f kwalunkwe post ha nsaqsik ghal xi haga IMachineService, irridek ittini MachineService.cs.

        {
            _machineService = machineService;//itfa l parameter(MachineService) god dependency li kreajna
            _logger = logger;//di tal logger

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
            _logger.LogInformation("Retrieving machine with ID {MachineId}", id);
            Machine? result = await task;

            if (result != null)
            {
                
                return Ok(result);

            }

            else
            {
                _logger.LogWarning("Machine {MachineId} was not found", id);//we are not handling exceptions, if something
                //unexpected fails the exception-handling middleware deals with the HTTP error response, the logging system records the 
                //failure internally
                return NotFound();

            }
            

        }


        [HttpPost]

        public async Task<ActionResult> CreateAsync(CreateMachineDto dto)

        {
            Machine machine = new Machine
            {
                Name = dto.Name,
                Status = dto.Status
            };

            Machine created = await _machineService.CreateAsync(machine);

            return StatusCode(201, created);


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



//Powershell example
/*
PS C:\Users\rysten> $machine = Invoke-RestMethod `
>>     -Uri "https://localhost:7293/api/machines/5" `
>>     -Method Get
PS C:\Users\rysten> $body = @{
>>     name = "PowerShell Machine"
>>     status = "Running"
>> } | ConvertTo-Json
PS C:\Users\rysten> $body
{
    "name":  "PowerShell Machine",
    "status":  "Running"
}
PS C:\Users\rysten> $response = Invoke-RestMethod `
>>     -Uri "https://localhost:7293/api/machines" `
>>     -Method Post `
>>     -Body $body `
>>     -ContentType "application/json"
PS C:\Users\rysten> $response

id name               status
-- ----               ------
 0 PowerShell Machine Running
*/