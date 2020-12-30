using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebProject.Hubs
{
    public class TableHub : Hub
    {
        [HubMethodName("updateAvailable")]
        public async void updateAvailable()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TableHub>();
            await context.Clients.All.newUpdate();   //Lets the Database Script know that the Person table has changed
        }
    }
}