using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API_MES.Entities;
using EF_ORACLE;
using EF_ORACLE.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using API_MES.Servise;
using API_MES.Dtos;
using AutoMapper;
using EF_ORACLE.Model.TMES;
using Microsoft.AspNetCore.Mvc;
using API_MES.Helper;
using Microsoft.AspNetCore.Cors;
using System.IO;
using System.Data;

namespace API_MES.Controllers
{
    //Take()方法的作用是从查询结果中提取前n个结果；而Skip()方法则是跳过前n个结果，返回剩余的结果
    //[Authorize]
    /// <summary>
    /// MESController
    /// </summary>
    [EnableCors("any")]
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SkipActionFilter]
    public class MESController : ControllerBase
    {
        //EF中实现left join（左联接）查询 https://www.jianshu.com/p/9475f176d581
        private readonly T100Context _t100Context;
        private readonly IMesRepository _imesRepository;
        private readonly ILogger<MESController> _logger;
        private readonly IMapper _mapper; 
        public MESController(IMapper mapper,
                             T100Context t100Context,
                             IMesRepository MesRepository,
                             ILogger<MESController> logger )
        {
            _logger = logger;
            _t100Context = t100Context ?? throw new ArgumentNullException(nameof(t100Context));
            _imesRepository = MesRepository ?? throw new ArgumentNullException(nameof(MesRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); 
        }
        // GET: api/MES
        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
      
        public async Task<IActionResult> Get()
        {
            //var list = from o in context.CTMS_OD_ORDERS
            //           join d in context.CTMS_SUP_DOCTOR
            //           on o.OWNERDOCID equals d.USERID
            //           join e in context.CTMS_OD_ORDERSEVALUATION
            //           on o.ORDERID equals e.ORDERID
            //           select o;`
            //var dd = AppContext.FQA_FIRSTARTICLELINE.ToList();
            //var lastResult =   _t100Context.iMAA_Ts.Where(x=>x.IMAA001== MO).ToList();
            //var lastResult = from q in _t100Context.sFBA_Ts.Where(x => x.SFBADOCNO == MO)
            //                 from p in _t100Context.iMAA_Ts
            //                 where (p.IMAA001 == q.SFBA006 && q.SFBAENT == p.IMAAENT)
            //                 select new { q.SFBA006, p.IMAAUD009, p.IMAAUD010 };
            //var dds = await lastResult.ToListAsync();
            var ss = "8S%SB18C51767%9421056143ATL19BF------------NVT6AA6874";
            //var lastResult = from p in _t100Context.students
            //                 from q in _t100Context.userTables.Where(x => x.USID == 34)O
            //                 where p.Name == q.USNAME
            //                 select new { sd = p.UserId, q.USNAME, q.USID };
            //var lastResult = from p in _t100Context.students
            //                 from q in _t100Context.userTables.Where(x => x.USID == 34)
            //                 where p.Name == q.USNAME
            //                 select new { sd = p.UserId, q.USNAME, q.USID };
            //var lastResult = from p in A
            //                 from q in B.GroupBy(x => x.AID).Select(x => new { Key = x.Key, Value = string.Join(",", B.Where(y => y.AID == x.Key).Select(y => y.BName)) })
            //                 where p.AID == q.Key
            //                 select new
            //                 {
            //                     CLASS = p.Class,
            //                     Name = q.Value,
            //                 };
            //var dd = lastResult.ToList();
            ss = "9FT9K20731911395";
            //UserInfor.OracleConnection
            //DataTable dAtable = OracleHelper.ExecuteDataTable(UserInfor.OracleConnection, "SELECT PHYSICSNO,WORKORDER,SN,IMEI2,MEID FROM ODM_IMEILINKNETCODE T WHERE T.SN='" + ss + "'");
            //if (dAtable.Rows.Count > 0)
            //{

            //}
            var res = await _imesRepository.GetSNBY_IMEILINKNETCODE(ss);

            return Ok(res);
            //return new string[] { "value1", "value2", ss.Length.ToString() };
        }
        [HttpGet]
        [Route("GetMidTIME")]
        public async Task<IActionResult> GetMidTime(string Switch)
        {
            //var correlationId = HttpContext.Request.Headers["x-correlation-id"].ToString();
            string Msg = string.Empty;
            if (string.IsNullOrEmpty(Switch))
            {
                Msg = "传入值为空！";
                return Ok(new JsonResult(new { Err = Msg, success = false }));
            }
            Newtonsoft.Json.Linq.JObject jobject = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(Switch);
            string ProcessGUID = jobject.Properties().Any(p => p.Name == "ProcessGUID") ? jobject["ProcessGUID"].ToString() : "";
            string TPROCESS = jobject.Properties().Any(p => p.Name == "TPROCESS") ? jobject["TPROCESS"].ToString() : "";
            string ITEMCODE = jobject.Properties().Any(p => p.Name == "ITEMCODE") ? jobject["ITEMCODE"].ToString() : "";
            string DTIME1 = jobject.Properties().Any(p => p.Name == "DTIME1") ? jobject["DTIME1"].ToString() : "";
            string DTIME2 = jobject.Properties().Any(p => p.Name == "DTIME2") ? jobject["DTIME2"].ToString() : "";
            var listCt = await _imesRepository.GetMidTimeAsync(ITEMCODE, TPROCESS, DTIME1, DTIME2);
            #region
            //var listCt = AppContext.MidTimes.ToList();
            //if (jobject.Property("ITEMCODE") != null)
            //{
            //    if (!string.IsNullOrEmpty(ITEMCODE))
            //    {
            //        listCt = listCt.Where(x => x.ITEMCODE == ITEMCODE).ToList();
            //    }
            //}
            //if (jobject.Property("TPROCESS") != null)
            //{
            //    if (!string.IsNullOrEmpty(TPROCESS))
            //    {
            //        listCt = listCt.Where(x => x.DTYPE == Convert.ToInt32(TPROCESS)).ToList();
            //    }

            //}
            //if (jobject.Property("DTIME1") != null)
            //{
            //    if (!string.IsNullOrEmpty(DTIME1))
            //        listCt = listCt.Where(x => x.INPUTDATE >= Convert.ToDateTime(DTIME1) && x.INPUTDATE <= Convert.ToDateTime(DTIME2)).ToList();
            //    //.Where(c => c.Date > dtToday && c.Date < dtNexDay)
            //    //Students = Students.Where(s => s.Name.Substring(0, 1) >= "a"
            //    //        && s.Name.Substring(0, 1) <= "e").ToList();
            //}
            #endregion
            var lss = listCt.Count();
            if (lss > 0)
            {
                try
                {
                    //var sames = (from a in listCt orderby a.INPUTDATE select new { a.ITEMCODE, a.DLTIME, INPUTDATE = a.INPUTDATE.Value.ToString("yyyy-MM-dd HH:mm:ss"), a.DTYPE }).ToList();
                    var lastResult = _mapper.Map<IEnumerable<FQA_MIDTIMEDto>>(listCt);
                    return Ok(lastResult);

                }
                catch (Exception ex)
                {
                    Msg = ex.Message;
                    return Ok(new JsonResult(new { Err = Msg, success = false }));
                }

            }
            else
            {
                Msg = "查询完成，无数据记录！";
                return Ok(new JsonResult(new { Err = Msg, success = false }));
            }
        }
        [HttpGet]
        [Produces("application/json",
            "application/vnd.company.hateoas+json",
            "application/vnd.company.company.friendly+json",
            "application/vnd.company.company.friendly.hateoas+json",
            "application/vnd.company.company.full+json",
            "application/vnd.company.company.full.hateoas+json")]
        [Route("GetMo")]
        public async Task<ActionResult<ModelDot>> GetMO(string mo)
        {
            #region
            //var listCt = AppContext.ModelMOs.Where(x => x.WORKJOB_CODE.Contains(mo)).Count();
            //_logger.LogInformation("接口：GetMO"); //写入LOG，
            //if (listCt > 0)
            //{
            //    //var query = await (from b in _appContext.ModelMOs where b.WORKJOB_CODE == mo select b).ToListAsync();
            //    var sames = AppContext.ModelMOs.Where(x => x.WORKJOB_CODE.Contains(mo)).OrderByDescending(x => x.INPUTDATE).Take(50).ToList().Select(d => new
            //    {
            //        d.WORKJOB_CODE,
            //        d.CLIENTNAME,
            //        d.ITEM_CODE,
            //        d.ITEM_NUM,
            //        INPUTDATE = d.INPUTDATE.Value.ToString("yyyy-MM-dd HH:mm:ss")
            //    });
            //    return Ok(sames);
            //}
            //else
            //{
            //    var sames = AppContext.ModelMOs.OrderByDescending(x => x.INPUTDATE).Take(50).ToList().Select(d => new
            //    {
            //        d.WORKJOB_CODE,
            //        d.CLIENTNAME,
            //        d.ITEM_CODE,
            //        d.ITEM_NUM,
            //        INPUTDATE = d.INPUTDATE.Value.ToString("yyyy-MM-dd HH:mm:ss")
            //    });
            //    return Ok(sames);
            //}
            #endregion
            if (!string.IsNullOrEmpty(mo))
            {
                mo = mo.Trim();
            }
            var Model = await _imesRepository.GetMO(mo);
            // var lastResult = _mapper.Map<IEnumerable<ModelDot>>(Model); 
            return Ok(_mapper.Map<IEnumerable<ModelDot>>(Model));
        }
        [HttpGet]
        [Route("FQA_OBA_PASS")]
        public IActionResult FQA_OBA_PASS(string ID)
        { 
            return Ok(_imesRepository.FQA_OBA_PASS(ID));
        }
        [HttpGet]
        [Route("GetLCode")]
        public async Task<IActionResult> GetLCode(string nb)
        {
            int nb1 = 0;
            if (!string.IsNullOrEmpty(nb))
            {
                nb1 = Convert.ToInt32(nb);
            }
            var lastResult = await _imesRepository.GetLINE(nb1);
            var rtnResult = _mapper.Map<IEnumerable<LineDtos>>(lastResult);
            return Ok(rtnResult);
        }
        [HttpGet]
        [Route("GetLINE")]
        public async Task<IActionResult> GetLINE()
        {
            var lastResult = await _imesRepository.GetLINE(1);
            return Ok(lastResult);
        }
        [HttpGet]
        [Route("GetLINE1")]
        public async Task<IActionResult> GetLINE1()
        {
            var lastResult = await _imesRepository.GetLINE(2);
            var rtnResult = _mapper.Map<IEnumerable<LineDtos>>(lastResult);
            return Ok(rtnResult);
        }
        [HttpGet]
        [Route("GetBELINE")]
        public async Task<IActionResult> GetBELINE()
        {
            var lastResult = await _imesRepository.GetBEATELINE(0);
            var rtnResult = _mapper.Map<IEnumerable<ODM_BEATEDtos>>(lastResult.OrderBy(x=>x.LINECODE));
            return Ok(rtnResult);
        }
        [HttpGet]
        [Route("GetBEATELINEMODEL")]
        public async Task<IActionResult> GetBEATELINEMODEL()
        {
            var lastResult = await _imesRepository.GetBEATELINEMODEL(); 
            return Ok(lastResult);
        }
        [HttpGet]
        [Route("GetSNBY_IMEILINKNETCODE")]
        public async Task<IActionResult> GetSNBY_IMEILINKNETCODE(string SN)
        {
            var lastResult = await _imesRepository.GetSNBY_IMEILINKNETCODE(SN);
            return Ok(lastResult);
        }
        [HttpGet]
        [Route("GetSNmodel")]
        public async Task<IActionResult> GetSNmodel(string SN,int Ty)
        {
            BARCODEREMP bARCODEREMP = new BARCODEREMP();
            bARCODEREMP =await _imesRepository.GetBARCODEREMP("", SN);
            if (bARCODEREMP==null)
            {
                ErrMessage errMessage = new ErrMessage
                {
                    Err = SN + "不存在",
                    success = false
                };
                return Ok(errMessage);
            } 
            var Model = await _imesRepository.GetMO(bARCODEREMP.WORKCODE); 
            return Ok(_mapper.Map<IEnumerable<ModelDot>>(Model)); 
        }
        [HttpGet]
        [Route("GetImeiTop")]
        public async Task<IActionResult> GetImeiTop(string mo)
        {
            if (!string.IsNullOrEmpty(mo))
            {
                mo = mo.Trim();
            }
            var lastResult = await _imesRepository.GetLISTIMEIL(mo);
            if (lastResult != null)
            {
                var lastResult1 = _mapper.Map<IEnumerable<ODM_IMEILINKNETCODEDtos>>(lastResult);
                var lastResult2 = lastResult1.DistinctBy(p => p.PHYSICSNO);
                //var lastResult3 = lastResult1.GroupBy(p => new { p.PHYSICSNO }).Select(g => g.First()).ToList();
                return Ok(lastResult2);
            }
            return Ok();
        }
        [HttpGet]
        [Route("ListFQA_OBARESHUOTIME")]
        public async Task<IActionResult> ListFQA_OBARESHUOTIME(string BOM,string DATE1,string DATE2)
        {
            if (!string.IsNullOrEmpty(BOM))
            {
                BOM = BOM.Trim();
            }
            return Ok(await _imesRepository.ListFQA_OBARESHUOTIME(BOM, DATE1, DATE2));
        }
        [HttpGet]
        [Route("GetFQA_OBARESHUOTIME")]
        public async Task<IActionResult> GetFQA_OBARESHUOTIME(string bom)
        {
            if (!string.IsNullOrEmpty(bom))
            {
                bom = bom.Trim();
            }
            var lastResult = await _imesRepository.GetFQA_OBARESHUOTIME(bom); 
            return Ok(lastResult);
        }
        [HttpGet]
        [Route("GetFQA_OBAITEMTIME")]
        public async Task<IActionResult> GetFQA_OBAITEMTIME(string bom)
        {
            if (!string.IsNullOrEmpty(bom))
            {
                bom = bom.Trim();
            }
            var lastResult = await _imesRepository.GetFQA_OBAITEMTIME(bom);
            return Ok(lastResult);
        }
        [HttpGet]
        [Route("GetBARCODEREMP")]
        public async Task<IActionResult> GetBARCODEREMP(string mo, string sn)
        {
            if (!string.IsNullOrEmpty(mo))
            {
                mo = mo.Trim();
            }
            if (!string.IsNullOrEmpty(sn))
            {
                sn = sn.Trim();
            }
            var lastResult = await _imesRepository.GetBARCODEREMP(mo, sn);
            if (lastResult == null)
            {
                ODM_IMEILINKNETCODE lis = await _imesRepository.GetODM_IMEILINKNETCODE(mo, sn);
                //if (lis==null)
                //{
                //    return Ok(lis);
                //} 
                // IMEILNTOBARCODEDtos someDomainModel = _mapper.DynamicMap<ODM_IMEILINKNETCODE, IMEILNTOBARCODEDtos>(lis);
                var employeeDto = _mapper.Map<IMEILNTOBARCODEDtos>(lis);
                return Ok(employeeDto);
            }
            return Ok(lastResult);
        }

        [HttpGet]
        [Route("GetODM_MODEL")]
        public async Task<IActionResult> GetODM_MODEL(string bom)
        {
            var employeeDto = await _imesRepository.GetODM_MODEL(bom);
            return Ok(employeeDto);
        }
        [HttpGet]
        [Route("GetFIRST_MO")]
        public async Task<IActionResult> GetFIRST_MO(string MO, string LINE, string IUSER)
        {
            if (!string.IsNullOrEmpty(MO))
            {
                MO = MO.Trim();
            }

            var dd = await _imesRepository.GetICMo1(MO);
            int ct = dd.Count();
            var qq = await _imesRepository.GetICMo2(MO, LINE);
            var ds = from p in dd
                     from q in qq
                     where p.MO == q.MO && p.MTYPE == q.MTYPE && p.MODEL == q.MODEL
                     select (new { p.MO, p.MTYPE });
            int ct1 = ds.Count();
            int ct2 = qq.Count();
            if (ct != ct1)
            {
                var notIn = from a in dd
                            where !(from b in ds select b.MTYPE).Contains(a.MTYPE)
                            select a;
                var ds2 = dd.Where(a => !ds.Select(b => b.MTYPE).Contains(a.MTYPE) && ds.Select(b => b.MO).Contains(a.MO));
                //var ds2 = from p in dd
                //          from q in ds
                //          where (p.MO == q.MO && p.MTYPE!= q.MTYPE)
                //          select (new { p.MO, p.MTYPE });
                var dd1 = ds2.ToList();
                var Log = string.Empty;
                if (dd1.Count == 0)
                {
                    var t1 = dd.Where(x => x.MTYPE == 20).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-整机Logo-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 1).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-软件版本-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 2).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-充电器-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 5).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-数据线-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 7).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-彩盒-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 9).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-彩盒标签-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 3).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-说明书-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 6).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-耳机-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 8).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-PC壳-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 10).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-中箱标签-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 11).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-三联多联定制标签-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 12).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-保护膜-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 13).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-卖点膜-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 15).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-机身定制标签-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 16).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-技改-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 17).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-EC-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 18).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-彩盒腰封-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 19).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-卡针-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 14).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-电池盖保护膜-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 4).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-保修卡-";
                    }

                    t1 = dd.Where(x => x.MTYPE == 21).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-其它01-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 22).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-其它02-";
                    }
                }
                else
                {
                    var t1 = ds2.Where(x => x.MTYPE == 20).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-整机Logo-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 1).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-软件版本-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 2).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-充电器-";
                    }

                    t1 = ds2.Where(x => x.MTYPE == 5).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-数据线-";
                    }

                    t1 = ds2.Where(x => x.MTYPE == 7).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-彩盒-";
                    }

                    t1 = ds2.Where(x => x.MTYPE == 9).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-彩盒标签-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 3).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-说明书-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 6).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-耳机-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 8).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-PC壳-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 10).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-中箱标签-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 11).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-三联多联定制标签-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 12).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-保护膜-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 13).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-卖点膜-";
                    }

                    t1 = ds2.Where(x => x.MTYPE == 15).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-机身定制标签-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 16).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-技改-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 17).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-EC-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 18).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-彩盒腰封-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 19).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-卡针-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 14).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-电池盖保护膜-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 4).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-保修卡-";
                    }

                    t1 = ds2.Where(x => x.MTYPE == 21).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-其它01- ";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 22).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-其它02-";
                    }
                }
                if (ct2 != 0)
                {
                    await IS_ODMLOCKMO(MO, LINE, 0, IUSER);
                }
                return Ok(new JsonResult(new { Err = Log, success = false }));
            }
            if (ct2 != 0)
            {
                await IS_ODMLOCKMO(MO, LINE, 1, IUSER);
            }
            return Ok(new JsonResult(new { Err = "OK", success = true }));
        }
        [HttpGet]
        [Route("GetFIRST_MO1")]
        public async Task<IActionResult> GetFIRST_MO1(string MO, string LINE, string IUSER, string SN)
        {
            if (!string.IsNullOrEmpty(MO))
            {
                MO = MO.Trim();
            }
            var dd = await _imesRepository.GetICMo1(MO);
            int ct = dd.Count();
            var qq = await _imesRepository.GetICMo2(MO, LINE);
            var ds = from p in dd
                     from q in qq
                     where p.MO == q.MO && p.MTYPE == q.MTYPE && p.MODEL == q.MODEL
                     select (new { p.MO, p.MTYPE });
            int ct1 = ds.Count();
            int ct2 = qq.Count();
            if (ct != ct1)
            {
                var notIn = from a in dd
                            where !(from b in ds select b.MTYPE).Contains(a.MTYPE)
                            select a;
                var ds2 = dd.Where(a => !ds.Select(b => b.MTYPE).Contains(a.MTYPE) && ds.Select(b => b.MO).Contains(a.MO));
                var dd1 = ds2.ToList();
                var Log = string.Empty;
                if (dd1.Count == 0)
                {
                    var t1 = dd.Where(x => x.MTYPE == 20).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-整机Logo-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 1).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-软件版本-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 2).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-充电器-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 5).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-数据线-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 7).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-彩盒-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 9).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-彩盒标签-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 3).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-说明书-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 6).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-耳机-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 8).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-PC壳-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 10).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-中箱标签-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 11).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-三联多联定制标签-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 12).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-保护膜-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 13).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-卖点膜-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 15).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-机身定制标签-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 16).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-技改-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 17).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-EC-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 18).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-彩盒腰封-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 19).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-卡针-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 14).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-电池盖保护膜-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 4).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-保修卡-";
                    }

                    t1 = dd.Where(x => x.MTYPE == 21).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-其它01-";
                    }
                    t1 = dd.Where(x => x.MTYPE == 22).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-其它02-";
                    }
                }
                else
                {
                    var t1 = ds2.Where(x => x.MTYPE == 20).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-整机Logo-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 1).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-软件版本-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 2).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-充电器-";
                    }

                    t1 = ds2.Where(x => x.MTYPE == 5).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-数据线-";
                    }

                    t1 = ds2.Where(x => x.MTYPE == 7).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-彩盒-";
                    }

                    t1 = ds2.Where(x => x.MTYPE == 9).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-彩盒标签-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 3).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-说明书-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 6).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-耳机-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 8).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-PC壳-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 10).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-中箱标签-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 11).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-三联多联定制标签-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 12).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-保护膜-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 13).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-卖点膜-";
                    }

                    t1 = ds2.Where(x => x.MTYPE == 15).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-机身定制标签-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 16).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-技改-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 17).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-EC-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 18).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-彩盒腰封-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 19).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-卡针-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 14).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-电池盖保护膜-";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 4).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-保修卡-";
                    }

                    t1 = ds2.Where(x => x.MTYPE == 21).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-其它01- ";
                    }
                    t1 = ds2.Where(x => x.MTYPE == 22).ToList();
                    if (t1.Count != 0)
                    {
                        Log += "-其它02-";
                    }
                }
                if (ct2 != 0)
                {
                    await IS_ODMLOCKMO1(MO, LINE, 0, IUSER, SN);
                    var lastResult = _mapper.Map<IEnumerable<FQA_FIRSTARTICLELINELOG>>(qq);
                    foreach (var item in lastResult)
                    {
                        _imesRepository.AddFQA_FIRSTARTICLELINELOG(item);
                    }
                    await _imesRepository.SaveAsync();

                }
                return Ok(new JsonResult(new { Err = Log, success = false }));
            }
            if (ct2 != 0)
            {
                await IS_ODMLOCKMO1(MO, LINE, 1, IUSER, SN);
                var lastResult = _mapper.Map<IEnumerable<FQA_FIRSTARTICLELINELOG>>(qq);
                foreach (var item in lastResult)
                {
                    _imesRepository.AddFQA_FIRSTARTICLELINELOG(item);
                }
                await _imesRepository.SaveAsync();
            }
            return Ok(new JsonResult(new { Err = "OK", success = true }));
        }
        [HttpGet]
        [Route("GetFIRSTARTICLE")]
        //public async Task<IActionResult> GetFIRSTARTICLE(string mo)
        public async Task<IActionResult> GetFIRSTARTICLE(string mo)
        {
            string Msg = "";
            if (!string.IsNullOrEmpty(mo))
            {
                mo = mo.Trim();
            }
            var LView = await _imesRepository.GetICMo1(mo.Trim());
            string I_TS = LView.Where(u => u.MTYPE == 1 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_CA = LView.Where(u => u.MTYPE == 2 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_BOOK = LView.Where(u => u.MTYPE == 3 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_RE = LView.Where(u => u.MTYPE == 4 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_USB = LView.Where(u => u.MTYPE == 5 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_HDS = LView.Where(u => u.MTYPE == 6 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_GP = LView.Where(u => u.MTYPE == 7 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_PC = LView.Where(u => u.MTYPE == 8 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_GB = LView.Where(u => u.MTYPE == 9 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_MP = LView.Where(u => u.MTYPE == 10 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_3DZ = LView.Where(u => u.MTYPE == 11 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_PROF = LView.Where(u => u.MTYPE == 12 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_MDM = LView.Where(u => u.MTYPE == 13 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_DCP = LView.Where(u => u.MTYPE == 14 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_MAC = LView.Where(u => u.MTYPE == 15 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_JEC = LView.Where(u => u.MTYPE == 16 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_EC = LView.Where(u => u.MTYPE == 17 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_GF = LView.Where(u => u.MTYPE == 18 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_KZ = LView.Where(u => u.MTYPE == 19 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_LOG = LView.Where(u => u.MTYPE == 20 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_EL1 = LView.Where(u => u.MTYPE == 21 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_EL2 = LView.Where(u => u.MTYPE == 22 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_NOTES1 = LView.Where(u => u.MTYPE == 1 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES2 = LView.Where(u => u.MTYPE == 2 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES3 = LView.Where(u => u.MTYPE == 3 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES4 = LView.Where(u => u.MTYPE == 4 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES5 = LView.Where(u => u.MTYPE == 5 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES6 = LView.Where(u => u.MTYPE == 6 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES7 = LView.Where(u => u.MTYPE == 7 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES8 = LView.Where(u => u.MTYPE == 8 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES9 = LView.Where(u => u.MTYPE == 9 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES10 = LView.Where(u => u.MTYPE == 10 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES11 = LView.Where(u => u.MTYPE == 11 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES12 = LView.Where(u => u.MTYPE == 12 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES13 = LView.Where(u => u.MTYPE == 13 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES14 = LView.Where(u => u.MTYPE == 14 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES15 = LView.Where(u => u.MTYPE == 15 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES16 = LView.Where(u => u.MTYPE == 16 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES17 = LView.Where(u => u.MTYPE == 17 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES18 = LView.Where(u => u.MTYPE == 18 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES19 = LView.Where(u => u.MTYPE == 19 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES20 = LView.Where(u => u.MTYPE == 20 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES21 = LView.Where(u => u.MTYPE == 21 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES22 = LView.Where(u => u.MTYPE == 22 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            //});
            if (string.IsNullOrEmpty(I_TS) && string.IsNullOrEmpty(I_CA) && string.IsNullOrEmpty(I_BOOK) && string.IsNullOrEmpty(I_RE) && string.IsNullOrEmpty(I_USB)
                && string.IsNullOrEmpty(I_HDS) && string.IsNullOrEmpty(I_GP) && string.IsNullOrEmpty(I_PC) && string.IsNullOrEmpty(I_GB) && string.IsNullOrEmpty(I_MP)
                && string.IsNullOrEmpty(I_3DZ) && string.IsNullOrEmpty(I_PROF) && string.IsNullOrEmpty(I_MDM) && string.IsNullOrEmpty(I_DCP) && string.IsNullOrEmpty(I_MAC)
                && string.IsNullOrEmpty(I_JEC) && string.IsNullOrEmpty(I_EC) && string.IsNullOrEmpty(I_GF) && string.IsNullOrEmpty(I_KZ) && string.IsNullOrEmpty(I_LOG)
                && string.IsNullOrEmpty(I_EL1) && string.IsNullOrEmpty(I_EL2))
            {
                Msg = "查询无数据记录！！";
                return Ok(new JsonResult(new { Err = Msg, success = false }));
            }
            else
            {
                #region
                return Ok(new JsonResult(new
                {
                    I_TS,
                    I_CA,
                    I_BOOK,
                    I_RE,
                    I_USB,
                    I_HDS,
                    I_GP,
                    I_PC,
                    I_GB,
                    I_MP,
                    I_3DZ,
                    I_PROF,
                    I_MDM,
                    I_DCP,
                    I_MAC,
                    I_JEC,
                    I_EC,
                    I_GF,
                    I_KZ,
                    I_LOG,
                    I_EL1,
                    I_EL2,
                    I_NOTES1,
                    I_NOTES2,
                    I_NOTES3,
                    I_NOTES4,
                    I_NOTES5,
                    I_NOTES6,
                    I_NOTES7,
                    I_NOTES8,
                    I_NOTES9,
                    I_NOTES10,
                    I_NOTES11,
                    I_NOTES12,
                    I_NOTES13,
                    I_NOTES14,
                    I_NOTES15,
                    I_NOTES16,
                    I_NOTES17,
                    I_NOTES18,
                    I_NOTES19,
                    I_NOTES20,
                    I_NOTES21,
                    I_NOTES22,
                    success = true
                }));
                #endregion
            }
        }
        [HttpGet]
        [Route("GetFIRSTARTICLELINE")]
        //public async Task<IActionResult> GetFIRSTARTICLE(string mo) GetSNBY_IMEILINKNETCODE
        public async Task<IActionResult> GetFIRSTARTICLELINE(string MO, string LINE)
        {
            if (!string.IsNullOrEmpty(MO))
            {
                MO = MO.Trim();
            }
            if (!string.IsNullOrEmpty(LINE))
            {
                LINE = LINE.Trim();
            }
            string Msg = "";
            var LView = await _imesRepository.GetICMo2(MO, LINE);
            string I_TS = LView.Where(u => u.MTYPE == 1 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_CA = LView.Where(u => u.MTYPE == 2 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_BOOK = LView.Where(u => u.MTYPE == 3 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_RE = LView.Where(u => u.MTYPE == 4 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_USB = LView.Where(u => u.MTYPE == 5 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_HDS = LView.Where(u => u.MTYPE == 6 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_GP = LView.Where(u => u.MTYPE == 7 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_PC = LView.Where(u => u.MTYPE == 8 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_GB = LView.Where(u => u.MTYPE == 9 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_MP = LView.Where(u => u.MTYPE == 10 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_3DZ = LView.Where(u => u.MTYPE == 11 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_PROF = LView.Where(u => u.MTYPE == 12 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_MDM = LView.Where(u => u.MTYPE == 13 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_DCP = LView.Where(u => u.MTYPE == 14 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_MAC = LView.Where(u => u.MTYPE == 15 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_JEC = LView.Where(u => u.MTYPE == 16 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_EC = LView.Where(u => u.MTYPE == 17 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_GF = LView.Where(u => u.MTYPE == 18 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_KZ = LView.Where(u => u.MTYPE == 19 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_LOG = LView.Where(u => u.MTYPE == 20 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_EL1 = LView.Where(u => u.MTYPE == 21 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_EL2 = LView.Where(u => u.MTYPE == 22 && u.DTYPE == 0).Select(x => x.MODEL).FirstOrDefault() ?? "";
            string I_NOTES1 = LView.Where(u => u.MTYPE == 1 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES2 = LView.Where(u => u.MTYPE == 2 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES3 = LView.Where(u => u.MTYPE == 3 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES4 = LView.Where(u => u.MTYPE == 4 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES5 = LView.Where(u => u.MTYPE == 5 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES6 = LView.Where(u => u.MTYPE == 6 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES7 = LView.Where(u => u.MTYPE == 7 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES8 = LView.Where(u => u.MTYPE == 8 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES9 = LView.Where(u => u.MTYPE == 9 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES10 = LView.Where(u => u.MTYPE == 10 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES11 = LView.Where(u => u.MTYPE == 11 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES12 = LView.Where(u => u.MTYPE == 12 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES13 = LView.Where(u => u.MTYPE == 13 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES14 = LView.Where(u => u.MTYPE == 14 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES15 = LView.Where(u => u.MTYPE == 15 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES16 = LView.Where(u => u.MTYPE == 16 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES17 = LView.Where(u => u.MTYPE == 17 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES18 = LView.Where(u => u.MTYPE == 18 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES19 = LView.Where(u => u.MTYPE == 19 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES20 = LView.Where(u => u.MTYPE == 20 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES21 = LView.Where(u => u.MTYPE == 21 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            string I_NOTES22 = LView.Where(u => u.MTYPE == 22 && u.DTYPE == 0).Select(x => x.NOTES).FirstOrDefault() ?? "";
            //});
            if (string.IsNullOrEmpty(I_TS) && string.IsNullOrEmpty(I_CA) && string.IsNullOrEmpty(I_BOOK) && string.IsNullOrEmpty(I_RE) && string.IsNullOrEmpty(I_USB)
                && string.IsNullOrEmpty(I_HDS) && string.IsNullOrEmpty(I_GP) && string.IsNullOrEmpty(I_PC) && string.IsNullOrEmpty(I_GB) && string.IsNullOrEmpty(I_MP)
                && string.IsNullOrEmpty(I_3DZ) && string.IsNullOrEmpty(I_PROF) && string.IsNullOrEmpty(I_MDM) && string.IsNullOrEmpty(I_DCP) && string.IsNullOrEmpty(I_MAC)
                && string.IsNullOrEmpty(I_JEC) && string.IsNullOrEmpty(I_EC) && string.IsNullOrEmpty(I_GF) && string.IsNullOrEmpty(I_KZ) && string.IsNullOrEmpty(I_LOG)
                && string.IsNullOrEmpty(I_EL1) && string.IsNullOrEmpty(I_EL2))
            {
                Msg = "查询无数据记录！！";
                return Ok(new JsonResult(new { Err = Msg, success = false }));
            }
            else
            {
                #region
                return Ok(new JsonResult(new
                {
                    I_TS,
                    I_CA,
                    I_BOOK,
                    I_RE,
                    I_USB,
                    I_HDS,
                    I_GP,
                    I_PC,
                    I_GB,
                    I_MP,
                    I_3DZ,
                    I_PROF,
                    I_MDM,
                    I_DCP,
                    I_MAC,
                    I_JEC,
                    I_EC,
                    I_GF,
                    I_KZ,
                    I_LOG,
                    I_EL1,
                    I_EL2,
                    I_NOTES1,
                    I_NOTES2,
                    I_NOTES3,
                    I_NOTES4,
                    I_NOTES5,
                    I_NOTES6,
                    I_NOTES7,
                    I_NOTES8,
                    I_NOTES9,
                    I_NOTES10,
                    I_NOTES11,
                    I_NOTES12,
                    I_NOTES13,
                    I_NOTES14,
                    I_NOTES15,
                    I_NOTES16,
                    I_NOTES17,
                    I_NOTES18,
                    I_NOTES19,
                    I_NOTES20,
                    I_NOTES21,
                    I_NOTES22,
                    success = true
                }));
                #endregion
            }
        }

        [HttpGet]
        [Route("GetT100MO")]
        public async Task<IActionResult> GetT100MO(string mo)
        {
            var employeeDto = await _imesRepository.GetSFAA_T(mo);
            return Ok(employeeDto);
        }
        [HttpGet]
        [Route("GetT100MObom")]
        public IActionResult GetT100MObom(string MO)
        {
            if (!string.IsNullOrEmpty(MO))
            {
                MO = MO.Trim();
            }
            //var lastResult = from q in _t100Context.sFBA_Ts.Where(x => x.SFBADOCNO == MO)
            //                 from p in _t100Context.iMAA_Ts
            //                 where (p.IMAA001 == q.SFBA006 && q.SFBAENT == p.IMAAENT)
            //                 select new { q.SFBA006, p.IMAAUD009, p.IMAAUD010 };
            var lastResult = from q in _t100Context.sFBA_Ts.Where(x => x.SFBADOCNO == MO)
                             from p in _t100Context.iMAAL_Ts.Where(x => x.IMAALENT == 100 && x.IMAAL002 == "zh_CN")
                             where p.IMAAL001 == q.SFBA006 && q.SFBAENT == p.IMAALENT
                             select new { q.SFBA006, IMAAUD009 = p.IMAAL003, IMAAUD010 = p.IMAAL004 };
            var lastResult1 = lastResult.OrderBy(x => x.IMAAUD009).ToList();
            if (lastResult1 is null)
            {
                return NotFound();
            }
            return Ok(lastResult1);
        }
        [HttpGet]
        [Route("GetT100BYbom")]
        public IActionResult GetT100BYbom(string BOM)
        {
            if (!string.IsNullOrEmpty(BOM))
            {
                BOM = BOM.Trim();
            }
            var lastResult = _t100Context.iMAAL_Ts.Where(x => x.IMAAL001 == BOM && x.IMAALENT == 100 && x.IMAAL002 == "zh_CN");
            var lastResult1 = lastResult.OrderBy(x => x.IMAAL001).ToList();
            if (lastResult1 is null)
            {
                return NotFound();
            }
            return Ok(lastResult1);
        }
        [HttpGet]
        [Route("GetODMOLOCK")]
        public async Task<IActionResult> GetODMOLOCK(string MO, string LINE, string DATE1, string DATE2)
        {
            if (!string.IsNullOrEmpty(MO))
            {
                MO = MO.Trim();
            }
            return Ok(await _imesRepository.GetODMOLOCK(MO, LINE, DATE1, DATE2));
        }
        [HttpGet]
        [Route("GetSSCCINFOLOG")]
        public async Task<IActionResult> GetSSCCINFOLOG(string SSCC, string PALLET, string PASSORNG, string DATE1, string DATE2)
        {
            return Ok(await _imesRepository.GetEDI_SSCCINFOLOG(SSCC, PALLET, PASSORNG, DATE1, DATE2));
        }
        [HttpGet]
        [Route("GetFQA_CK_SNLOG")]
        public async Task<IActionResult> GetFQA_CK_SNLOG(string MO, string SN, string LINE, string TY, string DATE1, string DATE2, string ISC, string ISOK, string Ems)
        {
            if (!string.IsNullOrEmpty(MO))
            {
                MO = MO.Trim();
            }
            if (!string.IsNullOrEmpty(SN))
            {
                SN = SN.Trim();
            }
            if (!string.IsNullOrEmpty(LINE))
            {
                LINE = LINE.Trim();
            }
            if (!string.IsNullOrEmpty(TY))
            {
                TY = TY.Trim();
            }
            var liResult = await _imesRepository.GetFQA_CK_SNLOG(MO, SN, LINE, TY, DATE1, DATE2, ISC, ISOK, Ems);
            return Ok(liResult);
        }
        [HttpGet]
        [Route("GetFQA_CK_SnImeiLOG")]
        public async Task<IActionResult> GetFQA_CK_SnImeiLOG(string MO, string SN, string IMEI, string LINE, string TY, string DATE1, string DATE2, string ISC, string ISOK, string Ems, string TS)
        {
            if (!string.IsNullOrEmpty(MO))
            {
                MO = MO.Trim();
            }
            if (!string.IsNullOrEmpty(SN))
            {
                SN = SN.Trim();
            }
            if (!string.IsNullOrEmpty(LINE))
            {
                LINE = LINE.Trim();
            }
            if (!string.IsNullOrEmpty(IMEI))
            {
                IMEI = IMEI.Trim();
            }
            if (!string.IsNullOrEmpty(TY))
            {
                TY = TY.Trim();
            }
            var liResult = await _imesRepository.GetFQA_CK_SNIMEILOG(MO, SN, IMEI, LINE, TY, DATE1, DATE2, ISC, ISOK, Ems, TS);
            return Ok(liResult);
        }
        [HttpGet]
        [Route("GetFIRSTARTICLE1")]
        public async Task<IActionResult> GetFIRSTARTICLE1(string MO, string BOM, string DATE1, string DATE2, string MTYPE)
        {
            if (!string.IsNullOrEmpty(MO))
            {
                MO = MO.Trim();
            }
            var liResult = await _imesRepository.FIRSTARTICLEs(MO, BOM, DATE1, DATE2, MTYPE);
            return Ok(liResult);
        }
        [HttpGet]
        [Route("GetODM_TEMPERATUREBOARD")]
        public async Task<IActionResult> GetODM_TEMPERATUREBOARD(string SN)
        {
            ODM_TEMPERATUREBOARD dd = await _imesRepository.GetODM_TEMPERATUREBOARD(SN);
            return Ok(dd);
        }
        [HttpGet]
        [Route("GetODM_TEMPERATUREBOARD_DTOS")]
        public async Task<IActionResult> GetODM_TEMPERATUREBOARD_DTOS(string SN)
        {
            return Ok(await _imesRepository.GetODM_TEMPERATUREBOARD_DTOS(SN));
        }
        [HttpGet]
        [Route("IsTemperatureboad")]
        public async Task<IActionResult> IsTemperatureboad(string SN)
        {
            string Msg;
            bool bl;
            if (string.IsNullOrWhiteSpace(SN) || SN == "undefaul")
            {
                Msg = "SN 不能为空";
                bl = false;
                goto END;
            }
            var oDM_TEMP = await _imesRepository.GetODM_TEMPERATUREBOARD(SN);
            if (oDM_TEMP == null)
            {
                Msg = "SN 不存在";
                bl = false;
                goto END;
            }
            if (oDM_TEMP.FSTATUS == 1)
            {
                Msg = "SN 已报废";
                bl = false;
                goto END;
            }
            var oDM_TEMP1 = await _imesRepository.GetODM_TEMPERATUREBOARD_DTL(SN);
            var count1 = oDM_TEMP1.ToList().Count;
            if (count1 == oDM_TEMP.FCOUNT)
            {
                Msg = "SN使用次数已经完成";
                bl = false;
                goto END;
            }
            else
            {
                Msg = count1.ToString();
                bl = true;
                goto END;
            }
        END:
            DT_RETURN rETURN = new DT_RETURN
            {
                Msg = Msg,
                Success = bl,
                Service = "IsTemperatureboad"
            };
            return Ok(rETURN);
        }
        [HttpGet]
        [Route("GetFQA_LINETIMEOW")]
        public async Task<IActionResult> GetFQA_LINETIMEOW(string LINE)
        {
            var lis = await _imesRepository.GetFQA_LINETIMEOW(LINE);
            return Ok(lis);
        }
        [HttpGet]
        [Route("GetODM_SIDKEYLOG")]
        public async Task<IActionResult> GetODM_SIDKEYLOG(string SID)
        {
            if (!string.IsNullOrEmpty(SID))
            {
                SID = SID.Trim();
            }

            return Ok(await _imesRepository.GetODM_SIDKEYLOG(SID));
        }
        [HttpGet]
        [Route("GetODM_SIDKEYCT")]
        public IActionResult GetODM_SIDKEYCT()
        {
            return Ok(_imesRepository.GetODM_SIDKEYCT());
        }
        [HttpGet]
        [Route("GetODM_SIDKEYCTBYMODEL")]
        public IActionResult  GetODM_SIDKEYCTBYMODEL(string code)
        {
            return Ok( _imesRepository.GetODM_SIDKEYCTBYMODEL(code));
        }
        [HttpGet]
        [Route("GetRE_MODEL")]
        public async Task<IActionResult> GetRE_MODEL()
        {
            return Ok(await _imesRepository.GetRE_MODEL());
        }
        [HttpGet]
        [Route("GetRE_BUG_TYPE")]
        public async Task<IActionResult> GetRE_BUG_TYPE(string CODE, string MODEL, string TYPE)
        {
            return Ok(await _imesRepository.GetRE_BUG_TYPE(CODE,MODEL,TYPE));
        }
        [HttpGet]
        [Route("GetRE_CODE")]
        public async Task<IActionResult> GetRE_CODE(string CODE, string MODEL, string TYPE)
        {
            return Ok(await _imesRepository.GetRE_CODE(CODE, MODEL, TYPE));
        }
        [HttpGet]
        [Route("IsFqa_Ck_Snlog")]
        public async Task<IActionResult> IsFqa_Ck_Snlog(string SN)
        { 
            return Ok(await _imesRepository.ErrMessage(SN, "抽样批次输入", "", 1));
        }
        [HttpGet]
        [Route("G_FQA_ERRTY")]
        public async Task<IActionResult> G_FQA_ERRTY(string code)
        {
            return Ok(await _imesRepository.FQA_ERRTY(code));
        }
        [HttpGet]
        [Route("G_FQA_ERRTYCODE")]
        public async Task<IActionResult> G_FQA_ERRTYCODE(string Tcode, string code)
        {
            return Ok(await _imesRepository.FQA_ERRTYCODE(Tcode,code));
        }
        [HttpGet]
        [Route("G_850appendix")]
        public async Task<IActionResult> G_850appendix(string po)
        {
            return Ok(await _imesRepository.G_850appendix(po));
        }
        [HttpGet]
        [Route("G_ODM_PRESSUREMODEL")]
        public async Task<IActionResult> G_ODM_PRESSUREMODEL(string code)
        {
            return Ok(await _imesRepository.G_ODM_PRESSUREMODEL(code));
        }
      
        /// <summary>GetODM_SIDKEYCTBYMODEL
        /// 20201123  测试失败次数
        /// </summary>
        /// <param name="STATION"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetODM_TESTFAILNUM")]
        public async Task<IActionResult> GetODM_TESTFAILNUM(string STATION)
        {
            return Ok(await _imesRepository.GetODM_TESTFAILNUM(STATION));
        }
        [HttpGet]
        [Route("GetLIST_TESTFAILNUM")]
        public async Task<IActionResult> GetLIST_TESTFAILNUM()
        {
            return Ok(await _imesRepository.GetLIST_TESTFAILNUM());
        }
        //ort_barcode_new
        [HttpGet]
        [Route("GetOrt_barcode")]
        public async Task<IActionResult> GetOrt_barcode(string V_MO, string V_SN,string V_EMS_M2,string V_DATE1, string V_DATE2, string V_EMS_M1)
        {
            return Ok(await _imesRepository.GetOrt_barcode(V_MO, V_SN, V_EMS_M2, V_DATE1, V_DATE2, V_EMS_M1));
        }
        [HttpGet]
        [Route("G_ODM_PACKINGELSE")]
        public async Task<IActionResult> G_ODM_PACKINGELSE(string V_MO, string DATE1, string SID, string CARTONID)
        {
            return Ok(await _imesRepository.G_ODM_PACKINGELSE(V_MO, DATE1, SID, CARTONID));
        }
        [HttpPost]
        [Route("UpDateORT")]
        public async Task<IActionResult> UpDateORT([FromForm] IFormCollection fm)
        {
            string ID = fm["ID"].ToString() == "" ? "" : fm["ID"].ToString().Trim();
            string PCUSER = fm["PCUSER"].ToString() == "" ? "" : fm["PCUSER"].ToString().Trim();
            string ORT = fm["ORT"].ToString() == "" ? "" : fm["ORT"].ToString().Trim();
            return Ok(await _imesRepository.UpDateORT(ID, PCUSER, ORT));
        }
        //ODM_PRESSURE
        [HttpPost]
        [Route("GetODM_PRESSURE")]
        public async Task<IActionResult> GetODM_PRESSURE([FromForm] IFormCollection fm)
        {
            string CLIENTCODE = string.IsNullOrWhiteSpace(fm["CLIENTCODE"].ToString()) ? "" : fm["CLIENTCODE"].ToString().Trim();
            string DATE1 = string.IsNullOrWhiteSpace(fm["DATE1"].ToString()) ? "" : fm["DATE1"].ToString().Trim();
            string DATE2 = string.IsNullOrWhiteSpace(fm["DATE2"].ToString()) ? "" : fm["DATE2"].ToString().Trim();
            string SN = string.IsNullOrWhiteSpace(fm["SN"].ToString()) ? "" : fm["SN"].ToString().Trim();
            string MO = string.IsNullOrWhiteSpace(fm["MO"].ToString()) ? "" : fm["MO"].ToString().Trim();
            string LINE = string.IsNullOrWhiteSpace(fm["LINE"].ToString()) ? "" : fm["LINE"].ToString().Trim();
            string STATION = string.IsNullOrWhiteSpace(fm["STATION"].ToString()) ? "" : fm["STATION"].ToString().Trim();
            
            if (string.IsNullOrWhiteSpace(MO)&& string.IsNullOrWhiteSpace(SN) && string.IsNullOrWhiteSpace(DATE1))
            {
                RT_MESSAGE err = new RT_MESSAGE
                {
                    success = false,
                    Err = "工单、SN、时间不能同时为空"
                };
                 return Ok(err);
            }
            if (string.IsNullOrWhiteSpace(DATE1) && !string.IsNullOrWhiteSpace(DATE2))
            {
                RT_MESSAGE err = new RT_MESSAGE
                {
                    success = false,
                    Err = "开始时间不能为空"
                };
                return Ok(err);
            }
            if (!string.IsNullOrWhiteSpace(DATE1)&& string.IsNullOrWhiteSpace(DATE2))
            {
                RT_MESSAGE err = new RT_MESSAGE
                {
                    success = false,
                    Err = "结束时间不能为空"
                };
                return Ok(err);
            }
            return Ok(await _imesRepository.GetODM_PRESSURE(MO, SN, DATE1, DATE2, STATION, LINE, CLIENTCODE)); 
        }
        [HttpPost]
        [Route("GetODM_PRESSUREFREE")]
        public async Task<IActionResult> GetODM_PRESSUREFREE([FromForm] IFormCollection fm)
        { 
            string DATE1 = string.IsNullOrWhiteSpace(fm["DATE1"].ToString()) ? "" : fm["DATE1"].ToString().Trim();
            string DATE2 = string.IsNullOrWhiteSpace(fm["DATE2"].ToString()) ? "" : fm["DATE2"].ToString().Trim();
            string SN = string.IsNullOrWhiteSpace(fm["SN"].ToString()) ? "" : fm["SN"].ToString().Trim();
            string ID_USER = string.IsNullOrWhiteSpace(fm["ID_USER"].ToString()) ? "" : fm["ID_USER"].ToString().Trim(); 

            if (string.IsNullOrWhiteSpace(ID_USER) && string.IsNullOrWhiteSpace(SN) && string.IsNullOrWhiteSpace(DATE1))
            {
                RT_MESSAGE err = new RT_MESSAGE
                {
                    success = false,
                    Err = "工单、释放ID、时间不能同时为空"
                };
                return Ok(err);
            }
            if (string.IsNullOrWhiteSpace(DATE1) && !string.IsNullOrWhiteSpace(DATE2))
            {
                RT_MESSAGE err = new RT_MESSAGE
                {
                    success = false,
                    Err = "开始时间不能为空"
                };
                return Ok(err);
            }
            if (!string.IsNullOrWhiteSpace(DATE1) && string.IsNullOrWhiteSpace(DATE2))
            {
                RT_MESSAGE err = new RT_MESSAGE
                {
                    success = false,
                    Err = "结束时间不能为空"
                };
                return Ok(err);
            }
            return Ok(await _imesRepository.GetODM_PRESSUREFREE(SN,DATE1,DATE2,ID_USER));
        }
        [HttpPost]
        [Route("AddODM_TESTFAILNUM")]
        public async Task<IActionResult> AddODM_TESTFAILNUM([FromForm] IFormCollection fm)
        {
            string STATION = fm["STATION"].ToString() == "" ? "" : fm["STATION"].ToString().Trim();
            string FAILNUM = fm["FAILNUM"].ToString() == "" ? "0" : fm["FAILNUM"].ToString().Trim();
            try
            {
                ODM_TESTFAILNUM dM_TESTFAILNUM = await _imesRepository.GetODM_TESTFAILNUM(STATION);
                if (dM_TESTFAILNUM == null)
                {
                    ODM_TESTFAILNUM oDM_TESTFAILNUM = new ODM_TESTFAILNUM
                    {
                        STATION = STATION,
                        FAILNUM = Convert.ToInt32(FAILNUM)
                    };
                    _imesRepository.AddODM_TESTFAILNUM(oDM_TESTFAILNUM);
                }
                else
                {
                    dM_TESTFAILNUM.STATION = STATION;
                    dM_TESTFAILNUM.FAILNUM = Convert.ToInt32(FAILNUM);
                    //ODM_TESTFAILNUM oDM_TESTFAILNUM = new ODM_TESTFAILNUM
                    //{
                    //    STATION = STATION,
                    //    FAILNUM = Convert.ToInt32(FAILNUM)
                    //};
                    _imesRepository.UpdateODM_TESTFAILNUM(dM_TESTFAILNUM);
                }
            }
            catch (Exception ex)
            {
                return Ok(new ErrMessage { Err = ex.ToString(), success = false });
                throw;
            }
            await _imesRepository.SaveAsync();
            return Ok(new ErrMessage { Err = "添加成功", success = true });
        }
        [HttpPost]
        [Route("DelODM_TESTFAILNUM")]
        public async Task<IActionResult> DelODM_TESTFAILNUM([FromForm] IFormCollection fm)
        {
            string STATION = fm["STATION"].ToString() == "" ? "" : fm["STATION"].ToString().Trim();
            return Ok(await _imesRepository.DelODM_TESTFAILNUM(STATION));
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="MO"></param>
        /// <param name="LINE"></param>
        /// <param name="ISLOCK"></param>
        /// <param name="I_USER"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("IS_ODMLOCKMO")]
        public async Task<IActionResult> IS_ODMLOCKMO(string MO, string LINE, int ISLOCK, string I_USER)
        {
            if (!string.IsNullOrEmpty(MO))
            {
                MO = MO.Trim();
            }
            LOCKMO oCKMO = new LOCKMO
            {
                MO = MO,
                LINE = LINE,
                ISLOCK = ISLOCK,
                CRT_DATE = DateTime.Now,
                CRT_USER = I_USER
            };
            _imesRepository.AddLOCKMO(oCKMO);
            await _imesRepository.SaveAsync();
            return Ok();
        }
        [HttpPost]
        [Route("IS_ODMLOCKMO1")]
        public async Task<IActionResult> IS_ODMLOCKMO1(string MO, string LINE, int ISLOCK, string I_USER, string SN)
        {
            if (!string.IsNullOrEmpty(MO))
            {
                MO = MO.Trim();
            }
            LOCKMO oCKMO = new LOCKMO
            {
                MO = MO,
                LINE = LINE,
                ISLOCK = ISLOCK,
                CRT_DATE = DateTime.Now,
                CRT_USER = I_USER,
                DESCRIBE = SN
            };
            _imesRepository.AddLOCKMO(oCKMO);
            await _imesRepository.SaveAsync();
            return Ok();
        }
        [HttpPost]
        [Route("FIRSTARTICLE")]
        public async Task<IActionResult> FIRSTARTICLE(string SaveJson)
        {
            string Msg = string.Empty; Msg = "提交完成";
            Newtonsoft.Json.Linq.JObject jobject = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(SaveJson);
            string MO = jobject.Properties().Any(p => p.Name == "MO") ? jobject["MO"].ToString() : "";
            string I_USER = jobject.Properties().Any(p => p.Name == "I_USER") ? jobject["I_USER"].ToString() : "";
            string I_TS = jobject.Properties().Any(p => p.Name == "I_TS") ? jobject["I_TS"].ToString() : "";
            string I_CA = jobject.Properties().Any(p => p.Name == "I_CA") ? jobject["I_CA"].ToString() : "";
            string I_BOOK = jobject.Properties().Any(p => p.Name == "I_BOOK") ? jobject["I_BOOK"].ToString() : "";
            string I_RE = jobject.Properties().Any(p => p.Name == "I_RE") ? jobject["I_RE"].ToString() : "";
            string I_USB = jobject.Properties().Any(p => p.Name == "I_USB") ? jobject["I_USB"].ToString() : "";
            string I_HDS = jobject.Properties().Any(p => p.Name == "I_HDS") ? jobject["I_HDS"].ToString() : "";
            string I_GP = jobject.Properties().Any(p => p.Name == "I_GP") ? jobject["I_GP"].ToString() : "";
            string I_PC = jobject.Properties().Any(p => p.Name == "I_PC") ? jobject["I_PC"].ToString() : "";
            string I_GB = jobject.Properties().Any(p => p.Name == "I_GB") ? jobject["I_GB"].ToString() : "";
            string I_MP = jobject.Properties().Any(p => p.Name == "I_MP") ? jobject["I_MP"].ToString() : "";
            string I_3DZ = jobject.Properties().Any(p => p.Name == "I_3DZ") ? jobject["I_3DZ"].ToString() : "";
            string I_PROF = jobject.Properties().Any(p => p.Name == "I_PROF") ? jobject["I_PROF"].ToString() : "";
            string I_MDM = jobject.Properties().Any(p => p.Name == "I_MDM") ? jobject["I_MDM"].ToString() : "";
            string I_DCP = jobject.Properties().Any(p => p.Name == "I_DCP") ? jobject["I_DCP"].ToString() : "";
            string I_MAC = jobject.Properties().Any(p => p.Name == "I_MAC") ? jobject["I_MAC"].ToString() : "";
            string I_JEC = jobject.Properties().Any(p => p.Name == "I_JEC") ? jobject["I_JEC"].ToString() : "";
            string I_EC = jobject.Properties().Any(p => p.Name == "I_EC") ? jobject["I_EC"].ToString() : "";
            string I_GF = jobject.Properties().Any(p => p.Name == "I_GF") ? jobject["I_GF"].ToString() : "";
            string I_KZ = jobject.Properties().Any(p => p.Name == "I_KZ") ? jobject["I_KZ"].ToString() : "";
            string I_LOG = jobject.Properties().Any(p => p.Name == "I_LOG") ? jobject["I_LOG"].ToString() : "";
            string I_EL1 = jobject.Properties().Any(p => p.Name == "I_EL1") ? jobject["I_EL1"].ToString() : "";
            string I_EL2 = jobject.Properties().Any(p => p.Name == "I_EL2") ? jobject["I_EL2"].ToString() : "";
            string I_NOTES1 = jobject.Properties().Any(p => p.Name == "I_NOTES1") ? jobject["I_NOTES1"].ToString() : "";
            string I_NOTES2 = jobject.Properties().Any(p => p.Name == "I_NOTES2") ? jobject["I_NOTES2"].ToString() : "";
            string I_NOTES3 = jobject.Properties().Any(p => p.Name == "I_NOTES3") ? jobject["I_NOTES3"].ToString() : "";
            string I_NOTES4 = jobject.Properties().Any(p => p.Name == "I_NOTES4") ? jobject["I_NOTES4"].ToString() : "";
            string I_NOTES5 = jobject.Properties().Any(p => p.Name == "I_NOTES5") ? jobject["I_NOTES5"].ToString() : "";
            string I_NOTES6 = jobject.Properties().Any(p => p.Name == "I_NOTES6") ? jobject["I_NOTES6"].ToString() : "";
            string I_NOTES7 = jobject.Properties().Any(p => p.Name == "I_NOTES7") ? jobject["I_NOTES7"].ToString() : "";
            string I_NOTES8 = jobject.Properties().Any(p => p.Name == "I_NOTES8") ? jobject["I_NOTES8"].ToString() : "";
            string I_NOTES9 = jobject.Properties().Any(p => p.Name == "I_NOTES9") ? jobject["I_NOTES9"].ToString() : "";
            string I_NOTES10 = jobject.Properties().Any(p => p.Name == "I_NOTES10") ? jobject["I_NOTES10"].ToString() : "";
            string I_NOTES11 = jobject.Properties().Any(p => p.Name == "I_NOTES11") ? jobject["I_NOTES11"].ToString() : "";
            string I_NOTES12 = jobject.Properties().Any(p => p.Name == "I_NOTES12") ? jobject["I_NOTES12"].ToString() : "";
            string I_NOTES13 = jobject.Properties().Any(p => p.Name == "I_NOTES13") ? jobject["I_NOTES13"].ToString() : "";
            string I_NOTES14 = jobject.Properties().Any(p => p.Name == "I_NOTES14") ? jobject["I_NOTES14"].ToString() : "";
            string I_NOTES15 = jobject.Properties().Any(p => p.Name == "I_NOTES15") ? jobject["I_NOTES15"].ToString() : "";
            string I_NOTES16 = jobject.Properties().Any(p => p.Name == "I_NOTES16") ? jobject["I_NOTES16"].ToString() : "";
            string I_NOTES17 = jobject.Properties().Any(p => p.Name == "I_NOTES17") ? jobject["I_NOTES17"].ToString() : "";
            string I_NOTES18 = jobject.Properties().Any(p => p.Name == "I_NOTES18") ? jobject["I_NOTES18"].ToString() : "";
            string I_NOTES19 = jobject.Properties().Any(p => p.Name == "I_NOTES19") ? jobject["I_NOTES19"].ToString() : "";
            string I_NOTES20 = jobject.Properties().Any(p => p.Name == "I_NOTES20") ? jobject["I_NOTES20"].ToString() : "";
            string I_NOTES21 = jobject.Properties().Any(p => p.Name == "I_NOTES21") ? jobject["I_NOTES21"].ToString() : "";
            string I_NOTES22 = jobject.Properties().Any(p => p.Name == "I_NOTES22") ? jobject["I_NOTES22"].ToString() : "";
            var ListView = await _imesRepository.GetICMo1(MO);
            if (!string.IsNullOrWhiteSpace(I_TS))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 1).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    //var province = new FQA_FIRSTARTICLE() 
                    //{
                    //    MO = MO,
                    //    MTYPE = 1,
                    //    MODEL = I_TS,
                    //    CREAT_DT = DateTime.Now,
                    //    DTYPE = 0,
                    //    NOTES = I_NOTES1
                    //};

                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 1).FirstOrDefault();
                    province.MODEL = I_TS;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES1;
                    province.CREAT_US = I_USER;
                    ////.Select(new
                    ////{
                    ////    MO = MO,
                    ////    MTYPE = 1,
                    ////    MODEL = I_TS,
                    ////    CREAT_DT = DateTime.Now,
                    ////    DTYPE = 0,
                    ////    NOTES = I_NOTES1
                    ////}); 
                    //0.0创建修改的 实体对象
                    //var province = new FQA_FIRSTARTICLE(); 
                    //province.NOTES = "新的数据";
                    //province.MODEL = "新的数据~~~~~";
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 1,
                        MODEL = I_TS,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES1
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 1 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_CA))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 2).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 2).FirstOrDefault();
                    province.MODEL = I_CA;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES2;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 2,
                        MODEL = I_CA,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES2

                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 2 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_BOOK))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 3).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 3).FirstOrDefault();
                    province.MODEL = I_BOOK;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES3;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 3,
                        MODEL = I_BOOK,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES3
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 3 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_RE))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 4).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 4).FirstOrDefault();
                    province.MODEL = I_RE;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES4;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 4,
                        MODEL = I_RE,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES4
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 4 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_USB))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 5).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 5).FirstOrDefault();
                    province.MODEL = I_USB;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES5;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 5,
                        MODEL = I_USB,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES5
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 5 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_HDS))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 6).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 6).FirstOrDefault();
                    province.MODEL = I_HDS;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES6;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 6,
                        MODEL = I_HDS,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES6
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 6 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_GP))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 7).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 7).FirstOrDefault();
                    province.MODEL = I_GP;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES7;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 7,
                        MODEL = I_GP,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES7
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 7 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_PC))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 8).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 8).FirstOrDefault();
                    province.MODEL = I_PC;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES8;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 8,
                        MODEL = I_PC,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES8
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 8 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_GB))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 9).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 9).FirstOrDefault();
                    province.MODEL = I_GB;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES9;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 9,
                        MODEL = I_GB,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES9
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 9 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_MP))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 10).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 10).FirstOrDefault();
                    province.MODEL = I_MP;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES10;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 10,
                        MODEL = I_MP,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES10
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 10 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_3DZ))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 11).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 11).FirstOrDefault();
                    province.MODEL = I_3DZ;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES11;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 11,
                        MODEL = I_3DZ,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES11
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 11 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_PROF))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 12).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 12).FirstOrDefault();
                    province.MODEL = I_PROF;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES12;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 12,
                        MODEL = I_PROF,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES12
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 12 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_MDM))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 13).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 13).FirstOrDefault();
                    province.MODEL = I_MDM;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES13;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 13,
                        MODEL = I_MDM,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES13
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 13 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_DCP))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 14).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 14).FirstOrDefault();
                    province.MODEL = I_DCP;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES14;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 14,
                        MODEL = I_DCP,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES14
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 14 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_MAC))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 15).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 15).FirstOrDefault();
                    province.MODEL = I_MAC;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES15;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 15,
                        MODEL = I_MAC,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES15
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 15 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_JEC))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 16).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 16).FirstOrDefault();
                    province.MODEL = I_JEC;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES16;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 16,
                        MODEL = I_JEC,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES17
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 16 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_EC))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 17).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 17).FirstOrDefault();
                    province.MODEL = I_EC;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES17;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 17,
                        MODEL = I_EC,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES17
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 17 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_GF))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 18).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 18).FirstOrDefault();
                    province.MODEL = I_GF;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES18;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 18,
                        MODEL = I_GF,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES18
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 18 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_KZ))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 19).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 19).FirstOrDefault();
                    province.MODEL = I_KZ;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES19;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 19,
                        MODEL = I_KZ,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES19
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 19 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_LOG))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 20).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 20).FirstOrDefault();
                    province.MODEL = I_LOG;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES20;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 20,
                        MODEL = I_LOG,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES20
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 20 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_EL1))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 21).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 21).FirstOrDefault();
                    province.MODEL = I_EL1;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES21;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 21,
                        MODEL = I_EL1,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES21
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 21 && x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 21 && x.DTYPE == 0);
                    if (userinfo6 != null)
                    {
                        _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(I_EL2))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 22).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE province = ListView.Where(x => x.MTYPE == 22).FirstOrDefault();
                    province.MODEL = I_EL2;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES22;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLE()
                    {
                        MO = MO,
                        MTYPE = 22,
                        MODEL = I_EL2,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES22
                    };
                    _imesRepository.AddFQA_FIRSTARTICLE(province);
                }
            }
            else
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 22 && x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 22 && x.DTYPE == 0);
                    if (userinfo6 != null)
                    {
                        _imesRepository.DeleteFQA_FIRSTARTICLE(userinfo6);
                    }
                }
            }
            await _imesRepository.SaveAsync();
            return Ok(new ErrMessage { Err = Msg, success = true });
        }
        [HttpPost]
        [Route("FIRSTARTICLELINE")]
        public async Task<IActionResult> FIRSTARTICLELINE(string SaveJson)
        {
            string Msg = string.Empty; Msg = "提交完成";
            var jobject = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(SaveJson);
            string MO = jobject.Properties().Any(p => p.Name == "MO") ? jobject["MO"].ToString() : "";// province.CREAT_US = I_USER;
            string I_USER = jobject.Properties().Any(p => p.Name == "I_USER") ? jobject["I_USER"].ToString() : "";
            string LINE = jobject.Properties().Any(p => p.Name == "I_LINE") ? jobject["I_LINE"].ToString() : "";
            string I_TS = jobject.Properties().Any(p => p.Name == "I_TS") ? jobject["I_TS"].ToString() : "";
            string I_CA = jobject.Properties().Any(p => p.Name == "I_CA") ? jobject["I_CA"].ToString() : "";
            string I_BOOK = jobject.Properties().Any(p => p.Name == "I_BOOK") ? jobject["I_BOOK"].ToString() : "";
            string I_RE = jobject.Properties().Any(p => p.Name == "I_RE") ? jobject["I_RE"].ToString() : "";
            string I_USB = jobject.Properties().Any(p => p.Name == "I_USB") ? jobject["I_USB"].ToString() : "";
            string I_HDS = jobject.Properties().Any(p => p.Name == "I_HDS") ? jobject["I_HDS"].ToString() : "";
            string I_GP = jobject.Properties().Any(p => p.Name == "I_GP") ? jobject["I_GP"].ToString() : "";
            string I_PC = jobject.Properties().Any(p => p.Name == "I_PC") ? jobject["I_PC"].ToString() : "";
            string I_GB = jobject.Properties().Any(p => p.Name == "I_GB") ? jobject["I_GB"].ToString() : "";
            string I_MP = jobject.Properties().Any(p => p.Name == "I_MP") ? jobject["I_MP"].ToString() : "";
            string I_3DZ = jobject.Properties().Any(p => p.Name == "I_3DZ") ? jobject["I_3DZ"].ToString() : "";
            string I_PROF = jobject.Properties().Any(p => p.Name == "I_PROF") ? jobject["I_PROF"].ToString() : "";
            string I_MDM = jobject.Properties().Any(p => p.Name == "I_MDM") ? jobject["I_MDM"].ToString() : "";
            string I_DCP = jobject.Properties().Any(p => p.Name == "I_DCP") ? jobject["I_DCP"].ToString() : "";
            string I_MAC = jobject.Properties().Any(p => p.Name == "I_MAC") ? jobject["I_MAC"].ToString() : "";
            string I_JEC = jobject.Properties().Any(p => p.Name == "I_JEC") ? jobject["I_JEC"].ToString() : "";
            string I_EC = jobject.Properties().Any(p => p.Name == "I_EC") ? jobject["I_EC"].ToString() : "";
            string I_GF = jobject.Properties().Any(p => p.Name == "I_GF") ? jobject["I_GF"].ToString() : "";
            string I_KZ = jobject.Properties().Any(p => p.Name == "I_KZ") ? jobject["I_KZ"].ToString() : "";
            string I_LOG = jobject.Properties().Any(p => p.Name == "I_LOG") ? jobject["I_LOG"].ToString() : "";
            string I_EL1 = jobject.Properties().Any(p => p.Name == "I_EL1") ? jobject["I_EL1"].ToString() : "";
            string I_EL2 = jobject.Properties().Any(p => p.Name == "I_EL2") ? jobject["I_EL2"].ToString() : "";
            string I_NOTES1 = jobject.Properties().Any(p => p.Name == "I_NOTES1") ? jobject["I_NOTES1"].ToString() : "";
            string I_NOTES2 = jobject.Properties().Any(p => p.Name == "I_NOTES2") ? jobject["I_NOTES2"].ToString() : "";
            string I_NOTES3 = jobject.Properties().Any(p => p.Name == "I_NOTES3") ? jobject["I_NOTES3"].ToString() : "";
            string I_NOTES4 = jobject.Properties().Any(p => p.Name == "I_NOTES4") ? jobject["I_NOTES4"].ToString() : "";
            string I_NOTES5 = jobject.Properties().Any(p => p.Name == "I_NOTES5") ? jobject["I_NOTES5"].ToString() : "";
            string I_NOTES6 = jobject.Properties().Any(p => p.Name == "I_NOTES6") ? jobject["I_NOTES6"].ToString() : "";
            string I_NOTES7 = jobject.Properties().Any(p => p.Name == "I_NOTES7") ? jobject["I_NOTES7"].ToString() : "";
            string I_NOTES8 = jobject.Properties().Any(p => p.Name == "I_NOTES8") ? jobject["I_NOTES8"].ToString() : "";
            string I_NOTES9 = jobject.Properties().Any(p => p.Name == "I_NOTES9") ? jobject["I_NOTES9"].ToString() : "";
            string I_NOTES10 = jobject.Properties().Any(p => p.Name == "I_NOTES10") ? jobject["I_NOTES10"].ToString() : "";
            string I_NOTES11 = jobject.Properties().Any(p => p.Name == "I_NOTES11") ? jobject["I_NOTES11"].ToString() : "";
            string I_NOTES12 = jobject.Properties().Any(p => p.Name == "I_NOTES12") ? jobject["I_NOTES12"].ToString() : "";
            string I_NOTES13 = jobject.Properties().Any(p => p.Name == "I_NOTES13") ? jobject["I_NOTES13"].ToString() : "";
            string I_NOTES14 = jobject.Properties().Any(p => p.Name == "I_NOTES14") ? jobject["I_NOTES14"].ToString() : "";
            string I_NOTES15 = jobject.Properties().Any(p => p.Name == "I_NOTES15") ? jobject["I_NOTES15"].ToString() : "";
            string I_NOTES16 = jobject.Properties().Any(p => p.Name == "I_NOTES16") ? jobject["I_NOTES16"].ToString() : "";
            string I_NOTES17 = jobject.Properties().Any(p => p.Name == "I_NOTES17") ? jobject["I_NOTES17"].ToString() : "";
            string I_NOTES18 = jobject.Properties().Any(p => p.Name == "I_NOTES18") ? jobject["I_NOTES18"].ToString() : "";
            string I_NOTES19 = jobject.Properties().Any(p => p.Name == "I_NOTES19") ? jobject["I_NOTES19"].ToString() : "";
            string I_NOTES20 = jobject.Properties().Any(p => p.Name == "I_NOTES20") ? jobject["I_NOTES20"].ToString() : "";
            string I_NOTES21 = jobject.Properties().Any(p => p.Name == "I_NOTES21") ? jobject["I_NOTES21"].ToString() : "";
            string I_NOTES22 = jobject.Properties().Any(p => p.Name == "I_NOTES22") ? jobject["I_NOTES22"].ToString() : "";
            string I_TALLSN = jobject.Properties().Any(p => p.Name == "I_TALLSN") ? jobject["I_TALLSN"].ToString() : "";
            var ListView = await _imesRepository.GetICMo2(MO, LINE);
            if (!string.IsNullOrWhiteSpace(I_TS))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 1).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 1).FirstOrDefault();
                    province.MODEL = I_TS;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES1;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 1,
                        MODEL = I_TS,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES1,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 1 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_CA))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 2).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 2).FirstOrDefault();
                    province.MODEL = I_CA;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES2;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 2,
                        MODEL = I_CA,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES2,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 2 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_BOOK))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 3).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 3).FirstOrDefault();
                    province.MODEL = I_BOOK;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES3;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 3,
                        MODEL = I_BOOK,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES3,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 3 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_RE))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 4).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 4).FirstOrDefault();
                    province.MODEL = I_RE;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES4;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 4,
                        MODEL = I_RE,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES4,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 4 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_USB))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 5).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 5).FirstOrDefault();
                    province.MODEL = I_USB;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES5;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 5,
                        MODEL = I_USB,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES5,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 5 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_HDS))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 6).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 6).FirstOrDefault();
                    province.MODEL = I_HDS;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES6;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 6,
                        MODEL = I_HDS,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES6,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 6 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_GP))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 7).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 7).FirstOrDefault();
                    province.MODEL = I_GP;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES7;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 7,
                        MODEL = I_GP,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES7,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 7 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_PC))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 8).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 8).FirstOrDefault();
                    province.MODEL = I_PC;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES8;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 8,
                        MODEL = I_PC,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES8,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 8 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_GB))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 9).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 9).FirstOrDefault();
                    province.MODEL = I_GB;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES9;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 9,
                        MODEL = I_GB,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES9,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 9 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_MP))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 10).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 10).FirstOrDefault();
                    province.MODEL = I_MP;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES10;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 10,
                        MODEL = I_MP,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES10,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 10 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_3DZ))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 11).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 11).FirstOrDefault();
                    province.MODEL = I_3DZ;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES11;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 11,
                        MODEL = I_3DZ,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES11,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 11 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_PROF))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 12).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 12).FirstOrDefault();
                    province.MODEL = I_PROF;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES12;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 12,
                        MODEL = I_PROF,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES12,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 12 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_MDM))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 13).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 13).FirstOrDefault();
                    province.MODEL = I_MDM;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES13;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 13,
                        MODEL = I_MDM,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES13,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 13 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_DCP))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 14).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 14).FirstOrDefault();
                    province.MODEL = I_DCP;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES14;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 14,
                        MODEL = I_DCP,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES14,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 14 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_MAC))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 15).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 15).FirstOrDefault();
                    province.MODEL = I_MAC;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES15;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 15,
                        MODEL = I_MAC,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES15,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 15 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_JEC))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 16).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 16).FirstOrDefault();
                    province.MODEL = I_JEC;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES16;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 16,
                        MODEL = I_JEC,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES17,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 16 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_EC))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 17).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 17).FirstOrDefault();
                    province.MODEL = I_EC;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES17;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 17,
                        MODEL = I_EC,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES17,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 17 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_GF))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 18).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 18).FirstOrDefault();
                    province.MODEL = I_GF;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES18;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 18,
                        MODEL = I_GF,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES18,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 18 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_KZ))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 19).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 19).FirstOrDefault();
                    province.MODEL = I_KZ;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES19;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 19,
                        MODEL = I_KZ,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES19,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 19 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_LOG))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 20).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 20).FirstOrDefault();
                    province.MODEL = I_LOG;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES20;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 20,
                        MODEL = I_LOG,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES20,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 20 && x.DTYPE == 0);
                if (userinfo6 != null)
                {
                    _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                }
            }
            if (!string.IsNullOrWhiteSpace(I_EL1))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 21).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 21).FirstOrDefault();
                    province.MODEL = I_EL1;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES21;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 21,
                        MODEL = I_EL1,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES21,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 21 && x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 21 && x.DTYPE == 0);
                    if (userinfo6 != null)
                    {
                        _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(I_EL2))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 22).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 22).FirstOrDefault();
                    province.MODEL = I_EL2;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES22;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 22,
                        MODEL = I_EL2,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES22,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 22 && x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 22 && x.DTYPE == 0);
                    if (userinfo6 != null)
                    {
                        _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(I_TALLSN))
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 23).Where(x => x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE province = ListView.Where(x => x.MTYPE == 23).FirstOrDefault();
                    province.MODEL = I_TALLSN;
                    province.CREAT_DT = DateTime.Now;
                    province.NOTES = I_NOTES22;
                    province.CREAT_US = I_USER;
                    _imesRepository.UpdateFQA_FIRSTARTICLELINE(province);
                }
                else
                {
                    var province = new FQA_FIRSTARTICLELINE()
                    {
                        MO = MO,
                        MTYPE = 23,
                        MODEL = I_TALLSN,
                        CREAT_DT = DateTime.Now,
                        DTYPE = 0,
                        CREAT_US = I_USER,
                        NOTES = I_NOTES22,
                        LINE = LINE
                    };
                    _imesRepository.AddFQA_FIRSTARTICLELINE(province);
                }
            }
            else
            {
                var listCt1 = ListView.Where(x => x.MTYPE == 23 && x.DTYPE == 0).Count();
                if (listCt1 > 0)
                {
                    FQA_FIRSTARTICLELINE userinfo6 = ListView.FirstOrDefault(x => x.MTYPE == 23 && x.DTYPE == 0);
                    if (userinfo6 != null)
                    {
                        _imesRepository.DeleteFQA_FIRSTARTICLELINE(userinfo6);
                    }
                }
            }
            await _imesRepository.SaveAsync();
            return Ok(new ErrMessage { Err = Msg, success = true });
        }
        [HttpPost]
        [Route("AddMidTIME")]
        public async Task<IActionResult> AddMidTime(FQA_MIDTIME model)
        {
            string Msg;
            if (model.ITEMCODE == null)
            {
                Msg = "传入值为空！";
                return Ok(new JsonResult((Err: Msg, success: false)));
            }
            var lisview = await _imesRepository.GetFQA_MIDTIMEAsync(model);
            var listCt = lisview.Count();
            if (listCt == 0)
            {
                var province = new FQA_MIDTIME()
                {
                    ITEMCODE = model.ITEMCODE,
                    DLTIME = model.DLTIME,
                    DTYPE = model.DTYPE,
                    INPUTDATE = DateTime.Now
                };
                _imesRepository.AddFQA_MIDTIME(province);
                Msg = "添加成功";
            }
            else
            {
                model.INPUTDATE = DateTime.Now;
                _imesRepository.UpdateFQA_MIDTIME(model);
                Msg = "修改成功";
            }
            await _imesRepository.SaveAsync();
            //JsonResult result = new JsonResult((Err: Msg, success: true));
            return Ok(new ErrMessage { Err = Msg, success = true });
        }
        [HttpPost]
        [Route("SaveSid")]
        public async Task<IActionResult> SaveSid()
        {
            string s = HttpContext.Connection.RemoteIpAddress.ToString();
            DT_RETURN rETURN = new DT_RETURN();
            //判断临时表和主表数据是否重复--
            var listmo = await _imesRepository.ListODM_SIDKEYTEMP();
            string Msg;
            if (listmo != null)
            {
                var liter = listmo.ToList();
                if (liter.Count > 0)
                {
                    string err = "";
                    foreach (var item in liter)
                    {
                        err += item.SID + ",";
                    }
                    Msg = "数据库已经存在数据(不允许重复导入):";
                    rETURN.Msg = Msg + err;
                    rETURN.Success = false;
                    rETURN.Service = s;
                    return Ok(rETURN);
                }
            }
            var lis1 = _imesRepository.GetODM_SIDKEYTEMP();
            if (lis1 != null)
            {
                _imesRepository.SaveODM_SIDKEYTEMP();
                Msg = "数据保存完成";
                rETURN.Msg = Msg;
                rETURN.Success = true;
                rETURN.Service = s;
                return Ok(rETURN);
            }
            else
            {
                Msg = "临时表无数据";
                rETURN.Msg = Msg;
                rETURN.Success = false;
                rETURN.Service = s;
                return Ok(rETURN);
            }
        }
        [HttpPost]
        [Route("AddFQA_LINETIMEOW")]
        public async Task<IActionResult> AddFQA_LINETIMEOW([FromForm] IFormCollection fm)
        {
            var LINE = fm["LINE"].ToString() == "" ? "" : fm["LINE"].ToString().Trim();
            var DATE1 = fm["DATE1"].ToString() == "" ? "" : fm["DATE1"].ToString().Trim();
            var DATE2 = fm["DATE2"].ToString() == "" ? "" : fm["DATE2"].ToString().Trim();
            var did = Guid.NewGuid().ToString();
            var province = new FQA_LINETIMEOW()
            {
                ID = did,
                CREAT_DT = DateTime.Now,
                LINE = LINE,
                TIME1 = DATE1,
                TIME2 = DATE2
            };
            _imesRepository.AddFQA_LINETIMEOW(province);
            await _imesRepository.SaveAsync();
            return Ok(new ErrMessage { Err = "添加成功", success = true });
        }
        [HttpPost]
        [Route("AddFQA_OBARESHUOTIME")]
        public async Task<IActionResult> AddFQA_OBARESHUOTIME([FromForm] IFormCollection fm)
        {
            var ITEMCODE = fm["BOM"].ToString() == "" ? "" : fm["BOM"].ToString().Trim();
            var DLTIME = fm["DLTIME"].ToString() == "" ? "" : fm["DLTIME"].ToString().Trim();
            var lastResult = await _imesRepository.GetFQA_OBARESHUOTIME(ITEMCODE);
            if (lastResult!=null)
            {
                lastResult.DLTIME = Convert.ToInt32(DLTIME);
                lastResult.CRT_DATE = DateTime.Now;
                _imesRepository.UpdateFQA_OBARESHUOTIME(lastResult);
                await _imesRepository.SaveAsync();
                return Ok(new ErrMessage { Err = "更新成功", success = true });
            }
            var province = new FQA_OBARESHUOTIME()
            {
                 ITEMCODE= ITEMCODE,
                 DLTIME=Convert.ToInt32(DLTIME),
                 CRT_DATE=DateTime.Now

            };
            _imesRepository.AddFQA_OBARESHUOTIME(province);
            await _imesRepository.SaveAsync();
            return Ok(new ErrMessage { Err = "添加成功", success = true });
        }
        [HttpPost]
        [Route("Up850appendix")]
        public IActionResult Up850appendix([FromForm] IFormCollection fm)
        {
            var PONUMBER = fm["PONUMBER"].ToString() == "" ? "" : fm["PONUMBER"].ToString().Trim();
            var SHIPEARLYSTATUS = fm["SHIPEARLYSTATUS"].ToString() == "" ? "" : fm["SHIPEARLYSTATUS"].ToString().Trim();
            var STOP_RELEASE = fm["STOP_RELEASE"].ToString() == "" ? "" : fm["STOP_RELEASE"].ToString().Trim();
            var STOP_SHIP = fm["STOP_SHIP"].ToString() == "" ? "" : fm["STOP_SHIP"].ToString().Trim();
           
            _imesRepository.Up850appendix(PONUMBER, SHIPEARLYSTATUS, STOP_RELEASE, STOP_SHIP);
            return Ok(new ErrMessage { Err = "解除完", success = true });
        }
        [HttpPost]
        [Route("UpFqa_ck_sn")]
        public IActionResult UpFqa_ck_sn([FromForm] IFormCollection fm)
        {
            var SN1 = fm["SN"].ToString() == "" ? "" : fm["SN"].ToString().Trim();
            var PCUSER1 = fm["PCUSER"].ToString() == "" ? "" : fm["PCUSER"].ToString().Trim();
            var SN2 = AppHelper.GetSnByImei(SN1);
            if (!string.IsNullOrEmpty(SN2))
            {
                SN1 = SN2;
            }

            if (!AppHelper.ISCK_SNL(SN1))
            {
                return Ok(new ErrMessage { Err = "传入条码不需要取消抽样", success = true });
            }
            _imesRepository.UpdateFQA_CK_SN(SN1, PCUSER1); 
            return Ok(new ErrMessage { Err = "取消成功", success = true });
        }
        [HttpPost]
        [Route("AddLOCKREWORK")]
        public async Task<IActionResult> AddLOCKREWORK([FromForm] IFormCollection fm)
        {
            DT_RETURN rETURN = new DT_RETURN();
            var TY = fm["TY"].ToString() == "" ? "" : fm["TY"].ToString().Trim();
            var SN = fm["SN"].ToString() == "" ? "" : fm["SN"].ToString().Trim();
            var MO = fm["MO"].ToString() == "" ? "" : fm["MO"].ToString().Trim();
            var ISCHECK = fm["ISCHECK"].ToString() == "" ? "" : fm["ISCHECK"].ToString().Trim();
            var DATE1 = fm["DATE1"].ToString() == "" ? "" : fm["DATE1"].ToString().Trim();
            var DATE2 = fm["DATE2"].ToString() == "" ? "" : fm["DATE2"].ToString().Trim();
            var CRT_USER= fm["CRT_USER"].ToString() == "" ? "QA" : fm["CRT_USER"].ToString().Trim();
            var did = Guid.NewGuid().ToString();
            var Msg = "PASS！";
            if (TY == "INSERT")
            {
                var listsn = await _imesRepository.GetBARCODEREMP("", SN);
                if (listsn==null)
                {
                    var listsn1 = await _imesRepository.GetSNBY_IMEILINKNETCODE(SN);
                    if (listsn1==null)
                    {
                        rETURN.Msg = "扫描【" + SN + "】不存在MES中";
                        rETURN.Success = false;
                        rETURN.Service = "AddLOCKREWORK";
                        return Ok(rETURN);
                    }
                    SN = listsn1.SN;
                    MO = listsn1.WORKORDER;
                }
                else
                {
                    MO = listsn.WORKCODE;
                }

                var station = "包装投料";
                var lisst = await _imesRepository.GetMESTOOL("ODM_LOCKREWORK");
                if (lisst!=null)
                {
                    station = lisst.TOOLVER;
                }
                var lisss = await _imesRepository.GetMTL_SUB_ATTEMPER(MO, station);
                int subid;
                if (lisss != null)
                {
                    subid = (int)lisss.SERIAL_NUMBER;
                }
                else
                {

                    rETURN.Msg = "扫描【" + SN + "】不存在工序" + station;
                    rETURN.Success = false;
                    rETURN.Service = "AddLOCKREWORK";
                    return Ok(rETURN);

                }
                ODM_LOCKREWORK oDM_LOCKREWORK = new ODM_LOCKREWORK
                {
                    SUBID = subid,
                    UDID = did,
                    SN = SN,
                    CRT_DATE = DateTime.Now,
                    MO= MO,
                    LOCKIN=0,
                    STATION= station,
                    CRT_USER= CRT_USER
                };
                _imesRepository.AddODM_LOCKREWORK(oDM_LOCKREWORK);
                await _imesRepository.SaveAsync();
                rETURN.Msg = SN+ Msg;
                rETURN.Success = true;
                rETURN.Service = "AddLOCKREWORK";
                return Ok(rETURN);
            }
            if (TY == "SELECT")
            {
                if (!string.IsNullOrWhiteSpace(SN))
                {
                    ODM_IMEILINKNETCODE listsn2 = await _imesRepository.GetSNBY_IMEILINKNETCODE(SN);
                    if (listsn2 != null)
                    {
                        if (!string.IsNullOrWhiteSpace(listsn2.SN))
                        {
                            SN = listsn2.SN;
                        }
                       
                    }
                }
                var list = await _imesRepository.GetODM_LOCKREWORK(SN, MO, ISCHECK, DATE1, DATE2);
                return Ok(list); 
            }
            if (TY == "UPDATE")
            {

                if (string.IsNullOrWhiteSpace(SN))
                {
                    var listsn2 = await _imesRepository.GetSNBY_IMEILINKNETCODE(SN);
                    if (listsn2 != null)
                    {
                        SN = listsn2.SN;
                    }
                }
                var ll =await _imesRepository.GetODM_LOCKREWORKDID(SN);
                ll.LOCKIN = 1;
                _imesRepository.UpdateODM_LOCKREWORK(ll);
                await _imesRepository.SaveAsync();
                Msg = "取消完成";
            }
            rETURN.Msg = Msg;
            rETURN.Success = true;
            rETURN.Service = "AddLOCKREWORK";
            return Ok(rETURN);
        }
        [HttpPost]
        [Route("ADD_TEMPERATUREBOARD_DTL")]
        public async Task<IActionResult> ADD_TEMPERATUREBOARD_DTL([FromForm] IFormCollection fm)
        {
            var SN = fm["SN"].ToString();
            var USER = fm["USER"].ToString();
            var REMART = fm["REMART"].ToString();
            var FSTATUS = fm["FSTATUS"].ToString();
            if (string.IsNullOrEmpty(FSTATUS))
            {
                FSTATUS = "0";
            }
            var did = Guid.NewGuid().ToString();
            ODM_TEMPERATUREBOARD_DTL oDM_TEMPERATUREBOARD = new ODM_TEMPERATUREBOARD_DTL
            {
                DID = did.ToString(),
                SN = SN,
                CRTUSER = USER,
                REMART = REMART,
                CRTTIME = DateTime.Now,
                FSTATUS = Convert.ToInt32(FSTATUS)
            };
            _imesRepository.AddODM_TEMPERATUREBOARD_DTL(oDM_TEMPERATUREBOARD);
            ODM_TEMPERATUREBOARD listbarcode = await _imesRepository.GetODM_TEMPERATUREBOARD(SN);
            if (listbarcode == null)
            {
                ODM_TEMPERATUREBOARD oDM_ = new ODM_TEMPERATUREBOARD
                {
                    FSTATUS = Convert.ToInt32(FSTATUS),
                    SN = SN,
                    FCOUNT = 100
                };
                _imesRepository.AddODM_TEMPERATUREBOARD(oDM_);
            }
            else
            {
                if (FSTATUS == "1")
                {
                    listbarcode.FSTATUS = 1;
                    _imesRepository.UpdateTEMPERATUREBOARD(listbarcode);
                }
            }
            await _imesRepository.SaveAsync();
            //return Ok("添加成功");
            string Msg = "添加成功！";
            DT_RETURN rETURN = new DT_RETURN
            {
                Msg = Msg,
                Success = true,
                Service = "ADD_TEMPERATUREBOARD_DTL"
            };
            return Ok(rETURN);
        }
        [HttpPost]
        [Route("MULCHING")]
        public async Task<IActionResult> MULCHING([FromForm] IFormCollection fm)
        {
            string TYPE =string.IsNullOrWhiteSpace(fm["TYPE"].ToString())? "" : fm["TYPE"].ToString().Trim();
            string CLIENTCODE = string.IsNullOrWhiteSpace(fm["CLIENTCODE"].ToString())? "" : fm["CLIENTCODE"].ToString().Trim();
            string BARCODE = string.IsNullOrWhiteSpace(fm["BARCODE"].ToString()) ? "" : fm["BARCODE"].ToString().Trim();
            string LCMBARCODE = string.IsNullOrWhiteSpace(fm["LCMBARCODE"].ToString()) ? "" : fm["LCMBARCODE"].ToString().Trim();
            string TOPBARCODE = string.IsNullOrWhiteSpace(fm["TOPBARCODE"].ToString()) ? "" : fm["TOPBARCODE"].ToString().Trim();
            string UserID = string.IsNullOrWhiteSpace(fm["UserID"].ToString()) ? "" : fm["UserID"].ToString().Trim();
            string MO = string.IsNullOrWhiteSpace(fm["MO"].ToString()) ? "" : fm["MO"].ToString().Trim();
            string LINE = string.IsNullOrWhiteSpace(fm["LINE"].ToString()) ? "" : fm["LINE"].ToString().Trim();
            string T100CT = string.IsNullOrWhiteSpace(fm["T100CT"].ToString()) ? "0" : fm["T100CT"].ToString().Trim();
            string HOUR = string.IsNullOrWhiteSpace(fm["HOUR"].ToString()) ? "" : fm["HOUR"].ToString().Trim();
            switch (TYPE)
            {
                case "21":
                    return Ok(await _imesRepository.mULCHING21(BARCODE, CLIENTCODE, MO, T100CT));
                    //break;
                case "3":
                    return Ok(await _imesRepository.mULCHING3(BARCODE, CLIENTCODE, LCMBARCODE, TOPBARCODE, UserID,MO,LINE,T100CT));
                case "33":
                    return Ok(await _imesRepository.mULCHING33(BARCODE, CLIENTCODE));
                case "4":
                    return Ok(await _imesRepository.mULCHING4(BARCODE, HOUR, UserID));
                //break;
                default:
                    break;
            }
            return Ok();
            //return Ok(await _imesRepository.DelODM_TESTFAILNUM(TYPE));
        }
        [HttpPost]
        [Route("ADD_RE_BUG_TYPE")]
        public async Task<IActionResult> ADD_RE_BUG_TYPE([FromForm] IFormCollection fm)
        {
            var V_CLIENNAME = fm["V_CLIENNAME"].ToString() == "" ? "" : fm["V_CLIENNAME"].ToString().Trim();
            var V_CLIENCODE = fm["V_CLIENCODE"].ToString() == "" ? "" : fm["V_CLIENCODE"].ToString().Trim();
            var V_REASONTYPE = fm["V_REASONTYPE"].ToString() == "" ? "" : fm["V_REASONTYPE"].ToString().Trim();
            var V_CODE = fm["V_CODE"].ToString() == "" ? "" : fm["V_CODE"].ToString().Trim();
            var V_NAME = fm["V_NAME"].ToString() == "" ? "" : fm["V_NAME"].ToString().Trim();
            var V_DESCRIBE = fm["V_DESCRIBE"].ToString() == "" ? "" : fm["V_DESCRIBE"].ToString().Trim();
            var V_BUG_TYPE = fm["V_BUG_TYPE"].ToString() == "" ? "" : fm["V_BUG_TYPE"].ToString().Trim();
            var V_WORKCODE = fm["V_WORKCODE"].ToString() == "" ? "" : fm["V_WORKCODE"].ToString().Trim(); 
            return Ok( await _imesRepository.ADD_RE_BUG_TYPE(V_CLIENNAME, V_CLIENCODE, V_REASONTYPE, V_CODE, V_NAME, V_DESCRIBE, V_BUG_TYPE, V_WORKCODE));
        }
        [HttpPost]
        [Route("CHA_RE_BUG_TYPE")]
        public async Task<IActionResult> CHA_RE_BUG_TYPE([FromForm] IFormCollection fm)
        {
            var V_ID = fm["V_ID"].ToString() == "" ? "" : fm["V_ID"].ToString().Trim(); 
            var V_NAME = fm["V_NAME"].ToString() == "" ? "" : fm["V_NAME"].ToString().Trim();
            var V_DESCRIBE = fm["V_DESCRIBE"].ToString() == "" ? "" : fm["V_DESCRIBE"].ToString().Trim();
            var V_BUG_TYPE = fm["V_BUG_TYPE"].ToString() == "" ? "" : fm["V_BUG_TYPE"].ToString().Trim(); 
            return Ok(await _imesRepository.CHA_RE_BUG_TYPE(V_ID, V_NAME, V_DESCRIBE, V_BUG_TYPE));
        }
        [HttpPost]
        [Route("DEL_RE_BUG_TYPE")]
        public async Task<IActionResult> DEL_RE_BUG_TYPE([FromForm] IFormCollection fm)
        {
            var V_ID = fm["V_ID"].ToString() == "" ? "" : fm["V_ID"].ToString().Trim();
            return Ok(await _imesRepository.DEL_RE_BUG_TYPE(V_ID));
        }
        [HttpPost]
        [Route("COPY_RE_BUG_TYPE")]
        public async Task<IActionResult> COPY_RE_BUG_TYPE([FromForm] IFormCollection fm)
        {
            var V_CLIENT = fm["V_CLIENT"].ToString() == "" ? "" : fm["V_CLIENT"].ToString().Trim();
            var V_CLIENT1 = fm["V_CLIENT1"].ToString() == "" ? "" : fm["V_CLIENT1"].ToString().Trim();
            var V_REASONTYPE = fm["V_REASONTYPE"].ToString() == "" ? "" : fm["V_REASONTYPE"].ToString().Trim(); 
            return Ok(await _imesRepository.COPY_RE_BUG_TYPE(V_CLIENT, V_CLIENT1, V_REASONTYPE));
        }
        [HttpPost]
        [Route("UPDATE_TEMPERATUREBOARD")]
        public async Task<IActionResult> UPDATE_TEMPERATUREBOARD([FromForm] IFormCollection fm)
        {
            var SN = fm["SN"].ToString();
            //var USER = fm["USER"].ToString();
            var FCOUNT = fm["FCOUNT"].ToString();
            var FSTATUS = fm["FSTATUS"].ToString();
            ODM_TEMPERATUREBOARD listbarcode = await _imesRepository.GetODM_TEMPERATUREBOARD(SN);
            bool bl;
            string Msg;
            if (listbarcode == null)
            {
                Msg = SN + "系统不存在";
                bl = false;
                goto Endpoint;
            }
            else
            {
                if (!string.IsNullOrEmpty(FCOUNT))
                {
                    listbarcode.FCOUNT = Convert.ToInt32(FCOUNT);

                }
                if (!string.IsNullOrEmpty(FSTATUS))
                {
                    listbarcode.FSTATUS = Convert.ToInt32(FSTATUS);

                }
                _imesRepository.UpdateTEMPERATUREBOARD(listbarcode);
                await _imesRepository.SaveAsync();
                Msg = "已更新";
                bl = true;
                goto Endpoint;
            }
        Endpoint:
            DT_RETURN rETURN = new DT_RETURN
            {
                Msg = Msg,
                Success = bl,
                Service = "UPDATE_TEMPERATUREBOARD"
            };
            return Ok(rETURN);
        }


        [HttpPost]
        [Route("GetListDATE")]
        public IActionResult GetListDATE([FromForm] IFormCollection fm)
        {
            var DT_LINE = fm["DT_LINE"].ToString() == "" ? "" : fm["DT_LINE"].ToString().Trim();
            var DT_WORK = fm["DT_WORK"].ToString() == "" ? "" : fm["DT_WORK"].ToString().Trim();
            var DT_CHECK = fm["DT_CHECK"].ToString() == "" ? "" : fm["DT_CHECK"].ToString().Trim();
            var DT_TIME = fm["DT_TIME"].ToString() == "" ? "" : fm["DT_TIME"].ToString().Trim();

            return Ok(new ErrMessage { Err = "添加成功", success = true });
        }
        [HttpPost]
        [Route("GetListDATE1")]
        public  IActionResult  GetListDATE1()
        { 
            return Ok(new ErrMessage { Err = "添加成功", success = true });
        }
        [HttpPost]
        [Route("Post")]
        public void Post()
        {

            return;
        }
        [HttpPost]
        [Route("DELMidTIME")]
        public IActionResult DELMidTIME(dynamic data)
        {
            //HttpContext context;
            //string OriSelect = HttpContext.Request["FUNTION"];
            _ = HttpContext.Request.Headers["x-correlation-id"].ToString();
            //var correlationId1 = HttpContext.Request.["x-correlation-id"].ToString();
            //var strName = Convert.ToString(data.NAME);
            //var oCharging = Newtonsoft.Json.JsonConvert.DeserializeObject<TB_CHARGING>(Convert.ToString(data.Charging));
            //return strName;
            #region MyRegion
            //compsite object type:
            _ = JsonConvert.DeserializeObject<FQA_MIDTIME>(data.ToString());

            ////list or dictionary object type 
            //var c1 = JsonConvert.DeserializeObject<ComplexObject1>(data.c1.ToString());

            //var c2 = JsonConvert.DeserializeObject<ComplexObject2>(data.c2.ToString());
            //string a = Request.Form["ITEMCODE"];
            //string OriSelect = Request.["FUNTION"]; 
            #endregion
            JsonResult result = new JsonResult(new { Err = "无数据记录", success = false });
            return result;
        }
        [HttpDelete]
        [Route("DelFQA_OBARESHUOTIME")]
        // DELETE: api/ApiWithActions/5
        public async Task<IActionResult> DelFQA_OBARESHUOTIME(string ID)
        {
            try
            { 
                if (!string.IsNullOrWhiteSpace(ID))
                {
                    var lastResult = await _imesRepository.GetFQA_OBARESHUOTIME(ID);
                    if (lastResult != null)
                    {
                        _imesRepository.DeleteFQA_OBARESHUOTIME(lastResult);
                        await _imesRepository.SaveAsync();
                        string Msg = "删除成功。";
                        return Ok(new ErrMessage { Err = Msg, success = true });
                    }
                    else
                    {
                        string Msg = "传人编码不存在，不需要删除。";
                        return Ok(new ErrMessage { Err = Msg, success = false });

                    } 
                }
                else
                {
                    string Msg = "传人编码为空。";
                    return Ok(new ErrMessage { Err = Msg, success = false });
                }
            }
            catch (Exception ex)
            {
                string Msg = ex.Message;
                return Ok(new ErrMessage { Err = Msg, success = false });
            }
        }
        [HttpDelete]
        [Route("DelFQA_LINETIMEOW")]
        // DELETE: api/ApiWithActions/5
        public async Task<IActionResult> DelFQA_LINETIMEOW(string ID)
        {
            try
            {
                // var ID = fm["ID"].ToString() == "" ? "" : fm["ID"].ToString().Trim();
                if (!string.IsNullOrWhiteSpace(ID))
                {
                    bool bl = _imesRepository.DelFQA_LINETIMEOW(ID);
                    await _imesRepository.SaveAsync();
                    if (bl)
                    {
                        string Msg = "删除完成。";
                        return Ok(new ErrMessage { Err = Msg, success = true });
                    }
                    else
                    {
                        string Msg = "删除失败。";
                        return Ok(new ErrMessage { Err = Msg, success = false });

                    }
                }
                else
                {
                    string Msg = "传人ID为空。";
                    return Ok(new ErrMessage { Err = Msg, success = false });
                }
            }
            catch (Exception ex)
            {
                string Msg = ex.Message;
                return Ok(new ErrMessage { Err = Msg, success = false });
            }
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string DeleJson)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject jobject = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(DeleJson);
                string ProcessGUID = jobject.Properties().Any(p => p.Name == "ProcessGUID") ? jobject["ProcessGUID"].ToString() : "";
                if (Equals(ProcessGUID, "CDA0436B-0F05-4922-B3D8-93E9DF3CE0CA"))
                {
                    string ITEMCODE = jobject.Properties().Any(p => p.Name == "ITEMCODE") ? jobject["ITEMCODE"].ToString() : "";
                    FQA_MIDTIME delSysUser3 = new FQA_MIDTIME() { ITEMCODE = ITEMCODE };
                    //删除数据--
                    //var Ct = OracleDbContext.MidTimes.Where(x => x.ITEMCODE == ITEMCODE).Count(); 
                    FQA_MIDTIME userinfo6 = await _imesRepository.GetFQA_MIDTIMEFirstAsync(ITEMCODE);
                    //FQA_MIDTIME province = OracleDbContext.MidTimes.Where(x => x.ITEMCODE == ITEMCODE).Take<FQA_MIDTIME>; //查询全部 
                    //OracleDbContext.RemoveRange(province);//删除  查到数据来删除
                    //var user = OracleDbContext.MidTimes.FirstOrDefault(d => d.ITEMCODE == ITEMCODE);
                    //FQA_MIDTIME user1 = OracleDbContext.MidTimes.Where(d => d.ITEMCODE == ITEMCODE).FirstOrDefault();
                    if (userinfo6 != null)
                    {
                        _imesRepository.DeleteFQA_MIDTIME(userinfo6);
                        await _imesRepository.SaveAsync();
                        string Msg = ITEMCODE + "删除成功";
                        return Ok(new ErrMessage { Err = Msg, success = true });
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                string Msg = ex.Message;
                return Ok(new ErrMessage { Err = Msg, success = false });
            }
            return Ok();
        }
        [HttpPost("DownExcel")]
        public async Task<IActionResult> DownExcel([FromForm] IFormCollection fm)
        {

            //string MODEL = fm["MODEL"].ToString() == "" ? "" : fm["MODEL"].ToString().Trim();
            var MO = fm["MO"].ToString() == "" ? "" : fm["MO"].ToString().Trim();
            var SN = fm["SN"].ToString() == "" ? "" : fm["SN"].ToString().Trim();
            var LINE = fm["LINE"].ToString() == "" ? "" : fm["LINE"].ToString().Trim();
            var TY = fm["TY"].ToString() == "" ? "" : fm["TY"].ToString().Trim();
            var DATE1 = fm["DATE1"].ToString() == "" ? "" : fm["DATE1"].ToString().Trim();
            var DATE2 = fm["DATE2"].ToString() == "" ? "" : fm["DATE2"].ToString().Trim();
            var ISC = fm["ISC"].ToString() == "" ? "0" : fm["ISC"].ToString().Trim();
            var ISOK = fm["ISOK"].ToString() == "" ? "" : fm["ISOK"].ToString().Trim();
            var Ems = fm["Ems"].ToString() == "" ? "" : fm["Ems"].ToString().Trim();
            //GetFQA_CK_SNLOG(string MO, string SN, string LINE, string TY, string DATE1, string DATE2, string ISC,string ISOK,string Ems)
            IEnumerable<FQA_CK_SNLOG> rsVal = await _imesRepository.GetFQA_CK_SNLOG(MO, SN, LINE, TY, DATE1, DATE2, ISC, ISOK, Ems);
            //var ListVIEW = new List<FQA_CK_SNLOG>();
            IEnumerable<FQA_CK_SNLOGDtos> rsVal1 = _mapper.Map<IEnumerable<FQA_CK_SNLOGDtos>>(rsVal);
            //ListVIEW = rsVal.ToList();
            if (new List<FQA_CK_SNLOG>() == null)
            {
                return Ok();
            }
            //DateTime date1 = Convert.ToDateTime(DATE1);
            //int pYear = date1.Year;
            //int pMonth = date1.Month;
            // IEnumerable<Student> list = Student.GetStudents();
            ExcelHelper excelHeper = new ExcelHelper();
            List<ExcelGridModel> config = new List<ExcelGridModel>
            {
                new ExcelGridModel{name="MO",label="工单号", align="left",},
                new ExcelGridModel{name="SN",label="SN", align="left",},
                new ExcelGridModel{name="LINE",label="线体", align="left",},
                new ExcelGridModel{name="ISOK",label="是否已检", align="left",},
                new ExcelGridModel{name="TLL",label="星级", align="left",},
                new ExcelGridModel{name="PCNAME",label="电脑名", align="left",},
                new ExcelGridModel{name="CREATEDATE",label="生成时间", align="left",},
                new ExcelGridModel{name="T_DES",label="备注", align="left",},
            };
            var fileName = fm["FileName"].ToString();
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "nOName.xls";
            }
            return excelHeper.ExcelDownload(rsVal1, config, fileName);
        }
        [HttpPost("DownExcelFQA_OBA_SAMPLE")]
        public async Task<IActionResult> DownExcelFQA_OBA_SAMPLE([FromForm] IFormCollection fm)
        {

            //string MODEL = fm["MODEL"].ToString() == "" ? "" : fm["MODEL"].ToString().Trim();
            var ID = fm["ID"].ToString() == "" ? "" : fm["ID"].ToString().Trim();  
            IEnumerable<Model.FQA_OBA_SAMPLE> rsVal = await _imesRepository.GetFQA_OBA_SAMPLE(ID);
            //var ListVIEW = new List<FQA_CK_SNLOG>();
            //IEnumerable<FQA_CK_SNLOGDtos> rsVal1 = _mapper.Map<IEnumerable<FQA_CK_SNLOGDtos>>(rsVal);
            //ListVIEW = rsVal.ToList();
            if (rsVal == null)
            {
                return Ok();
            } 
            ExcelHelper excelHeper = new ExcelHelper();
            List<ExcelGridModel> config = new List<ExcelGridModel>
            {
                new ExcelGridModel{name="ID",label="ID", align="left",},
                new ExcelGridModel{name="MIDCARTONID",label="MIDCARTONID", align="left",},
                new ExcelGridModel{name="IMEI",label="IMEI", align="left",},
                new ExcelGridModel{name="SN",label="SN", align="left",},
                new ExcelGridModel{name="IMEI2",label="IMEI2", align="left",},
                new ExcelGridModel{name="MEID",label="MEID", align="left",},
                new ExcelGridModel{name="INPUT_DATE",label="INPUT_DATE", align="left",},
                new ExcelGridModel{name="FQA_OBA_ID",label="FQA_OBA_ID", align="left",},
            };
            var fileName = fm["FileName"].ToString() == "" ? "" : fm["FileName"].ToString().Trim();
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "nOName.xls";
            }
            return excelHeper.ExcelDownload(rsVal, config, fileName);
        }
        [HttpPost]
        [Route("SidPostFile")]
        public async Task<IActionResult> SidPostFile([FromForm] IFormCollection formCollection)
        {
            var PRODUCT_MODEL = formCollection["PRODUCT_MODEL"].ToString() == "" ? "" : formCollection["PRODUCT_MODEL"].ToString().Trim();
            string result = "Fail"; 
            string Uid = Guid.NewGuid().ToString();
            string s = HttpContext.Connection.RemoteIpAddress.ToString();
            _imesRepository.DelODM_SIDKEYTEMP();//删除表 
            FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
            foreach (IFormFile file in fileCollection)
            {
                StreamReader reader = new StreamReader(file.OpenReadStream());
                string content = reader.ReadToEnd();
                //var dd = reader.ReadLine();
                string name = file.FileName;
                string[] sArray = content.Split("\r\n");
                List<string> list = new List<string>(sArray);
                DT_RETURN rETURN = new DT_RETURN();
                string Msg;
                if (list.Distinct().Count() != list.Count())
                {
                    Msg = "导入文件【" + name + "】存在重复数据！";
                    rETURN.Msg = Msg;
                    rETURN.Success = false;
                    rETURN.Service = s;
                    return Ok(rETURN);
                }
                //上传临时表
                if (list.Count > 0)
                {
                    foreach (string item in list)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            var item1 = item.Trim();
                            if (item1.Length != 16)
                            {
                                Msg = "导入SID【" + item + "】位数不等于16，请确认！";
                                rETURN.Msg = Msg;
                                rETURN.Success = false;
                                rETURN.Service = s;
                                return Ok(rETURN);
                            }
                            ODM_SIDKEYTEMP oDM_SIDKEYTEMP = new ODM_SIDKEYTEMP
                            {
                                SID = item1,
                                KEY = Uid,
                                CREATE_DATE = DateTime.Now,
                                ISUSER = 0,
                                PRODUCT_MODEL= PRODUCT_MODEL
                            };
                            _imesRepository.AddODM_SIDKEYTEMP(oDM_SIDKEYTEMP);
                        }
                    }
                    await _imesRepository.SaveAsync();
                }
                else
                {
                    Msg = "导入文件【" + name + "】为空文件！";
                    rETURN.Msg = Msg;
                    rETURN.Success = false;
                    rETURN.Service = s;
                    return Ok(rETURN);
                }
                //判断临时表和主表数据是否重复--
                var listmo = await _imesRepository.ListODM_SIDKEYTEMP();
                var liter = listmo.ToList();
                if (liter.Count > 0)
                {
                    string err = "";
                    foreach (var item in liter)
                    {
                        err += item.SID + ",";
                    }
                    Msg = "数据库已经存在数据(不允许重复导入):";
                    rETURN.Msg = Msg + err;
                    rETURN.Success = false;
                    rETURN.Service = s;
                    return Ok(rETURN);
                }
                Msg = "导入文件【" + name + "】数据校验完成,总数【" + list.Count + "】请点击保存按钮提交结果！";
                rETURN.Msg = Msg;
                rETURN.Success = true;
                rETURN.Service = s;
                //写LOG
                ODM_SIDKEYLOG oDM_SIDKEYLOG = new ODM_SIDKEYLOG
                {
                    KEY = Uid,
                    SNCOUNT = list.Count.ToString(),
                    FILENAME = name,
                    CREATE_DATE = DateTime.Now,
                    PRODUCT_MODEL= PRODUCT_MODEL
                };
                _imesRepository.AddODM_SIDKEYLOG(oDM_SIDKEYLOG);
                await _imesRepository.SaveAsync();
                return Ok(rETURN);
            }

            return Ok(result);
        }

        [Route("ExportByTable_PRESSURE")]
        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> ExportByTable_PRESSURE([FromForm] IFormCollection fm)
        {
            string CLIENTCODE = string.IsNullOrWhiteSpace(fm["CLIENTCODE"].ToString()) ? "" : fm["CLIENTCODE"].ToString().Trim();
            string DATE1 = string.IsNullOrWhiteSpace(fm["DATE1"].ToString()) ? "" : fm["DATE1"].ToString().Trim();
            string DATE2 = string.IsNullOrWhiteSpace(fm["DATE2"].ToString()) ? "" : fm["DATE2"].ToString().Trim();
            string SN = string.IsNullOrWhiteSpace(fm["SN"].ToString()) ? "" : fm["SN"].ToString().Trim();
            string MO = string.IsNullOrWhiteSpace(fm["MO"].ToString()) ? "" : fm["MO"].ToString().Trim();
            string LINE = string.IsNullOrWhiteSpace(fm["LINE"].ToString()) ? "" : fm["LINE"].ToString().Trim();
            string STATION = string.IsNullOrWhiteSpace(fm["STATION"].ToString()) ? "" : fm["STATION"].ToString().Trim();

            if (string.IsNullOrWhiteSpace(MO) && string.IsNullOrWhiteSpace(SN) && string.IsNullOrWhiteSpace(DATE1))
            {
                RT_MESSAGE err = new RT_MESSAGE
                {
                    success = false,
                    Err = "工单、SN、时间不能同时为空"
                };
                return Ok(err);
            }
            if (string.IsNullOrWhiteSpace(DATE1) && !string.IsNullOrWhiteSpace(DATE2))
            {
                RT_MESSAGE err = new RT_MESSAGE
                {
                    success = false,
                    Err = "开始时间不能为空"
                };
                return Ok(err);
            }
            if (!string.IsNullOrWhiteSpace(DATE1) && string.IsNullOrWhiteSpace(DATE2))
            {
                RT_MESSAGE err = new RT_MESSAGE
                {
                    success = false,
                    Err = "结束时间不能为空"
                };
                return Ok(err);
            }
            RT_MESSAGE Lg =   await _imesRepository.GetODM_PRESSURE(MO, SN, DATE1, DATE2, STATION, LINE, CLIENTCODE);
            IEnumerable<DA_ODM_PRESSURE> rsVal = Lg.dA_ODM_PRESSURE;
            if (rsVal == null)
            {
                RT_MESSAGE err = new RT_MESSAGE
                {
                    success = false,
                    Err = "无数据记录"
                };
                return Ok(err);
            }
            ExcelHelper excelHeper = new ExcelHelper();
            List<ExcelGridModel> config = new List<ExcelGridModel>
            {   new ExcelGridModel{name="ID",label="ID", align="left",},
                new ExcelGridModel{name="BARCODE",label="SN", align="left",},
                new ExcelGridModel{name="TYPE",label="TYPE", align="left",},
                new ExcelGridModel{name="CREAT_TIME",label="CREAT_TIME", align="left",},
                new ExcelGridModel{name="MO",label="MO", align="left",},
                new ExcelGridModel{name="LINE",label="LINE", align="left",},
                new ExcelGridModel{name="CREAT_PERSON",label="CREAT_PERSON", align="left",},
                new ExcelGridModel{name="SEG1",label="SEG1", align="left",},
                new ExcelGridModel{name="SEG2",label="SEG2", align="left",},
                new ExcelGridModel{name="SEG3",label="SEG3", align="left",},
                new ExcelGridModel{name="CLIENTCODE",label="CLIENTCODE", align="left",},
            };
            string Names = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName =$"保压数据查询{Names}.xls";
            return excelHeper.ExcelDownload(rsVal, config, fileName);
        }

        [Route("ExportByTable_PRESSFREE")]
        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> ExportByTable_PRESSFREE([FromForm] IFormCollection fm)
        { 
            string DATE1 = string.IsNullOrWhiteSpace(fm["DATE1"].ToString()) ? "" : fm["DATE1"].ToString().Trim();
            string DATE2 = string.IsNullOrWhiteSpace(fm["DATE2"].ToString()) ? "" : fm["DATE2"].ToString().Trim();
            string SN = string.IsNullOrWhiteSpace(fm["SN"].ToString()) ? "" : fm["SN"].ToString().Trim();
            string ID_USER = string.IsNullOrWhiteSpace(fm["ID_USER"].ToString()) ? "" : fm["ID_USER"].ToString().Trim(); 

            if (string.IsNullOrWhiteSpace(ID_USER) && string.IsNullOrWhiteSpace(SN) && string.IsNullOrWhiteSpace(DATE1))
            {
                RT_MESSAGE err = new RT_MESSAGE
                {
                    success = false,
                    Err = "释放ID、SN、时间不能同时为空"
                };
                return Ok(err);
            }
            if (string.IsNullOrWhiteSpace(DATE1) && !string.IsNullOrWhiteSpace(DATE2))
            {
                RT_MESSAGE err = new RT_MESSAGE
                {
                    success = false,
                    Err = "开始时间不能为空"
                };
                return Ok(err);
            }
            if (!string.IsNullOrWhiteSpace(DATE1) && string.IsNullOrWhiteSpace(DATE2))
            {
                RT_MESSAGE err = new RT_MESSAGE
                {
                    success = false,
                    Err = "结束时间不能为空"
                };
                return Ok(err);
            }
            RT_MESSAGE Lg = await _imesRepository.GetODM_PRESSUREFREE( SN, DATE1, DATE2, ID_USER);
            IEnumerable<DA_ODM_PRESSFREE> rsVal = Lg.dA_ODM_PRESSFREE;
            if (rsVal == null)
            {
                RT_MESSAGE err = new RT_MESSAGE
                {
                    success = false,
                    Err = "无数据记录"
                };
                return Ok(err);
            }
            ExcelHelper excelHeper = new ExcelHelper();
            List<ExcelGridModel> config = new List<ExcelGridModel>
            {   
                new ExcelGridModel{name="BARCODE",label="条码", align="left",},
                new ExcelGridModel{name="TYPE",label="工序", align="left",},
                new ExcelGridModel{name="CREAT_TIME",label="CREAT_TIME", align="left",}, 
                new ExcelGridModel{name="CREAT_PERSON",label="CREAT_PERSON", align="left",},
                new ExcelGridModel{name="LINKSN",label="关联条码", align="left",},
                new ExcelGridModel{name="FRR_TIME",label="释放时间", align="left",},
                new ExcelGridModel{name="CRT_USER",label="释放ID", align="left",}, 
            };
            string Names = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"保压释放数据记录{Names}";
            return excelHeper.ExcelDownload(rsVal, config, fileName);
        }
    }   
}