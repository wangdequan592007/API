using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Helper
{
    public class UserInfo
    {
        public static string OracleConnectionStringIms = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.12.180)(PORT=1521))) "
                       + " (CONNECT_DATA=(SERVICE_NAME=orcl)));User Id=ims;Password=imsorcl123";
        public static string SqlConnectionStringbyd = "server=192.168.12.168;database=BYD_HWOUTPUT; uid=cpms;pwd=cpms";
        public static string SqlConnectionStringck = "server=192.168.12.168;database=CK_HWOUTPUT; uid=cpms;pwd=cpms";
        public static string SqlConnectionStringDBG = "server=192.168.12.168;database=DBG_HWOUTPUT; uid=cpms;pwd=cpms";
        public static string SqlConnectionString40 = "server=192.168.12.40;database=jiepai; uid=sa;pwd=wtffqu";
        public static string SqlConnectionString45 = "server=192.168.12.40;database=jiepai; uid=administrator;pwd=gdenok-321";
        public static string SqlConnectionString158 = "server=172.17.5.234;database=WEBDBA; uid=sa;pwd=chinoe-123";
        public static string SqlConnectionString168 = "server=192.168.12.168;database=HWOUTPUT; uid=cpms;pwd=cpms";
        public static string OracleConnectionStringT100 = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.12.73)(PORT=1521))) "
               + " (CONNECT_DATA=(SERVICE_NAME=topprd)));User Id=dsdata;Password=dsdata";
    }
}
