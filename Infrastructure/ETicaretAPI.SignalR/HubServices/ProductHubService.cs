using ETicaretAPI.Application.Abstraction.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.SignalR.HubServices
{
    public class ProductHubService : IProductHubService
    {
        public Task ProductAddedMessageAsync(string message)
        {
            throw new NotImplementedException();
        }
    }
}
