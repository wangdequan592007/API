using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace API_COM.Helper
{
    public class TxtHelper
    {
        public static FileStream DataTableToTxt(DataTable table, string file, string strTitle)
        {

            string[] title = strTitle.Split(',');

            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            //StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            StreamWriter sw = new StreamWriter(new BufferedStream(fs), Encoding.UTF8);
            //StreamWriter sw = new StreamWriter(new BufferedStream(fs), System.Text.Encoding.Default);
            for (int i = 0; i < title.Count(); i++)
            {
                if (i == title.Count() - 1)
                {
                    sw.Write(title[i]);//写TXT
                }
                else
                {
                    sw.Write(title[i] + "\t");//写TXT
                }

            }
            sw.Write("\r\n");
            //for (int i = 0; i < table.Rows.Count; i++)
            //{
            //    for (var x = 0; x < 38; x++)//38列数据
            //    {
            //        if (table.Rows[i][x].ToString().Trim() == "")//为空时
            //        {
            //            sw.Write("\t");//写空Tab
            //        }
            //        else if (x == 37)
            //        {
            //            sw.Write(table.Rows[i][x].ToString().Trim());//写TXT
            //        }
            //        else
            //        {
            //            sw.Write(table.Rows[i][x].ToString().Trim() + "\t");//写TXT
            //        }
            //    }
            //    if (i < table.Rows.Count - 1)
            //    {
            //        sw.Write("\r\n");//换行
            //    }

            //}
            sw.Close();
            fs.Close();
            return fs;
        }
        public void ExportTxt(HttpContext context)
        { 
            ////HttpContextBase context1 = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            ////HttpRequestBase request = context.Request;//定义传统request对象    
            //string str = "temp";
            //context.Response.Clear();
            //context.Response.Buffer = false;
            //context.Response.ContentEncoding = Encoding.UTF8;
            //context.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(str) + ".txt");
            //context.Response.ContentType = "text/plain";
            //string s = "HELLO WORK";
            //context.Response.Write(s);
            //context.Response.End();
        }
        public void Write(string path, string tile)
        {
            //byte[] buffer = Encoding.GetEncoding("GB2312").GetBytes(tile);
            //string myscript = Encoding.UTF8.GetString(buffer); 
            string[] dtr = tile.Split('\n');
            FileStream fs = new FileStream(path, FileMode.Create);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("GB2312"));//通过
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            StringBuilder sb3 = new StringBuilder();
            for (int i = 0; i < dtr.Length; i++)
            {
                sb3.Append(dtr[i] + "\r\n");
            }
            //开始写入
            sw.Write(sb3);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

    }
}
