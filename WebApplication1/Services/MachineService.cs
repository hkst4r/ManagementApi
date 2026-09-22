using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Services
{
    public class MachineService : IMachineService//MachineService ha jiehu dak il kuntratt biex tkun zgur li l methods li ghandi bzonn ha jigu kreati, pero
        //ha niktbu wkoll KIF ghandhom jigu kreati, fl interface ghidna, isma, jien ma jimpurtanix kif se jsiru, imma rridhom isiru u jirretornaw dan l output
        //f machineservice, qed nghidu il methods kif se jahdmu
    {
        //public class MachineService//we have 2 levels of di, machinescontroller ghandu bzonn xi haga imachineservive(ghidnilu juza din il class),
        //MachineService ghandu bzonn db context, li rregistrajna u ghidna kif se tkun f program.cs
        //{
        //    private readonly AppDbContext _context;

        //    public MachineService(AppDbContext context)//"MachineService ghandu bzonn AppDbContext. Naf kif nikkrea wiehed ghax gie rregistrat f program.cs"
        //    {
        //        _context = context;
        //    }
        //} 



        //*
        //
        //await = this method can't continue until this operation finishes, but the thread doesn't need to sit here doing nothing.

        private readonly AppDbContext _dbContext;

        //qed nuzaw async, Task u await, biex ASP.net jista juza t thread al affarijiet ohra waqt li qed niehdu jew nibatu lid database, serive awaits ef core, controller awaits service  
        public MachineService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }


        public async Task<Machine> CreateAsync(Machine machine)
        {
            _dbContext.Machines.Add(machine);//tell EF what changed
            await _dbContext.SaveChangesAsync();//actually persist the changes to the database, start sql save, await*, save finishes, continue and return machine
            return machine;
        }

        public async Task<Machine?> DeleteAsync(int id)
        {
            Machine? toDelete = await _dbContext.Machines.FirstOrDefaultAsync(mach => mach.id ==id );
            if (toDelete != null)
            {
                _dbContext.Machines.Remove(toDelete);
                await _dbContext.SaveChangesAsync();
                return toDelete;

            }

            return null;
        }

        public async Task<List<Machine>> GetAllAsync()
        {
            //Task<List<Machine>> task = _dbContext.Machines.ToListAsync();
            //List<Machine> result = await task;

            List<Machine> result = await _dbContext.Machines.ToListAsync();






            return result;
        }

        public async Task<Machine?> RetrieveByIdAsync(int id)
        {
            //Task<Machine?> task = _dbContext.Machines.FirstOrDefaultAsync(m => m.id == id);
            //Machine? idedMachine = await task;

            Machine? idedMachine = await _dbContext.Machines.FirstOrDefaultAsync(m => m.id == id);
            return idedMachine;
        }

        public async Task<Machine?> UpdateAsync(int id, Machine updatedMachine)
        {
            //Task<Machine?> task = _dbContext.Machines.FirstOrDefaultAsync(m => m.id == id);
            //Machine result = await task;


            Machine? result = await _dbContext.Machines.FirstOrDefaultAsync(m => m.id == id);
            if (result != null)
            {
                result.Name = updatedMachine.Name;
                result.Status = updatedMachine.Status;
                await _dbContext.SaveChangesAsync();
                return result;//actual value in list, updatedmachine kull mhu ha turi x dahal fl http request, mux xhemm fil lista.
            }

          
                return null;
            



        }
    }
}
