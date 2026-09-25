using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Tests
{
    internal class FakeMachineService : IMachineService
    {
        public Task<Machine> CreateAsync(Machine machine)
        {
            throw new NotImplementedException();
        }

        public Task<Machine?> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Machine>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Machine?> RetrieveByIdAsync(int id)
        {

            if (id == 5)
            {
                Machine machine = new Machine { id = 5, Name = "machine", Status = "Running" };

                return Task.FromResult<Machine?>(machine);
            }

            return Task.FromResult<Machine?>(null);//the fake must obey the same contract.   Task.FromResult basically says: "Here's a Task, and it's already finished. Its result is this object."
        }

        public Task<Machine?> UpdateAsync(int id, Machine updatedMachine)
        {
            throw new NotImplementedException();
        }
    }
}
