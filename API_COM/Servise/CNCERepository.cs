using API_COM.Helper;
using API_COM.Modelclass;
using API_COM.MyClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API_COM.Servise
{
    public class CNCERepository : CNCEInterface
    {
        public Task<DT_RETURN> CRT_WMS_DNCODE()
        {
            DT_RETURN dT_RETURN = new DT_RETURN();
            string sql = "SELECT 'CNCE'||TO_CHAR(SYSDATE,'YYYYMMDD')CNCE,LPAD(1,5,0)RS  FROM DUAL";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable==null)
            {
                dT_RETURN.Success = false;
                dT_RETURN.Msg = "数据查询异常"+ UserHelp.OracleConnection + OracleHelper.OracleErrMsg;
                return Task.FromResult(dT_RETURN);
            }
            string DnTOP = string.Empty;
            string rsEnd = string.Empty;
            if (dataTable.Rows.Count>0)
            {
                DnTOP=dataTable.Rows[0][0].ToString();
                rsEnd = dataTable.Rows[0][1].ToString();
            }
            string DN = string.Empty;
            string Rs = string.Empty;
            sql = $"SELECT '{DnTOP}'||LPAD(NVL(A.SERIALNO,0)+1,5,'0'),SERIALNO+1  FROM SERIALNO_INFO A WHERE A.PREFIX= '{DnTOP}'";
            dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable.Rows.Count > 0)
            {
                DN = dataTable.Rows[0][0].ToString();
                Rs = dataTable.Rows[0][1].ToString();
                sql = $"UPDATE SERIALNO_INFO SET SERIALNO=SERIALNO+1 WHERE PREFIX='{DnTOP}' AND F_TYPE=9";
                OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, sql);
            }
            else
            {
                DN = DnTOP + rsEnd;
                sql = $"INSERT INTO SERIALNO_INFO(PREFIX,SERIALNO,F_TYPE)VALUES('{DnTOP}',1,9)";
                OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, sql);
            }
            dT_RETURN.Success = true;
            dT_RETURN.Msg = DN; 
            return Task.FromResult(dT_RETURN);
        }

        public Task<DT_RETURN> CRT_WMS_ORDERS(string v_DN, string t100MO, string v_PO, string v_POLINE, string v_PN, string v_CPN, string v_DESC, string v_PONB, string v_CODE, string v_NAME, string v_DELIVERY_ID, string v_DELIVERY_QUANTITY, string v_LOGISTICSORDER, string v_LOGISTICS, string v_ADDRESS, string v_DELIEVRYDATE, string v_WAREHOUSE_ID, string v_DELIVERY_STOCK,string cRT_USER,string v_SHIPUSER, string v_SHIPADDRESS)
        {
            string DNty =AppHelper.G_WMS_SSCCMODEL(v_DN);
            if (!string.IsNullOrWhiteSpace(DNty))
            {
                switch (DNty)
                {
                    case "1":
                        DT_RETURN H1_MESSAGE = new DT_RETURN
                        {
                            Success = false,
                            Msg = "发货单当前状态客户收货,不允许修改"
                        };
                        return Task.FromResult(H1_MESSAGE);
                        break;
                    case "99":
                        DT_RETURN H1_MESSAGE1 = new DT_RETURN
                        {
                            Success = false,
                            Msg = "发货单当前状态出货检验,不允许修改"
                        };
                        return Task.FromResult(H1_MESSAGE1);
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(v_CPN))
            {
                //string T2 = v_CPN.Substring(0, 2);
                if (!v_CPN.StartsWith("74"))
                {
                    DT_RETURN H1_MESSAGE1 = new DT_RETURN
                    {
                        Success = false,
                        Msg = "H1客户编码前缀74"
                    };
                    return Task.FromResult(H1_MESSAGE1);
                }
            }
            string EmsCode = AppHelper.G_EMSCODE(cRT_USER);
            //发货数量判断
            int V_PONB = Convert.ToInt32(v_PONB);
            int v_QUANTITY = Convert.ToInt32(v_DELIVERY_QUANTITY);
            if (v_QUANTITY > V_PONB)
            {
                DT_RETURN H1_MESSAGE = new DT_RETURN
                {
                    Success = false,
                    Msg = "操作失败:发货数量不能大于订单总数"
                };
                return Task.FromResult(H1_MESSAGE);
            }
            //获取当前已经发货数据
            string V_Qty = AppHelper.DELIVERY_QUANTITY(v_PO);
            string V_Qty1=AppHelper.ToTalDELIVERY_QUANTITY(v_PO, v_DN);
            int v_QUANTITY1 = 0;
            int v_QUANTITY2 = 0;
            if (!string.IsNullOrEmpty(V_Qty))
            {
                v_QUANTITY1 = Convert.ToInt32(V_Qty); 
            }
            if (!string.IsNullOrEmpty(V_Qty1))
            {
                v_QUANTITY2 = Convert.ToInt32(V_Qty1);
            }
            int TalQty = v_QUANTITY + v_QUANTITY1 + v_QUANTITY2;
            int TalQty1 = v_QUANTITY1 + v_QUANTITY2;
            if (TalQty> V_PONB)
            {
                DT_RETURN H1_MESSAGE = new DT_RETURN
                {
                    Success = false,
                    Msg = "操作失败:发货数量不能大于订单总数,当前PO已创建发货数" + TalQty1
                };
                return Task.FromResult(H1_MESSAGE);
            }
            string sql = $"SELECT V_DN FROM WMS_ORDERS WHERE V_DN='{v_DN}'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable.Rows.Count>0)
            {
                sql = $"UPDATE WMS_ORDERS SET V_PO='{v_PO}',V_POLINE= '{v_POLINE}',V_PN='{v_PN}',V_CPN='{v_CPN}',V_PROD_DESC_CUST='{v_DESC}',V_PONB='{v_PONB}',T100MO='{t100MO}',V_CODE='{v_CODE}',V_NAME='{v_NAME}',V_DELIVERY_ID='{v_DELIVERY_ID}',V_LOGISTICSORDER='{v_LOGISTICSORDER}',V_LOGISTICS='{v_LOGISTICS}',V_ADDRESS='{v_ADDRESS}',V_DELIEVRYDATE='{v_DELIEVRYDATE}',V_WAREHOUSE_ID='{v_WAREHOUSE_ID}',V_DELIVERY_STOCK='{v_DELIVERY_STOCK}',V_DELIVERY_QUANTITY='{v_DELIVERY_QUANTITY}',CRT_USER='{cRT_USER}', V_SHIPUSER='{v_SHIPUSER}', V_SHIPADDRESS='{v_SHIPADDRESS}' WHERE V_DN='{v_DN}'" ;
            }
            else
            {
                sql = "INSERT INTO WMS_ORDERS(V_DN,V_PO,V_POLINE,V_PN,V_CPN,V_PROD_DESC_CUST,V_PONB,T100MO,V_CODE,V_NAME,V_DELIVERY_ID,V_LOGISTICSORDER,V_LOGISTICS,V_ADDRESS,V_DELIEVRYDATE,V_WAREHOUSE_ID,V_DELIVERY_STOCK,V_DELIVERY_QUANTITY,CRT_USER, V_SHIPUSER, V_SHIPADDRESS,V_EMSCODE)" +
                $"VALUES('{v_DN}','{v_PO}','{v_POLINE}','{v_PN}','{v_CPN}','{v_DESC}','{v_PONB}','{t100MO}','{v_CODE}','{v_NAME}','{v_DELIVERY_ID}','{v_LOGISTICSORDER}','{v_LOGISTICS}','{v_ADDRESS}','{v_DELIEVRYDATE}','{v_WAREHOUSE_ID}','{v_DELIVERY_STOCK}','{v_DELIVERY_QUANTITY}','{cRT_USER}','{v_SHIPUSER}','{v_SHIPADDRESS}','{EmsCode}')";
            }  
            int Ins = OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, sql);
            if (Ins > 0)
            {
                DT_RETURN H1_MESSAGE = new DT_RETURN
                {
                    Success = true,
                    Msg = "提交OK"
                };
                return Task.FromResult(H1_MESSAGE);
            }
            else
            {
                DT_RETURN H1_MESSAGE = new DT_RETURN
                {
                    Success = false,
                    Msg = "操作失败" 
                };
                return Task.FromResult(H1_MESSAGE);
            }
        }

      
        public Task<H1_MESSAGE> I_receiveDeliveryData(string delivery_id, string batch_no, string batch_line, string prod_code_cust, string actual_inbound_qty, string actual_inbound_date, string cust_purchaseorder, string cust_purchaseline, string totalreceiv_qty,string cname)
        {
            string sql1 = "SELECT * FROM DUAL ";
            DataTable dataTable =OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql1);
            string d = OracleHelper.OracleErrMsg;
            string sql = $"INSERT INTO WMS_STOCK(DELIVERY_ID,BATCH_NO,BATCH_LINE,PROD_CODE_CUST,ACTUAL_INBOUND_QTY,ACTUAL_INBOUND_DATE,CUST_PURCHASEORDER,CUST_PURCHASELINE,TOTALRECEIV_QTY,CRT_USRE,CLIENCODE)" +
                $"VALUES('{delivery_id}','{batch_no}','{batch_line}','{prod_code_cust}','{actual_inbound_qty}','{actual_inbound_date}','{cust_purchaseorder}','{cust_purchaseline}','{totalreceiv_qty}','{cname}',48)";
            int Ins = OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, sql);
            if (Ins>0)
            {
                H1_MESSAGE H1_MESSAGE = new H1_MESSAGE
                {
                    code = "200",
                    message = "提交OK"
                };
                return Task.FromResult(H1_MESSAGE); 
            }
            else
            {
                H1_MESSAGE H1_MESSAGE = new H1_MESSAGE
                {
                    code = "500",
                    message = "操作失败"
                };
                return Task.FromResult(H1_MESSAGE);
            }
            //throw new NotImplementedException();
        }

        public List<DT_DNDATA> LISTDT_DNDATA(string dN, string cRT_USER)
        {
            string EmsCode = AppHelper.G_EMSCODE(cRT_USER);
            List<DT_DNDATA> dT_DNDATA = new List<DT_DNDATA>();
            string sql = $"SELECT T.V_DN,T.V_PO,T.V_POLINE,T.V_PN,T.V_CPN,T.V_PROD_DESC_CUST,T.V_DELIVERY_QUANTITY,T.V_LOGISTICSORDER,T.V_LOGISTICS FROM WMS_ORDERS T WHERE V_DN LIKE'%{dN}%'";
            if (!string.IsNullOrWhiteSpace(EmsCode)&& EmsCode!="ZN")
            {
                sql += $"  AND V_EMSCODE='{EmsCode}'";
            }
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DT_DNDATA dT_DNDATAs = new DT_DNDATA();
                    dT_DNDATAs.V_DN = dataTable.Rows[i]["V_DN"].ToString();
                    dT_DNDATAs.V_PO = dataTable.Rows[i]["V_PO"].ToString();
                    dT_DNDATAs.V_POLINE = dataTable.Rows[i]["V_POLINE"].ToString();
                    dT_DNDATAs.V_CPN = dataTable.Rows[i]["V_CPN"].ToString();
                    dT_DNDATAs.V_PROD_DESC_CUST = dataTable.Rows[i]["V_PROD_DESC_CUST"].ToString();
                    dT_DNDATAs.V_DELIVERY_QUANTITY = dataTable.Rows[i]["V_DELIVERY_QUANTITY"].ToString();
                    dT_DNDATAs.V_LOGISTICSORDER = dataTable.Rows[i]["V_LOGISTICSORDER"].ToString();
                    dT_DNDATAs.V_LOGISTICS = dataTable.Rows[i]["V_LOGISTICS"].ToString();
                    dT_DNDATA.Add(dT_DNDATAs);
                }
            }
            return dT_DNDATA;
        }

        public List<DT_SODATA> LISTDT_SODATA(string DN)
        {
            List<DT_SODATA> dT_SODATAs = new List<DT_SODATA>();
            string sql = $@"SELECT A.XMDH001,B.XMDA033,A.XMDH002,XMDH034,XMDH016,XMDH006,C.IMAAL004,D.XMDG007,PMAAL003 FROM XMDH_T A 
                            LEFT JOIN XMDA_T B ON A.XMDH001=B.XMDADOCNO 
                            LEFT JOIN IMAAL_T C ON C.IMAAL001=XMDH006
                            LEFT JOIN XMDG_T D ON D.XMDGDOCNO=A.XMDHDOCNO
                            LEFT JOIN PMAAL_T E ON E.PMAAL001=D.XMDG007
                            WHERE XMDH001 = '{DN}' OR XMDHDOCNO = '{DN}' ";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleDbaT100, sql);
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DT_SODATA dT_SODATA = new DT_SODATA
                    {
                        XMDH001 = dataTable.Rows[i]["XMDH001"].ToString(),
                        XMDA033 = dataTable.Rows[i]["XMDA033"].ToString(),
                        XMDH002 = dataTable.Rows[i]["XMDH002"].ToString(),
                        XMDH034 = dataTable.Rows[i]["XMDH034"].ToString(),
                        XMDH016 = dataTable.Rows[i]["XMDH016"].ToString(),
                        XMDH006 = dataTable.Rows[i]["XMDH006"].ToString(),
                        IMAAL004 = dataTable.Rows[i]["IMAAL004"].ToString(),
                        XMDG007 = dataTable.Rows[i]["XMDG007"].ToString(),
                        PMAAL003 = dataTable.Rows[i]["PMAAL003"].ToString()
                    };
                    dT_SODATAs.Add(dT_SODATA);
                }
            }
            return dT_SODATAs;
        }

        public Task<DT_RETURN> WMS_CK_CARTONID(string v_DN, string cARTONID, string tYCAR, string v_PN)
        {
            DT_RETURN dT_RETURN = new DT_RETURN();
            string DnTy = AppHelper.SSCCMODEL(v_DN);
            if (!string.IsNullOrEmpty(DnTy))
            {
                if (DnTy == "2")
                {
                    DT_RETURN dT_RETURN1 = new DT_RETURN
                    {
                        Success = false,
                        Service = "WMS_CK_TOTALCARTONID",
                        Msg = "发货单已经拒收，不允许重新送检，请重新创建发货单发货"
                    };
                    return Task.FromResult(dT_RETURN1);
                }
                if (DnTy == "1")
                {
                    DT_RETURN dT_RETURN1 = new DT_RETURN
                    {
                        Success = false,
                        Service = "WMS_CK_TOTALCARTONID",
                        Msg = "发货单已经允收，不允许重新送检"
                    };
                    return Task.FromResult(dT_RETURN1);
                }
                if (DnTy == "0")
                {
                    DT_RETURN dT_RETURN1 = new DT_RETURN
                    {
                        Success = false,
                        Service = "WMS_CK_TOTALCARTONID",
                        Msg = "发货单当前状态发货检验，不允许重新送检"
                    };
                    return Task.FromResult(dT_RETURN1);
                }
            }
           
            string sql = $"SELECT DN1 FROM WMS_SSCCINFO T WHERE BOXSN='{cARTONID}' AND NVL(T.WWNO,'D')!='NG' AND NVL(T.WWNO,'D')!='REWORK'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable.Rows.Count>0)
            {
                string dn1 = dataTable.Rows[0][0].ToString();
                dT_RETURN.Success = false;
                dT_RETURN.Msg = $"{cARTONID}：已经绑定DN：{dn1},请先解绑";
                return Task.FromResult(dT_RETURN);
            }

            switch (tYCAR)
            {
                case "BIG":
                    if (AppHelper.I_BigCartoninfo(cARTONID))
                    {
                        //判断编码---产品编码客户编码。。。
                        string mo = AppHelper.G_BigMo(cARTONID);
                        if (string.IsNullOrWhiteSpace(mo))
                        {
                            dT_RETURN.Success = false;
                            dT_RETURN.Msg = $"栈板{cARTONID}关联IMEI数据为空";
                            return Task.FromResult(dT_RETURN);
                        }
                        //判断栈板毛重
                        string gw1 = AppHelper.G_BigGW(cARTONID);
                        if (string.IsNullOrWhiteSpace(gw1))
                        {
                            dT_RETURN.Success = false;
                            dT_RETURN.Msg = $"栈板{cARTONID}未维护毛重";
                            return Task.FromResult(dT_RETURN);
                        }
                        string pN = string.Empty;
                        string cPN = string.Empty;
                        string mSG = string.Empty;
                        if (AppHelper.I_MODEL(mo,ref pN,ref cPN,ref mSG))
                        {
                            if (pN==v_PN||cPN== v_PN)
                            {
                                //判断数据重复!
                                string ct = AppHelper.G_SSCCINFOLOGCTbyBig(cARTONID);
                                if (!ct.Equals("0"))
                                {
                                    dT_RETURN.Success = false;
                                    dT_RETURN.Msg = $"栈板{cARTONID}存在已允收数据{ct}PCS，已经出货的不允许再上传";
                                    return Task.FromResult(dT_RETURN);
                                }
                                dT_RETURN.Success = true;
                                dT_RETURN.Msg = $"PASS";
                                return Task.FromResult(dT_RETURN);
                            }
                            else
                            {
                                dT_RETURN.Success = false;
                                dT_RETURN.Msg = $"栈板{cARTONID}的客户编码{cPN}不符合出货订单{v_DN}要求";
                                return Task.FromResult(dT_RETURN);
                            }
                        }
                        else
                        {
                            dT_RETURN.Success = false;
                            dT_RETURN.Msg = mSG;
                            return Task.FromResult(dT_RETURN);
                        }
                    }
                    else
                    {
                        dT_RETURN.Success = false;
                        dT_RETURN.Msg = $"栈板{cARTONID}不存在";
                        return Task.FromResult(dT_RETURN);
                    }
                    break;
                case "MID":
                    if (AppHelper.I_MidCartoninfo(cARTONID))
                    {
                        //判断编码---产品编码客户编码。。。
                        string mo = AppHelper.G_MidMo(cARTONID);
                        if (string.IsNullOrWhiteSpace(mo))
                        {
                            dT_RETURN.Success = false;
                            dT_RETURN.Msg = $"中箱{cARTONID}关联IMEI数据为空";
                            return Task.FromResult(dT_RETURN);
                        }
                        string pN = string.Empty;
                        string cPN = string.Empty;
                        string mSG = string.Empty;
                        if (AppHelper.I_MODEL(mo, ref pN, ref cPN, ref mSG))
                        {
                            if (pN == v_PN || cPN == v_PN)
                            {
                                string ct = AppHelper.G_SSCCINFOLOGCTbyMid(cARTONID);
                                if (!ct.Equals("0"))
                                {
                                    dT_RETURN.Success = false;
                                    dT_RETURN.Msg = $"中箱{cARTONID}存在已允收数据{ct}PCS,已经出货的不允许再上传";
                                    return Task.FromResult(dT_RETURN);
                                }
                                dT_RETURN.Success = true;
                                dT_RETURN.Msg = $"PASS";
                                return Task.FromResult(dT_RETURN);
                            }
                            else
                            {
                                dT_RETURN.Success = false;
                                dT_RETURN.Msg = $"中箱{cARTONID}的客户编码{cPN}不符合出货订单{v_DN}要求";
                                return Task.FromResult(dT_RETURN);
                            }
                        }
                        else
                        {
                            dT_RETURN.Success = false;
                            dT_RETURN.Msg = mSG;
                            return Task.FromResult(dT_RETURN);
                        }
                    }
                    else
                    {
                        dT_RETURN.Success = false;
                        dT_RETURN.Msg = $"中箱{cARTONID}不存在";
                        return Task.FromResult(dT_RETURN);
                    }
                    break;
                default:
                    break;
            }
            throw new NotImplementedException();
        }

        public Task<DT_RETURN> WMS_CK_TOTALCARTONID(string v_DN, string dATA, string tYCAR, string V_delivery_quantity, string cRT_USER)
        {
            string DnTy = AppHelper.SSCCMODEL(v_DN);
            if (!string.IsNullOrEmpty(DnTy))
            {
                if (DnTy=="2")
                {
                    DT_RETURN dT_RETURN = new DT_RETURN
                    {
                        Success = false,
                        Service = "WMS_CK_TOTALCARTONID",
                        Msg = "发货单已经拒收，不允许重新送检，请重新创建发货单发货"
                    };
                    return Task.FromResult(dT_RETURN);
                }
                if (DnTy == "1")
                {
                    DT_RETURN dT_RETURN = new DT_RETURN
                    {
                        Success = false,
                        Service = "WMS_CK_TOTALCARTONID",
                        Msg = "发货单已经允收，不允许重新送检"
                    };
                    return Task.FromResult(dT_RETURN);
                }
                if (DnTy == "0")
                {
                    DT_RETURN dT_RETURN = new DT_RETURN
                    {
                        Success = false,
                        Service = "WMS_CK_TOTALCARTONID",
                        Msg = "发货单当前状态发货检验，不允许重新送检"
                    };
                    return Task.FromResult(dT_RETURN);
                }
            }
            List<string> LisSql = new List<string>();
            List<string> ls = JsonConvert.DeserializeObject<List<string>>(dATA);
            string lisPall = string.Empty;
            string sSCC = Guid.NewGuid().ToString(); 
            string sql = $"INSERT INTO WMS_SSCCMODEL(DN1,INPUTER,BOXTYPE,SSCC)VALUES('{v_DN}','{cRT_USER}','{tYCAR}','{sSCC}')";
            LisSql.Add(sql);
            for (int i = 0; i < ls.Count; i++)
            {
                lisPall += "'" + ls[i] + "',";
                  sql = $"INSERT INTO WMS_SSCCINFO(DN1,BOXSN,INPUTER,BOXTYPE,SSCC)VALUES('{v_DN}','{ls[i]}','{cRT_USER}','{tYCAR}','{sSCC}')";
                LisSql.Add(sql);
            }
            if (lisPall.Length > 0)
            {
                lisPall = lisPall.Substring(0, lisPall.Length - 1);
            }
            //当前发货单状态判断---


            string DN1 = AppHelper.G_SSCCINFO(lisPall);
            if (!string.IsNullOrEmpty(DN1))
            {
                DT_RETURN dT_RETURN = new DT_RETURN
                {
                    Success = false,
                    Service = "WMS_CK_TOTALCARTONID",
                    Msg = "Source:CNCERepository,Line:297,Error:" + lisPall + "存在绑定的发货单" + DN1 + "不允许重新送检,请品质解除绑定"
                };
                return Task.FromResult(dT_RETURN);
            }
            //判断发货数量---
            switch (tYCAR)
            {
                case "BIG":
                    string bigCount = AppHelper.G_BigImeiCount(lisPall);
                    if (bigCount!= V_delivery_quantity)
                    {
                        DT_RETURN dT_RETURN = new DT_RETURN
                        {
                            Success = false,
                            Service = "WMS_CK_TOTALCARTONID",
                            Msg = $"Source:CNCERepository,Line:312,Error:表格生产数据{bigCount}与发货单发货数据{V_delivery_quantity}不符"
                        };
                        return Task.FromResult(dT_RETURN);
                    }
                    break;
                case "MID":
                    string midCount = AppHelper.G_MidImeiCount(lisPall);
                    if (midCount != V_delivery_quantity)
                    {
                        DT_RETURN dT_RETURN = new DT_RETURN
                        {
                            Success = false,
                            Service = "WMS_CK_TOTALCARTONID",
                            Msg = $"Source:CNCERepository,Line:312,Error:表格生产数据{midCount}与发货单发货数据{V_delivery_quantity}不符"
                        };
                        return Task.FromResult(dT_RETURN);
                    }
                    break;
                default:
                    break;
            } 
            bool bl = OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, LisSql);
            if (bl)
            {
                DT_RETURN dT_RETURN = new DT_RETURN
                {
                    Success = true,
                    Service = "WMS_CK_TOTALCARTONID",
                    Msg = "已送检"
                };
                return Task.FromResult(dT_RETURN);
            }
            else
            {
                DT_RETURN dT_RETURN = new DT_RETURN
                {
                    Success = true,
                    Service = "WMS_CK_TOTALCARTONID",
                    Msg = "Source:WMS_CK_TOTALCARTONID,Line:352,Error:送检失败"
                };
                return Task.FromResult(dT_RETURN);
            } 
        }

        public Task<DT_RETURN> WMS_QACHECK_DATA(string v_DN, string v_delievrydate, string v_date1, string v_date2, string v_dntype, string cRT_USER)
        {
            DT_RETURN dT_RETURN = new DT_RETURN();
            List<DT_QACHECK_DATA> LsdT_s = new List<DT_QACHECK_DATA>();
            string EmsCode = AppHelper.G_EMSCODE(cRT_USER);
            string sql = @"SELECT SSCC,A.DN1,INPUTDATE,A.INPUTER,B.V_DELIVERY_QUANTITY,V_PO,V_POLINE,V_PONB,V_PN,V_CPN,V_PROD_DESC_CUST,T100MO,V_CODE,V_NAME,V_DELIVERY_ID,V_LOGISTICSORDER,V_LOGISTICS,V_ADDRESS
                        , V_DELIEVRYDATE, V_WAREHOUSE_ID, V_DELIVERY_STOCK, V_SHIPUSER, V_SHIPADDRESS,DNTYPE,V_EMSCODE,A.CRT_DATE,A.CRT_USER
                         FROM WMS_SSCCMODEL  A
                        LEFT JOIN WMS_ORDERS B ON A.DN1 = B.V_DN WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(v_DN))
            {
                sql += $" AND DN1='{v_DN}'";
            }
            if (!string.IsNullOrWhiteSpace(v_delievrydate))
            {
                sql += $" AND V_DELIEVRYDATE='{v_delievrydate}'";
            }
            if (!string.IsNullOrWhiteSpace(v_date1))
            {
                sql += $" AND INPUTDATE BETWEEN TO_DATE('{v_date1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{v_date2}','YYYY-MM-DD HH24:MI:SS')";
            }
            if (!string.IsNullOrWhiteSpace(EmsCode) && EmsCode != "ZN")
            {
                sql += $" AND V_EMSCODE='{EmsCode}'";
            }
            sql += $" AND DNTYPE='{v_dntype}'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable.Rows.Count>0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DT_QACHECK_DATA dT_QACHECK_DATA = new DT_QACHECK_DATA();
                    dT_QACHECK_DATA.DN1 = dataTable.Rows[i]["DN1"].ToString();
                    dT_QACHECK_DATA.INPUTDATE = dataTable.Rows[i]["INPUTDATE"].ToString();
                    dT_QACHECK_DATA.V_DELIVERY_QUANTITY = dataTable.Rows[i]["V_DELIVERY_QUANTITY"].ToString();
                    dT_QACHECK_DATA.V_PO = dataTable.Rows[i]["V_PO"].ToString();
                    dT_QACHECK_DATA.V_POLINE = dataTable.Rows[i]["V_POLINE"].ToString();
                    dT_QACHECK_DATA.V_PONB = dataTable.Rows[i]["V_PONB"].ToString();
                    dT_QACHECK_DATA.V_PN = dataTable.Rows[i]["V_PN"].ToString();
                    dT_QACHECK_DATA.V_CPN = dataTable.Rows[i]["V_CPN"].ToString();
                    dT_QACHECK_DATA.V_PROD_DESC_CUST = dataTable.Rows[i]["V_PROD_DESC_CUST"].ToString();
                    dT_QACHECK_DATA.T100MO = dataTable.Rows[i]["T100MO"].ToString();
                    dT_QACHECK_DATA.V_CODE = dataTable.Rows[i]["V_CODE"].ToString();
                    dT_QACHECK_DATA.V_NAME = dataTable.Rows[i]["V_NAME"].ToString();
                    dT_QACHECK_DATA.V_DELIVERY_ID = dataTable.Rows[i]["V_DELIVERY_ID"].ToString();
                    dT_QACHECK_DATA.V_LOGISTICSORDER = dataTable.Rows[i]["V_LOGISTICSORDER"].ToString();
                    dT_QACHECK_DATA.V_LOGISTICS = dataTable.Rows[i]["V_LOGISTICS"].ToString();
                    dT_QACHECK_DATA.V_ADDRESS = dataTable.Rows[i]["V_ADDRESS"].ToString();
                    dT_QACHECK_DATA.V_DELIEVRYDATE = dataTable.Rows[i]["V_DELIEVRYDATE"].ToString();
                    dT_QACHECK_DATA.V_WAREHOUSE_ID = dataTable.Rows[i]["V_WAREHOUSE_ID"].ToString();
                    dT_QACHECK_DATA.V_DELIVERY_STOCK = dataTable.Rows[i]["V_DELIVERY_STOCK"].ToString();
                    dT_QACHECK_DATA.V_SHIPUSER = dataTable.Rows[i]["V_SHIPUSER"].ToString();
                    dT_QACHECK_DATA.V_SHIPADDRESS = dataTable.Rows[i]["V_SHIPADDRESS"].ToString();
                    dT_QACHECK_DATA.INPUTER = dataTable.Rows[i]["INPUTER"].ToString();
                    dT_QACHECK_DATA.SSCC = dataTable.Rows[i]["SSCC"].ToString();
                    dT_QACHECK_DATA.DNTYPE = dataTable.Rows[i]["DNTYPE"].ToString();
                    dT_QACHECK_DATA.V_EMSCODE = dataTable.Rows[i]["V_EMSCODE"].ToString();
                    dT_QACHECK_DATA.CRT_DATE = dataTable.Rows[i]["CRT_DATE"].ToString();
                    dT_QACHECK_DATA.CRT_USER = dataTable.Rows[i]["CRT_USER"].ToString();
                    LsdT_s.Add(dT_QACHECK_DATA);
                }
            }
            dT_RETURN.Success = true;
            dT_RETURN.Msg = "查询完成";
            dT_RETURN.QA_DATA = LsdT_s;
            return Task.FromResult(dT_RETURN);
        }
        public Task<DT_RETURN> WMS_QAENDOUT_DATA(string v_DN, string v_delievrydate, string v_date1, string v_date2, string v_dntype, string cRT_USER)
        {
            DT_RETURN dT_RETURN = new DT_RETURN();
            List<DT_QACHECK_DATA> LsdT_s = new List<DT_QACHECK_DATA>();
            string EmsCode = AppHelper.G_EMSCODE(cRT_USER);
            string sql = @"SELECT SSCC,A.DN1,INPUTDATE,A.INPUTER,B.V_DELIVERY_QUANTITY,V_PO,V_POLINE,V_PONB,V_PN,V_CPN,V_PROD_DESC_CUST,T100MO,V_CODE,V_NAME,V_DELIVERY_ID,V_LOGISTICSORDER,V_LOGISTICS,V_ADDRESS
                        , V_DELIEVRYDATE, V_WAREHOUSE_ID, V_DELIVERY_STOCK, V_SHIPUSER, V_SHIPADDRESS,DNTYPE,V_EMSCODE,A.CRT_DATE,A.CRT_USER
                         FROM WMS_SSCCMODEL  A
                        LEFT JOIN WMS_ORDERS B ON A.DN1 = B.V_DN WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(v_DN))
            {
                sql += $" AND DN1='{v_DN}'";
            }
            if (!string.IsNullOrWhiteSpace(v_delievrydate))
            {
                sql += $" AND V_DELIEVRYDATE='{v_delievrydate}'";
            }
            if (!string.IsNullOrWhiteSpace(v_date1))
            {
                sql += $" AND A.CRT_DATE BETWEEN TO_DATE('{v_date1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{v_date2}','YYYY-MM-DD HH24:MI:SS')";
            }
            if (!string.IsNullOrWhiteSpace(EmsCode) && EmsCode != "ZN")
            {
                sql += $" AND V_EMSCODE='{EmsCode}'";
            }
            sql += $" AND DNTYPE='{v_dntype}'";
           
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DT_QACHECK_DATA dT_QACHECK_DATA = new DT_QACHECK_DATA();
                    dT_QACHECK_DATA.DN1 = dataTable.Rows[i]["DN1"].ToString();
                    dT_QACHECK_DATA.INPUTDATE = dataTable.Rows[i]["INPUTDATE"].ToString();
                    dT_QACHECK_DATA.V_DELIVERY_QUANTITY = dataTable.Rows[i]["V_DELIVERY_QUANTITY"].ToString();
                    dT_QACHECK_DATA.V_PO = dataTable.Rows[i]["V_PO"].ToString();
                    dT_QACHECK_DATA.V_POLINE = dataTable.Rows[i]["V_POLINE"].ToString();
                    dT_QACHECK_DATA.V_PONB = dataTable.Rows[i]["V_PONB"].ToString();
                    dT_QACHECK_DATA.V_PN = dataTable.Rows[i]["V_PN"].ToString();
                    dT_QACHECK_DATA.V_CPN = dataTable.Rows[i]["V_CPN"].ToString();
                    dT_QACHECK_DATA.V_PROD_DESC_CUST = dataTable.Rows[i]["V_PROD_DESC_CUST"].ToString();
                    dT_QACHECK_DATA.T100MO = dataTable.Rows[i]["T100MO"].ToString();
                    dT_QACHECK_DATA.V_CODE = dataTable.Rows[i]["V_CODE"].ToString();
                    dT_QACHECK_DATA.V_NAME = dataTable.Rows[i]["V_NAME"].ToString();
                    dT_QACHECK_DATA.V_DELIVERY_ID = dataTable.Rows[i]["V_DELIVERY_ID"].ToString();
                    dT_QACHECK_DATA.V_LOGISTICSORDER = dataTable.Rows[i]["V_LOGISTICSORDER"].ToString();
                    dT_QACHECK_DATA.V_LOGISTICS = dataTable.Rows[i]["V_LOGISTICS"].ToString();
                    dT_QACHECK_DATA.V_ADDRESS = dataTable.Rows[i]["V_ADDRESS"].ToString();
                    dT_QACHECK_DATA.V_DELIEVRYDATE = dataTable.Rows[i]["V_DELIEVRYDATE"].ToString();
                    dT_QACHECK_DATA.V_WAREHOUSE_ID = dataTable.Rows[i]["V_WAREHOUSE_ID"].ToString();
                    dT_QACHECK_DATA.V_DELIVERY_STOCK = dataTable.Rows[i]["V_DELIVERY_STOCK"].ToString();
                    dT_QACHECK_DATA.V_SHIPUSER = dataTable.Rows[i]["V_SHIPUSER"].ToString();
                    dT_QACHECK_DATA.V_SHIPADDRESS = dataTable.Rows[i]["V_SHIPADDRESS"].ToString();
                    dT_QACHECK_DATA.INPUTER = dataTable.Rows[i]["INPUTER"].ToString();
                    dT_QACHECK_DATA.SSCC = dataTable.Rows[i]["SSCC"].ToString();
                    dT_QACHECK_DATA.DNTYPE = dataTable.Rows[i]["DNTYPE"].ToString();
                    dT_QACHECK_DATA.V_EMSCODE = dataTable.Rows[i]["V_EMSCODE"].ToString();
                    dT_QACHECK_DATA.CRT_DATE = dataTable.Rows[i]["CRT_DATE"].ToString();
                    dT_QACHECK_DATA.CRT_USER = dataTable.Rows[i]["CRT_USER"].ToString();
                    LsdT_s.Add(dT_QACHECK_DATA);
                }
            }
            dT_RETURN.Success = true;
            dT_RETURN.Msg = "查询完成";
            dT_RETURN.QA_DATA = LsdT_s;
            return Task.FromResult(dT_RETURN);
        }
        /// <summary>
        /// 数据发送华盛
        /// </summary>
        /// <param name="v_DN"></param>
        /// <param name="v_SSCC"></param>
        /// <param name="v_CODE"></param>
        /// <param name="cRT_USER"></param>
        /// <returns></returns>
        public Task<DT_RETURN> WMS_QAOK_DATA(string v_DN,string v_SSCC, string v_CODE, string cRT_USER)
        {
            DT_RETURN dT_RETURN = new DT_RETURN();
            string sql = $"SELECT BOXTYPE,DNTYPE FROM WMS_SSCCMODEL T WHERE SSCC='{v_SSCC}' AND DN1='{v_DN}'  ";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable==null)
            {
                dT_RETURN.Success = false;
                dT_RETURN.Msg = "Source:CNCERepository,Line:424,Error:数据库错误";
                return Task.FromResult(dT_RETURN);
            }
            if (dataTable.Rows.Count > 0)
            {
                string boxType = dataTable.Rows[0]["BOXTYPE"].ToString();
                string dnType = dataTable.Rows[0]["DNTYPE"].ToString();
                if (dnType == "2")
                {
                    dT_RETURN.Success = false;
                    dT_RETURN.Msg = $"Source:CNCERepository,Line:424,Error:发货单{v_DN}已经拒收";
                    return Task.FromResult(dT_RETURN);
                }
                if (dnType == "1")
                {
                    dT_RETURN.Success = false;
                    dT_RETURN.Msg = $"Source:CNCERepository,Line:424,Error:发货单{v_DN}已经允收";
                    return Task.FromResult(dT_RETURN);
                }
                bool bl = true;
                string Err = string.Empty;

                sql = $"SELECT A.BOXSN FROM WMS_SSCCINFO A WHERE  SSCC='{v_SSCC}' AND DN1='{v_DN}'";
                DataTable dataTable1 = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
                if (dataTable1.Rows.Count > 0)
                {
                    //for (int i = 0; i < dataTable1.Rows.Count; i++) 
                        //string boXsn = dataTable1.Rows[i][0].ToString();//栈板 中箱号查IMEI？
                        string APP_ID = "ENyuRk8Ebm";
                        string APPSECRET = "YSZX2Igx11Svb3IyK467hOSqH3CdyxIL";
                        //string APPKEY = "ENyuRk8Ebm";//生产环境
                               APPSECRET = "33O4kWiYYFGm1pWp68leB6ANtp8HOHg3";
                        string appMethod = "";
                        string TIMESTAMP = "";
                        string TRANS_ID = "";
                        sql = " SELECT TRUNC(DBMS_RANDOM.VALUE(100000,999999))DAT1,TO_CHAR(systimestamp,'yyyy-MM-dd HH24:MM:SS ff3')DAT2 FROM DUAL T";
                        dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
                        if (dataTable.Rows.Count > 0)
                        {
                            TIMESTAMP = dataTable.Rows[0][1].ToString();
                            string Rnb = dataTable.Rows[0][0].ToString();
                            TRANS_ID = TIMESTAMP.Replace(" ", "");
                            TRANS_ID = TRANS_ID.Replace("-", "");
                            TRANS_ID = TRANS_ID.Replace(":", "");
                            TRANS_ID = TRANS_ID + Rnb;
                        }
                        string TOKEN1 = "APP_ID" + APP_ID + "TIMESTAMP" + TIMESTAMP + "TRANS_ID" + TRANS_ID + APPSECRET;
                        string TOKEN = MD5_F.Encrypt(TOKEN1);
                        H1_HELP h1_HELP = new H1_HELP();
                        List<RESERVED> rESERVED = new List<RESERVED>();
                        RESERVED rESERVED1 = new RESERVED
                        {
                            RESERVED_ID = "",
                            RESERVED_VALUE = ""
                        };
                        rESERVED.Add(rESERVED1);
                        UNI_BSS_HEAD uNI_BSS_HEAD = new UNI_BSS_HEAD
                        {
                            APP_ID = APP_ID,
                            TIMESTAMP = TIMESTAMP,
                            TRANS_ID = TRANS_ID,
                            TOKEN = TOKEN,
                            RESERVED = rESERVED
                        };
                        h1_HELP.UNI_BSS_HEAD = uNI_BSS_HEAD;
                        RECEIVE_DELIVERY_REQ rECEIVE_DELIVERY_REQ = new RECEIVE_DELIVERY_REQ();
                        string sql1 = string.Empty;
                        sql = $@"SELECT V_DN V_DELIVERY_ID,V_LOGISTICSORDER,V_LOGISTICS,V_ADDRESS,V_DELIEVRYDATE,V_WAREHOUSE_ID,V_DELIVERY_STOCK,V_PN V_PROD_CODE_SALE,V_CPN V_PROD_CODE_CUST,V_PROD_DESC_CUST,V_PO V_CUST_PURCHASEORDER, V_POLINE V_CUST_PURCHASELINE,
                       V_DELIVERY_QUANTITY FROM WMS_ORDERS WHERE V_DN = '{v_DN}'";
                        dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
                        if (dataTable.Rows.Count > 0)
                        {
                            int ShipCount = Convert.ToInt32(dataTable.Rows[0]["V_DELIVERY_QUANTITY"].ToString());
                            string date1 = dataTable.Rows[0]["V_DELIEVRYDATE"].ToString();
                            if (!string.IsNullOrWhiteSpace(date1))
                            {
                                date1 = date1.Replace("-","");
                            }
                            receiveDelivery receiveDelivery = new receiveDelivery
                            {
                                delivery_id = dataTable.Rows[0]["V_DELIVERY_ID"].ToString(),
                                logisticsorder = dataTable.Rows[0]["V_LOGISTICSORDER"].ToString(),
                                logistics = dataTable.Rows[0]["V_LOGISTICS"].ToString(),
                                address = dataTable.Rows[0]["V_ADDRESS"].ToString(),
                                delievrydate = date1,
                                warehouse_id = dataTable.Rows[0]["V_WAREHOUSE_ID"].ToString(),
                                delivery_stock = dataTable.Rows[0]["V_DELIVERY_STOCK"].ToString(),
                                prod_code_sale = dataTable.Rows[0]["V_PROD_CODE_SALE"].ToString(),
                                prod_code_cust = dataTable.Rows[0]["V_PROD_CODE_CUST"].ToString(),
                                prod_desc_cust = dataTable.Rows[0]["V_PROD_DESC_CUST"].ToString(),
                                cust_purchaseorder = dataTable.Rows[0]["V_CUST_PURCHASEORDER"].ToString(),
                                cust_purchaseline = dataTable.Rows[0]["V_CUST_PURCHASELINE"].ToString(),
                                delivery_quantity = Convert.ToInt32(dataTable.Rows[0]["V_DELIVERY_QUANTITY"].ToString())
                            };
                            List<receiveLine> lisRData = new List<receiveLine>();
                              sql1 = $@"SELECT '{v_SSCC}'SSCC, D.EANCODE EAN_UPC_CODE,C.PHYSICSNO IMEI,C.IMEI2,C.MEID,C.WIFI MAC,E.BARCODE PRODUCT_BARCODE,A.MIDCARTONID CATON_ID_HW,B.BIGCARTONID PALLET_ID_HW,
                                    (SELECT T.WEIGHT / 1000 FROM PRM_GIFTBOXWEIGHT T WHERE T.SN = E.BARCODE)WEIGHT1,(SELECT T1.WEIGHT FROM ODM_MIDCARTONINFO T1 WHERE T1.MIDCARTONID = A.MIDCARTONID)WEIGHT2,
                                    (SELECT T2.ROUGH_WEIGHT FROM EDI_CARTONWEIGHT T2 WHERE T2.CARTON = B.BIGCARTONID)WEIGHT3,C.UDID,NVL((CASE WHEN LENGTH(C.MEMORY)=32 THEN C.MEMORY ELSE (CASE WHEN INSTR(C.MEMORY,',',1,3)>0 THEN
                                    SUBSTR(C.MEMORY,INSTR(C.MEMORY,',',1,3)+1,LENGTH(C.MEMORY)-INSTR(C.MEMORY,',',1,3)) ELSE SUBSTR(C.MEMORY,INSTR(C.MEMORY,',',1,2)+1,LENGTH(C.MEMORY)-INSTR(C.MEMORY,',',1,2)) END) END),C.MEMID)EMMC_ID,NVL(C.SVER,NVL(C.SVER2,D.SOFTVER))SVER
                                     FROM ODM_PACKING A
                                    LEFT OUTER JOIN ODM_LINKLISTOFPHYSICSNO C ON TO_CHAR(A.IMEI) = C.PHYSICSNO
                                    LEFT OUTER JOIN ODM_BIGCARTONPACKING B ON B.MIDCARTONID = A.MIDCARTONID
                                    LEFT OUTER JOIN WORK_WORKJOB D ON A.WORKORDER = D.WORKJOB_CODE
                                    LEFT OUTER JOIN BARCODEREMP E ON C.SN = E.BARCODE     WHERE 1=1 ";
                            if (boxType=="MID")
                            {
                                sql1 += $" AND A.MIDCARTONID IN (SELECT A.BOXSN FROM WMS_SSCCINFO A WHERE  SSCC='{v_SSCC}' AND DN1='{v_DN}')";
                            }
                            else
                            {
                                sql1 += $" AND B.BIGCARTONID  IN (SELECT A.BOXSN FROM WMS_SSCCINFO A WHERE  SSCC='{v_SSCC}' AND DN1='{v_DN}')";
                            }  
                            DataTable dataTable2 = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql1);
                            if (dataTable2.Rows.Count > 0)
                            {
                                if (ShipCount!= dataTable2.Rows.Count)
                                {
                                    dT_RETURN.Success = false;
                                    dT_RETURN.Msg = $"Source:CNCERepository,Line:559,Error:生产数据与订单{v_DN}发货数量不一致";
                                    return Task.FromResult(dT_RETURN);
                                }
                                for (int j = 0; j < ShipCount; j++)
                                {
                                    receiveLine receiveLine1 = new receiveLine
                                    {
                                        ean_upc_code = dataTable2.Rows[j]["EAN_UPC_CODE"].ToString(),
                                        imei = dataTable2.Rows[j]["IMEI"].ToString(),
                                        imei2 = dataTable2.Rows[j]["IMEI2"].ToString(),
                                        meid = dataTable2.Rows[j]["MEID"].ToString(),
                                        mac = dataTable2.Rows[j]["MAC"].ToString(),
                                        product_barcode = dataTable2.Rows[j]["PRODUCT_BARCODE"].ToString(),
                                        caton_id_hw = dataTable2.Rows[j]["CATON_ID_HW"].ToString(),
                                        pallet_id_hw = dataTable2.Rows[j]["PALLET_ID_HW"].ToString(),
                                        weight1 = dataTable2.Rows[j]["WEIGHT1"].ToString(),
                                        weight2 = dataTable2.Rows[j]["WEIGHT2"].ToString(),
                                        weight3 = dataTable2.Rows[j]["WEIGHT3"].ToString(),
                                        udid = dataTable2.Rows[j]["UDID"].ToString(),
                                        emmc_id = dataTable2.Rows[j]["EMMC_ID"].ToString(),
                                    };
                                    lisRData.Add(receiveLine1);
                                }
                            }
                            else
                            {
                                dT_RETURN.Success = false;
                                dT_RETURN.Msg = $"Source:CNCERepository,Line:559,Error:订单{v_DN}查询无数据记录";
                                return Task.FromResult(dT_RETURN);
                            }
                            receiveDelivery.items = lisRData;
                            rECEIVE_DELIVERY_REQ.data = receiveDelivery;
                        }
                        //rECEIVE_DELIVERY_REQ.items = lisRData;
                        UNI_BSS_BODY uNI_BSS_BODY = new UNI_BSS_BODY
                        {
                            RECEIVE_DELIVERY_REQ = rECEIVE_DELIVERY_REQ
                        };
                        h1_HELP.UNI_BSS_BODY = uNI_BSS_BODY;

                        UNI_BSS_ATTACHED uNI_BSS_ATTACHED = new UNI_BSS_ATTACHED
                        {
                            MEDIA_INFO = ""
                        };
                        h1_HELP.UNI_BSS_ATTACHED = uNI_BSS_ATTACHED;
                        //string uSqls = "INSERT INTO WMS_SSCCSTOCKLOG (SSCC,IMEI,IMEI2,MEID,MAC,PRODUCT_BARCODE,CATON_ID_HW,PALLET_ID_HW,WEIGHT1,WEIGHT2,WEIGHT3,UDID,EMMC_ID) SELECT SSCC,IMEI,IMEI2,MEID,MAC,PRODUCT_BARCODE,CATON_ID_HW,PALLET_ID_HW,WEIGHT1,WEIGHT2,WEIGHT3,UDID,EMMC_ID FROM(" + sql1 + ")";
                        //int Inss = OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, uSqls);
                        string Url = "https://open.chinaunicom.cn/api/huasheng/tianduan/receiveDelivery/v1";
                        string strJson = JsonConvert.SerializeObject(h1_HELP);
                        string Lg = AppHelper.HttpApi(Url, strJson, "POST");
                        //json反编译
                        JObject jo = (JObject)JsonConvert.DeserializeObject(Lg);
                        if (jo==null)
                        {
                            AppHelper.InsertWMS_LOG(v_DN, Lg, boxType, boxType, v_SSCC, cRT_USER, "JObject异常", "JObject异常");
                            dT_RETURN.Success = false;
                            dT_RETURN.Msg = $"Source:CNCERepository,Line:596,ErrCode: JObject异常";
                            return Task.FromResult(dT_RETURN);
                        }
                        string JUni_bss_head = string.IsNullOrWhiteSpace(jo["UNI_BSS_HEAD"].ToString()) ? "" : jo["UNI_BSS_HEAD"].ToString();
                        string JUni_bss_body = string.IsNullOrWhiteSpace(jo["UNI_BSS_BODY"].ToString()) ? "" : jo["UNI_BSS_BODY"].ToString();
                        if (!string.IsNullOrWhiteSpace(JUni_bss_head))
                        {
                            jo = (JObject)JsonConvert.DeserializeObject(JUni_bss_head);
                            if (jo == null)
                            {
                                AppHelper.InsertWMS_LOG(v_DN, Lg, boxType, boxType, v_SSCC, cRT_USER, "JObject异常", "JObject异常");
                                dT_RETURN.Success = false;
                                dT_RETURN.Msg = $"Source:CNCERepository,Line:609,ErrCode: JObject异常";
                                return Task.FromResult(dT_RETURN);
                            }
                            string RESP_CODE = string.IsNullOrWhiteSpace(jo["RESP_CODE"].ToString()) ? "" : jo["RESP_CODE"].ToString();
                            string RESP_DESC = string.IsNullOrWhiteSpace(jo["RESP_DESC"].ToString()) ? "" : jo["RESP_DESC"].ToString();
                            //接收天擎系统数据
                            if (RESP_CODE == "00000")
                            {
                                jo = (JObject)JsonConvert.DeserializeObject(JUni_bss_body);
                                if (jo == null)
                                {
                                    AppHelper.InsertWMS_LOG(v_DN, Lg, boxType, boxType, v_SSCC, cRT_USER, "JObject异常", "JObject异常");
                                    dT_RETURN.Success = false;
                                    dT_RETURN.Msg = $"Source:CNCERepository,Line:662,ErrCode: JObject异常";
                                    return Task.FromResult(dT_RETURN);
                                }
                                 string RECEIVE_DELIVERY_RSP= string.IsNullOrWhiteSpace(jo["RECEIVE_DELIVERY_RSP"].ToString()) ? "" : jo["RECEIVE_DELIVERY_RSP"].ToString();
                                jo = (JObject)JsonConvert.DeserializeObject(RECEIVE_DELIVERY_RSP);
                                if (jo == null)
                                {
                                    AppHelper.InsertWMS_LOG(v_DN, Lg, boxType, boxType, v_SSCC, cRT_USER, "JObject异常", "JObject异常");
                                    dT_RETURN.Success = false;
                                    dT_RETURN.Msg = $"Source:CNCERepository,Line:630,ErrCode: JObject异常";
                                    return Task.FromResult(dT_RETURN);
                                }
                                string code = string.IsNullOrWhiteSpace(jo["code"].ToString()) ? "" : jo["code"].ToString();
                                string message = string.IsNullOrWhiteSpace(jo["message"].ToString()) ? "" : jo["message"].ToString();
                                //接收华盛接口数据
                                if (code == "200")
                                {
                                    AppHelper.InsertWMS_LOG(v_DN, Lg, boxType, boxType, v_SSCC, cRT_USER, RESP_CODE, RESP_DESC);
                                    sql = $"SELECT A.BOXSN FROM WMS_SSCCINFO A WHERE  SSCC='{v_SSCC}' AND DN1='{v_DN}'";
                                    DataTable dataTable2 = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
                                    if (dataTable2.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dataTable2.Rows.Count; i++)
                                        {
                                            string boXsn = dataTable2.Rows[i][0].ToString();
                                            string uSql = $"INSERT INTO WMS_SSCCSTOCKOUT(DN1,BOXTYPE,BOXSN,SSCC,INPUTER)VALUES('{v_DN}','{boxType}','{boXsn}','{v_SSCC}','{cRT_USER}')";
                                            int Ins = OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, uSql);
                                            uSql = $"UPDATE  WMS_SSCCINFO T SET T.SHIPDATE=TO_CHAR(SYSDATE,'YYYYMMDD'),WWNO='OK' WHERE BOXSN='{boXsn}'";
                                            Ins = OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, uSql);
                                           
                                        }
                                    } 
                                }
                                else
                                {
                                    AppHelper.InsertWMS_LOG(v_DN, Lg, boxType, boxType, v_SSCC, cRT_USER, RESP_CODE, RESP_DESC);
                                    dT_RETURN.Success = false;
                                    dT_RETURN.Msg = $"Source:CNCERepository,Line:659,ErrCode:{RESP_CODE},Error:订单{v_DN}上传失败:{message}";
                                    return Task.FromResult(dT_RETURN);
                                }
                            
                            }
                            else
                            {
                                AppHelper.InsertWMS_LOG(v_DN, Lg, boxType, boxType, v_SSCC, cRT_USER, RESP_CODE, RESP_DESC);
                                dT_RETURN.Success = false;
                                dT_RETURN.Msg = $"Source:CNCERepository,Line:668,ErrCode:{RESP_CODE},Error:订单{v_DN}上传失败:{RESP_DESC}";
                                return Task.FromResult(dT_RETURN);
                            }
                        }
                    string uSql1 = "INSERT INTO WMS_SSCCSTOCKLOG (SSCC,IMEI,IMEI2,MEID,MAC,PRODUCT_BARCODE,CATON_ID_HW,PALLET_ID_HW,WEIGHT1,WEIGHT2,WEIGHT3,UDID,EMMC_ID,SVER) SELECT SSCC,IMEI,IMEI2,MEID,MAC,PRODUCT_BARCODE,CATON_ID_HW,PALLET_ID_HW,WEIGHT1,WEIGHT2,WEIGHT3,UDID,EMMC_ID,SVER FROM(" + sql1 + ")";
                    OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, uSql1);
                    //上传完成后写log
                    string udSql = $"UPDATE WMS_SSCCMODEL SET DNTYPE=1,CRT_DATE=SYSDATE,CRT_USER='{cRT_USER}' WHERE  SSCC='{v_SSCC}' AND DN1='{v_DN}' ";
                    OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, udSql);
                    dT_RETURN.Success = true;
                    dT_RETURN.Msg = $"允收OK";
                    return Task.FromResult(dT_RETURN);
                }
                else
                {
                    dT_RETURN.Success = false;
                    dT_RETURN.Msg = "Source:CNCERepository,Line:424,Error:WMS_SSCCINFO表无数据";
                    return Task.FromResult(dT_RETURN);
                } 
            }
            else
            {
                dT_RETURN.Success = false;
                dT_RETURN.Msg = $"Source:CNCERepository,Line:424,Error:发货单{v_DN}不存在";
                return Task.FromResult(dT_RETURN);
            }
            throw new NotImplementedException();
        }

        public IEnumerable<DT_QAOUT_DATA> GetQAOK_DATA(string v_DN, string v_SSCC, string v_CODE, ref string v_msg)
        {
            List<DT_QAOUT_DATA> LsdT_TAs = new List<DT_QAOUT_DATA>();
            string sql = $"SELECT BOXTYPE,DNTYPE FROM WMS_SSCCMODEL T WHERE SSCC='{v_SSCC}' AND DN1='{v_DN}'  ";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                v_msg = "数据库异常";
                return null;
            }
            if (dataTable.Rows.Count > 0)
            {
                string boxType = dataTable.Rows[0]["BOXTYPE"].ToString();
                string dnType = dataTable.Rows[0]["DNTYPE"].ToString();
                string sql1 = string.Empty;
                if (boxType == "MID")
                {
                    sql1 += $@" SELECT A1.V_DN,V_PO,V_POLINE,V_PN,V_CPN,V_DELIVERY_QUANTITY, D.EANCODE EAN_UPC_CODE,C.PHYSICSNO IMEI,C.IMEI2,C.MEID,C.WIFI MAC,E.BARCODE PRODUCT_BARCODE,A.MIDCARTONID CATON_ID_HW,B.BIGCARTONID PALLET_ID_HW,
                                    (SELECT T.WEIGHT  FROM PRM_GIFTBOXWEIGHT T WHERE T.SN = E.BARCODE)WEIGHT1,(SELECT T1.WEIGHT FROM ODM_MIDCARTONINFO T1 WHERE T1.MIDCARTONID = A.MIDCARTONID)WEIGHT2,
                                    (SELECT T2.ROUGH_WEIGHT FROM EDI_CARTONWEIGHT T2 WHERE T2.CARTON = B.BIGCARTONID)WEIGHT3,C.UDID,NVL((CASE WHEN LENGTH(C.MEMORY)=32 THEN C.MEMORY ELSE (CASE WHEN INSTR(C.MEMORY,',',1,3)>0 THEN
                                     SUBSTR(C.MEMORY,INSTR(C.MEMORY,',',1,3)+1,LENGTH(C.MEMORY)-INSTR(C.MEMORY,',',1,3)) ELSE SUBSTR(C.MEMORY,INSTR(C.MEMORY,',',1,2)+1,LENGTH(C.MEMORY)-INSTR(C.MEMORY,',',1,2)) END) END),C.MEMID)EMMC_ID,NVL(C.SVER, NVL(C.SVER2, D.SOFTVER))SVER
                                      FROM  WMS_ORDERS A1
                                    LEFT OUTER JOIN WMS_SSCCINFO A2 ON A1.V_DN = A2.DN1 
                                    LEFT OUTER JOIN ODM_PACKING A ON A.MIDCARTONID = A2.BOXSN
                                    LEFT OUTER JOIN ODM_BIGCARTONPACKING B ON B.MIDCARTONID = A.MIDCARTONID
                                    LEFT OUTER JOIN ODM_LINKLISTOFPHYSICSNO C ON TO_CHAR(A.IMEI) = C.PHYSICSNO
                                    LEFT OUTER JOIN WORK_WORKJOB D ON A.WORKORDER = D.WORKJOB_CODE
                                    LEFT OUTER JOIN BARCODEREMP E ON C.SN = E.BARCODE     WHERE 1 = 1
                                    AND  SSCC='{v_SSCC}' AND DN1='{v_DN}'";
                }
                else
                {
                    sql1 += $@" SELECT A1.V_DN,V_PO,V_POLINE,V_PN,V_CPN,V_DELIVERY_QUANTITY, D.EANCODE EAN_UPC_CODE,C.PHYSICSNO IMEI,C.IMEI2,C.MEID,C.WIFI MAC,E.BARCODE PRODUCT_BARCODE,A.MIDCARTONID CATON_ID_HW,B.BIGCARTONID PALLET_ID_HW,
                                    (SELECT T.WEIGHT  FROM PRM_GIFTBOXWEIGHT T WHERE T.SN = E.BARCODE)WEIGHT1,(SELECT T1.WEIGHT FROM ODM_MIDCARTONINFO T1 WHERE T1.MIDCARTONID = A.MIDCARTONID)WEIGHT2,
                                    (SELECT T2.ROUGH_WEIGHT FROM EDI_CARTONWEIGHT T2 WHERE T2.CARTON = B.BIGCARTONID)WEIGHT3,C.UDID,NVL((CASE WHEN LENGTH(C.MEMORY)=32 THEN C.MEMORY ELSE (CASE WHEN INSTR(C.MEMORY,',',1,3)>0 THEN
                                     SUBSTR(C.MEMORY,INSTR(C.MEMORY,',',1,3)+1,LENGTH(C.MEMORY)-INSTR(C.MEMORY,',',1,3)) ELSE SUBSTR(C.MEMORY,INSTR(C.MEMORY,',',1,2)+1,LENGTH(C.MEMORY)-INSTR(C.MEMORY,',',1,2)) END) END),C.MEMID)EMMC_ID,NVL(C.SVER, NVL(C.SVER2, D.SOFTVER))SVER
                                      FROM  WMS_ORDERS A1
                                    LEFT OUTER JOIN WMS_SSCCINFO A2 ON A1.V_DN = A2.DN1
                                    LEFT OUTER JOIN ODM_BIGCARTONPACKING B ON B.BIGCARTONID = A2.BOXSN
                                    LEFT OUTER JOIN ODM_PACKING A ON A.MIDCARTONID = B.MIDCARTONID
                                    LEFT OUTER JOIN ODM_LINKLISTOFPHYSICSNO C ON TO_CHAR(A.IMEI) = C.PHYSICSNO
                                    LEFT OUTER JOIN WORK_WORKJOB D ON A.WORKORDER = D.WORKJOB_CODE
                                    LEFT OUTER JOIN BARCODEREMP E ON C.SN = E.BARCODE     WHERE 1 = 1
                                    AND  SSCC='{v_SSCC}' AND DN1='{v_DN}'";
                }
                DataTable dataTable1 = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable1.Rows.Count; i++)
                    {
                        DT_QAOUT_DATA t_QAOUT_DATA = new DT_QAOUT_DATA
                        {
                            V_DN = dataTable1.Rows[i]["V_DN"].ToString(),
                            V_PO = dataTable1.Rows[i]["V_PO"].ToString(),
                            V_POLINE = dataTable1.Rows[i]["V_POLINE"].ToString(),
                            V_PN = dataTable1.Rows[i]["V_PN"].ToString(),
                            V_CPN = dataTable1.Rows[i]["V_CPN"].ToString(),
                            V_DELIVERY_QUANTITY = dataTable1.Rows[i]["V_DELIVERY_QUANTITY"].ToString(),
                            EAN_UPC_CODE = dataTable1.Rows[i]["EAN_UPC_CODE"].ToString(),
                            IMEI = dataTable1.Rows[i]["IMEI"].ToString(),
                            IMEI2 = dataTable1.Rows[i]["IMEI2"].ToString(),
                            MAC = dataTable1.Rows[i]["MAC"].ToString(),
                            PRODUCT_BARCODE = dataTable1.Rows[i]["PRODUCT_BARCODE"].ToString(),
                            CATON_ID_HW = dataTable1.Rows[i]["CATON_ID_HW"].ToString(),
                            PALLET_ID_HW = dataTable1.Rows[i]["PALLET_ID_HW"].ToString(),
                            WEIGHT1 = dataTable1.Rows[i]["WEIGHT1"].ToString(),
                            WEIGHT2 = dataTable1.Rows[i]["WEIGHT2"].ToString(),
                            WEIGHT3 = dataTable1.Rows[i]["WEIGHT3"].ToString(),
                            UDID = dataTable1.Rows[i]["UDID"].ToString(),
                            EMMC_ID = dataTable1.Rows[i]["EMMC_ID"].ToString(),
                            SVER = dataTable1.Rows[i]["SVER"].ToString()
                        };
                        LsdT_TAs.Add(t_QAOUT_DATA);
                    }
                }
            }
            return LsdT_TAs;
            throw new NotImplementedException();
        }

        public Task<DT_RETURN> WMS_QANG_DATA(string v_DN, string v_SSCC, string v_CODE, string cRT_USER)
        {
            DT_RETURN dT_RETURN = new DT_RETURN();
            string sql = $"SELECT BOXTYPE,DNTYPE FROM WMS_SSCCMODEL T WHERE SSCC='{v_SSCC}' AND DN1='{v_DN}'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable == null)
            {
                dT_RETURN.Success = false;
                dT_RETURN.Msg = "Source:CNCERepository,Line:786,Error:数据库错误";
                return Task.FromResult(dT_RETURN);
            }
            if (dataTable.Rows.Count > 0)
            {
                string boxType = dataTable.Rows[0]["BOXTYPE"].ToString();
                string dnType = dataTable.Rows[0]["DNTYPE"].ToString();
                if (dnType == "2")
                {
                    dT_RETURN.Success = false;
                    dT_RETURN.Msg = $"Source:CNCERepository,Line:797,Error:发货单{v_DN}已经拒收";
                    return Task.FromResult(dT_RETURN);
                }
                if (dnType == "1")
                {
                    dT_RETURN.Success = false;
                    dT_RETURN.Msg = $"Source:CNCERepository,Line:803,Error:发货单{v_DN}已经允收";
                    return Task.FromResult(dT_RETURN);
                }
                string uSql = $"UPDATE  WMS_SSCCINFO T SET T.SHIPDATE=TO_CHAR(SYSDATE,'YYYYMMDD'),WWNO='NG' WHERE  SSCC='{v_SSCC}' AND DN1='{v_DN}'";
                OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, uSql);
                uSql = $"UPDATE WMS_SSCCMODEL SET DNTYPE=2,CRT_DATE=SYSDATE,CRT_USER='{cRT_USER}' WHERE  SSCC='{v_SSCC}' AND DN1='{v_DN}' ";
                OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, uSql);
                dT_RETURN.Success = true;
                dT_RETURN.Msg = $"拒收OK";
                return Task.FromResult(dT_RETURN);
            }
            else
            {
                dT_RETURN.Success = false;
                dT_RETURN.Msg = $"Source:CNCERepository,Line:424,Error:发货单{v_DN}不存在";
                return Task.FromResult(dT_RETURN); 
            }
            //throw new NotImplementedException();
        }

        public Task<DT_DNINFOR> WMS_DN_INFOR(string dN)
        {
            DT_DNINFOR dT_DNINFOR = new DT_DNINFOR();
            string sql = $"SELECT V_DN,V_PO,V_POLINE,V_PN,V_CPN,V_PROD_DESC_CUST,V_PONB,T100MO,V_CODE,V_NAME,V_DELIVERY_ID,V_LOGISTICSORDER,V_LOGISTICS,V_ADDRESS,V_DELIEVRYDATE,V_WAREHOUSE_ID,V_DELIVERY_STOCK,V_DELIVERY_QUANTITY,V_OUT,V_SHIPUSER,V_SHIPADDRESS FROM WMS_ORDERS T WHERE V_DN='{dN}'";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable.Rows.Count>0)
            {
                dT_DNINFOR.V_DN = dataTable.Rows[0]["V_DN"].ToString();
                dT_DNINFOR.V_PO = dataTable.Rows[0]["V_PO"].ToString();
                dT_DNINFOR.V_POLINE = dataTable.Rows[0]["V_POLINE"].ToString();
                dT_DNINFOR.V_PN = dataTable.Rows[0]["V_PN"].ToString();
                dT_DNINFOR.V_CPN = dataTable.Rows[0]["V_CPN"].ToString();
                dT_DNINFOR.V_PROD_DESC_CUST = dataTable.Rows[0]["V_PROD_DESC_CUST"].ToString();
                dT_DNINFOR.V_PONB = dataTable.Rows[0]["V_PONB"].ToString();
                dT_DNINFOR.T100MO = dataTable.Rows[0]["T100MO"].ToString();
                dT_DNINFOR.V_CODE = dataTable.Rows[0]["V_CODE"].ToString();
                dT_DNINFOR.V_NAME = dataTable.Rows[0]["V_NAME"].ToString();
                dT_DNINFOR.V_DELIVERY_ID = dataTable.Rows[0]["V_DELIVERY_ID"].ToString();
                dT_DNINFOR.V_LOGISTICSORDER = dataTable.Rows[0]["V_LOGISTICSORDER"].ToString();
                dT_DNINFOR.V_LOGISTICS = dataTable.Rows[0]["V_LOGISTICS"].ToString();
                dT_DNINFOR.V_ADDRESS = dataTable.Rows[0]["V_ADDRESS"].ToString();
                dT_DNINFOR.V_DELIEVRYDATE = dataTable.Rows[0]["V_DELIEVRYDATE"].ToString();
                dT_DNINFOR.V_WAREHOUSE_ID = dataTable.Rows[0]["V_WAREHOUSE_ID"].ToString();
                dT_DNINFOR.V_DELIVERY_STOCK = dataTable.Rows[0]["V_DELIVERY_STOCK"].ToString();
                dT_DNINFOR.V_DELIVERY_QUANTITY = dataTable.Rows[0]["V_DELIVERY_QUANTITY"].ToString();
                dT_DNINFOR.V_OUT = dataTable.Rows[0]["V_OUT"].ToString();
                dT_DNINFOR.V_SHIPUSER = dataTable.Rows[0]["V_SHIPUSER"].ToString();
                dT_DNINFOR.V_SHIPADDRESS = dataTable.Rows[0]["V_SHIPADDRESS"].ToString();
            }
            return Task.FromResult(dT_DNINFOR);
        }

        public IEnumerable<H1_EXCELOUT>  OutExportData(string v_DN, string v_SSCC, string v_CODE, ref string v_MSG)
        {
            List<H1_EXCELOUT> LsdT_s = new List<H1_EXCELOUT>();
            string sql = $"SELECT BOXTYPE,DNTYPE FROM WMS_SSCCMODEL T WHERE SSCC='{v_SSCC}' AND DN1='{v_DN}'  ";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable.Rows.Count > 0)
            {
                sql = "";
                string boxType = dataTable.Rows[0]["BOXTYPE"].ToString();
                string dnType = dataTable.Rows[0]["DNTYPE"].ToString();
                switch (boxType)
                {
                    case "BIG":
                        sql = $@"SELECT A.V_CPN ,D.IMEI V_IMEI, A.V_PO,A.V_PROD_DESC_CUST,(SELECT T.V_PROVINCE FROM WMS_WAREHOUSE T WHERE T.V_ID =A.V_WAREHOUSE_ID)V_PROVINCE
                                 FROM WMS_ORDERS  A LEFT JOIN WMS_SSCCINFO B ON A.V_DN = B.DN1
                                 LEFT JOIN ODM_BIGCARTONPACKING C ON C.BIGCARTONID = B.BOXSN
                                 LEFT JOIN ODM_PACKING D ON D.MIDCARTONID = C.MIDCARTONID
                                WHERE B.DN1 = '{v_DN}'AND B.SSCC = '{v_SSCC}'";
                        break;
                    case "MID":
                        sql = $@"SELECT A.V_CPN ,D.IMEI V_IMEI, A.V_PO,A.V_PROD_DESC_CUST,(SELECT T.V_PROVINCE FROM WMS_WAREHOUSE T WHERE T.V_ID =A.V_WAREHOUSE_ID)V_PROVINCE
                                 FROM WMS_ORDERS  A LEFT JOIN WMS_SSCCINFO B ON A.V_DN = B.DN1 
                                 LEFT JOIN ODM_PACKING D ON D.MIDCARTONID = B.BOXSN
                                WHERE B.DN1 = '{v_DN}'AND B.SSCC = '{v_SSCC}'";
                        break;
                    default:
                        break;
                }
                dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
                if (dataTable.Rows.Count>0)
                {
                    foreach (DataRow item in dataTable.Rows)
                    {
                        H1_EXCELOUT h1_EXCELOUT = new H1_EXCELOUT();
                        h1_EXCELOUT.V_CPN = item["V_CPN"].ToString();
                        h1_EXCELOUT.V_IMEI = item["V_IMEI"].ToString();
                        h1_EXCELOUT.V_PO = item["V_PO"].ToString();
                        h1_EXCELOUT.V_PROD_DESC_CUST = item["V_PROD_DESC_CUST"].ToString();
                        h1_EXCELOUT.V_PROVINCE = item["V_PROVINCE"].ToString();
                        LsdT_s.Add(h1_EXCELOUT);
                    }
                }
            }
            return LsdT_s;
        }

        public Task<DT_RETURN> WMS_PALWEIGHT_INFOR(string pALLET)
        {
            DT_RETURN dT_RETURN = new DT_RETURN
            {
                Service = "WMS_PALWEIGHT_INFOR"
            };
            if (!AppHelper.I_BigCartoninfo(pALLET)) {
                dT_RETURN.Success = false;
                dT_RETURN.Msg = $"Source:WMS_PALWEIGHT_INFOR,Line:1058,Error:当前栈板{pALLET}不存在";
                return Task.FromResult(dT_RETURN); 
            }
            string nw = AppHelper.G_BigNW(pALLET);
            if (string.IsNullOrWhiteSpace(nw))
            {
                dT_RETURN.Success = false;
                dT_RETURN.Msg = $"Source:WMS_PALWEIGHT_INFOR,Line:1066,Error:当前栈板{pALLET}未关联中箱数据";
                return Task.FromResult(dT_RETURN);
            }
            //nw = Math.Round(Convert.ToDouble(nw) / 1000, 2).ToString();
            dT_RETURN.Success = true;
            dT_RETURN.Msg = "PASS";
            dT_RETURN.Nw = nw;
            dT_RETURN.Gw = AppHelper.G_BigGW(pALLET);
            return Task.FromResult(dT_RETURN);
            throw new NotImplementedException();
        }

        public Task<DT_RETURN> WMS_PALWEIGHT_IN(string v_PALLET, string v_GW, string v_NW)
        {
            DT_RETURN dT_RETURN = new DT_RETURN
            {
                Service = "WMS_PALWEIGHT_IN"
            };
            double gw = Convert.ToDouble(v_GW);
            double nw = Convert.ToDouble(v_NW);
            if (nw>gw)
            {
                dT_RETURN.Success = false;
                dT_RETURN.Msg = $"Source:WMS_PALWEIGHT_IN,Line:1091,Error:毛重重量不能小于净重重量";
                return Task.FromResult(dT_RETURN);
            }
            double ctt = gw - nw;
            if (ctt>=50)
            {
                dT_RETURN.Success = false;
                dT_RETURN.Msg = $"Source:WMS_PALWEIGHT_IN,Line:1091,Error:毛重、净重相差不能大于50";
                return Task.FromResult(dT_RETURN);
            }
            string mo = AppHelper.G_BigMobyInfor(v_PALLET);
            string strinsert = string.Format(@"MERGE INTO EDI_CARTONWEIGHT A USING DUAL ON(CARTON='{1}')WHEN MATCHED THEN UPDATE SET  A.WORKORDER='{0}',A.NET_WEIGHT='{2}',A.ROUGH_WEIGHT='{3}',A.TMODEL='{4}'
            WHEN NOT MATCHED THEN INSERT(A.WORKORDER,A.CARTON,A.NET_WEIGHT,A.ROUGH_WEIGHT,A.TMODEL)VALUES('{0}', '{1}','{2}','{3}','{4}')", mo, v_PALLET, v_NW, v_GW, "WEB");
            int indexn = OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, strinsert);
            dT_RETURN.Success = true;
            dT_RETURN.Msg = v_PALLET+"重量维护完成"; 
            return Task.FromResult(dT_RETURN);
        }

        public  DT_RETURN WMS_NETCODE_DATA(string v_DN, string v_SSCC)
        {
            DT_RETURN dT_RETURN = new DT_RETURN();
            List<DT_NETCODE> LsdT_s = new List<DT_NETCODE>(0);
            int NetCount = 0;
            string Mo = string.Empty;
            string Titles = string.Empty;
            string Dts= string.Empty;
            string sql = $"SELECT BOXTYPE,DNTYPE,TO_CHAR(SYSDATE,'YYYYMMDD')DT FROM WMS_SSCCMODEL T WHERE SSCC='{v_SSCC}' AND DN1='{v_DN}'  ";
            DataTable dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
            if (dataTable.Rows.Count > 0)
            {
                sql = "";
                string boxType = dataTable.Rows[0]["BOXTYPE"].ToString();
                string dnType = dataTable.Rows[0]["DNTYPE"].ToString();
                Dts = dataTable.Rows[0]["DT"].ToString();
                switch (boxType)
                {
                    case "BIG":
                        sql = $@"SELECT  D.IMEI IMEI1,E.IMEI2,E.MEID,E.NETCODE,D.WORKORDER
                                 FROM WMS_ORDERS  A LEFT JOIN WMS_SSCCINFO B ON A.V_DN = B.DN1
                                 LEFT JOIN ODM_BIGCARTONPACKING C ON C.BIGCARTONID = B.BOXSN
                                 LEFT JOIN ODM_PACKING D ON D.MIDCARTONID = C.MIDCARTONID
                                 LEFT JOIN ODM_LINKLISTOFPHYSICSNO E ON E.PHYSICSNO=TO_CHAR(D.IMEI)
                                WHERE B.DN1 = '{v_DN}'AND B.SSCC = '{v_SSCC}' ";
                        break;
                    case "MID":
                        sql = $@"SELECT  D.IMEI IMEI1,E.IMEI2,E.MEID,E.NETCODE,D.WORKORDER
                                 FROM WMS_ORDERS  A LEFT JOIN WMS_SSCCINFO B ON A.V_DN = B.DN1 
                                 LEFT JOIN ODM_PACKING D ON D.MIDCARTONID = B.BOXSN
                                 LEFT JOIN ODM_LINKLISTOFPHYSICSNO E ON E.PHYSICSNO=TO_CHAR(D.IMEI)
                                WHERE B.DN1 = '{v_DN}'AND B.SSCC = '{v_SSCC}'";
                        break;
                    default:
                        break;
                }
                sql += "  AND E.NETCODE IS NOT NULL";
                dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
               
                if (dataTable.Rows.Count > 0)
                {
                    Mo = dataTable.Rows[0][4].ToString();
                    NetCount = dataTable.Rows.Count;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        DT_NETCODE h1_EXCELOUT = new DT_NETCODE();
                        h1_EXCELOUT.IMEI1 = item["IMEI1"].ToString();

                        h1_EXCELOUT.IMEI2 = item["IMEI2"].ToString();
                        h1_EXCELOUT.MEID = item["MEID"].ToString();
                        h1_EXCELOUT.NETCODE = item["NETCODE"].ToString(); 
                        LsdT_s.Add(h1_EXCELOUT);
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(Mo))
            {
                Titles = AppHelper.GetNT_IDByMo(Mo);
            }
            string DnTOP = Titles + "_" + NetCount + "_" + Dts;
            string server = DnTOP + "0001";
            if (NetCount.Equals(0))
            {
                dT_RETURN.Success = false;
                dT_RETURN.Msg = "无进网";
                return dT_RETURN;
            }
            else
            {
                sql = $"SELECT '{DnTOP}'||LPAD(NVL(A.SERIALNO,0)+1,4,'0')   FROM SERIALNO_INFO A WHERE A.PREFIX= '{DnTOP}'  AND F_TYPE=10";
                dataTable = OracleHelper.ExecuteDataTable(UserHelp.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    server = dataTable.Rows[0][0].ToString();
                    sql = $"UPDATE SERIALNO_INFO SET SERIALNO=SERIALNO+1 WHERE PREFIX='{DnTOP}' AND F_TYPE=10";
                    OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, sql);
                }
                else
                { 
                    sql = $"INSERT INTO SERIALNO_INFO(PREFIX,SERIALNO,F_TYPE)VALUES('{DnTOP}',1,10)";
                    OracleHelper.ExecuteNonQuery(UserHelp.OracleConnection, sql);
                }
            }
            dT_RETURN.Service = server;
            dT_RETURN.Success = true;
            dT_RETURN.Msg = "共"+NetCount+"PCS";
            dT_RETURN.DT_NETCODE = LsdT_s;
            return dT_RETURN; 
        }

        public Task<DT_RETURN> WMS_ADRESSS(string plan)
        {
            string sql = "";
            throw new NotImplementedException();
        }
    }
}
