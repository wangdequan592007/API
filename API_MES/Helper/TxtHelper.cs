using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_MES.Helper
{
    public class TxtHelper
    {
        private FileStream DataTableToTxt(DataTable table, string file, string strTitle)
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
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (var x = 0; x < 38; x++)//38列数据
                {
                    if (table.Rows[i][x].ToString().Trim() == "")//为空时
                    {
                        sw.Write("\t");//写空Tab
                    }
                    else if (x == 37)
                    {
                        sw.Write(table.Rows[i][x].ToString().Trim());//写TXT
                    }
                    else
                    {
                        sw.Write(table.Rows[i][x].ToString().Trim() + "\t");//写TXT
                    }
                }
                if (i < table.Rows.Count - 1)
                {
                    sw.Write("\r\n");//换行
                }

            }
            sw.Close();
            fs.Close();
            return fs;
        }


    }
}
