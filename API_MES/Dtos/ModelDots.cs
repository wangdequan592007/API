using System;
namespace API_MES.Dtos
{
    public class ModelDot
    { 
        public string WORKJOB_CODE { get; set; } 
        public string ITEM_CODE { get; set; } 
        public string CLIENTNAME { get; set; }
        public string CLIENTCODE { get; set; }
        public int? ITEM_NUM { get; set; } 
        public string INPUTDATE { get; set; }
        public string BEGIN_BARCODE { get; set; }
        public string END_BARCODE { get; set; }
        public string WORKORDER { get; set; }
        public string ITEM_TYPE { get; set; }
    }
}
