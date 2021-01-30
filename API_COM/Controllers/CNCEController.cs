using API_COM.Helper;
using API_COM.Modelclass;
using API_COM.Servise;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_COM.Controllers
{
    [EnableCors("any")]
    [ApiController]
    [Route("[controller]")]
    public class CNCEController : ControllerBase
    {
        private readonly CNCEInterface _cNCEInterface;
        public CNCEController(CNCEInterface cNCEInterface)
        {
            _cNCEInterface = cNCEInterface ?? throw new ArgumentNullException(nameof(cNCEInterface));
        }
        // GET: api/<CNCEController>
        [HttpGet]
        [SkipActionFilter]
        public IEnumerable<string> Get()
        {
            //string V0 = MD5_F.Encrypt("CNCE2020cw");
            //string Vl = MD5_F.MD5Decrypt(V0, V0);
            return new string[] { "value1", "value2" };
        }

        // GET api/<CNCEController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        [HttpGet]
        [Route("GetMd5")]
        [SkipActionFilter]
        public string GetMd5(string Md5)
        {
            var MD5_f = MD5_F.Encrypt(Md5);
            return MD5_f;
        }

        [HttpGet]
        [Route("ExportTxt")]
        [SkipActionFilter]
        public IActionResult ExportTxt()
        {

            return File(new byte[0], "text/plain", "welcome.txt");
        }
        [HttpGet]
        [Route("GetportTxt")]
        [SkipActionFilter]
        public IActionResult GetportTxt(string dn, string UserID)
        {
            string warehouse = AppHelper.V_WAREHOUSE_IDEN(dn);
            string warehouseid = AppHelper.V_WAREHOUSE_ID(dn);
            string factory_id = AppHelper.MES_FACTORY_ID(UserID);
            if (string.IsNullOrEmpty(factory_id))
            {
                factory_id = "ZN";
            }
            string Las = "WMS2021" + factory_id;
            string LableNames = AppHelper.V_SCRIPT(Las);
            if (string.IsNullOrWhiteSpace(LableNames))
            {
                var dTReturn = new DT_RETURN
                {
                    Success = false,
                    Service = "GetportTxt",
                    Msg = "请维护脚本:" + Las
                };
                return Ok(dTReturn);
            }
            if (string.IsNullOrEmpty(warehouse))
            {
                warehouse = "Null";
            }
            LableNames = LableNames.Replace("$DN$", dn);
            LableNames = LableNames.Replace("$WAREHOUSE$", warehouse);
            LableNames = LableNames.Replace("$WAREID$", warehouseid);
            //WAREID
            var dTReturn1 = new DT_RETURN
            {
                Success = true,
                Service = "GetportTxt",
                Msg = LableNames
            };
            return Ok(dTReturn1);
        }
        [HttpGet]
        [Route("WMS_ADRESSS")]
        [SkipActionFilter]
        public IActionResult WMS_ADRESSS(string plan)
        {
            string adr = AppHelper.MES_FACTORY("ADRH1", plan);
            DT_RETURN dT_RETURN = new DT_RETURN
            {
                Success = true,
                Msg = adr
            };
            return Ok(dT_RETURN);
        }
        [HttpGet]
        [Route("DELIVERY_STOCK")]
        [SkipActionFilter]
        public IActionResult DELIVERY_STOCK(string plan)
        {
            var adr = AppHelper.MES_FACTORY("H1", plan);
            var dTReturn = new DT_RETURN
            {
                Success = true,
                Msg = adr
            };
            return Ok(dTReturn);
        }
        #region MyRegion
        // POST api/<CNCEController>
        [HttpPost]
        [Route("purchaseOrderReceiveData")]
        public IActionResult purchaseOrderReceiveData(purchaseOrderReceiveData receiveDeliveryData)
        {
            string appKey = receiveDeliveryData.appKey;
            string appMethod = receiveDeliveryData.appMethod;
            if (appKey != "ENyuRk8Ebm")
            {
                H1_MESSAGE h1_MESSAGE = new H1_MESSAGE
                {
                    code = "404",
                    message = "appKey错误"
                };
                return Ok(h1_MESSAGE);
            }
            if (appMethod != "YSZX2Igx11Svb3IyK467hOSqH3CdyxIL" && appMethod != "33O4kWiYYFGm1pWp68leB6ANtp8HOHg3")
            {
                H1_MESSAGE h1_MESSAGE = new H1_MESSAGE
                {
                    code = "404",
                    message = "appMethod错误"
                };
                return Ok(h1_MESSAGE);
            }
            string cname = string.Empty;
            if (appMethod == "YSZX2Igx11Svb3IyK467hOSqH3CdyxIL")
            {
                cname = "TS";
            }
            if (appMethod == "33O4kWiYYFGm1pWp68leB6ANtp8HOHg3")
            {
                cname = "H1";
            }
            string json = "";// receiveDeliveryData.data;
            if (!jsonHelp.IsJson(json))
            {
                H1_MESSAGE h1_MESSAGE = new H1_MESSAGE
                {
                    code = "404",
                    message = "data非jsons数组"
                };
                return Ok(h1_MESSAGE);
            }
            Newtonsoft.Json.Linq.JArray arrdata = Newtonsoft.Json.Linq.JArray.Parse(json);
            if (arrdata.Count > 0)
            {
                foreach (Newtonsoft.Json.Linq.JToken jToken in arrdata)
                {
                    string delivery_id = jToken["delivery_id"] == null ? "" : jToken["delivery_id"].ToString();//供应商发货单号
                    string batch_no = jToken["batch_no"] == null ? "" : jToken["batch_no"].ToString();//客户侧对应的唯一入库批次号
                    string batch_line = jToken["batch_line"] == null ? "" : jToken["batch_line"].ToString();//客户侧对应的唯一入库行项目
                    string prod_code_cust = jToken["prod_code_cust"] == null ? "" : jToken["prod_code_cust"].ToString();//客户产品编码
                    string actual_inbound_qty = jToken["actual_inbound_qty"] == null ? "" : jToken["actual_inbound_qty"].ToString();//客户实际入库数量
                    string actual_inbound_date = jToken["actual_inbound_date"] == null ? "" : jToken["actual_inbound_date"].ToString();//客户实际入库时间
                    string cust_purchaseorder = jToken["cust_purchaseorder"] == null ? "" : jToken["cust_purchaseorder"].ToString();//客户采购凭证号
                    string cust_purchaseline = jToken["cust_purchaseline"] == null ? "" : jToken["cust_purchaseline"].ToString();//客户采购凭证行号
                    string totalreceiv_qty = jToken["totalreceiv_qty"] == null ? "" : jToken["totalreceiv_qty"].ToString();//累计入库数量 
                    System.Threading.Tasks.Task<H1_MESSAGE> resquit = _cNCEInterface.I_receiveDeliveryData(delivery_id, batch_no, batch_line, prod_code_cust, actual_inbound_qty, actual_inbound_date, cust_purchaseorder, cust_purchaseline, totalreceiv_qty, cname);
                    if (resquit.Result.code == "500")
                    {
                        H1_MESSAGE h1_MESSAGE = new H1_MESSAGE
                        {
                            code = "500",
                            message = "操作失败"
                        };
                        return Ok(h1_MESSAGE);
                    }
                }
                H1_MESSAGE H1_MESSAGE = new H1_MESSAGE
                {
                    code = "200",
                    message = "提交OK"
                };
                return Ok(H1_MESSAGE);
            }
            else
            {
                H1_MESSAGE h1_MESSAGE = new H1_MESSAGE
                {
                    code = "404",
                    message = "无数据记录"
                };
                return Ok(h1_MESSAGE);
            }
            return Ok();
        }
        #endregion 
        [HttpPost]
        [Route("purchaseOrderReceive")]
        [SkipActionFilter]
        public IActionResult purchaseOrderReceive(pOrderReceive receiveDeliveryData)
        {
            string appKey = receiveDeliveryData.appKey;
            string appMethod = receiveDeliveryData.appMethod;
            if (appKey != "ENyuRk8Ebm")
            {
                H1_MESSAGE h1_MESSAGE = new H1_MESSAGE
                {
                    code = "404",
                    message = "appKey错误"
                };
                return Ok(h1_MESSAGE);
            }
            if (appMethod != "YSZX2Igx11Svb3IyK467hOSqH3CdyxIL" && appMethod != "33O4kWiYYFGm1pWp68leB6ANtp8HOHg3")
            {
                H1_MESSAGE h1_MESSAGE = new H1_MESSAGE
                {
                    code = "404",
                    message = "appMethod错误"
                };
                return Ok(h1_MESSAGE);
            }
            string cname = string.Empty;
            if (appMethod == "YSZX2Igx11Svb3IyK467hOSqH3CdyxIL")
            {
                cname = "TS";
            }
            if (appMethod == "33O4kWiYYFGm1pWp68leB6ANtp8HOHg3")
            {
                cname = "H1";
            }
            List<Datum> data = new List<Datum>();
            data = receiveDeliveryData.data;

            if (data.Count == 0)
            {
                H1_MESSAGE h1_MESSAGE = new H1_MESSAGE
                {
                    code = "404",
                    message = "data无数据"
                };
            }
            for (int i = 0; i < data.Count; i++)
            {
                Datum datum = data[i];
                string delivery_id = datum.delivery_id;//供应商发货单号
                string batch_no = datum.batch_no; //客户侧对应的唯一入库批次号
                string batch_line = datum.batch_line; //客户侧对应的唯一入库行项目
                string prod_code_cust = datum.prod_code_cust;//客户产品编码
                string actual_inbound_qty = datum.actual_inbound_qty; //客户实际入库数量
                string actual_inbound_date = datum.actual_inbound_date;//客户实际入库时间
                string cust_purchaseorder = datum.cust_purchaseorder; //客户采购凭证号
                string cust_purchaseline = datum.cust_purchaseline; //客户采购凭证行号
                string totalreceiv_qty = datum.totalreceiv_qty;//累计入库数量
                System.Threading.Tasks.Task<H1_MESSAGE> resquit = _cNCEInterface.I_receiveDeliveryData(delivery_id, batch_no, batch_line, prod_code_cust, actual_inbound_qty, actual_inbound_date, cust_purchaseorder, cust_purchaseline, totalreceiv_qty, cname);
                if (resquit.Result.code == "500")
                {
                    H1_MESSAGE h1_MESSAGE = new H1_MESSAGE
                    {
                        code = "500",
                        message = "操作失败"
                    };
                    return Ok(h1_MESSAGE);
                }
            }
            H1_MESSAGE H1_MESSAGE = new H1_MESSAGE
            {
                code = "200",
                message = "提交OK"
            };
            return Ok(H1_MESSAGE);
            //string json = "";// fm["data"].ToString() == "" ? "" : fm["data"].ToString().Trim();
            //if (!jsonHelp.IsJson(json))
            //{
            //    H1_MESSAGE h1_MESSAGE = new H1_MESSAGE
            //    {
            //        code = "404",
            //        message = "data非jsons数组"
            //    };
            //    return Ok(h1_MESSAGE);
            //}
            //Newtonsoft.Json.Linq.JArray arrdata = Newtonsoft.Json.Linq.JArray.Parse(json);
            //if (arrdata.Count > 0)
            //{
            //    foreach (Newtonsoft.Json.Linq.JToken jToken in arrdata)
            //    {
            //        string delivery_id = jToken["delivery_id"] == null ? "" : jToken["delivery_id"].ToString();//供应商发货单号
            //        string batch_no = jToken["batch_no"] == null ? "" : jToken["batch_no"].ToString();//客户侧对应的唯一入库批次号
            //        string batch_line = jToken["batch_line"] == null ? "" : jToken["batch_line"].ToString();//客户侧对应的唯一入库行项目
            //        string prod_code_cust = jToken["prod_code_cust"] == null ? "" : jToken["prod_code_cust"].ToString();//客户产品编码
            //        string actual_inbound_qty = jToken["actual_inbound_qty"] == null ? "" : jToken["actual_inbound_qty"].ToString();//客户实际入库数量
            //        string actual_inbound_date = jToken["actual_inbound_date"] == null ? "" : jToken["actual_inbound_date"].ToString();//客户实际入库时间
            //        string cust_purchaseorder = jToken["cust_purchaseorder"] == null ? "" : jToken["cust_purchaseorder"].ToString();//客户采购凭证号
            //        string cust_purchaseline = jToken["cust_purchaseline"] == null ? "" : jToken["cust_purchaseline"].ToString();//客户采购凭证行号
            //        string totalreceiv_qty = jToken["totalreceiv_qty"] == null ? "" : jToken["totalreceiv_qty"].ToString();//累计入库数量
            //        //if (string.IsNullOrWhiteSpace(delivery_id))
            //        //{
            //        //    H1_MESSAGE h1_MESSAGE = new H1_MESSAGE
            //        //    {
            //        //        code = "404",
            //        //        message = "供应商发货单号为空"
            //        //    };
            //        //    return Ok(h1_MESSAGE);
            //        //}
            //        System.Threading.Tasks.Task<H1_MESSAGE> resquit = _cNCEInterface.I_receiveDeliveryData(delivery_id, batch_no, batch_line, prod_code_cust, actual_inbound_qty, actual_inbound_date, cust_purchaseorder, cust_purchaseline, totalreceiv_qty, cname);
            //        if (resquit.Result.code == "500")
            //        {
            //            H1_MESSAGE h1_MESSAGE = new H1_MESSAGE
            //            {
            //                code = "500",
            //                message = "操作失败"
            //            };
            //            return Ok(h1_MESSAGE);
            //        }
            //    }
            //    H1_MESSAGE H1_MESSAGE = new H1_MESSAGE
            //    {
            //        code = "200",
            //        message = "提交OK"
            //    };
            //    return Ok(H1_MESSAGE);
            //}
            //else
            //{
            //    H1_MESSAGE h1_MESSAGE = new H1_MESSAGE
            //    {
            //        code = "404",
            //        message = "无数据记录"
            //    };
            //    return Ok(h1_MESSAGE);
            //}
            //return Ok();
        }
        [HttpPost]
        [Route("CRT_WMS_DNCODE")]
        public IActionResult CRT_WMS_DNCODE([FromForm] IFormCollection fm)
        {
            var V_PO = fm["V_PO"].ToString() == "" ? "" : fm["V_PO"].ToString().Trim();
            //获取序列号
            return Ok(_cNCEInterface.CRT_WMS_DNCODE());
        }
        [HttpPost]
        [Route("CRT_WMS_ORDERS")]
        public IActionResult CRT_WMS_ORDERS([FromForm] IFormCollection fm)
        {
            var V_DN = fm["V_DN"].ToString() == "" ? "" : fm["V_DN"].ToString().Trim();
            var T100MO = fm["T100MO"].ToString() == "" ? "" : fm["T100MO"].ToString().Trim();
            var V_PO = fm["V_PO"].ToString() == "" ? "" : fm["V_PO"].ToString().Trim();
            var V_POLINE = fm["V_POLINE"].ToString() == "" ? "" : fm["V_POLINE"].ToString().Trim();
            V_POLINE = "10";
            var V_PN = fm["V_PN"].ToString() == "" ? "" : fm["V_PN"].ToString().Trim();
            var V_CPN = fm["V_CPN"].ToString() == "" ? "" : fm["V_CPN"].ToString().Trim();
            var V_DESC = fm["V_DESC"].ToString() == "" ? "" : fm["V_DESC"].ToString().Trim();
            var V_PONB = fm["V_PONB"].ToString() == "" ? "" : fm["V_PONB"].ToString().Trim();
            var V_CODE = fm["V_CODE"].ToString() == "" ? "" : fm["V_CODE"].ToString().Trim();
            var V_NAME = fm["V_NAME"].ToString() == "" ? "" : fm["V_NAME"].ToString().Trim();
            var V_DELIVERY_ID = fm["V_DELIVERY_ID"].ToString() == "" ? "" : fm["V_DELIVERY_ID"].ToString().Trim();
            var V_DELIVERY_QUANTITY = fm["V_DELIVERY_QUANTITY"].ToString() == "" ? "" : fm["V_DELIVERY_QUANTITY"].ToString().Trim();
            var V_LOGISTICSORDER = fm["V_LOGISTICSORDER"].ToString() == "" ? "" : fm["V_LOGISTICSORDER"].ToString().Trim();//物流单号
            var V_LOGISTICS = fm["V_LOGISTICS"].ToString() == "" ? "" : fm["V_LOGISTICS"].ToString().Trim();//物流商
            var V_ADDRESS = fm["V_ADDRESS"].ToString() == "" ? "" : fm["V_ADDRESS"].ToString().Trim();//发货地址
            var V_DELIEVRYDATE = fm["V_DELIEVRYDATE"].ToString() == "" ? "" : fm["V_DELIEVRYDATE"].ToString().Trim();//发货日期
            var V_WAREHOUSE_ID = fm["V_WAREHOUSE_ID"].ToString() == "" ? "" : fm["V_WAREHOUSE_ID"].ToString().Trim();//客户到货仓库ID
            var V_DELIVERY_STOCK = fm["V_DELIVERY_STOCK"].ToString() == "" ? "" : fm["V_DELIVERY_STOCK"].ToString().Trim();//发货仓库
            var CRT_USER = fm["CRT_USER"].ToString() == "" ? "" : fm["CRT_USER"].ToString().Trim();  // 
            var V_SHIPUSER = fm["V_SHIPUSER"].ToString() == "" ? "" : fm["V_SHIPUSER"].ToString().Trim();
            var V_SHIPADDRESS = fm["V_SHIPADDRESS"].ToString() == "" ? "" : fm["V_SHIPADDRESS"].ToString().Trim();

            return Ok(_cNCEInterface.CRT_WMS_ORDERS(V_DN, T100MO, V_PO, V_POLINE, V_PN, V_CPN, V_DESC, V_PONB, V_CODE, V_NAME, V_DELIVERY_ID, V_DELIVERY_QUANTITY, V_LOGISTICSORDER, V_LOGISTICS, V_ADDRESS, V_DELIEVRYDATE, V_WAREHOUSE_ID, V_DELIVERY_STOCK, CRT_USER, V_SHIPUSER, V_SHIPADDRESS));
        }
        [HttpPost]
        [Route("WMS_SHIP_DATA")]
        public async Task<IActionResult> WMS_SHIP_DATA([FromForm] IFormCollection fm)
        {
            var V_PALLET = fm["V_PALLET"].ToString() == "" ? "" : fm["V_PALLET"].ToString().Trim();
            var V_TY = fm["V_TY"].ToString() == "" ? "" : fm["V_TY"].ToString().Trim();//中箱栈板
            var V_DN = fm["V_DN"].ToString() == "" ? "" : fm["V_DN"].ToString().Trim();

            return Ok();
        }
        [HttpPost]
        [Route("WMS_SO_DATA")]
        public IActionResult WMS_SO_DATA([FromForm] IFormCollection fm)
        {
            var DN = fm["V_DN"].ToString() == "" ? "" : fm["V_DN"].ToString().Trim();
            if (string.IsNullOrWhiteSpace(DN))
            {
                var dTReturn = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_SO_DATA",
                    Msg = "T100出通单号不能为空!"
                };
                return Ok(dTReturn);
            }
            var lsoData = _cNCEInterface.LISTDT_SODATA(DN);
            if (lsoData.Count == 0)
            {
                var dTReturn = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_SO_DATA",
                    Msg = "输入的T100出通单号无数据记录!"
                };
                return Ok(dTReturn);
            }
            else
            {
                var dTReturn = new DT_RETURN
                {
                    Success = true,
                    Service = "WMS_SO_DATA",
                    Msg = "查询完成!",
                    LSO_DATA = lsoData
                };
                return Ok(dTReturn);
            }

        }
        [HttpPost]
        [Route("WMS_DN_DATA")]
        public IActionResult WMS_DN_DATA([FromForm] IFormCollection fm)
        {
            var DN = fm["V_DN"].ToString() == "" ? "" : fm["V_DN"].ToString().Trim();
            var CRT_USER = fm["CRT_USER"].ToString() == "" ? "" : fm["CRT_USER"].ToString().Trim();  // 
            if (string.IsNullOrWhiteSpace(DN))
            {
                var dTReturn = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_DN_DATA",
                    Msg = "发货单不能为空!"
                };
                return Ok(dTReturn);
            }
            var ldnData = _cNCEInterface.LISTDT_DNDATA(DN, CRT_USER);
            if (ldnData.Count == 0)
            {
                var dTReturn = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_DN_DATA",
                    Msg = "输入的发货单无数据记录!"
                };
                return Ok(dTReturn);
            }
            else
            {
                var dTReturn = new DT_RETURN
                {
                    Success = true,
                    Service = "WMS_DN_DATA",
                    Msg = "查询完成!",
                    LDN_DATA = ldnData
                };
                return Ok(dTReturn);
            }

        }
        [HttpPost]
        [Route("WMS_DN_INFOR")]
        public IActionResult WMS_DN_INFOR([FromForm] IFormCollection fm)
        {
            var dn = fm["V_DN"].ToString() == "" ? "" : fm["V_DN"].ToString().Trim();
            if (string.IsNullOrWhiteSpace(dn))
            {
                DT_RETURN dTReturn = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_DN_DATA",
                    Msg = "发货单不能为空!"
                };
                return Ok(dTReturn);
            }
            var ldnData = _cNCEInterface.WMS_DN_INFOR(dn);
            return Ok(ldnData);
        }
        [HttpPost]
        [Route("WMS_NETCODE_DATA")]
        public IActionResult WMS_NETCODE_DATA([FromForm] IFormCollection fm)
        {
            var DN = fm["V_DN"].ToString() == "" ? "" : fm["V_DN"].ToString().Trim();
            var vSscc = fm["V_SSCC"].ToString() == "" ? "" : fm["V_SSCC"].ToString().Trim();
            if (string.IsNullOrWhiteSpace(DN))
            {
                var dTReturn = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_NETCODE_DATA",
                    Msg = "发货单不能为空!"
                };
                return Ok(dTReturn);
            }
            //IntPtr vlu = Marshal.StringToCoTaskMemUTF8(DN);
            var ldnData = _cNCEInterface.WMS_NETCODE_DATA(DN, vSscc);
            return Ok(ldnData);
        }
        [HttpPost]
        [Route("WMS_CK_CARTONID")]
        public IActionResult WMS_CK_CARTONID([FromForm] IFormCollection fm)
        {
            var vDn = fm["V_DN"].ToString() == "" ? "" : fm["V_DN"].ToString().Trim();
            var cartonid = fm["CARTONID"].ToString() == "" ? "" : fm["CARTONID"].ToString().Trim();
            var tycar = fm["TYCAR"].ToString() == "" ? "" : fm["TYCAR"].ToString().Trim();
            var vPn = fm["V_PN"].ToString() == "" ? "" : fm["V_PN"].ToString().Trim();
            if (string.IsNullOrWhiteSpace(vDn))
            {
                var dTReturn = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_CK_CARTONID",
                    Msg = "发货单不能为空!"
                };
                return Ok(dTReturn);
            }
            if (string.IsNullOrWhiteSpace(cartonid))
            {
                var dTReturn = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_CK_CARTONID",
                    Msg = "扫描条码不能为空!"
                };
                return Ok(dTReturn);
            }
            var resQuit = _cNCEInterface.WMS_CK_CARTONID(vDn, cartonid, tycar, vPn);
            return Ok(resQuit);
        }
        [HttpPost]
        [Route("WMS_CK_DNCATONID")]
        public IActionResult WMS_CK_DNCATONID([FromForm] IFormCollection fm)
        {
            var vDn = fm["V_DN"].ToString() == "" ? "" : fm["V_DN"].ToString().Trim();
            var v_PN = fm["V_PN"].ToString() == "" ? "" : fm["V_PN"].ToString().Trim();
            var v_Model = fm["MODEL"].ToString() == "" ? "" : fm["MODEL"].ToString().Trim();
            if (string.IsNullOrWhiteSpace(vDn))
            {
                DT_RETURN dT_RETURN = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_CK_DNCATONID",
                    Msg = "发货单不能为空!"
                };
                return Ok(dT_RETURN);
            }
            string Err = "";
            if (v_Model == "0")
            {
                if (!AppHelper.V_CK_DNCATONID(vDn, ref Err))
                {
                    DT_RETURN dT_RETURN = new DT_RETURN
                    {
                        Success = false,
                        Service = "WMS_CK_DNCATONID",
                        Msg = Err
                    };
                    return Ok(dT_RETURN);
                }
                else
                {
                    DT_RETURN dT_RETURN = new DT_RETURN
                    {
                        Success = true,
                        Service = "WMS_CK_DNCATONID",
                        Msg = "PASS!"
                    };
                    return Ok(dT_RETURN);
                }
            }
            else
            {
                if (!AppHelper.V_CK_DNCATONID(vDn, ref Err))
                {
                    DT_RETURN dT_RETURN = new DT_RETURN
                    {
                        Success = false,
                        Service = "WMS_CK_DNCATONID",
                        Msg = Err
                    };
                    return Ok(dT_RETURN);
                }
                else
                {
                    //栈板发货单绑定---
                    string dn1 = AppHelper.Gt_DNbyPallet(v_PN);
                    if (string.IsNullOrWhiteSpace(dn1))
                    {
                        DT_RETURN dT_RETURN2 = new DT_RETURN
                        {
                            Success = false,
                            Service = "WMS_CK_DNCATONID",
                            Msg = $"扫描栈板{v_PN}未绑定发货单号。"
                        };
                        return Ok(dT_RETURN2);
                    }
                    if (!dn1.Equals(vDn))
                    {
                        DT_RETURN dT_RETURN2 = new DT_RETURN
                        {
                            Success = false,
                            Service = "WMS_CK_DNCATONID",
                            Msg = $"扫描栈板绑定发货单号{dn1},与当前发货单不一致"
                        };
                        return Ok(dT_RETURN2);
                    }
                    DT_RETURN dT_RETURN = new DT_RETURN
                    {
                        Success = true,
                        Service = "WMS_CK_DNCATONID",
                        Msg = "PASS!"
                    };
                    return Ok(dT_RETURN);
                }
            }
        }
        [HttpPost]
        [Route("WMS_CK_TOTALCARTONID")]
        public async Task<IActionResult> WMS_CK_TOTALCARTONID([FromForm] IFormCollection fm)
        {
            var V_DN = string.IsNullOrWhiteSpace(fm["V_DN"].ToString()) ? "" : fm["V_DN"].ToString().Trim();
            var DATA = string.IsNullOrWhiteSpace(fm["DATA"].ToString()) ? "" : fm["DATA"].ToString().Trim();
            var CRT_USER = string.IsNullOrWhiteSpace(fm["CRT_USER"].ToString()) ? "" : fm["CRT_USER"].ToString().Trim();  // 
            var TYCAR = string.IsNullOrWhiteSpace(fm["TYCAR"].ToString()) ? "" : fm["TYCAR"].ToString().Trim();
            var vDeliveryQuantity = string.IsNullOrWhiteSpace(fm["V_delivery_quantity"].ToString()) ? "0" : fm["V_delivery_quantity"].ToString().Trim();
            if (string.IsNullOrWhiteSpace(V_DN))
            {
                DT_RETURN dT_RETURN = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_CK_TOTALCARTONID",
                    Msg = "发货单不能为空!"
                };
                return Ok(dT_RETURN);
            }
            var sDeliveryQuantity = Convert.ToInt32(vDeliveryQuantity);
            if (sDeliveryQuantity == 0)
            {
                DT_RETURN dT_RETURN = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_CK_TOTALCARTONID",
                    Msg = "发货数量不能为0!"
                };
                return Ok(dT_RETURN);
            }
            //DT_RETURN ResQuit = await _cNCEInterface.WMS_CK_TOTALCARTONID(V_DN, DATA, TYCAR, V_delivery_quantity, CRT_USER);
            return Ok(await _cNCEInterface.WMS_CK_TOTALCARTONID(V_DN, DATA, TYCAR, vDeliveryQuantity, CRT_USER));
        }
        [HttpPost]
        [Route("WMS_QACHECK_DATA")]
        public async Task<IActionResult> WMS_QACHECK_DATA([FromForm] IFormCollection fm)
        {
            var V_DN = string.IsNullOrWhiteSpace(fm["V_DN"].ToString()) ? "" : fm["V_DN"].ToString().Trim();
            var V_delievrydate = string.IsNullOrWhiteSpace(fm["V_delievrydate"].ToString()) ? "" : fm["V_delievrydate"].ToString().Trim();
            var V_date1 = string.IsNullOrWhiteSpace(fm["V_date1"].ToString()) ? "" : fm["V_date1"].ToString().Trim();
            var V_date2 = string.IsNullOrWhiteSpace(fm["V_date2"].ToString()) ? "" : fm["V_date2"].ToString().Trim();
            var V_dntype = string.IsNullOrWhiteSpace(fm["V_dntype"].ToString()) ? "" : fm["V_dntype"].ToString().Trim();
            var CRT_USER = fm["CRT_USER"].ToString() == "" ? "" : fm["CRT_USER"].ToString().Trim();
            if (V_DN == "" && V_delievrydate == "" && V_date1 == "")
            {
                DT_RETURN dT_RETURN = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_QACHECK_DATA",
                    Msg = "出货单号、发货日期、送检时间不能都为空"
                };
                return Ok(dT_RETURN);
            }
            return Ok(await _cNCEInterface.WMS_QACHECK_DATA(V_DN, V_delievrydate, V_date1, V_date2, V_dntype, CRT_USER));
        }
        [HttpPost]
        [Route("WMS_QAENDOUT_DATA")]
        public async Task<IActionResult> WMS_QAENDOUT_DATA([FromForm] IFormCollection fm)
        {
            var V_DN = string.IsNullOrWhiteSpace(fm["V_DN"].ToString()) ? "" : fm["V_DN"].ToString().Trim();
            var V_delievrydate = string.IsNullOrWhiteSpace(fm["V_delievrydate"].ToString()) ? "" : fm["V_delievrydate"].ToString().Trim();
            var V_date1 = string.IsNullOrWhiteSpace(fm["V_date1"].ToString()) ? "" : fm["V_date1"].ToString().Trim();
            var V_date2 = string.IsNullOrWhiteSpace(fm["V_date2"].ToString()) ? "" : fm["V_date2"].ToString().Trim();
            var V_dntype = string.IsNullOrWhiteSpace(fm["V_dntype"].ToString()) ? "" : fm["V_dntype"].ToString().Trim();
            var CRT_USER = fm["CRT_USER"].ToString() == "" ? "" : fm["CRT_USER"].ToString().Trim();
            if (V_DN == "" && V_delievrydate == "" && V_date1 == "")
            {
                DT_RETURN dT_RETURN = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_QACHECK_DATA",
                    Msg = "出货单号、发货日期、送检时间不能都为空"
                };
                return Ok(dT_RETURN);
            }
            return Ok(await _cNCEInterface.WMS_QAENDOUT_DATA(V_DN, V_delievrydate, V_date1, V_date2, V_dntype, CRT_USER));
        }
        [HttpPost]
        [Route("WMS_QAOK_DATA")]
        public async Task<IActionResult> WMS_QAOK_DATA([FromForm] IFormCollection fm)
        {
            var V_DN = string.IsNullOrWhiteSpace(fm["V_DN"].ToString()) ? "" : fm["V_DN"].ToString().Trim();
            var V_SSCC = string.IsNullOrWhiteSpace(fm["V_SSCC"].ToString()) ? "" : fm["V_SSCC"].ToString().Trim();
            var V_CODE = string.IsNullOrWhiteSpace(fm["V_CODE"].ToString()) ? "" : fm["V_CODE"].ToString().Trim();
            var cRT_USER = string.IsNullOrWhiteSpace(fm["CRT_USER"].ToString()) ? "" : fm["CRT_USER"].ToString().Trim();
            if (V_DN == "")
            {
                DT_RETURN dT_RETURN = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_QAOK_DATA",
                    Msg = "请点击要允收的发货单"
                };
                return Ok(dT_RETURN);
            }
            return Ok(await _cNCEInterface.WMS_QAOK_DATA(V_DN, V_SSCC, V_CODE, cRT_USER));
        }
        [HttpPost]
        [Route("WMS_QANG_DATA")]
        public async Task<IActionResult> WMS_QANG_DATA([FromForm] IFormCollection fm)
        {
            var V_DN = string.IsNullOrWhiteSpace(fm["V_DN"].ToString()) ? "" : fm["V_DN"].ToString().Trim();
            var V_SSCC = string.IsNullOrWhiteSpace(fm["V_SSCC"].ToString()) ? "" : fm["V_SSCC"].ToString().Trim();
            var V_CODE = string.IsNullOrWhiteSpace(fm["V_CODE"].ToString()) ? "" : fm["V_CODE"].ToString().Trim();
            var cRT_USER = string.IsNullOrWhiteSpace(fm["CRT_USER"].ToString()) ? "" : fm["CRT_USER"].ToString().Trim();
            if (V_DN == "")
            {
                DT_RETURN dT_RETURN = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_QAOK_DATA",
                    Msg = "请点击要允收的发货单"
                };
                return Ok(dT_RETURN);
            }
            return Ok(await _cNCEInterface.WMS_QANG_DATA(V_DN, V_SSCC, V_CODE, cRT_USER));
        }
        [HttpPost]
        [Route("WMS_PALWEIGHT_INFOR")]
        public IActionResult WMS_PALWEIGHT_INFOR([FromForm] IFormCollection fm)
        {
            var PALLET = fm["PALLET"].ToString() == "" ? "" : fm["PALLET"].ToString().Trim();
            if (string.IsNullOrWhiteSpace(PALLET))
            {
                DT_RETURN dT_RETURN = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_PALWEIGHT_INFOR",
                    Msg = "栈板号不能为空!"
                };
                return Ok(dT_RETURN);
            }
            var LDN_DATA = _cNCEInterface.WMS_PALWEIGHT_INFOR(PALLET);
            return Ok(LDN_DATA);
        }
        [HttpPost]
        [Route("WMS_PALCT_INFOR")]
        public IActionResult WMS_PALCT_INFOR([FromForm] IFormCollection fm)
        {
            var V_DN = string.IsNullOrWhiteSpace(fm["V_DN"].ToString()) ? "" : fm["V_DN"].ToString().Trim();
            var V_SSCC = string.IsNullOrWhiteSpace(fm["V_SSCC"].ToString()) ? "" : fm["V_SSCC"].ToString().Trim();
            var ct = AppHelper.Gt_CtPalletBydn(V_DN, V_SSCC);
            DT_RETURN dT_RETURN = new DT_RETURN
            {
                Success = true,
                Service = "WMS_PALCT_INFOR",
                Msg = ct
            };
            return Ok(dT_RETURN);
        }

        [HttpPost]
        [Route("WMS_DNTYPE")]
        public IActionResult WMS_DNTYPE([FromForm] IFormCollection fm)
        {
            var V_DN = string.IsNullOrWhiteSpace(fm["V_DN"].ToString()) ? "" : fm["V_DN"].ToString().Trim();
            var V_SSCC = string.IsNullOrWhiteSpace(fm["V_SSCC"].ToString()) ? "" : fm["V_SSCC"].ToString().Trim();
            var ct = AppHelper.Gt_DNTYPE(V_DN, V_SSCC);
            DT_RETURN dT_RETURN = new DT_RETURN
            {
                Success = true,
                Service = "WMS_PALCT_INFOR",
                Msg = ct
            };
            return Ok(dT_RETURN);
        }
        [HttpPost]
        [Route("WMS_PALWEIGHT_IN")]
        public IActionResult WMS_PALWEIGHT_IN([FromForm] IFormCollection fm)
        {
            var V_PALLET = string.IsNullOrWhiteSpace(fm["V_PALLET"].ToString()) ? "" : fm["V_PALLET"].ToString().Trim();
            var V_GW = string.IsNullOrWhiteSpace(fm["V_GW"].ToString()) ? "0" : fm["V_GW"].ToString().Trim();
            var V_NW = string.IsNullOrWhiteSpace(fm["V_NW"].ToString()) ? "0" : fm["V_NW"].ToString().Trim();
            var LDN_DATA = _cNCEInterface.WMS_PALWEIGHT_IN(V_PALLET, V_GW, V_NW);
            return Ok(LDN_DATA);
        }
        [HttpPost]
        [Route("PostExportData")]
        public IActionResult PostExportData([FromForm] IFormCollection fm)
        {
            DataTable dataTable = new DataTable();
            string Names = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            var V_DN = string.IsNullOrWhiteSpace(fm["V_DN"].ToString()) ? "" : fm["V_DN"].ToString().Trim();
            var V_SSCC = string.IsNullOrWhiteSpace(fm["V_SSCC"].ToString()) ? "" : fm["V_SSCC"].ToString().Trim();
            var CRT_USER = string.IsNullOrWhiteSpace(fm["CRT_USER"].ToString()) ? "" : fm["CRT_USER"].ToString().Trim();
            var V_CODE = string.IsNullOrWhiteSpace(fm["V_CODE"].ToString()) ? "" : fm["V_CODE"].ToString().Trim();
            var V_MSG = string.Empty;
            var rsVal = _cNCEInterface.GetQAOK_DATA(V_DN, V_SSCC, V_CODE, ref V_MSG);
            if (rsVal == null)
            {
                return Ok();
            }
            ExcelHelper excelHeper = new ExcelHelper();
            List<ExcelGridModel> config = new List<ExcelGridModel>
            {   new ExcelGridModel{name="V_DN",label="出货单号", align="left",},
                new ExcelGridModel{name="V_PO",label="客户采购凭证号", align="left",},
                new ExcelGridModel{name="V_POLINE",label="客户采购凭证行号", align="left",},
                new ExcelGridModel{name="V_PN",label="销售编码", align="left",},
                new ExcelGridModel{name="V_CPN",label="客户Item", align="left",},
                new ExcelGridModel{name="V_DELIVERY_QUANTITY",label="发货数量", align="left",},
                new ExcelGridModel{name="IMEI",label="IMEI", align="left",},
                new ExcelGridModel{name="IMEI2",label="IMEI2", align="left",},
                new ExcelGridModel{name="MEID",label="MEID", align="left",},
                new ExcelGridModel{name="MAC",label="MAC", align="left",},
                new ExcelGridModel{name="PRODUCT_BARCODE",label="SN", align="left",},
                new ExcelGridModel{name="SVER",label="软件版本", align="left",},
                new ExcelGridModel{name="CATON_ID_HW",label="中箱", align="left",},
                new ExcelGridModel{name="PALLET_ID_HW",label="栈板", align="left",},
                new ExcelGridModel{name="WEIGHT1",label="彩盒重量", align="left",},
                new ExcelGridModel{name="WEIGHT2",label="中箱重量", align="left",},
                new ExcelGridModel{name="WEIGHT3",label="栈板重量", align="left",},
                new ExcelGridModel{name="EAN_UPC_CODE",label="EAN", align="left",},
                new ExcelGridModel{name="UDID",label="UDID", align="left",},
                new ExcelGridModel{name="EMMC_ID",label="EMMC_ID", align="left",},
            };
            var fileName = $"查询{Names}.xlsx";
            return excelHeper.ExcelDownloadNPOI(rsVal, config, fileName);
        }
        [HttpPost]
        [Route("PostExportDataHW")]
        public IActionResult PostExportDataHW([FromForm] IFormCollection fm)
        {
            DataTable dataTable = new DataTable();
            string Names = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            var V_DN = string.IsNullOrWhiteSpace(fm["V_DN"].ToString()) ? "" : fm["V_DN"].ToString().Trim();
            var V_SSCC = string.IsNullOrWhiteSpace(fm["V_SSCC"].ToString()) ? "" : fm["V_SSCC"].ToString().Trim();
            var CRT_USER = string.IsNullOrWhiteSpace(fm["CRT_USER"].ToString()) ? "" : fm["CRT_USER"].ToString().Trim();
            var V_CODE = string.IsNullOrWhiteSpace(fm["V_CODE"].ToString()) ? "" : fm["V_CODE"].ToString().Trim();
            var V_MSG = string.Empty;
            var rsValTb = _cNCEInterface.GetHW_DATA(V_DN, V_SSCC, V_CODE, ref V_MSG);
            if (rsValTb == null)
            {
                return Ok();
            }
            ExcelHelper excelHeper = new ExcelHelper();
            List<ExcelGridModel> config = new List<ExcelGridModel>
            {
                new ExcelGridModel{name="PHYSICSNO",label="PhysicsNo", align="left",},
                new ExcelGridModel{name="IMEI",label="IMEI", align="left",},
                new ExcelGridModel{name="IMEI_1",label="IMEI_1", align="left",},
                new ExcelGridModel{name="IMEI_2",label="IMEI_2", align="left",},
                new ExcelGridModel{name="IMEI_3",label="IMEI_3", align="left",},
                new ExcelGridModel{name="MEID",label="MEID", align="left",},
                new ExcelGridModel{name="MEID_DEC",label="MEID_DEC", align="left",},
                new ExcelGridModel{name="MEID_DEC_18",label="MEID_DEC_18", align="left",},
                new ExcelGridModel{name="MEID_HEX",label="MEID_HEX", align="left",},
                new ExcelGridModel{name="MEID_HEX_14",label="MEID_HEX_14", align="left",},
                new ExcelGridModel{name="PESN_DEC",label="pESN_DEC", align="left",},
                new ExcelGridModel{name="PESN_HEX",label="pESN_HEX", align="left",},
                new ExcelGridModel{name="ESN_HEX2",label="ESN_HEX2", align="left",},
                new ExcelGridModel{name="ESN_DEC2",label="ESN_DEC2", align="left",},
                new ExcelGridModel{name="MAC_1",label="MAC_1", align="left",},
                new ExcelGridModel{name="MAC_2",label="MAC_2", align="left",},
                new ExcelGridModel{name="WIFI",label="WIFI", align="left",},
                new ExcelGridModel{name="MAC",label="MAC", align="left",},
                new ExcelGridModel{name="PCBA_BARCODE",label="PCBA_BARCODE", align="left",},
                new ExcelGridModel{name="PRODUCT_BARCODE",label="PRODUCT_BARCODE", align="left",},
                new ExcelGridModel{name="PACKING2",label="Packing2", align="left",},
                new ExcelGridModel{name="PACKING3",label="Packing3", align="left",},
                new ExcelGridModel{name="PACKING4",label="Packing4", align="left",},
                new ExcelGridModel{name="SPECIAL_SN_ID",label="SPECIAL_SN_ID", align="left",},
                new ExcelGridModel{name="SPECIAL_MID_ID",label="SPECIAL_MID_ID", align="left",},
                new ExcelGridModel{name="SPECIAL_BIGCARTON_ID",label="SPECIAL_BIGCARTON_ID", align="left",},
                new ExcelGridModel{name="SPECIAL_PALLET_ID",label="SPECIAL_PALLET_ID", align="left",},
                new ExcelGridModel{name="PACKINGWEIGHT2",label="PackingWeight2", align="left",},
                new ExcelGridModel{name="PACKINGWEIGHT3",label="PackingWeight3", align="left",},
                new ExcelGridModel{name="ITEM_BOM",label="Item_BOM", align="left",},
                new ExcelGridModel{name="COLOR",label="COLOR", align="left",},
                new ExcelGridModel{name="SOFTWARE_VERSION",label="SOFTWARE_VERSION", align="left",},
                new ExcelGridModel{name="PRODUCT_DATE",label="PRODUCT_DATE", align="left",},
                new ExcelGridModel{name="COMMENTS",label="COMMENTS", align="left",},
                new ExcelGridModel{name="BATTERY_SN",label="BATTERY_SN", align="left",},
                new ExcelGridModel{name="BATTERYNO_A",label="BATTERYNO_A", align="left",},
                new ExcelGridModel{name="BATTERYNO_B",label="BATTERYNO_B", align="left",},
                new ExcelGridModel{name="CHARGERNO_A",label="CHARGERNO_A", align="left",},
                new ExcelGridModel{name="NETCODE",label="NETCODE", align="left",},
                new ExcelGridModel{name="NETCODE_VALIDITY",label="NETCODE_VALIDITY", align="left",},
                new ExcelGridModel{name="EAN_UPC_CODE",label="EAN_UPC_CODE", align="left",},
                new ExcelGridModel{name="NETWORK_ACCESS",label="Network_Access", align="left",},
                new ExcelGridModel{name="IMEI_MEID",label="IMEI_MEID", align="left",},
                new ExcelGridModel{name="MDN_RULE",label="MDN_RULE", align="left",},
                new ExcelGridModel{name="MSIN",label="MSIN", align="left",},
                new ExcelGridModel{name="TRUE_MSIN",label="TRUE_MSIN", align="left",},
                new ExcelGridModel{name="COUNTRY",label="COUNTRY", align="left",},
                new ExcelGridModel{name="VENDOR",label="VENDOR", align="left",},
                new ExcelGridModel{name="FRP_KEY",label="FRP_KEY", align="left",},
                new ExcelGridModel{name="MDN",label="MDN", align="left",},
                new ExcelGridModel{name="SIM_ICCID",label="SIM_ICCID", align="left",},
                new ExcelGridModel{name="MSN",label="MSN", align="left",},
                new ExcelGridModel{name="CLOUD_BIND_KEY",label="CLOUD_BIND_KEY", align="left",},
                new ExcelGridModel{name="IMSI",label="IMSI", align="left",},
                new ExcelGridModel{name="MOBILE_NO",label="MOBILE_NO", align="left",},
                new ExcelGridModel{name="ARRIVAL_DATE",label="ARRIVAL_DATE", align="left",},
                new ExcelGridModel{name="RSN",label="RSN", align="left",},
                new ExcelGridModel{name="SCN",label="SCN", align="left",},
                new ExcelGridModel{name="SIMCARD_MODE",label="SIMCARD_MODE", align="left",},
                new ExcelGridModel{name="FCK_SIMLOCK",label="FCK_SIMLOCK", align="left",},
                new ExcelGridModel{name="USIM",label="USIM", align="left",},
                new ExcelGridModel{name="EMMC_ID",label="EMMC_ID", align="left",},
                new ExcelGridModel{name="PUBLICKEY",label="PUBLICKEY", align="left",},
                new ExcelGridModel{name="USB_PORT_MODE",label="USB_PORT_MODE", align="left",},
                new ExcelGridModel{name="NCK_SIMLOCK",label="NCK_SIMLOCK", align="left",},
                new ExcelGridModel{name="PLMNNS",label="PLMNNS", align="left",},
                new ExcelGridModel{name="PLMNNW",label="PLMNNW", align="left",},
                new ExcelGridModel{name="PLMNSP",label="PLMNSP", align="left",},
                new ExcelGridModel{name="PLMN_MCCMNC",label="PLMN_MCCMNC", align="left",},
                new ExcelGridModel{name="PLMN_MSIN",label="PLMN_MSIN", align="left",},
                new ExcelGridModel{name="PLMN_SID",label="PLMN_SID", align="left",},
                new ExcelGridModel{name="PLMNCP",label="PLMNCP", align="left",},
                new ExcelGridModel{name="PLMNSM",label="PLMNSM", align="left",},
                new ExcelGridModel{name="NCK_FEATUREINDS",label="NCK_FEATUREINDS", align="left",},
                new ExcelGridModel{name="DCK_COUNT_MAX",label="DCK_COUNT_MAX", align="left",},
                new ExcelGridModel{name="NCK_DIAGUNLOCK",label="NCK_DIAGUNLOCK", align="left",},
                new ExcelGridModel{name="NCK_NCKNSCKSPCKRESET",label="NCK_NCKNSCKSPCKRESET", align="left",},
                new ExcelGridModel{name="UDID",label="UDID", align="left",},
                new ExcelGridModel{name="PRODUCT_NAME",label="PRODUCT_NAME", align="left",},
                new ExcelGridModel{name="TRUST_DEVICE_IDS",label="TRUST_DEVICE_IDS", align="left",},
                new ExcelGridModel{name="CERTIFY_CODE",label="CERTIFY_CODE", align="left",},

            };
            var fileName = $"查询{Names}.xlsx";
            return excelHeper.ExcelDownloadNPOI(rsValTb, config, fileName);
        }
        [HttpPost]
        [Route("OutExportData")]
        public IActionResult OutExportData([FromForm] IFormCollection fm)
        {
            DataTable dataTable = new DataTable();
            string Names = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            var V_DN = string.IsNullOrWhiteSpace(fm["V_DN"].ToString()) ? "" : fm["V_DN"].ToString().Trim();
            var V_SSCC = string.IsNullOrWhiteSpace(fm["V_SSCC"].ToString()) ? "" : fm["V_SSCC"].ToString().Trim();
            var CRT_USER = string.IsNullOrWhiteSpace(fm["CRT_USER"].ToString()) ? "" : fm["CRT_USER"].ToString().Trim();
            var V_CODE = string.IsNullOrWhiteSpace(fm["V_CODE"].ToString()) ? "" : fm["V_CODE"].ToString().Trim();
            var V_MSG = string.Empty;
            var rsVal = _cNCEInterface.OutExportData(V_DN, V_SSCC, V_CODE, ref V_MSG);
            if (rsVal == null)
            {
                return Ok();
            }
            ExcelHelper excelHeper = new ExcelHelper();
            List<ExcelGridModel> config = new List<ExcelGridModel>
            {
                new ExcelGridModel{name="V_CPN",label="商品编码", align="left",},
                new ExcelGridModel{name="V_IMEI",label="串号", align="left",},
                new ExcelGridModel{name="V_PO",label="华盛采购单号", align="left",},
                new ExcelGridModel{name="V_PROD_DESC_CUST",label="商品名称", align="left",},
                new ExcelGridModel{name="V_PROVINCE",label="省分", align="left",},
            };
            var fileName = $"查询{Names}.xlsx";
            return excelHeper.ExcelDownloadNPOI(rsVal, config, fileName);
        }
    }
}
