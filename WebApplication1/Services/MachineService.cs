using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class MachineService : IMachineService//MachineService ha jiehu dak il kuntratt biex tkun zgur li l methods li ghandi bzonn ha jigu kreati, pero
        //ha niktbu wkoll KIF ghandhom jigu kreati, fl interface ghidna, isma, jien ma jimpurtanix kif se jsiru, imma rridhom isiru u jirretornaw dan l output
        //f machineservice, qed nghidu il methods kif se jahdmu
    {

        private List<Machine> _machines = new List<Machine>();
        public Machine Create(Machine machine)
        {
            _machines.Add(machine);
            return machine;
        }

        public Machine? Delete(int id)
        {
            Machine? toDelete = _machines.FirstOrDefault(mach => mach.id ==id );
            if (toDelete != null)
            {
                _machines.Remove(toDelete);
                return toDelete;

            }

            return null;
        }

        public IEnumerable<Machine> GetAll()
        {
            return _machines;
        }

        public Machine? RetrieveById(int id)
        {
            Machine? idedMachine = _machines.FirstOrDefault(m => m.id == id);
            return idedMachine;
        }

        public Machine? Update(int id, Machine updatedMachine)
        {
            Machine? toUpdate = _machines.FirstOrDefault(m => m.id == id);
            if (toUpdate != null)
            {
                toUpdate.Name = updatedMachine.Name;
                toUpdate.Status = updatedMachine.Status;
                return toUpdate;//actual value in list, updatedmachine kull mhu ha turi x dahal fl http request, mux xhemm fil lista.
            }

          
                return null;
            



        }
    }
}
