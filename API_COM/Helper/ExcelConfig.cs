using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace API_COM.Helper
{
    /// <summary>
    /// 描 述：Excel导入导出设置
    /// </summary>
    public class ExcelConfig
    {

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 前景色
        /// </summary>
        public Color ForeColor { get; set; }
        /// <summary>
        /// 背景色
        /// </summary>
        public Color Background { get; set; }
        private short _titlepoint;
        /// <summary>
        /// 标题字号
        /// </summary>
        public short TitlePoint
        {
            get
            {
                if (_titlepoint == 0)
                {
                    return 20;
                }
                else
                {
                    return _titlepoint;
                }
            }
            set { _titlepoint = value; }
        }
        private short _headpoint;
        /// <summary>
        /// 列头字号
        /// </summary>
        public short HeadPoint
        {
            get
            {
                if (_headpoint == 0)
                {
                    return 10;
                }
                else
                {
                    return _headpoint;
                }
            }
            set { _headpoint = value; }
        }
        /// <summary>
        /// 标题高度
        /// </summary>
        public short TitleHeight { get; set; }
        /// <summary>
        /// 列标题高度
        /// </summary>
        public short HeadHeight { get; set; }
        private string _titlefont;
        /// <summary>
        /// 标题字体
        /// </summary>
        public string TitleFont
        {
            get
            {
                if (_titlefont == null)
                {
                    return "微软雅黑";
                }
                else
                {
                    return _titlefont;
                }
            }
            set { _titlefont = value; }
        }
        private string _headfont;
        /// <summary>
        /// 列头字体
        /// </summary>
        public string HeadFont
        {
            get
            {
                if (_headfont == null)
                {
                    return "微软雅黑";
                }
                else
                {
                    return _headfont;
                }
            }
            set { _headfont = value; }
        }
        /// <summary>
        /// 是否按内容长度来适应表格宽度
        /// </summary>
        public bool IsAllSizeColumn { get; set; }
        /// <summary>
        /// 列设置
        /// </summary>
        public List<ColumnModel> ColumnEntity { get; set; }

    }
    /// <summary>
    /// 描 述：表格属性模型
    /// </summary>
    public class ExcelGridModel
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// excel列名
        /// </summary>
        public string label { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public string width { get; set; }
        /// <summary>
        /// 对其方式
        /// </summary>
        public string align { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public string height { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public string hidden { get; set; }
    }
    /// <summary>
    /// 描 述：Excel导入导出列设置模型
    /// </summary>
    public class ColumnModel
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// Excel列名
        /// </summary>
        public string ExcelColumn { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 前景色
        /// </summary>
        public Color ForeColor { get; set; }
        /// <summary>
        /// 背景色
        /// </summary>
        public Color Background { get; set; }
        /// <summary>
        /// 字体
        /// </summary>
        public string Font { get; set; }
        /// <summary>
        /// 字号
        /// </summary>
        public short Point { get; set; }
        /// <summary>
        ///对齐方式
        ///left 左
        ///center 中间
        ///right 右
        ///fill 填充
        ///justify 两端对齐
        ///centerselection 跨行居中
        ///distributed
        /// </summary>
        public string Alignment { get; set; }
    }
}