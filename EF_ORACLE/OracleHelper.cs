using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Oracle.ManagedDataAccess.Client;

namespace EF_ORACLE
{
    public class OracleHelper
    {
        public static string OracleConnection { get; set; }
        public static string OracleErrMsg = string.Empty;
        /// <summary>
        /// 执行SQL查询,返回datatable
        /// </summary>
        /// <param name="strConnString">oracle连接字符串</param>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string strConnString, string strSql)
        {
            DataTable dt = null;
            OracleConnection conn = new OracleConnection(strConnString);
            try
            {
                conn.Open();
                OracleDataAdapter oda = new OracleDataAdapter(strSql, conn);
                dt = new DataTable();
                oda.Fill(dt);
            }
            catch (Exception ex)
            {
                OracleErrMsg = "查询语句错误,错误如下： \r\n " + strSql + "  " + ex.ToString();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return dt;
        }
        public static DataTable ExecuteDataTable1(string connstring, string sql)
        {
            DataTable dt = null;
            using (OracleConnection conn = new OracleConnection(connstring))
            {
                try
                {
                    OracleDataAdapter oda = new OracleDataAdapter(sql, conn);
                    conn.Open();
                    dt = new DataTable();
                    oda.Fill(dt);
                }
                catch (Exception ex)
                {
                    OracleErrMsg = "查询语句错误,错误如下： \r\n " + ex.ToString();
                    //MessageBox.Show(OracleErrMsg);
                }
            }
            return dt;
        }
        public static DataTable ExecuteDataTablesql(string connstring, string sql)
        {
            DataTable dt = null;
            try
            {
                SqlConnection conn = new SqlConnection(connstring);
                SqlDataAdapter oda = new SqlDataAdapter(sql, conn);
                dt = new DataTable();
                oda.Fill(dt);
            }
            catch (Exception ex)
            {
                OracleErrMsg = "查询语句错误,错误如下： \r\n " + ex.ToString();
            }
            return dt;
        }
        /// <summary>  
        /// 执行数据库查询操作,返回DataSet类型的结果集  
        /// </summary>  
        /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
        /// <param name="commandParameters">命令参数集合</param>  
        /// <returns>当前查询操作返回的DataSet类型的结果集</returns> 
        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, string strConnString, params OracleParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            using (OracleConnection connection = new OracleConnection(strConnString))
            {
                try
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(ds);
                    cmd.Parameters.Clear();

                }
                catch
                {
                    throw;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                    connection.Dispose();

                }
            }
            return ds;
        }
       
        /// <summary>  
        /// 执行数据库命令前的准备工作  
        /// </summary>  
        /// <param name="command">Command对象</param>  
        /// <param name="connection">数据库连接对象</param>  
        /// <param name="trans">事务对象</param>  
        /// <param name="cmdType">Command类型</param>  
        /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
        /// <param name="commandParameters">命令参数集合</param> 
        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter[] commandParameters)
        {

            //Open the connection if required
            if (conn.State != ConnectionState.Open)
                conn.Open();

            //Set up the command
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            //Bind it to the transaction if it exists
            if (trans != null)
                cmd.Transaction = trans;

            // Bind the parameters passed in
            if (commandParameters != null)
            {
                foreach (OracleParameter parm in commandParameters)
                    cmd.Parameters.Add(parm);
            }
        }
        public static DataTable ExecuteDataTable_sql(string connstring, string sql)
        {
            DataTable dt = null;
            try
            {
                SqlConnection conn = new SqlConnection(connstring);
                conn.Open();
                SqlDataAdapter oda = new SqlDataAdapter(sql, conn);
                dt = new DataTable();
                oda.Fill(dt);

            }
            catch (Exception ex1)
            {
                OracleErrMsg = "查询语句错误,错误如下： \r\n " + ex1.ToString();
            }


            return dt;
        }

        /// <summary>
        /// 带参数的查询，返回DATATABLE;
        /// </summary>
        /// <param name="connstring">连接字符串</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string connstring, string sql, params OracleParameter[] param)
        {
            DataTable dt = null;
            try
            {
                OracleConnection conn = new OracleConnection(connstring);
                conn.Open();
                OracleDataAdapter oda = new OracleDataAdapter(sql, conn);

                foreach (OracleParameter prm in param)
                {
                    oda.SelectCommand.Parameters.Add(prm);
                }
                try
                {
                    DataSet ds = new DataSet();
                    oda.Fill(ds);
                    dt = ds.Tables[0];

                }
                catch (OracleException ex)
                {
                    OracleErrMsg = "查询语句错误,错误如下： \r\n " + ex.ToString();
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex1)
            {
                OracleErrMsg = "查询语句错误,错误如下： \r\n " + ex1.ToString();
            }


            return dt;
        }






        /// <summary>
        /// 执行SQL查询,返回dataset
        /// </summary>
        /// <param name="connstring"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string connstring, string sql)
        {
            DataSet ds = null;
            try
            {
                OracleConnection conn = new OracleConnection(connstring);
                OracleDataAdapter oda = new OracleDataAdapter(sql, conn);
                ds = new DataSet();
                oda.Fill(ds);
            }
            catch (Exception ex)
            {
                OracleErrMsg = "查询语句错误,错误如下： \r\n " + ex.ToString();
            }
            return ds;
        }

        /// <summary> 
        /// 返回查询结果，传出查询参数；
        /// </summary>
        /// <param name="connstring"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string connstring, string sql, params OracleParameter[] param)
        {
            DataSet ds = null;
            try
            {
                OracleConnection conn = new OracleConnection(connstring);
                conn.Open();
                OracleDataAdapter oda = new OracleDataAdapter(sql, conn);
                foreach (OracleParameter prm in param)
                {
                    oda.SelectCommand.Parameters.Add(prm);
                }
                try
                {
                    ds = new DataSet();
                    oda.Fill(ds);
                }
                catch (OracleException ex)
                {
                    OracleErrMsg = "查询语句错误,错误如下： \r\n " + ex.ToString();
                }
            }
            catch (Exception ex1)
            {
                OracleErrMsg = "查询语句错误,错误如下： \r\n " + ex1.ToString();
            }

            return ds;
        }

        /// <summary>
        /// 获取单个值
        /// </summary>
        /// <param name="connstring"></param>
        /// <param name="selectSQL"></param>
        /// <returns></returns>
        public static object SelectValue(string connstring, string selectSQL)
        {
            OracleConnection conn = new OracleConnection(connstring);
            OracleCommand cmd = new OracleCommand(selectSQL, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (OracleException ex)
            {
                OracleErrMsg = ex.ToString();
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="connstring"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connstring, string sql)
        {
            OracleConnection conn = new OracleConnection(connstring);
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql, conn);
                return Convert.ToInt32(cmd.ExecuteNonQuery());
            }
            catch (OracleException ex)
            {
                OracleErrMsg = ex.ToString();
                return -2;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="connstring"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string ExecuteNonQueryRstring(string connstring, string sql)
        {
            OracleConnection conn = new OracleConnection(connstring);
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery();
                return "OK";
            }
            catch (OracleException ex)
            {
                OracleErrMsg = ex.ToString();
                return OracleErrMsg;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public static int ExecuteNonQuery1(string connstring, string sql, params OracleParameter[] param)
        {
            OracleConnection conn = new OracleConnection(connstring);
            OracleCommand cmd = new OracleCommand(sql, conn)
            {
                CommandType = CommandType.Text
            };


            foreach (OracleParameter prm in param)
            {
                cmd.Parameters.Add(prm);
            }

            cmd.Connection = conn;
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                OracleErrMsg = ex.ToString();
                return -1;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

        }

        /// <summary> 执行带参数的存储过程
        /// 
        /// </summary>
        /// <param name="name">存储名称</param>
        /// <param name="paramList">列表</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connstring, string name, params OracleParameter[] param)
        {
            OracleConnection conn = new OracleConnection(connstring);
            OracleCommand cmd = new OracleCommand(name)
            {
                CommandType = CommandType.StoredProcedure
            };

            foreach (OracleParameter prm in param)
            {
                cmd.Parameters.Add(prm);
            }

            cmd.Connection = conn;
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                OracleErrMsg = ex.ToString();
                return -1;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

        }

        /// <summary> 
        /// 执行有返回值的存储过程；
        /// </summary>
        /// <param name="connstring"></param> 连接数据库参数；
        /// <param name="name"></param>    存储过程名字
        /// <param name="nreval"></param>   第几个是返回值
        /// <param name="param"></param>     数组变量；
        /// <returns></returns>           返回值；小于0为失败；
        public static int ExecuteProc(string connstring, string name, int nreval, params OracleParameter[] param)
        {
            OracleConnection conn = new OracleConnection(connstring);
            conn.Open();
            OracleCommand cmd = new OracleCommand(name, conn)
            {
                CommandType = CommandType.StoredProcedure
            };


            foreach (OracleParameter prm in param)
            {
                cmd.Parameters.Add(prm);
            }

            try
            {
                cmd.ExecuteNonQuery();
                return Convert.ToInt32(cmd.Parameters[nreval].Value);

            }
            catch
            {
                return -5;
            }


        }


        /// <summary> 
        /// 执行有返回值的存储过程；
        /// </summary>
        /// <param name="connstring"></param> 连接数据库参数；
        /// <param name="name"></param>    存储过程名字
        /// <param name="nreval"></param>   第几个是返回值
        /// <param name="param"></param>     数组变量；
        /// <returns></returns>           返回值；小于0为失败；
        public static string ExecuteFunc(string connstring, string name, params OracleParameter[] param)
        {
            OracleConnection conn = new OracleConnection(connstring);
            OracleCommand cmd = new OracleCommand(name, conn)
            {
                CommandType = CommandType.StoredProcedure
            };


            foreach (OracleParameter prm in param)
            {
                cmd.Parameters.Add(prm);
            }

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return cmd.Parameters["RESULT"].Value.ToString();
            }
            catch
            {
                return "";
            }


        }

        /// <summary> ；
        /// 带有参数的SQL语句
        /// </summary>
        /// <param name="connstring">连接字符串</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="paramList">参数</param>
        /// <returns></returns>
        public static bool Update(string connstring, string sql, System.Data.Common.DbParameter[] paramList)
        {
            OracleConnection conn = new OracleConnection(connstring);
            OracleCommand cmd = new OracleCommand(sql);
            cmd.Parameters.Add(paramList);
            cmd.Connection = conn;
            try
            {
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false
                        ;
                }
            }
            catch (OracleException ex)
            {
                OracleErrMsg = ex.ToString();
                return false;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// 批量更新数据库
        /// </summary>
        /// <param name="connstring">连接字符串</param>
        /// <param name="sqls">多个SQL语句</param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(string connstring, List<string> sqls)
        {
            OracleConnection conn = new OracleConnection(connstring);
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand
                {
                    Connection = conn
                };
                OracleTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                try
                {
                    foreach (string sql in sqls)
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                    tran.Commit();
                    return true;
                }
                catch (OracleException ex)
                {
                    OracleErrMsg = ex.ToString();
                    tran.Rollback();
                    return false;
                }
            }
            catch (OracleException ex)
            {
                OracleErrMsg = ex.ToString();
                return false;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// 批量更新数据库
        /// </summary>
        /// <param name="connstring">连接字符串</param>
        /// <param name="sqls">多个SQL语句</param>
        /// <returns></returns>
        public static bool ExecuteNonQuery_sql(string connstring, List<string> sqls)
        {
            SqlConnection conn = new SqlConnection(connstring);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn
                };
                SqlTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                try
                {
                    foreach (string sql in sqls)
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                    tran.Commit();
                    return true;
                }
                catch (SqlException ex)
                {
                    OracleErrMsg = ex.ToString();
                    tran.Rollback();
                    return false;
                }
            }
            catch (SqlException ex)
            {
                OracleErrMsg = ex.ToString();
                return false;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }


        ///  2016-7-31 yu  新增ExecuteFunc2  返回存储过程中V_OUTRESULT变量值
        /// <summary> 执行有返回值的存储过程2；

        /// </summary>
        /// <param name="connstring"></param> 连接数据库参数；
        /// <param name="name"></param>    存储过程名字
        /// <param name="nreval"></param>   第几个是返回值
        /// <param name="param"></param>     数组变量；
        /// <returns></returns>           返回值；小于0为失败；
        public static string ExecuteFunc2(string connstring, string name, params OracleParameter[] param)
        {
            OracleConnection conn = new OracleConnection(connstring);
            OracleCommand cmd = new OracleCommand(name, conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            foreach (OracleParameter prm in param)
            {
                cmd.Parameters.Add(prm);
            }

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return cmd.Parameters["V_OUTRESULT"].Value.ToString();
            }
            catch
            {
                return "异常";
            }
        }



        /// <summary>   判断主键是否重复；
        /// 
        /// </summary>
        /// <param name="connstring">连接字符串</param>
        /// <param name="table">表名称</param>  
        /// <param name="key">主关键字，字符类型</param>
        /// <param name="keyvalue">值</param>
        /// <returns>TRUE为不存在，可以添加</returns>
        public static bool ExistKey(string connstring, string table, string key, string keyvalue)
        {
            bool l = false;
            string StrSelect = "SELECT * FROM " + table + " WHERE " + key + "='" + keyvalue + "'  ";
            try
            {
                DataSet ds = ExecuteDataSet(connstring, StrSelect);
                if (ds == null)
                {

                }
                else
                {
                    if (ds.Tables[0].Rows.Count == 0)
                        l = true;
                }
            }
            catch (OracleException ex)
            {
                OracleErrMsg = "查询语句错误,错误如下： \r\n " + ex.ToString();
                l = false;
            }

            return l;
        }
    }
}