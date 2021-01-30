using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_COM.Helper
{
    public class UserHelp
    {
        public static string OracleConnection { get; set; }
        public static string OracleDbaT100 { get; set; }

        public static string OracleConnectionString101 = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.12.101)(PORT=1521))) "
                       + " (CONNECT_DATA=(SERVICE_NAME=rs)));User Id=dmsnew;Password=chpass";

    }
}
