using WebApplication1.Models;


namespace WebApplication1.Services
{
    public interface IMachineService//qed niktbu 'kuntratt', kull class li jkolla xtaqsam mieghi, trid timplementja dawn il methods
    {
        Task<List<Machine>> GetAllAsync();

        public Task <Machine?> RetrieveByIdAsync(int id);

        public Task<Machine> CreateAsync(Machine machine);

        public Task<Machine?> DeleteAsync(int id);

        public Task<Machine?> UpdateAsync(int id, Machine updatedMachine);







    }
}
