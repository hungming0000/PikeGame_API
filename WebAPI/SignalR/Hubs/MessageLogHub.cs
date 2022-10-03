using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;



namespace POSAPI.SignalR.Hubs
{


    [HubName("getMsg")]
    public class MessageLogHub:Hub
    {


        List<string> messageList = new List<string>();
   
        static IHubContext HubContext = GlobalHost.ConnectionManager.GetHubContext<MessageLogHub>();

        List<string> clientIds = new List<string>();

        public override Task OnConnected()
        {

            // Get the clientId
            var clientId = Context.ConnectionId;
            //Keep it in memory
            clientIds.Add(clientId);

            return base.OnConnected();
        
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //Get the clientId
            var clientId = Context.ConnectionId;
            //Remove it from memory
            clientIds.Remove(clientId);

            //custom logic here
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public IEnumerable<string> GetAllMessagesLog()
        {
            string GUID1 = Guid.NewGuid().ToString("N");

            string GUID2 = Guid.NewGuid().ToString("N");

            string GUID3 = Guid.NewGuid().ToString("N");

            messageList.Add(GUID1);
            messageList.Add(GUID2);
            messageList.Add(GUID3);
            getMsg(messageList, string.Empty);

            return messageList.ToArray();


        }

        public IEnumerable<string> GetMessagesLog()
        {
            string GUID1 = Guid.NewGuid().ToString("N");

            string GUID2 = Guid.NewGuid().ToString("N");

            string GUID3 = Guid.NewGuid().ToString("N");

            messageList.Add(GUID1);
            messageList.Add(GUID2);
            messageList.Add(GUID3);
            getMsg(messageList, string.Empty);

            return messageList.ToArray();


        }

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


        //public IEnumerable<string> GetAllMessagesLog()
        //{

        //    messageList.Add("a");
        //    messageList.Add("ab");
        //    messageList.Add("as");
        //    messageList.Add("aasj");
        //    messageList.Add("aasasas");
        //    HubContext.Clients.All.getMsg(messageList.ToArray());

        //    return messageList.ToArray();

        //    //var messageList = new List<string>();
        //    //messageList.Add("a");
        //    //messageList.Add("ab");
        //    //messageList.Add("as");
        //    //messageList.Add("aasj");
        //    //messageList.Add("aasasas");
        //    //HubContext.Clients.All.getMsg(messageList.ToArray());

        //    //return messageList.ToArray();

        //}

        public static void InsertTask(string task, string connectionId)
        {
            if (!String.IsNullOrEmpty(connectionId))
            {
                HubContext.Clients.Client(connectionId).onInsertTask(task, false);
                HubContext.Clients.AllExcept(connectionId).onInsertTask(task, true);
            }
            else
            {
                HubContext.Clients.All.onInsertTask(task, true);
            }
        }

      
    }
}