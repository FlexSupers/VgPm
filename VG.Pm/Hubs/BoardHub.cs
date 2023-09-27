using Microsoft.AspNetCore.SignalR;
using VG.Pm.Data.ViewModel;

namespace VG.Pm.Hubs
{
    public class BoardHub : Hub
    {
        public async Task ItemUpdate(TaskViewModel item)
        {
            await Clients.All.SendAsync("ItemUpdate", item);
        }

        public async Task ColumnUpdate(int columnId)
        {
            await Clients.All.SendAsync("ColumnUpdate", columnId);
        }

        public async Task ItemAdded(int columnId, int itemId)
        {
            await Clients.All.SendAsync("ItemAdded", columnId, itemId);
        }

        public async Task ItemDeleted(int columnId, int itemId)
        {
            await Clients.All.SendAsync("ItemDeleted", columnId, itemId);
        }
        public async Task ColumnAdded(int columnId)
        {
            await Clients.All.SendAsync("ColumnAdded", columnId);
        }

        public async Task ColumnDeleted(int columnId)
        {
            await Clients.All.SendAsync("ColumnDeleted", columnId);
        }
    }
}
