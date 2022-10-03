using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Web;

namespace WebAPI.Class
{
    /// <summary>
    /// 出單機列印測試程式
    /// </summary>
    public class Printer
    {
        string title = "";
        string body = "";
        string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myTitle"></param>
        /// <param name="myBody"></param>
        public void print(string myTitle,string myBody)
        {
            string print = System.Configuration.ConfigurationManager.AppSettings["print"];

            body = myBody;
            title = myTitle;

            PrintDocument PD = new PrintDocument();

            PrinterSettings ps = new PrinterSettings();

            //預設size
            PaperSize psize = new PaperSize("Custom", 100, 200);

            PD.DefaultPageSettings.PaperSize = psize;

            PD.PrintPage += new PrintPageEventHandler(pdoc_PrintPage);

            //自訂紙張長度
            PD.DefaultPageSettings.PaperSize.Height = 300;

            //自訂紙張寬度
            PD.DefaultPageSettings.PaperSize.Width = 285;

            //印表機名稱
            PD.PrinterSettings.PrinterName = print;

            //預覽列印
            //PrintPreviewDialog PPD = new PrintPreviewDialog();
            //PPD.Document = PD;
            //PPD.ShowDialog();

            //直接列印
            PD.Print();
        }
        void pdoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            string font_type = "標楷體";
            int Offset = 27;

            Graphics graphics = e.Graphics;

            int startX = 0;//橫寬

            graphics.DrawString(title, new Font(font_type, 20),
                                new SolidBrush(Color.Black), 100, 0);

            //Line
            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));
            e.Graphics.DrawLine(pen, 300, Offset, startX, Offset);
            Offset = Offset + 3;

            //table number
            graphics.DrawString("T12",
                     new Font(font_type, 10),
                     new SolidBrush(Color.Black), 100, Offset);

            graphics.DrawString("N0.123",
                     new Font(font_type, 10),
                     new SolidBrush(Color.Black), 150, Offset);
            Offset = Offset + 18;

            //Line
            pen = new Pen(Color.FromArgb(255, 0, 0, 0));
            e.Graphics.DrawLine(pen, 300, Offset, startX, Offset);
            Offset = Offset + 5;

            //
            //listview
            //
            string line = null;
            //int count = 0,
            int price = 0;//表示第一行 
            string[] list_pro = body.Split('|').ToArray();


            foreach (string pp in list_pro)
            {
                string prod = pp.Split(',')[0];
                string q = pp.Split(',')[2];
                string p = pp.Split(',')[1];

                //priduct
                line = prod;
                graphics.DrawString(line, new Font(font_type, 10), new SolidBrush(Color.Black), startX, Offset);

                //Quantity
                line = q;
                graphics.DrawString(line, new Font(font_type, 10), new SolidBrush(Color.Black), 180, Offset);

                //Price
                line = p;

                //price = int.Parse(list_pro.Items[i].SubItems[2].Text) * int.Parse(list_pro.Items[i].SubItems[1].Text);
                price = int.Parse(p) * int.Parse(q);
                graphics.DrawString("$" + Convert.ToString(price), new Font(font_type, 10), new SolidBrush(Color.Black), 230, Offset);
                Offset = Offset + 15;

            }

            //for (int i = 0; i < list_pro.Count; i++)
            //{
            //    //priduct
            //    line = list_pro.Items[i].Text;
            //    graphics.DrawString(line, new Font(font_type, 10), new SolidBrush(Color.Black), startX, Offset);

            //    //Quantity
            //    line = list_pro.Items[i].SubItems[1].Text;
            //    graphics.DrawString(line, new Font(font_type, 10), new SolidBrush(Color.Black), 180, Offset);

            //    //Price
            //    line = list_pro.Items[i].SubItems[2].Text;
            //    price = int.Parse(list_pro.Items[i].SubItems[2].Text) * int.Parse(list_pro.Items[i].SubItems[1].Text);
            //    graphics.DrawString("$" + Convert.ToString(price), new Font(font_type, 10), new SolidBrush(Color.Black), 230, Offset);
            //    Offset = Offset + 15;

            //    //taste
            //    line = list_pro.Items[i].SubItems[3].Text;
            //    graphics.DrawString(line, new Font(font_type, 10), new SolidBrush(Color.Black), 25, Offset);
            //    Offset = Offset + 18;

            //    count++;
            //}
            Offset = Offset + 3;

            //Line
            pen = new Pen(Color.FromArgb(255, 0, 0, 0));
            e.Graphics.DrawLine(pen, 300, Offset, startX, Offset);

            Offset = Offset + 5;

            //合計
            graphics.DrawString("12",
                     new Font(font_type, 10),
                     new SolidBrush(Color.Black), startX, Offset);

            //總計
            graphics.DrawString("$250",
                     new Font(font_type, 10),
                     new SolidBrush(Color.Black), 200, Offset);
            Offset = Offset + 18;

            //Line
            pen = new Pen(Color.FromArgb(255, 0, 0, 0));
            e.Graphics.DrawLine(pen, 300, Offset, startX, Offset);
            Offset = Offset + 5;

            //time
            graphics.DrawString(datetime,
                    new Font(font_type, 7),
                    new SolidBrush(Color.Black), startX, Offset);

        }
    }
}