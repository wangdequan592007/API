using System;
namespace API_MES.Result
{
    public class BaseResult
    {
        /// <summary>
        /// 结果编码
        /// </summary>
        public int Errcode { get; set; }
        /// <summary>
        /// 结果消息 如果不成功，返回的错误信息
        /// </summary>
        public string Errmsg { get; set; }
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public BaseResult()
        {

        }

        /// <summary>
        /// 有参数构造函数
        /// </summary>
        /// <param name="_errcode"></param>
        /// <param name="_errmsg"></param>
        public BaseResult(int _errcode, string _errmsg)
        {
            Errcode = _errcode;
            Errmsg = _errmsg;
        }
    }
}