using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API_MES.Dtos;
using Spire.Xls;

namespace API_MES.Helper
{
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public class ExcelHelpers
    {
        #region 导入
        /// <summary>
        /// 将Excel以文件流转换DataTable
        /// </summary>
        /// <param name="hasTitle">是否有表头</param>
        /// <param name="path">文件路径</param>
        /// <param name="tableindex">文件簿索引</param>
        public static DataTable ExcelToDataTableFormPath(bool hasTitle = true, string path = "", int tableindex = 0)
        {
            //新建Workbook
            Workbook workbook = new Workbook();
            //将当前路径下的文件内容读取到workbook对象里面
            workbook.LoadFromFile(path);
            //得到第一个Sheet页
            Worksheet sheet = workbook.Worksheets[tableindex];
            return SheetToDataTable(hasTitle, sheet);
        }

        

        /// <summary>
        /// 将Excel以文件流转换DataTable
        /// </summary>
        /// <param name="hasTitle">是否有表头</param>
        /// <param name="stream">文件流</param>
        /// <param name="tableindex">文件簿索引</param>
        public static DataTable ExcelToDataTableFormStream(bool hasTitle = true, Stream stream = null, int tableindex = 0)
        {
            //新建Workbook
            Workbook workbook = new Workbook();
            //将文件流内容读取到workbook对象里面
            workbook.LoadFromStream(stream);
            //得到第一个Sheet页
            Worksheet sheet = workbook.Worksheets[tableindex];

            int iRowCount = sheet.Rows.Length;
            int iColCount = sheet.Columns.Length;
            DataTable dt = new DataTable();
            //生成列头
            for (int i = 0; i < iColCount; i++)
            {
                var name = "column" + i;
                if (hasTitle)
                {
                    var txt = sheet.Range[1, i + 1].Text;
                    if (!string.IsNullOrEmpty(txt)) name = txt;
                }
                while (dt.Columns.Contains(name)) name = name + "_1";//重复行名称会报错。
                dt.Columns.Add(new DataColumn(name, typeof(string)));
            }
            //生成行数据
            int rowIdx = hasTitle ? 2 : 1;
            for (int iRow = rowIdx; iRow <= iRowCount; iRow++)
            {
                DataRow dr = dt.NewRow();
                for (int iCol = 1; iCol <= iColCount; iCol++)
                {
                    dr[iCol - 1] = sheet.Range[iRow, iCol].Text;
                }
                dt.Rows.Add(dr);
            }
            return SheetToDataTable(hasTitle, sheet);
        }

        private static DataTable SheetToDataTable(bool hasTitle, Worksheet sheet)
        {
            int iRowCount = sheet.Rows.Length;
            int iColCount = sheet.Columns.Length;
            var dt = new DataTable();
            //生成列头
            for (var i = 0; i < iColCount; i++)
            {
                var name = "column" + i;
                if (hasTitle)
                {
                    var txt = sheet.Range[1, i + 1].Text;
                    if (!string.IsNullOrEmpty(txt)) name = txt;
                }
                while (dt.Columns.Contains(name)) name = name + "_1";//重复行名称会报错。
                dt.Columns.Add(new DataColumn(name, typeof(string)));
            }
            //生成行数据
            // ReSharper disable once SuggestVarOrType_BuiltInTypes
            var rowIdx = hasTitle ? 2 : 1;
            for (var iRow = rowIdx; iRow <= iRowCount; iRow++)
            {
                var dr = dt.NewRow();
                for (var iCol = 1; iCol <= iColCount; iCol++)
                {
                    dr[iCol - 1] = sheet.Range[iRow, iCol].Text;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion 
        #region 导出
        /// <summary>
        /// 将DaTaTable转成byte[]类型
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="hasTitle">是否有表头</param>
        /// <returns></returns>
        public static byte[] GetDataTableToByte(DataTable dt, bool hasTitle)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Workbook workbook = new Workbook();
                Worksheet sheet = workbook.Worksheets[0];//第一个工作簿
                if (hasTitle) //表头
                {
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        sheet.Range[1, j + 1].Text = dt.Columns[j].ColumnName;
                        sheet.Range[1, j + 1].ColumnWidth = 22;
                        sheet.Range[1, j + 1].Style.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;//边框
                        sheet.Range[1, j + 1].Style.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;//边框
                        sheet.Range[1, j + 1].Style.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;//边框
                        sheet.Range[1, j + 1].Style.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;//边框
                    }
                }

                //循环表数据
                for (var i = 0; i < dt.Rows.Count; i++)//循环赋值
                {
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        var dyg = sheet.Range[i + 2, j + 1];
                        dyg.Text = dt.Rows[i][j].ToString();
                        dyg.ColumnWidth = 22;
                        dyg.Style.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;//边框
                        dyg.Style.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
                        dyg.Style.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
                        dyg.Style.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
                    }
                }
                workbook.SaveToStream(ms, FileFormat.Version2007);
                byte[] data = ms.ToArray();
                return data;
            }
        }
        public static MemoryStream GetDataTableToMemory(DataTable dt, bool hasTitle)
        {
           // using (MemoryStream ms = new MemoryStream())
            {
                Workbook workbook = new Workbook();
                Worksheet sheet = workbook.Worksheets[0];//第一个工作簿
                if (hasTitle) //表头
                {
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        sheet.Range[1, j + 1].Text = dt.Columns[j].ColumnName;
                        sheet.Range[1, j + 1].ColumnWidth = 22;
                        sheet.Range[1, j + 1].Style.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;//边框
                        sheet.Range[1, j + 1].Style.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;//边框
                        sheet.Range[1, j + 1].Style.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;//边框
                        sheet.Range[1, j + 1].Style.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;//边框
                    }
                }

                //循环表数据
                for (var i = 0; i < dt.Rows.Count; i++)//循环赋值
                {
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        var dyg = sheet.Range[i + 2, j + 1];
                        dyg.Text = dt.Rows[i][j].ToString();
                        dyg.ColumnWidth = 22;
                        dyg.Style.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;//边框
                        dyg.Style.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
                        dyg.Style.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
                        dyg.Style.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
                    }
                }
                //sheet.InsertDataTable(dt, true, 1, 1);
                MemoryStream ms = new MemoryStream();
                workbook.SaveToStream(ms, FileFormat.Version2007);
                ms.Flush();
                ms.Position = 0;
                return ms;
                
                //byte[] data = ms.ToArray();
                //return ms;
            }
        }
        public static MemoryStream GetDataTableToMemory(DataTable dt, IEnumerable<string> colNames, bool hasTitle)
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];//第一个工作簿
            if (hasTitle) //表头
            {
                int colNamesCount = colNames.Count();
                foreach (var colName in colNames)
                {
                    var p = colName; 
                }
                for (var j = 0; j < colNamesCount; j++)
                {
                    sheet.Range[1, j + 1].Text = colNames.ElementAt(j);
                    sheet.Range[1, j + 1].ColumnWidth = 22;
                    sheet.Range[1, j + 1].Style.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;//边框
                    sheet.Range[1, j + 1].Style.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;//边框
                    sheet.Range[1, j + 1].Style.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;//边框
                    sheet.Range[1, j + 1].Style.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;//边框
                }
            }

            //循环表数据
            for (var i = 0; i < dt.Rows.Count; i++)//循环赋值
            {
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    var dyg = sheet.Range[i + 2, j + 1];
                    dyg.Text = dt.Rows[i][j].ToString();
                    dyg.ColumnWidth = 22;
                    dyg.Style.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;//边框
                    dyg.Style.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
                    dyg.Style.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
                    dyg.Style.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
                }
            }
            //sheet.InsertDataTable(dt, true, 1, 1);
            MemoryStream ms = new MemoryStream();
            workbook.SaveToStream(ms, FileFormat.Version2007);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }
        public static  MemoryStream  OutAsToExcelToMemory(DataTable data)
        {
                string Err = string.Empty;
             
                if (data.Rows.Count <= 0)
                {
                    Err = "无数据！";
                    return null ;
                }

                //创建一个Excel文档
                Workbook workbook = new Workbook();
                //获取第一个工作表
                Worksheet sheet = workbook.Worksheets[0];


                //将datatable导入到工作表，数据从工作表的第一行第一列开始写入
                sheet.InsertDataTable(data, true, 1, 1);
                //加边框       
                foreach (CellRange range in sheet.AllocatedRange.Rows)
                {
                    //设置边框颜色
                    range.BorderInside(LineStyleType.Thin, Color.Silver);
                    range.BorderAround(LineStyleType.Thin, Color.Silver);
                }
                //设置格式 每第二到最后列,数字类型为整数
                for (int i = 1; i < data.Columns.Count - 1; i++)
                {
                    sheet.Columns[i].NumberFormat = "0";
                }
                //首行行高
                sheet.SetRowHeight(1, 20);
                //sheet.SetColumnWidth(1, 23);

                //设置自适应列宽
                sheet.AllocatedRange.AutoFitColumns();

                //设置第一行的填充颜色
                sheet.Rows[0].Style.Color = Color.Silver;

                //保存文档
                //workbook.SaveToFile(filePath, ExcelVersion.Version2013);
                MemoryStream ms = new MemoryStream();
                workbook.SaveToStream(ms, FileFormat.Version2007);
                ms.Flush();
                ms.Position = 0;
                return ms;  
        }
        #endregion
        #region ListToExcel
        /// <summary>
        /// 类集合，导出到Excel，返回excel文件名
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="tList">List集合</param>
        /// <param name="filePath">存放文件目录</param>
        /// <returns>excel文件名</returns>
        public static string ListToExcel<T>(List<T> tList, string filePath) where T : class, new()
        {
            //创建一个Excel文件
            try
            {
                T t = new T();
                Type type = t.GetType();
                PropertyInfo[] p_list = type.GetProperties();
                FieldInfo[] fieldInfos = type.GetFields();
                DataTable dt = new DataTable();
                if (p_list != null && p_list.Length > 0)
                {
                    /**利用attribute将属性转成列名*/
                    for (int i = 0; i < p_list.Length; i++)
                    {
                        Attribute datareaderattr = p_list[i].GetCustomAttribute(typeof(DA_GROSSDtos1), false);
                        if (datareaderattr != null)
                        {
                            DA_GROSSDtos1 dataattr = new DA_GROSSDtos1();
                            string columName = dataattr.DT_BOM;
                            dt.Columns.Add(columName, Type.GetType("System.String"));
                        }
                    }
                }
                /**List数据转DataTable*/
                for (int j = 0; j < tList.Count; j++)
                {
                    DataRow row = dt.NewRow();
                    T tem = tList[j];
                    Type jtype = tem.GetType();
                    PropertyInfo[] j_list = type.GetProperties();
                    FieldInfo[] jfieldInfos = type.GetFields();
                    if (j_list != null && j_list.Length > 0)
                    {
                        /**利用循环将List中类的值填到dt中，一行值对应一个类*/
                        foreach (var item in j_list)
                        {
                            Attribute datareaderattr = item.GetCustomAttribute(typeof(DA_GROSSDtos1), false);
                            if (datareaderattr != null)
                            {
                                DA_GROSSDtos1 dataattr = new DA_GROSSDtos1();
                                row[dataattr.DT_CLIENTNAME] = item.GetValue(tem, null).ToString();
                            }
                        }
                        dt.Rows.Add(row);
                    }
                }
                /**新建excel*/
                Workbook newBook1 = new Workbook();
                newBook1.CreateEmptySheets(1);
                Worksheet newSheet1 = newBook1.Worksheets[0];
                /**利用双循环将dt值填到excel表中*/
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    newSheet1.Range[1, k + 1].Text = dt.Columns[k].ColumnName;
                    for (int z = 0; z < dt.Rows.Count; z++)
                    {
                        newSheet1.Range[z + 2, k + 1].Text = dt.Rows[z][k].ToString();
                    }
                }
                /**设置样式*/
                newSheet1.AllocatedRange.AutoFitColumns();//列宽自适应
                newSheet1.Range[1, 1, 1, newSheet1.LastColumn].Style.Font.IsBold = true;//首行字体加粗
                newSheet1.FreezePanes(2, 1);//首行冻结

                /**保存*/
                string strName = @"\Export" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                string path = filePath + strName;
                newBook1.SaveToFile(path, ExcelVersion.Version2013);
                return strName;
            }
            catch (Exception ex)
            {
                //LogTool.ExceptionLog(ex, ex.StackTrace);
                return ex.Message;
            }
        }
        #endregion
        public static async Task<bool> OutAsToExcel(DataTable data, string filePath )
        {
            string Err = string.Empty;
            var t = await Task.Run(() =>
            {
                if (data.Rows.Count <= 0)
                {
                    Err = "无数据！"; 
                    return false;
                }

                //创建一个Excel文档
                Workbook workbook = new Workbook();
                //获取第一个工作表
                Worksheet sheet = workbook.Worksheets[0];


                //将datatable导入到工作表，数据从工作表的第一行第一列开始写入
                sheet.InsertDataTable(data, true, 1, 1);
                //加边框       
                foreach (CellRange range in sheet.AllocatedRange.Rows)
                {
                    //设置边框颜色
                    range.BorderInside(LineStyleType.Thin, Color.Silver);
                    range.BorderAround(LineStyleType.Thin, Color.Silver);
                }
                //设置格式 每第二到最后列,数字类型为整数
                for (int i = 1; i < data.Columns.Count - 1; i++)
                {
                    sheet.Columns[i].NumberFormat = "0";
                }
                //首行行高
                sheet.SetRowHeight(1, 20);
                //sheet.SetColumnWidth(1, 23);

                //设置自适应列宽
                sheet.AllocatedRange.AutoFitColumns();

                //设置第一行的填充颜色
                sheet.Rows[0].Style.Color = Color.Silver;

                //保存文档
                workbook.SaveToFile(filePath, ExcelVersion.Version2013);
                return true;
            });
            return t;
        }
    }
}
