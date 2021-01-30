using API_MES.Entities;
using API_MES.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Servise
{
    public class OtherRepository : OtherInterface
    {
        public async Task<ErrMessage> DEL_LENOVOSFTP()
        {
            ErrMessage rETURN = new ErrMessage();
            await Task.Run(() =>
            {
                SFTPHelper sftp = new SFTPHelper("bjmft.lenovo.com", "22", "Ex_Ontim_P", "Ex_Ontim_P");
                string remotePath = "/856_Appendix/out/tmp";
                bool bl = true;
                bool bl1 = true;
                Dictionary<string, string> dic = sftp.GetFileListFileName(remotePath, ".txt", ref bl);
                if (bl)
                {
                    dic = dic.OrderBy(r => r.Value).ToDictionary(r => r.Key, r => r.Value);
                    foreach (KeyValuePair<string, string> kvp in dic)
                    {
                        string extenname = kvp.Key;
                        sftp.Delete(remotePath + "/" + extenname);
                    }
                }
                Dictionary<string, string> dic1 = sftp.GetFileListFileName(remotePath, ".trigger", ref bl1);
                if (bl1)
                {
                    dic1 = dic1.OrderBy(r => r.Value).ToDictionary(r => r.Key, r => r.Value);
                    foreach (KeyValuePair<string, string> kvp in dic1)
                    {
                        string extenname = kvp.Key;
                        sftp.Delete(remotePath + "/" + extenname);
                    }
                }
            });
            rETURN.Err = "执行完成";
            rETURN.success = true; 
            return rETURN;
        }
    }
}
