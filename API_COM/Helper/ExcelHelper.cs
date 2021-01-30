using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API_COM.Helper
{
    public class ExcelHelper
    {
        public ActionResult ExcelDownload<T>(IEnumerable<T> list, IEnumerable<ExcelGridModel> columnList, string fileName)
        {
            var excelConfig = ConvertExcelGridModelToConfig(columnList, fileName);
            DataTable rowData = list.ToDataTable(columnList.Select(i => i.name));
            var streams = NpoiHelper.ExcelStream(rowData, columnList.Select(i => i.label), true);
            //var streams = NpoiHelper.ExcelStream();
            //var streams = ExcelHelpers.OutAsToExcelToMemory(rowData);
            //var stream = ExportMemoryStream(rowData, excelConfig);
            return new FileStreamResult(streams, MIMEType.xlsx) { FileDownloadName = fileName };

        }
        public ActionResult ExcelDownloadNPOI<T>(IEnumerable<T> list, IEnumerable<ExcelGridModel> columnList, string fileName)
        {
            var excelConfig = ConvertExcelGridModelToConfig(columnList, fileName);
            DataTable rowData = list.ToDataTable(columnList.Select(i => i.name));
            //var streams = NpoiHelper.GetExcelFromDataTable(rowData, fileName);
            var streams = NpoiHelper.GetDataTableToMemory(rowData,columnList.Select(i => i.label), true);
            FileStreamResult FileNpoi = new FileStreamResult(streams, MIMEType.xlsx) { FileDownloadName = fileName };
            
            return FileNpoi;

        }
        #region 辅助方法
        /// <summary>
        /// 如果文件名中没有后缀名，增加文件后缀名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string JointXls(string fileName)
        {
            if (!fileName.EndsWith(".xls"))
            {
                fileName += ".xls";
            }

            return fileName;
        } 
        private ExcelConfig ConvertExcelGridModelToConfig(IEnumerable<ExcelGridModel> columnList, string fileName)
        {
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = fileName;
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            foreach (ExcelGridModel columnModel in columnList)
            {
                excelconfig.ColumnEntity.Add(new ColumnModel()
                {
                    Column = columnModel.name,
                    ExcelColumn = columnModel.label,
                    Alignment = columnModel.align,
                });
            }

            return excelconfig;
        } 
        /// <summary>
        /// MIME文件类型
        /// </summary>
        class MIMEType
        {
            public const string xls = "application/ms-excel";
            public const string xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }
        #endregion
    }
}
