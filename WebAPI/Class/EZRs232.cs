using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace WebAPI.Class
{
    public class EZRs232
    {
        private const int ACK = 6;
        public int ackTimeout;
        public short cntOfRetrySendMsg;
        private const int ETX = 3;
        private const int NAK = 0x15;
        public string recvMsg;
        public string sendMsg;
        public SerialPort serialPort = new SerialPort();
        private const int STX = 2;

        private static string ConvertByteArrayToString(byte[] ByteOutput)
        {
            return Encoding.UTF8.GetString(ByteOutput);
        }

        public static byte[] ConvertStringToByte(string Input)
        {
            return Encoding.UTF8.GetBytes(Input);
        }

        public void run()
        {
            if (this.serialPort.IsOpen)
            {
                //MessageBox.Show("COM Port已開啟");
                recvMsg = "COM Port已開啟";
            }
            else
            {
                try
                {
                    //bool flag;
                    int num3;
                    //bool flag2;
                    short num5;
                    serialPort.Open();
                    sendMsg = char.ConvertFromUtf32(2) + sendMsg + char.ConvertFromUtf32(3);
                    int readTimeout = serialPort.ReadTimeout;
                    serialPort.ReadTimeout = ackTimeout;
                    goto Label_0146;
                Label_0073:
                    //flag = false;
                    serialPort.Write(sendMsg); //傳送資料給EDC
                    int num = serialPort.ReadByte();
                    if (num != 6)
                    {
                        //MessageBox.Show("format error, must be ACK");//格式錯誤
                        recvMsg = "輸入格式錯誤";
                        goto Label_0166;
                    }
                    if ((num == 0x15) || (num != 6))
                    {
                        goto Label_0146;
                    }
                    serialPort.ReadTimeout = readTimeout;
                    goto Label_013F;
                Label_00DF:
                    if ((num3 = serialPort.BytesToRead) > 0)
                    {
                        byte[] buffer = new byte[num3];
                        serialPort.Read(buffer, 0, num3);
                        recvMsg = recvMsg + ConvertByteArrayToString(buffer);
                        cntOfRetrySendMsg = 0;
                        goto Label_0146;
                    }
                    Thread.Sleep(0x7d0);
                Label_013F:
                    //flag2 = true;
                    goto Label_00DF;
                Label_0146:
                    cntOfRetrySendMsg = (short)((num5 = cntOfRetrySendMsg) - 1);
                    if (num5 > 0)
                    {
                        goto Label_0073;
                    }
                Label_0166:
                    serialPort.Write(char.ConvertFromUtf32(6));//傳送資料給EDC
                    serialPort.Close();
                }
                catch (TimeoutException exception)
                {
                    serialPort.Close();
                    recvMsg = recvMsg + exception.Message;
                }
                catch (IOException exception2)
                {
                    recvMsg = recvMsg + exception2.Message;
                }
            }
        }
    }
}