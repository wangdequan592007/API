using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public class NLogHelp
    {
        /// <summary>
        /// 输出操作日志到NLog
        /// </summary>
        public static void WriteInfo(string msg)
        {
            //写入操作日志
            const string mainLogger = "logger";
            var logger = NLog.LogManager.GetLogger(mainLogger);
            logger.Info(msg);
        }
        /// <summary>
        /// 输出错误日志到NLog
        /// </summary>
        public static void WriteError(string msg)
        {
            //写入操作日志
            const string mainLogger = "logger";
            var logger = NLog.LogManager.GetLogger(mainLogger);
            logger.Error(msg);
        }
        /// <summary>
        /// 输出异常日志到NLog
        /// </summary>
        public static void WriteDebug(string msg)
        {
            //写入操作日志
            const string mainLogger = "logger";
            var logger = NLog.LogManager.GetLogger(mainLogger);
            logger.Debug(msg);
        }
        public static int LOG_LEVENL = 3;
        //在网站根目录下创建日志目录
        public static string path = Directory.GetCurrentDirectory() + "\\logs";

        /**
         * 向日志文件写入调试信息
         * @param className 类名
         * @param content 写入内容
         */
        public static void Debug(string className, string content)
        {
            if (LOG_LEVENL >= 3)
            {
                WriteLog("DEBUG", className, content);
            }
        }

        /**
        * 向日志文件写入运行时信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Info(string className, string content)
        {
            if (LOG_LEVENL >= 2)
            {
                WriteLog("INFO", className, content);
            }
        }

        /**
        * 向日志文件写入出错信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Error(string className, string content)
        {
            if (LOG_LEVENL >= 1)
            {
                WriteLog("ERROR", className, content);
            }
        }

        /**
        * 实际的写日志操作
        * @param type 日志记录类型
        * @param className 类名
        * @param content 写入内容
        */
        private static readonly object ob = new object();
        protected static void WriteLog(string type, string className, string content)
        {
            lock (ob)
            {
                if (!Directory.Exists(path))//如果日志目录不存在就创建
                {
                    Directory.CreateDirectory(path);
                }

                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
                string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名

                //创建或打开日志文件，向日志文件末尾追加记录
                StreamWriter mySw = File.AppendText(filename);

                //向日志文件写入内容
                string write_content = time + " " + type + " " + className + ": " + content;
                mySw.WriteLine(write_content);

                //关闭日志文件
                mySw.Close();
            }
        }
    }
}
