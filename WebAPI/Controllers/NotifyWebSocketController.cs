using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;
using WebBO.Areas.Pikegame.Controllers;
using WebBO.General.Repository.Connection;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/NotifyWebSocket")]
    public class NotifyWebSocketController : ApiController
    {
        protected ConnectionFactory _connectionFactory;
        private static List<WebSocket> _sockets = new List<WebSocket>();
        [Route]
        [HttpGet]
        public HttpResponseMessage Connect()
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(ProcessWSSingle);
            }
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols); //構造同意切換至Web Socket的Response.
            //return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }

        private async Task ProcessWSSingle(AspNetWebSocketContext arg)
        {
            WebSocket socket = arg.WebSocket;
            _sockets.Add(socket);//此處將web socket對象加入一個靜態列表中
            while (true)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (socket.State == WebSocketState.Open)
                {
                    var sendmessagedt = "";
                    string message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

                    //var resultdt = new DataTable();
                    var resultdt = new SessiondetialController().GetWisdomGunhead(message);


                    if (resultdt.Rows.Count > 0)
                    {
                        var redhit = resultdt.Rows[0]["redhit"].ToString();
                        var bluehit = resultdt.Rows[0]["bluehit"].ToString();
                        var sessiondetialid = resultdt.Rows[0]["sessiondetialid"].ToString();
                        var sessionid = resultdt.Rows[0]["sessionid"].ToString();
                        if (redhit == "True")
                        {
                            sendmessagedt = "Red_"+ redhit+"_"+ sessiondetialid+"_"+ sessionid;
                        }
                        else if (bluehit == "True")
                        {
                            sendmessagedt = "Blue_" + bluehit + "_" + sessiondetialid + "_" + sessionid;
                        }                        
                    }
                    else
                    {
                        sendmessagedt = "目前沒有最新得分資訊!";

                    }

                    string returnMessage = sendmessagedt;

                    buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(returnMessage));
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Datatable轉化為Byte[]
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static byte[] DatatableToByte(DataTable dt)
        {
            byte[] bytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, dt);
                bytes = memoryStream.GetBuffer();
            }
            return bytes;

        }
    }
}