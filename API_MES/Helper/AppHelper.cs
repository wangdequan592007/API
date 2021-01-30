using EF_ORACLE;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace API_MES.Helper
{
    public static class AppHelper
    { 
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        public static string GetSnByImei(string SN)
        {
            string sql = $"SELECT SN FROM ODM_LINKLISTOFPHYSICSNO WHERE PHYSICSNO='{SN}'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable==null)
            {
                return null;
            }
            if (dataTable.Rows.Count>0)
            {
                return dataTable.Rows[0]["SN"].ToString();
            }
            return null;
        }
        public static string GetSubidByMo(string MO1,string STation1)
        {
            string sql = $"SELECT SERIAL_NUMBER FROM MTL_SUB_ATTEMPER T WHERE T.ATTEMPTER_CODE='{MO1}' AND T.TESTPOSITION='{STation1}'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["SERIAL_NUMBER"].ToString();
            }
            return null;
        }
        public static bool ISDBG(string MO)
        {
            string sql = $"SELECT EMS_CODE FROM WORK_WORKJOB T WHERE T.EMS_CODE='DBG' AND WORKJOB_CODE='{MO}'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable == null)
            {
                return false;
            }
            if (dataTable.Rows.Count > 0)
            {
                return  true;
            }
            return false;
        }
        public static bool ISCK_SNL(string SN)
        {
            string sql = $"SELECT SN FROM FQA_CK_SNLOG T WHERE T.SN='{SN}' AND ISOK=0";
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable == null)
            {
                return false;
            }
            if (dataTable.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        public static DataTable GetRETableSchema(List<string> ListD, int TTT)
        {

            System.Data.DataTable dt = new System.Data.DataTable();
            List<string> ListDate = new List<string>();
            List<string> ListDate1 = new List<string>();
            string strselects = "";
            if (0 <= TTT && TTT <= 3)
            {
                strselects = @"SELECT   * FROM(
           WITH T AS
                 (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
            SELECT 
            (
                 TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 00:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 00:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
            ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 00:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
            FROM T   ) WHERE  STARTDATE2<='04:00' ";
            }
            if (4 <= TTT && TTT <= 7)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 04:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 04:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 04:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   ) WHERE  STARTDATE2<='08:00' ";
            }
            if (8 <= TTT && TTT <= 11)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 08:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 08:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 08:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   ) WHERE  STARTDATE2<='12:00' ";
            }
            if (12 <= TTT && TTT <= 15)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 12:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 12:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 12:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   ) WHERE  STARTDATE2<='16:00' ";
            }
            if (16 <= TTT && TTT <= 19)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=25)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 16:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 16:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 16:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   )";
            }
            if (20 <= TTT && TTT <= 23)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=25)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 20:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 20:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 20:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   )  ";
            }
            System.Data.DataTable dt1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselects);
            if (dt1.Rows.Count > 0)
            {
                //统计工序列表
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    ListDate.Add(dt1.Rows[i][1].ToString());
                    ListDate1.Add(dt1.Rows[i][0].ToString());
                }
            }
            //if (ListD.Count > 0)
            //{
            //    for (int i = 0; i < ListD.Count; i++)
            //    {
            //        var date1 = ListD[i];
            //        System.DateTime dateTime1 = Convert.ToDateTime(date1);
            //        string date = dateTime1.ToString("HH:mm");
            //        ListDate.Remove(date);
            //    } 
            //}
            //List = ListDate1;
            //线程ID
            DataColumn dc = new DataColumn
            {
                ColumnName = "工序名称"
            };
            dt.Columns.Add(dc);
            for (int i = 0; i < ListDate.Count; i++)
            {
                dc = new DataColumn
                {
                    ColumnName = ListDate[i],
                    DataType = Type.GetType("System.String")
                };
                dt.Columns.Add(dc);
            }
            dc = new DataColumn
            {
                ColumnName = "  ",
                DataType = Type.GetType("System.String")
            };
            dt.Columns.Add(dc);
            dc = new DataColumn
            {
                ColumnName = "不良数合计",
                DataType = Type.GetType("System.String")
            };
            dt.Columns.Add(dc);
            dc = new DataColumn
            {
                ColumnName = "FTY",
                DataType = Type.GetType("System.String")
            };
            dt.Columns.Add(dc);
            return dt;
        }
        public static DataTable GetTableSchema(List<string> ListD, int TTT)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            List<string> ListDate = new List<string>();
            List<string> ListDate1 = new List<string>();
            string strselects = "";
            if (0 <= TTT && TTT <= 3)
            {
                strselects = @"SELECT   * FROM(
           WITH T AS
                 (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
            SELECT 
            (
                 TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 00:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 00:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
            ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 00:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
            FROM T   ) WHERE  STARTDATE2<='04:00' ";
            }
            if (4 <= TTT && TTT <= 7)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 04:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 04:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 04:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   ) WHERE  STARTDATE2<='08:00' ";
            }
            if (8 <= TTT && TTT <= 11)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 08:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 08:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 08:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   ) WHERE  STARTDATE2<='12:00' ";
            }
            if (12 <= TTT && TTT <= 15)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 12:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 12:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 12:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   ) WHERE  STARTDATE2<='16:00' ";
            }
            if (16 <= TTT && TTT <= 19)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=25)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 16:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 16:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 16:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   )";
            }
            if (20 <= TTT && TTT <= 23)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=25)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 20:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 20:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 20:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   )  ";
            }
            System.Data.DataTable dt1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselects);
            if (dt1.Rows.Count > 0)
            {
                //统计工序列表
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    ListDate.Add(dt1.Rows[i][1].ToString());
                    ListDate1.Add(dt1.Rows[i][0].ToString());
                }
            }
            //if (ListD.Count > 0)
            //{
            //    for (int i = 0; i < ListD.Count; i++)
            //    {
            //        string date1 = ListD[i];
            //        DateTime dateTime1 = Convert.ToDateTime(date1);
            //        string date = dateTime1.ToString("HH:mm");
            //        ListDate.Remove(date);
            //    }

            //}
            //List = ListDate1;
            //线程ID
            DataColumn dc = new DataColumn
            {
                ColumnName = "工序名称"
            };
            dt.Columns.Add(dc);
            for (int i = 0; i < ListDate.Count; i++)
            {
                dc = new DataColumn
                {
                    ColumnName = ListDate[i],
                    DataType = Type.GetType("System.String")
                };
                dt.Columns.Add(dc);
            }
            dc = new DataColumn
            {
                ColumnName = "红灯次数",
                DataType = Type.GetType("System.String")
            };
            dt.Columns.Add(dc);
            dc = new DataColumn
            {
                ColumnName = "黄灯次数",
                DataType = Type.GetType("System.String")
            };
            dt.Columns.Add(dc);
            dc = new DataColumn
            {
                ColumnName = "节拍达成率",
                DataType = Type.GetType("System.String")
            };
            dt.Columns.Add(dc);
            return dt;
        }

        public static int Get_ODM_STATION_LINENUM(string strline, string strstation)
        {
            string strselec = " SELECT ITNUM FROM ODM_STATION_LINENUM WHERE LINE = '" + strline + "' AND STATION = '" + strstation + "'";
            DataTable dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselec);
            if (dt.Rows.Count > 0)
            {
                string number = dt.Rows[0][0].ToString();
                if (number == "" || number == "0")
                {
                    return 1;
                }
                else
                {
                    return Convert.ToInt32(number);
                }
            }
            return 1;
        }
        public static string DataTableToJson(DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc]);
                }
                list.Add(result);
            }
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            //serialize.MaxJsonLength = Int32.MaxValue;
            //serialize.RecursionLimit = recursionLimit;
            return serialize.Serialize(list); //亮点是可以解决特殊字符问题
        }
        public static string REVL1(int CNaC, int Rs,double minNb,double maxNb,double MtpNb,string val4)
        { 
            if (!string.IsNullOrWhiteSpace(val4))
            {
                if (CNaC == Rs)
                {
                    val4 += "-0";
                }
                else
                {
                    double Nb1 = Convert.ToDouble(val4);
                    if (minNb <= Nb1 && Nb1 <= maxNb)
                    {
                        val4 += "-1";
                    }
                    if (minNb > Nb1)
                    {
                        val4 += "-2";
                    }
                    if (maxNb <= Nb1 && Nb1 < MtpNb)
                    {
                        val4 += "-3";
                    }
                    if (Nb1 > MtpNb)
                    {
                        val4 += "-4";
                    }
                }
                return val4;
            }
            return "";
        }

        public static DataTable DT_ODM_DEFECTRECORD_EXT(string date, string lINE, string testation)
        {
            string sql = string.Format(@"SELECT COUNT(TMP.BARCODE) TOTALNUM,TMP.TESTPOSITION,TMP.NEWTIME
                                FROM (SELECT  A.BARCODE,A.TESTPOSITION, -- ID,状态
                                         TO_CHAR(A.SCAN_TIME,'YYY-MM-DD HH24:MI:SS') OLDTIME,  (CASE  FLOOR((TO_CHAR(SCAN_TIME,'MI'))/10) 
                                       WHEN  0 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':00:00'
                                       WHEN  1 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':10:00'
                                       WHEN  2 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':20:00'
                                       WHEN  3 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':30:00'
                                       WHEN  4 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':40:00'
                                       WHEN  5 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':50:00'
                                 END)   AS NEWTIME ,RN
                                 FROM(
                         SELECT PROCESSID TESTPOSITION, LINECODE LINE_CODE2,SN BARCODE,SYS_CRT_TIME SCAN_TIME, ROW_NUMBER() OVER(PARTITION BY SN || PROCESSID ORDER BY SN || PROCESSID) RN 
                          FROM ODM_DEFECTRECORD_EXT A WHERE A.PROCESSID='{2}' AND A.LINECODE='{1}'
                        AND TO_CHAR(A.SYS_CRT_TIME,'YYYY-MM-DD')='{0}')A WHERE RN=1
                        ) TMP   GROUP BY TMP.NEWTIME,TMP.TESTPOSITION  ORDER BY TMP.NEWTIME", date, lINE, testation);
            DataTable dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            return dt;
        }

        public static DataTable DT_ODM_SPI_AOI_LOG(string date, string lINE, string testation)
        {
            string sql = string.Format(@"SELECT COUNT(TMP.TESTPOSITION) TOTALNUM,TMP.TESTPOSITION,TMP.NEWTIME
                                FROM (SELECT  A.BARCODE,A.TESTPOSITION, -- ID,状态
                                         TO_CHAR(A.SCAN_TIME,'YYY-MM-DD HH24:MI:SS') OLDTIME,  (CASE  FLOOR((TO_CHAR(SCAN_TIME,'MI'))/10) 
                                       WHEN  0 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':00:00'
                                       WHEN  1 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':10:00'
                                       WHEN  2 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':20:00'
                                       WHEN  3 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':30:00'
                                       WHEN  4 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':40:00'
                                       WHEN  5 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':50:00'
                                 END)   AS NEWTIME ,RN
                                 FROM(
                         SELECT STATION TESTPOSITION, LINECODE LINE_CODE2,SN BARCODE,BEGINDATE SCAN_TIME, ROW_NUMBER() OVER(PARTITION BY SN || STATION ORDER BY SN || STATION) RN 
                          FROM ODM_SPI_AOI_LOG A WHERE UPPER(A.RESULT3) IN('NG','FAULT','FAIL') AND A.STATION='{2}'  AND A.LINECODE='{1}'
                        AND TO_CHAR(A.BEGINDATE,'YYYY-MM-DD')='{0}')A  
                        ) TMP   GROUP BY TMP.NEWTIME,TMP.TESTPOSITION  ORDER BY TMP.NEWTIME", date, lINE, testation);
            DataTable dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            return dt;
        }

        public static bool IsODM_DEFECTRECORD_EXT(string date, string lINE, string testation)
        {
            string sql = string.Format(@"SELECT PROCESSID FROM ODM_DEFECTRECORD_EXT A WHERE A.PROCESSID='{2}' AND A.LINECODE='{1}'
                            AND TO_CHAR(A.SYS_CRT_TIME,'YYYY-MM-DD')= '{0}' AND ROWNUM = 1", date, lINE, testation);
            DataTable dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            } 
        }

        public static bool IsODM_SPI_AOI_LOG(string date, string lINE, string testation)
        {
            string sql = string.Format(@"SELECT A.STATION FROM  ODM_SPI_AOI_LOG A WHERE UPPER(A.RESULT3) IN('NG','FAULT','FAIL') AND A.STATION='{2}'   AND A.LINECODE='{1}'
                            AND TO_CHAR(A.BEGINDATE,'YYYY-MM-DD')= '{0}' AND ROWNUM = 1", date, lINE, testation);
            DataTable dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        } 
        public static string REVL2(int CNaC, int Rs, string v)
        {
            if (!string.IsNullOrWhiteSpace(v)) {
                if (CNaC == Rs)
                {
                    v += "-0";
                }
                else
                {
                    double Nb1 = Convert.ToDouble(v);
                    if (Nb1==0)
                    {
                        v += "-1";
                    }
                    else
                    {
                        v += "-2";
                    }
                }
                return v;
            }
            return "";
        }

        internal static DataTable GetRETableDt(List<string> ListD, int TTT)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            List<string> ListDate = new List<string>();
            List<string> ListDate1 = new List<string>();
            string strselects = "";
            if (0 <= TTT && TTT <= 3)
            {
                strselects = @"SELECT   * FROM(
           WITH T AS
                 (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
            SELECT 
            (
                 TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 00:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 00:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
            ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 00:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
            FROM T   ) WHERE  STARTDATE2<='04:00' ";
            }
            if (4 <= TTT && TTT <= 7)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 04:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 04:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 04:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   ) WHERE  STARTDATE2<='08:00' ";
            }
            if (8 <= TTT && TTT <= 11)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 08:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 08:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 08:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   ) WHERE  STARTDATE2<='12:00' ";
            }
            if (12 <= TTT && TTT <= 15)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 12:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 12:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 12:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   ) WHERE  STARTDATE2<='16:00' ";
            }
            if (16 <= TTT && TTT <= 19)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=25)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 16:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 16:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 16:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   )";
            }
            if (20 <= TTT && TTT <= 23)
            {
                strselects = @"SELECT   * FROM(
                   WITH T AS
                         (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=25)
                    SELECT 
                    (
                         TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 20:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM-1)/24/6,'HH24:MI')||'~'|| TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 20:00'),'YYYY-MM-DD HH24:MI') + (ROWNUM)/24/6,'HH24:MI')
                    ) STARTDATE,( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'yyyy-MM-dd'),' 20:00'),'yyyy-MM-dd hh24:mi') + (rownum-1)/24/6,'hh24:mi'))STARTDATE2
                    FROM T   )  ";
            }

            DataTable dt1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselects);
            if (dt1.Rows.Count > 0)
            {
                //统计工序列表
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    ListDate.Add(dt1.Rows[i][1].ToString());
                    ListDate1.Add(dt1.Rows[i][0].ToString());
                }
            }
            //if (ListD.Count > 0)
            //{
            //    for (int i = 0; i < ListD.Count; i++)
            //    {
            //        var date1 = ListD[i];
            //        System.DateTime dateTime1 = Convert.ToDateTime(date1);
            //        string date = dateTime1.ToString("HH:mm");
            //        ListDate.Remove(date);
            //    }

            //}
            //List = ListDate1;
            DataColumn dc = null;
            //线程ID
            dc = new DataColumn
            {
                ColumnName = "工序名称"
            };
            dt.Columns.Add(dc);
            for (int i = 0; i < ListDate.Count; i++)
            {
                dc = new DataColumn
                {
                    ColumnName = ListDate[i],
                    DataType = Type.GetType("System.String")
                };
                dt.Columns.Add(dc);
            }
            dc = new DataColumn
            {
                ColumnName = "  ",
                DataType = Type.GetType("System.String")
            };
            dt.Columns.Add(dc);
            dc = new DataColumn
            {
                ColumnName = "DT损失数合计",
                DataType = Type.GetType("System.String")
            };
            dt.Columns.Add(dc);
            dc = new DataColumn
            {
                ColumnName = "DT%",
                DataType = Type.GetType("System.String")
            };
            dt.Columns.Add(dc);
            return dt;
        }

        internal static string REVL3(int cNaC, int v1, string v2, string v3)
        {
            if (!string.IsNullOrWhiteSpace(v2))
            {
                if (cNaC == v1)
                {
                    v2 += "-0";
                }
                else
                {
                    double Nb1 = Convert.ToDouble(v3);
                    if (Nb1 == 1)
                    {
                        v2 += "-2";
                    }
                    else if (Nb1 == 2)
                    {
                        v2 += "-3";
                    }
                }
                return v2;
            }
            return "";
        }
        public static void dBtnInsert(DataTable exid, string TableName,int totalRow)
        {
            if (exid.Rows.Count > 0)
            {
                Stopwatch sw = new Stopwatch();
                using (SqlConnection conn = new SqlConnection(UserInfo.SqlConnectionString168))
                {
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(conn)
                    {
                        DestinationTableName = TableName,
                        BatchSize = totalRow
                    };
                    conn.Open();
                    sw.Start();
                    bulkCopy.WriteToServer(exid);
                    //Infor("\n" + DateTime.Now.ToString() + string.Format("\t插入{0}条记录共花费{1}毫秒", totalRow, sw.ElapsedMilliseconds));
                    //LAPProunter.WriteMesSql("\n" + DateTime.Now.ToString() + string.Format("\t插入{0}条记录共花费{1}毫秒", totalRow, sw.ElapsedMilliseconds));
                }
                //exid = null;
            }
            else
            {
                //Infor("\r" + DateTime.Now.ToString() + "\t" + "无数据插入！！！");
                //LAPProunter.WriteMesSql("\r" + DateTime.Now.ToString() + "\t" + "无数据插入！！！");
            }
        }

        public static DataTable GetTableSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("ID",typeof(int)),
                new DataColumn("IF_SEQ",typeof(string)),
                new DataColumn("MOVE_FLAG",typeof(string)),
                new DataColumn("MOVE_TIME",typeof(string)),
                new DataColumn("FACTORY",typeof(string)),
                new DataColumn("Center_Order_Id",typeof(string)),
                new DataColumn("Ems_Order_Id",typeof(string)),
                new DataColumn("Oper",typeof(string)),
                new DataColumn("FROM_TIME",typeof(string)),
                new DataColumn("TO_TIME",typeof(string)),
                new DataColumn("Station_ID",typeof(string)),
                new DataColumn("Plan_Line",typeof(string)),
                new DataColumn("Mfg_Line",typeof(string)),
                new DataColumn("FIRST_IN_Qty",typeof(string)),
                new DataColumn("IN_Qty",typeof(string)),
                new DataColumn("GOOD_QTY",typeof(string)),
                new DataColumn("SCRAP_QTY",typeof(string)),
                new DataColumn("REPAIR_QTY",typeof(string)),
                new DataColumn("REPAIR_GOOD_QTY",typeof(string)),
                new DataColumn("REPAIR_WORK_QTY",typeof(string)),
                new DataColumn("REWORK_QTY",typeof(string)),
                new DataColumn("REWORK_GOOD_QTY",typeof(string)),
                new DataColumn("REWORK_WORK_QTY",typeof(string)),
                new DataColumn("DEFECT_QTY",typeof(string)),
                new DataColumn("FIRST_DEFECT_QTY",typeof(string)),
                new DataColumn("FIRST_GOOD_QTY",typeof(string)),
                new DataColumn("TEST_FAULT_QTY",typeof(string)),
                new DataColumn("MAT_MODEL",typeof(string)),
                new DataColumn("EMS_MAT_ID",typeof(string)),
                new DataColumn("HW_MAT_ID",typeof(string)),
                new DataColumn("CREATE_USER_ID",typeof(string)),
                new DataColumn("CREATE_TIME",typeof(string)),
                new DataColumn("UPDATE_USER_ID",typeof(string)),
                new DataColumn("UPDATE_TIME",typeof(string)),
                new DataColumn("TEST_FAULT_SN_QTY",typeof(string)),
                new DataColumn("Creation_Time",typeof(string)),
                new DataColumn("GETFLAG",typeof(string)),
                new DataColumn("GETTIME",typeof(string)),
                new DataColumn("ACTIONFLAG",typeof(string)),
                new DataColumn("S_FACTORY",typeof(string)),
                new DataColumn("SEGMENT2",typeof(string)),
                new DataColumn("SEGMENT3",typeof(string)),
                new DataColumn("SEGMENT4",typeof(string)),
                new DataColumn("SEGMENT5",typeof(string))
            });
            return dt;
        }
    }
}
