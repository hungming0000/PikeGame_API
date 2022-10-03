using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace WebBO.General
{

    class SendSearch
    {
        public GetResult Search(string UserID, string Passwd, string MessageID, string OnlineID)
        {
            GetResult gr = new GetResult();

            //GetDB DB = new GetDB();
            //DataTable d = new DataTable();
            //string getsql = "";


            //string ServerName = "api.hiair.hinet.net";
            //string ServerPort = "8000";
            //HiAirLib.HiNetSMS hn = new HiAirLib.HiNetSMS();

            //if (MessageID.Equals(""))
            //{
            //    gr.Error = "簡訊ID輸入錯誤！";
            //    return gr;
            //}

            //string ReturnMsg = "";
            //short Get = hn.StartCon(ServerName, ServerPort, UserID, Passwd);
            //if (Get == 0)
            //{
            //    bool SendRejected = false;
            //    bool CanRun = false;
            //    short ret;
            //    ret = hn.QueryMsg(MessageID); //查詢狀態
            //    if (ret == 0)
            //    {
            //        CanRun = true;
            //        ReturnMsg += "申請人已收到簡訊!";
            //        gr.Result = ReturnMsg;
            //    }
            //    else
            //    {

            //        switch (ret)
            //        {
            //            case 1:
            //                CanRun = true;
            //                ReturnMsg += "手機未開或在受訊範圍外";
            //                break;
            //            case 2:
            //                CanRun = true;
            //                ReturnMsg += "系統無此messageID的資料";
            //                break;
            //            case 3:
            //                ReturnMsg += "MessageID 格式有誤";
            //                break;
            //            case 4:
            //                CanRun = true;
            //                ReturnMsg += "已送至簡訊中心，尚未完成結果回報";
            //                break;
            //            case 5:
            //                ReturnMsg += "預約傳送時間有誤";
            //                break;
            //            case 6:
            //                ReturnMsg += "傳送二進位訊息到呼叫器";
            //                break;
            //            case 7:
            //                ReturnMsg += "訊息轉碼失敗";
            //                break;
            //            case 8:
            //                ReturnMsg += "手機號碼或簡訊內容格式錯誤";
            //                break;
            //            case 9:
            //                ReturnMsg += "簡訊在Queue Server端，已過期";
            //                break;
            //            case 10:
            //                ReturnMsg += "簡訊在簡訊中心已被移除或超過重送時間";
            //                break;
            //            case 15:
            //                ReturnMsg += "訊息狀態有誤";
            //                break;
            //            case 16:
            //                ReturnMsg += "傳送失敗";
            //                break;
            //            case 17:
            //                ReturnMsg += "訊息無法送達對方";
            //                break;
            //            case 18:
            //                ReturnMsg += "無法判斷的錯誤訊息";
            //                break;
            //            case 19:
            //                CanRun = true;
            //                ReturnMsg += "訊息已送至簡訊中心";
            //                break;
            //            case 20:
            //                CanRun = true;
            //                ReturnMsg += "預約簡訊，等待傳送中。";
            //                break;
            //            case 21:
            //                ReturnMsg += "預約簡訊，已取消傳送。";
            //                break;
            //            case 22:
            //                ReturnMsg += "簡訊內容不合規定(疑似詐財簡訊)";
            //                break;
            //            case 23:
            //                SendRejected = true;
            //                ReturnMsg += "受訊客戶要求拒收加值簡訊，請不要重送";
            //                getsql = string.Format(@" UPDATE tbApplicantOnline SET  [SendRejected] = 'true' WHERE OnlineID='{0}' "
            //            , OnlineID);
            //                string err = DB.SetData(getsql, "PlantTree");
            //                if (!err.Equals(""))
            //                    ReturnMsg += "\n 資料更新錯誤：" + err;
            //                break;
            //            default:
            //                string Ret_Content = hn.Get_Message();
            //                ReturnMsg += "申請人未收到簡訊!";
            //                ReturnMsg += "\n";
            //                ReturnMsg += "原因:";
            //                ReturnMsg += Ret_Content;
            //                ReturnMsg += ",ret_code:";
            //                ReturnMsg += ret;
            //                break;
            //        }

            //        gr.Result = ReturnMsg;
            //    }


            //    getsql = string.Format(@" select tsm.* from tbSendResult tsr left join tbSendMessageID tsm on tsr.SendCreatNum=tsm.SendCreatNum where MessageID='{0}' and SendResult='true'", MessageID);
            //    d = DB.getData(getsql, "PlantTree");

            //    List<Dictionary<string, string>> ListData = new List<Dictionary<string, string>>();
            //    ListData = DB.DataTableToList(d);

            //    if (ListData.Count <= 0)
            //    {
            //        gr.Result = "查無資料！";
            //        return gr;
            //    }
            //    string CreatNumber = ListData[0]["CreatNumber"];
            //    string SendCreatNum = ListData[0]["SendCreatNum"];

            //    if (!CanRun | ret == 0)
            //    {
            //        //更新處理情形
            //        getsql = string.Format(@" UPDATE tbSendResult SET  [Receive] = '{0}',[ReceiveMessage] ='{1}' WHERE SendCreatNum='{2}' "
            //            , CanRun, ReturnMsg, SendCreatNum);

            //        string err = DB.SetData(getsql, "PlantTree");
            //        if (!CanRun && err.Equals(""))                                    //已確定是不能傳送的簡訊
            //        {
            //            if (!SendRejected)
            //            getsql = string.Format(@" UPDATE tbSendLetter SET SendStatus = '{0}' WHERE CreatNumber='{1}' ", "false", CreatNumber);
            //            err = DB.SetData(getsql, "PlantTree");

            //        }

            //        if (!err.Equals(""))
            //            ReturnMsg += "錯誤：" + err;
            //    }
            //    else
            //    {
            //        getsql = string.Format(@" UPDATE tbSendResult SET  [ReceiveMessage] ='{0}' WHERE SendCreatNum='{1}' "
            //            , ReturnMsg, SendCreatNum);

            //        string err = DB.SetData(getsql, "PlantTree");
            //        if (!err.Equals(""))
            //            ReturnMsg += "錯誤：" + err;
            //    }

            //}
            //else
            //{
            //    switch (Get)
            //    {
            //        case 1:
            //            ReturnMsg = "密碼錯誤";
            //            break;
            //        case 2:
            //            ReturnMsg = "帳號不存在";
            //            break;
            //        case 3:
            //            ReturnMsg = "超過准許最大連線數";
            //            break;
            //        case -1:
            //            ReturnMsg = "Socket 連結失敗";
            //            break;
            //        case -2:
            //            ReturnMsg = "查server dns -> ip 錯誤";
            //            break;
            //        case -3:
            //            ReturnMsg = "Socket初始化錯誤";
            //            break;
            //        case -4:
            //            ReturnMsg = "Socket 傳送/接收資料錯誤";
            //            break;
            //        default:
            //            ReturnMsg = "其他：" + Get;
            //            break;
            //    }
            //    gr.Error = ReturnMsg;
            //    return gr;
            //}
            return gr;


        }


    }
}
