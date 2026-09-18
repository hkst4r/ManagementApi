using WebApplication1.Models;


namespace WebApplication1.Services
{
    public interface IMachineService//qed niktbu 'kuntratt', kull class li jkolla xtaqsam mieghi, trid timplementja dawn il methods
    {
        public IEnumerable<Machine> GetAll();

        public Machine? RetrieveById(int id);

        public Machine Create(Machine machine);

        public Machine? Delete(int id);

        public Machine? Update(int id, Machine updatedMachine);







    }
}
