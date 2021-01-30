using API_COM.Modelclass;
using API_COM.MyClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_COM.Servise
{
    public interface CNCEInterface
    { 
        Task<H1_MESSAGE> I_receiveDeliveryData(string delivery_id, string batch_no, string batch_line, string prod_code_cust, string actual_inbound_qty, string actual_inbound_date, string cust_purchaseorder, string cust_purchaseline, string totalreceiv_qty, string cname);
        List<DT_SODATA> LISTDT_SODATA(string DN);
        Task<DT_RETURN> CRT_WMS_DNCODE();
        Task<DT_RETURN> CRT_WMS_ORDERS(string v_DN, string t100MO, string v_PO, string v_POLINE, string v_PN, string v_CPN, string v_DESC, string v_PONB, string v_CODE, string v_NAME, string v_DELIVERY_ID, string v_DELIVERY_QUANTITY, string v_LOGISTICSORDER, string v_LOGISTICS, string v_ADDRESS, string v_DELIEVRYDATE, string v_WAREHOUSE_ID, string v_DELIVERY_STOCK, string cRT_USER,string v_SHIPUSER, string v_SHIPADDRESS);
        List<DT_DNDATA> LISTDT_DNDATA(string dN, string cRT_USER);
        Task<DT_RETURN> WMS_CK_CARTONID(string v_DN, string cARTONID,string tYCAR,string v_PN);
        Task<DT_RETURN> WMS_CK_TOTALCARTONID(string v_DN, string dATA, string tYCAR, string V_delivery_quantity,string cRT_USER);
        Task<DT_RETURN> WMS_QACHECK_DATA(string v_DN, string v_delievrydate, string v_date1, string v_date2,string v_dntype, string cRT_USER);
        Task<DT_RETURN> WMS_QAOK_DATA(string v_DN,string v_SSCC,string v_CODE, string cRT_USER);
        IEnumerable<DT_QAOUT_DATA> GetQAOK_DATA(string v_DN, string v_SSCC, string v_CODE,ref string v_msg);
        Task<DT_RETURN> WMS_QANG_DATA(string v_DN, string v_SSCC, string v_CODE, string cRT_USER);
        Task<DT_DNINFOR> WMS_DN_INFOR(string dN);
        Task<DT_RETURN> WMS_QAENDOUT_DATA(string v_DN, string v_delievrydate, string v_date1, string v_date2, string v_dntype, string cRT_USER);
        IEnumerable<H1_EXCELOUT>  OutExportData(string v_DN, string v_SSCC, string v_CODE, ref string v_MSG);
        Task<DT_RETURN> WMS_ADRESSS(string plan);
        Task<DT_RETURN> WMS_PALWEIGHT_INFOR(string pALLET);
        Task<DT_RETURN> WMS_PALWEIGHT_IN(string v_PALLET, string v_GW, string v_NW);
        DT_RETURN  WMS_NETCODE_DATA(string v_DN, string v_SSCC);

    }
}
