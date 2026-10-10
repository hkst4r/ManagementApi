using Microsoft.AspNetCore.SignalR;

namespace WebApplication1.Hubs
{//test bis sens https://localhost:7293/signalr-test.html
    public class MachineHub : Hub
    {
        public async Task SendMachineUpdate(int id, string status)
        {
            await Clients.All.SendAsync(//clients.all send a message to every client currently connected to this Hub.
                
                "MachineStatusChanged",//send an event named MachineStatusChanged to connected clients, including the machine ID and its new status.
                id,
                status);
        }
    }
}

/*SignalR does not replace rest, nistaw nuzaw rest ghal
  
GET /api/machines
POST / api / machines
PUT / api / machines / 5
DELETE / api / machines / 5


u nistaw nuzaw signalR

MachineStatusChanged
MachineConnected
MachineDisconnected




For example:
PowerShell
    |
    | PUT /api/machines/5
    | Status = Stopped
    v
MachinesController
    |
    v
MachineService
    |
    v
SQL Server
    |
    | Update successful
    v
SignalR
    |
    | MachineStatusChanged(5, "Stopped")
    |
    +----------> Dashboard A
    |
    +----------> Dashboard B


*/