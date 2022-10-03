using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using isRock.Framework;
using isRock.Framework.WebAPI;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;



/// <summary>
/// 產品邏輯與推播
/// Date:2017/07/11
/// Author:ivis
/// </summary>
namespace WebAPI.Logic
{
    #region "邏輯是類別內必要的"
    /// <summary>
    /// 邏輯層
    /// </summary>
    public class UserData : BusinessLogicBase
    {
        /// <summary>
        /// 客戶資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> CustData(JObject obj)
        {
            string sql = "";
           // sql = "select *   from  CustData  ";
            sql = ReturnFace.ReturnSQL("CustData", obj);
            return ReturnFace.ReturnData(sql);
        }


        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> GetUserData(JObject obj)
        {
            string sql = "";
            sql = "select *   from  CustData  ";
            
            return ReturnFace.ReturnData(sql);
        }


        /// <summary>
        /// Log資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> EventLog(JObject obj)
        {
            string sql = "";
            //sql = "select *   from  EventLog  ";
            sql = ReturnFace.ReturnSQL("EventLog", obj);
            return ReturnFace.ReturnData(sql);
        }

        /// <summary>
        /// 群組資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> GroupData(JObject obj)
        {
            string sql = "";
            //sql = "select *   from  GroupData  ";
            sql = ReturnFace.ReturnSQL("GroupData", obj);
            return ReturnFace.ReturnData(sql);
        }

        /// <summary>
        /// 待辦事項資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> MemoItem(JObject obj)
        {
            string sql = "";
            //sql = "select *   from  MemoItem  ";
            sql = ReturnFace.ReturnSQL("MemoItem", obj);
            return ReturnFace.ReturnData(sql);
        }

        /// <summary>
        /// 訂單明細資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> OrderDetail(JObject obj)
        {
            string sql = "";
            //sql = "select *   from  OrderDetail  ";
            sql = ReturnFace.ReturnSQL("OrderDetail", obj);
            return ReturnFace.ReturnData(sql);
        }

        /// <summary>
        /// 訂單資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> Orders(JObject obj)
        {
            string sql = "";
            //sql = "select *   from  Orders  ";
            sql = ReturnFace.ReturnSQL("Orders", obj);
            return ReturnFace.ReturnData(sql);
        }


        /// <summary>
        /// 上層功能資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> ParentFuction(JObject obj)
        {
            string sql = "";
           // sql = "select *   from  ParentFuction  ";
            sql = ReturnFace.ReturnSQL("ParentFuction", obj);
            return ReturnFace.ReturnData(sql);
        }


        /// <summary>
        /// 權限資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> Permit(JObject obj)
        {
            string sql = "";
            //sql = "select *   from  Permit  ";
            sql = ReturnFace.ReturnSQL("Permit", obj);
            return ReturnFace.ReturnData(sql);
        }

        /// <summary>
        /// 價格資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> Price(JObject obj)
        {
            string sql = "";
           // sql = "select *   from  Price  ";
            sql = ReturnFace.ReturnSQL("Price", obj);
            return ReturnFace.ReturnData(sql);
        }


        /// <summary>
        /// 折扣資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> PriceDiscount(JObject obj)
        {
            string sql = "";
           // sql = "select *   from  PriceDiscount  ";
            sql = ReturnFace.ReturnSQL("PriceDiscount", obj);
            return ReturnFace.ReturnData(sql);
        }


        /// <summary>
        /// 價格類別資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> PriceType(JObject obj)
        {
            string sql = "";
            //sql = "select *   from  PriceType  ";
            sql = ReturnFace.ReturnSQL("PriceType", obj);
            return ReturnFace.ReturnData(sql);
        }

        /// <summary>
        /// 產品資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> Products(JObject obj)
        {
            string sql = "";
           // sql = "select *   from  Products  ";
            sql = ReturnFace.ReturnSQL("Products", obj);
            return ReturnFace.ReturnData(sql);
        }

        /// <summary>
        /// 產品類別資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> ProductType(JObject obj)
        {
            string sql = "";
            sql = "select *   from  ProductType  ";
            return ReturnFace.ReturnData(sql);
        }

        /// <summary>
        /// 員工資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> StaffData(JObject obj)
        {
            string sql = "";
            //sql = "select *   from  StaffData  ";
             sql = ReturnFace.ReturnSQL("StaffData", obj);
            return ReturnFace.ReturnData(sql);
        }

        /// <summary>
        /// 供應商資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> Supplier(JObject obj)
        {
            string sql = "";
            //sql = "select *   from  Supplier  ";
            sql = ReturnFace.ReturnSQL("Supplier", obj);
            return ReturnFace.ReturnData(sql);
        }

        /// <summary>
        /// 系統功能資料
        /// </summary>
        /// <returns></returns>
        public ExecuteCommandDefaultResult<List<Dictionary<string, object>>> SysFunction(JObject obj)
        {
            string sql = "";
            //sql = "select *   from  SysFunction  ";
            sql = ReturnFace.ReturnSQL("SysFunction", obj);
            return ReturnFace.ReturnData(sql);
        }

    }
    #endregion


    #region "推播資料並不是必要的，需要才參考本類別自行增加"
    /// <summary>
    /// 推播層
    /// </summary>    
    [HubName("UserDataHub")]
    public class UserDataHub : Hub
    {
        //制式寫法
        static IHubContext HubContext = GlobalHost.ConnectionManager.GetHubContext<POSDataHub>();

        //客戶端IDs
        static List<string> clientIds = new List<string>();

        /// <summary>
        /// override OnConnected
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            // Get the clientId
            var clientId = Context.ConnectionId;

            //Keep it in memory
            clientIds.Add(clientId);
            return base.OnConnected();
        }

        /// <summary>
        /// override OnDisconnected
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            //Get the clientId
            var clientId = Context.ConnectionId;

            //Remove it from memory
            clientIds.Remove(clientId);
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        ///  override OnReconnected
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        /// <summary>
        /// 用來GetHubName，抓注意 typeof(ProductDataHub) 的class 名稱問題
        /// 其實就是用來抓取 [HubName("ProductDataHub")] 
        /// </summary>
        /// <returns>HubName</returns>
        private static string GetHubName()
        {
            Type type = typeof(POSDataHub);
            HubNameAttribute attribute =
                type.GetCustomAttributes(typeof(HubNameAttribute), false).Cast<HubNameAttribute>().SingleOrDefault();
            return attribute != null ? attribute.HubName : type.Name;
        }

        /// <summary>
        /// 用來建構HubName、Method的集合 ，方便吐JSON時一併帶出
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns>HubName、Method的集合</returns>
        public static Dictionary<string, object> buindHubs(string methodName)
        {
            Dictionary<string, object> dc_hub = new Dictionary<string, object>();
            dc_hub.Add("HubName", GetHubName());
            dc_hub.Add("Method", methodName);
            return dc_hub;
        }


        /// <summary>
        /// 推播，主要是註冊函數給client用
        /// </summary>
        /// <param name="task">要被註冊的物件param>
        /// <param name="connectionId">連接ID</param>
        /// <returns>回傳註冊完的HubName、Method</returns>
        public static object asyncMainData(object task, string connectionId)
        {
            if (!String.IsNullOrEmpty(connectionId))
            {
                //故意 註冊.asyncMainData 跟函數名稱一樣 比較好管理
                HubContext.Clients.Client(connectionId).asyncMainData(task, false);
                HubContext.Clients.AllExcept(connectionId).asyncMainData(task, true);
            }
            else
            {
                //故意 註冊.asyncMainData 跟函數名稱一樣 比較好管理
                HubContext.Clients.All.asyncMainData(task, true);
            }

            string myMethod = System.Reflection.MethodBase.GetCurrentMethod().Name;

            return buindHubs(myMethod);
        }


    }
    #endregion

}