using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using API_MES.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_MES.Controllers
{
    [Route("api/[controller]")]
    [SkipActionFilter]
    public class XlsxController : Controller
    {
        /// <summary>
        /// 通过依赖注入获取HostingEnvironment，对应可以获取程序的相关目录及属性。
        /// </summary>
        //private IHostingEnvironment _hostingEnvironment;
        private IHostEnvironment _hostingEnvironment;
        public XlsxController(IHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //} 
        /// <summary>
        /// 公用路径
        /// </summary>
        /// <returns></returns>
        private Tuple<string, string> GetTuple()
        {
            //string sWebRootFolder = _hostingEnvironment.WebRootPath + @"\execl";
            string sWebRootFolder = _hostingEnvironment.ContentRootPath;
            string sFileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}-{Guid.NewGuid()}.xlsx";
            return Tuple.Create(sWebRootFolder, sFileName);
        }

 

        #region 封装后-导出、导入Excel.xlsx文件
        [HttpGet]
        [Route("Export2")]
        public IActionResult Export2()
        {
            string dt1 = DateTime.Now.ToString("yyyy-MM-dd/HH:mm:ss.fff");
            string path = GetTuple().Item1;
            ExcelHelper.Export(GetDic(), path, out Tuple<string, string> t);
            string dt2 = DateTime.Now.ToString("yyyy-MM-dd/HH:mm:ss.fff");
            //return Json(new {
            //    msg = "ok",
            //    data = $"开始时间：{dt1},结束时间：{dt2}"
            //});

            return File(t.Item1, t.Item2);
        }

        private Dictionary<string, DataTable> GetDic()
        {
            DataTable dt = new DataTable("cart");
            DataColumn dc1 = new DataColumn("prizename", Type.GetType("System.String"));
            DataColumn dc2 = new DataColumn("point", Type.GetType("System.Int16"));
            DataColumn dc3 = new DataColumn("number", Type.GetType("System.Int16"));
            DataColumn dc4 = new DataColumn("totalpoint", Type.GetType("System.Int64"));
            DataColumn dc5 = new DataColumn("prizeid", Type.GetType("System.String"));
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            dt.Columns.Add(dc5);
            //以上代码完成了DataTable的构架，但是里面是没有任何数据的  
            for (int i = 0; i <= 100000; i++)
            {
                DataRow dr = dt.NewRow();
                dr["prizename"] = $"娃娃{i}";
                dr["point"] = 10;
                dr["number"] = 1;
                dr["totalpoint"] = 10;
                dr["prizeid"] = $"t00{i}";
                dt.Rows.Add(dr);
            }
            //填充了100000条相同的记录进去  

            var dic = new Dictionary<string, DataTable>();

            for (int i = 0; i <= 10; i++)
            {
                dic.Add($"{dt.TableName}-{i}", dt);
            }

            return dic;
        }

        [HttpPost]
        public IActionResult Import2(IFormFile excelFile)
        {
            string sWebRootFolder = GetTuple().Item1;
            try
            {
                ExcelHelper.Import(excelFile, sWebRootFolder, out string content);
                return Content(content);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion
        ///  
        //导出excel功能控制器   可以
        //[Authorize]
        [HttpGet("/Export")]
        //[HttpGet]
        //[ActionName("Export")]//控制器名称重新定义，加上可以直接请求Exprot
        public HttpResponseMessage PostExportData()
        {

            var file = ExcelStream();
            //string csv = _service.GetData(model);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(file);
            //a text file is actually an octet-stream (pdf, etc)
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream"); 
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
            //we used attachment to force download
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = "file.xls";
            return result;
        }
        //得到excel文件流
        private System.IO.Stream ExcelStream()
        {
             //var list = dc.v_bs_dj_bbcdd1.Where(eps).ToList();
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
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
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            file.Seek(0, SeekOrigin.Begin);

            return file;

            //return File(file, "application/vnd.ms-excel", "保税订单.xls");
        }
        [HttpGet("/DownExcel")]
        public IActionResult DownExcel()
        {
            var list = Student.GetStudents();
            ExcelHelper excelHeper = new ExcelHelper();
            var config = new List<ExcelGridModel> {
                 new ExcelGridModel{name="Id",label="学号", align="left",},
                  new ExcelGridModel{name="Name",label="姓名", align="left",},
                   new ExcelGridModel{name="IsBanZhang",label="是否班长", align="left",},
            };
            var fileName = "a.excel";
            return excelHeper.ExcelDownload(list, config, fileName);
        }
        /// 
        /// 5.2、考场总表导出   可以
        /// 
        [Route("ExamRoomExport")]
        [HttpGet]
        public FileResult ExamRoomExport()
        {
            #region 生成xls
            string[] temArr = { "考场", "科目时间", "监考老师", "座位号", "姓名", "准考证", "学生班级" };
            List<string> lstTitle = new List<string>(temArr);
            //var lstTitle = new List { "考场", "科目时间", "监考老师", "座位号", "姓名", "准考证", "学生班级" };//, 
            IWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("Sheet1");
            IRow rowTitle = sheet.CreateRow(0);
            ICellStyle style = book.CreateCellStyle();
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//垂直居中  
            for (int i = 0; i < lstTitle.Count; i++)
            {

                rowTitle.CreateCell(i).SetCellValue(lstTitle[i]);

            }
            //var list = null;
            ////var list = data.GetListResultStudent(examId);
            //if (list != null)
            //{
            //    list.OrderBy(o => o.RoomId);
            //    int start = 0;//记录同组开始行号
            //    int end = 0;//记录同组结束行号
            //    string temp = "";//记录上一行的值
            //    for (int i = 0; i < list.Count; i++)
            //    {

            //        IRow row = sheet.CreateRow(i + 1);

            //        row.CreateCell(0).SetCellValue(list[i].RoomName);
            //        row.CreateCell(1).SetCellValue(list[i].ExamTime.ToLongDateString() + list[i].ExamTime.ToLongTimeString());
            //        row.CreateCell(2).SetCellValue(list[i].TeacherName);
            //        row.CreateCell(3).SetCellValue(list[i].ZuoWeiNumber);
            //        row.CreateCell(4).SetCellValue(list[i].StudentName);
            //        row.CreateCell(5).SetCellValue(list[i].ZhunNumber);
            //        row.CreateCell(6).SetCellValue(list[i].ClassName);




            //        row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellType(CellType.String);
            //        var cellText = row.Cells[0].StringCellValue;//获取当前行 第1列的单元格的值

            //        if (cellText == temp)//上下行相等，记录要合并的最后一行
            //        {
            //            end = i;
            //        }
            //        else//上下行不等，记录
            //        {
            //            if (start != end)
            //            {
            //                //设置一个合并单元格区域，使用上下左右定义CellRangeAddress区域
            //                //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
            //                CellRangeAddress region = new CellRangeAddress(start + 1, end + 1, 0, 0);
            //                sheet.AddMergedRegion(region);
            //            }
            //            start = i;
            //            end = i;
            //            temp = cellText;
            //        }
            //    }
            //}

            #endregion
            for (int i = 0; i < 7; i++)
            {
                sheet.AutoSizeColumn(i);//i：根据标题的个数设置自动列宽
            }

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "考场总表导出.xls");
        }
        

    }
    class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsBanZhang { get; set; }

        public static IEnumerable<Student> GetStudents()
        {
            return new List<Student>
                {
                      new Student{Name="小强",Id=1,IsBanZhang=false},
                new Student{Name="小文",Id=2,IsBanZhang=true},
                new Student{Name="小黄",Id=3,IsBanZhang=false},
                new Student{Name="小刚",Id=3,IsBanZhang=false},
                };
        }
    }
}
