using LitJson;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net; 
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using Oracle.ManagedDataAccess.Client;

namespace API_COM.Helper
{
    public class AppHelper
    {
        public static string G_EMSCODE(string userId)
        {
            string sql = $"SELECT B.DESCRIBE FROM RS_USER1 A LEFT OUTER JOIN   SYS_DEPARTMENT B  ON A.DEPARTMENT=B.DEPART_NAME WHERE USERID='{userId}'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["DESCRIBE"].ToString();
            }
            return null;
        }
        public static string GetSubidByMo(string MO1, string STation1)
        {
            string sql = $"SELECT SERIAL_NUMBER FROM MTL_SUB_ATTEMPER T WHERE T.ATTEMPTER_CODE='{MO1}' AND T.TESTPOSITION='{STation1}'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
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
        public static bool I_BigCartoninfo(string PALLET)
        {
            string sql = $"SELECT BIGCARTONID FROM ODM_BIGCARTONINFO T WHERE T.BIGCARTONID='{PALLET}' ";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
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
        public static bool I_MidCartoninfo(string PALLET)
        {
            string sql = $"SELECT MIDCARTONID FROM ODM_MIDCARTONINFO T WHERE T.MIDCARTONID='{PALLET}' ";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
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
        public static string G_MidMo(string PALLET)
        {
            string sql = $"SELECT WORKORDER FROM ODM_MIDCARTONINFO T WHERE T.MIDCARTONID='{PALLET}' ";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["WORKORDER"].ToString();
            }
            return null;
        }
        public static string G_BigMo(string PALLET)
        {
            string sql = $"SELECT  A.WORKORDER FROM ODM_BIGCARTONPACKING T LEFT JOIN ODM_MIDCARTONINFO A ON A.MIDCARTONID=T.MIDCARTONID WHERE T.BIGCARTONID='{PALLET}' AND ROWNUM=1";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["WORKORDER"].ToString();
            }
            return null;
        }
        public static string G_BigMobyInfor(string PALLET)
        {
            string sql = $"SELECT A.WORKORDER FROM ODM_BIGCARTONINFO A WHERE A.BIGCARTONID='{PALLET}'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["WORKORDER"].ToString();
            }
            return null;
        }
        public static string G_BigNW(string PALLET)
        {
            string sql = @$"SELECT  SUM(B.WEIGHT) WEIGHT FROM ODM_BIGCARTONPACKING  A
                            LEFT JOIN  ODM_MIDCARTONINFO B ON A.MIDCARTONID = B.MIDCARTONID
                            WHERE A.BIGCARTONID = '{PALLET}'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["WEIGHT"].ToString();
            }
            return null;
        }
        public static string G_BigGW(string PALLET)
        {
            string sql = @$"SELECT T.ROUGH_WEIGHT FROM EDI_CARTONWEIGHT T WHERE T.CARTON='{PALLET}'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["ROUGH_WEIGHT"].ToString();
            }
            return null;
        }
        public static string G_BigImeiCount(string PALLET)
        {
            string sql = $"SELECT  COUNT(*) FROM ODM_BIGCARTONPACKING T LEFT JOIN ODM_PACKING A ON A.MIDCARTONID=T.MIDCARTONID WHERE T.BIGCARTONID IN ({PALLET} )";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0][0].ToString();
            }
            return null;
        }
        public static string G_MidImeiCount(string PALLET)
        {
            string sql = $"SELECT  COUNT(*) FROM ODM_PACKING T   WHERE T.MIDCARTONID IN ({PALLET}) ";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0][0].ToString();
            }
            return null;
        }
        public static string G_WMS_SSCCMODEL(string Vdn)
        {
            string sql = $"SELECT NVL(DNTYPE,99) FROM WMS_SSCCMODEL T  WHERE DN1='{Vdn}' ORDER BY T.INPUTDATE DESC ";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0][0].ToString();
            }
            return null;
        }
        public static string G_SSCCINFO(string PALLET)
        {
            string sql = $"SELECT DN1 FROM WMS_SSCCINFO T  WHERE BOXSN IN ({PALLET}) AND ROWNUM=1 AND NVL(T.WWNO,'D')!='NG' AND NVL(T.WWNO,'D')!='REWORK'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["DN1"].ToString();
            }
            return null;
        }
        public static string G_SSCCINFOLOGCTbyBig(string PALLET)
        {
            string sql = $"SELECT COUNT(*)CT FROM  WMS_SSCCSTOCKLOG T WHERE EXISTS(SELECT A.IMEI   FROM ODM_PACKING A WHERE EXISTS(SELECT MIDCARTONID FROM  ODM_BIGCARTONPACKING B WHERE B.MIDCARTONID = A.MIDCARTONID AND B.BIGCARTONID='{PALLET}')AND　T.IMEI=A.IMEI)";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["CT"].ToString();
            }
            return null;
        }
        public static string G_SSCCINFOLOGCTbyMid(string PALLET)
        {
            string sql = $"SELECT COUNT(*)CT FROM  WMS_SSCCSTOCKLOG T WHERE EXISTS(SELECT A.IMEI   FROM ODM_PACKING A WHERE  A.MIDCARTONID='{PALLET}' AND　T.IMEI=A.IMEI)";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["CT"].ToString();
            }
            return null;
        }
        public static bool I_MODEL(string mO,ref string pN,ref string cPN,ref string mESG)
        {
            string sql = $"SELECT A.ITEM_CODE PN,B.PNCODE CPN FROM WORK_WORKJOB A LEFT JOIN ODM_MODEL B ON A.ITEM_CODE=B.BOM WHERE A.WORKJOB_CODE='{mO}'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                mESG = "数据库查询失败";
                return false;
            }
            if (dataTable.Rows.Count > 0)
            {
                pN= dataTable.Rows[0]["PN"].ToString();
                cPN = dataTable.Rows[0]["CPN"].ToString();
            }
            else
            {
                mESG = mO+ "产品编码绑定机种代码信息未维护";
            }

            return true;
        }
        /// <summary>
        /// HmacSHA256算法,返回的结果始终是32位
        /// </summary>
        /// <param name="key">加密的键，可以是任何数据</param>
        /// <param name="content">待加密的内容</param>
        /// <returns></returns>
        public static byte[] HmacSHA256(byte[] key, byte[] content)
        {
            using (var hmacsha256 = new HMACSHA256(key))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(content);
                return hashmessage;
            }
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64(string source)
        {
            return Base64Encode(Encoding.UTF8, source);
        }
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode(Encoding encodeType, string source)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }
        public static string MD5(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(strText));
            return System.Text.Encoding.Default.GetString(result);
        }
        private bool GetYunmiApiInfo(string mac)
        {
            try
            {
                string appKey = "8QCWlcowc9dHO2VO";
                string appSecret = "TOZ2nkjOTM6FDkLC";
                //云米API接口
                //appKey: rK0RxXqx0dNbRiLt   
                //appSecret: JjROXkfdaWIsclGH

                //string url = "https://openapi-test.viomi.com.cn/api/factory/open-api/v1/module/getDeviceSecretInfo";


                //appKey：8QCWlcowc9dHO2VO
                //appSecret：TOZ2nkjOTM6FDkLC
                string url = "https://openapi.viomi.com.cn/api/factory/open-api/v1/module/getDeviceSecretInfo";

                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(url);
                myReq.Timeout = 1000000000;

                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                string timestamp = Convert.ToInt64(ts.TotalMilliseconds).ToString();

                string nonce = new Random().NextDouble().ToString();
                string body = Base64(MD5("{ \"mac\":\"" + mac + "\"}"));
                //body = "hlZh8B47XWGAzayqMS5fkg==";
                string StringToSign = "POST" + "\n" +
                                    "application/json" + "\n" +
                                    "application/json" + "\n" +
                                    "x-viomi-api-version:v1" + "\n" +
                                    "x-viomi-request-payload:" + body + "\n" +
                                    "x-viomi-signature-method:HmacSHA256" + "\n" +
                                    "x-viomi-signature-nonce:" + nonce + "\n" +
                                    "x-viomi-timestamp:" + timestamp + "\n" +
                                    "/api/factory/open-api/v1/module/getDeviceSecretInfo";


                UTF8Encoding utf8 = new UTF8Encoding();
                Byte[] byte_StringToSign = utf8.GetBytes(StringToSign);
                Byte[] byte_appSecret = utf8.GetBytes(appSecret);

                string signature = Convert.ToBase64String(HmacSHA256(byte_appSecret, byte_StringToSign));


                myReq.Method = "POST";
                myReq.Accept = "application/json";
                myReq.ContentType = "application/json";

                myReq.Headers.Add("X-VIOMI-API-Version", "v1");
                myReq.Headers.Add("X-VIOMI-Request-Payload", body);
                myReq.Headers.Add("X-VIOMI-Signature-Method", "HmacSHA256");
                myReq.Headers.Add("X-VIOMI-Signature-Nonce", nonce);
                myReq.Headers.Add("X-VIOMI-Timestamp", timestamp);
                //Authorization:VIOMI:appKey:Singature
                myReq.Headers.Add("Authorization", "VIOMI:" + appKey + ":" + signature);

                var postData = Encoding.ASCII.GetBytes("{ \"mac\":\"" + mac + "\"}");
                myReq.ContentLength = postData.Length;
                using (var postStream = myReq.GetRequestStream())
                {
                    postStream.Write(postData, 0, postData.Length);
                }


                HttpWebResponse httpWResp = (HttpWebResponse)myReq.GetResponse();

                Stream mystream = httpWResp.GetResponseStream();
                StreamReader sr = new StreamReader(mystream, Encoding.UTF8);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                } 
                string result = strBuilder.ToString(); 
                JsonData jd = JsonMapper.ToObject(result); 
                string code = jd["code"].ToString();
                string description = jd["desc"].ToString();
                string api_code = code;
                string api_desc = description;
                if (code == "100")
                {
                    JsonData jd2 = jd["result"];

                    string did = jd2["did"].ToString();
                    string cloud_public_key = jd2["cloudPublicKey"].ToString();
                    string device_access_key = jd2["deviceAccessKey"].ToString();
                    string bootloaderKey = jd2["bootloaderKey"].ToString();
                    string flashKey = jd2["flashKey"].ToString();

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {

                return false;
            } 
        }

        /// <summary>  
        /// 调用api返回json  
        /// </summary>  
        /// <param name="url">api地址</param>  
        /// <param name="jsonstr">接收参数</param>  
        /// <param name="type">类型</param>  
        /// <returns></returns>  
        public static string HttpApi(string url, string jsonstr, string type)
        { 
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);//webrequest请求api地址  
            request.Timeout = 1000000000;
            request.Accept = "application/json"; 
            request.ContentType = "application/json";
            request.Headers.Add("Accept-Encoding", "");
            request.Method = type.ToUpper().ToString();//get或者post  
            byte[] buffer = encoding.GetBytes(jsonstr);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        public static bool  InsertWMS_LOG(string dn1, string csn, string boxtype, string boxsn, string sscc, string crt_user,string errCode,string errName)
        {
          string  StrInsert = "INSERT INTO WMS_LOG(DN1,CSN,BOXTYPE,BOXSN,SSCC,CRT_USER,ERR_CODE,ERR_NAME)VALUES('" + dn1 + "',:CSN,'" + boxtype + "','" + boxsn + "',  '" + sscc + "','" + crt_user + "','" + errCode + "','" + errName + "')";
            OracleConnection conn = new OracleConnection(UserHelp.OracleConnection);
            try
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(StrInsert, conn))
                {
                    OracleParameter oracleParameter = new OracleParameter("CSN", OracleDbType.Clob);
                    oracleParameter.Value = csn;
                    cmd.Parameters.Add(oracleParameter);
                    cmd.ExecuteNonQuery(); 
                    conn.Close(); 
                    return true; 
                }
            }
            catch (OracleException ex)
            {
                return false;
            }
        }

        public static string GetNT_IDByMo(string mo)
        {
            string sql = $"SELECT A.NETCODE_ACCESS FROM ODM_MODEL A WHERE EXISTS(SELECT T.ITEM_CODE FROM WORK_WORKJOB T   WHERE T.ITEM_CODE= A.BOM AND T.WORKJOB_CODE= '{mo}')";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["NETCODE_ACCESS"].ToString();
            }
            return null;
        }
        public static string MES_FACTORY_ID(string id)
        {
            string sql = $"SELECT  B.DESCRIBE FROM RS_USER1 A LEFT OUTER JOIN   SYS_DEPARTMENT B  ON A.DEPARTMENT=B.DEPART_NAME WHERE USERID='{id}'";
            DataTable dt = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public static string MES_FACTORY(string code, string id)
        {
            string facId = MES_FACTORY_ID(id);
            string sql = $"SELECT T.PARENT_VAL FROM MES_FACTORY T WHERE T.PARENT_CODE='{code}' AND T.PARENT_ID='{facId}'";
            DataTable dt = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }
        public static string DELIVERY_QUANTITY(string v_po)
        {
            string sql = $" SELECT SUM(TO_NUMBER( A.V_DELIVERY_QUANTITY)) FROM WMS_ORDERS A WHERE EXISTS(SELECT B.DN1 FROM WMS_SSCCMODEL B WHERE A.V_DN=B.DN1 AND B.DNTYPE IN(1,0)) AND A.V_PO='{v_po}'";
            DataTable dt = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }
        public static string ToTalDELIVERY_QUANTITY(string v_po,string v_dn)
        {
            string sql = $" SELECT  SUM(TO_NUMBER( A.V_DELIVERY_QUANTITY)) FROM WMS_ORDERS A WHERE NOT EXISTS(SELECT B.DN1 FROM WMS_SSCCMODEL B WHERE A.V_DN=B.DN1 AND B.DNTYPE IN(1,0)) AND A.V_PO='{v_po}' AND A.V_DN<>'{v_dn}'";
            DataTable dt = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }
        public static string  SSCCMODEL(string dn1)
        {
            string sql = $"SELECT T.DNTYPE FROM WMS_SSCCMODEL T WHERE DN1='{dn1}'";
            DataTable dt = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
