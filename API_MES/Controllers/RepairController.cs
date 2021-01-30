using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API_MES.Dtos;
using API_MES.Helper;
using API_MES.Servise;
using AutoMapper;
using EF_ORACLE;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace API_MES.Controllers
{
   //[Route("api/[controller]")]
    //[ApiController]
    [EnableCors("any")]
    [ApiVersion("4")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SkipActionFilter]
    public class RepairController : ControllerBase
    {
        private readonly T100Context _t100Context;
        private readonly IRepairRepository _repairRepository;
        private readonly ILogger<MESController> _logger;
        private readonly IMapper _mapper;
        public RepairController(IMapper mapper,
                             T100Context t100Context,
                             IRepairRepository  repairRepository,
                             ILogger<MESController> logger)
        {
            _logger = logger;
            _t100Context = t100Context ?? throw new ArgumentNullException(nameof(t100Context));
            _repairRepository = repairRepository ?? throw new ArgumentNullException(nameof(repairRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        //public async Task<IActionResult> Hw_RepairIms(string Date1)
        //{ 
        //    return Ok(await _repairRepository.Hw_RepairIms(Date1));
        //}
        //public async Task<IActionResult> Hw_RepairMes(string Date1)
        //{    
        //    return Ok(await _repairRepository.Hw_RepairMes(Date1));
        //}
        //public async Task<IActionResult> Hw_FpyIms(string Date1)
        //{ 
        //    return Ok();
        //} 
        /// <summary>
        /// 静置时间
        /// </summary>
        /// <param name="code"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("G_PressureTime")]
        public async Task<IActionResult> G_PressureTime(string code)
        {
            return Ok(await _repairRepository.G_PressureTime(code));
        }
        [HttpGet]
        [Route("G_PressureWg")]
        public async Task<IActionResult> G_PressureWg(string code, string sn, string user)
        {
            return Ok(await _repairRepository.G_PressureWg(code, sn, user));
        }
        [HttpGet]
        [Route("G_XMDATEY")]
        public async Task<IActionResult> G_XMDATEY(string code)
        {
            return Ok(await _repairRepository.G_XMDATEY(code));
        }
        [HttpGet]
        [Route("G_HwReDATE")]
        public async Task<IActionResult> G_HwReDATE(string code)
        {
            return Ok(await _repairRepository.G_HwReDATE(code));
        }
        [HttpGet]
        [Route("G_REPairDATa")]
        public async Task<IActionResult> G_REPairDATa(string code, string date)
        {
            return Ok(await _repairRepository.G_REPairDATa(code, date));
        }
        [HttpGet]
        [Route("GetRepairList")]
        public async Task<IActionResult> GetRepairList(string code,string date1,string date2)
        {
            var lastResult = await _repairRepository.GetRepairList(code, date1, date2);
            return Ok(lastResult);
        }
        [HttpGet]
        [Route("GetRepairListByDate")]
        public async Task<IActionResult> GetRepairListByDate(string code, string date1)
        {
            var lastResult = await _repairRepository.GetRepairListByDate(code, date1);
            return Ok(lastResult);
        }
        [HttpGet]
        [Route("GetRepairListCount")]
        public async Task<IActionResult> GetRepairListCount(string code, string date1, string date2)
        {
            var lastResult = await _repairRepository.GetRepairListCount(code, date1, date2);
            return Ok(lastResult);
        }
        [HttpGet]
        [Route("GetRepairListCountByTable")]
        public async Task<IActionResult> GetRepairListCountByTable(string code, string date1, string date2)
        {
            var lastResult = await _repairRepository.GetRepairListCountByTable(code, date1, date2);
            return Ok(lastResult);
        }
        [HttpGet]
        [Route("GetSenDataListByTable")]
        public async Task<IActionResult> GetSenDataListByTable(string code, string date1, string date2)
        {
            var lastResult = await _repairRepository.GetSenDataListByTable(code, date1, date2);
            return Ok(lastResult);
        }
        [HttpGet]
        [Route("GetDataListByTable")]
        public async Task<IActionResult> GetDataListByTable(string code, string date1, string date2)
        {
            var lastResult = await _repairRepository.GetDataListByTable(code, date1, date2);
            return Ok(lastResult);
        }
        [HttpGet]
        [Route("GetODM_PRESSURETIME")]
        public async Task<IActionResult> GetODM_PRESSURETIME(string code)
        {
            var lastResult = await _repairRepository.GetODM_PRESSURETIME(code);
            return Ok(lastResult);
        }
        [HttpGet]
        [Route("Hw_RepairBYImsMes")]
        public async Task<IActionResult> Hw_RepairBYImsMes(string code)
        {
            var lastResult = await _repairRepository.Hw_RepairIms(code);
            var lastResult1 = await _repairRepository.Hw_RepairMes(code);
            var lastResult2 = await _repairRepository.Hw_FtyMesMmi2(code);
            return Ok(lastResult1);
        }
        [HttpGet]
        [Route("Hw_FtyBYImsMes")]
        public async Task<IActionResult> Hw_FtyBYImsMes(string code)
        {
            //var lastResult = await _repairRepository.Hw_FtyIms(code);
            var lastResult1 = await _repairRepository.Hw_FtyMes(code);
            return Ok(lastResult1);
        }


        [HttpPost("BEAT_OUT")]
        public async Task<IActionResult> BEAT_OUT([FromForm] IFormCollection fm)
        {
            DT_RETURN dT_RETURN = new DT_RETURN();
            string LINE = fm["LINE"].ToString() == "" ? "" : fm["LINE"].ToString().Trim();
            string CODE = fm["CODE"].ToString() == "" ? "" : fm["CODE"].ToString().Trim();
            string MODEL = fm["MODEL"].ToString() == "" ? "" : fm["MODEL"].ToString().Trim();
            if (!_repairRepository.IODM_BEATE(LINE))
            {
                dT_RETURN.Success = false;
                dT_RETURN.Msg = "当前线别未维护节拍";
                dT_RETURN.Service = "BEAT_OUT";
                return Ok(dT_RETURN);
            }
            switch (MODEL)
            {
                case "TT":
                    Model.DT_BEAT lastResult = await _repairRepository.BEAT_OUTTT(LINE, CODE, MODEL);
                    return Ok(lastResult);
                default:
                    break;
            }
            return Ok();
        }
        [HttpPost("AddPressuretime")]
        public async Task<IActionResult> AddPressuretime([FromForm] IFormCollection fm) {
            string CLIENCODE = fm["CLIENCODE"].ToString() == "" ? "" : fm["CLIENCODE"].ToString().Trim();
            string DTTIME = fm["DTTIME"].ToString() == "" ? "" : fm["DTTIME"].ToString().Trim();
            string NUMCODE = fm["NUMCODE"].ToString() == "" ? "" : fm["NUMCODE"].ToString().Trim();
            string WGSTATION = fm["WGSTATION"].ToString() == "" ? "" : fm["WGSTATION"].ToString().Trim();
            Entities.ErrMessage lastResult = await _repairRepository.AddPressuretime(CLIENCODE, DTTIME, NUMCODE, WGSTATION);
            return Ok(lastResult);
        }


        [HttpPost("DownExcel")]
        public async Task<IActionResult> DownExcel([FromForm] IFormCollection fm)
        {
            string code = fm["code"].ToString() == "" ? "" : fm["code"].ToString().Trim();
            string date1 = fm["date1"].ToString() == "" ? "" : fm["date1"].ToString().Trim();
            string date2 = fm["date2"].ToString() == "" ? "" : fm["date2"].ToString().Trim(); 
            //GetFQA_CK_SNLOG(string MO, string SN, string LINE, string TY, string DATE1, string DATE2, string ISC,string ISOK,string Ems)
            IEnumerable<REPAIDtos> rsVal = await _repairRepository.GetRepairList(code, date1, date2); 
            //ListVIEW = rsVal.ToList();
            if (rsVal == null)
            {
                return Ok();
            } 
            ExcelHelper excelHeper = new ExcelHelper();
            List<ExcelGridModel> config = new List<ExcelGridModel>
            {
                new ExcelGridModel{name="FACTORY",label="FACTORY", align="left",},
                new ExcelGridModel{name="SN1",label="SN1", align="left",},
                new ExcelGridModel{name="SN2",label="SN2", align="left",},
                new ExcelGridModel{name="COMMAND_CODE",label="COMMAND_CODE", align="left",},
                new ExcelGridModel{name="REPAIRBUGTYPE",label="REPAIRBUGTYPE", align="left",},
                new ExcelGridModel{name="SENDPERSON",label="SENDPERSON", align="left",},
                new ExcelGridModel{name="PERSON",label="PERSON", align="left",},
                new ExcelGridModel{name="ERR_DESCRIBE",label="ERR_DESCRIBE", align="left",},
                new ExcelGridModel{name="SCAN_TIME",label="SCAN_TIME", align="left",},
                new ExcelGridModel{name="POSTION_ITEMVERSION",label="POSTION_ITEMVERSION", align="left",},
                new ExcelGridModel{name="SENDDATE",label="SENDDATE", align="left",},
                new ExcelGridModel{name="POSITION_CODE",label="POSITION_CODE", align="left",},
                new ExcelGridModel{name="BAD_ITEM_CODE",label="BAD_ITEM_CODE", align="left",},
                new ExcelGridModel{name="REPAIRDATE",label="REPAIRDATE", align="left",},
                new ExcelGridModel{name="SECONDTIME",label="SECONDTIME", align="left",},
                new ExcelGridModel{name="SNRESULT",label="SNRESULT", align="left",},
                new ExcelGridModel{name="LINE_CODE",label="LINE_CODE", align="left",},
                new ExcelGridModel{name="RECEIVE_PERSON",label="RECEIVE_PERSON", align="left",}, 
                new ExcelGridModel{name="SYSDATE",label="SYSDATE", align="left",},
            };
            string fileName = fm["FileName"].ToString();
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "nOName.xls";
            }
            return excelHeper.ExcelDownload(rsVal, config, fileName);
        }
        [Route("ExamRoomExport")]
        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> ExamRoomExportAsync([FromForm] IFormCollection fm)
        { 
            string CLIENTNAME = fm["CLIENTNAME"].ToString();
            string DATE1 = fm["DATE1"].ToString();
            string DATE2 = fm["DATE2"].ToString();
            string FileName = string.Empty;
            string SheetName = string.Empty;
            string[] temArr = { };
            DataTable boundTable = new DataTable();
            SheetName = "维护及时率";
            FileName = SheetName + $"{DateTime.Now:yyyyMMddHHmmssfff}.xls"; 
            string[] temArr1 = { "FACTORY", "SN1", "SN2", "COMMAND_CODE", "REPAIRBUGTYPE", "SENDPERSON", "PERSON", "ERR_DESCRIBE", "SCAN_TIME", "POSTION_ITEMVERSION", "SENDDATE", "POSITION_CODE", "BAD_ITEM_CODE", "REPAIRDATE", "SECONDTIME", "SNRESULT", "LINE_CODE", "RECEIVE_PERSON","SYSDATE" };
            temArr = temArr1;
            boundTable = _repairRepository.GetRepairListTB(CLIENTNAME, DATE1, DATE2); 
            List<string> lstTitle = new List<string>(temArr);
            IWorkbook book = new HSSFWorkbook(); 
            //ISheet sheet = book.CreateSheet("Sheet1");
            ISheet sheet = book.CreateSheet(SheetName);
            IRow rowTitle = sheet.CreateRow(0);
            ICellStyle style = book.CreateCellStyle();
            style.VerticalAlignment = VerticalAlignment.Center;//垂直居中 
            //左右位置  CellHorizontalAlignment.位置值
            style.Alignment = HorizontalAlignment.Center;
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom =BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            ICellStyle headStyle = book.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.Center;
            IFont font = book.CreateFont();
            font.FontHeightInPoints = 10;
            font.Boldweight = 700;
            headStyle.SetFont(font);
            headStyle.VerticalAlignment =VerticalAlignment.Center;//垂直居中
            headStyle.Alignment = HorizontalAlignment.Center;
            headStyle.BorderTop = BorderStyle.Thin;
            headStyle.BorderBottom = BorderStyle.Thin;
            headStyle.BorderLeft = BorderStyle.Thin;
            headStyle.BorderRight = BorderStyle.Thin;
            //sheet.AutoFitColumns();//自动适应所有列宽
            //sheet.AutoFitRows();//自动适应所有行高
            int rowIndex = 0;
            for (int i = 0; i < lstTitle.Count; i++)
            {
                //sheet.SetColumnWidth(i, 14 * 256);
                rowTitle.CreateCell(i).SetCellValue(lstTitle[i]);
                rowTitle.GetCell(i).CellStyle = headStyle;
                rowIndex = 1;
            }
            for (int i = 0; i < boundTable.Rows.Count; i++)
            {
              
                IRow row = sheet.CreateRow(i + 1);
                DataRow dr = boundTable.Rows[i];
                for (int j = 0; j < boundTable.Columns.Count; j++)
                {
                    ICell cell = row.CreateCell(j);
                    cell.CellStyle = style;
                    cell.SetCellValue(dr[j].ToString());
                }
                rowIndex++;
            }
//            //  表尾
//            if (true)
//            {
////                HSSFRow headerRow = (HSSFRow)sheet.CreateRow(rowIndex);
////                headerRow.Height = 4 * 256;
////                headerRow.CreateCell(0).SetCellValue(@"1.记录人快速填写机台代码，开始时间，停机代号，停止时间，签名，责任线长技术员分析停机原因
////2.停机事项表内部包含代码事件，可单独编码记录并提报（自加编码优先按26英文字符顺序添加，超出从A字母重计加“1”示例：A1.B1….）
////3.自加编码规则要一直沿用前一页标准一致"); //定义表头
////                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, boundTable.Columns.Count - 1));
////                HSSFCellStyle headStyle1 = (HSSFCellStyle)book.CreateCellStyle();
////                headStyle1.WrapText = true;
////                headStyle1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
////                headStyle1.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
////                headerRow.GetCell(0).CellStyle = headStyle; 
//                HSSFRow headerRow1 = (HSSFRow)sheet.CreateRow(rowIndex + 1);
//                headerRow1.Height = 1 * 256;
//                headerRow1.CreateCell(0).SetCellValue(@"送修总数：       维修总数：           送修及时数：	"); //定义表头
//                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex + 1, rowIndex + 1, 0, boundTable.Columns.Count - 1));
//                HSSFCellStyle headStyle2 = (HSSFCellStyle)book.CreateCellStyle();
//                headStyle2.WrapText = true;
//                headStyle2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
//                headStyle2.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
//                headerRow1.GetCell(0).CellStyle = headStyle2;
//            }

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            //return File(ms, "application/vnd.ms-excel", "考场总表导出.xls");
            return File(ms, "application/vnd.ms-excel", FileName);
        }

        [Route("ExamRoomExportByTable")]
        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> ExamRoomExportByTable([FromForm] IFormCollection fm)
        {
            string code = fm["CLIENTNAME"].ToString();
            string date1 = fm["DATE1"].ToString();
            string date2 = fm["DATE2"].ToString();
            IEnumerable<REPAIDtos> rsVal = await _repairRepository.GetRepairListByDateByTable(code, date1, date2); 
            if (rsVal == null)
            {
                return Ok();
            }
            ExcelHelper excelHeper = new ExcelHelper();
            List<ExcelGridModel> config = new List<ExcelGridModel>
            {   new ExcelGridModel{name="CODE",label="客户代码", align="left",},
                new ExcelGridModel{name="FACTORY",label="加工厂", align="left",},
                new ExcelGridModel{name="SN1",label="SN1", align="left",},
                new ExcelGridModel{name="SN2",label="SN2", align="left",},
                new ExcelGridModel{name="COMMAND_CODE",label="工单", align="left",},
                new ExcelGridModel{name="REPAIRBUGTYPE",label="责任描述", align="left",},
                new ExcelGridModel{name="SENDPERSON",label="送修人", align="left",},
                new ExcelGridModel{name="PERSON",label="维修人", align="left",},
                new ExcelGridModel{name="ERR_DESCRIBE",label="不良原因", align="left",},
                new ExcelGridModel{name="SCAN_TIME",label="故障发生时间", align="left",},
                new ExcelGridModel{name="POSTION_ITEMVERSION",label="故障发生工序", align="left",},
                new ExcelGridModel{name="SENDDATE",label="送修时间", align="left",},
                new ExcelGridModel{name="POSITION_CODE",label="POSITION_CODE", align="left",},
                new ExcelGridModel{name="BAD_ITEM_CODE",label="BAD_ITEM_CODE", align="left",},
                new ExcelGridModel{name="REPAIRDATE",label="维修时间", align="left",},
                new ExcelGridModel{name="SECONDTIME",label="维修完成时间", align="left",},
                new ExcelGridModel{name="SNRESULT",label="维修结果", align="left",},
                new ExcelGridModel{name="LINE_CODE",label="线别", align="left",},
                new ExcelGridModel{name="RECEIVE_PERSON",label="接收人", align="left",},
            };
            var fileName = code+ "维护及时率数据.xls";
            return excelHeper.ExcelDownload(rsVal, config, fileName);
        }

        [HttpPost("OutMESREPAIRExcel")]
        public async Task<IActionResult> OutMESREPAIRExcel([FromForm] IFormCollection fm)
        {
            string code = fm["CLIENTNAME"].ToString();
            string date1 = fm["DATE1"].ToString(); 
            //GetFQA_CK_SNLOG(string MO, string SN, string LINE, string TY, string DATE1, string DATE2, string ISC,string ISOK,string Ems)
            IEnumerable<REPAIDtos> rsVal = await _repairRepository.GetRepairListByDate(code, date1);
            //ListVIEW = rsVal.ToList();
            if (rsVal == null)
            {
                return Ok();
            }
            ExcelHelper excelHeper = new ExcelHelper();
            List<ExcelGridModel> config = new List<ExcelGridModel>
            {   new ExcelGridModel{name="CODE",label="客户代码", align="left",},
                new ExcelGridModel{name="FACTORY",label="加工厂", align="left",},
                new ExcelGridModel{name="SN1",label="SN1", align="left",},
                new ExcelGridModel{name="SN2",label="SN2", align="left",},
                new ExcelGridModel{name="COMMAND_CODE",label="工单", align="left",},
                new ExcelGridModel{name="REPAIRBUGTYPE",label="责任描述", align="left",},
                new ExcelGridModel{name="SENDPERSON",label="送修人", align="left",},
                new ExcelGridModel{name="PERSON",label="维修人", align="left",},
                new ExcelGridModel{name="ERR_DESCRIBE",label="不良原因", align="left",},
                new ExcelGridModel{name="SCAN_TIME",label="故障发生时间", align="left",},
                new ExcelGridModel{name="POSTION_ITEMVERSION",label="故障发生工序", align="left",},
                new ExcelGridModel{name="SENDDATE",label="送修时间", align="left",},
                new ExcelGridModel{name="POSITION_CODE",label="POSITION_CODE", align="left",},
                new ExcelGridModel{name="BAD_ITEM_CODE",label="BAD_ITEM_CODE", align="left",},
                new ExcelGridModel{name="REPAIRDATE",label="维修时间", align="left",},
                new ExcelGridModel{name="SECONDTIME",label="维修完成时间", align="left",},
                new ExcelGridModel{name="SNRESULT",label="维修结果", align="left",},
                new ExcelGridModel{name="LINE_CODE",label="线别", align="left",},
                new ExcelGridModel{name="RECEIVE_PERSON",label="接收人", align="left",},  
            };
            var fileName = date1+" "+ "维护及时率数据.xls"; 
            return excelHeper.ExcelDownload(rsVal, config, fileName);
        }



    }
}
