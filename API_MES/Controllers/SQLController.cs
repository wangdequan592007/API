using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API_MES.Data;
using API_MES.Dtos;
using API_MES.Helper;
using API_MES.Servise;
using AutoMapper;
using EF_SQL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//Take()方法的作用是从查询结果中提取前n个结果；而Skip()方法则是跳过前n个结果，返回剩余的结果
namespace API_MES.Controllers
{
    [EnableCors("any")]
    [ApiVersion("3")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SkipActionFilter]
    public class SQLController : ControllerBase
    {
        private readonly ILogger<MESController> _logger;
        private readonly ISqlRepository _sqlRepository;
        private readonly EF6sqlserver23 _eF6Sqlserver23;
        private readonly IMapper _mapper;
        public SQLController(IMapper mapper, ILogger<MESController> logger,ISqlRepository sqlRepository, EF6sqlserver23 eF6Sqlserver23)
        {
            _logger = logger;
            _sqlRepository = sqlRepository ?? throw new ArgumentNullException(nameof(sqlRepository));
            _eF6Sqlserver23 = eF6Sqlserver23 ?? throw new ArgumentNullException(nameof(eF6Sqlserver23));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
          // var ls = _eF6Sqlserver23.employee23s.Where(x=>x.Code=="11110532").ToList();
           return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("GetUsrName")]
        public  IActionResult  GetUsrName(string UsrID)
        {
            var ls = _eF6Sqlserver23.employee23s.Where(x => x.Code == UsrID).ToList();
            return Ok(ls);
        }
        [HttpGet]
        [Route("GetUsrNameLI")]
        public IActionResult GetUsrNameLI(string UsrID)
        {
            var ls = _eF6Sqlserver23.employee23s.Where(x => x.Code.Contains(UsrID)||x.CnName.Contains(UsrID)).Take(10).ToList();
            return Ok(ls);
        }
        [HttpGet]
        [Route("GetPriceBybom")]
        public async Task<IActionResult> GetPriceBybom(string BOM)
        {
           return Ok (await _sqlRepository.GetDA_BOM(BOM));
        }
        [HttpGet]
        [Route("GetPriceBymodel")]
        public async Task<IActionResult> GetPriceBymodel(string MODEL)
        {
            return Ok(await _sqlRepository.GetDA_BOMPrice(MODEL));
        }
        [HttpGet]
        [Route("GetPrice")]
        public async Task<IActionResult> GetPrice(string BOM, string DATE1, string DATE2,string CLIENTNAME)
        {
            return Ok(await _sqlRepository.GetBom(BOM, DATE1, DATE2, CLIENTNAME));
        }
        [HttpGet]
        [Route("GetGROSS")]
        public async Task<IActionResult> GetGROSS(string MO, string CLIENTNAME, string MANAGER, string DATE1, string DATE2)
        { 
            return Ok(_mapper.Map<IEnumerable<DA_GROSSDtos1>>(await _sqlRepository.GetGROSS(MO,CLIENTNAME,MANAGER,DATE1,DATE2)));
        }
        [HttpGet]
        [Route("GetELSEGROSS")]
        public async Task<IActionResult> GetELSEGROSS(string CLIENTNAME, string MODEL, string DATE1, string DATE2)
        {
            return Ok(_mapper.Map<IEnumerable<DA_ELSEGROSSDtos>>(await _sqlRepository.GetELSEGROSS(CLIENTNAME, MODEL, DATE1, DATE2)));
        }
        [HttpGet]
        [Route("GetINSTAL")]
        public async Task<IActionResult> GetINSTAL(string CLIENTNAME, string MODEL, string DATE1, string DATE2)
        {
            var dd = await _sqlRepository.GetINSTAL(CLIENTNAME, MODEL, DATE1, DATE2);
            return Ok(dd);
        }
        [HttpGet]
        [Route("GetVIEW")]
        public async Task<IActionResult> GetVIEW(string CLIENTNAME, string DATE1)
        {
            var ddd = await _sqlRepository.getDA_VIEW(DATE1, CLIENTNAME);
            return Ok(ddd);
        }
        [HttpGet]
        [Route("GetVIEWMODEL")]
        public async Task<IActionResult> GetVIEWMODEL(string CLIENTNAME, string DATE1)
        {
            var ddd = await _sqlRepository.getDA_VIEWMODEL(DATE1, CLIENTNAME);
            return Ok(ddd);
        }
        [HttpGet]
        [Route("GetPAY")]
        public async Task<IActionResult> GetPAY(string CLIENTNAME, string MODEL, string DATE1, string DATE2)
        {
            switch (MODEL)
            {
                case "3001":
                    return Ok(_mapper.Map<IEnumerable<DA_PAYPERSONDtos>>(await _sqlRepository.GetPAYPERSON(CLIENTNAME, MODEL, DATE1, DATE2)));
                    //break;
                case "3002":
                    return Ok(_mapper.Map<IEnumerable<DA_PAYLOSSDtos>>(await _sqlRepository.GetPAYLOSS(CLIENTNAME, MODEL, DATE1, DATE2)));
                    //break;
                case "3003":
                    return Ok(_mapper.Map<IEnumerable<DA_PAYLOSSDtos>>(await _sqlRepository.GetPAYLOSS(CLIENTNAME, MODEL, DATE1, DATE2)));
                    //break;
                case "3004":
                    return Ok(_mapper.Map<IEnumerable<DA_PAYLOSSDtos>>(await _sqlRepository.GetPAYLOSS(CLIENTNAME, MODEL, DATE1, DATE2)));
                    ////break;
                case "3005":
                    return Ok(_mapper.Map<IEnumerable<DA_PAYMONTHDtos>>(await _sqlRepository.GetPAYMONTH(CLIENTNAME, MODEL, DATE1, DATE2)));
                    //break;
                case "3006":
                    return Ok(_mapper.Map<IEnumerable<DA_PAYMONTHDtos>>(await _sqlRepository.GetPAYMONTH(CLIENTNAME, MODEL, DATE1, DATE2)));
                    //break;
                case "3007":
                    return Ok(_mapper.Map<IEnumerable<DA_PAYMONTHDtos>>(await _sqlRepository.GetPAYMONTH(CLIENTNAME, MODEL, DATE1, DATE2)));
                    //break;
                case "3008":
                    return Ok(_mapper.Map<IEnumerable<DA_PAYMONTHDtos>>(await _sqlRepository.GetPAYMONTH(CLIENTNAME, MODEL, DATE1, DATE2)));
                    //break;
                default:
                    break;
            }
            return Ok();
        }
        [HttpGet]
        [Route("GetCLENTNAME")]
        public async Task<IActionResult> GetCLENTNAME(string NAME)
        {
            return Ok(await _sqlRepository.GET_CLENTNAME(NAME));
        }
        [HttpGet]
        [Route("GET_CLENTNAMEBU1")]
        public async Task<IActionResult> GET_CLENTNAMEBU1(string NAME)
        {
            return Ok(await _sqlRepository.GET_CLENTNAMEBU1(NAME));
        }
        [HttpGet]
        [Route("GetCLICODE")]
        public async Task<IActionResult> GetCLICODE() {
            return Ok(await _sqlRepository.GetDA_CLENTCODEs());
        }
        [HttpGet]
        [Route("GetDASTAORE")]
        public async Task<IActionResult> GetDASTAORE()
        {
            return Ok(await _sqlRepository.GetDA_STAOREs());
        }
        [HttpPost]
        [Route("ADDBOM")]
        public async Task<IActionResult> ADDBOM([FromForm] IFormCollection fm)
        {
            var BOM = fm["BOM"].ToString();
            var PRICE = fm["PRICE"].ToString();
            var CRT_USER = fm["CRT_USER"].ToString();
            var MODEL = fm["MODEL"].ToString();
            var CLIENTNAME = fm["CLIENTNAME"].ToString();
            var CLIENTCODE = fm["CLIENTCODE"].ToString();
            if (string.IsNullOrWhiteSpace(BOM))
            {
                return NotFound();
            }
            else {
                BOM = BOM.Trim();
                PRICE = PRICE.Trim();
            }
            if (string.IsNullOrEmpty(MODEL))
            {
                MODEL = MODEL.Trim();
            }
            var getbom =await _sqlRepository.GetDA_BOM(BOM);
            if (getbom==null)
            {
                var dA_BOM = new DA_BOM
                {
                    BOM = BOM,
                    Id = new Guid(),
                    PRICE= PRICE,
                    CRT_USER= CRT_USER,
                    MODEL= MODEL,
                    CLIENTCODE=CLIENTCODE,
                    CLIENTNAME=CLIENTNAME
                    
                };
                 _sqlRepository.AddDA_BOM(dA_BOM);
                await _sqlRepository.SaveAsync();
                return Ok(BOM+"保存完成！！");
            }
            else
            {
                getbom.PRICE = PRICE;
                getbom.CRT_DATE = System.DateTime.Now;
                getbom.PRICE = PRICE;
                getbom.CRT_USER = CRT_USER;
                getbom.MODEL = MODEL;
                getbom.CLIENTCODE = CLIENTCODE;
                getbom.CLIENTNAME = CLIENTNAME;
                _sqlRepository.UpdateDA_BOM(getbom);
                await _sqlRepository.SaveAsync();
                return Ok(BOM+"更新完成！！");
            }
            //return Ok();
        } 
        [HttpPost]
        [Route("ADD_INSTORE")]
        public async Task<IActionResult> ADD_INSTORE([FromForm] IFormCollection fm)
        {
            var BOM = fm["BOM"].ToString();
            var PRICE = fm["PRICE"].ToString();
            var CRT_USER = fm["CRT_USER"].ToString();
            var MODEL = fm["MODEL"].ToString(); 
            var NAME = fm["NAME"].ToString();
            var COUNT = fm["COUNT"].ToString();
            var DT_DATE1 = fm["DT_DATE"].ToString();
            var KD_USER = fm["KD_USER"].ToString();
            if (string.IsNullOrEmpty(COUNT))
            {
                COUNT = COUNT.Trim();
            }
            DT_INSTORE dT_INSTORE = new DT_INSTORE
            {
                ID=new Guid(),
                DT_BOM = BOM,
                DT_COUNT = COUNT,
                DT_MODEL = MODEL,
                DT_NAME = NAME,
                DT_PRICE = PRICE,
                DT_DATE= Convert.ToDateTime(DT_DATE1),
                CRT_USER= CRT_USER,
                KD_USER= KD_USER
            };
            _sqlRepository.ADD_INSTORE(dT_INSTORE);
            await _sqlRepository.SaveAsync();
            return Ok("添加成功");
        }
        [HttpPost]
        [Route("ADD_DATEINSTORE")]
        public async Task<IActionResult> ADD_DATEINSTORE([FromForm] IFormCollection fm)
        {
            var DT_DATE = fm["DT_DATE"].ToString()==""?"":fm["DT_DATE"].ToString().Trim();
            var DT_LINE = fm["DT_LINE"].ToString() == "" ? "" : fm["DT_LINE"].ToString().Trim();
            var DT_MO = fm["DT_MO"].ToString() == "" ? "" : fm["DT_MO"].ToString().Trim();
            var DT_BOM = fm["DT_BOM"].ToString() == "" ? "" : fm["DT_BOM"].ToString().Trim();
            var DT_MODEL = fm["DT_MODEL"].ToString() == "" ? "" : fm["DT_MODEL"].ToString().Trim();
            var DT_PLAN = fm["DT_PLAN"].ToString() == "" ? "" : fm["DT_PLAN"].ToString().Trim();
            var DT_STORENB = fm["DT_STORENB"].ToString() == "" ? "0" : fm["DT_STORENB"].ToString().Trim();
            var DT_MANAGER = fm["DT_MANAGER"].ToString() == "" ? "" : fm["DT_MANAGER"].ToString().Trim();
            var DT_MANNAME = fm["DT_MANNAME"].ToString() == "" ? "" : fm["DT_MANNAME"].ToString().Trim();
            var DT_PERSON = fm["DT_PERSON"].ToString() == "" ? "" : fm["DT_PERSON"].ToString().Trim();
            var DT_USERHOUR = fm["DT_USERHOUR"].ToString() == "" ? "0" : fm["DT_USERHOUR"].ToString().Trim();
            var DT_OFFUSERNB = fm["DT_OFFUSERNB"].ToString() == "" ? "0" : fm["DT_OFFUSERNB"].ToString().Trim();
            var DT_OFFTIMENB = fm["DT_OFFTIMENB"].ToString() == "" ? "0" : fm["DT_OFFTIMENB"].ToString().Trim();
            var DT_CLASS = fm["DT_CLASS"].ToString() == "" ? "" : fm["DT_CLASS"].ToString().Trim();
            var DT_REMARK = fm["DT_REMARK"].ToString() == "" ? "" : fm["DT_REMARK"].ToString().Trim();
            var CRT_USER = fm["CRT_USER"].ToString() == "" ? "" : fm["CRT_USER"].ToString().Trim();
            var DT_TYPE = fm["DT_TYPE"].ToString() == "" ? "" : fm["DT_TYPE"].ToString().Trim();
            DA_DATEINSTORE dT_INSTORE = new DA_DATEINSTORE
            {
                ID = new Guid(),
                DT_BOM = DT_BOM, 
                DT_MODEL = DT_MODEL,
                DT_DATE=Convert.ToDateTime(DT_DATE),
                DT_CLASS = DT_CLASS,
                DT_LINE = DT_LINE,
                DT_MANAGER = DT_MANAGER,
                DT_MANNAME = DT_MANNAME,
                DT_MO = DT_MO,
                DT_OFFTIMENB = DT_OFFTIMENB,
                DT_OFFUSERNB = DT_OFFUSERNB,
                DT_PERSON = DT_PERSON,
                DT_PLAN = DT_PLAN,
                DT_REMARK = DT_REMARK,
                DT_STORENB = DT_STORENB,
                DT_USERHOUR = DT_USERHOUR,
                CRT_USER = CRT_USER,
                CRT_DATE=System.DateTime.Now,
                DT_TYPE= DT_TYPE

            };
            _sqlRepository.AddDA_DATEINSTORE(dT_INSTORE);
            await _sqlRepository.SaveAsync();
            return Ok("添加成功");
        }
        [HttpPost]
        [Route("ADD_ELSEGROSS")]
        public async Task<IActionResult> ADD_ELSEGROSS([FromForm] IFormCollection fm)
        {
            var DT_DATE1 = fm["DT_DATE"].ToString();
            var DT_MODEL1 = fm["DT_MODEL"].ToString();
            var DT_NAME1 = fm["DT_NAME"].ToString();
            var DT_VALUE1 = fm["DT_VALUE"].ToString();
            var DT_CLIENTNAME1 = fm["DT_CLIENTNAME"].ToString();
            var DT_CLIENTCODE1 = fm["DT_CLIENTCODE"].ToString();
            var CRT_USER1 = fm["CRT_USER"].ToString();
            DateTime dateTime1 = Convert.ToDateTime(DT_DATE1);
            int nb = Convert.ToInt32(DT_MODEL1);
            var Table1 =await _sqlRepository.GtDA_ELSEGROSS(dateTime1,nb, DT_CLIENTNAME1,DT_CLIENTCODE1);
           
            if (Table1==null)
            {
                var dA_ELSEGROSS = new DA_ELSEGROSS
                {
                    DT_DATE= dateTime1,
                    DT_MODEL=nb,
                    DT_NAME= DT_NAME1,
                    DT_VALUE= DT_VALUE1,
                    CRT_USER= CRT_USER1,
                    DT_CLIENTNAME= DT_CLIENTNAME1,
                    DT_CLIENTCODE= DT_CLIENTCODE1,
                };
                _sqlRepository.AddDA_ELSEGROSS(dA_ELSEGROSS);
                await _sqlRepository.SaveAsync();
                return Ok(DT_NAME1 + "保存完成！！");
            }
            else
            {
                Table1.CRT_USER = CRT_USER1; 
                Table1.DT_VALUE = DT_VALUE1;
                Table1.CRT_DATE = System.DateTime.Now;
                Table1.DT_CLIENTCODE = DT_CLIENTCODE1;
                _sqlRepository.UpdateDA_ELSEGROSS(Table1);
                await _sqlRepository.SaveAsync();
                return Ok(DT_NAME1 + "修改完成！！");
            }
        }
        [HttpPost]
        [Route("ADD_GROSS")]
        public async Task<IActionResult> ADD_GROSS([FromForm] IFormCollection fm)
        { 
            var DT_CLIENTNAME1 = fm["DT_CLIENTNAME"].ToString();
            var DT_MODEL1 = fm["DT_MODEL"].ToString();
            var DT_NAME1 = fm["DT_NAME"].ToString();
            var DT_DATE1 = fm["DT_DATE"].ToString();
            var DT_MO1 = fm["DT_MO"].ToString();
            var DT_BOM1 = fm["DT_BOM"].ToString();
            var DT_PRICE1 = fm["DT_PRICE"].ToString();
            var DT_STORENB1 = fm["DT_STORENB"].ToString();
            var DT_MANAGER1 = fm["DT_MANAGER"].ToString();
            var DT_MANNAME1 = fm["DT_MANNAME"].ToString();
            var DT_ON1 = fm["DT_ON"].ToString();
            var DT_IN1 = fm["DT_IN"].ToString();
            var DT_OUT1 = fm["DT_OUT"].ToString();
            var DT_OFFWORK1 = fm["DT_OFFWORK"].ToString();
            var DT_ONWORK1 = fm["DT_ONWORK"].ToString();
            var DT_PERSONHOUR1 = fm["DT_PERSONHOUR"].ToString();
            var DT_LINE = fm["DT_LINE"].ToString();
            var DT_PLAN = fm["DT_PLAN"].ToString();
            var DT_NOTE = fm["DT_NOTE"].ToString();
            var CRT_USER1 = fm["CRT_USER"].ToString();
            var DT_DIRECTHOUR = fm["DT_DIRECTHOUR"].ToString();
            var DT_CLIENTCODE = fm["DT_CLIENTCODE"].ToString();
            DateTime dateTime1 = Convert.ToDateTime(DT_DATE1);
           // int nb = Convert.ToInt32(DT_MODEL1);
            _sqlRepository.AddDA_GROSS(new DA_GROSS
            {
                DT_CLIENTNAME = DT_CLIENTNAME1,
                DT_MODEL = DT_MODEL1,
                DT_NAME = DT_NAME1,
                DT_DATE = dateTime1,
                DT_MO = DT_MO1,
                DT_BOM = DT_BOM1,
                DT_LINE = DT_LINE,
                DT_PRICE = DT_PRICE1,
                DT_PLAN = DT_PLAN,
                DT_STORENB = DT_STORENB1,
                DT_MANAGER = DT_MANAGER1,
                DT_MANNAME = DT_MANNAME1,
                DT_ON = DT_ON1,
                DT_IN = DT_IN1,
                DT_OUT = DT_OUT1,
                DT_OFFWORK = DT_OFFWORK1,
                DT_ONWORK = DT_ONWORK1,
                DT_PERSONHOUR = DT_PERSONHOUR1,
                DT_NOTE = DT_NOTE,
                CRT_USER = CRT_USER1,
                DT_DIRECTHOUR= DT_DIRECTHOUR,
                DT_CLIENTCODE= DT_CLIENTCODE
            });
            await _sqlRepository.SaveAsync();
            return Ok("添加成功");
        }
        [HttpPost]
        [Route("ADD_PAY")]
        public async Task<IActionResult> ADD_PAY([FromForm] IFormCollection fm)
        {
            var DT_CLIENTNAME1 = fm["DT_CLIENTNAME"].ToString();
            var DT_CLIENTCODE1 = fm["DT_CLIENTCODE"].ToString();
            var DT_MODEL1 = fm["DT_MODEL"].ToString();
            var DT_NAME1 = fm["DT_NAME"].ToString();
            var DT_DATE1 = fm["DT_DATE"].ToString();
            var DT_REMARKS1 = fm["DT_REMARKS"].ToString();
            var CRT_USER1 = fm["CRT_USER"].ToString();
            switch (DT_MODEL1)
            {
                case "3001"://人员支出
                    DateTime dateTime1 = Convert.ToDateTime(DT_DATE1);
                    int nb = Convert.ToInt32(DT_MODEL1);
                    var DT_INDIRECTWORKTIME1 = fm["DT_INDIRECTWORKTIME"].ToString();
                    var DT_INDIRECTWAGE1 = fm["DT_INDIRECTWAGE"].ToString();
                    var DT_DIRECTHOUR1 = fm["DT_DIRECTHOUR"].ToString();
                    var DT_INDIRECTMOUTH1 = fm["DT_INDIRECTMOUTH"].ToString();
                    DA_PAYPERSON dA_ = new DA_PAYPERSON {
                        DT_CLIENTNAME=DT_CLIENTNAME1,
                        DT_DATE=dateTime1,
                        DT_MODEL=nb,
                        DT_NAME=DT_NAME1,
                        DT_DIRECTHOUR=DT_DIRECTHOUR1,
                        DT_INDIRECTMOUTH=DT_INDIRECTMOUTH1,
                        DT_INDIRECTWAGE=DT_INDIRECTWAGE1,
                        DT_INDIRECTWORKTIME=DT_INDIRECTWORKTIME1,
                        DT_REMARKS=DT_REMARKS1,
                        CRT_USER= CRT_USER1
                    };
                    _sqlRepository.AddDA_PAYPERSON(dA_);
                    await  _sqlRepository.SaveAsync();
                    break;
                case "3002":
                    dateTime1 = Convert.ToDateTime(DT_DATE1);
                    var DT_BOM2= fm["DT_BOM2"].ToString();
                    var DT_PRICE2 = fm["DT_PRICE2"].ToString();
                    var DT_LOSS2 = fm["DT_LOSS2"].ToString();
                    DA_PAYLOSS dA_1 = new DA_PAYLOSS {
                        DT_BOM=DT_BOM2,
                        DT_CLIENTNAME=DT_CLIENTNAME1,
                        DT_DATE=dateTime1,
                        DT_LOSS=DT_LOSS2,
                        DT_MODEL=3002,
                        DT_NAME=DT_NAME1,
                        DT_PRICE=DT_PRICE2,
                        DT_REMARKS=DT_REMARKS1,
                        CRT_USER=CRT_USER1,
                        DT_CLIENTCODE = DT_CLIENTCODE1,
                    };
                    _sqlRepository.AddDA_PAYLOSS(dA_1);
                    await _sqlRepository.SaveAsync();
                    break;
                case "3003":
                    dateTime1 = Convert.ToDateTime(DT_DATE1);
                    var DT_BOM3 = fm["DT_BOM3"].ToString();
                    var DT_PRICE3 = fm["DT_PRICE3"].ToString();
                    var DT_LOSS3 = fm["DT_LOSS3"].ToString();
                    DA_PAYLOSS dA_3 = new DA_PAYLOSS
                    {
                        DT_BOM = DT_BOM3,
                        DT_CLIENTNAME = DT_CLIENTNAME1,
                        DT_DATE = dateTime1,
                        DT_LOSS = DT_LOSS3,
                        DT_MODEL = 3003,
                        DT_NAME = DT_NAME1,
                        DT_PRICE = DT_PRICE3,
                        DT_REMARKS = DT_REMARKS1,
                        DT_CLIENTCODE = DT_CLIENTCODE1,
                        CRT_USER = CRT_USER1
                    };
                    _sqlRepository.AddDA_PAYLOSS(dA_3);
                    await _sqlRepository.SaveAsync();
                    break;
                case "3004":
                    dateTime1 = Convert.ToDateTime(DT_DATE1);
                    var DT_BOM4 = fm["DT_BOM4"].ToString();
                    var DT_PRICE4 = fm["DT_PRICE4"].ToString();
                    var DT_LOSS4 = fm["DT_LOSS4"].ToString();
                    DA_PAYLOSS dA_4 = new DA_PAYLOSS
                    {
                        DT_BOM = DT_BOM4,
                        DT_CLIENTNAME = DT_CLIENTNAME1,
                        DT_DATE = dateTime1,
                        DT_LOSS = DT_LOSS4,
                        DT_MODEL = 3004,
                        DT_NAME = DT_NAME1,
                        DT_PRICE = DT_PRICE4,
                        DT_REMARKS = DT_REMARKS1,
                        DT_CLIENTCODE = DT_CLIENTCODE1,
                        CRT_USER = CRT_USER1
                    };
                    _sqlRepository.AddDA_PAYLOSS(dA_4);
                    await _sqlRepository.SaveAsync();
                    break;
                case "3005":
                    dateTime1 = Convert.ToDateTime(DT_DATE1);
                    var DT_EXPEND5 = fm["DT_EXPEND5"].ToString(); 
                    DA_PAYMONTH dA_5 = new DA_PAYMONTH
                    {
                        DT_CLIENTNAME = DT_CLIENTNAME1,
                        DT_REMARKS=DT_REMARKS1,
                        DT_DATE=dateTime1,
                        DT_EXPEND=DT_EXPEND5,
                        DT_MODEL= 3005,
                        DT_NAME=DT_NAME1,
                        CRT_USER= CRT_USER1 
                    };
                    _sqlRepository.AddDA_PAYMONTH(dA_5);
                    await _sqlRepository.SaveAsync();
                    break;
                case "3006":
                    dateTime1 = Convert.ToDateTime(DT_DATE1);
                    var DT_EXPEND6 = fm["DT_EXPEND6"].ToString();
                    DA_PAYMONTH dA_6 = new DA_PAYMONTH
                    {
                        DT_CLIENTNAME = DT_CLIENTNAME1,
                        DT_REMARKS = DT_REMARKS1,
                        DT_DATE = dateTime1,
                        DT_EXPEND = DT_EXPEND6,
                        DT_MODEL = 3006,
                        DT_NAME = DT_NAME1,
                        CRT_USER = CRT_USER1
                    };
                    _sqlRepository.AddDA_PAYMONTH(dA_6);
                    await _sqlRepository.SaveAsync();
                    break;
                case "3007":
                    dateTime1 = Convert.ToDateTime(DT_DATE1);
                    var DT_EXPEND7= fm["DT_EXPEND7"].ToString();
                    DA_PAYMONTH dA_7 = new DA_PAYMONTH
                    {
                        DT_CLIENTNAME = DT_CLIENTNAME1,
                        DT_REMARKS = DT_REMARKS1,
                        DT_DATE = dateTime1,
                        DT_EXPEND = DT_EXPEND7,
                        DT_MODEL = 3007,
                        DT_NAME = DT_NAME1,
                        CRT_USER = CRT_USER1
                    };
                    _sqlRepository.AddDA_PAYMONTH(dA_7);
                    await _sqlRepository.SaveAsync();
                    break;
                case "3008":
                    dateTime1 = Convert.ToDateTime(DT_DATE1);
                    var DT_EXPEND8 = fm["DT_EXPEND8"].ToString();
                    DA_PAYMONTH dA_8 = new DA_PAYMONTH
                    {
                        DT_CLIENTNAME = DT_CLIENTNAME1,
                        DT_REMARKS = DT_REMARKS1,
                        DT_DATE = dateTime1,
                        DT_EXPEND = DT_EXPEND8,
                        DT_MODEL = 3008,
                        DT_NAME = DT_NAME1,
                        CRT_USER = CRT_USER1
                    };
                    _sqlRepository.AddDA_PAYMONTH(dA_8);
                    await _sqlRepository.SaveAsync();
                    break;
                default:
                    break;
            }
            return Ok("添加成功");
        }
        [HttpPost]
        [Route("ADDCLENTNAME")]
        public async Task<IActionResult> ADDCLENTNAME([FromForm] IFormCollection fm)
        {
            var NAME = fm["NAME"].ToString();
            var CODE = fm["CODE"].ToString();
            var PAY1 = fm["PAY1"].ToString();
            var PAY2 = fm["PAY2"].ToString();
            var PAY3 = fm["PAY3"].ToString();
            if (string.IsNullOrWhiteSpace(NAME))
            {
                return NotFound();
            }
            var getbom = await _sqlRepository.GET_CLENTNAME1(NAME);
            if (getbom == null)
            {
                var dA_CLENTNAME = new DA_CLENTNAME
                {
                    NAME = NAME,
                    PAY1 = PAY1,
                    PAY2 = PAY2,
                    PAY3 = PAY3,
                    BU=1,
                    CODE= CODE
                };
                _sqlRepository.AddDA_CLENTNAME(dA_CLENTNAME);
                await _sqlRepository.SaveAsync();
                return Ok(NAME + "保存完成！！");
            }
            else
            {
                getbom.PAY1 = PAY1;
                getbom.PAY2 = PAY2;
                getbom.PAY3 = PAY3;
                getbom.CODE = CODE;
                _sqlRepository.UpdateDA_CLENTNAME(getbom);
                await _sqlRepository.SaveAsync();
                return Ok(NAME + "更新完成！！");
            }
        }
        [HttpPost]
        [Route("ADD_ATTENDANCE")]
        public async Task<IActionResult> ADD_ATTENDANCE([FromForm] IFormCollection fm)
        {
            var ID_DATE = fm["ID_DATE"].ToString() == "" ? "" : fm["ID_DATE"].ToString().Trim();
            var ID_LINE = fm["ID_LINE"].ToString() == "" ? "" : fm["ID_LINE"].ToString().Trim();
            var ID_MANAGER = fm["ID_MANAGER"].ToString() == "" ? "" : fm["ID_MANAGER"].ToString().Trim();
            var ID_ONWORK = fm["ID_ONWORK"].ToString() == "" ? "" : fm["ID_ONWORK"].ToString().Trim();
            var ID_OFFWORK = fm["ID_OFFWORK"].ToString() == "" ? "" : fm["ID_OFFWORK"].ToString().Trim();
            var ID_OUTWORK = fm["ID_OUTWORK"].ToString() == "" ? "" : fm["ID_OUTWORK"].ToString().Trim();
            var ID_FREEWORK = fm["ID_FREEWORK"].ToString() == "" ? "" : fm["ID_FREEWORK"].ToString().Trim();
            var ID_NONWORK = fm["ID_NONWORK"].ToString() == "" ? "" : fm["ID_NONWORK"].ToString().Trim();
            var ID_CODE = fm["ID_CODE"].ToString() == "" ? "" : fm["ID_CODE"].ToString().Trim();
            var ID_MODEL = fm["ID_MODEL"].ToString() == "" ? "" : fm["ID_MODEL"].ToString().Trim();
            var ID_TIME = fm["ID_TIME"].ToString() == "" ? "" : fm["ID_TIME"].ToString().Trim();
            var ID_TUSER = fm["ID_TUSER"].ToString() == "" ? "" : fm["ID_TUSER"].ToString().Trim();
            var ID_CLASS = fm["ID_CLASS"].ToString() == "" ? "" : fm["ID_CLASS"].ToString().Trim();
            var CRT_USER = fm["CRT_USER"].ToString() == "" ? "" : fm["CRT_USER"].ToString().Trim();

            DateTime dateTime1 = Convert.ToDateTime(ID_DATE);
            var dA_CLENTNAME = new DA_ATTENDANCE
            {
                ID= Guid.NewGuid(),
                CRT_DATE =DateTime.Now,
                CRT_USER= CRT_USER,
                ID_DATE= dateTime1,
                ID_LINE= ID_LINE,
                ID_MANAGER= ID_MANAGER,
                ID_ONWORK= ID_ONWORK,
                ID_OFFWORK= ID_OFFWORK,
                ID_OUTWORK= ID_OUTWORK,
                ID_FREEWORK= ID_FREEWORK,
                ID_NONWORK= ID_NONWORK,
                ID_CODE= ID_CODE,
                ID_MODEL= ID_MODEL,
                ID_TIME= ID_TIME,
                ID_TUSER= ID_TUSER,
                ID_CLASS= ID_CLASS, 
            };
            _sqlRepository.AddDA_ATTENDANCE(dA_CLENTNAME);
            await _sqlRepository.SaveAsync();
            DT_RETURN rETURN = new DT_RETURN
            {
                Msg = "提交完成",
                Success = false,
                Service = "ADD_ATTENDANCE"
            };
            return Ok(rETURN); 
             
        }
        [HttpDelete]
        [Route("DE_PRICE")]
        public async Task<IActionResult> DE_PRICE(string Guids)
        {
            DA_BOM dA_ = await _sqlRepository.GetPrice(Guids);
            if (dA_ != null)
            {
                _sqlRepository.DeletePrice(dA_);
                await _sqlRepository.SaveAsync();
            }
            return Ok();
        }
        [HttpDelete]
        [Route("DE_CLENTNAME")]
        public async Task<IActionResult> DE_CLENTNAME(string Guids)
        {
            DA_CLENTNAME dA_ = await _sqlRepository.GET_CLENTNAME1(Guids);
            if (dA_ != null)
            {
                _sqlRepository.DeleteCLENTNAME(dA_);
                await _sqlRepository.SaveAsync();
            }
            return Ok();
        }
        [HttpDelete]
        [Route("DE_GROSS")]
        public async Task<IActionResult> DE_GROSS(string Guids)
        {
            DA_GROSS dA_=await _sqlRepository.GetGROSS(Guids);
            if (dA_!=null)
            {
                _sqlRepository.DeleteGROSS(dA_);
               await _sqlRepository.SaveAsync();  
            }
            return Ok();
        }
        [HttpDelete]
        [Route("DE_ELSEGROSS")]
        public async Task<IActionResult> DE_ELSEGROSS(string Guids)
        {
            DA_ELSEGROSS dA_ = await _sqlRepository.GetELSEGROSS(Guids);
            if (dA_ != null)
            {
                _sqlRepository.DeleteELSEGROSS(dA_);
                await _sqlRepository.SaveAsync();
            }
            return Ok();
        }
        [HttpDelete]
        [Route("DE_INSTORE")]
        public async Task<IActionResult> DE_INSTORE(string Guids)
        {
            DT_INSTORE dA_ = await _sqlRepository.GetINSTORE(Guids);
            if (dA_ != null)
            {
                _sqlRepository.DE_INSTORE(dA_);
                await _sqlRepository.SaveAsync();
            }
            return Ok();
        }
        [HttpDelete]
        [Route("DE_PLAY")]
        public async Task<IActionResult> DE_PLAY(string Guids,string MODEL)
        {
            switch (MODEL)
            {
                case "3001":
                    DA_PAYPERSON dA_ = await _sqlRepository.GetPAYPERSON(Guids);
                    _sqlRepository.DeletePAYPERSON(dA_);
                    await _sqlRepository.SaveAsync();
                    break;
                case "3002":
                    DA_PAYLOSS dA_1 = await _sqlRepository.GetPAYLOSS(Guids);
                    _sqlRepository.DeletePAYLOSS(dA_1);
                    await _sqlRepository.SaveAsync();
                    break;
                case "3003":
                    DA_PAYLOSS dA_2 = await _sqlRepository.GetPAYLOSS(Guids);
                    _sqlRepository.DeletePAYLOSS(dA_2);
                    await _sqlRepository.SaveAsync();
                    break;
                case "3004":
                    DA_PAYLOSS dA_3 = await _sqlRepository.GetPAYLOSS(Guids);
                    _sqlRepository.DeletePAYLOSS(dA_3);
                    break;
                case "3005":
                    DA_PAYMONTH dA_4 = await _sqlRepository.GetPAYMONTH(Guids);
                    _sqlRepository.DeletePAYMONTH(dA_4);
                    await _sqlRepository.SaveAsync();
                    break;
                case "3006":
                    DA_PAYMONTH dA_5 = await _sqlRepository.GetPAYMONTH(Guids);
                    _sqlRepository.DeletePAYMONTH(dA_5);
                    await _sqlRepository.SaveAsync();
                    break;
                case "3007":
                    DA_PAYMONTH dA_6 = await _sqlRepository.GetPAYMONTH(Guids);
                    _sqlRepository.DeletePAYMONTH(dA_6);
                    await _sqlRepository.SaveAsync();
                    break;
                case "3008":
                    DA_PAYMONTH dA_7 = await _sqlRepository.GetPAYMONTH(Guids);
                    _sqlRepository.DeletePAYMONTH(dA_7);
                    await _sqlRepository.SaveAsync();
                    break;
                default:
                    break;
            }
            return Ok();
        }
        [Route("ExamRoomExport")]
        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> ExamRoomExportAsync([FromForm] IFormCollection fm)
        {
            string MODEL = fm["MODEL"].ToString();
            string CLIENTNAME = fm["CLIENTNAME"].ToString();
            string DATE1 = fm["DATE1"].ToString();
            string DATE2 = fm["DATE2"].ToString();
            string FileName = string.Empty;
            string SheetName = string.Empty;
            string[] temArr = { };
            DataTable boundTable = new DataTable();
            switch (MODEL)
            {
                case "2001":
                    SheetName = "生产日报";
                    FileName = SheetName + $"{DateTime.Now:yyyyMMddHHmmssfff}.xls"; 
                    string MO = fm["MO"].ToString(); 
                    string MANAGER = fm["MANAGER"].ToString(); 
                    string[] temArr1 = { "客户类型", "名称", "工单", "编码", "单价", "入库数量", "拉长", "在册", "借入", "借出", "缺勤", "实出勤", "工时", "入库日期", "创建人" };
                    temArr = temArr1;
                    IEnumerable<DA_GROSS> dTable = await _sqlRepository.GetGROSS(MO, CLIENTNAME, MANAGER, DATE1, DATE2);
                    IEnumerable<DA_GROSSDtos> dTableDtos = _mapper.Map<IEnumerable<DA_GROSSDtos>>(dTable);
                    boundTable = API_MES.Helper.DataTableExtensions.ToDataTable(dTableDtos); 
                    break;
                case "1001":
                    SheetName = "其他收入";
                    string MOD = fm["MOD"].ToString();
                    FileName = SheetName + $"{DateTime.Now:yyyyMMddHHmmssfff}.xls";
                    string[] temArr2 = { "客户类型", "入库类型",  "入库数量", "入库日期", "创建人" };
                    temArr = temArr2;
                    IEnumerable<DA_ELSEGROSS> dTable1 = await _sqlRepository.GetELSEGROSS(CLIENTNAME, MOD, DATE1, DATE2);
                    IEnumerable<DA_ELSEGROSSDtos1> dTableDtos1 = _mapper.Map<IEnumerable<DA_ELSEGROSSDtos1>>(dTable1);
                    boundTable = API_MES.Helper.DataTableExtensions.ToDataTable(dTableDtos1);
                    break;
                case "3001":
                    SheetName = "人员支出";
                    FileName = SheetName + $"{DateTime.Now:yyyyMMddHHmmssfff}.xls";
                    string[] temArr3 = { "客户类型","支出类型", "间接人员出勤数", "间接人员月薪", "直接人员时薪","支出日期","创建人", "备注" };
                    temArr = temArr3;
                    IEnumerable<DA_PAYPERSON> dTable2 = await _sqlRepository.GetPAYPERSON(CLIENTNAME, MODEL, DATE1, DATE2);
                    IEnumerable<DA_PAYPERSONDtos1> dTableDtos2 = _mapper.Map<IEnumerable<DA_PAYPERSONDtos1>>(dTable2);
                    boundTable = API_MES.Helper.DataTableExtensions.ToDataTable(dTableDtos2);
                    break;
                case "3002":
                    SheetName = "生产辅料消耗";
                    FileName = SheetName + $"{DateTime.Now:yyyyMMddHHmmssfff}.xls";
                    string[] temArr4 = { "客户类型", "支出类型", "编码", "单价", "消耗", "支出日期", "创建人", "备注" };
                    temArr = temArr4;
                    IEnumerable<DA_PAYLOSS> dTable4 = await _sqlRepository.GetPAYLOSS(CLIENTNAME, MODEL, DATE1, DATE2);
                    IEnumerable<DA_PAYLOSSDtos1> dTableDtos4 = _mapper.Map<IEnumerable<DA_PAYLOSSDtos1>>(dTable4);
                    boundTable = API_MES.Helper.DataTableExtensions.ToDataTable(dTableDtos4);
                    break;
                case "3003":
                    SheetName = "低值易耗消耗";
                    FileName = SheetName + $"{DateTime.Now:yyyyMMddHHmmssfff}.xls";
                    string[] temArr5 = { "客户类型", "支出类型", "编码", "单价", "消耗", "支出日期", "创建人", "备注" };
                    temArr = temArr5;
                    IEnumerable<DA_PAYLOSS> dTable5 = await _sqlRepository.GetPAYLOSS(CLIENTNAME, MODEL, DATE1, DATE2);
                    IEnumerable<DA_PAYLOSSDtos1> dTableDtos5 = _mapper.Map<IEnumerable<DA_PAYLOSSDtos1>>(dTable5);
                    boundTable = API_MES.Helper.DataTableExtensions.ToDataTable(dTableDtos5);
                    break;
                case "3004":
                    SheetName = "超损物料损耗";
                    FileName = SheetName + $"{DateTime.Now:yyyyMMddHHmmssfff}.xls";
                    string[] temArr6 = { "客户类型", "支出类型", "编码", "单价", "消耗", "支出日期", "创建人", "备注" };
                    temArr = temArr6;
                    IEnumerable<DA_PAYLOSS> dTable6 = await _sqlRepository.GetPAYLOSS(CLIENTNAME, MODEL, DATE1, DATE2);
                    IEnumerable<DA_PAYLOSSDtos1> dTableDtos6 = _mapper.Map<IEnumerable<DA_PAYLOSSDtos1>>(dTable6);
                    boundTable = API_MES.Helper.DataTableExtensions.ToDataTable(dTableDtos6);
                    break;
                case "3005":
                    SheetName = "平台部门费用支出";
                    FileName = SheetName + $"{DateTime.Now:yyyyMMddHHmmssfff}.xls";
                    string[] temArr7 = { "客户类型", "支出类型", "支出", "支出日期", "创建人", "备注" };
                    temArr = temArr7;
                    IEnumerable<DA_PAYMONTH> dTable7 = await _sqlRepository.GetPAYMONTH(CLIENTNAME, MODEL, DATE1, DATE2);
                    IEnumerable<DA_PAYMONTHDtos1> dTableDtos7 = _mapper.Map<IEnumerable<DA_PAYMONTHDtos1>>(dTable7);
                    boundTable = API_MES.Helper.DataTableExtensions.ToDataTable(dTableDtos7);
                    break;
                case "3006":
                    SheetName = "公摊费用支出";
                    FileName = SheetName + $"{DateTime.Now:yyyyMMddHHmmssfff}.xls";
                    string[] temArr8 = { "客户类型", "支出类型", "支出", "支出日期", "创建人", "备注" };
                    temArr = temArr8;
                    IEnumerable<DA_PAYMONTH> dTable8 = await _sqlRepository.GetPAYMONTH(CLIENTNAME, MODEL, DATE1, DATE2);
                    IEnumerable<DA_PAYMONTHDtos1> dTableDtos8 = _mapper.Map<IEnumerable<DA_PAYMONTHDtos1>>(dTable8);
                    boundTable = API_MES.Helper.DataTableExtensions.ToDataTable(dTableDtos8);
                    break;
                case "3007":
                    SheetName = "其他支出";
                    FileName = SheetName + $"{DateTime.Now:yyyyMMddHHmmssfff}.xls";
                    string[] temArr9 = { "客户类型", "支出类型", "支出", "支出日期", "创建人", "备注" };
                    temArr = temArr9;
                    IEnumerable<DA_PAYMONTH> dTable9 = await _sqlRepository.GetPAYMONTH(CLIENTNAME, MODEL, DATE1, DATE2);
                    IEnumerable<DA_PAYMONTHDtos1> dTableDtos9 = _mapper.Map<IEnumerable<DA_PAYMONTHDtos1>>(dTable9);
                    boundTable = API_MES.Helper.DataTableExtensions.ToDataTable(dTableDtos9);
                    break;
                default:
                    break;
            }  
            List<string> lstTitle = new List<string>(temArr); 
            IWorkbook book = new HSSFWorkbook();
            //ISheet sheet = book.CreateSheet("Sheet1");
            ISheet sheet = book.CreateSheet(SheetName);
            IRow rowTitle = sheet.CreateRow(0);
            ICellStyle style = book.CreateCellStyle(); 
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//垂直居中
            NPOI.SS.UserModel.ICellStyle headStyle = book.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.Center;
            IFont font = book.CreateFont();
            font.FontHeightInPoints = 10;
            font.Boldweight = 700;
            headStyle.SetFont(font);
            for (int i = 0; i < lstTitle.Count; i++)
            { 
                rowTitle.CreateCell(i).SetCellValue(lstTitle[i]);
                rowTitle.GetCell(i).CellStyle = headStyle;
            } 
            for(int i = 0; i < boundTable.Rows.Count; i++)
                    {
                IRow row = sheet.CreateRow(i + 1);
                DataRow dr = boundTable.Rows[i];
                for (int j = 0; j < boundTable.Columns.Count; j++)
                {
                    ICell cell = row.CreateCell(j);
                    cell.CellStyle = style;
                    cell.SetCellValue(dr[j].ToString());
                }
            }
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            //return File(ms, "application/vnd.ms-excel", "考场总表导出.xls");
            return File(ms, "application/vnd.ms-excel", FileName);
        }
        [HttpPost("DownExcel")]
        public async Task<IActionResult> DownExcel([FromForm] IFormCollection fm)
        {
            string DATE1 = fm["DATE1"].ToString();
           // DATE1 = "2020-04";
            string CLIENTNAME = fm["CLIENTNAME"].ToString();
            IEnumerable<DA_VIEW> rsVal = await _sqlRepository.getDA_VIEW(DATE1, CLIENTNAME);
            //_ = new List<DA_VIEW>();
            //List<DA_VIEW> ListVIEW = rsVal.ToList();
            if (new List<DA_VIEW>() == null)
            {
                return Ok();
            }
            DateTime date1 = Convert.ToDateTime(DATE1);
            //int pYear = date1.Year;
            int pMonth = date1.Month;
            // IEnumerable<Student> list = Student.GetStudents();
            ExcelHelper excelHeper = new ExcelHelper();
            List<ExcelGridModel> config = new List<ExcelGridModel> {
                 new ExcelGridModel{name="TYPES",label="类别", align="left",},
                 new ExcelGridModel{name="TYPES1",label="类别", align="left",},
                 new ExcelGridModel{name="MODEL",label="机型", align="left",},
                 new ExcelGridModel{name="DT_1",label=pMonth+"月1号", align="left",},
                 new ExcelGridModel{name="DT_2",label=pMonth+"月2号", align="left",},
                 new ExcelGridModel{name="DT_3",label=pMonth+"月3号", align="left",},
                 new ExcelGridModel{name="DT_4",label=pMonth+"月4号", align="left",},
                 new ExcelGridModel{name="DT_5",label=pMonth+"月5号", align="left",},
                 new ExcelGridModel{name="DT_6",label=pMonth+"月6号", align="left",},
                 new ExcelGridModel{name="DT_7",label=pMonth+"月7号", align="left",},
                 new ExcelGridModel{name="DT_8",label=pMonth+"月8号", align="left",},
                 new ExcelGridModel{name="DT_9",label=pMonth+"月9号", align="left",},
                 new ExcelGridModel{name="DT_10",label=pMonth+"月10号", align="left",},
                 new ExcelGridModel{name="DT_11",label=pMonth+"月11号", align="left",},
                 new ExcelGridModel{name="DT_12",label=pMonth+"月12号", align="left",},
                 new ExcelGridModel{name="DT_13",label=pMonth+"月13号", align="left",},
                 new ExcelGridModel{name="DT_14",label=pMonth+"月14号", align="left",},
                 new ExcelGridModel{name="DT_15",label=pMonth+"月15号", align="left",},
                 new ExcelGridModel{name="DT_16",label=pMonth+"月16号", align="left",},
                 new ExcelGridModel{name="DT_17",label=pMonth+"月17号", align="left",},
                 new ExcelGridModel{name="DT_18",label=pMonth+"月18号", align="left",},
                 new ExcelGridModel{name="DT_19",label=pMonth+"月19号", align="left",},
                 new ExcelGridModel{name="DT_20",label=pMonth+"月20号", align="left",},
                 new ExcelGridModel{name="DT_21",label=pMonth+"月21号", align="left",},
                 new ExcelGridModel{name="DT_22",label=pMonth+"月22号", align="left",},
                 new ExcelGridModel{name="DT_23",label=pMonth+"月23号", align="left",},
                 new ExcelGridModel{name="DT_24",label=pMonth+"月24号", align="left",},
                 new ExcelGridModel{name="DT_25",label=pMonth+"月25号", align="left",},
                 new ExcelGridModel{name="DT_26",label=pMonth+"月26号", align="left",},
                 new ExcelGridModel{name="DT_27",label=pMonth+"月27号", align="left",},
                 new ExcelGridModel{name="DT_28",label=pMonth+"月28号", align="left",},
                 new ExcelGridModel{name="DT_29",label=pMonth+"月29号", align="left",},
                 new ExcelGridModel{name="DT_30",label=pMonth+"月30号", align="left",},
                 new ExcelGridModel{name="DT_31",label=pMonth+"月31号", align="left",},
                 new ExcelGridModel{name="TOTAL",label="本月累计", align="left",},
            };
            var fileName = fm["FileName"].ToString();
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "nOName.xls";
            } 
            return excelHeper.ExcelDownload(rsVal, config, fileName);
           // return File(excelHeper.ExcelMemory(rsVal, config, fileName), "application/vnd.ms-excel", fileName);
        }
    }
}
