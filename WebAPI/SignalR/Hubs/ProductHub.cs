using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace POSAPI.SignalR.Hubs
{
    public class AdUser
    {
        public String Cn { get; set; }
        public String Sn { get; set; }
        public String GivenName { get; set; }
        public String DisplayName { get; set; }
        public String Status { get; set; }
    }


    [HubName("ProductHub")]
    public class ProductHub:Hub
    {
        static IHubContext HubContext = GlobalHost.ConnectionManager.GetHubContext<ProductHub>();


        public void getMsg(List<string> task, string connectionId)
        {



            if (!String.IsNullOrEmpty(connectionId))
            {
                HubContext.Clients.Client(connectionId).getMsg(task, false);
                HubContext.Clients.AllExcept(connectionId).getMsg(task, true);
            }
            else
            {
                HubContext.Clients.All.getMsg(task, true);
            }

            //HubContext.Clients.All.getMsg(messageList,true);


        }
        public void Update(IEnumerable<object> adUsers, String connId)
        {

            if (!String.IsNullOrEmpty(connId))
            {
                //Send back to certain client with Connection id
                HubContext.Clients.Client(connId).setPordName(adUsers);
            }
            else
            {
                //Send back to all connections
                HubContext.Clients.All.setPordName(adUsers);
            }
        }


    }

}