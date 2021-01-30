using System;
namespace API_MES.Result
{
    public class UploadEntity
    {
        /// <summary>
        /// 图片主键
        /// </summary>
        public string Picguid { get; set; }
        /// <summary>
        /// 原始图片地址
        /// </summary>
        public string Originalurl { get; set; }
        /// <summary>
        /// 缩略图地址
        /// </summary>
        public string Thumburl { get; set; }
    }
}
