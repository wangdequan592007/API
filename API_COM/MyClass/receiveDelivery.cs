using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_COM.Modelclass
{
    public class receiveDelivery
    {
        public string delivery_id { get; set; }//交货单号
        public string logisticsorder { get; set; }//物流单号
        public string logistics { get; set; }//物流商
        public string address { get; set; }//发货地址
        public string delievrydate { get; set; }//发货日期
        public string warehouse_id { get; set; }//客户到货仓库ID
        public string delivery_stock { get; set; }//发货仓库
        public string prod_code_sale { get; set; }//销售编码
        public string prod_code_cust { get; set; }//客户Item
        public string prod_desc_cust { get; set; }//客户产品描述
        public string cust_purchaseorder { get; set; }//客户采购凭证号
        public string cust_purchaseline { get; set; }//客户采购凭证行号
        public int delivery_quantity { get; set; }//发货数量 
        public List<receiveLine> items { get; set; }
    }
    public class receiveLine
    {
        public string ean_upc_code { get; set; }//商品EAN
        public string imei { get; set; }//IMEI
        public string imei2 { get; set; }//IMEI_2
        public string meid { get; set; }//MEID
        public string mac { get; set; }//MAC
        public string product_barcode { get; set; }//通用编码规则的整机SN，必须填写数据
        public string caton_id_hw { get; set; }//中箱ID
        public string pallet_id_hw { get; set; }//栈板ID
        public string weight1 { get; set; }//彩盒重量
        public string weight2 { get; set; }//卡通箱重量
        public string weight3 { get; set; }//栈板重量
        public string udid { get; set; }//udid号
        public string emmc_id { get; set; }//EMMC-ID 
    }
}
