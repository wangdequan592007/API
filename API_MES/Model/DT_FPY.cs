using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Model
{
    public class DT_FPY
    {
        public string ID { get; set; }
        public string IF_SEQ { get; set; }
        public string MOVE_FLAG { get; set; }
        public string MOVE_TIME { get; set; }
        public string FACTORY { get; set; }
        public string Center_Order_Id { get; set; }
        public string Ems_Order_Id { get; set; }
        public string Oper { get; set; }
        public string FROM_TIME { get; set; }
        public string TO_TIME { get; set; }
        public string Station_ID { get; set; }
        public string Plan_Line { get; set; }
        public string Mfg_Line { get; set; }
        public string FIRST_IN_Qty { get; set; }
        public string IN_Qty { get; set; }
        public string GOOD_QTY { get; set; }
        public string SCRAP_QTY { get; set; }
        public string REPAIR_QTY { get; set; }
        public string REPAIR_GOOD_QTY { get; set; }
        public string REPAIR_WORK_QTY { get; set; }
        public string REWORK_QTY { get; set; }
        public string REWORK_GOOD_QTY { get; set; }
        public string REWORK_WORK_QTY { get; set; }
        public string DEFECT_QTY { get; set; }
        public string FIRST_DEFECT_QTY { get; set; }
        public string FIRST_GOOD_QTY { get; set; }
        public string TEST_FAULT_QTY { get; set; }
        public string MAT_MODEL { get; set; }
        public string EMS_MAT_ID { get; set; }
        public string HW_MAT_ID { get; set; }
        public string CREATE_USER_ID { get; set; }
        public string CREATE_TIME { get; set; }
        public string UPDATE_USER_ID { get; set; }
        public string UPDATE_TIME { get; set; }
        public string TEST_FAULT_SN_QTY { get; set; }
        public string Creation_Time { get; set; }
        public string GETFLAG { get; set; }
        public string GETTIME { get; set; }
        public string ACTIONFLAG { get; set; }
        public string S_FACTORY { get; set; }
        public string SEGMENT2 { get; set; }
        public string SEGMENT3 { get; set; }
        public string SEGMENT4 { get; set; }
        public string SEGMENT5 { get; set; }

    }
}
