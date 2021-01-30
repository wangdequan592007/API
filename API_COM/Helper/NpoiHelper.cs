using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace API_COM.Helper
{
    public class NpoiHelper
    {

        public static MemoryStream GetExcelFromDataTable(DataTable dt,string fileName)
        {
            try
            {
                IWorkbook workbook = new XSSFWorkbook(); // create *.xlsx file, use HSSFWorkbook() for creating *.xls file.XSSFWorkbook
                ISheet sheet1 = workbook.CreateSheet();
                IRow row1 = sheet1.CreateRow(0);
                //for (int i = 0; dt.Columns.Count > i; i++)
                //{
                //    row1.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                //}
                IRow rowHeader = sheet1.CreateRow(0);
                //生成excel标题
                rowHeader.CreateCell(0).SetCellValue("汇通单号");
                rowHeader.CreateCell(1).SetCellValue("单据日期");
                rowHeader.CreateCell(2).SetCellValue("订单号");
                rowHeader.CreateCell(3).SetCellValue("收件人");
                rowHeader.CreateCell(4).SetCellValue("收件人电话");
                rowHeader.CreateCell(5).SetCellValue("收件人地址");
                rowHeader.CreateCell(6).SetCellValue("物流公司");
                rowHeader.CreateCell(7).SetCellValue("运单号");
                rowHeader.CreateCell(8).SetCellValue("数量");
                rowHeader.CreateCell(9).SetCellValue("状态");

                for (int i = 0; dt.Rows.Count > i; i++)
                {
                    IRow row = sheet1.CreateRow(i + 1);
                    for (int j = 0; dt.Columns.Count > j; j++)
                    {
                        row.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                    }
                }
                //using (var ms = new MemoryStream())
                //{
                //    using (var sw = new StreamWriter(ms, Encoding.UTF8, 1024, true))
                //    {
                //        sw.WriteLine("data");
                //        sw.WriteLine("data 2");
                //    }

                //    ms.Position = 0; 
                //}  
                //MemoryStream ms1 = new MemoryStream();
                //workbook.Write(ms1); 
                //ms1.Flush();
                //ms1.Position = 0;
                //return ms1;
                NpoiMemoryStream ms = new NpoiMemoryStream
                {
                    AllowClose = false
                };
                workbook.Write(ms);
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                ms.AllowClose = true;
                return ms;
                //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                //result.Content = new StreamContent(ms);
                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                //result.Content.Headers.ContentDisposition.FileName = string.Format("{0}.xlsx", fileName); 
            }
            catch (System.Exception ex)
            {

                return null;
            }
            
        } 
        public static MemoryStream GetDataTableToMemory(DataTable rowData, IEnumerable<string> colNames, bool v)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            string TableName = rowData.TableName;
            if (string.IsNullOrEmpty(TableName))
            {
                TableName = "Sheet1";
            }
            XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet(TableName);
            XSSFCellStyle dateStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            XSSFDataFormat format = (XSSFDataFormat)workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
            //dateStyle.FillBackgroundColor = IndexedColors.Blue.Index;
            //dateStyle.FillPattern = FillPattern.Bricks;
            XSSFCellStyle cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            //设置样式---
            //设置单元格上下左右边框线
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            //文字水平和垂直对齐方式
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            //cellStyle.DataFormat = 194;
            //cellStyle.FillForegroundColor = IndexedColors.Pink.Index;
            //cellStyle.FillPattern = FillPattern.SolidForeground;
            int rowIndex = 0;
            XSSFRow eheaderRow = (XSSFRow)sheet.CreateRow(0);
            List<string> lcolName = new List<string>();
            foreach (var item in colNames)
            {
                lcolName.Add(item);
            }
            for (int i = 0; i < lcolName.Count; i++)
            {
                sheet.SetColumnWidth(i, 14 * 256);
                XSSFCellStyle headStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                //headStyle.Alignment = CellHorizontalAlignment.CENTER;
                XSSFFont font = (XSSFFont)workbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.Boldweight = 700;
                headStyle.SetFont(font);
                headStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                headStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                headStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                headStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                //文字水平和垂直对齐方式
                headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                headStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                eheaderRow.CreateCell(i).SetCellValue(lcolName[i]);
                eheaderRow.GetCell(i).CellStyle = headStyle;
            }
            rowIndex = 1;
            foreach (DataRow row in rowData.Rows)
            { 
                XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                #region 填充内容
                foreach (DataColumn column in rowData.Columns)
                {
                    if (column.ColumnName == "CARTON_GROSS_WEIGTH")
                    {
                        string ssw = row[column].ToString();
                    }
                    string ss = row[column].ToString();
                    XSSFCell newCell = (XSSFCell)dataRow.CreateCell(column.Ordinal);
                    string type = row[column].GetType().FullName.ToString(); 
                    newCell.SetCellValue(GetValue(row[column].ToString(), type)); 
                    newCell.CellStyle = cellStyle;
                }
                #endregion
                rowIndex++;
            }
            NpoiMemoryStream ms = new NpoiMemoryStream
            {
                AllowClose = false
            };
            workbook.Write(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;
            return ms;
            //throw new NotImplementedException();
        }
        public static bool DataSetToExcel(DataSet ds, string Path, string strTitle)
        {
            try
            {
                string[] title = strTitle.Split(',');
                bool result = false;
                FileStream fs = null;
                XSSFWorkbook workbook = new XSSFWorkbook();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    string TableName = ds.Tables[i].TableName;
                    if (string.IsNullOrEmpty(TableName))
                    {
                        TableName = "Sheet1";
                    }
                    XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet(TableName);
                    XSSFCellStyle dateStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFDataFormat format = (XSSFDataFormat)workbook.CreateDataFormat();
                    dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
                    //dateStyle.FillBackgroundColor = IndexedColors.Blue.Index;
                    //dateStyle.FillPattern = FillPattern.Bricks;
                    XSSFCellStyle cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    //设置样式---
                    //设置单元格上下左右边框线
                    cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    //文字水平和垂直对齐方式
                    cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                    cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                    //cellStyle.DataFormat = 194;
                    //cellStyle.FillForegroundColor = IndexedColors.Pink.Index;
                    //cellStyle.FillPattern = FillPattern.SolidForeground;
                    int rowIndex = 0;

                    #region 新建表，填充表头，填充列头，样式

                    if (rowIndex == 0)
                    {
                        #region 列头及样式
                        XSSFRow eheaderRow = (XSSFRow)sheet.CreateRow(0);
                        //headerRow.Height = 1 * 256;
                        //for (int i = 0; i < columnNum; i++) //写入字段  
                        //    datas[0, i] = title[j];
                        for (int j = 0; j < title.Length; j++)
                        {
                            sheet.SetColumnWidth(j, 14 * 256);
                            XSSFCellStyle headStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                            //headStyle.Alignment = CellHorizontalAlignment.CENTER;
                            XSSFFont font = (XSSFFont)workbook.CreateFont();
                            font.FontHeightInPoints = 10;
                            font.Boldweight = 700;
                            headStyle.SetFont(font);
                            headStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                            headStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                            headStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                            headStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                            //文字水平和垂直对齐方式
                            headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                            headStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                            eheaderRow.CreateCell(j).SetCellValue(title[j]);
                            eheaderRow.GetCell(j).CellStyle = headStyle;
                        }

                        #endregion
                        rowIndex = 1;
                    }
                    #endregion

                    foreach (DataRow row in ds.Tables[i].Rows)
                    {
                        //string ss = row[0].ToString();

                        XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                        #region 填充内容
                        foreach (DataColumn column in ds.Tables[i].Columns)
                        {
                            if (column.ColumnName == "CARTON_GROSS_WEIGTH")
                            {
                                string ssw = row[column].ToString();
                            }
                            string ss = row[column].ToString();
                            XSSFCell newCell = (XSSFCell)dataRow.CreateCell(column.Ordinal);
                            string type = row[column].GetType().FullName.ToString();
                            //if (type == "System.Decimal")
                            //{
                            //    newCell.SetCellValue(double.Parse(row[column].ToString()));
                            //}
                            //else
                            //{
                            newCell.SetCellValue(GetValue(row[column].ToString(), type));
                            //}
                            newCell.CellStyle = cellStyle;
                        }
                        #endregion
                        rowIndex++;
                    }

                }
                using (fs = File.OpenWrite(Path))
                {
                    workbook.Write(fs);//向打开的这个xls文件中写入数据
                    result = true;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private static string GetValue(string cellValue, string type)
        {
            object value = string.Empty;
            switch (type)
            {
                case "System.String"://字符串类型
                    value = cellValue;
                    break;
                case "System.DateTime"://日期类型
                    System.DateTime dateV;
                    System.DateTime.TryParse(cellValue, out dateV);
                    value = dateV;
                    break;
                case "System.Boolean"://布尔型
                    bool boolV = false;
                    bool.TryParse(cellValue, out boolV);
                    value = boolV;
                    break;
                case "System.Int16"://整型
                    value = cellValue;
                    break;
                case "System.Int32":
                    value = cellValue;
                    break;
                case "System.Int64":
                    value = cellValue;
                    break;
                case "System.Byte":
                    int intV = 0;
                    int.TryParse(cellValue, out intV);
                    value = intV;
                    break;
                case "System.Decimal"://浮点型
                                      //value = double.Parse(cellValue);
                    double doubV = 0;
                    double.TryParse(cellValue, out doubV);
                    value = doubV;
                    break;
                case "System.Double":
                    double doubV1 = 0;
                    double.TryParse(cellValue, out doubV1);
                    value = doubV1;
                    break;
                case "System.DBNull"://空值处理
                    value = string.Empty;
                    break;
                default:
                    value = string.Empty;
                    break;
            }
            return value.ToString();
        }
        //得到excel文件流
        public static MemoryStream ExcelStream(DataTable dt, IEnumerable<string> colNames, bool hasTitle)
        {

            try
            {
                //var list = dc.v_bs_dj_bbcdd1.Where(eps).ToList();
                XSSFWorkbook hssfworkbook = new XSSFWorkbook();

                ISheet sheet1 = hssfworkbook.CreateSheet("保税订单");

                IRow rowHeader = sheet1.CreateRow(0);
                //生成excel标题
                rowHeader.CreateCell(0).SetCellValue("汇通单号");
                rowHeader.CreateCell(1).SetCellValue("单据日期");
                rowHeader.CreateCell(2).SetCellValue("订单号");
                rowHeader.CreateCell(3).SetCellValue("收件人");
                rowHeader.CreateCell(4).SetCellValue("收件人电话");
                rowHeader.CreateCell(5).SetCellValue("收件人地址");
                rowHeader.CreateCell(6).SetCellValue("物流公司");
                rowHeader.CreateCell(7).SetCellValue("运单号");
                rowHeader.CreateCell(8).SetCellValue("数量");
                rowHeader.CreateCell(9).SetCellValue("状态");

                //生成excel内容
                //for (int i = 0; i < list.Count; i++)
                //{
                //    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                //    rowtemp.CreateCell(0).SetCellValue(list[i].bh_user);
                //    rowtemp.CreateCell(1).SetCellValue(list[i].rq.Value.ToString("yyyy-MM-dd HH:mm:dd"));
                //    rowtemp.CreateCell(2).SetCellValue(list[i].bh_khdd);
                //    rowtemp.CreateCell(3).SetCellValue(list[i].re_name);
                //    rowtemp.CreateCell(4).SetCellValue(list[i].re_tel);
                //    rowtemp.CreateCell(5).SetCellValue(list[i].re_fulladdress);
                //    rowtemp.CreateCell(6).SetCellValue(list[i].bm_kdgs);
                //    rowtemp.CreateCell(7).SetCellValue(list[i].kddh);
                //    rowtemp.CreateCell(8).SetCellValue((int)list[i].sl_total);
                //    rowtemp.CreateCell(9).SetCellValue(list[i].mc_state_dd);
                //}

                for (int i = 0; i < 10; i++)
                    sheet1.AutoSizeColumn(i);

                //MemoryStream file = new MemoryStream();
                //hssfworkbook.Write(file);
                //file.Seek(0, SeekOrigin.Begin);
                //return file;
                //导出Excel文件的方法
                var ms = new NpoiMemoryStream();
                ms.AllowClose = false;
                hssfworkbook.Write(ms);
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                ms.AllowClose = true;
                return ms;
            }
            catch (System.Exception ex)
            {

                return null;
            }
          
            //return File(file, "application/vnd.ms-excel", "保税订单.xls");
        }
        //public FileStreamResult PDFGenerator(int id)
        //{
        //    MemoryStream ms = GeneratePDF(id);

        //    byte[] file = ms.ToArray();
        //    MemoryStream output = new MemoryStream();
        //    output.Write(file, 0, file.Length);
        //    output.Position = 0;
        //    HttpContext.Response.AddHeader("content-disposition",
        //    "inline; filename=myPDF.pdf");

        //    return File(output, "application/pdf", fileDownloadName = "myPDF.pdf");
        //}
        //新建类 重写Npoi流方法
        public class NpoiMemoryStream : MemoryStream
        {
            public NpoiMemoryStream()
            {
                AllowClose = true;
            }

            public bool AllowClose { get; set; }

            public override void Close()
            {
                if (AllowClose)
                    base.Close();
            }
        }
        //public static MemoryStream GetDataTableToMemory(DataTable dt, IEnumerable<string> colNames, bool hasTitle)
        //{ 
        //    MemoryStream ms = new MemoryStream();
        //    workbook.SaveToStream(ms, FileFormat.Version2007);
        //    ms.Flush();
        //    ms.Position = 0;
        //    return ms;
        //}
    }
}
