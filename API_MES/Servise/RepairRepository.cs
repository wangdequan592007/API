using API_MES.Dtos;
using API_MES.Entities;
using API_MES.Helper;
using API_MES.Model;
using EF_ORACLE;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Servise
{
    public class RepairRepository : IRepairRepository
    {
        private readonly MESContext _context;
        //private readonly IPropertyMappingService _propertyMappingService;
        public RepairRepository(MESContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //_propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService)); 
        }
        public async Task<IEnumerable<REPAIDtos>> GetRepairList(string code,string date1, string date2)
        {
            List<REPAIDtos> LsRe = new List<REPAIDtos>();
            string sql = @$"WITH TB AS(
            SELECT '中诺' FACTORY,'' SN1,A.REPAIR_CARD_CODE SN2, A.COMMAND_CODE,A.REASON_OWE REPAIRBUGTYPE, A.SENDPERSON,A.PERSON,A.ERR_DESCRIBE,(CASE WHEN A.DEFECT_TIME IS NOT NULL THEN
             TO_CHAR(A.DEFECT_TIME, 'YYYY-MM-DD HH24:MI:SS')  ELSE WTF_GET_SENDREPAIRTIME1(A.REPAIR_CARD_CODE,0,  TO_CHAR(A.SENDDATE, 'YYYY-MM-DD HH24:MI:SS'),UPPER(A.GONGWEI)) END) SCAN_TIME, 
              UPPER(A.GONGWEI) POSTION_ITEMVERSION,POSTION_ITEMVERSION POSTION_ITEMVERSION1, TO_CHAR(A.SENDDATE, 'YYYY-MM-DD HH24:MI:SS') SENDDATE,A.POSITION_CODE,A.BAD_ITEM_CODE,TO_CHAR(A.REPAIRDATE, 'YYYY-MM-DD HH24:MI:SS') REPAIRDATE, 
                WTF_GET_OUTSECONDTIME(A.REPAIR_CARD_CODE, 0, TO_CHAR(A.REPAIRDATE, 'YYYY-MM-DD HH24:MI:SS')) SECONDTIME,''SNRESULT,A.RES,
                A.LINE_CODE,A.RECEIVE_PERSON
            FROM QA_IPQC_SUBREPAIR A LEFT OUTER JOIN WORK_WORKJOB Y ON A.COMMAND_CODE = Y.WORKJOB_CODE  WHERE
            NOT EXISTS(SELECT X.PUSER FROM ODM_XLUSER X WHERE X.PUSER = A.SENDPERSON)
            AND NOT EXISTS(SELECT USERID || '[' || USERID || ']' USID FROM RS_USER1  WHERE EMAILADDRESS = '1' AND USERID || '[' || USERID || ']' = A.SENDPERSON)  AND Y.CLIENTCODE = '{code}'
            AND SENDDATE BETWEEN TO_DATE('{date1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{date2}','YYYY-MM-DD HH24:MI:SS') 
            AND NOT EXISTS(SELECT NAME FROM QA_BUG_TYPE WHERE QA_BUG_TYPE.NAME = A.ERR_DESCRIBE AND QA_BUG_TYPE.TESTPOSITION = 'N')
            AND NVL(A.REASON_CODE,'X')<> '单板未知缺陷')
            SELECT FACTORY,SN1,SN2,COMMAND_CODE,REPAIRBUGTYPE,SENDPERSON,PERSON,ERR_DESCRIBE,SCAN_TIME,POSTION_ITEMVERSION,SENDDATE,POSITION_CODE,BAD_ITEM_CODE,REPAIRDATE,SECONDTIME,
            (CASE WHEN RES='报废' THEN 'NG'ELSE NVL2(SECONDTIME,'PASS','')END)SNRESULT,LINE_CODE,RECEIVE_PERSON,SYSDATE
            FROM TB";
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable.Rows.Count>0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    REPAIDtos rEPAIDtos = new REPAIDtos
                    {
                        FACTORY = dataTable.Rows[i]["FACTORY"].ToString(),
                        SN1 = dataTable.Rows[i]["SN1"].ToString(),
                        SN2 = dataTable.Rows[i]["SN2"].ToString(),
                        COMMAND_CODE = dataTable.Rows[i]["COMMAND_CODE"].ToString(),
                        REPAIRBUGTYPE = dataTable.Rows[i]["REPAIRBUGTYPE"].ToString(),
                        SENDPERSON = dataTable.Rows[i]["SENDPERSON"].ToString(),
                        PERSON = dataTable.Rows[i]["PERSON"].ToString(),
                        ERR_DESCRIBE = dataTable.Rows[i]["ERR_DESCRIBE"].ToString(),
                        SCAN_TIME = dataTable.Rows[i]["SCAN_TIME"].ToString(),
                        POSTION_ITEMVERSION = dataTable.Rows[i]["POSTION_ITEMVERSION"].ToString(),
                        SENDDATE = dataTable.Rows[i]["SENDDATE"].ToString(),
                        POSITION_CODE = dataTable.Rows[i]["POSITION_CODE"].ToString(),
                        BAD_ITEM_CODE = dataTable.Rows[i]["BAD_ITEM_CODE"].ToString(),
                        REPAIRDATE = dataTable.Rows[i]["REPAIRDATE"].ToString(),
                        SECONDTIME = dataTable.Rows[i]["SECONDTIME"].ToString(),
                        SNRESULT = dataTable.Rows[i]["SNRESULT"].ToString(),
                        LINE_CODE = dataTable.Rows[i]["LINE_CODE"].ToString(),
                        RECEIVE_PERSON = dataTable.Rows[i]["RECEIVE_PERSON"].ToString(),
                        SYSDATE = dataTable.Rows[i]["SYSDATE"].ToString()
                    };
                    LsRe.Add(rEPAIDtos);
                } 
            }
            return LsRe;
            //throw new NotImplementedException();
        }

        public async Task<RTrEPAIDtos> GetRepairListCount(string code, string date1, string date2)
        {
            RTrEPAIDtos rTrEPAIDtos = new RTrEPAIDtos(); 
            List<REPAIDtosAll> LsRe = new List<REPAIDtosAll>();
            rTrEPAIDtos.success = false;
            await Task.Run(() =>
            { 
                string sql = @$"WITH TB AS(
                SELECT '中诺' FACTORY, '' SN1, A.REPAIR_CARD_CODE SN2, A.COMMAND_CODE, A.REASON_OWE REPAIRBUGTYPE, A.SENDPERSON, A.PERSON, A.ERR_DESCRIBE, (CASE WHEN A.DEFECT_TIME IS NOT NULL THEN
                 TO_CHAR(A.DEFECT_TIME, 'YYYY-MM-DD HH24:MI:SS')  ELSE WTF_GET_SENDREPAIRTIME1(A.REPAIR_CARD_CODE, 0, TO_CHAR(A.SENDDATE, 'YYYY-MM-DD HH24:MI:SS'), UPPER(A.GONGWEI)) END) SCAN_TIME, 
                  UPPER(A.GONGWEI) POSTION_ITEMVERSION,POSTION_ITEMVERSION POSTION_ITEMVERSION1, TO_CHAR(A.SENDDATE, 'YYYY-MM-DD HH24:MI:SS') SENDDATE,A.POSITION_CODE,A.BAD_ITEM_CODE,TO_CHAR(A.REPAIRDATE, 'YYYY-MM-DD HH24:MI:SS') REPAIRDATE, 
                    WTF_GET_OUTSECONDTIME(A.REPAIR_CARD_CODE, 0, TO_CHAR(A.REPAIRDATE, 'YYYY-MM-DD HH24:MI:SS')) SECONDTIME,''SNRESULT,A.RES,
                    A.LINE_CODE,A.RECEIVE_PERSON
                FROM QA_IPQC_SUBREPAIR A    LEFT OUTER JOIN WORK_WORKJOB Y ON A.COMMAND_CODE = Y.WORKJOB_CODE  WHERE
                  NOT EXISTS(SELECT X.PUSER FROM ODM_XLUSER X WHERE X.PUSER = A.SENDPERSON)
                AND NOT EXISTS(SELECT USERID || '[' || USERID || ']' USID FROM RS_USER1  WHERE EMAILADDRESS = '1' AND USERID || '[' || USERID || ']' = A.SENDPERSON)  AND Y.CLIENTCODE = '{code}'
                AND SENDDATE BETWEEN TO_DATE('{date1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{date2}','YYYY-MM-DD HH24:MI:SS') 
                AND NOT EXISTS(SELECT NAME FROM QA_BUG_TYPE WHERE QA_BUG_TYPE.NAME = A.ERR_DESCRIBE AND QA_BUG_TYPE.TESTPOSITION = 'N')
                AND NVL(A.REASON_CODE,'X')<> '单板未知缺陷')
                --SELECT FROM TB
                --SELECT FACTORY,SN1,SN2,COMMAND_CODE,REPAIRBUGTYPE,SENDPERSON,PERSON,ERR_DESCRIBE,SCAN_TIME,POSTION_ITEMVERSION,SENDDATE,POSITION_CODE,BAD_ITEM_CODE,REPAIRDATE,SECONDTIME,SNRESULT,DT1,DT2,DT3,WTF_GET_OUTDT(DT3, ISRUNNING)ISRUNTEN,ISRUNNING FROM(
                 SELECT FACTORY, SN1, SN2, COMMAND_CODE, REPAIRBUGTYPE, SENDPERSON, PERSON, ERR_DESCRIBE, SCAN_TIME, POSTION_ITEMVERSION, SENDDATE, POSITION_CODE, BAD_ITEM_CODE, REPAIRDATE, SECONDTIME,
                 (CASE WHEN RES = '报废' THEN 'NG'ELSE NVL2(SECONDTIME,'PASS','')END)SNRESULT,CEIL((TO_DATE(SENDDATE, 'YYYY-MM-DD HH24:MI:SS') - TO_DATE(SCAN_TIME, 'YYYY-MM-DD HH24:MI:SS')) * 24)DT1,
                CEIL((NVL(TO_DATE(REPAIRDATE, 'YYYY-MM-DD HH24:MI:SS'), SYSDATE) - TO_DATE(SENDDATE, 'YYYY-MM-DD HH24:MI:SS')) * 24)DT2,WTF_GET_LAOHUATIME(COMMAND_CODE, POSTION_ITEMVERSION1, REPAIRDATE, SECONDTIME) ISRUNTEN
                          --(CASE WHEN REPAIRDATE IS NOT NULL THEN  TO_CHAR(CEIL((NVL(TO_DATE(SECONDTIME, 'YYYY-MM-DD HH24:MI:SS'), SYSDATE) - TO_DATE(REPAIRDATE, 'YYYY-MM-DD HH24:MI:SS')) * 24)) ELSE '' END)DT3,
                --(WTF_GET_LAOHUA(COMMAND_CODE, POSTION_ITEMVERSION1))ISRUNNING
                  FROM TB--)";
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    rTrEPAIDtos.success = true;
                    rTrEPAIDtos.AllCount = dataTable.Rows.Count;
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        REPAIDtosAll rEPAIDtos = new REPAIDtosAll
                        {
                            FACTORY = dataTable.Rows[i]["FACTORY"].ToString(),
                            SN1 = dataTable.Rows[i]["SN1"].ToString(),
                            SN2 = dataTable.Rows[i]["SN2"].ToString(),
                            COMMAND_CODE = dataTable.Rows[i]["COMMAND_CODE"].ToString(),
                            REPAIRBUGTYPE = dataTable.Rows[i]["REPAIRBUGTYPE"].ToString(),
                            SENDPERSON = dataTable.Rows[i]["SENDPERSON"].ToString(),
                            PERSON = dataTable.Rows[i]["PERSON"].ToString(),
                            ERR_DESCRIBE = dataTable.Rows[i]["ERR_DESCRIBE"].ToString(),
                            SCAN_TIME = dataTable.Rows[i]["SCAN_TIME"].ToString(),
                            POSTION_ITEMVERSION = dataTable.Rows[i]["POSTION_ITEMVERSION"].ToString(),
                            SENDDATE = dataTable.Rows[i]["SENDDATE"].ToString(),
                            POSITION_CODE = dataTable.Rows[i]["POSITION_CODE"].ToString(),
                            BAD_ITEM_CODE = dataTable.Rows[i]["BAD_ITEM_CODE"].ToString(),
                            REPAIRDATE = dataTable.Rows[i]["REPAIRDATE"].ToString(),
                            SECONDTIME = dataTable.Rows[i]["SECONDTIME"].ToString(),
                            SNRESULT = dataTable.Rows[i]["SNRESULT"].ToString(),
                            DT1 = dataTable.Rows[i]["DT1"].ToString(),
                            DT2 = dataTable.Rows[i]["DT2"].ToString(),
                            ISRUNTEN = dataTable.Rows[i]["ISRUNTEN"].ToString()
                        };
                        LsRe.Add(rEPAIDtos);
                    }
                }
                if (rTrEPAIDtos.success)
                {
                    List<REPAIDtosAll> LsRe1 = LsRe.Where(x => (x.DT1 == "" ? 0 : Convert.ToInt32(x.DT1)) > 4).ToList();
                    List<REPAIDtosAll> LsRe2 = LsRe.Where(x => (x.DT2 == "" ? 0 : Convert.ToInt32(x.DT1)) > 4).ToList();
                    List<REPAIDtosAll> LsRe3 = LsRe.Where(x => (x.ISRUNTEN == "" ? 3 : Convert.ToInt32(x.ISRUNTEN)) == 0).ToList();
                    List<REPAIDtosAll> LsRe4 = LsRe.Where(x => x.REPAIRDATE != "").ToList();
                    List<REPAIDtosAll> LsRe5 = LsRe.Where(x => x.SNRESULT != "").ToList();

                    rTrEPAIDtos.RECOUNT = LsRe4.Count();
                    rTrEPAIDtos.SEOKCOUNT = LsRe1.Count();
                    rTrEPAIDtos.REOKCOUNT = LsRe2.Count();
                    rTrEPAIDtos.RTOKCOUNT = LsRe3.Count();
                }
            });
            return rTrEPAIDtos;
        }
        public async Task<RTrEPAIDtos> GetRepairListCountByTable(string code, string date1, string date2)
        {
            RTrEPAIDtos rTrEPAIDtos = new RTrEPAIDtos();
            List<REPAIDtosAll> LsRe = new List<REPAIDtosAll>();
            rTrEPAIDtos.success = false;
            await Task.Run(() =>
            {
                string sql = @$" SELECT * FROM ODM_MESREPAIR T WHERE T.CODE='{code}' AND  TO_DATE(SENDDATE,'YYYY-MM-DD HH24:MI:SS') BETWEEN TO_DATE('{date1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{date2}','YYYY-MM-DD HH24:MI:SS') ";

                if (code=="27")
                {
                    sql = sql + " AND NVL(RECEIVE_PERSON,'X')<>'李智' AND NVL(T.PERSON,'X') <>'11311792[李智]'";
                }
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    rTrEPAIDtos.success = true;
                    rTrEPAIDtos.AllCount = dataTable.Rows.Count;
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        REPAIDtosAll rEPAIDtos = new REPAIDtosAll
                        {
                            FACTORY = dataTable.Rows[i]["FACTORY"].ToString(),
                            SN1 = dataTable.Rows[i]["SN1"].ToString(),
                            SN2 = dataTable.Rows[i]["SN2"].ToString(),
                            COMMAND_CODE = dataTable.Rows[i]["COMMAND_CODE"].ToString(),
                            REPAIRBUGTYPE = dataTable.Rows[i]["REPAIRBUGTYPE"].ToString(),
                            SENDPERSON = dataTable.Rows[i]["SENDPERSON"].ToString(),
                            PERSON = dataTable.Rows[i]["PERSON"].ToString(),
                            ERR_DESCRIBE = dataTable.Rows[i]["ERR_DESCRIBE"].ToString(),
                            SCAN_TIME = dataTable.Rows[i]["SCAN_TIME"].ToString(),
                            POSTION_ITEMVERSION = dataTable.Rows[i]["POSTION_ITEMVERSION"].ToString(),
                            SENDDATE = dataTable.Rows[i]["SENDDATE"].ToString(),
                            POSITION_CODE = dataTable.Rows[i]["POSITION_CODE"].ToString(),
                            BAD_ITEM_CODE = dataTable.Rows[i]["BAD_ITEM_CODE"].ToString(),
                            REPAIRDATE = dataTable.Rows[i]["REPAIRDATE"].ToString(),
                            SECONDTIME = dataTable.Rows[i]["SECONDTIME"].ToString(),
                            SNRESULT = dataTable.Rows[i]["SNRESULT"].ToString(),
                            DT1 = dataTable.Rows[i]["DT1"].ToString(),
                            DT2 = dataTable.Rows[i]["DT2"].ToString(),
                            ISRUNTEN = dataTable.Rows[i]["ISRUNTEN"].ToString()
                        };
                        LsRe.Add(rEPAIDtos);
                    }
                }
                if (rTrEPAIDtos.success)
                {
                    List<REPAIDtosAll> LsRe1 = LsRe.Where(x => (x.DT1 == "" ? 0 : Convert.ToInt32(x.DT1)) < 4).ToList();
                    List<REPAIDtosAll> LsRe2 = LsRe.Where(x => (x.DT2 == "" ? 0 : Convert.ToInt32(x.DT2)) < 4).ToList();
                    List<REPAIDtosAll> LsRe3 = LsRe.Where(x => (x.ISRUNTEN == "" ? 3 : Convert.ToInt32(x.ISRUNTEN)) == 0).ToList();
                    List<REPAIDtosAll> LsRe4 = LsRe.Where(x => x.REPAIRDATE != "").ToList();
                    List<REPAIDtosAll> LsRe5 = LsRe.Where(x => x.SNRESULT != "").ToList();

                    rTrEPAIDtos.RECOUNT = LsRe4.Count();
                    rTrEPAIDtos.SEOKCOUNT = LsRe1.Count();
                    rTrEPAIDtos.REOKCOUNT = LsRe2.Count();
                    rTrEPAIDtos.RTOKCOUNT = LsRe3.Count();
                }
            });
            return rTrEPAIDtos;
        }
        public System.Data.DataTable GetRepairListTB(string code, string date1, string date2)
        {
            string sql = @$"WITH TB AS(
            SELECT '中诺' FACTORY,'' SN1,A.REPAIR_CARD_CODE SN2, A.COMMAND_CODE,A.REASON_OWE REPAIRBUGTYPE, A.SENDPERSON,A.PERSON,A.ERR_DESCRIBE,(CASE WHEN A.DEFECT_TIME IS NOT NULL THEN
             TO_CHAR(A.DEFECT_TIME, 'YYYY-MM-DD HH24:MI:SS')  ELSE WTF_GET_SENDREPAIRTIME1(A.REPAIR_CARD_CODE,0,  TO_CHAR(A.SENDDATE, 'YYYY-MM-DD HH24:MI:SS'),UPPER(A.GONGWEI)) END) SCAN_TIME, 
              UPPER(A.GONGWEI) POSTION_ITEMVERSION,POSTION_ITEMVERSION POSTION_ITEMVERSION1, TO_CHAR(A.SENDDATE, 'YYYY-MM-DD HH24:MI:SS') SENDDATE,A.POSITION_CODE,A.BAD_ITEM_CODE,TO_CHAR(A.REPAIRDATE, 'YYYY-MM-DD HH24:MI:SS') REPAIRDATE, 
                WTF_GET_OUTSECONDTIME(A.REPAIR_CARD_CODE, 0, TO_CHAR(A.REPAIRDATE, 'YYYY-MM-DD HH24:MI:SS')) SECONDTIME,''SNRESULT,A.RES,
                A.LINE_CODE,A.RECEIVE_PERSON
            FROM QA_IPQC_SUBREPAIR A LEFT OUTER JOIN WORK_WORKJOB Y ON A.COMMAND_CODE = Y.WORKJOB_CODE  WHERE
            NOT EXISTS(SELECT X.PUSER FROM ODM_XLUSER X WHERE X.PUSER = A.SENDPERSON)
            AND NOT EXISTS(SELECT USERID || '[' || USERID || ']' USID FROM RS_USER1  WHERE EMAILADDRESS = '1' AND USERID || '[' || USERID || ']' = A.SENDPERSON)  AND Y.CLIENTCODE = '{code}'
            AND SENDDATE BETWEEN TO_DATE('{date1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{date2}','YYYY-MM-DD HH24:MI:SS') 
            AND NOT EXISTS(SELECT NAME FROM QA_BUG_TYPE WHERE QA_BUG_TYPE.NAME = A.ERR_DESCRIBE AND QA_BUG_TYPE.TESTPOSITION = 'N')
            AND NVL(A.REASON_CODE,'X')<> '单板未知缺陷')
            SELECT FACTORY,SN1,SN2,COMMAND_CODE,REPAIRBUGTYPE,SENDPERSON,PERSON,ERR_DESCRIBE,SCAN_TIME,POSTION_ITEMVERSION,SENDDATE,POSITION_CODE,BAD_ITEM_CODE,REPAIRDATE,SECONDTIME,
            (CASE WHEN RES='报废' THEN 'NG'ELSE NVL2(SECONDTIME,'PASS','')END)SNRESULT,LINE_CODE,RECEIVE_PERSON,SYSDATE  
            FROM TB";
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable.Rows.Count > 0)
            {
                return dataTable;
            }
            return null;
        }
        /// <summary>
        /// 每天更新
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ErrMessage> G_XMDATEY(string code)
        {
            ErrMessage errMessage = new ErrMessage();
            string sql = $"SELECT SN1 FROM ODM_MESREPAIR T WHERE T.US_DATE=TRUNC(SYSDATE-1) AND CODE='{code}' AND ROWNUM=1";
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable.Rows.Count > 0)
            {
                int Index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, $"DELETE ODM_MESREPAIR T WHERE CODE='{code}' AND T.US_DATE=TRUNC(SYSDATE-1)");
            }
            sql = @"INSERT INTO ODM_MESREPAIR WITH TB AS(SELECT  '27'CODE,'中诺' FACTORY, '' SN1, A.REPAIR_CARD_CODE SN2, A.COMMAND_CODE, A.REASON_OWE REPAIRBUGTYPE, A.SENDPERSON, A.PERSON, A.ERR_DESCRIBE, (CASE WHEN A.DEFECT_TIME IS NOT NULL THEN TO_CHAR(A.DEFECT_TIME, 'YYYY-MM-DD HH24:MI:SS')  
            ELSE WTF_GET_SENDREPAIRTIME1(A.REPAIR_CARD_CODE, 0, TO_CHAR(A.SENDDATE, 'YYYY-MM-DD HH24:MI:SS'), UPPER(A.GONGWEI)) END) SCAN_TIME,UPPER(A.GONGWEI) POSTION_ITEMVERSION,POSTION_ITEMVERSION POSTION_ITEMVERSION1, 
            TO_CHAR(NVL(R.CREAT_DATE,A.SENDDATE), 'YYYY-MM-DD HH24:MI:SS') SENDDATE,A.POSITION_CODE,A.BAD_ITEM_CODE,TO_CHAR(A.REPAIRDATE, 'YYYY-MM-DD HH24:MI:SS') REPAIRDATE,WTF_GET_OUTSECONDTIME(A.REPAIR_CARD_CODE, 0, TO_CHAR(A.REPAIRDATE, 'YYYY-MM-DD HH24:MI:SS')) SECONDTIME,
            ''SNRESULT,A.RES,A.LINE_CODE,A.RECEIVE_PERSON FROM QA_IPQC_SUBREPAIR A    LEFT OUTER JOIN WORK_WORKJOB Y ON A.COMMAND_CODE = Y.WORKJOB_CODE  LEFT OUTER JOIN QA_IPQC_SUBREPAIR_INSTORE R ON R.ROWNUMBER=A.ROWNUMBER   WHERE NOT EXISTS(SELECT X.PUSER FROM ODM_XLUSER X WHERE X.PUSER = A.SENDPERSON)
            AND NOT EXISTS(SELECT USERID || '[' || USERID || ']' USID FROM RS_USER1  WHERE EMAILADDRESS = '1' AND USERID || '[' || USERID || ']' = A.SENDPERSON)  AND Y.CLIENTCODE = '27'
            AND TRUNC(SENDDATE)=TRUNC(SYSDATE-1) AND NOT EXISTS(SELECT NAME FROM QA_BUG_TYPE WHERE QA_BUG_TYPE.NAME = A.ERR_DESCRIBE AND QA_BUG_TYPE.TESTPOSITION = 'N') AND NVL(A.REASON_CODE,'X')<> '单板未知缺陷')
                SELECT CODE,FACTORY, SN1, SN2, COMMAND_CODE, REPAIRBUGTYPE, SENDPERSON, PERSON, ERR_DESCRIBE, SCAN_TIME, POSTION_ITEMVERSION, SENDDATE, POSITION_CODE, BAD_ITEM_CODE, REPAIRDATE, SECONDTIME,
            (CASE WHEN RES = '报废' THEN 'NG'ELSE NVL2(SECONDTIME,'PASS','')END)SNRESULT,CEIL((TO_DATE(SENDDATE, 'YYYY-MM-DD HH24:MI:SS') - TO_DATE(SCAN_TIME, 'YYYY-MM-DD HH24:MI:SS')) * 24)DT1,
                CEIL((NVL(TO_DATE(REPAIRDATE, 'YYYY-MM-DD HH24:MI:SS'), SYSDATE) - TO_DATE(SENDDATE, 'YYYY-MM-DD HH24:MI:SS')) * 24)DT2,WTF_GET_LAOHUATIME(COMMAND_CODE, POSTION_ITEMVERSION1, REPAIRDATE, SECONDTIME) ISRUNTEN,SYSDATE CRT_DATE,TRUNC(SYSDATE-1)US_DATE,LINE_CODE,RECEIVE_PERSON
            FROM TB "; 
            //ConcurrentBag<ExamRecord> list = new ConcurrentBag<ExamRecord>();
            //DataTable dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql); 
            //Parallel.For(0, dt.Rows.Count, i =>
            //{

            //}); 
            await Task.Run(() =>
            {
                int Index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
            });
            errMessage.success = true;
            errMessage.Err = "执行完成";
            return errMessage;
        }
        /// <summary>
        /// 按时间更新
        /// </summary>
        /// <param name="code"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<ErrMessage> G_REPairDATa(string code,string date)
        { 
            ErrMessage errMessage = new ErrMessage();
            string sql = $"SELECT SN1 FROM ODM_MESREPAIR T WHERE T.US_DATE=TO_DATE('{date}','YYYY-MM-DD') AND CODE='{code}' AND ROWNUM=1";
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable.Rows.Count > 0)
            {
                int Index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, $"DELETE ODM_MESREPAIR T WHERE CODE='{code}' AND T.US_DATE=TO_DATE('{date}','YYYY-MM-DD')");
            }
            sql = @$"INSERT INTO ODM_MESREPAIR WITH TB AS(SELECT  '{code}'CODE,'中诺' FACTORY, '' SN1, A.REPAIR_CARD_CODE SN2, A.COMMAND_CODE, A.REASON_OWE REPAIRBUGTYPE, A.SENDPERSON, A.PERSON, A.ERR_DESCRIBE, (CASE WHEN A.DEFECT_TIME IS NOT NULL THEN TO_CHAR(A.DEFECT_TIME, 'YYYY-MM-DD HH24:MI:SS')  
            ELSE WTF_GET_SENDREPAIRTIME1(A.REPAIR_CARD_CODE, 0, TO_CHAR(A.SENDDATE, 'YYYY-MM-DD HH24:MI:SS'), UPPER(A.GONGWEI)) END) SCAN_TIME,UPPER(A.GONGWEI) POSTION_ITEMVERSION,POSTION_ITEMVERSION POSTION_ITEMVERSION1, 
             TO_CHAR(NVL(R.CREAT_DATE,A.SENDDATE), 'YYYY-MM-DD HH24:MI:SS')  SENDDATE,A.POSITION_CODE,A.BAD_ITEM_CODE,TO_CHAR(A.REPAIRDATE, 'YYYY-MM-DD HH24:MI:SS') REPAIRDATE,WTF_GET_OUTSECONDTIME(A.REPAIR_CARD_CODE, 0, TO_CHAR(A.REPAIRDATE, 'YYYY-MM-DD HH24:MI:SS')) SECONDTIME,
            ''SNRESULT,A.RES,A.LINE_CODE,A.RECEIVE_PERSON FROM QA_IPQC_SUBREPAIR A    LEFT OUTER JOIN WORK_WORKJOB Y ON A.COMMAND_CODE = Y.WORKJOB_CODE  LEFT OUTER JOIN QA_IPQC_SUBREPAIR_INSTORE R ON R.ROWNUMBER=A.ROWNUMBER  WHERE NOT EXISTS(SELECT X.PUSER FROM ODM_XLUSER X WHERE X.PUSER = A.SENDPERSON)
            AND NOT EXISTS(SELECT USERID || '[' || USERID || ']' USID FROM RS_USER1  WHERE EMAILADDRESS = '1' AND USERID || '[' || USERID || ']' = A.SENDPERSON)  AND Y.CLIENTCODE = '{code}'
            AND TRUNC(SENDDATE)=(TO_DATE('{date}','YYYY-MM-DD')) AND NOT EXISTS(SELECT NAME FROM QA_BUG_TYPE WHERE QA_BUG_TYPE.NAME = A.ERR_DESCRIBE AND QA_BUG_TYPE.TESTPOSITION = 'N') AND NVL(A.REASON_CODE,'X')<> '单板未知缺陷')
                SELECT CODE,FACTORY, SN1, SN2, COMMAND_CODE, REPAIRBUGTYPE, SENDPERSON, PERSON, ERR_DESCRIBE, SCAN_TIME, POSTION_ITEMVERSION, SENDDATE, POSITION_CODE, BAD_ITEM_CODE, REPAIRDATE, SECONDTIME,
            (CASE WHEN RES = '报废' THEN 'NG'ELSE NVL2(SECONDTIME,'PASS','')END)SNRESULT,CEIL((TO_DATE(SENDDATE, 'YYYY-MM-DD HH24:MI:SS') - TO_DATE(SCAN_TIME, 'YYYY-MM-DD HH24:MI:SS')) * 24)DT1,
                CEIL((NVL(TO_DATE(REPAIRDATE, 'YYYY-MM-DD HH24:MI:SS'), SYSDATE) - TO_DATE(SENDDATE, 'YYYY-MM-DD HH24:MI:SS')) * 24)DT2,WTF_GET_LAOHUATIME(COMMAND_CODE, POSTION_ITEMVERSION1, REPAIRDATE, SECONDTIME) ISRUNTEN,SYSDATE CRT_DATE,TO_DATE('{date}','YYYY-MM-DD')US_DATE,LINE_CODE,RECEIVE_PERSON
            FROM TB ";
            await Task.Run(() =>
            {
                int Index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
            });
            errMessage.success = true;
            errMessage.Err = "执行完成";
            return errMessage;
        }

        public async Task<IEnumerable<REPAIDtos>> GetRepairListByDate(string code, string date1)
        {
            await G_REPairDATa(code, date1);
            List<REPAIDtos> LsRe = new List<REPAIDtos>();
            string sql = $"SELECT * FROM ODM_MESREPAIR T WHERE T.US_DATE=TO_DATE('{date1}','YYYY-MM-DD') AND CODE='{code}' ";
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    REPAIDtos rEPAIDtos = new REPAIDtos
                    {
                        CODE = dataTable.Rows[i]["CODE"].ToString(),
                        FACTORY = dataTable.Rows[i]["FACTORY"].ToString(),
                        SN1 = dataTable.Rows[i]["SN1"].ToString(),
                        SN2 = dataTable.Rows[i]["SN2"].ToString(),
                        COMMAND_CODE = dataTable.Rows[i]["COMMAND_CODE"].ToString(),
                        REPAIRBUGTYPE = dataTable.Rows[i]["REPAIRBUGTYPE"].ToString(),
                        SENDPERSON = dataTable.Rows[i]["SENDPERSON"].ToString(),
                        PERSON = dataTable.Rows[i]["PERSON"].ToString(),
                        ERR_DESCRIBE = dataTable.Rows[i]["ERR_DESCRIBE"].ToString(),
                        SCAN_TIME = dataTable.Rows[i]["SCAN_TIME"].ToString(),
                        POSTION_ITEMVERSION = dataTable.Rows[i]["POSTION_ITEMVERSION"].ToString(),
                        SENDDATE = dataTable.Rows[i]["SENDDATE"].ToString(),
                        POSITION_CODE = dataTable.Rows[i]["POSITION_CODE"].ToString(),
                        BAD_ITEM_CODE = dataTable.Rows[i]["BAD_ITEM_CODE"].ToString(),
                        REPAIRDATE = dataTable.Rows[i]["REPAIRDATE"].ToString(),
                        SECONDTIME = dataTable.Rows[i]["SECONDTIME"].ToString(),
                        SNRESULT = dataTable.Rows[i]["SNRESULT"].ToString(),
                        LINE_CODE = dataTable.Rows[i]["LINE_CODE"].ToString(),
                        RECEIVE_PERSON = dataTable.Rows[i]["RECEIVE_PERSON"].ToString(),
                    };
                    LsRe.Add(rEPAIDtos);
                }
            }
            return LsRe;
        }

        public async Task<IEnumerable<REPAIDtos>> GetRepairListByDateByTable(string code, string date1, string date2)
        { 
            List<REPAIDtos> LsRe = new List<REPAIDtos>(); 
            await Task.Run(() =>
            {
                string sql = @$" SELECT * FROM ODM_MESREPAIR T WHERE T.CODE='{code}' AND  TO_DATE(SENDDATE,'YYYY-MM-DD HH24:MI:SS') BETWEEN TO_DATE('{date1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{date2}','YYYY-MM-DD HH24:MI:SS') ";
                if (code == "27")
                {
                    sql = sql + " AND NVL(RECEIVE_PERSON,'X')<>'李智' AND NVL(T.PERSON,'X') <>'11311792[李智]'";
                }
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                { 
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        REPAIDtos rEPAIDtos = new REPAIDtos
                        {
                            CODE = dataTable.Rows[i]["CODE"].ToString(),
                            FACTORY = dataTable.Rows[i]["FACTORY"].ToString(),
                            SN1 = dataTable.Rows[i]["SN1"].ToString(),
                            SN2 = dataTable.Rows[i]["SN2"].ToString(),
                            COMMAND_CODE = dataTable.Rows[i]["COMMAND_CODE"].ToString(),
                            REPAIRBUGTYPE = dataTable.Rows[i]["REPAIRBUGTYPE"].ToString(),
                            SENDPERSON = dataTable.Rows[i]["SENDPERSON"].ToString(),
                            PERSON = dataTable.Rows[i]["PERSON"].ToString(),
                            ERR_DESCRIBE = dataTable.Rows[i]["ERR_DESCRIBE"].ToString(),
                            SCAN_TIME = dataTable.Rows[i]["SCAN_TIME"].ToString(),
                            POSTION_ITEMVERSION = dataTable.Rows[i]["POSTION_ITEMVERSION"].ToString(),
                            SENDDATE = dataTable.Rows[i]["SENDDATE"].ToString(),
                            POSITION_CODE = dataTable.Rows[i]["POSITION_CODE"].ToString(),
                            BAD_ITEM_CODE = dataTable.Rows[i]["BAD_ITEM_CODE"].ToString(),
                            REPAIRDATE = dataTable.Rows[i]["REPAIRDATE"].ToString(),
                            SECONDTIME = dataTable.Rows[i]["SECONDTIME"].ToString(),
                            SNRESULT = dataTable.Rows[i]["SNRESULT"].ToString(),
                            LINE_CODE = dataTable.Rows[i]["LINE_CODE"].ToString(),
                            RECEIVE_PERSON = dataTable.Rows[i]["RECEIVE_PERSON"].ToString()
                        };
                        LsRe.Add(rEPAIDtos);
                    }
                } 
            });
            return LsRe;
        }

        public async Task<IEnumerable<SenDataList>> GetSenDataListByTable(string code, string date1, string date2)
        {
            List<SenDataList> LsRe = new List<SenDataList>();
            await Task.Run(() =>
            {
                string sql = @$" WITH TS AS( SELECT US_DATE DT,COUNT(*)CT FROM ODM_MESREPAIR T WHERE T.CODE='{code}' AND  TO_DATE(SENDDATE,'YYYY-MM-DD HH24:MI:SS') BETWEEN TO_DATE('{date1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{date2}','YYYY-MM-DD HH24:MI:SS') 
               ";
                if (code == "27")
                {
                    sql = sql + " AND NVL(RECEIVE_PERSON,'X')<>'李智' AND NVL(T.PERSON,'X') <>'11311792[李智]'";
                }
                sql +=   "GROUP BY T.US_DATE ) SELECT * FROM  TS ORDER BY DT";

                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        SenDataList rEPAIDtos = new SenDataList();
                        rEPAIDtos.DT = dataTable.Rows[i]["DT"].ToString();
                        rEPAIDtos.CT = dataTable.Rows[i]["CT"].ToString();
                        LsRe.Add(rEPAIDtos);
                    }
                }
            }); 
            return LsRe;
        }
        public async Task<ListDataList> GetDataListByTable(string code, string date1, string date2)
        {
            ListDataList listDataList = new ListDataList();
            List<SenDataList> LsRe = new List<SenDataList>();
            List<SenDataList> LsRe1 = new List<SenDataList>();
            List<SenDataList> LsRe2 = new List<SenDataList>();
            List<SenDataList> LsRe3 = new List<SenDataList>();
            List<SenDataList> LsRe4 = new List<SenDataList>();
            List<SenDataList> LsRe5 = new List<SenDataList>();
            await Task.Run(() =>
            {
                string sql = @$" WITH TS AS( SELECT TO_CHAR(US_DATE,'YYYY.MM.DD') DT,COUNT(*)CT FROM ODM_MESREPAIR T WHERE T.CODE='{code}' AND  TO_DATE(SENDDATE,'YYYY-MM-DD HH24:MI:SS') BETWEEN TO_DATE('{date1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{date2}','YYYY-MM-DD HH24:MI:SS') 
                    ";
                if (code == "27")
                {
                    sql = sql + " AND NVL(RECEIVE_PERSON,'X')<>'李智' AND NVL(T.PERSON,'X') <>'11311792[李智]'";
                }
                sql +=" GROUP BY T.US_DATE ) SELECT * FROM  TS ORDER BY DT";
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        SenDataList rEPAIDtos = new SenDataList
                        {
                            DT = dataTable.Rows[i]["DT"].ToString(),
                            CT = dataTable.Rows[i]["CT"].ToString()
                        };
                        LsRe.Add(rEPAIDtos);
                    }
                }
                  sql = @$" WITH TS AS( SELECT TO_CHAR(US_DATE,'YYYY.MM.DD') DT,COUNT(*)CT FROM ODM_MESREPAIR T WHERE T.CODE='{code}' AND  TO_DATE(SENDDATE,'YYYY-MM-DD HH24:MI:SS') BETWEEN TO_DATE('{date1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{date2}','YYYY-MM-DD HH24:MI:SS') 
                 AND T.REPAIRDATE IS NOT NULL  ";
                if (code == "27")
                {
                    sql = sql + " AND NVL(RECEIVE_PERSON,'X')<>'李智' AND NVL(T.PERSON,'X') <>'11311792[李智]'";
                }
                sql += "   GROUP BY T.US_DATE ) SELECT * FROM  TS ORDER BY DT";
                  dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        SenDataList rEPAIDtos = new SenDataList
                        {
                            DT = dataTable.Rows[i]["DT"].ToString(),
                            CT = dataTable.Rows[i]["CT"].ToString()
                        };
                        LsRe1.Add(rEPAIDtos);
                    }
                }
                sql = @$" WITH TS AS( SELECT TO_CHAR(US_DATE,'YYYY.MM.DD') DT,COUNT(*)CT FROM ODM_MESREPAIR T WHERE T.CODE='{code}' AND  TO_DATE(SENDDATE,'YYYY-MM-DD HH24:MI:SS') BETWEEN TO_DATE('{date1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{date2}','YYYY-MM-DD HH24:MI:SS') 
                 AND T.ISRUNTEN=0 ";
                if (code == "27")
                {
                    sql = sql + " AND NVL(RECEIVE_PERSON,'X')<>'李智' AND NVL(T.PERSON,'X') <>'11311792[李智]'";
                }
                sql += "  GROUP BY T.US_DATE ) SELECT * FROM  TS ORDER BY DT";
                dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        SenDataList rEPAIDtos = new SenDataList
                        {
                            DT = dataTable.Rows[i]["DT"].ToString(),
                            CT = dataTable.Rows[i]["CT"].ToString()
                        };
                        LsRe2.Add(rEPAIDtos);
                    }
                }
                sql = @$" WITH TS AS(                 
                  SELECT NVL(T.REPAIRBUGTYPE,'待维修')DT FROM ODM_MESREPAIR T WHERE T.CODE = '{code}' AND
                   TO_DATE(SENDDATE, 'YYYY-MM-DD HH24:MI:SS') BETWEEN TO_DATE('{date1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{date2}','YYYY-MM-DD HH24:MI:SS')
                        ";
                if (code == "27")
                {
                    sql = sql + " AND NVL(RECEIVE_PERSON,'X')<>'李智' AND NVL(T.PERSON,'X') <>'11311792[李智]'";
                }
                sql += ") SELECT DT, COUNT(*)CT FROM  TS GROUP BY DT";
                dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        SenDataList rEPAIDtos = new SenDataList
                        {
                            DT = dataTable.Rows[i]["DT"].ToString(),
                            CT = dataTable.Rows[i]["CT"].ToString()
                        };
                        LsRe3.Add(rEPAIDtos);
                    }
                }
                sql = @$" WITH TS AS(                 
                  SELECT POSTION_ITEMVERSION DT FROM ODM_MESREPAIR T WHERE T.CODE = '{code}' AND
                   TO_DATE(SENDDATE, 'YYYY-MM-DD HH24:MI:SS') BETWEEN TO_DATE('{date1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{date2}','YYYY-MM-DD HH24:MI:SS') 
                   ";
                if (code == "27")
                {
                    sql = sql + " AND NVL(RECEIVE_PERSON,'X')<>'李智' AND NVL(T.PERSON,'X') <>'11311792[李智]'";
                }
                sql += " ) SELECT DT, COUNT(*)CT FROM  TS GROUP BY DT";
                dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        SenDataList rEPAIDtos = new SenDataList
                        {
                            DT = dataTable.Rows[i]["DT"].ToString(),
                            CT = dataTable.Rows[i]["CT"].ToString()
                        };
                        LsRe4.Add(rEPAIDtos);
                    }
                }
                sql = @$" WITH TS AS(                 
                  SELECT ERR_DESCRIBE DT FROM ODM_MESREPAIR T WHERE T.CODE = '{code}' AND
                   TO_DATE(SENDDATE, 'YYYY-MM-DD HH24:MI:SS') BETWEEN TO_DATE('{date1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{date2}','YYYY-MM-DD HH24:MI:SS')
                  ";
                if (code == "27")
                {
                    sql = sql + " AND NVL(RECEIVE_PERSON,'X')<>'李智' AND NVL(T.PERSON,'X') <>'11311792[李智]'";
                }
                sql += " )  SELECT *FROM(SELECT *FROM (SELECT DT,COUNT(*)CT FROM  TS GROUP BY DT) ORDER BY CT DESC)WHERE ROWNUM<11";
                dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        SenDataList rEPAIDtos = new SenDataList
                        {
                            DT = dataTable.Rows[i]["DT"].ToString(),
                            CT = dataTable.Rows[i]["CT"].ToString()
                        };
                        LsRe5.Add(rEPAIDtos);
                    }
                }
            }); 
            listDataList.SENNUMBER = LsRe;
            listDataList.RENUMBER = LsRe1;
            listDataList.RTNUMBER = LsRe2;
            listDataList.PIERES = LsRe3;
            listDataList.PIESTATION = LsRe4;
            listDataList.ERR_DESCRIBE = LsRe5;
            return listDataList;
        }

        public async Task<ErrMessage> G_PressureTime(string code)
        {
            ErrMessage errMessage = new ErrMessage();
            errMessage.success = false;
            string sql = $"SELECT DTTIME,NUMCODE FROM ODM_PRESSURETIME T WHERE CLIENCODE='{code}' AND ROWNUM=1";
            await Task.Run(() =>
            {
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    string dT = dataTable.Rows[0][0].ToString();
                    string dN = dataTable.Rows[0][1].ToString();
                    errMessage.success = true;
                    if (dN == "1")
                    {
                        errMessage.Err = "当前客户维护的保压-静置时长" + dT;
                    }
                    else
                    {
                        errMessage.Err = "当前客户维护的静置时长" + dT;
                    }

                }
                else
                {
                    errMessage.Err = "未维护静置时长";
                }
            });
            return errMessage;
        }

        public async Task<ErrMessage> G_PressureWg(string code, string sn,string user)
        {
            ErrMessage errMessage = new ErrMessage();
            errMessage.success = true;
           
            if (string.IsNullOrEmpty(sn))
            {
                errMessage.Err = $"扫描条码为空";
                return errMessage;
            }
            string Barcode = sn.Trim();
            string Insert = string.Empty;
            string dT = string.Empty;
            string sql = $"SELECT DTTIME,NUMCODE,WGSTATION FROM ODM_PRESSURETIME T WHERE CLIENCODE='{code}' AND MODEL=0 AND ROWNUM=1 ";
            await Task.Run(() =>
            {
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    dT = dataTable.Rows[0][0].ToString();
                    string dN = dataTable.Rows[0][1].ToString();
                    string dW = dataTable.Rows[0][2].ToString();
                    sql = $"SELECT TO_CHAR(CREAT_TIME,'YYYY-MM-DD HH24:MI:SS')CREAT_TIME FROM ODM_PRESSURE T WHERE BARCODE='{Barcode}' AND TYPE=5";
                    dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                    if (dataTable.Rows.Count > 0)
                    {
                        errMessage.Err = $"扫描条码{sn}外观已经扫描";
                        errMessage.success = false;
                    }
                    else
                    {
                        string Ct = "";
                        //判断静置是否已经扫描
                        sql = $"SELECT TO_CHAR(CREAT_TIME,'YYYY-MM-DD HH24:MI:SS')CREAT_TIME FROM ODM_PRESSURE T WHERE BARCODE='{Barcode}' AND TYPE=4";
                        dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                        if (dataTable.Rows.Count > 0)
                        {
                            Ct = dataTable.Rows[0][0].ToString();
                        }
                        sql = $"SELECT TO_CHAR(CREAT_TIME,'YYYY-MM-DD HH24:MI:SS')CREAT_TIME FROM ODM_PRESSURE T WHERE LCM='{Barcode}' AND TYPE=4";
                        dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                        if (dataTable.Rows.Count > 0)
                        {
                            Ct = dataTable.Rows[0][0].ToString();
                        }
                        sql = $"SELECT TO_CHAR(CREAT_TIME,'YYYY-MM-DD HH24:MI:SS')CREAT_TIME FROM ODM_PRESSURE T WHERE BARCODE='{Barcode}' AND TYPE=1";
                        dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                        if (dataTable.Rows.Count > 0)
                        { 
                            sql = $"SELECT TO_CHAR(CREAT_TIME,'YYYY-MM-DD HH24:MI:SS')CREAT_TIME FROM ODM_PRESSURE T WHERE   TYPE=4 AND EXISTS(SELECT CREAT_TIME FROM ODM_PRESSURE T1  WHERE LINKSN='{Barcode}' AND TYPE=2 AND T.BARCODE=T1.BARCODE)";
                            dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                            if (dataTable.Rows.Count > 0)
                            {
                                Ct = dataTable.Rows[0][0].ToString();
                            }
                        }
                        sql = $"SELECT BARCODE FROM ODM_PRESSURE T WHERE LINKSN='{Barcode}' AND TYPE=2";
                        if (dataTable.Rows.Count > 0)
                        {
                            Barcode = dataTable.Rows[0][0].ToString();
                        }
                        if (string.IsNullOrEmpty(Ct))
                        {
                            errMessage.Err = $"扫描条码{sn}未进行静置扫描";
                            errMessage.success = false;
                        }
                        else
                        {
                            if (dW != "0")//不去校验超时
                            {
                                switch (dN)
                                {
                                    case "1":
                                        string sql = string.Format(@"WITH TS AS(
                                        SELECT LINKSN,CREAT_TIME,BARCODE FROM ODM_PRESSURE A WHERE (A.LCM='{0}' OR BARCODE='{0}')) 
                                        SELECT TO_CHAR(CREAT_TIME,'YYYY-MM-DD HH24:MI:SS') EE,TO_CHAR((CREAT_TIME+{1}/24),'YYYY-MM-DD HH24:MI:SS') EF FROM(
                                        SELECT MAX(B.CREAT_TIME) CREAT_TIME FROM ODM_PRESSURE B,TS C  WHERE   B.TYPE=3 AND C.LINKSN=B.LINKSN AND B.CREAT_TIME<C.CREAT_TIME 
                                         ) WHERE (SYSDATE- CREAT_TIME)<({1}/24)", Barcode, dT);
                                        //sql = $"SELECT SYSDATE FROM DUAL T WHERE  (SYSDATE-TO_DATE('{Ct}','YYYY-MM-DD HH24:MI:SS'))<({dT}/24)";
                                        dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                                        if (dataTable.Rows.Count > 0)
                                        {
                                            string Dtime = dataTable.Rows[0][0].ToString();
                                            errMessage.Err = $"条码{sn}保压开始时间{Dtime}需要{dT}小时后才静置完成,不能投入外观工序";
                                            errMessage.success = false;
                                        }
                                        else
                                        {
                                            errMessage.Err = $"条码{sn}扫描完成";
                                        }
                                        break;
                                    case "0":
                                        sql = $"SELECT SYSDATE FROM DUAL T WHERE  (SYSDATE-TO_DATE('{Ct}','YYYY-MM-DD HH24:MI:SS'))<({dT}/24)";
                                        dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                                        if (dataTable.Rows.Count > 0)
                                        {
                                            errMessage.Err = $"条码{sn}静置开始时间{Ct}需要{dT}小时后才静置完成,不能投入外观工序";
                                            errMessage.success = false;

                                        }
                                        else
                                        {
                                            errMessage.Err = $"条码{sn}扫描完成";
                                        }  
                                        break;
                                    default:
                                        errMessage.Err = $"条码{sn}扫描完成";
                                        break;
                                }
                               
                            }
                            else
                            {
                                errMessage.Err = $"条码{sn}扫描完成"; 
                            }
                        } 
                    } 
                }
                else
                {
                    errMessage.Err = "请到保压外观检查设置模块维护静置时长";
                    errMessage.success = false;
                }
            });
            if (errMessage.success)
            {
                Insert = $"INSERT INTO ODM_PRESSURE(BARCODE,TYPE,CLIENTCODE,HOUR,CREAT_PERSON)VALUES('{sn}',5,'{code}','{dT}','{user}') ";
                int index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, Insert);
            }
            return errMessage;
        }

        public async Task<ErrMessage> AddPressuretime(string cLIENCODE, string dTTIME, string nUMCODE, string wGSTATION)
        {
            ErrMessage errMessage = new ErrMessage();
            errMessage.success = true;
            errMessage.Err = "维护完成";
            if (!string.IsNullOrEmpty(dTTIME))
            {
                dTTIME = dTTIME.Trim();
            }
            if (string.IsNullOrEmpty(nUMCODE))
            {
                nUMCODE = "0";
            }
            if (string.IsNullOrEmpty(wGSTATION))
            {
                wGSTATION = "0";
            }
            await Task.Run(() => {
                string Insert = string.Format("MERGE INTO ODM_PRESSURETIME USING DUAL ON(CLIENCODE='{0}')WHEN MATCHED THEN UPDATE SET DTTIME='{1}',NUMCODE='{2}',WGSTATION='{3}' WHEN NOT MATCHED THEN INSERT(CLIENCODE,DTTIME,NUMCODE,WGSTATION) VALUES('{0}','{1}','{2}','{3}')", cLIENCODE, dTTIME, nUMCODE, wGSTATION);
                int index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, Insert);
            });
            return errMessage;
        }

        public async Task<ODM_PRESSURETIME> GetODM_PRESSURETIME(string code)
        {
            return await _context.oDM_PRESSURETIMEs.Where(x => x.CLIENCODE == code).FirstOrDefaultAsync();
        } 
        public async Task<ErrMessage> G_HwReDATE(string code)
        {
            ErrMessage errMessage = new ErrMessage();
            string sql = @"SELECT * FROM ( 
                            SELECT '中诺' FACTORY,'' SN1,A.REPAIR_CARD_CODE SN2,A.COMMAND_CODE,A.REASON_OWE REPAIRBUGTYPE,A.SENDPERSON,A.PERSON,A.ERR_DESCRIBE,(CASE WHEN A.DEFECT_TIME IS NOT NULL THEN TO_CHAR(A.DEFECT_TIME, 'YYYY-MM-DD HH24:MI:SS')
                            ELSE WTF_GET_SENDREPAIRTIME1(A.REPAIR_CARD_CODE,B.PTYPE,  TO_CHAR(A.SENDDATE,'YYYY-MM-DD HH24:MI:SS'),UPPER(A.GONGWEI)) END) SCAN_TIME, UPPER(A.GONGWEI) POSTION_ITEMVERSION,TO_CHAR(A.SENDDATE,'YYYY-MM-DD HH24:MI:SS') 
                            SENDDATE,A.POSITION_CODE,A.BAD_ITEM_CODE,TO_CHAR(A.REPAIRDATE,  'YYYY-MM-DD HH24:MI:SS') REPAIRDATE,TO_CHAR(G.TESTDATE,'YYYY-MM-DD HH24:MI:SS') SECONDTIME,UPPER(G.TESTRESULT) SNRESULT,A.LINE_CODE,A.RECEIVE_PERSON
                            FROM QA_IPQC_SUBREPAIR A 
                            LEFT OUTER JOIN ODM_XLUSER B ON A.SENDPERSON=B.PUSER 
                            LEFT OUTER JOIN WORK_WORKJOB Y ON A.COMMAND_CODE=Y.WORKJOB_CODE 
                            LEFT OUTER JOIN ODM_REPAIRTR G ON G.SN=A.REPAIR_CARD_CODE AND  G.TESTDATE>A.REPAIRDATE 
                            WHERE A.SENDPERSON=B.PUSER AND Y.CLIENTCODE='01' AND  ((( NVL(A.RES ,'XX') <>'报废' ) AND (NVL(A.REASON_CODE,'XX') NOT IN('误判','误测'))))  AND NVL(PERSON,'XX')<>'11409044[王甫令]' 
                            AND B.PTYPE='1'  AND  TRUNC(SENDDATE)=TRUNC(SYSDATE-1) AND A.GONGWEI<>'外观' AND  NOT EXISTS(SELECT NAME FROM QA_BUG_TYPE WHERE QA_BUG_TYPE.NAME=A.ERR_DESCRIBE AND QA_BUG_TYPE.TESTPOSITION='N') 
                            ) K 
                            WHERE  K.SNRESULT='PASS' AND NVL(UPPER(K.POSITION_CODE),'XX')<>'NDF' AND K.SECONDTIME=(SELECT MIN(TO_CHAR(H.TESTDATE,'YYYY-MM-DD HH24:MI:SS')) FROM ODM_REPAIRTR H WHERE H.SN=K.SN2  AND UPPER(H.TESTRESULT)='PASS'
                            AND TO_CHAR(H.TESTDATE,'YYYY-MM-DD HH24:MI:SS')>K.REPAIRDATE) ";
            sql += @" UNION SELECT * FROM (SELECT '中诺' FACTORY,'' SN1,A.REPAIR_CARD_CODE SN2,A.COMMAND_CODE,A.REASON_OWE REPAIRBUGTYPE,A.SENDPERSON,A.PERSON,A.ERR_DESCRIBE,(CASE WHEN A.DEFECT_TIME IS NOT NULL THEN TO_CHAR(A.DEFECT_TIME, 'YYYY-MM-DD HH24:MI:SS') 
                     ELSE WTF_GET_SENDREPAIRTIME1(A.REPAIR_CARD_CODE,B.PTYPE,  TO_CHAR(A.SENDDATE,'YYYY-MM-DD HH24:MI:SS'),UPPER(A.GONGWEI)) END) SCAN_TIME,UPPER(A.GONGWEI) POSTION_ITEMVERSION,
                     TO_CHAR(A.SENDDATE,'YYYY-MM-DD HH24:MI:SS') SENDDATE,A.POSITION_CODE,A.BAD_ITEM_CODE,TO_CHAR(A.REPAIRDATE,  'YYYY-MM-DD HH24:MI:SS') REPAIRDATE,TO_CHAR(G.TESTDATE,'YYYY-MM-DD HH24:MI:SS')
                     SECONDTIME,UPPER(G.TESTRESULT) SNRESULT,A.LINE_CODE,A.RECEIVE_PERSON  FROM QA_IPQC_SUBREPAIR A LEFT OUTER JOIN ODM_XLUSER B ON A.SENDPERSON=B.PUSER
                     LEFT OUTER JOIN WORK_WORKJOB Y ON A.COMMAND_CODE=Y.WORKJOB_CODE  LEFT OUTER JOIN ODM_REPAIRTR G ON G.SN=A.REPAIR_CARD_CODE AND  G.TESTDATE>A.REPAIRDATE WHERE
                     A.SENDPERSON=B.PUSER AND Y.CLIENTCODE='01'  AND  ((( NVL(A.RES ,'XX') <>'报废' ) AND  (NVL(A.REASON_CODE,'XX') NOT IN('误判','误测')))) AND NVL(PERSON,'XX')<>'11409044[王甫令]'  AND B.PTYPE='1' 
                     AND  TRUNC(SENDDATE)=TRUNC(SYSDATE-1) AND A.GONGWEI<>'外观'  AND  NOT EXISTS(SELECT NAME FROM QA_BUG_TYPE WHERE QA_BUG_TYPE.NAME=A.ERR_DESCRIBE AND QA_BUG_TYPE.TESTPOSITION='N') ) K 
                     WHERE  NVL(UPPER(K.POSITION_CODE),'XX')<>'NDF' ";
            //组装送修
            sql += @" UNION SELECT '中诺' FACTORY,'' SN1,A.REPAIR_CARD_CODE SN2,A.COMMAND_CODE,A.REASON_OWE REPAIRBUGTYPE,A.SENDPERSON,A.PERSON,A.ERR_DESCRIBE,(CASE WHEN A.DEFECT_TIME IS NOT NULL THEN TO_CHAR(A.DEFECT_TIME, 'YYYY-MM-DD HH24:MI:SS')  
                    ELSE WTF_GET_SENDREPAIRTIME1(A.REPAIR_CARD_CODE,B.PTYPE,  TO_CHAR(A.SENDDATE,'YYYY-MM-DD HH24:MI:SS'),UPPER(A.GONGWEI)) END) SCAN_TIME,UPPER(A.GONGWEI) POSTION_ITEMVERSION,
                    TO_CHAR(A.SENDDATE,'YYYY-MM-DD HH24:MI:SS') SENDDATE,A.POSITION_CODE,A.BAD_ITEM_CODE,TO_CHAR(A.REPAIRDATE,  'YYYY-MM-DD HH24:MI:SS') REPAIRDATE, WTF_GET_OUTREPAIRTIME(A.REPAIR_CARD_CODE,B.PTYPE,
                    TO_CHAR(A.REPAIRDATE,'YYYY-MM-DD HH24:MI:SS')) SECONDTIME,NVL2(WTF_GET_OUTREPAIRTIME(A.REPAIR_CARD_CODE,B.PTYPE,TO_CHAR(A.REPAIRDATE,'YYYY-MM-DD HH24:MI:SS')),'PASS','')  SNRESULT,
                    A.LINE_CODE,A.RECEIVE_PERSON  
                    FROM QA_IPQC_SUBREPAIR A 
                    LEFT OUTER JOIN ODM_XLUSER B ON A.SENDPERSON=B.PUSER 
                     LEFT OUTER JOIN WORK_WORKJOB Y ON A.COMMAND_CODE=Y.WORKJOB_CODE  
                     WHERE A.SENDPERSON=B.PUSER  AND B.PTYPE='0'  AND Y.CLIENTCODE='01' AND ((( NVL(A.RES ,'XX') <>'报废' ) AND  
                     (NVL(A.REASON_CODE,'XX') NOT IN('误判','误测'))))  AND NVL(PERSON,'XX')<>'11409044[王甫令]'  
                     AND   TRUNC(SENDDATE)=TRUNC(SYSDATE-1) AND A.GONGWEI<>'外观' 
                     AND  NOT EXISTS(SELECT NAME FROM QA_BUG_TYPE WHERE QA_BUG_TYPE.NAME=A.ERR_DESCRIBE AND QA_BUG_TYPE.TESTPOSITION='N') AND NVL(UPPER(A.POSITION_CODE),'XX')<>'NDF' ";
            //外观维修
            sql += @" UNION SELECT '中诺' FACTORY,'' SN1,A.REPAIR_CARD_CODE SN2,A.COMMAND_CODE,A.REASON_OWE REPAIRBUGTYPE,A.SENDPERSON,A.PERSON,A.ERR_DESCRIBE,(CASE WHEN A.DEFECT_TIME IS NOT NULL THEN TO_CHAR(A.DEFECT_TIME, 'YYYY-MM-DD HH24:MI:SS')  
                     ELSE WTF_GET_SENDREPAIRTIME1(A.REPAIR_CARD_CODE,B.PTYPE,  TO_CHAR(A.SENDDATE,'YYYY-MM-DD HH24:MI:SS'),UPPER(A.GONGWEI)) END ) SCAN_TIME,UPPER(A.GONGWEI) POSTION_ITEMVERSION,TO_CHAR(A.SENDDATE,'YYYY-MM-DD HH24:MI:SS') 
                     SENDDATE,A.POSITION_CODE,A.BAD_ITEM_CODE,TO_CHAR(A.REPAIRDATE,  'YYYY-MM-DD HH24:MI:SS') REPAIRDATE,TO_CHAR(A.REPAIRDATE,  'YYYY-MM-DD HH24:MI:SS') SECONDTIME,NVL2(TO_CHAR(A.REPAIRDATE,  'YYYY-MM-DD HH24:MI:SS'),'PASS','') 
                     SNRESULT,A.LINE_CODE,A.RECEIVE_PERSON  
                     FROM QA_IPQC_SUBREPAIR A 
                     LEFT OUTER JOIN ODM_XLUSER B ON A.SENDPERSON=B.PUSER  
                     LEFT OUTER JOIN WORK_WORKJOB Y ON A.COMMAND_CODE=Y.WORKJOB_CODE  
                     LEFT OUTER JOIN ODM_REPAIRTR G ON G.SN=A.REPAIR_CARD_CODE AND  G.TESTDATE>A.REPAIRDATE 
                     WHERE A.SENDPERSON=B.PUSER AND Y.CLIENTCODE='01' AND  ((( NVL(A.RES ,'XX') <>'报废' ) 
                     AND  (NVL(A.REASON_CODE,'XX') NOT IN('误判','误测'))))  AND NVL(PERSON,'XX')<>'11409044[王甫令]'  AND B.PTYPE='2'  
                     AND    TRUNC(SENDDATE)=TRUNC(SYSDATE-1)  AND A.GONGWEI<>'外观' 
                     AND  NOT EXISTS(SELECT NAME FROM QA_BUG_TYPE WHERE QA_BUG_TYPE.NAME=A.ERR_DESCRIBE AND QA_BUG_TYPE.TESTPOSITION='N') AND NVL(UPPER(A.POSITION_CODE),'XX')<>'NDF' ";
            sql += @" UNION  SELECT '中诺' FACTORY,'' SN1,A.REPAIR_CARD_CODE SN2,A.COMMAND_CODE,A.REASON_OWE REPAIRBUGTYPE,A.SENDPERSON,A.PERSON,A.ERR_DESCRIBE,(CASE WHEN A.DEFECT_TIME IS NOT NULL THEN  
                  TO_CHAR(A.DEFECT_TIME, 'YYYY-MM-DD HH24:MI:SS')  ELSE WTF_GET_SENDREPAIRTIME1(A.REPAIR_CARD_CODE,0,  TO_CHAR(A.SENDDATE,'YYYY-MM-DD HH24:MI:SS'),UPPER(A.GONGWEI)) END) SCAN_TIME, 
                   UPPER(A.GONGWEI) POSTION_ITEMVERSION,TO_CHAR(A.SENDDATE,'YYYY-MM-DD HH24:MI:SS') SENDDATE,A.POSITION_CODE,A.BAD_ITEM_CODE,TO_CHAR(A.REPAIRDATE,  'YYYY-MM-DD HH24:MI:SS') REPAIRDATE, 
                     WTF_GET_OUTREPAIRTIME(A.REPAIR_CARD_CODE,0,TO_CHAR(A.REPAIRDATE,'YYYY-MM-DD HH24:MI:SS')) SECONDTIME,NVL2(WTF_GET_OUTREPAIRTIME(A.REPAIR_CARD_CODE,0,  TO_CHAR(A.REPAIRDATE,'YYYY-MM-DD HH24:MI:SS')),'PASS','')
                       SNRESULT,A.LINE_CODE,A.RECEIVE_PERSON  
                       FROM QA_IPQC_SUBREPAIR A    
                       LEFT OUTER JOIN WORK_WORKJOB Y ON A.COMMAND_CODE=Y.WORKJOB_CODE  WHERE  NOT EXISTS(SELECT X.PUSER FROM ODM_XLUSER X WHERE X.PUSER=A.SENDPERSON)  
                       AND NOT EXISTS( SELECT USERID||'['||USERID ||']' USID FROM RS_USER1  WHERE EMAILADDRESS='1' AND USERID||'['||USERID ||']'=A.SENDPERSON )  AND Y.CLIENTCODE='01' 
                       AND ((( NVL(A.RES ,'XX') <>'报废' ) AND  (NVL(A.REASON_CODE,'XX') NOT IN('误判','误测'))))  AND NVL(PERSON,'XX')<>'11409044[王甫令]'    
                       AND  TRUNC(SENDDATE)=TRUNC(SYSDATE-1) AND A.GONGWEI<>'外观'
                       AND    NOT EXISTS(SELECT NAME FROM QA_BUG_TYPE WHERE QA_BUG_TYPE.NAME=A.ERR_DESCRIBE AND QA_BUG_TYPE.TESTPOSITION='N') AND NVL(UPPER(A.POSITION_CODE),'XX')<>'NDF'   ";
            sql += @" UNION   SELECT '中诺' FACTORY,'' SN1,A.REPAIR_CARD_CODE SN2,A.COMMAND_CODE,A.REASON_OWE REPAIRBUGTYPE,A.SENDPERSON,A.PERSON,A.ERR_DESCRIBE,(CASE WHEN A.DEFECT_TIME IS NOT NULL THEN   TO_CHAR(A.DEFECT_TIME, 'YYYY-MM-DD HH24:MI:SS')  
                   ELSE WTF_GET_SENDREPAIRTIME1(A.REPAIR_CARD_CODE,0,  TO_CHAR(A.SENDDATE,'YYYY-MM-DD HH24:MI:SS'),UPPER(A.GONGWEI)) END) SCAN_TIME,   UPPER(A.GONGWEI) POSTION_ITEMVERSION,TO_CHAR(A.SENDDATE,'YYYY-MM-DD HH24:MI:SS') SENDDATE
                  ,A.POSITION_CODE,A.BAD_ITEM_CODE,TO_CHAR(A.REPAIRDATE,  'YYYY-MM-DD HH24:MI:SS') REPAIRDATE,     WTF_GET_OUTREPAIRTIME(A.REPAIR_CARD_CODE,0,TO_CHAR(A.REPAIRDATE,'YYYY-MM-DD HH24:MI:SS')) SECONDTIME,
                  NVL2(WTF_GET_OUTREPAIRTIME(A.REPAIR_CARD_CODE,1, TO_CHAR(A.REPAIRDATE,'YYYY-MM-DD HH24:MI:SS')),'PASS','')  SNRESULT,A.LINE_CODE,A.RECEIVE_PERSON  
                  FROM QA_IPQC_SUBREPAIR A LEFT OUTER JOIN WORK_WORKJOB Y ON A.COMMAND_CODE=Y.WORKJOB_CODE    
                  WHERE ((( NVL(A.RES ,'XX') <>'报废' ) AND  (NVL(A.REASON_CODE,'XX') NOT IN('误判','误测'))))  AND NVL(PERSON,'XX')<>'11409044[王甫令]'    
                  AND TRUNC(SENDDATE)=TRUNC(SYSDATE-1) AND A.GONGWEI<>'外观' AND NOT EXISTS(SELECT NAME FROM QA_BUG_TYPE WHERE QA_BUG_TYPE.NAME=A.ERR_DESCRIBE AND QA_BUG_TYPE.TESTPOSITION='N' )
                  AND NVL(UPPER(A.POSITION_CODE),'XX')<>'NDF' AND A.ITEM_DESCRIBE='IMS'";
            string sql2 = @$" SELECT '01'CODE,FACTORY,SN1,SN2,COMMAND_CODE,REPAIRBUGTYPE,SENDPERSON,PERSON,ERR_DESCRIBE,SCAN_TIME,POSTION_ITEMVERSION,SENDDATE,POSITION_CODE,BAD_ITEM_CODE,
                              REPAIRDATE,SECONDTIME,SNRESULT,''DT1,''DT2,''ISRUNTEN,SYSDATE CRT_DATE, TRUNC(SYSDATE - 1)US_DATE,LINE_CODE,RECEIVE_PERSON FROM ({sql})";
            string sql1 = @"SELECT   '01'CODE,'中诺' FACTORY,''SN1, T7.LB_ID SN2, T7.MO COMMAND_CODE,(SELECT F.DUTY_DESCRIBE_NAME FROM TB_ENOK_DUTY_DESCRIBE@DLIMS F WHERE F.DUTY_DESCRIBE_ID=T8.DUTY_DESCRIBE_ID)REPAIRBUGTYPE ,'IMS_SYSTEM'SENFPERSON,T7.SYS_CRT_USER||'['||T10.USER_NAME||']' PERSON,
                         T8.BAD_CAUSE  ERR_DESCRIBE,NVL(FUN_GETSCAN_TIME@DLIMS( T7.LB_ID,T8.SEND_DATE), TO_CHAR(T8.SEND_DATE,'YYYY-MM-DD HH24:MI:SS'))   SCAN_TIME,T8.TB_PM_RP_DT_EX5 POSTION_ITEMVERSION, TO_CHAR(T8.SEND_DATE,'YYYY-MM-DD HH24:MI:SS')  SENDDATE,T8.BAD_POINT POSITION_CODE,T8.MTRLID_OLD BAD_ITEM_CODE
                         ,TO_CHAR(T7.SYS_CRT_DATE,'YYYY-MM-DD HH24:MI:SS') REPAIRDATE, ( SELECT MIN(TO_CHAR(T5.SYS_CRT_DATE,'YYYY-MM-DD HH24:MI:SS')) FROM TB_PM_MO_LBWP@DLIMS T5 WHERE T5.SYS_CRT_DATE>T7.SYS_CRT_DATE AND T5.LB_ID=T7.LB_ID ) SECONDTIME,DECODE((SELECT T6.IS_PASS FROM TB_PM_MO_LBWP@DLIMS T6 
                         WHERE T6.SYS_CRT_DATE=(SELECT MIN(T5.SYS_CRT_DATE) FROM TB_PM_MO_LBWP@DLIMS T5 WHERE T5.SYS_CRT_DATE>T7.SYS_CRT_DATE AND T5.LB_ID=T7.LB_ID) AND T6.LB_ID=T7.LB_ID ),'Y','PASS','N','FAIL',NULL,NULL ) SNRESULT 
                         ,''DT1,''DT2,''ISRUNTEN,SYSDATE CRT_DATE,TRUNC(SYSDATE-1)US_DATE, T7.PL_ID LINE_CODE,''RECEIVE_PERSON FROM TB_PM_RP_HD@DLIMS T7  LEFT OUTER JOIN TB_PM_RP_DT@DLIMS T8 ON T7.RP_ID=T8.RP_ID  LEFT OUTER JOIN TB_BS_BAD_TYPE@DLIMS T9 ON T8.BAD_TYPE_ID=T9.BAD_TYPE_ID  LEFT OUTER JOIN TB_SYS_USER@DLIMS T10 ON T10.USER_ID=T7.SYS_CRT_USER 
                         LEFT OUTER JOIN TB_PP_MO@DLIMS T11 ON T11.MO= T7.MO WHERE 
                            TRUNC(T7.SYS_CRT_DATE )=TRUNC(SYSDATE-1)
                           AND T8.PR_RESULT<>'报废' AND T8.PR_RESULT<>'重新测试'  AND T8.RP_SEQ=1 AND T7.SYS_CRT_USER IN('11408734','0910633','11311792')
                         AND  T8.TB_PM_RP_DT_EX5 NOT IN('SPI','AOI','AOI1','AOI2') AND T11.CUST_ID IN('20.001.0002','20.001.0001','20.001.0005','2200001','2200002','2200003')";
            string insert = $"INSERT INTO ODM_MESREPAIR ({sql1})";
            await Task.Run(() =>
            {
                int Index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, $"INSERT INTO ODM_MESREPAIR ({sql2})");
                Index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, $"INSERT INTO ODM_MESREPAIR ({sql1})");
            });
            errMessage.success = true;
            errMessage.Err = "执行完成";
            return errMessage;
        }
        public async Task<ErrMessage> Hw_RepairIms(string date1)
        {
            ErrMessage errMessage = new ErrMessage();
            //半小时一次--
            string Date1 = string.Empty;
            string Date2 = string.Empty;
            string sql= @"WITH TS AS(SELECT(TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24') || ':00:00')DATE1, (TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24') || ':29:59')DATE2,TO_NUMBER(TO_CHAR(SYSDATE, 'MI'))NB1  FROM DUAL 
                          )SELECT* FROM TS WHERE NB1 >= 30";
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable.Rows.Count>0)
            {
                Date1 = dataTable.Rows[0][0].ToString();
                Date2 = dataTable.Rows[0][1].ToString();
            }
            else
            {
                sql = @"WITH TS AS(SELECT (TO_CHAR(SYSDATE-(1/24),'YYYY-MM-DD HH24')||':30:00' )DATE1, (TO_CHAR(SYSDATE-(1/24),'YYYY-MM-DD HH24')||':59:59' )DATE2,TO_NUMBER(TO_CHAR(SYSDATE, 'MI'))NB1 FROM DUAL)
                    SELECT* FROM TS WHERE NB1 < 30";
                dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    Date1 = dataTable.Rows[0][0].ToString();
                    Date2 = dataTable.Rows[0][1].ToString();
                }
            }
            if (!string.IsNullOrEmpty(Date1))
            {
                string strselect = string.Format(@"SELECT ''ID,''IF_SEQ,''MOVE_FLAG,''MOVE_TIME,'CHINO-E'FACTORY,A.LB_ID LOT_ID,'1'REP_SEQ,A.MO EMS_ORDER_ID,
                                                COALESCE(GET_T100MODEL@MES(A.PROD_ID,1),GET_MODEL(A.PROD_ID),(SELECT PROD_MODEL FROM TB_BS_MTRL A WHERE A.MTRL_ID=A.PROD_ID))MAT_MODEL
                                                ,A.PROD_ID MAT_ID,A.PROD_ID HW_MAT_ID,B.TB_PM_RP_DT_EX5 REQ_OPER,(SELECT A2.BIG_CLASS_NAME FROM TB_BS_BAD_ITEM A1 LEFT OUTER JOIN TB_ENOK_BAD_BIG_CLASS A2 ON  A1.BIG_CLASS_ID=A2.BIG_CLASS_ID WHERE A1.BAD_ITEM_ID=B.FIELD_EX3)DEFECT_REA_TYPE,B.FIELD_EX3 DEFECT_REA_CODE,
                                                A.SYS_CRT_USER  REQ_USER,C.REMARK REQ_COMMENT,RP_USER REP_USER,DECODE(A.SYS_CRT_USER,'11408734','R','0910633','R','L')REP_TYPE,''REPAIR_COMMENT,DECODE(A.RP_RS,'2','F','R') REPAIR_STATUS,(CASE WHEN A.RP_RS=2 THEN '报废' ELSE SUBSTR(A.RP_NEXT_WPID,1,INSTR(A.RP_NEXT_WPID,'_',-1)-1) END)RETURN_OPER, TO_CHAR(A.SYS_CRT_DATE,'YYYYMMDDHH24MISS') LAST_TRAN_TIME,
                                                B.RP_USER LAST_TRAN_USER,C.SYS_CRT_USER CREATE_USER_ID, TO_CHAR(B.SEND_DATE,'YYYYMMDDHH24MISS') CREATE_TIME,(CASE WHEN H.CODE IS NULL THEN (DECODE( E.BAD_TYPE_ID,'-',D.TB_PM_QC_DT_EX4,NVL(E.BAD_TYPE_ID,D.TB_PM_QC_DT_EX4)))   ELSE H.CODE END ) DEFECT_CODE
                                                ,(CASE WHEN H.NAME IS NULL THEN (DECODE( E.BAD_TYPE_ID,'-',D.TB_PM_QC_DT_EX1,NVL(E.BAD_TYPE_NAME,D.TB_PM_QC_DT_EX1)))   ELSE  H.NAME END )DEFECT_DESC
                                                ,(SELECT F.DUTY_DESCRIBE_NAME FROM TB_ENOK_DUTY_DESCRIBE F WHERE F.DUTY_DESCRIBE_ID=B.DUTY_DESCRIBE_ID)DEFECT_TYPE_CODE,A.MO CENTER_ORDER_ID,B.PR_RESULT REPAIR_CODE_1,B.BAD_POINT  DEFECT_POSITION_1,  
                                                (CASE WHEN A.RP_RS=2 THEN A.PROD_ID ELSE  NVL(B.MTRLID_OLD,B.RP_MTRLID) END) DEFECT_RAW_MAT_ID_1, (CASE WHEN A.RP_RS=2 THEN A.LB_ID ELSE  NVL(B.RID_OLD,B.RP_RID) END)DEFECT_RAW_LOT_ID_1,
                                                (CASE WHEN A.RP_RS=2 THEN A.LB_ID ELSE (NVL(J1.VDCODE,
                                                (CASE WHEN J.SUP_ID='20.811034' THEN GET_VDCODE(NVL(B.MTRLID_OLD,B.RP_MTRLID)) ELSE  NVL(J.FIELD_EX2,J.SUP_ID)  END))) END) RAW_MAT_SUPPLIER_CODE_1,
                                                (CASE WHEN A.RP_RS=2 THEN 'CHINO-E' ELSE NVL((SELECT  TO_CHAR(SUP_NAME) FROM TB_BS_SUP   WHERE SUP_ID=(NVL(J1.VDCODE,
                                                (CASE WHEN J.SUP_ID='20.811034' THEN GET_VDCODE(NVL(B.MTRLID_OLD,B.RP_MTRLID)) ELSE     NVL(J.FIELD_EX2,J.SUP_ID) END)))),(NVL(J1.VDCODE,
                                                (CASE WHEN J.SUP_ID='20.811034' THEN GET_VDCODE(NVL(B.MTRLID_OLD,B.RP_MTRLID)) ELSE    NVL(J.FIELD_EX2,J.SUP_ID) END)))) END)RAW_MAT_SUPPLIER_NAME_1 
                                                ,(CASE WHEN  A.RP_RS=2 THEN TO_CHAR(B.SEND_DATE,'YYYYMMDD') ELSE  J.LOTNO END) LOTE_CODE_1,(CASE WHEN A.RP_RS=2  THEN TO_CHAR(B.SEND_DATE,'YYYYMMDD') ELSE  J.DATECODE END)DATE_CODE_1
                                                ,TO_NUMBER(B.RP_POINT_COUNT) DEFECT_DOT_1, TO_CHAR(D.SYS_CRT_DATE,'YYYYMMDDHH24MISS') DEFECT_TIME,''DEFECT_OPER_EQPID, TO_CHAR(D.SYS_CRT_DATE,'YYYYMMDDHH24MISS') IN_REPAIR_TIME, TO_CHAR(A.SYS_CRT_DATE,'YYYYMMDDHH24MISS') REPAIR_TIME, TO_CHAR(A.SYS_CRT_DATE,'YYYYMMDDHH24MISS') OUT_REPAIR_TIME,''RETURN_LINE_TIME,DECODE(A.RP_RS,'2','F','P') REPAIR_RESULT
                                                ,''SMT_LINE,''SMT_DATE,''ASSEMBLY_LINE,''ASSEMBLY_DATE,''DEFECT_VALUE,''DEFECT_VALUE_DESC,''DEFECT_MEASUREMENT_CODE,''DEFECT_MEASUREMENT_DESC,
                                                  ''COMPUTER_NAME,''TPS_NAME_TPS,''TPS_VERSION_TPS,''REPAIR_LEVEL,''FROM_TO_LOT_ID,''VERIFY_TRAN_TIME,''GETFLAG,''GETTIME,NULL ACTIONFLAG,'CHINO-E'S_FACTORY,
                                                   (CASE WHEN A.RP_RS=2 THEN 'PCBA' ELSE NVL(GET_T100MODEL@MES(NVL(B.MTRLID_OLD,B.RP_MTRLID),4),K.ITEMNAME) END) SEGMENT2, 
                                                   (CASE WHEN A.RP_RS=2 THEN 'PCBA' ELSE COALESCE(GET_T100MODEL@MES(NVL(B.MTRLID_OLD,B.RP_MTRLID),2),GET_T100MODEL@MES(NVL(B.MTRLID_OLD,B.RP_MTRLID),4),K.ITEMTYPEBIG) END) SEGMENT3,'量产'SEGMENT4,
                                                   (CASE WHEN A.RP_RS=2 THEN 'PCBA' ELSE COALESCE(GET_T100MODEL@MES(NVL(B.MTRLID_OLD,B.RP_MTRLID),3),GET_T100MODEL@MES(NVL(B.MTRLID_OLD,B.RP_MTRLID),4),K.ITEMTYPELIT) END) SEGMENT5,
                                                GET_HWCODE(CASE WHEN A.RP_RS=2 THEN A.PROD_ID ELSE  NVL(B.MTRLID_OLD,B.RP_MTRLID) END) DEFECT_RAW_MAT_ID_2
                                                FROM TB_PM_RP_HD A LEFT OUTER JOIN TB_PM_RP_DT B ON A.RP_ID=B.RP_ID 
                                                LEFT OUTER JOIN TB_PP_MO A1 ON A1.MO=A.MO
                                                LEFT OUTER JOIN TB_PM_QC_HD C ON C.RP_ID=A.RP_ID
                                                LEFT OUTER JOIN TB_PM_QC_DT D ON D.QC_ID=C.QC_ID 
                                                LEFT OUTER JOIN TB_BS_BAD_TYPE E ON E.BAD_TYPE_ID=D.BAD_ITEM_ID 
                                                LEFT OUTER JOIN TB_ENOK_QA_BUG_TYPE H ON   H.CODE2=E.BAD_TYPE_ID
                                                LEFT OUTER JOIN TB_MM_RID_STOCK J ON  NVL(B.RID_OLD,B.RP_RID)=J.RID
                                                LEFT OUTER JOIN TB_MM_RID_PRINT J1 ON  NVL(B.RID_OLD,B.RP_RID)=J1.RID
                                                LEFT OUTER JOIN TB_ENOK_ODM_ITEMCODE K ON K.ITEMCODE=NVL(B.MTRLID_OLD,B.RP_MTRLID) WHERE  A1.CUST_ID IN('20.001.0002','20.001.0001','20.001.0005','2200001','2200002','2200003') AND  B.RP_USER  IS NOT NULL AND B.TB_PM_RP_DT_EX5  IS NOT NULL AND  
                                                (CASE WHEN H.CODE IS NULL THEN (DECODE( E.BAD_TYPE_ID,'-',D.TB_PM_QC_DT_EX4,NVL(E.BAD_TYPE_ID,D.TB_PM_QC_DT_EX4)))   ELSE H.CODE END ) IS NOT NULL
                                                AND (CASE WHEN A.RP_RS=2  THEN 'OK'   ELSE   (CASE WHEN B.DUTY_DESCRIBE_ID='4' THEN (CASE WHEN  NVL(B.RID_OLD,B.RP_RID) IS NULL THEN 'NG' ELSE 'OK' END)   ELSE  'OK' END)END)='OK'  ");
                strselect = string.Format("{0} AND A.SYS_CRT_DATE BETWEEN TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{2}', 'YYYY-MM-DD HH24:MI:SS')", strselect, Date1, Date2);
                await Task.Run(() =>
                {
                    DataTable dt = OracleHelper.ExecuteDataTable1(UserInfo.OracleConnectionStringIms, strselect);
                    if (dt.Rows.Count > 0)
                    {
                        int totalRow = dt.Rows.Count;
                        string TableName = "[HWOUTPUT].[dbo].[Repair_History_Data]";
                        AppHelper.dBtnInsert(dt, TableName, totalRow);
                    }
                });
               
            }
            errMessage.success = true;
            errMessage.Err = "执行完成Hw_RepairIms"; 
            return errMessage;
        }
        public async Task<ErrMessage> Hw_RepairMes(string date1)
        {
            ErrMessage errMessage = new ErrMessage();
            //string sql = "SELECT (TO_CHAR(SYSDATE-(1/24),'YYYY-MM-DD HH24')||':30:00' )DATE1, (TO_CHAR(SYSDATE-(1/24),'YYYY-MM-DD HH24')||':59:59' )DATE1 FROM DUAL";
            //半小时一次--
            string Date1 = string.Empty;
            string Date2 = string.Empty;
            string sql = @"WITH TS AS(SELECT(TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24') || ':00:00')DATE1, (TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24') || ':29:59')DATE2,TO_NUMBER(TO_CHAR(SYSDATE, 'MI'))NB1  FROM DUAL 
                          )SELECT* FROM TS WHERE NB1 >= 30";
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable.Rows.Count > 0)
            {
                Date1 = dataTable.Rows[0][0].ToString();
                Date2 = dataTable.Rows[0][1].ToString();
            }
            else
            {
                sql = @"WITH TS AS(SELECT (TO_CHAR(SYSDATE-(1/24),'YYYY-MM-DD HH24')||':30:00' )DATE1, (TO_CHAR(SYSDATE-(1/24),'YYYY-MM-DD HH24')||':59:59' )DATE2,TO_NUMBER(TO_CHAR(SYSDATE, 'MI'))NB1 FROM DUAL)
                    SELECT* FROM TS WHERE NB1 < 30";
                dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    Date1 = dataTable.Rows[0][0].ToString();
                    Date2 = dataTable.Rows[0][1].ToString();
                }
            }
            if (!string.IsNullOrEmpty(Date1))
            {
                  sql = @"SELECT ''ID,''IF_SEQ,''MOVE_FLAG,''MOVE_TIME,'CHINO-E'FACTORY,T.REPAIR_CARD_CODE LOT_ID, T.REPAIRNUM REP_SEQ, T.COMMAND_CODE EMS_ORDER_ID,
                        UPPER(COALESCE(GET_T100MODEL(D.ITEM_CODE,1),GET_MODEL(D.ITEM_CODE),TO_CHAR(REPLACE((SELECT PRODUCT_MODEL FROM ODM_MODEL WHERE  BOM=D.ITEM_CODE),'/','' ))))  MAT_MODEL, 
                        D.ITEM_CODE MAT_ID, D.ITEM_CODE HW_MAT_ID,DECODE(T.GONGWEI,'MMI2','MMI2',DECODE(T.POSTION_ITEMVERSION,'MMI_OFFLINE','MMI1','Runin_In','RUNIN',T.POSTION_ITEMVERSION)) REQ_OPER,  NVL(H.WORKSEQUENCE,(SELECT  WORKSEQUENCE FROM QA_ERROR    WHERE CODE=H.CODE))
                         DEFECT_REA_TYPE, H.CODE DEFECT_REA_CODE,(CASE WHEN INSTR(T.RECEIVE_PERSON,'[',1)=0 THEN T.RECEIVE_PERSON ELSE  SUBSTR(T.RECEIVE_PERSON,0,INSTR(T.RECEIVE_PERSON,'[',1,1)-1) END ) REQ_USER,
                        T.ERR_DESCRIBE REQ_COMMENT,(CASE WHEN INSTR( T.PERSON,'[',1)=0 THEN T.PERSON ELSE  SUBSTR(T.PERSON,0,INSTR(T.PERSON,'[',1,1)-1) END ) REP_USER, DECODE(T.REPAIRDEPTYPE, '功能', 'R', 'L')
                        REP_TYPE,'' REPAIR_COMMENT, DECODE(T.RES, '报废', 'F', 'R')REPAIR_STATUS,T.REBACKGONGWEI RETURN_OPER,TO_CHAR(T.REPAIRDATE,'YYYYMMDDHH24MISS')   LAST_TRAN_TIME,(CASE WHEN INSTR( T.PERSON,'[',1)=0 
                        THEN T.PERSON ELSE  SUBSTR(T.PERSON,0,INSTR(T.PERSON,'[',1,1)-1) END )  LAST_TRAN_USER,
                        (CASE WHEN INSTR( T.SENDPERSON,'[',1)=0 THEN T.SENDPERSON ELSE  SUBSTR(T.SENDPERSON,0,INSTR(T.SENDPERSON,'[',1,1)-1) END )  CREATE_USER_ID,TO_CHAR(T.SENDDATE,'YYYYMMDDHH24MISS')  CREATE_TIME,
                        NVL(T.ERR_CODE,T.ERR_DESCRIBE) DEFECT_CODE, NVL(T.ERR_DESCRIBE,T.ERR_CODE)DEFECT_DESC,T.REASON_OWE DEFECT_TYPE_CODE, T.COMMAND_CODE CENTER_ORDER_ID,
                        T.REPAIRRESULT   REPAIR_CODE_1, 
                        NVL(NVL(T.POSITION_CODE, NVL((GET_T100MODEL(CASE WHEN T.BAD_ITEM_CODE IS NULL THEN (CASE WHEN C.BAD_ITEM_CODE1 IS NULL
                        THEN (SELECT ITEMCODE FROM IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT 
                        ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）) ELSE TO_CHAR(C.BAD_ITEM_CODE1) END ) ELSE  T.BAD_ITEM_CODE END,4)),GET_T100MODEL(T.BAD_ITEM_CODE,4))), 
                        (CASE WHEN  T.REPAIRRESULT='更换单板' THEN 'PCBA' ELSE ''END)) DEFECT_POSITION_1,  
                        (NVL( (CASE WHEN T.BAD_ITEM_CODE IS NULL THEN (CASE WHEN C.BAD_ITEM_CODE1 IS NULL THEN (SELECT ITEMCODE FROM IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN  TO_CHAR(C.BAD_ITEMMPK1)  IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）) ELSE TO_CHAR(C.BAD_ITEM_CODE1) END ) ELSE  T.BAD_ITEM_CODE END), (CASE WHEN  T.REPAIRRESULT='更换单板' THEN  T.ITEM_CODE ELSE ''END))) DEFECT_RAW_MAT_ID_1,
                        NVL(（CASE WHEN C.BAD_ITEMMPK1 IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）,(CASE WHEN  T.REPAIRRESULT='更换单板' THEN  T.REPAIR_CARD_CODE ELSE '' END))  DEFECT_RAW_LOT_ID_1,
                        NVL( (  CASE WHEN C.VENDOR1  IS NULL THEN (SELECT SUPPLIERCODE FROM  IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN T.OLDXINWEEK IS NULL THEN (CASE WHEN C.BAD_ITEMMPK1 IS NULL THEN T.QUALITYIMPROVEMENT  ELSE TO_CHAR(C.BAD_ITEMMPK1) END)  ELSE T.OLDXINWEEK END))  ELSE TO_CHAR(C.VENDOR1) END    ),(CASE WHEN  T.REPAIRRESULT='更换单板' THEN 'CHINO-E' ELSE ''END))  RAW_MAT_SUPPLIER_CODE_1,
                        NVL((SELECT NAME FROM QA_SUPPLIERINFO WHERE QA_SUPPLIERINFO.CODE = (CASE WHEN C.VENDOR1  IS NULL THEN (SELECT SUPPLIERCODE FROM  IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN T.OLDXINWEEK IS NULL THEN (CASE WHEN C.BAD_ITEMMPK1 IS NULL THEN T.QUALITYIMPROVEMENT    ELSE TO_CHAR(C.BAD_ITEMMPK1) END)  ELSE T.OLDXINWEEK END))  ELSE TO_CHAR(C.VENDOR1) END  )), (CASE WHEN  T.REPAIRRESULT='更换单板' THEN 'CHINO-E' ELSE  '' END)) RAW_MAT_SUPPLIER_NAME_1, 
                        NVL( (CASE WHEN  C.LOTCODE1 IS NULL THEN (SELECT LOTCODE FROM  IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN T.OLDXINWEEK IS NULL THEN (CASE WHEN C.BAD_ITEMMPK1 IS NULL THEN T.QUALITYIMPROVEMENT  ELSE TO_CHAR(C.BAD_ITEMMPK1) END)  ELSE T.OLDXINWEEK END))  ELSE TO_CHAR(C.LOTCODE1)  END),(CASE WHEN  T.REPAIRRESULT='更换单板' THEN TO_CHAR(T.SENDDATE,'YYYYMMDD') ELSE '' END))LOTE_CODE_1,
                        NVL((CASE WHEN  C.DATECODE1 IS NULL THEN (SELECT DATECODE FROM  IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN T.OLDXINWEEK IS NULL THEN (CASE WHEN C.BAD_ITEMMPK1 IS NULL THEN T.QUALITYIMPROVEMENT  ELSE TO_CHAR(C.BAD_ITEMMPK1) END)  ELSE T.OLDXINWEEK END)) ELSE TO_CHAR(C.DATECODE1)  END),(CASE WHEN  T.REPAIRRESULT='更换单板' THEN  TO_CHAR(T.SENDDATE,'YYYYMMDD') ELSE '' END))   DATE_CODE_1, 
                        C.DEFECT_POINT1 DEFECT_DOT_1, TO_CHAR( T.DEFECT_TIME,'YYYYMMDDHH24MISS') DEFECT_TIME, ''DEFECT_OPER_EQPID,TO_CHAR(T.SENDDATE,'YYYYMMDDHH24MISS') IN_REPAIR_TIME,TO_CHAR(T.REPAIRDATE,'YYYYMMDDHH24MISS')  REPAIR_TIME,TO_CHAR(T.REPAIRDATE,'YYYYMMDDHH24MISS') OUT_REPAIR_TIME,CASE WHEN  T.REPAIRDATE IS NOT NULL THEN TO_CHAR(DECODE((SELECT A.PTYPE FROM ODM_XLUSER A WHERE A.PUSER = T.SENDPERSON) , '1', (SELECT MIN(TESTDATE) FROM ODM_REPAIRTR WHERE SN = T.REPAIR_CARD_CODE AND TESTDATE > T.REPAIRDATE),T.REPAIRDATE,'YYYYMMDDHH24MISS') ,'YYYYMMDDHH24MISS') END  RETURN_LINE_TIME,
                        DECODE(T.RES, '报废', 'B', 'P')REPAIR_RESULT,''SMT_LINE,''SMT_DATE,''ASSEMBLY_LINE,''ASSEMBLY_DATE,''DEFECT_VALUE,''DEFECT_VALUE_DESC,''DEFECT_MEASUREMENT_CODE,''DEFECT_MEASUREMENT_DESC,
                        ''COMPUTER_NAME,''TPS_NAME_TPS,''TPS_VERSION_TPS,''REPAIR_LEVEL,''FROM_TO_LOT_ID,''VERIFY_TRAN_TIME,''GETFLAG,''GETTIME,NULL ACTIONFLAG,NVL((SELECT  WORKSHOP2 FROM WORKPRODUCE T WHERE  LINE=T.LINE_CODE ),'CHINO-E')S_FACTORY, 
                        NVL((GET_T100MODEL(CASE WHEN T.BAD_ITEM_CODE IS NULL THEN (CASE WHEN C.BAD_ITEM_CODE1 IS NULL THEN (SELECT ITEMCODE FROM IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）) ELSE TO_CHAR(C.BAD_ITEM_CODE1) END ) ELSE  T.BAD_ITEM_CODE END,4) ),(CASE WHEN  T.REPAIRRESULT='更换单板' THEN 'PCBA' ELSE GET_T100MODEL(T.BAD_ITEM_CODE,4) END))   SEGMENT2,
                        NVL((GET_T100MODEL(CASE WHEN T.BAD_ITEM_CODE IS NULL THEN (CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (SELECT ITEMCODE FROM IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）) ELSE TO_CHAR(C.BAD_ITEM_CODE1) END ) ELSE  T.BAD_ITEM_CODE END,2)),(CASE WHEN  T.REPAIRRESULT='更换单板' THEN 'PCBA' ELSE NVL(GET_T100MODEL(T.BAD_ITEM_CODE,2),GET_T100MODEL(T.BAD_ITEM_CODE,4)) END))SEGMENT3,(CASE WHEN INSTR(T.COMMAND_CODE,'S-')>0 THEN '试制' ELSE '量产' END) SEGMENT4, 
                        NVL( (GET_T100MODEL(CASE WHEN T.BAD_ITEM_CODE IS NULL THEN (CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (SELECT ITEMCODE FROM IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）) ELSE TO_CHAR(C.BAD_ITEM_CODE1) END ) ELSE  T.BAD_ITEM_CODE END,3)),(CASE WHEN  T.REPAIRRESULT='更换单板' THEN 'PCBA' ELSE NVL(GET_T100MODEL(T.BAD_ITEM_CODE,3),GET_T100MODEL(T.BAD_ITEM_CODE,4)) END))SEGMENT5,
                        GET_HWCODE(NVL( (CASE WHEN T.BAD_ITEM_CODE IS NULL THEN (CASE WHEN C.BAD_ITEM_CODE1 IS NULL THEN (SELECT ITEMCODE FROM IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN  TO_CHAR(C.BAD_ITEMMPK1)  IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）) ELSE TO_CHAR(C.BAD_ITEM_CODE1) END ) ELSE  T.BAD_ITEM_CODE END), (CASE WHEN  T.REPAIRRESULT='更换单板' THEN  T.ITEM_CODE ELSE ''END))) DEFECT_RAW_MAT_ID_2
                        FROM QA_IPQC_SUBREPAIR T LEFT OUTER JOIN  WORK_WORKJOB D ON T.COMMAND_CODE = D.WORKJOB_CODE   LEFT OUTER JOIN  REPAIR_DEFECT_INFO C ON C.SN = T.REPAIR_CARD_CODE  AND T.ROWNUMBER = C.ROWNUMBER 
                        LEFT OUTER JOIN QA_ERROR H ON H.CODE = T.HUAWEIBAD_CODE WHERE D.CLIENTCODE = '01'   AND T.REPAIRDATE IS NOT NULL ";
                  string sql2 = @"SELECT ''ID,''IF_SEQ,''MOVE_FLAG,''MOVE_TIME,'CHINO-E'FACTORY,T.REPAIR_CARD_CODE LOT_ID, T.REPAIRNUM REP_SEQ, T.COMMAND_CODE EMS_ORDER_ID, 
                        COALESCE(GET_MODEL(D.ITEMCODE),GET_T100MODEL(D.ITEMCODE,1),TO_CHAR(REPLACE((SELECT PRODUCT_MODEL FROM ODM_MODEL WHERE  BOM=D.ITEMCODE),'/','')))  MAT_MODEL, D.ITEMCODE MAT_ID, D.ITEMCODE HW_MAT_ID, T.POSTION_ITEMVERSION REQ_OPER,  NVL(H.WORKSEQUENCE,(SELECT  WORKSEQUENCE FROM QA_ERROR    WHERE CODE=H.CODE)) DEFECT_REA_TYPE, H.CODE DEFECT_REA_CODE,(CASE WHEN INSTR(T.RECEIVE_PERSON,'[',1)=0 THEN T.RECEIVE_PERSON ELSE  SUBSTR(T.RECEIVE_PERSON,0,INSTR(T.RECEIVE_PERSON,'[',1,1)-1) END ) REQ_USER,
                        T.ERR_DESCRIBE REQ_COMMENT,(CASE WHEN INSTR(T.PERSON, '[', 1) = 0 THEN T.PERSON ELSE  SUBSTR(T.PERSON, 0, INSTR(T.PERSON, '[', 1, 1) - 1) END ) REP_USER, DECODE(T.REPAIRDEPTYPE, '功能', 'R', 'L') REP_TYPE,'' REPAIR_COMMENT, DECODE(T.RES, '报废', 'F', 'R')REPAIR_STATUS,T.REBACKGONGWEI RETURN_OPER, TO_CHAR(T.REPAIRDATE, 'YYYYMMDDHH24MISS')   LAST_TRAN_TIME,(CASE WHEN INSTR(T.PERSON,'[',1)= 0 THEN T.PERSON ELSE  SUBSTR(T.PERSON, 0, INSTR(T.PERSON, '[', 1, 1) - 1) END )  LAST_TRAN_USER,
                        (CASE WHEN INSTR(T.SENDPERSON,'[',1)= 0 THEN T.SENDPERSON ELSE  SUBSTR(T.SENDPERSON, 0, INSTR(T.SENDPERSON, '[', 1, 1) - 1) END )  CREATE_USER_ID,TO_CHAR(T.SENDDATE, 'YYYYMMDDHH24MISS')  CREATE_TIME,
                        NVL(T.ERR_CODE,T.ERR_DESCRIBE) DEFECT_CODE, NVL(T.ERR_DESCRIBE,T.ERR_CODE)DEFECT_DESC,T.REASON_OWE DEFECT_TYPE_CODE, T.COMMAND_CODE CENTER_ORDER_ID,   REPAIRRESULT   REPAIR_CODE_1,
                        NVL(NVL(T.POSITION_CODE, NVL((GET_T100MODEL(CASE WHEN T.BAD_ITEM_CODE IS NULL THEN (CASE WHEN C.BAD_ITEM_CODE1 IS NULL  THEN (SELECT ITEMCODE FROM IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT  ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）) ELSE TO_CHAR(C.BAD_ITEM_CODE1) END ) ELSE  T.BAD_ITEM_CODE END,4)),GET_T100MODEL(T.BAD_ITEM_CODE,4))),  (CASE WHEN  T.REPAIRRESULT='更换单板' THEN 'PCBA' ELSE ''END)) DEFECT_POSITION_1,  
                        ( NVL( (CASE WHEN T.BAD_ITEM_CODE IS NULL THEN (CASE WHEN C.BAD_ITEM_CODE1 IS NULL THEN (SELECT ITEMCODE FROM IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）) ELSE TO_CHAR(C.BAD_ITEM_CODE1) END ) ELSE  T.BAD_ITEM_CODE END),(CASE WHEN  T.REPAIRRESULT='更换单板' THEN  T.ITEM_CODE ELSE ''END))) DEFECT_RAW_MAT_ID_1,
                        NVL(（CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）,  (CASE WHEN  T.REPAIRRESULT='更换单板' THEN  T.REPAIR_CARD_CODE ELSE GET_T100MODEL(T.BAD_ITEM_CODE,4) END))  DEFECT_RAW_LOT_ID_1,
                        NVL((  CASE WHEN C.VENDOR1  IS NULL THEN (SELECT SUPPLIERCODE FROM  IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）)  ELSE TO_CHAR(C.VENDOR1) END    ),  (CASE WHEN  T.REPAIRRESULT='更换单板' THEN  T.REPAIR_CARD_CODE ELSE ''END))  RAW_MAT_SUPPLIER_CODE_1,
                        NVL( (SELECT NAME FROM QA_SUPPLIERINFO WHERE QA_SUPPLIERINFO.CODE = (CASE WHEN C.VENDOR1  IS NULL THEN (SELECT SUPPLIERCODE FROM  IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）)  ELSE TO_CHAR(C.VENDOR1) END  )),  (CASE WHEN  T.REPAIRRESULT='更换单板' THEN  T.REPAIR_CARD_CODE ELSE ''END)) RAW_MAT_SUPPLIER_NAME_1, 
                        NVL((CASE WHEN  C.LOTCODE1 IS NULL THEN (SELECT LOTCODE FROM  IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）)  ELSE TO_CHAR(C.LOTCODE1)  END),    (CASE WHEN  T.REPAIRRESULT='更换单板' THEN TO_CHAR(T.SENDDATE,'YYYYMMDD') ELSE '' END))LOTE_CODE_1,
                        NVL( (CASE WHEN  C.DATECODE1 IS NULL THEN (SELECT DATECODE FROM  IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）) ELSE TO_CHAR(C.DATECODE1)  END),(CASE WHEN  T.REPAIRRESULT='更换单板' THEN  TO_CHAR(T.SENDDATE,'YYYYMMDD') ELSE ''END))   DATE_CODE_1, 
                        C.DEFECT_POINT1 DEFECT_DOT_1,TO_CHAR( T.DEFECT_TIME, 'YYYYMMDDHH24MISS') DEFECT_TIME,
                        ''DEFECT_OPER_EQPID,TO_CHAR(T.SENDDATE, 'YYYYMMDDHH24MISS') IN_REPAIR_TIME,TO_CHAR(T.REPAIRDATE, 'YYYYMMDDHH24MISS')  REPAIR_TIME,TO_CHAR(T.REPAIRDATE, 'YYYYMMDDHH24MISS') OUT_REPAIR_TIME,CASE WHEN T.REPAIRDATE IS NOT NULL THEN TO_CHAR(DECODE((SELECT A.PTYPE FROM ODM_XLUSER A WHERE A.PUSER = T.SENDPERSON) , '1', (SELECT MIN(TESTDATE) FROM ODM_REPAIRTR WHERE SN = T.REPAIR_CARD_CODE AND TESTDATE > T.REPAIRDATE),T.REPAIRDATE,'YYYYMMDDHH24MISS') ,'YYYYMMDDHH24MISS') END RETURN_LINE_TIME,
                        DECODE(T.RES, '报废', 'B', 'P')REPAIR_RESULT,''SMT_LINE,''SMT_DATE,''ASSEMBLY_LINE,''ASSEMBLY_DATE,''DEFECT_VALUE,''DEFECT_VALUE_DESC,''DEFECT_MEASUREMENT_CODE,''DEFECT_MEASUREMENT_DESC,
                        ''COMPUTER_NAME,''TPS_NAME_TPS,''TPS_VERSION_TPS,''REPAIR_LEVEL,''FROM_TO_LOT_ID,''VERIFY_TRAN_TIME,''GETFLAG,''GETTIME,NULL ACTIONFLAG,NVL((SELECT  WORKSHOP2 FROM WORKPRODUCE T WHERE  LINE=T.LINE_CODE),'CHINO-E')S_FACTORY,
                        NVL((GET_T100MODEL(CASE WHEN T.BAD_ITEM_CODE IS NULL THEN (CASE WHEN C.BAD_ITEM_CODE1 IS NULL THEN (SELECT ITEMCODE FROM IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）) ELSE TO_CHAR(C.BAD_ITEM_CODE1) END ) ELSE  T.BAD_ITEM_CODE END,4) ),(CASE WHEN  T.REPAIRRESULT='更换单板' THEN 'PCBA' ELSE GET_T100MODEL(T.BAD_ITEM_CODE,4) END))   SEGMENT2,
                        NVL((GET_T100MODEL(CASE WHEN T.BAD_ITEM_CODE IS NULL THEN (CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (SELECT ITEMCODE FROM IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）) ELSE TO_CHAR(C.BAD_ITEM_CODE1) END ) ELSE  T.BAD_ITEM_CODE END,2)),(CASE WHEN  T.REPAIRRESULT='更换单板' THEN 'PCBA' ELSE NVL(GET_T100MODEL(T.BAD_ITEM_CODE,2),GET_T100MODEL(T.BAD_ITEM_CODE,4)) END))SEGMENT3,(CASE WHEN INSTR(T.COMMAND_CODE,'S-')>0 THEN '试制' ELSE '量产' END) SEGMENT4, 
                        NVL( (GET_T100MODEL(CASE WHEN T.BAD_ITEM_CODE IS NULL THEN (CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (SELECT ITEMCODE FROM IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN TO_CHAR(C.BAD_ITEMMPK1) IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）) ELSE TO_CHAR(C.BAD_ITEM_CODE1) END ) ELSE  T.BAD_ITEM_CODE END,3)),(CASE WHEN  T.REPAIRRESULT='更换单板' THEN 'PCBA' ELSE NVL(GET_T100MODEL(T.BAD_ITEM_CODE,3),GET_T100MODEL(T.BAD_ITEM_CODE,4)) END))SEGMENT5,
                        GET_HWCODE(NVL( (CASE WHEN T.BAD_ITEM_CODE IS NULL THEN (CASE WHEN C.BAD_ITEM_CODE1 IS NULL THEN (SELECT ITEMCODE FROM IQC_ITEMMPK_INFOR WHERE ITEMMPK=（CASE WHEN  TO_CHAR(C.BAD_ITEMMPK1)  IS NULL THEN (CASE WHEN T.OLDXINWEEK IS NULL THEN T.QUALITYIMPROVEMENT ELSE TO_CHAR(T.OLDXINWEEK) END)  ELSE TO_CHAR(C.BAD_ITEMMPK1) END）) ELSE TO_CHAR(C.BAD_ITEM_CODE1) END ) ELSE  T.BAD_ITEM_CODE END), (CASE WHEN  T.REPAIRRESULT='更换单板' THEN  T.ITEM_CODE ELSE ''END))) DEFECT_RAW_MAT_ID_2
                        FROM QA_IPQC_SUBREPAIR T  LEFT OUTER JOIN  ODM_ITEMCODE D   ON T.ITEM_CODE = D.ITEMCODE   LEFT OUTER JOIN  REPAIR_DEFECT_INFO C ON C.SN = T.REPAIR_CARD_CODE  AND T.ROWNUMBER = C.ROWNUMBER
                        LEFT OUTER JOIN QA_ERROR H ON H.CODE = T.HUAWEIBAD_CODE
                        WHERE T.REPAIRDATE IS NOT NULL AND T.CLIENTCODE='01'   AND T.ITEM_DESCRIBE='IMS'";
                  sql = string.Format("{0} AND T.REPAIRDATE BETWEEN TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{2}', 'YYYY-MM-DD HH24:MI:SS')", sql, Date1, Date2);
                  sql2 = string.Format("{0} AND T.REPAIRDATE BETWEEN TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{2}', 'YYYY-MM-DD HH24:MI:SS')", sql2, Date1, Date2); 
                  sql = sql + " UNION " + sql2;
                  await Task.Run(() =>
                  {
                        DataTable dt = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, sql);
                        if (dt.Rows.Count > 0)
                        {
                            int totalRow = dt.Rows.Count;
                            string TableName = "[HWOUTPUT].[dbo].[Repair_History_Data]";
                            AppHelper.dBtnInsert(dt, TableName, totalRow);
                        }
                  }); 
            }
            //throw new NotImplementedException();
            errMessage.success = true;
            errMessage.Err = "执行完成Hw_RepairMes";
            return errMessage;
        }
        public async Task<DT_BEAT> BEAT_OUTTT(string lINE, string cODE, string mODEL)
        {
            double minNb = 0;//下限
            double maxNb = 0;//上限
            double MalNb = 0;//标准值
            double MtpNb = 0;//最高上限
            int Hour = 0;
            int TYP1 = 0;
            List<string> Liststation = new List<string>();//工序
            List<string> Liststation1 = new List<string>();
            List<string> ListDate = new List<string>();//时间段
            List<string> ListDate1 = new List<string>();//时间点
            string date = string.Empty;
            string sql = "SELECT ROUND(600/ BEATE_NUM),CEIL(600/BEATE_MIN),TRUNC(600/BEATE_MAX),TRUNC(600/BEATE_TOP) FROM ODM_BEATE WHERE LINECODE='" + lINE + "'";
            DataTable table = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (table.Rows.Count > 0)
            {
                MalNb = string.IsNullOrWhiteSpace(table.Rows[0][0].ToString()) ? 0 : Convert.ToInt32(table.Rows[0][0].ToString());
                maxNb = string.IsNullOrWhiteSpace(table.Rows[0][1].ToString()) ? 0 : Convert.ToInt32(table.Rows[0][1].ToString());
                minNb = string.IsNullOrWhiteSpace(table.Rows[0][2].ToString()) ? 0 : Convert.ToInt32(table.Rows[0][2].ToString());
                MtpNb = string.IsNullOrWhiteSpace(table.Rows[0][3].ToString())?0:Convert.ToInt32(table.Rows[0][3].ToString());
            }
            sql = "SELECT  NAME FROM ODM_STATIONTT T  WHERE CODE='" + cODE + "' ORDER BY CODEFUNCTION";
            table = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (table.Rows.Count > 0)
            { 
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string station = table.Rows[i][0].ToString();
                    if (station == "贴片")
                    {
                        station = "AOI2";
                    }
                    Liststation.Add(station);
                }
            }
            bool ismes = true;
            if (lINE.Substring(0, 1) == "S")
            {
                //if (!IsImsLinecode(strlinecode))
                //{
                    ismes = false;
                //}
            }
            sql = "SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD'),TO_NUMBER(TO_CHAR(SYSDATE,'HH24')), TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') FROM DUAL";
            table = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (table.Rows.Count > 0)
            {
                date = table.Rows[0][0].ToString();
                Hour = Convert.ToInt32(table.Rows[0][1].ToString()); 
            }
            //Hour = 10;
            if (0 <= Hour && Hour <= 3)
            {
                sql = @"SELECT * FROM(WITH T AS  (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                        SELECT
                        ( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 00:00:00'),'YYYY-MM-DD HH24:MI:SS') + (ROWNUM - 1)/24/6, 'YYYY-MM-DD HH24:MI:SS')
                        ) STARTDATE
                        FROM T )  WHERE TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS')<= TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 04:00:00'), 'YYYY-MM-DD HH24:MI:SS')";
                TYP1 = 0;
            }
            if (4 <= Hour && Hour <= 7)
            {
                sql = @"SELECT * FROM(WITH T AS  (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                        SELECT
                        ( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 04:00:00'),'YYYY-MM-DD HH24:MI:SS') + (ROWNUM - 1)/24/6, 'YYYY-MM-DD HH24:MI:SS')
                        ) STARTDATE
                        FROM T )  WHERE TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS')<= TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 08:00:00'), 'YYYY-MM-DD HH24:MI:SS')";
                TYP1 = 1;
            }
            if (8 <= Hour && Hour <= 11)
            {
                sql = @"SELECT * FROM(WITH T AS  (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                        SELECT
                        ( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 08:00:00'),'YYYY-MM-DD HH24:MI:SS') + (ROWNUM - 1)/24/6, 'YYYY-MM-DD HH24:MI:SS')
                        ) STARTDATE
                        FROM T )  WHERE TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS')<= TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 12:00:00'), 'YYYY-MM-DD HH24:MI:SS')";
                TYP1 = 2;
            }
            if (12 <= Hour && Hour <= 15)
            {
                sql = @"SELECT * FROM(WITH T AS  (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                        SELECT
                        ( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 12:00:00'),'YYYY-MM-DD HH24:MI:SS') + (ROWNUM - 1)/24/6, 'YYYY-MM-DD HH24:MI:SS')
                        ) STARTDATE
                        FROM T )  WHERE TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS')<= TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 16:00:00'), 'YYYY-MM-DD HH24:MI:SS')";
                TYP1 = 3;
            }
            if (16 <= Hour && Hour <= 19)
            {
                sql = @"SELECT * FROM(WITH T AS  (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                        SELECT
                        ( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 16:00:00'),'YYYY-MM-DD HH24:MI:SS') + (ROWNUM - 1)/24/6, 'YYYY-MM-DD HH24:MI:SS')
                        ) STARTDATE
                        FROM T )  WHERE TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS')<= TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 20:00:00'), 'YYYY-MM-DD HH24:MI:SS')";
                TYP1 = 4;
            }
            if (20 <= Hour && Hour <= 23)
            {
                sql = @"SELECT * FROM(WITH T AS  (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                        SELECT
                        ( TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE,'YYYY-MM-DD'),' 20:00:00'),'YYYY-MM-DD HH24:MI:SS') + (ROWNUM - 1)/24/6, 'YYYY-MM-DD HH24:MI:SS')
                        ) STARTDATE
                        FROM T )  WHERE TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS')<= TO_DATE(CONCAT(TO_CHAR(SYSDATE+1, 'YYYY-MM-DD'), ' 00:00:00'), 'YYYY-MM-DD HH24:MI:SS')";
                TYP1 = 5;
            }
            string sql1 = sql;
            table = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    ListDate.Add(table.Rows[i][0].ToString());
                }
            }
            if (0 <= Hour && Hour <= 3)
            {
                sql = @"SELECT * FROM(WITH T AS  (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                        SELECT
                        (TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 00:00:00'), 'YYYY-MM-DD HH24:MI:SS') + (ROWNUM - 1) / 24 / 6, 'YYYY-MM-DD HH24:MI:SS')
                        ) STARTDATE
                        FROM T )  WHERE TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS')<= SYSDATE  AND TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS') < TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 04:00:00'), 'YYYY-MM-DD HH24:MI:SS')";
            }
            if (4 <= Hour && Hour <= 7)
            {
                sql = @"SELECT * FROM(WITH T AS  (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                        SELECT
                        (TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 04:00:00'), 'YYYY-MM-DD HH24:MI:SS') + (ROWNUM - 1) / 24 / 6, 'YYYY-MM-DD HH24:MI:SS')
                        ) STARTDATE
                        FROM T )  WHERE TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS')<= SYSDATE  AND TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS') < TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 08:00:00'), 'YYYY-MM-DD HH24:MI:SS')";
            }
            if (8 <= Hour && Hour <= 11)
            {
                sql = @"SELECT * FROM(WITH T AS  (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                        SELECT
                        (TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 08:00:00'), 'YYYY-MM-DD HH24:MI:SS') + (ROWNUM - 1) / 24 / 6, 'YYYY-MM-DD HH24:MI:SS')
                        ) STARTDATE
                        FROM T )  WHERE TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS')<= SYSDATE  AND TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS') < TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 12:00:00'), 'YYYY-MM-DD HH24:MI:SS')";
            }
            if (12 <= Hour && Hour <= 15)
            {
                sql = @"SELECT * FROM(WITH T AS  (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                        SELECT
                        (TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 12:00:00'), 'YYYY-MM-DD HH24:MI:SS') + (ROWNUM - 1) / 24 / 6, 'YYYY-MM-DD HH24:MI:SS')
                        ) STARTDATE
                        FROM T )  WHERE TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS')<= SYSDATE  AND TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS') < TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 16:00:00'), 'YYYY-MM-DD HH24:MI:SS')";
            }
            if (16 <= Hour && Hour <= 19)
            {
                sql = @"SELECT * FROM(WITH T AS  (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                        SELECT
                        (TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 16:00:00'), 'YYYY-MM-DD HH24:MI:SS') + (ROWNUM - 1) / 24 / 6, 'YYYY-MM-DD HH24:MI:SS')
                        ) STARTDATE
                        FROM T )  WHERE TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS')<= SYSDATE  AND TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS') < TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 20:00:00'), 'YYYY-MM-DD HH24:MI:SS')";
            }
            if (20 <= Hour && Hour <= 23)
            {
                sql = @"SELECT * FROM(WITH T AS  (SELECT ROWNUM RN FROM DUAL CONNECT BY ROWNUM<=72)
                        SELECT
                        (TO_CHAR(TO_DATE(CONCAT(TO_CHAR(SYSDATE, 'YYYY-MM-DD'), ' 20:00:00'), 'YYYY-MM-DD HH24:MI:SS') + (ROWNUM - 1) / 24 / 6, 'YYYY-MM-DD HH24:MI:SS')
                        ) STARTDATE
                        FROM T )  WHERE TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS')<= SYSDATE  AND TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS') < TO_DATE(CONCAT(TO_CHAR(SYSDATE+1, 'YYYY-MM-DD'), ' 00:00:00'), 'YYYY-MM-DD HH24:MI:SS')";
            }
            table = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    ListDate1.Add(table.Rows[i][0].ToString());
                }
            }
            List<string> LisDTtime = new List<string>();
            //休息时间
            sql = @"SELECT T.*,ROWID FROM ODM_BEATE_DT T WHERE T.LINECODE='" + lINE + "' AND TO_CHAR(TO_DATE(T.BEATE_BEGIN,'YYYY-MM-DD HH24:MI:SS'),'YYYY-MM-DD')='" + date + "'";
            table = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string strdate1 = table.Rows[i][0].ToString();
                    string strdate11 = table.Rows[i][0].ToString();
                    sql = @"SELECT    (CASE  FLOOR((TO_CHAR(SCAN_TIME,'MI'))/10) 
                                       WHEN  0 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')|| ':00:00'
                                       WHEN  1 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')|| ':10:00'
                                       WHEN  2 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')|| ':20:00'
                                       WHEN  3 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')|| ':30:00'
                                       WHEN  4 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')|| ':40:00'
                                       WHEN  5 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')|| ':50:00'
                                 END)   AS NEWTIME
                                 FROM(SELECT TO_DATE('" + strdate11 + "', 'YYYY-MM-DD HH24:MI:SS') SCAN_TIME FROM DUAL)";
                    System.Data.DataTable dm11 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                    if (dm11.Rows.Count > 0)
                    {
                        strdate11 = dm11.Rows[0][0].ToString();
                    }
                    string strdate2 = table.Rows[i][1].ToString();
                    sql = string.Format(@"{0}  AND TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS') BETWEEN  TO_DATE('" + strdate11 + "', 'YYYY-MM-DD HH24:MI:SS') AND  TO_DATE('" + strdate2 + "', 'YYYY-MM-DD HH24:MI:SS')", sql1);
                    System.Data.DataTable dm1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                    if (dm1.Rows.Count > 0)
                    {
                        for (int j = 0; j < dm1.Rows.Count; j++)
                        {
                            string strdate3 = dm1.Rows[j][0].ToString();
                            LisDTtime.Add(strdate3);
                        }
                    }
                }
            }
            else
            {
                sql = @"SELECT T.*,ROWID FROM ODM_BEATE_DT1 T WHERE T.LINECODE='" + lINE + "' ";
                table = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (table.Rows.Count > 0)
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        string strdate1 = date + " " + table.Rows[i][0].ToString();
                        string strdate11 = date + " " + table.Rows[i][0].ToString();
                        sql = @"SELECT    (CASE  FLOOR((TO_CHAR(SCAN_TIME,'MI'))/10) 
                                       WHEN  0 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')|| ':00:00'
                                       WHEN  1 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')|| ':10:00'
                                       WHEN  2 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')|| ':20:00'
                                       WHEN  3 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')|| ':30:00'
                                       WHEN  4 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')|| ':40:00'
                                       WHEN  5 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')|| ':50:00'
                                 END)   AS NEWTIME
                                 FROM(SELECT TO_DATE('" + strdate11 + "', 'YYYY-MM-DD HH24:MI:SS') SCAN_TIME FROM DUAL)";
                        System.Data.DataTable dm11 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                        if (dm11.Rows.Count > 0)
                        {
                            strdate11 = dm11.Rows[0][0].ToString();
                        }
                        int Dss1 = table.Rows[i][0].ToString().Replace(":", "") == "" ? 0 : Convert.ToInt32(table.Rows[i][0].ToString().Replace(":", ""));
                        int Dss2 = table.Rows[i][1].ToString().Replace(":", "") == "" ? 0 : Convert.ToInt32(table.Rows[i][1].ToString().Replace(":", ""));
                        string strdate2 = date + " " + table.Rows[i][1].ToString();
                        if (Dss1 <= 1200 && Dss2 >= 1200)
                        {
                            LisDTtime.Add(date + " 12:00:00");
                        }
                        if (Dss1 <= 1200 && Dss2 >= 1200)
                        {
                            LisDTtime.Add(date + " 12:00:00");
                        }
                        if (Dss1 <= 1600 && Dss2 >= 1600)
                        {
                            LisDTtime.Add(date + " 16:00:00");
                        }
                        if (Dss1 <= 800 && Dss2 >= 800)
                        {
                            LisDTtime.Add(date + " 08:00:00");
                        }
                        sql = string.Format(@"{0}  AND TO_DATE(STARTDATE,'YYYY-MM-DD HH24:MI:SS') BETWEEN  TO_DATE('" + strdate11 + "', 'YYYY-MM-DD HH24:MI:SS') AND  TO_DATE('" + strdate2 + "', 'YYYY-MM-DD HH24:MI:SS')", sql1);
                        System.Data.DataTable dm1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                        if (dm1.Rows.Count > 0)
                        {
                            for (int j = 0; j < dm1.Rows.Count; j++)
                            {
                                string strdate3 = dm1.Rows[j][0].ToString();
                                LisDTtime.Add(strdate3);
                            }
                        }
                    }
                }
            }
            if (LisDTtime.Count > 0)
            {
                for (int i = 0; i < LisDTtime.Count; i++)
                {
                    string dateT = LisDTtime[i];
                    //ListDate1.Remove(dateT);
                    //ListDate.Remove(dateT);
                }
            }
            DataTable exidre1 = AppHelper.GetTableSchema(LisDTtime,Hour);
            for (int i = 0; i < Liststation.Count; i++)
            {
                string station = Liststation[i];
                DataRow drq = exidre1.NewRow();
                drq[0] = station;
                for (int j = 0; j < ListDate.Count; j++)
                {
                    string date1 = ListDate[j];
                    drq[j + 1] = "";
                }
                exidre1.Rows.Add(drq);
            }
            int RC = 0;
            for (int k1 = 0; k1 < ListDate1.Count; k1++)
            {
                int num = k1 + 1;
                for (int k2 = 0; k2 < exidre1.Rows.Count; k2++)
                {
                    string cloumname = date+" "+ exidre1.Columns[num].ColumnName+":00"; 
                    exidre1.Rows[k2][num] = "0";  
                }
                var t1 = ListDate1[k1];
                if (LisDTtime.Contains(t1))
                {
                    RC++;
                }
            } 
            string str = string.Empty;
            for (int i = 0; i < Liststation.Count; i++)
            {
                string station = Liststation[i];
                sql = $"SELECT CODE,NAME,CODENAME,CODEFUNCTION,TO_CHAR(SYSDATE,'YYYY-MM-DD')DD,DESCRIBE,TO_CHAR(SYSDATE,'HH24:MI:SS') TT,DESCRIBE1 FROM ODM_STATION_SEL T WHERE NAME='{station}'";
                DataTable   dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dt.Rows.Count > 0)
                {
                    string code1 = dt.Rows[0]["CODE"].ToString();//数据库
                    string name = dt.Rows[0]["NAME"].ToString();//工位
                    string codenaem = dt.Rows[0]["CODENAME"].ToString();//是否按工单查询
                    string codefunction = dt.Rows[0]["CODEFUNCTION"].ToString();//表
                    string describe = dt.Rows[0]["DESCRIBE"].ToString();//查询语句
                    string describe1 = dt.Rows[0]["DESCRIBE1"].ToString();
                    switch (code1)
                    {
                        case "MES":
                            if (codenaem == "MO")
                            {
                                List<string> ListWorkcodes1 = new List<string>();
                                str = string.Format("SELECT DISTINCT(T.WORKJOB_CODE)  FROM TODAYWORKJOB T    WHERE T.CREATEDATE = TRUNC(SYSDATE) AND T.TESTPOSITION ='" + station + "'  AND T.LINE_CODE='{0}'", lINE);
                                dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, str);
                                if (dt.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        ListWorkcodes1.Add(dt.Rows[j][0].ToString());
                                    }
                                }
                                if (ListWorkcodes1.Count > 0)
                                {
                                    for (int j = 0; j < ListWorkcodes1.Count; j++)
                                    {
                                        string mo = ListWorkcodes1[j].ToString();
                                        string wo = mo.Replace("-", "_");
                                        str = string.Format(@"SELECT COUNT(TMP.BARCODE) TOTALNUM,TMP.TESTPOSITION,TMP.NEWTIME
                                            FROM
                                            (SELECT  BARCODE, TESTPOSITION, --ID, 状态
                                                     TO_CHAR(SCAN_TIME, 'YYY-MM-DD HH24:MI:SS') OLDTIME, (CASE  FLOOR((TO_CHAR(SCAN_TIME, 'MI')) / 10)
                                                   WHEN  0 THEN TO_CHAR(SCAN_TIME, 'YYYY-MM-DD HH24') || ':00:00'
                                                   WHEN  1 THEN TO_CHAR(SCAN_TIME, 'YYYY-MM-DD HH24') || ':10:00'
                                                   WHEN  2 THEN TO_CHAR(SCAN_TIME, 'YYYY-MM-DD HH24') || ':20:00'
                                                   WHEN  3 THEN TO_CHAR(SCAN_TIME, 'YYYY-MM-DD HH24') || ':30:00'
                                                   WHEN  4 THEN TO_CHAR(SCAN_TIME, 'YYYY-MM-DD HH24') || ':40:00'
                                                   WHEN  5 THEN TO_CHAR(SCAN_TIME, 'YYYY-MM-DD HH24') || ':50:00'
                                             END)   AS NEWTIME
                                             FROM(SELECT BARCODE, SCAN_TIME, TESTPOSITION FROM {0} T  WHERE   SCAN_POSITION = 'F' AND TRUNC（T.SCAN_TIME) = TRUNC(SYSDATE)   AND T.TESTPOSITION = '{1}' AND T.LINE_CODE2 = '{2}'
                                                UNION  SELECT BARCODE,A.SCAN_TIME,A.TESTPOSITION   FROM REWORK_PRIMARY_RECORD A WHERE A.ATTEMPTER_CODE='{3}' AND  SCAN_POSITION='F' AND 
                                                TRUNC(A.SCAN_TIME)= TRUNC(SYSDATE) AND A.TESTPOSITION ='{1}' AND A.LINE_CODE2='{2}'  
                                            )) TMP
                                           GROUP BY TMP.NEWTIME,TMP.TESTPOSITION
                                           ORDER BY TMP.NEWTIME", wo, station, lINE, mo);
                                        if (!string.IsNullOrEmpty(describe1))
                                        {
                                            str = describe1;
                                        }
                                        dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, str);
                                        if (dt.Rows.Count > 0)
                                        {
                                            for (int k = 0; k < dt.Rows.Count; k++)
                                            {
                                                bool bl = false;
                                                string tecount = dt.Rows[k][0].ToString();
                                                string testation = dt.Rows[k][1].ToString();
                                                string tetime = dt.Rows[k][2].ToString();
                                                int rowcount = 0;
                                                int clomcounts = 0;
                                                for (int l = 0; l < exidre1.Rows.Count; l++)
                                                {
                                                    string barcode = exidre1.Rows[l][0].ToString();
                                                    if (barcode == testation)
                                                    {
                                                        rowcount = l;
                                                        bl = true;
                                                    }
                                                }
                                                int num = 0;
                                                if (bl)
                                                {
                                                    for (int l = 0; l < ListDate1.Count; l++)
                                                    {
                                                        if (tetime == ListDate1[l])
                                                        {
                                                            num = l + 1;
                                                            if (!string.IsNullOrEmpty(exidre1.Rows[rowcount][num].ToString()))
                                                            {
                                                                string value = exidre1.Rows[rowcount][num].ToString();
                                                                clomcounts = Convert.ToInt32(value);
                                                            }
                                                            clomcounts += Convert.ToInt32(tecount);
                                                            exidre1.Rows[rowcount][num] = clomcounts;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(describe))
                                {
                                    sql = string.Format(describe, date, lINE, station);
                                    dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                                    if (dt.Rows.Count > 0)
                                    {
                                        for (int j = 0; j < dt.Rows.Count; j++)
                                        {
                                            string toalnum = dt.Rows[j][2].ToString();
                                            string ttimes = dt.Rows[j][1].ToString();
                                            bool bl = false;
                                            string tecount = toalnum;
                                            string testation = station;
                                            string tetime = ttimes;
                                            int rowcount = 0;
                                            int clomcounts = 0;
                                            for (int l = 0; l < exidre1.Rows.Count; l++)
                                            {
                                                string barcode = exidre1.Rows[l][0].ToString();
                                                if (barcode == testation)
                                                {
                                                    rowcount = l;
                                                    bl = true;
                                                }
                                            }
                                            int num = 0;
                                            if (bl)
                                            {
                                                for (int l = 0; l < ListDate1.Count; l++)
                                                {
                                                    if (tetime == ListDate1[l])
                                                    {
                                                        num = l + 1;
                                                        if (!string.IsNullOrEmpty(exidre1.Rows[rowcount][num].ToString()))
                                                        {
                                                            string value = exidre1.Rows[rowcount][num].ToString();
                                                            clomcounts = Convert.ToInt32(value);
                                                        }
                                                        clomcounts += Convert.ToInt32(tecount);
                                                        exidre1.Rows[rowcount][num] = clomcounts;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "IMS":
                            sql = string.Format(@"SELECT COUNT(TMP.LB_ID) TOTALNUM,TMP.WP_ID,TMP.NEWTIME
                                FROM
                                (SELECT  LB_ID, WP_ID, --ID, 状态
                                       TO_CHAR(WP_CMP_DATE, 'YYY-MM-DD HH24:MI:SS') OLDTIME, (CASE  FLOOR((TO_CHAR(WP_CMP_DATE, 'MI')) / 10)
                                       WHEN  0 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':00:00'
                                       WHEN  1 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':10:00'
                                       WHEN  2 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':20:00'
                                       WHEN  3 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':30:00'
                                       WHEN  4 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':40:00'
                                       WHEN  5 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':50:00'
                                 END) AS NEWTIME
                                 FROM(SELECT WP_CMP_DATE, WP_ID, LB_ID FROM TB_PM_MO_LBWP T WHERE PL_ID = '{1}' AND WP_ID = '{2}'  AND  WP_ID <> 'RE' AND  WP_CMP_DATE
                                 BETWEEN TO_DATE('{0} 00:00:00', 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE('{0} 23:59:59', 'YYYY/MM/DD HH24:MI:SS')) ) TMP
                                  GROUP BY TMP.NEWTIME,TMP.WP_ID
                                  ORDER BY TMP.NEWTIME", date, lINE, station);
                            dt = OracleHelper.ExecuteDataTable(UserInfo.OracleConnectionStringIms, sql);
                            if (dt.Rows.Count > 0)
                            {
                                for (int k = 0; k < dt.Rows.Count; k++)
                                {
                                    bool bl = false;
                                    string tecount = dt.Rows[k][0].ToString();
                                    string testation = dt.Rows[k][1].ToString();
                                    string tetime = dt.Rows[k][2].ToString();
                                    int rowcount = 0;
                                    int clomcounts = 0;
                                    for (int l = 0; l < exidre1.Rows.Count; l++)
                                    {
                                        string barcode = exidre1.Rows[l][0].ToString();
                                        if (barcode == testation)
                                        {
                                            rowcount = l;
                                            bl = true;
                                        }
                                    }
                                    int num = 0;
                                    if (bl)
                                    {
                                        for (int l = 0; l < ListDate1.Count; l++)
                                        {
                                            if (tetime == ListDate1[l])
                                            {
                                                num = l + 1;
                                                if (!string.IsNullOrEmpty(exidre1.Rows[rowcount][num].ToString()))
                                                {
                                                    string value = exidre1.Rows[rowcount][num].ToString();
                                                    clomcounts = Convert.ToInt32(value);
                                                }
                                                clomcounts += Convert.ToInt32(tecount);
                                                exidre1.Rows[rowcount][num] = clomcounts;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            break;
                        case "40":
                            int numcount =AppHelper.Get_ODM_STATION_LINENUM(lINE, station);
                            string str1 = string.Format(@"SELECT COUNT(*)*{3} NUM,STATION,DT FROM
                                           (
                                               SELECT STATION,CONVERT(VARCHAR(100),DATEADD(MI,(DATEDIFF(MI,CONVERT(VARCHAR(10),DATEADD(SS,1,CRT_TIME),120),DATEADD(SS,1,CRT_TIME))/10)*10,CONVERT(VARCHAR(10),CRT_TIME,120)), 120) AS DT 
                                               FROM [JIEPAI].[DBO].[JP_TT] WHERE CONVERT(VARCHAR(100), CRT_TIME, 23)='{0}' AND LINE='{1}' AND STATION='{2}' 
                                           ) AS T GROUP BY STATION,DT ", date, lINE, station, numcount);
                            dt = OracleHelper.ExecuteDataTablesql(UserInfo.SqlConnectionString40, str1);
                            if (dt.Rows.Count > 0)
                            {
                                for (int k = 0; k < dt.Rows.Count; k++)
                                {
                                    bool bl = false;
                                    string tecount = dt.Rows[k][0].ToString();
                                    string testation = dt.Rows[k][1].ToString();
                                    string tetime = dt.Rows[k][2].ToString();
                                    int rowcount = 0;
                                    int clomcounts = 0;
                                    for (int l = 0; l < exidre1.Rows.Count; l++)
                                    {
                                        string barcode = exidre1.Rows[l][0].ToString();
                                        if (barcode == testation)
                                        {
                                            rowcount = l;
                                            bl = true;
                                        }
                                    }
                                    int num = 0;
                                    if (bl)
                                    {
                                        for (int l = 0; l < ListDate1.Count; l++)
                                        {
                                            if (tetime == ListDate1[l])
                                            {
                                                num = l + 1;
                                                if (!string.IsNullOrEmpty(exidre1.Rows[rowcount][num].ToString()))
                                                {
                                                    string value = exidre1.Rows[rowcount][num].ToString();
                                                    clomcounts = Convert.ToInt32(value);
                                                }
                                                clomcounts += Convert.ToInt32(tecount);
                                                exidre1.Rows[rowcount][num] = clomcounts;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    string strstation = station;
                    if (ismes)
                    {
                        List<string> ListWorkcodes1 = new List<string>();
                        str = string.Format("SELECT DISTINCT(T.WORKJOB_CODE)  FROM TODAYWORKJOB T    WHERE T.CREATEDATE = TRUNC(SYSDATE) AND T.TESTPOSITION ='" + station + "'  AND T.LINE_CODE='{0}'", lINE);
                        dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, str);
                        if (dt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                ListWorkcodes1.Add(dt.Rows[j][0].ToString());
                            }
                        }
                        for (int j = 0; j < ListWorkcodes1.Count; j++)
                        {
                            string mo = ListWorkcodes1[j].ToString();
                            string wo = mo.Replace("-", "_");
                            str = string.Format(@"SELECT COUNT(TMP.BARCODE) TOTALNUM,TMP.TESTPOSITION,TMP.NEWTIME
                                            FROM
                                            (SELECT  BARCODE, TESTPOSITION, --ID, 状态
                                                     TO_CHAR(SCAN_TIME, 'YYY-MM-DD HH24:MI:SS') OLDTIME, (CASE  FLOOR((TO_CHAR(SCAN_TIME, 'MI')) / 10)
                                                   WHEN  0 THEN TO_CHAR(SCAN_TIME, 'YYYY-MM-DD HH24') || ':00:00'
                                                   WHEN  1 THEN TO_CHAR(SCAN_TIME, 'YYYY-MM-DD HH24') || ':10:00'
                                                   WHEN  2 THEN TO_CHAR(SCAN_TIME, 'YYYY-MM-DD HH24') || ':20:00'
                                                   WHEN  3 THEN TO_CHAR(SCAN_TIME, 'YYYY-MM-DD HH24') || ':30:00'
                                                   WHEN  4 THEN TO_CHAR(SCAN_TIME, 'YYYY-MM-DD HH24') || ':40:00'
                                                   WHEN  5 THEN TO_CHAR(SCAN_TIME, 'YYYY-MM-DD HH24') || ':50:00'
                                             END)   AS NEWTIME
                                             FROM(SELECT BARCODE, SCAN_TIME, TESTPOSITION FROM {0} T  WHERE   SCAN_POSITION = 'F' AND TRUNC（T.SCAN_TIME) = TRUNC(SYSDATE)   AND T.TESTPOSITION = '{1}' AND T.LINE_CODE2 = '{2}'
                                                UNION  SELECT BARCODE,A.SCAN_TIME,A.TESTPOSITION   FROM REWORK_PRIMARY_RECORD A WHERE A.ATTEMPTER_CODE='{3}' AND  SCAN_POSITION='F' AND 
                                               TRUNC(A.SCAN_TIME)= TRUNC(SYSDATE) AND A.TESTPOSITION ='{1}' AND A.LINE_CODE2='{2}'  
                                            )) TMP
                                           GROUP BY TMP.NEWTIME,TMP.TESTPOSITION
                                           ORDER BY TMP.NEWTIME", wo, station, lINE, mo);
                            dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, str);
                            if (dt.Rows.Count > 0)
                            {
                                for (int k = 0; k < dt.Rows.Count; k++)
                                {
                                    bool bl = false;
                                    string tecount = dt.Rows[k][0].ToString();
                                    string testation = dt.Rows[k][1].ToString();
                                    string tetime = dt.Rows[k][2].ToString();
                                    int rowcount = 0;
                                    int clomcounts = 0;
                                    for (int l = 0; l < exidre1.Rows.Count; l++)
                                    {
                                        string barcode = exidre1.Rows[l][0].ToString();
                                        if (barcode == testation)
                                        {
                                            rowcount = l;
                                            bl = true;
                                        }
                                    }
                                    int num = 0;
                                    if (bl)
                                    {
                                        for (int l = 0; l < ListDate1.Count; l++)
                                        {
                                            if (tetime == ListDate1[l])
                                            {
                                                num = l + 1;
                                                if (!string.IsNullOrEmpty(exidre1.Rows[rowcount][num].ToString()))
                                                {
                                                    string value = exidre1.Rows[rowcount][num].ToString();
                                                    clomcounts = Convert.ToInt32(value);
                                                }
                                                clomcounts += Convert.ToInt32(tecount);
                                                exidre1.Rows[rowcount][num] = clomcounts;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string strselectsql = string.Format(@"SELECT COUNT(TMP.LB_ID) TOTALNUM,TMP.WP_ID,TMP.NEWTIME
                                FROM
                                (SELECT  LB_ID, WP_ID, --ID, 状态
                                       TO_CHAR(WP_CMP_DATE, 'YYY-MM-DD HH24:MI:SS') OLDTIME, (CASE  FLOOR((TO_CHAR(WP_CMP_DATE, 'MI')) / 10)
                                       WHEN  0 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':00:00'
                                       WHEN  1 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':10:00'
                                       WHEN  2 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':20:00'
                                       WHEN  3 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':30:00'
                                       WHEN  4 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':40:00'
                                       WHEN  5 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':50:00'
                                 END) AS NEWTIME
                                 FROM(SELECT WP_CMP_DATE, WP_ID, LB_ID FROM TB_PM_MO_LBWP T WHERE PL_ID = '{0}'  AND  WP_ID <> 'RE' AND WP_ID='{2}' AND  WP_CMP_DATE
                                 BETWEEN TO_DATE('{1} 00:00:00', 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE('{1} 23:59:59', 'YYYY/MM/DD HH24:MI:SS')) ) TMP
                                  GROUP BY TMP.NEWTIME,TMP.WP_ID
                                  ORDER BY TMP.NEWTIME", lINE, date, strstation);
                        dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselectsql);
                        if (dt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                string tecount = dt.Rows[j][0].ToString();
                                string testation = dt.Rows[j][1].ToString();
                                string tetime = dt.Rows[j][2].ToString();
                                int rowcount = 0;
                                int clomcounts = 0;
                                bool bl = false;
                                for (int k = 0; k < exidre1.Rows.Count; k++)
                                {
                                    string barcode = exidre1.Rows[k][0].ToString();
                                    if (barcode == testation)
                                    {
                                        bl = true;
                                        rowcount = k;
                                    }
                                }

                                if (bl)
                                {
                                    for (int k = 0; k < ListDate1.Count; k++)
                                    {
                                        if (tetime == ListDate1[k])
                                        {
                                            int num = k + 1;
                                            if (!string.IsNullOrEmpty(exidre1.Rows[rowcount][num].ToString()))
                                            {
                                                string value = exidre1.Rows[rowcount][num].ToString();
                                                clomcounts = Convert.ToInt32(value);
                                            }
                                            clomcounts += Convert.ToInt32(tecount);
                                            exidre1.Rows[rowcount][num] = clomcounts;
                                            break;
                                        }
                                    }
                                }

                            }
                        }
                    } 
                    if (strstation != "AOI2")
                    {
                        int numcount = AppHelper.Get_ODM_STATION_LINENUM(lINE, strstation);
                        //MES中找不到的工序作为设备TT
                        string strselectsql = "select name from qa_mantaince_type where name='" + strstation + "'";
                        System.Data.DataTable dtm = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselectsql);
                        if (dtm.Rows.Count > 0)
                        {
                        }
                        else
                        {
                            if (strstation == "电池压合")
                            {
                                strselectsql = string.Format(@"WITH TS AS(SELECT T.BATTERYSN BARCODE,T.CRT_TIME SCAN_TIME,'电池压合'TESTPOSITION FROM ODM_BATTERYPRESSING T WHERE   INSTR(PCNAME,'{1}') > 0  AND  CRT_TIME BETWEEN TO_DATE('{0} 00:00:00','YYYY-MM-DD HH24:MI:SS')
                                     AND TO_DATE('{0} 23:59:59','YYYY-MM-DD HH24:MI:SS'))
                                    SELECT TMP.TESTPOSITION,TMP.NEWTIME,COUNT(TMP.BARCODE) TOTALNUM FROM(SELECT  BARCODE,TESTPOSITION, -- ID,状态
                                         TO_CHAR(SCAN_TIME,'YYY-MM-DD HH24:MI:SS') OLDTIME,  (CASE  FLOOR((TO_CHAR(SCAN_TIME,'MI'))/10) 
                                       WHEN  0 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':00:00'
                                       WHEN  1 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':10:00'
                                       WHEN  2 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':20:00'
                                       WHEN  3 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':30:00'
                                       WHEN  4 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':40:00'
                                       WHEN  5 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':50:00'
                                 END)   AS NEWTIME 
                                 FROM TS) TMP
                                GROUP BY TMP.NEWTIME,TMP.TESTPOSITION
                                ORDER BY TMP.NEWTIME", date, lINE);
                                dtm = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselectsql);
                            }
                            else
                            {
                                strselectsql = string.Format(@"select STATION,DT,COUNT(*)*{3} NUM from
                                           (
                                               select STATION,CONVERT(varchar(100),dateadd(mi,(datediff(mi,convert(varchar(10),dateadd(ss,1,CRT_TIME),120),dateadd(ss,1,CRT_TIME))/10)*10,convert(varchar(10),CRT_TIME,120)), 120) as DT 
                                               from [jiepai].[dbo].[JP_TT] where CONVERT(varchar(100), CRT_TIME, 23)='{0}' and Line='{1}' and STATION='{2}' 
                                           ) AS T group by STATION,DT ", date, lINE, strstation, numcount);
                                dtm = OracleHelper.ExecuteDataTablesql(UserInfo.SqlConnectionString40, strselectsql);
                            }
                            if (dtm.Rows.Count > 0)
                            {
                                int rowcount = 0;
                                bool bl = false;
                                for (int k = 0; k < exidre1.Rows.Count; k++)
                                {
                                    string barcode = exidre1.Rows[k][0].ToString();
                                    if (barcode == strstation)
                                    {
                                        bl = true;
                                        rowcount = k;
                                    }
                                }
                                if (bl)
                                {
                                    for (int j = 0; j < dtm.Rows.Count; j++)
                                    {
                                        string tetime = dtm.Rows[j][1].ToString();
                                        for (int k = 0; k < ListDate1.Count; k++)
                                        {
                                            //int num = 0;
                                            if (tetime == ListDate1[k])
                                            { 
                                                exidre1.Rows[rowcount][k + 1] = dtm.Rows[j][2].ToString();
                                                break;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            } 
            for (int k1 = 0; k1 < ListDate1.Count; k1++)
            {
                int num = k1 + 1;
                for (int k2 = 0; k2 < exidre1.Rows.Count; k2++)
                {
                    string cloumname = date + " " + exidre1.Columns[num].ColumnName + ":00";
                    if (LisDTtime.Contains(cloumname))
                    {
                        exidre1.Rows[k2][num] = "";
                    } 
                } 
            }
            int dd = RC;
            double Tct = MalNb;
            //double Lct = minNb;
            //double Rct = maxNb;
            double Lct = maxNb;
            double Rct = minNb;
            int CNb = ListDate1.Count + 1;
            int CNaC = ListDate1.Count; 
            CNb = ListDate1.Count;
            if (CNaC == 0)
            {
                CNaC = 0;
            }
            else
            {
                CNaC -= 1;
            } 
            for (int i = 0; i < exidre1.Rows.Count; i++)
            {
                int RNb = 0;
                int YNb = 0;
                double VNb = 0;
                for (int j = 1; j < CNb; j++)
                {
                    var Rval = exidre1.Rows[i][j].ToString();
                    double Vct = Rval == "" ? 0 : Convert.ToDouble(Rval);
                    if (Rct > Vct)
                    {
                        RNb++;
                    }
                    if (Vct > Lct)
                    {
                        YNb++;
                    }
                }
                if (CNaC == 0)
                {
                    VNb = 0;
                }
                else
                {
                    VNb = Math.Round(Convert.ToDouble(CNaC - RNb - YNb) / (CNaC - RC), 4);
                }
                exidre1.Rows[i][ListDate.Count + 1] = RNb - RC;
                exidre1.Rows[i][ListDate.Count + 2] = YNb;
                exidre1.Rows[i][ListDate.Count + 3] = Math.Round(VNb * 100, 2) + "%";
            }
         
            double cc = 0;
            List<TIME_VIEW> LisView = new List<TIME_VIEW>();
            for (int i = 0; i < exidre1.Rows.Count; i++)
            {
                var value = exidre1.Rows[i][ListDate.Count + 3].ToString();
                double a = value == "" ? 0 : Convert.ToDouble(value.Replace("%", ""));
                cc += a;
                TIME_VIEW tIME_VIEW = new TIME_VIEW();
                tIME_VIEW.station = exidre1.Rows[i][0].ToString(); 
                tIME_VIEW.T1 = AppHelper.REVL1(CNaC,0, minNb, maxNb, MtpNb, exidre1.Rows[i][1].ToString());
                tIME_VIEW.T2 = AppHelper.REVL1(CNaC, 1, minNb, maxNb, MtpNb, exidre1.Rows[i][2].ToString());
                tIME_VIEW.T3 = AppHelper.REVL1(CNaC, 2, minNb, maxNb, MtpNb, exidre1.Rows[i][3].ToString());
                tIME_VIEW.T4 = AppHelper.REVL1(CNaC, 3, minNb, maxNb, MtpNb, exidre1.Rows[i][4].ToString());
                tIME_VIEW.T5 = AppHelper.REVL1(CNaC, 4, minNb, maxNb, MtpNb, exidre1.Rows[i][5].ToString());
                tIME_VIEW.T6 = AppHelper.REVL1(CNaC, 5, minNb, maxNb, MtpNb, exidre1.Rows[i][6].ToString());
                tIME_VIEW.T7 = AppHelper.REVL1(CNaC, 6, minNb, maxNb, MtpNb, exidre1.Rows[i][7].ToString());
                tIME_VIEW.T8 = AppHelper.REVL1(CNaC, 7, minNb, maxNb, MtpNb, exidre1.Rows[i][8].ToString());
                tIME_VIEW.T9 = AppHelper.REVL1(CNaC, 8, minNb, maxNb, MtpNb, exidre1.Rows[i][9].ToString());
                tIME_VIEW.T10 = AppHelper.REVL1(CNaC, 9, minNb, maxNb, MtpNb, exidre1.Rows[i][10].ToString());
                tIME_VIEW.T11 = AppHelper.REVL1(CNaC, 10, minNb, maxNb, MtpNb, exidre1.Rows[i][11].ToString());
                tIME_VIEW.T12 = AppHelper.REVL1(CNaC, 11, minNb, maxNb, MtpNb, exidre1.Rows[i][12].ToString());
                tIME_VIEW.T13 = AppHelper.REVL1(CNaC, 12, minNb, maxNb, MtpNb, exidre1.Rows[i][13].ToString());
                tIME_VIEW.T14 = AppHelper.REVL1(CNaC, 13, minNb, maxNb, MtpNb, exidre1.Rows[i][14].ToString());
                tIME_VIEW.T15 = AppHelper.REVL1(CNaC, 14, minNb, maxNb, MtpNb, exidre1.Rows[i][15].ToString());
                tIME_VIEW.T16 = AppHelper.REVL1(CNaC, 15, minNb, maxNb, MtpNb, exidre1.Rows[i][16].ToString());
                tIME_VIEW.T17 = AppHelper.REVL1(CNaC, 16, minNb, maxNb, MtpNb, exidre1.Rows[i][17].ToString());
                tIME_VIEW.T18 = AppHelper.REVL1(CNaC, 17, minNb, maxNb, MtpNb, exidre1.Rows[i][18].ToString());
                tIME_VIEW.T19 = AppHelper.REVL1(CNaC, 18, minNb, maxNb, MtpNb, exidre1.Rows[i][19].ToString());
                tIME_VIEW.T20 = AppHelper.REVL1(CNaC, 19, minNb, maxNb, MtpNb, exidre1.Rows[i][20].ToString());
                tIME_VIEW.T21 = AppHelper.REVL1(CNaC, 20, minNb, maxNb, MtpNb, exidre1.Rows[i][21].ToString());
                tIME_VIEW.T22 = AppHelper.REVL1(CNaC, 21, minNb, maxNb, MtpNb, exidre1.Rows[i][22].ToString());
                tIME_VIEW.T23 = AppHelper.REVL1(CNaC, 22, minNb, maxNb, MtpNb, exidre1.Rows[i][23].ToString());
                tIME_VIEW.T24 = AppHelper.REVL1(CNaC, 23, minNb, maxNb, MtpNb, exidre1.Rows[i][24].ToString());
                //tIME_VIEW.T25 = exidre1.Rows[i][25].ToString();
                tIME_VIEW.F1 = exidre1.Rows[i][exidre1.Columns.Count - 3].ToString();
                tIME_VIEW.F2 = exidre1.Rows[i][exidre1.Columns.Count - 2].ToString();
                tIME_VIEW.F3 = exidre1.Rows[i][exidre1.Columns.Count - 1].ToString();
                LisView.Add(tIME_VIEW);
            }
            List<TABLE_VIEW> ListABLE_VIEWs = new List<TABLE_VIEW>();
            cellStyle cellStyle = new cellStyle();
            cellStyle.css = "{\"background - color\":\"red\"}";
            for (int i = 0; i < exidre1.Columns.Count; i++)
            {
                TABLE_VIEW tABLE_VIEW = new TABLE_VIEW();
                string d = exidre1.Columns[i].ColumnName.Trim();
                tABLE_VIEW.align = "center";
                tABLE_VIEW.field = d;
                tABLE_VIEW.title = d;
                tABLE_VIEW.width = 80;
                tABLE_VIEW.cellStyle = cellStyle; 
                ListABLE_VIEWs.Add(tABLE_VIEW);

            }
            string Value = "";
            //string Value = AppHelper.DataTableToJson(exidre1);
            double c1 = Math.Round(cc / exidre1.Rows.Count, 2); 
            string Ave = c1 + "%";

            //不良数据---
            List<string> ListWorkcode1 = new List<string>();
            sql = " SELECT  NAME FROM ODM_STATIONFTY T  WHERE CODE='" + cODE + "' ORDER BY CODEFUNCTION";
            string testsql = sql;
            table = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string station = table.Rows[i][0].ToString();
                    if (station == "贴片")
                    {
                        station = "AOI2";
                    }
                    Liststation1.Add(station);
                }
            }
            sql = string.Format("SELECT DISTINCT(T.WORKJOB_CODE)  FROM TODAYWORKJOB T  LEFT OUTER JOIN WORK_WORKJOB K " +
              "ON K.WORKJOB_CODE=T.WORKJOB_CODE WHERE T.CREATEDATE = TO_DATE('{0}','YYYY-MM-DD') AND T.TESTPOSITION IN(" + sql + ") AND K.CLIENTCODE='01' AND T.LINE_CODE='{1}'", date, lINE);
            table = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (table.Rows.Count > 0)
            {
                //统计工序列表
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    ListWorkcode1.Add(table.Rows[i][0].ToString());
                }
            }

            DataTable exidre2 = AppHelper.GetRETableSchema(LisDTtime, Hour);
            for (int i = 0; i < Liststation1.Count; i++)
            {
                string station = Liststation1[i];
                DataRow drq = exidre2.NewRow();
                drq[0] = station;
                for (int j = 0; j < ListDate.Count; j++)
                {
                    string date1 = ListDate[j];
                    drq[j + 1] = "";
                }
                exidre2.Rows.Add(drq);
            }
            for (int k1 = 0; k1 < ListDate1.Count; k1++)
            {
                int num = k1 + 1;
                for (int k2 = 0; k2 < exidre2.Rows.Count; k2++)
                {
                    exidre2.Rows[k2][num] = "0";
                }
            }
            if (ismes)
            {
                List<string> lisnumber = new List<string>();
                string Stringsqla = string.Format(@"SELECT COUNT(TMP.BARCODE) TOTALNUM,TMP.TESTPOSITION,TMP.NEWTIME
                            FROM 
                            (SELECT  A.BARCODE,A.TESTPOSITION, -- ID,状态
                                        TO_CHAR(A.SCAN_TIME,'YYY-MM-DD HH24:MI:SS') OLDTIME,  (CASE  FLOOR((TO_CHAR(SCAN_TIME,'MI'))/10) 
                                    WHEN  0 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':00:00'
                                    WHEN  1 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':10:00'
                                    WHEN  2 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':20:00'
                                    WHEN  3 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':30:00'
                                    WHEN  4 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':40:00'
                                    WHEN  5 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':50:00'
                                END)   AS NEWTIME 
                                FROM(
                                SELECT TESTPOSITION, LINE_CODE2, BARCODE,SCAN_TIME,RN FROM (
                                                                SELECT TESTPOSITION, LINE_CODE2, BARCODE,SCAN_TIME, ROW_NUMBER() OVER(PARTITION BY BARCODE || TESTPOSITION ORDER BY BARCODE || TESTPOSITION) RN FROM(
                                                                 SELECT A.BARCODE, A.TESTPOSITION, A.LINE_CODE2,SCAN_TIME FROM NTF_FAIL A
                                                                 LEFT OUTER JOIN (SELECT T.WORKJOB_CODE , T.TESTPOSITION, T.LINE_CODE FROM TODAYWORKJOB T
                                                                 LEFT OUTER JOIN WORK_WORKJOB K ON K.WORKJOB_CODE= T.WORKJOB_CODE WHERE T.CREATEDATE = TO_DATE('{0}', 'YYYY-MM-DD')  AND K.CLIENTCODE= '01' AND T.LINE_CODE= '{1}') B ON A.TESTPOSITION = B.TESTPOSITION
                                                                 AND A.ATTEMPTER_CODE = B.WORKJOB_CODE
                                                                 WHERE A.SCAN_POSITION = 'L' AND TO_CHAR（A.SCAN_TIME,'YYYY-MM-DD')='{0}'
                                                                 AND A.LINE_CODE2 = '{1}'
                                                                 UNION
                                                                SELECT S.REPAIR_CARD_CODE BARCODE,NVL((SELECT STANEW FROM ODM_BEATE_STATION WHERE STAOLD=S.GONGWEI AND ROWNUM=1),S.POSTION_ITEMVERSION) TESTPOSITION,S.LINE_CODE LINE_CODE2,SENDDATE SCAN_TIME FROM QA_IPQC_SUBREPAIR S  WHERE   TO_CHAR（S.SENDDATE,'YYYY-MM-DD')='{0}'  AND CLIENTCODE = '01'
                                                                AND S.LINE_CODE = '{1}'))WHERE RN=1) A) TMP
                            GROUP BY TMP.NEWTIME,TMP.TESTPOSITION
                            ORDER BY TMP.NEWTIME", date, lINE, testsql);
                if (true)
                {
                    Stringsqla = string.Format(@"SELECT COUNT(TMP.BARCODE) TOTALNUM,TMP.TESTPOSITION,TMP.NEWTIME
                            FROM 
                            (SELECT  A.BARCODE,A.TESTPOSITION, -- ID,状态
                                        TO_CHAR(A.SCAN_TIME,'YYY-MM-DD HH24:MI:SS') OLDTIME,  (CASE  FLOOR((TO_CHAR(SCAN_TIME,'MI'))/10) 
                                    WHEN  0 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':00:00'
                                    WHEN  1 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':10:00'
                                    WHEN  2 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':20:00'
                                    WHEN  3 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':30:00'
                                    WHEN  4 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':40:00'
                                    WHEN  5 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':50:00'
                                END)   AS NEWTIME 
                                FROM(
                                SELECT TESTPOSITION, LINE_CODE2, BARCODE,SCAN_TIME,RN FROM (
                                                                SELECT TESTPOSITION, LINE_CODE2, BARCODE,SCAN_TIME, ROW_NUMBER() OVER(PARTITION BY BARCODE || TESTPOSITION ORDER BY BARCODE || TESTPOSITION) RN FROM(
                                                                SELECT S.REPAIR_CARD_CODE BARCODE,NVL((SELECT STANEW FROM ODM_BEATE_STATION WHERE STAOLD=S.GONGWEI AND ROWNUM=1),S.POSTION_ITEMVERSION) TESTPOSITION,S.LINE_CODE LINE_CODE2,SENDDATE SCAN_TIME FROM QA_IPQC_SUBREPAIR S  WHERE   TO_CHAR（S.SENDDATE,'YYYY-MM-DD')='{0}'  AND CLIENTCODE = '01'
                                                                AND S.LINE_CODE = '{1}'))WHERE RN=1) A) TMP
                            GROUP BY TMP.NEWTIME,TMP.TESTPOSITION
                            ORDER BY TMP.NEWTIME", date, lINE, testsql);
                }
                DataTable dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, Stringsqla); 
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        string tecount = dt.Rows[j][0].ToString();
                        string testation = dt.Rows[j][1].ToString();
                        string tetime = dt.Rows[j][2].ToString();
                        int rowcount = 0;
                        int clomcounts = 0;
                        bool bl = false;
                        for (int k = 0; k < exidre2.Rows.Count; k++)
                        {
                            string barcode = exidre2.Rows[k][0].ToString();
                            if (barcode == testation)
                            {
                                rowcount = k;
                                bl = true;
                            }
                        }
                        int num = 0;
                        if (bl)
                        {
                            for (int k = 0; k < ListDate1.Count; k++)
                            {
                                if (tetime == ListDate1[k])
                                {
                                    num = k + 1;
                                    if (!string.IsNullOrEmpty(exidre2.Rows[rowcount][num].ToString()))
                                    {
                                        string value = exidre2.Rows[rowcount][num].ToString();
                                        clomcounts = Convert.ToInt32(value);
                                    }
                                    clomcounts += Convert.ToInt32(tecount);
                                    exidre2.Rows[rowcount][num] = clomcounts;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                const string Format = @"SELECT COUNT(TMP.LB_ID) TOTALNUM,TMP.WP_ID,TMP.NEWTIME FROM  (SELECT  LB_ID,WP_ID,TO_CHAR(SYS_CRT_DATE,'YYY-MM-DD HH24:MI:SS') OLDTIME, 
                 (CASE  FLOOR((TO_CHAR(SYS_CRT_DATE,'MI'))/10) 
                  WHEN  0 THEN TO_CHAR(SYS_CRT_DATE,'YYYY-MM-DD HH24')||':00:00'
                  WHEN  1 THEN TO_CHAR(SYS_CRT_DATE,'YYYY-MM-DD HH24')||':10:00'
                  WHEN  2 THEN TO_CHAR(SYS_CRT_DATE,'YYYY-MM-DD HH24')||':20:00'
                  WHEN  3 THEN TO_CHAR(SYS_CRT_DATE,'YYYY-MM-DD HH24')||':30:00'
                  WHEN  4 THEN TO_CHAR(SYS_CRT_DATE,'YYYY-MM-DD HH24')||':40:00'
                  WHEN  5 THEN TO_CHAR(SYS_CRT_DATE,'YYYY-MM-DD HH24')||':50:00'
                  END)   AS NEWTIME   FROM(
                SELECT  D.WP_ID,D.LB_ID,D.SYS_CRT_DATE  FROM  TB_PM_MO_LB C ,TB_ENOK_TEST_NG_TIME  D WHERE  C.LB_ID=D.LB_ID AND (D.NG_TIME = 1 OR
                (D.NG_TIME = 2 AND D.IS_CC = 'N')) AND D.SYS_CRT_DATE BETWEEN TO_DATE('{0} 00:00:00', 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE('{0} 23:59:59', 'YYYY/MM/DD HH24:MI:SS')    AND C.PL_ID='{1}'                                   
                UNION                               
                SELECT  D.WP_ID,D.LB_ID,D.SYS_CRT_DATE  FROM TB_PM_QC_HD D WHERE    D.SYS_CRT_DATE BETWEEN   TO_DATE('{0} 00:00:00', 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE('{0} 23:59:59', 'YYYY/MM/DD HH24:MI:SS') AND D.PL_ID='{1}')    ) TMP
                                                GROUP BY TMP.NEWTIME,TMP.WP_ID
                                                ORDER BY TMP.NEWTIME  ";
                string strselect = string.Format(Format, date, lINE);
                DataTable dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselect);
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        string tecount = dt.Rows[j][0].ToString();
                        string testation = dt.Rows[j][1].ToString();
                        string tetime = dt.Rows[j][2].ToString();
                        int rowcount = 0;
                        int clomcounts = 0;
                        bool bl = false;
                        for (int k = 0; k < exidre2.Rows.Count; k++)
                        {
                            string barcode = exidre2.Rows[k][0].ToString();
                            if (barcode == testation)
                            {
                                rowcount = k;
                                bl = true;
                            }
                        }
                        int num = 0;
                        if (bl)
                        { 
                            for (int k = 0; k < ListDate1.Count; k++)
                            {
                                if (tetime == ListDate1[k])
                                {
                                    num = k + 1;
                                    if (!string.IsNullOrEmpty(exidre2.Rows[rowcount][num].ToString()))
                                    {
                                        string value = exidre2.Rows[rowcount][num].ToString();
                                        clomcounts = Convert.ToInt32(value);
                                    }
                                    clomcounts += Convert.ToInt32(tecount);
                                    exidre2.Rows[rowcount][num] = clomcounts;
                                    break;
                                }
                            }
                        }
                    }
                }
            } 
            for (int i = 0; i < Liststation1.Count; i++)
            {
                string testation = Liststation1[i];
                int rowcount = 0;
                int clomcounts = 0;
                bool bl = false;
                for (int k = 0; k < exidre2.Rows.Count; k++)
                {
                    string barcode = exidre2.Rows[k][0].ToString();
                    if (barcode == testation)
                    {
                        bl = true;
                        rowcount = k;
                    }
                }
                if (bl)
                {
                    if (AppHelper.IsODM_DEFECTRECORD_EXT(date, lINE, testation))
                    {
                        DataTable ddd = AppHelper.DT_ODM_DEFECTRECORD_EXT(date, lINE, testation);
                        if (ddd.Rows.Count > 0)
                        {
                            for (int j = 0; j < ddd.Rows.Count; j++)
                            {
                                string tecount = ddd.Rows[j][0].ToString();
                                string tetime = ddd.Rows[j][2].ToString();
                                int num = 0;
                                for (int k = 0; k < ListDate1.Count; k++)
                                {
                                    if (tetime == ListDate1[k])
                                    {
                                        num = k + 1;
                                        if (!string.IsNullOrEmpty(exidre2.Rows[rowcount][num].ToString()))
                                        {
                                            string value = exidre2.Rows[rowcount][num].ToString();
                                            clomcounts = Convert.ToInt32(value);
                                        }
                                        clomcounts += Convert.ToInt32(tecount);
                                        exidre2.Rows[rowcount][num] = clomcounts;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (AppHelper.IsODM_SPI_AOI_LOG(date, lINE, testation))
                    {
                        DataTable ddd = AppHelper.DT_ODM_SPI_AOI_LOG(date, lINE, testation);
                        if (ddd.Rows.Count > 0)
                        {
                            for (int j = 0; j < ddd.Rows.Count; j++)
                            {
                                string tecount = ddd.Rows[j][0].ToString();
                                string tetime = ddd.Rows[j][2].ToString();
                                int num = 0;
                                for (int k = 0; k < ListDate1.Count; k++)
                                {
                                    if (tetime == ListDate1[k])
                                    {
                                        num = k + 1;
                                        if (!string.IsNullOrEmpty(exidre2.Rows[rowcount][num].ToString()))
                                        {
                                            string value = exidre2.Rows[rowcount][num].ToString();
                                            clomcounts = Convert.ToInt32(value);
                                        }
                                        clomcounts += Convert.ToInt32(tecount);
                                        exidre2.Rows[rowcount][num] = clomcounts;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            double allct = 0;
            double aFtyct = 1;
            double aFtyct1 = 1;
            for (int i = 0; i < exidre2.Rows.Count; i++)
            {
                double VNb = 0;
                for (int j = 1; j < CNb; j++)
                {
                    var Rval = exidre2.Rows[i][j].ToString();
                    double Vct = Rval == "" ? 0 : Convert.ToDouble(Rval);
                    VNb += Vct;
                }
                allct += VNb;
                double vFpy = Math.Round((1 - VNb / (Tct * 24)) * 100, 2);
                double vFpy1 = Math.Round((1 - VNb / (Tct * 24)), 4);
                exidre2.Rows[i][ListDate.Count + 2] = VNb;
                exidre2.Rows[i][ListDate.Count + 3] = vFpy + "%";
                aFtyct = Math.Round(vFpy1 * aFtyct, 4);
            }
            aFtyct1 = Math.Round(aFtyct * 100, 2);
            string Fty = (aFtyct1).ToString() + "%";
            string TrePairct = allct.ToString();
            for (int k1 = 0; k1 < ListDate1.Count; k1++)
            {
                int num = k1 + 1;
                for (int k2 = 0; k2 < exidre2.Rows.Count; k2++)
                {
                    string cloumname = date + " " + exidre2.Columns[num].ColumnName + ":00";
                    if (LisDTtime.Contains(cloumname))
                    {
                        exidre2.Rows[k2][num] = "";
                    }
                }
            }
            List<TIME_VIEW> LisViewRe = new List<TIME_VIEW>();
            for (int i = 0; i < exidre2.Rows.Count; i++)
            {
                TIME_VIEW tIME_VIEW = new TIME_VIEW();
                tIME_VIEW.station = exidre2.Rows[i][0].ToString();
                tIME_VIEW.T1 = AppHelper.REVL2(CNaC,0, exidre2.Rows[i][1].ToString());
                tIME_VIEW.T2 = AppHelper.REVL2(CNaC,1, exidre2.Rows[i][2].ToString());
                tIME_VIEW.T3 = AppHelper.REVL2(CNaC,2, exidre2.Rows[i][3].ToString());
                tIME_VIEW.T4 = AppHelper.REVL2(CNaC,3, exidre2.Rows[i][4].ToString());
                tIME_VIEW.T5 = AppHelper.REVL2(CNaC,4, exidre2.Rows[i][5].ToString());
                tIME_VIEW.T6 = AppHelper.REVL2(CNaC,5, exidre2.Rows[i][6].ToString());
                tIME_VIEW.T7 = AppHelper.REVL2(CNaC,6, exidre2.Rows[i][7].ToString());
                tIME_VIEW.T8 = AppHelper.REVL2(CNaC,7, exidre2.Rows[i][8].ToString());
                tIME_VIEW.T9 = AppHelper.REVL2(CNaC,8, exidre2.Rows[i][9].ToString());
                tIME_VIEW.T10 = AppHelper.REVL2(CNaC,9, exidre2.Rows[i][10].ToString());
                tIME_VIEW.T11 = AppHelper.REVL2(CNaC,10, exidre2.Rows[i][11].ToString());
                tIME_VIEW.T12 = AppHelper.REVL2(CNaC,11, exidre2.Rows[i][12].ToString());
                tIME_VIEW.T13 = AppHelper.REVL2(CNaC,12, exidre2.Rows[i][13].ToString());
                tIME_VIEW.T14 = AppHelper.REVL2(CNaC,13, exidre2.Rows[i][14].ToString());
                tIME_VIEW.T15 = AppHelper.REVL2(CNaC,14, exidre2.Rows[i][15].ToString());
                tIME_VIEW.T16 = AppHelper.REVL2(CNaC,15, exidre2.Rows[i][16].ToString());
                tIME_VIEW.T17 = AppHelper.REVL2(CNaC,16, exidre2.Rows[i][17].ToString());
                tIME_VIEW.T18 = AppHelper.REVL2(CNaC,17, exidre2.Rows[i][18].ToString());
                tIME_VIEW.T19 = AppHelper.REVL2(CNaC,18, exidre2.Rows[i][19].ToString());
                tIME_VIEW.T20 = AppHelper.REVL2(CNaC,19, exidre2.Rows[i][20].ToString());
                tIME_VIEW.T21 = AppHelper.REVL2(CNaC,20, exidre2.Rows[i][21].ToString());
                tIME_VIEW.T22 = AppHelper.REVL2(CNaC,21, exidre2.Rows[i][22].ToString());
                tIME_VIEW.T23 = AppHelper.REVL2(CNaC,22, exidre2.Rows[i][23].ToString());
                tIME_VIEW.T24 = AppHelper.REVL2(CNaC,23, exidre2.Rows[i][24].ToString());
                tIME_VIEW.F1 = exidre2.Rows[i][exidre2.Columns.Count - 3].ToString();
                tIME_VIEW.F2 = exidre2.Rows[i][exidre2.Columns.Count - 2].ToString();
                tIME_VIEW.F3 = exidre2.Rows[i][exidre2.Columns.Count - 1].ToString();
                LisViewRe.Add(tIME_VIEW);
            }
            //DT损失
            List<string> Liststation2 = new List<string>();
            List<string> ListWorkcode2 = new List<string>();
            string testsql1 = " SELECT  NAME FROM ODM_STATION T  WHERE CODE='" + cODE + "' ORDER BY CODEFUNCTION";
            table = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, testsql1);
            if (table.Rows.Count > 0)
            {  //统计工序列表
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string station = table.Rows[i][0].ToString();
                    if (station == "贴片")
                    {
                        station = "AOI2";
                    }
                    Liststation2.Add(station);
                }
            }
            testsql1 = " SELECT  NAME FROM ODM_STATION T  WHERE CODE='" + cODE + "'";
            testsql = testsql1;
            str = string.Format("SELECT DISTINCT(T.WORKJOB_CODE)  FROM TODAYWORKJOB T  LEFT OUTER JOIN WORK_WORKJOB K " +
            "ON K.WORKJOB_CODE=T.WORKJOB_CODE WHERE T.CREATEDATE = TRUNC(SYSDATE) AND T.TESTPOSITION IN(" + testsql1 + ") AND K.CLIENTCODE='01' AND T.LINE_CODE='{0}'", lINE);
            table = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, str);
            if (table.Rows.Count > 0)
            {
                //统计工序列表
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    ListWorkcode2.Add(table.Rows[i][0].ToString());
                }
            }
            DataTable  exidre3 = AppHelper.GetRETableDt(LisDTtime, Hour);
            DataTable exidre4 = AppHelper.GetRETableDt(LisDTtime, Hour);
            for (int i = 0; i < Liststation2.Count; i++)
            {
                string station = Liststation2[i];
                DataRow drq = exidre3.NewRow();
                DataRow drq1 = exidre4.NewRow();
                drq[0] = station;
                drq1[0] = station;
                for (int j = 0; j < ListDate.Count; j++)
                {
                    string date1 = ListDate[j];
                    drq[j + 1] = "";
                    drq1[j + 1] = "";
                }
                exidre3.Rows.Add(drq);
                exidre4.Rows.Add(drq1);
            }
            for (int k1 = 0; k1 < ListDate1.Count; k1++)
            {
                int num = k1 + 1;
                for (int k2 = 0; k2 < exidre3.Rows.Count; k2++)
                {
                    exidre3.Rows[k2][num] = "0";
                    exidre4.Rows[k2][num] = "0";
                }
            }
            if (ismes)
            {
                for (int i = 0; i < ListWorkcode2.Count; i++)
                {
                    string mo = ListWorkcode2[i].ToString();
                    string wo = mo.Replace("-", "_");
                    string Stringsqla = string.Format(@"SELECT COUNT(TMP.BARCODE) TOTALNUM,TMP.TESTPOSITION,TMP.NEWTIME
                                FROM 
                                (SELECT  BARCODE,TESTPOSITION, -- ID,状态
                                         TO_CHAR(SCAN_TIME,'YYY-MM-DD HH24:MI:SS') OLDTIME,  (CASE  FLOOR((TO_CHAR(SCAN_TIME,'MI'))/10) 
                                       WHEN  0 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':00:00'
                                       WHEN  1 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':10:00'
                                       WHEN  2 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':20:00'
                                       WHEN  3 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':30:00'
                                       WHEN  4 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':40:00'
                                       WHEN  5 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':50:00'
                                 END)   AS NEWTIME 
                                 FROM(SELECT BARCODE,SCAN_TIME,TESTPOSITION FROM {0} T  WHERE   SCAN_POSITION='F' AND TO_CHAR（T.SCAN_TIME,'YYYY-MM-DD')='{2}'   AND T.TESTPOSITION IN({4})  AND T.LINE_CODE2='{3}'
                                UNION  SELECT BARCODE,A.SCAN_TIME,A.TESTPOSITION   FROM REWORK_PRIMARY_RECORD A WHERE A.ATTEMPTER_CODE='{1}' AND  SCAN_POSITION='F' AND 
                                TO_CHAR（A.SCAN_TIME,'YYYY-MM-DD')='{2}'   AND A.TESTPOSITION IN({4}) AND A.LINE_CODE2='{3}'
                                )) TMP
                                GROUP BY TMP.NEWTIME,TMP.TESTPOSITION
                                ORDER BY TMP.NEWTIME", wo, mo, date, lINE, testsql);
                     DataTable   dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, Stringsqla);
                    if (dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            bool bl = false;
                            string tecount = dt.Rows[j][0].ToString();
                            string testation = dt.Rows[j][1].ToString();
                            string tetime = dt.Rows[j][2].ToString();
                            int rowcount = 0;
                            int clomcounts = 0;
                            for (int k = 0; k < exidre3.Rows.Count; k++)
                            {
                                string barcode = exidre3.Rows[k][0].ToString();
                                if (barcode == testation)
                                {
                                    rowcount = k;
                                    bl = true;
                                }
                            }
                            int num = 0;
                            if (bl)
                            {
                                for (int k = 0; k < ListDate1.Count; k++)
                                {
                                    if (tetime == ListDate1[k])
                                    {
                                        num = k + 1;
                                        if (!string.IsNullOrEmpty(exidre3.Rows[rowcount][num].ToString()))
                                        {
                                            string value = exidre3.Rows[rowcount][num].ToString();
                                            clomcounts = Convert.ToInt32(value);
                                        }
                                        clomcounts += Convert.ToInt32(tecount);
                                        exidre3.Rows[rowcount][num] = clomcounts;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                string strselectsql = string.Format(@"SELECT COUNT(TMP.LB_ID) TOTALNUM,TMP.WP_ID,TMP.NEWTIME
                                FROM
                                (SELECT  LB_ID, WP_ID, --ID, 状态
                                       TO_CHAR(WP_CMP_DATE, 'YYY-MM-DD HH24:MI:SS') OLDTIME, (CASE  FLOOR((TO_CHAR(WP_CMP_DATE, 'MI')) / 10)
                                       WHEN  0 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':00:00'
                                       WHEN  1 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':10:00'
                                       WHEN  2 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':20:00'
                                       WHEN  3 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':30:00'
                                       WHEN  4 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':40:00'
                                       WHEN  5 THEN TO_CHAR(WP_CMP_DATE, 'YYYY-MM-DD HH24') || ':50:00'
                                 END) AS NEWTIME
                                 FROM(SELECT WP_CMP_DATE, WP_ID, LB_ID FROM TB_PM_MO_LBWP T WHERE PL_ID = '{0}'  AND  WP_ID <> 'RE' AND  WP_CMP_DATE
                                 BETWEEN TO_DATE('{1} 00:00:00', 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE('{1} 23:59:59', 'YYYY/MM/DD HH24:MI:SS')) ) TMP
                                  GROUP BY TMP.NEWTIME,TMP.WP_ID
                                  ORDER BY TMP.NEWTIME", lINE, date);
                DataTable dt = OracleHelper.ExecuteDataTable1(UserInfo.OracleConnectionStringIms, strselectsql);
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        string tecount = dt.Rows[j][0].ToString();
                        string testation = dt.Rows[j][1].ToString();
                        string tetime = dt.Rows[j][2].ToString();
                        int rowcount = 0;
                        int clomcounts = 0;
                        bool bl = false;
                        for (int k = 0; k < exidre3.Rows.Count; k++)
                        {
                            string barcode = exidre3.Rows[k][0].ToString();
                            if (barcode == testation)
                            {
                                bl = true;
                                rowcount = k;
                            }
                        }
                        int num = 0;
                        if (bl)
                        {

                            for (int k = 0; k < ListDate1.Count; k++)
                            {
                                if (tetime == ListDate1[k])
                                {
                                    num = k + 1;
                                    if (!string.IsNullOrEmpty(exidre3.Rows[rowcount][num].ToString()))
                                    {
                                        string value = exidre3.Rows[rowcount][num].ToString();
                                        clomcounts = Convert.ToInt32(value);
                                    }
                                    clomcounts += Convert.ToInt32(tecount);
                                    exidre3.Rows[rowcount][num] = clomcounts;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            //给非MES工序赋值---
            for (int i = 0; i < Liststation2.Count; i++)
            {
                string strstation = Liststation2[i];
                if (strstation != "AOI2")
                {
                    int numcount =AppHelper.Get_ODM_STATION_LINENUM(lINE, strstation);
                    //string s22 = "";
                    //MES中找不到的工序作为设备TT
                    string strselectsql = "SELECT NAME FROM QA_MANTAINCE_TYPE WHERE NAME='" + strstation + "'";
                    DataTable dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strselectsql);
                    if (dtm.Rows.Count > 0)
                    {
                        //s22 = "11";
                    }
                    else
                    {
                        if (strstation == "电池压合")
                        {
                            strselectsql = string.Format(@"WITH TS AS(SELECT T.BATTERYSN BARCODE,T.CRT_TIME SCAN_TIME,'电池压合'TESTPOSITION FROM ODM_BATTERYPRESSING T WHERE   INSTR(PCNAME,'{1}') > 0  AND  CRT_TIME BETWEEN TO_DATE('{0} 00:00:00','YYYY-MM-DD HH24:MI:SS')
                                     AND TO_DATE('{0} 23:59:59','YYYY-MM-DD HH24:MI:SS'))
                                    SELECT TMP.TESTPOSITION,TMP.NEWTIME,COUNT(TMP.BARCODE) TOTALNUM FROM(SELECT  BARCODE,TESTPOSITION, -- ID,状态
                                         TO_CHAR(SCAN_TIME,'YYY-MM-DD HH24:MI:SS') OLDTIME,  (CASE  FLOOR((TO_CHAR(SCAN_TIME,'MI'))/10) 
                                       WHEN  0 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':00:00'
                                       WHEN  1 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':10:00'
                                       WHEN  2 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':20:00'
                                       WHEN  3 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':30:00'
                                       WHEN  4 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':40:00'
                                       WHEN  5 THEN TO_CHAR(SCAN_TIME,'YYYY-MM-DD HH24')||':50:00'
                                 END)   AS NEWTIME 
                                 FROM TS) TMP
                                GROUP BY TMP.NEWTIME,TMP.TESTPOSITION
                                ORDER BY TMP.NEWTIME", date, lINE);
                            dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strselectsql);
                        }
                        else
                        {
                            strselectsql = string.Format(@"SELECT STATION,DT,COUNT(*)*{3} NUM FROM
                                           (
                                               SELECT STATION,CONVERT(VARCHAR(100),DATEADD(MI,(DATEDIFF(MI,CONVERT(VARCHAR(10),DATEADD(SS,1,CRT_TIME),120),DATEADD(SS,1,CRT_TIME))/10)*10,CONVERT(VARCHAR(10),CRT_TIME,120)), 120) AS DT 
                                               FROM [JIEPAI].[DBO].[JP_TT] WHERE CONVERT(VARCHAR(100), CRT_TIME, 23)='{0}' AND LINE='{1}' AND STATION='{2}' 
                                           ) AS T GROUP BY STATION,DT ", date, lINE, strstation, numcount);
                            dtm = OracleHelper.ExecuteDataTablesql(UserInfo.SqlConnectionString40, strselectsql);
                        }
                        if (dtm.Rows.Count > 0)
                        {
                            int rowcount = 0;
                            for (int k = 0; k < exidre3.Rows.Count; k++)
                            {
                                string barcode = exidre3.Rows[k][0].ToString();
                                if (barcode == strstation)
                                {
                                    rowcount = k;
                                }
                            }
                            for (int j = 0; j < dtm.Rows.Count; j++)
                            {
                                string tetime = dtm.Rows[j][1].ToString();
                                for (int k = 0; k < ListDate1.Count; k++)
                                {
                                    int num = 0;
                                    if (tetime == ListDate1[k])
                                    {
                                        num = k + 1;
                                        //if (rowcount != 0)
                                        //{
                                        exidre3.Rows[rowcount][num] = dtm.Rows[j][2].ToString();
                                        //} 
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            double aVNb1 = 0;
            for (int i = 0; i < exidre3.Rows.Count; i++)
            {
                //int RNb = 0;
                //int YNb = 0;
                double VNb = 0;
                for (int j = 1; j < CNb; j++)
                {
                    var Rval = exidre3.Rows[i][j].ToString();
                    double Vct = Rval == "" ? 0 : Convert.ToDouble(Rval);
                    if (Rct > Vct)
                    {
                        var date1 = ListDate[j - 1];
                        double Vct1 = Tct - Vct; 
                        if (LisDTtime.Count > 0)
                        {
                            if (LisDTtime.Contains(date1))
                            {
                                exidre3.Rows[i][j] = "0";
                                exidre4.Rows[i][j] = "0";
                            }
                            else
                            {
                                VNb += Vct1;
                                exidre3.Rows[i][j] = Vct1;
                                exidre4.Rows[i][j] = "1";
                            }
                        }
                        else
                        {
                            VNb += Vct1;
                            exidre3.Rows[i][j] = Vct1;
                            exidre4.Rows[i][j] = "1";
                        }

                    }
                    if (Vct > Lct)
                    {
                        exidre3.Rows[i][j] = "0";
                        exidre4.Rows[i][j] = "0"; 
                    }
                    if (Rct <= Vct && Vct <= Lct)
                    {
                        exidre3.Rows[i][j] = "0";
                        exidre4.Rows[i][j] = "0";
                    }
                }
                exidre3.Rows[i][ListDate.Count + 2] = VNb;
                double vFpy1 = Math.Round((VNb / (Tct * 24)), 4);
                exidre3.Rows[i][ListDate.Count + 3] = Math.Round(vFpy1 * 100,2) + "%";
            }
            for (int i = 0; i < exidre3.Rows.Count; i++)
            {
                var Rval = exidre3.Rows[i][CNb].ToString();
                double Vct = Rval == "" ? 0 : Convert.ToDouble(Rval);
                if (Rct > Vct)
                {
                    var date1 = ListDate[CNb - 1];
                    double Vct1 = Tct - Vct;
                    if (LisDTtime.Count > 0)
                    {
                        if (LisDTtime.Contains(date1))
                        {
                            exidre3.Rows[i][CNb] = "0"; 
                        }
                        else
                        { 
                            exidre3.Rows[i][CNb] = Vct1; 
                        }
                    }
                    else
                    { 
                        exidre3.Rows[i][CNb] = Vct1; 
                    }

                }
                if (Vct > Lct)
                {
                    exidre3.Rows[i][CNb] = "0"; 
                }
                if (Rct <= Vct && Vct <= Lct)
                {
                    exidre3.Rows[i][CNb] = "0"; 
                }
            }
            for (int i = 1; i <= ListDate1.Count; i++)
            {
                int rows = 0;
                var val1 = "";
                bool bl = false;
                for (int j = 0; j < exidre3.Rows.Count; j++)
                {
                    string val = exidre3.Rows[j][i].ToString();

                    if (val == "0")
                    {
                        exidre4.Rows[j][i] = "0";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(val1))
                        {
                            val1 = val;
                        }
                        double Vct1 = val == "" ? 0 : Convert.ToDouble(val);
                        double Vct2 = val1 == "" ? 0 : Convert.ToDouble(val1);
                        if (Vct2 <= Vct1)
                        {
                            val1 = val;
                            rows = j;
                            bl = true;
                        }
                        exidre4.Rows[j][i] = "1";
                    }
                }
                if (bl)
                {
                    double Vct1 = exidre3.Rows[rows][i].ToString() == "" ? 0 : Convert.ToDouble(exidre3.Rows[rows][i].ToString());
                    exidre4.Rows[rows][i] = "2";
                    aVNb1 += Vct1;
                }
            }
            string TallDt = aVNb1.ToString();
            double vFpy2 = 1 - Math.Round((aVNb1 / (Tct * 24)), 4);
            string dtMax = Math.Round(vFpy2 * 100, 2) + "%";
            string Cqy = string.Empty;
            if (!string.IsNullOrWhiteSpace(Fty) && !string.IsNullOrWhiteSpace(dtMax))
            {
                string val1 = Fty.Replace("%", "");
                string val2 = dtMax.Replace("%", "");
                double vFpy3 = Math.Round((Convert.ToDouble(val1) * Convert.ToDouble(val2) / (100)), 2);
                Cqy = vFpy3 + "%";
            }
            for (int k1 = 0; k1 < ListDate1.Count; k1++)
            {
                int num = k1 + 1;
                for (int k2 = 0; k2 < exidre3.Rows.Count; k2++)
                {
                    string cloumname = date + " " + exidre3.Columns[num].ColumnName + ":00";
                    if (LisDTtime.Contains(cloumname))
                    {
                        exidre3.Rows[k2][num] = "";
                    }
                }
            }
            List<TIME_VIEW> LisViewDt = new List<TIME_VIEW>();
            for (int i = 0; i < exidre3.Rows.Count; i++)
            {
                TIME_VIEW tIME_VIEW = new TIME_VIEW();
                tIME_VIEW.station = exidre3.Rows[i][0].ToString();
                tIME_VIEW.T1 = AppHelper.REVL3(CNaC, 0, exidre3.Rows[i][1].ToString(), exidre4.Rows[i][1].ToString());
                tIME_VIEW.T2 = AppHelper.REVL3(CNaC, 1, exidre3.Rows[i][2].ToString(), exidre4.Rows[i][2].ToString());
                tIME_VIEW.T3 = AppHelper.REVL3(CNaC, 2, exidre3.Rows[i][3].ToString(), exidre4.Rows[i][3].ToString());
                tIME_VIEW.T4 = AppHelper.REVL3(CNaC, 3, exidre3.Rows[i][4].ToString(), exidre4.Rows[i][4].ToString());
                tIME_VIEW.T5 = AppHelper.REVL3(CNaC, 4, exidre3.Rows[i][5].ToString(), exidre4.Rows[i][5].ToString());
                tIME_VIEW.T6 = AppHelper.REVL3(CNaC, 5, exidre3.Rows[i][6].ToString(), exidre4.Rows[i][6].ToString());
                tIME_VIEW.T7 = AppHelper.REVL3(CNaC, 6, exidre3.Rows[i][7].ToString(), exidre4.Rows[i][7].ToString());
                tIME_VIEW.T8 = AppHelper.REVL3(CNaC, 7, exidre3.Rows[i][8].ToString(), exidre4.Rows[i][8].ToString());
                tIME_VIEW.T9 = AppHelper.REVL3(CNaC, 8, exidre3.Rows[i][9].ToString(), exidre4.Rows[i][9].ToString());
                tIME_VIEW.T10 = AppHelper.REVL3(CNaC, 9, exidre3.Rows[i][10].ToString(), exidre4.Rows[i][10].ToString());
                tIME_VIEW.T11 = AppHelper.REVL3(CNaC, 10, exidre3.Rows[i][11].ToString(), exidre4.Rows[i][11].ToString());
                tIME_VIEW.T12 = AppHelper.REVL3(CNaC, 11, exidre3.Rows[i][12].ToString(), exidre4.Rows[i][12].ToString());
                tIME_VIEW.T13 = AppHelper.REVL3(CNaC, 12, exidre3.Rows[i][13].ToString(), exidre4.Rows[i][13].ToString());
                tIME_VIEW.T14 = AppHelper.REVL3(CNaC, 13, exidre3.Rows[i][14].ToString(), exidre4.Rows[i][14].ToString());
                tIME_VIEW.T15 = AppHelper.REVL3(CNaC, 14, exidre3.Rows[i][15].ToString(), exidre4.Rows[i][15].ToString());
                tIME_VIEW.T16 = AppHelper.REVL3(CNaC, 15, exidre3.Rows[i][16].ToString(), exidre4.Rows[i][16].ToString());
                tIME_VIEW.T17 = AppHelper.REVL3(CNaC, 16, exidre3.Rows[i][17].ToString(), exidre4.Rows[i][17].ToString());
                tIME_VIEW.T18 = AppHelper.REVL3(CNaC, 17, exidre3.Rows[i][18].ToString(), exidre4.Rows[i][18].ToString());
                tIME_VIEW.T19 = AppHelper.REVL3(CNaC, 18, exidre3.Rows[i][19].ToString(), exidre4.Rows[i][19].ToString());
                tIME_VIEW.T20 = AppHelper.REVL3(CNaC, 19, exidre3.Rows[i][20].ToString(), exidre4.Rows[i][20].ToString());
                tIME_VIEW.T21 = AppHelper.REVL3(CNaC, 20, exidre3.Rows[i][21].ToString(), exidre4.Rows[i][21].ToString());
                tIME_VIEW.T22 = AppHelper.REVL3(CNaC, 21, exidre3.Rows[i][22].ToString(), exidre4.Rows[i][22].ToString());
                tIME_VIEW.T23 = AppHelper.REVL3(CNaC, 22, exidre3.Rows[i][23].ToString(), exidre4.Rows[i][23].ToString());
                tIME_VIEW.T24 = AppHelper.REVL3(CNaC, 23, exidre3.Rows[i][24].ToString(), exidre4.Rows[i][24].ToString());
                tIME_VIEW.F1 = exidre3.Rows[i][exidre3.Columns.Count - 3].ToString();
                tIME_VIEW.F2 = exidre3.Rows[i][exidre3.Columns.Count - 2].ToString();
                tIME_VIEW.F3 = exidre3.Rows[i][exidre3.Columns.Count - 1].ToString();
                LisViewDt.Add(tIME_VIEW);
            }
            DT_BEAT dT_BEAT = new DT_BEAT
            {
                RowTb= LisView,
                ReTb = LisViewRe,
                CloumTb = ListABLE_VIEWs,
                LisViewDt= LisViewDt,
                ValTb = Value,
                Ave = Ave,
                malNb = MalNb.ToString(),
                maxNb=maxNb.ToString(),
                minNb=minNb.ToString(),
                mtpNb=MtpNb.ToString(),
                TYP1= TYP1,
                Fty= Fty,
                TALLDT = TallDt,
                DTMAX= dtMax,
                Cqy= Cqy,
                TrePairct = TrePairct,
                CNaC = CNaC
            };
            //dT_BEAT
            return dT_BEAT;
            throw new NotImplementedException();
        }
        public bool IODM_BEATE(string LINE)
        {
            string sql = "SELECT ROUND(600/ BEATE_NUM),CEIL(600/BEATE_MIN),TRUNC(600/BEATE_MAX),TRUNC(600/BEATE_TOP) FROM ODM_BEATE WHERE LINECODE='" + LINE + "'";
            DataTable table = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<ErrMessage> Hw_FtyIms(string date1)
        {
            ErrMessage errMessage = new ErrMessage();
            string sql = "SELECT (TO_CHAR(SYSDATE-(1/24),'YYYY-MM-DD HH24')||':00:00' )DATE1, (TO_CHAR(SYSDATE-(1/24),'YYYY-MM-DD HH24')||':59:59' )DATE2,(TO_CHAR(SYSDATE-(1/24),'YYYYMMDDHH24')||'00' )DATE3, (TO_CHAR(SYSDATE-(1/24),'YYYYMMDDHH24')||'59' )DATE4,(TO_CHAR(SYSDATE,'YYYY-MM-DD'))DATE0 FROM DUAL";
            //1小时一次--
            string Date0 = string.Empty;
            string Date1 = string.Empty;
            string Date2 = string.Empty;
            string Date3 = string.Empty;
            string Date4 = string.Empty;
            int totalRow = 0;
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable.Rows.Count > 0)
            {
                Date1 = dataTable.Rows[0][0].ToString();
                Date2 = dataTable.Rows[0][1].ToString();
                Date3 = dataTable.Rows[0][2].ToString();
                Date4 = dataTable.Rows[0][3].ToString();
                Date0 = dataTable.Rows[0][4].ToString();
            }
            if (!string.IsNullOrEmpty(Date1))
            {
                string strselect = string.Format(@"SELECT ''ID,''IF_SEQ,''MOVE_FLAG,''MOVE_TIME,'CHINO-E'FACTORY,'NA'CENTER_ORDER_ID,T1.MO EMS_ORDER_ID,T1.WP_ID OPER,'{3}'FROM_TIME,'{4}'TO_TIME,''STATION_ID,T3.PL_ID PLAN_LINE,T3.PL_ID MFG_LINE,T1.FQTY FIRST_IN_QTY,T1.FQTY IN_QTY,NVL(T11.FQTY,0) GOOD_QTY,
                                 NVL(CM,0)SCRAP_QTY,NVL(CT,0)REPAIR_QTY,(TO_NUMBER(NVL(CT,0))-TO_NUMBER(NVL(CM,0)))REPAIR_GOOD_QTY,'0'REPAIR_WORK_QTY,'0'REWORK_QTY,'0'REWORK_GOOD_QTY,'0'REWORK_WORK_QTY,NVL(CN,0)DEFECT_QTY,(TO_NUMBER(NVL(CN,0))+TO_NUMBER(NVL(T2.NGQTY,0)))FIRST_DEFECT_QTY,(TO_NUMBER(T1.FQTY)-(TO_NUMBER(NVL(CN,0))+TO_NUMBER(NVL(T2.NGQTY,0))))FIRST_GOOD_QTY,NVL(T2.NGQTY,0)TEST_FAULT_QTY
                                 ,COALESCE(GET_T100MODEL@MES(T3.PROD_ID,1),GET_MODEL(T3.PROD_ID),(SELECT PROD_MODEL FROM TB_BS_MTRL A WHERE A.MTRL_ID=T3.PROD_ID))MAT_MODEL,T3.PROD_ID EMS_MAT_ID,T3.PROD_ID HW_MAT_ID,'IMS'CREATE_USER_ID,TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS')CREATE_TIME,''UPDATE_USER_ID,''UPDATE_TIME,''TEST_FAULT_SN_QTY,SYSDATE CREATION_TIME,'0'GETFLAG,''GETTIME,''ACTIONFLAG,'CHINO-E'S_FACTORY,''SEGMENT2,''SEGMENT3,''SEGMENT4,''SEGMENT5
                                  FROM   (SELECT A.MO,A.WP_ID,COUNT(*) FQTY   FROM  TB_PM_MO_LBWP A WHERE  1=1   AND A.WP_ID<>'RE' AND WP_CMP_DATE BETWEEN   TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{2}','YYYY/MM/DD HH24:MI:SS')   GROUP BY A.MO,A.WP_ID ) T1 
                                 LEFT OUTER JOIN    (SELECT A.MO,A.WP_ID,COUNT(*) FQTY   FROM  TB_PM_MO_LBWP A WHERE  1=1 AND  A.IS_PASS='Y'  AND A.WP_ID<>'RE' AND WP_CMP_DATE BETWEEN   TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{2}','YYYY/MM/DD HH24:MI:SS')   GROUP BY A.MO,A.WP_ID ) T11 ON T1.MO=T11.MO AND T1.WP_ID=T11.WP_ID  
                                 LEFT OUTER JOIN  
                                (SELECT MO,WP_ID,COUNT(*) NGQTY FROM (SELECT C.MO,D.WP_ID,D.LB_ID,D.SYS_CRT_DATE  FROM  TB_PM_MO_LB C ,TB_ENOK_TEST_NG_TIME  D WHERE  C.LB_ID=D.LB_ID AND (D.NG_TIME = 1 OR
                                (D.NG_TIME = 2 AND D.IS_CC = 'N')) AND D.SYS_CRT_DATE BETWEEN   TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{2}','YYYY/MM/DD HH24:MI:SS') AND NOT EXISTS(SELECT T.LB_ID FROM TB_PM_QC_HD T
                                 WHERE T.SYS_CRT_DATE BETWEEN  TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{2}','YYYY/MM/DD HH24:MI:SS') AND T.LB_ID=D.LB_ID AND T.WP_ID=C.WP_ID)) A  
                                 WHERE 1=1   GROUP BY A.MO,A.WP_ID)T2 
                                 ON T1.MO=T2.MO AND T1.WP_ID=T2.WP_ID    LEFT OUTER JOIN ( SELECT A1.MO,A3.WP_ID,COUNT(*) CT FROM TB_PM_RP_HD A1 LEFT OUTER JOIN TB_BS_TP_WP_MO A2 ON A1.MO=A2.MO AND A1.TP_WP_ID=A2.TP_WP_ID LEFT OUTER JOIN TB_BS_TP_WP_MO A3 ON A3.MO=A2.MO AND  A3.SEQ=A2.SEQ AND A3.WP_ID<>'RE'    
                                 WHERE A1.SYS_CRT_DATE   BETWEEN   TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{2}','YYYY/MM/DD HH24:MI:SS')   GROUP BY A1.MO,A3.WP_ID) T5   
                                 ON T5.MO=T1.MO  AND T5.WP_ID=T1.WP_ID   LEFT OUTER JOIN (SELECT MO,WP_ID,COUNT(*) CN FROM TB_PM_QC_HD T WHERE SYS_CRT_DATE   BETWEEN   TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{2}','YYYY/MM/DD HH24:MI:SS')  GROUP BY MO,WP_ID)T6 
                                 ON T6.MO=T1.MO  AND T6.WP_ID=T1.WP_ID   LEFT OUTER JOIN (SELECT A.MO,A.WP_ID,COUNT(*) CW   FROM  TB_PM_MO_LBWP A WHERE  1=1  AND WP_CMP_DATE   BETWEEN   TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{2}','YYYY/MM/DD HH24:MI:SS')    GROUP BY A.MO,A.WP_ID ) T7 
                                 ON T7.MO=T1.MO  AND T7.WP_ID=T1.WP_ID   LEFT OUTER JOIN  ( SELECT A1.MO,A3.WP_ID,COUNT(*) CM FROM TB_PM_RP_HD A1 LEFT OUTER JOIN TB_BS_TP_WP_MO A2 ON A1.MO=A2.MO AND A1.TP_WP_ID=A2.TP_WP_ID LEFT OUTER JOIN TB_BS_TP_WP_MO A3 ON A3.MO=A2.MO AND  A3.SEQ=A2.SEQ AND A3.WP_ID<>'RE'   WHERE A1.RP_RS=2  AND A1.SYS_CRT_DATE   BETWEEN   TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{2}','YYYY/MM/DD HH24:MI:SS')  GROUP BY A1.MO,A3.WP_ID) T8
                                 ON T8.MO=T1.MO  AND T8.WP_ID=T1.WP_ID   LEFT OUTER JOIN TB_PP_MO T3 ON T1.MO=T3.MO  WHERE CUST_ID IN('20.001.0002','20.001.0001','20.001.0005','2200001','2200002','2200003')
                                  ", Date0, Date1, Date2, Date3, Date4);
                await Task.Run(() =>
                {
                    DataTable dt = OracleHelper.ExecuteDataTable1(UserInfo.OracleConnectionStringIms, strselect);
                    if (dt.Rows.Count > 0)
                    {
                          totalRow = dt.Rows.Count;
                        string TableName = "[HWOUTPUT].[dbo].[First_Pass_Yield_Data]";
                        AppHelper.dBtnInsert(dt, TableName, totalRow);
                    }
                });
            }
            //throw new NotImplementedException();
            errMessage.success = true;
            errMessage.Err = "执行完成Hw_RepairMes:"+ totalRow;
            return errMessage;
        }
        public async Task<ErrMessage> Hw_FtyMes(string date1)
        {
            DataTable exid = null;
            exid =AppHelper.GetTableSchema();
            ErrMessage errMessage = new ErrMessage();
            string sql = "SELECT (TO_CHAR(SYSDATE-(1/24),'YYYY-MM-DD HH24')||':00:00' )DATE1, (TO_CHAR(SYSDATE-(1/24),'YYYY-MM-DD HH24')||':59:59' )DATE2,(TO_CHAR(SYSDATE-(1/24),'YYYYMMDDHH24')||'00' )DATE3, (TO_CHAR(SYSDATE-(1/24),'YYYYMMDDHH24')||'59' )DATE4,(TO_CHAR(SYSDATE,'YYYY-MM-DD'))DATE0 FROM DUAL";
            //1小时一次--
            string Date0 = string.Empty;
            string Date1 = string.Empty;
            string Date2 = string.Empty;
            string Date3 = string.Empty;
            string Date4 = string.Empty;
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable.Rows.Count > 0)
            {
                Date1 = dataTable.Rows[0][0].ToString();
                Date2 = dataTable.Rows[0][1].ToString();
                Date3 = dataTable.Rows[0][2].ToString();
                Date4 = dataTable.Rows[0][3].ToString();
                Date0 = dataTable.Rows[0][4].ToString();
            }
            if (!string.IsNullOrEmpty(Date1))
            {
                sql = $@"SELECT T.WORKJOB_CODE ,T.TESTPOSITION STATION,T.LINE_CODE  FROM TODAYWORKJOB T  LEFT OUTER JOIN WORK_WORKJOB K ON K.WORKJOB_CODE=T.WORKJOB_CODE WHERE T.CREATEDATE = TO_DATE('{Date0}','YYYY-MM-DD') 
                       AND T.TESTPOSITION IN(SELECT NAME FROM QA_MANTAINCE_TYPE WHERE FPYCOUNT = 1) AND K.CLIENTCODE = '01'";  
                await Task.Run(() =>
                {
                    DataTable dt = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, sql);
                    if (dt.Rows.Count > 0)
                    {
                        List<DT_FPY> lisDT_FPY = new List<DT_FPY>();
                        //工单记录
                        for (int i = 0; i < dt.Rows.Count; i++)
                        { 
                            DT_FPY dT_FPY = new DT_FPY();
                            string testpnum = "0"; //通过数
                            string mo = dt.Rows[i][0].ToString();
                            string wo = mo.Replace("-", "_");
                            string station= dt.Rows[i][1].ToString();
                            string Line = dt.Rows[i][2].ToString();
                            string Stringsqla = string.Format(@"SELECT COUNT(DISTINCT BARCODE) FROM {0} A WHERE   SCAN_TIME BETWEEN   TO_DATE('{3}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{4}','YYYY/MM/DD HH24:MI:SS') AND TESTPOSITION='{1}'   AND A.SCAN_POSITION='F' 
                                    AND LINE_CODE2='{2}' AND QULITY_FLAG='no' AND NOT EXISTS  
                             (SELECT  T1.REPAIR_CARD_CODE FROM QA_IPQC_SUBREPAIR T1 WHERE   T1.SENDDATE BETWEEN   TO_DATE('{3}', 'YYYY/MM/DD HH24:MI:SS') AND
                               TO_DATE('{4}', 'YYYY/MM/DD HH24:MI:SS') AND  COMMAND_CODE = '{5}' AND LINE_CODE = '{2}' AND  POSTION_ITEMVERSION = '{1}'  AND T1.REPAIRDATE IS NULL AND T1.REPAIR_CARD_CODE=A.BARCODE) ", wo, station, Line, Date1, Date2, mo);
                            DataTable dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, Stringsqla);
                            if (dtm.Rows.Count > 0)
                            {
                                testpnum = dtm.Rows[0][0].ToString(); 
                            }
                            //释放记录
                            string trefreedpnum = "0";
                            string Stringsql = string.Format("SELECT COUNT(DISTINCT BARCODE) FROM REWORK_PRIMARY_RECORD T WHERE  SCAN_TIME BETWEEN   TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  T.ATTEMPTER_CODE='{2}' AND T.SCAN_POSITION='F'" +
                                " AND LINE_CODE2='{3}' AND T.TESTPOSITION='{4}' AND T.SCAN_POSITION='F' ", Date1, Date2, mo, Line, station);
                            dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, Stringsql);
                            if (dtm.Rows.Count > 0)
                            {
                                trefreedpnum = dtm.Rows[0][0].ToString();
                            }
                            //返工投入数：
                            string tfgpnum = "0";
                            string sqldtfg = string.Format("SELECT  COUNT(DISTINCT BARCODE)   FROM  {0} A WHERE   SCAN_TIME BETWEEN   TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{2}','YYYY/MM/DD HH24:MI:SS') AND  TESTPOSITION='{3}' AND A.SCAN_POSITION='F' AND LINE_CODE2='{4}' " +
                                " AND EXISTS(SELECT   BARCODE  FROM REWORK_PRIMARY_RECORD T WHERE   SCAN_TIME BETWEEN   TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{2}','YYYY/MM/DD HH24:MI:SS') AND T.ATTEMPTER_CODE='{5}' " +
                                "AND LINE_CODE2='{4}' AND T.TESTPOSITION='{3}' AND T.SCAN_POSITION='F' AND T.BARCODE=A.BARCODE)", wo, Date1, Date2, station, Line, mo);
                            dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, sqldtfg);
                            if (dtm.Rows.Count > 0)
                            {
                                tfgpnum = dtm.Rows[0][0].ToString();
                            }
                            //投入数=通过数+返工投入数    //var P_INNUMBER = Convert.ToString(Convert.ToInt32(testpnum) + Convert.ToInt32(tfgpnum))
                            string P_INNUMBER = Convert.ToString(Convert.ToInt32(testpnum) + Convert.ToInt32(tfgpnum));
                            string P_FINUBER = "0";

                            string strselect = string.Format(@"SELECT COUNT(DISTINCT BARCODE) FROM (SELECT  BARCODE  FROM {0} A WHERE   SCAN_TIME BETWEEN   TO_DATE('{3}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{4}','YYYY/MM/DD HH24:MI:SS') AND TESTPOSITION='{1}'   AND A.SCAN_POSITION='F'
                                                        AND LINE_CODE2='{2}' UNION SELECT   BARCODE  FROM REWORK_PRIMARY_RECORD T WHERE  SCAN_TIME BETWEEN   TO_DATE('{3}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{4}','YYYY/MM/DD HH24:MI:SS') AND  T.ATTEMPTER_CODE='{5}' AND T.SCAN_POSITION='F' 
                                                        AND LINE_CODE2='{2}' AND T.TESTPOSITION='{1}' AND T.SCAN_POSITION='F' UNION 
                                                        SELECT BARCODE FROM NTF_FAIL T WHERE SCAN_TIME BETWEEN TO_DATE('{3}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{4}','YYYY/MM/DD HH24:MI:SS')   AND  T.ATTEMPTER_CODE='{5}' AND LINE_CODE2='{2}' AND T.TESTPOSITION='{1}'
                                                        UNION SELECT A1.REPAIR_CARD_CODE BARCODE FROM QA_IPQC_SUBREPAIR A1  WHERE A1.SENDDATE BETWEEN TO_DATE('{3}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{4}','YYYY/MM/DD HH24:MI:SS')  
                                                          AND A1.LINE_CODE='{2}' AND A1.POSTION_ITEMVERSION='{1}' AND   GONGWEI NOT IN('MMI2','AG')　 AND　COMMAND_CODE = '{5}' )  ", wo, station, Line, Date1, Date2, mo);
                            dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strselect);
                            if (dtm.Rows.Count > 0)
                            {
                                P_FINUBER = dtm.Rows[0][0].ToString();
                            }
                            if (P_FINUBER != "0")
                            {
                                //报废数
                                string P_BFNUMBER = "0";
                                string strsql = Stringsqla + " AND EXISTS(SELECT REPAIR_CARD_CODE FROM QA_IPQC_SUBREPAIR B   WHERE B.RES = '报废'  AND   GONGWEI NOT IN('MMI2','AG') AND LINE_CODE='" + Line + "' AND B.POSTION_ITEMVERSION = '" + station + "'  AND B.REPAIR_CARD_CODE=A.BARCODE)";
                                dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsql);
                                if (dtm.Rows.Count > 0)
                                {
                                    P_BFNUMBER = dtm.Rows[0][0].ToString();
                                }
                                //通过数；通过数-报废数
                                string P_TGNUMBER = Convert.ToString(Convert.ToInt32(testpnum) - Convert.ToInt32(P_BFNUMBER));
                                //返工投入数
                                string P_RINUBER = "0";
                                string strsql1 = Stringsqla + string.Format("AND EXISTS(SELECT   BARCODE  FROM REWORK_PRIMARY_RECORD T WHERE SCAN_TIME BETWEEN   TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  T.ATTEMPTER_CODE='" + mo + "' AND LINE_CODE2='" + Line + "' AND T.TESTPOSITION='" + station + "' AND T.SCAN_POSITION='F'  AND T.BARCODE=A.BARCODE)", Date1, Date2);
                                dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsql1);
                                if (dtm.Rows.Count > 0)
                                {
                                    P_RINUBER = dtm.Rows[0][0].ToString();
                                }
                                //投入数=一次投入数+返工投入数
                                P_INNUMBER = Convert.ToString(Convert.ToInt32(P_FINUBER) + Convert.ToInt32(P_RINUBER));
                                string strsqlk = Stringsql + string.Format(" AND EXISTS(SELECT  A.BARCODE FROM  " + wo + " A WHERE  SCAN_TIME BETWEEN   TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND   TESTPOSITION='" + station + "' AND A.BARCODE=T.BARCODE  AND A.SCAN_POSITION='F' AND LINE_CODE2='" + Line + "'   GROUP BY A.BARCODE HAVING COUNT(A.BARCODE) <2)", Date1, Date2);
                                //通过数-重投数+释放记录表数据！！（一次投入数）
                                string number1 = "0";
                                dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsqlk);
                                if (dtm.Rows.Count > 0)
                                {
                                    number1 = dtm.Rows[0][0].ToString();
                                }
                                //单天的维修记录
                                string strsql3 = string.Format("SELECT COUNT(*) FROM QA_IPQC_SUBREPAIR T WHERE GONGWEI NOT IN('MMI2','AG') AND  REPAIRDATE BETWEEN   TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  T.COMMAND_CODE = '" + mo + "' AND LINE_CODE='" + Line + "' AND T.POSTION_ITEMVERSION = '" + station + "'  ", Date1, Date2);
                                string P_WXNUMBER = "0"; //维修数
                                dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsql3);
                                if (dtm.Rows.Count > 0)
                                {
                                    P_WXNUMBER = dtm.Rows[0][0].ToString();
                                }
                                string P_WBNUMBER = "0";//维修报废数
                                string strsql4 = string.Format("SELECT COUNT(*) FROM QA_IPQC_SUBREPAIR T WHERE   GONGWEI NOT IN('MMI2','AG') AND  REPAIRDATE BETWEEN   TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  T.COMMAND_CODE = '" + mo + "' AND LINE_CODE='" + Line + "' AND T.POSTION_ITEMVERSION = '" + station + "'  AND T.RES = '报废'", Date1, Date2);
                                dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsql4);
                                if (dtm.Rows.Count > 0)
                                {
                                    P_WBNUMBER = dtm.Rows[0][0].ToString();
                                }
                                //维修重投数=返工重投+维修过的--
                                string P_RWNUMBER = "0";
                                string strsql5 = strsql1 + string.Format(" AND EXISTS(SELECT C.REPAIR_CARD_CODE FROM QA_IPQC_SUBREPAIR C WHERE C.REPAIRDATE BETWEEN   TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS')  AND   GONGWEI NOT IN('MMI2','AG') AND  C.COMMAND_CODE = '" + mo + "' AND LINE_CODE='" + Line + "' AND  C.POSTION_ITEMVERSION = '" + station + "'  AND C.REPAIR_CARD_CODE=A.BARCODE)", Date1, Date2);
                                dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsql5);
                                if (dtm.Rows.Count > 0)
                                {
                                    P_RWNUMBER = dtm.Rows[0][0].ToString();
                                }
                                //维修通过数=维修数-报废数
                                string P_WTNUBER = Convert.ToString(Convert.ToInt32(P_WXNUMBER) - Convert.ToInt32(P_WBNUMBER));
                                //单天的送修记录
                                string P_BLNUMBER = "0";
                                string strsql6 = string.Format("SELECT COUNT(*) FROM QA_IPQC_SUBREPAIR T WHERE SENDDATE BETWEEN   TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND T.COMMAND_CODE = '" + mo + "'  AND   GONGWEI NOT IN('MMI2','AG') AND T.POSTION_ITEMVERSION = '" + station + "' AND LINE_CODE='" + Line + "'  AND   GONGWEI NOT IN('MMI2','AG')", Date1, Date2);
                                dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsql6);
                                if (dtm.Rows.Count > 0)
                                {
                                    P_BLNUMBER = dtm.Rows[0][0].ToString();
                                }
                                string P_BLNUMBER1 = "0";
                                //YES状态数
                                string strsql7 = string.Format("SELECT  COUNT(DISTINCT BARCODE)   FROM  " + wo + " T WHERE SCAN_TIME BETWEEN TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') AND  TESTPOSITION='" + station + "' AND T.SCAN_POSITION='L' AND T.QULITY_FLAG='yes' " +
                                              "AND LINE_CODE2='" + Line + "' AND WATER IS  NULL  AND NOT EXISTS(SELECT C.REPAIR_CARD_CODE FROM QA_IPQC_SUBREPAIR C " +
                                              "WHERE C.COMMAND_CODE = '" + mo + "' AND C.REPAIRDATE IS NULL AND C.REPAIR_CARD_CODE=T.BARCODE) ", Date1, Date2);
                                dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsql7);
                                if (dtm.Rows.Count > 0)
                                {
                                    P_BLNUMBER1 = dtm.Rows[0][0].ToString();
                                }
                                P_BLNUMBER = Convert.ToString(Convert.ToInt32(P_BLNUMBER) + Convert.ToInt32(P_BLNUMBER1));
                                //首次不良数(第一次出现不良  ？该工序第一次出现不良 ？该工序该时间段出现不良)
                                string P_FSNUMBER1 = "0";
                                string strsql8 = strsql6 + " AND NOT EXISTS(SELECT F.REPAIR_CARD_CODE  FROM QA_IPQC_SUBREPAIR F WHERE F.COMMAND_CODE = '" + mo + "'  AND   GONGWEI NOT IN('MMI2','AG')  GROUP BY F.REPAIR_CARD_CODE HAVING COUNT(F.REPAIR_CARD_CODE)>1 AND F.REPAIR_CARD_CODE=T.REPAIR_CARD_CODE) ";
                                dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsql8);
                                if (dtm.Rows.Count > 0)
                                {
                                    P_FSNUMBER1 = dtm.Rows[0][0].ToString();
                                }
                                string P_FSNUMBER2 = "0";
                                string strsql9 = strsql7 + " AND NOT EXISTS(SELECT F.REPAIR_CARD_CODE  FROM QA_IPQC_SUBREPAIR F WHERE F.COMMAND_CODE = '" + mo + "'   AND   GONGWEI NOT IN('MMI2','AG') GROUP BY F.REPAIR_CARD_CODE HAVING COUNT(F.REPAIR_CARD_CODE)>1 AND F.REPAIR_CARD_CODE=T.BARCODE) ";
                                dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsql9);
                                if (dtm.Rows.Count > 0)
                                {
                                    P_FSNUMBER2 = dtm.Rows[0][0].ToString();
                                }
                                //工序
                                //一次性通过数 =通过数-不良数-返工数-报废数
                                string P_FSGNUMBER = "0";
                                string P_WCNUMEBER = "0";
                                string strsql11 = string.Format(@"SELECT COUNT(DISTINCT BARCODE) FROM NTF_FAIL T WHERE SCAN_TIME BETWEEN TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS')   
                                                        AND  T.ATTEMPTER_CODE='" + mo + "' AND LINE_CODE2='" + Line + "' AND T.TESTPOSITION='" + station + "'", Date1, Date2);
                                strsql11 = strsql11 + string.Format(" AND NOT EXISTS(SELECT C.REPAIR_CARD_CODE FROM QA_IPQC_SUBREPAIR C WHERE C.SENDDATE BETWEEN   TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS')  AND   GONGWEI NOT IN('MMI2','AG') AND  C.COMMAND_CODE = '" + mo + "' AND LINE_CODE='" + Line + "' AND  C.POSTION_ITEMVERSION = '" + station + "'  AND C.REPAIR_CARD_CODE=T.BARCODE)", Date1, Date2);
                                dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsql11);
                                if (dtm.Rows.Count > 0)
                                {
                                    P_WCNUMEBER = dtm.Rows[0][0].ToString();
                                }
                                string Plan_Line = "";
                                string MAT_MODEL = "";
                                string EMS_MAT_ID = "";
                                string Center_Order_Id = "";
                                string datends = "";
                                string datends1 = "";
                                //去除中文字符
                                string strsql12 = string.Format("SELECT T.LINE,COALESCE(GET_MODEL(T.ITEM_CODE),(GET_T100MODEL(T.ITEM_CODE,1)),TO_CHAR(REPLACE((SELECT PRODUCT_MODEL FROM ODM_MODEL WHERE  BOM=T.ITEM_CODE),'/','')))ITEM_CODES,T.ITEM_CODE,(CASE WHEN WORKORDER IS NULL THEN 'NA' ELSE WORKORDER END),TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS'),SYSDATE FROM WORK_WORKJOB T WHERE T.WORKJOB_CODE='{0}'", mo);
                                dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsql12);
                                if (dtm.Rows.Count > 0)
                                {
                                    Plan_Line = dtm.Rows[0][0].ToString();
                                    MAT_MODEL = dtm.Rows[0][1].ToString();
                                    EMS_MAT_ID = dtm.Rows[0][2].ToString();
                                    Center_Order_Id = dtm.Rows[0][3].ToString();
                                    datends = dtm.Rows[0][4].ToString();
                                    datends1 = dtm.Rows[0][5].ToString();
                                }
                                P_FSNUMBER1 = Convert.ToString(Convert.ToInt32(P_WCNUMEBER) + Convert.ToInt32(P_BLNUMBER));
                                P_FSGNUMBER = Convert.ToString(Convert.ToInt32(P_FINUBER) - Convert.ToInt32(P_FSNUMBER1));
                                if (Convert.ToInt32(P_FSGNUMBER) < 0)
                                {
                                    P_FSGNUMBER = "0";
                                }
                                string ISLINE = "CHINO-E";
                                strsql12 = "SELECT  WORKSHOP2 FROM WORKPRODUCE T WHERE  LINE='" + Line + "'";
                                dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsql12);
                                if (dtm.Rows.Count > 0)
                                {
                                    ISLINE = dtm.Rows[0][0].ToString();
                                }
                                if (string.IsNullOrEmpty(ISLINE))
                                {
                                    ISLINE = "CHINO-E";
                                }
                                if (station == "Runin_In") {
                                    station = "RUNIN";
                                }
                                if (station == "MMI_OFFLINE")
                                {
                                    station = "MMI1";
                                }
                                DataRow dr = exid.NewRow();
                                //dr[0] = "";
                                dr[1] = "";
                                dr[2] = "";
                                dr[3] = "";
                                dr[4] = "CHINO-E";
                                dr[5] = Center_Order_Id;
                                dr[6] = mo;
                                dr[7] = station;
                                dr[8] = Date3;
                                dr[9] = Date4;
                                dr[10] = "";
                                dr[11] = Plan_Line;
                                dr[12] = Line;
                                dr[13] = P_FINUBER;//一次投入数 
                                dr[14] = P_INNUMBER;//投入数 
                                dr[15] = P_TGNUMBER;//通过数
                                dr[16] = P_WBNUMBER;//报废数
                                dr[17] = P_WXNUMBER;//维修数 P_BFNUMBER
                                dr[18] = P_WTNUBER;//维修通过数
                                dr[19] = P_RWNUMBER;//维修重投数
                                dr[20] = trefreedpnum;//返工数
                                dr[21] = tfgpnum;//返工后通过数
                                dr[22] = tfgpnum;//返工后投入数
                                dr[23] = P_BLNUMBER;//不良数
                                dr[24] = P_FSNUMBER1;//首次不良数 
                                dr[25] = P_FSGNUMBER;//一次性通过数 
                                dr[26] = P_WCNUMEBER;//误测数 
                                dr[27] = MAT_MODEL;
                                dr[28] = EMS_MAT_ID;
                                dr[29] = EMS_MAT_ID;
                                dr[30] = "MES";
                                dr[31] = datends;
                                dr[32] = "";
                                dr[33] = "";
                                dr[34] = "";
                                dr[35] = datends1;
                                dr[36] = 0;
                                dr[37] = "";
                                dr[38] = null;//CHINO-E
                                dr[39] = ISLINE;
                                exid.Rows.Add(dr);
                            }
                        }  
                    }
                });
            }
            int totalRow = 0;
            if (exid.Rows.Count>0)
            {
                totalRow = exid.Rows.Count;
                string TableName = "[HWOUTPUT].[dbo].[First_Pass_Yield_Data]";
                AppHelper.dBtnInsert(exid, TableName, totalRow);
            }
            //throw new NotImplementedException();
            errMessage.success = true; 
            errMessage.Err = "执行完成Hw_RepairMes:"+ totalRow;
            return errMessage;
        }
        public async Task<ErrMessage> Hw_FtyMesMmi2(string date1)
        {
            DataTable exid = null;
            exid = AppHelper.GetTableSchema();
            ErrMessage errMessage = new ErrMessage();
            string sql = "SELECT (TO_CHAR(SYSDATE-(1/24),'YYYY-MM-DD HH24')||':00:00' )DATE1, (TO_CHAR(SYSDATE-(1/24),'YYYY-MM-DD HH24')||':59:59' )DATE2,(TO_CHAR(SYSDATE-(1/24),'YYYYMMDDHH24')||'00' )DATE3, (TO_CHAR(SYSDATE-(1/24),'YYYYMMDDHH24')||'59' )DATE4,(TO_CHAR(SYSDATE,'YYYY-MM-DD'))DATE0 FROM DUAL";
            //1小时一次--
            string Date0 = string.Empty;
            string Date1 = string.Empty;
            string Date2 = string.Empty;
            string Date3 = string.Empty;
            string Date4 = string.Empty;
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable.Rows.Count > 0)
            {
                Date1 = dataTable.Rows[0][0].ToString();
                Date2 = dataTable.Rows[0][1].ToString();
                Date3 = dataTable.Rows[0][2].ToString();
                Date4 = dataTable.Rows[0][3].ToString();
                Date0 = dataTable.Rows[0][4].ToString();
            }
            if (!string.IsNullOrEmpty(Date1))
            {
                string strDate1 = Date1;
                string strDate2 = Date2;
                string strselect = string.Format(@"SELECT T.WORKJOB_CODE ,T.TESTPOSITION STATION,T.LINE_CODE  FROM TODAYWORKJOB T  LEFT OUTER JOIN WORK_WORKJOB K ON K.WORKJOB_CODE=T.WORKJOB_CODE WHERE T.CREATEDATE = TO_DATE('{0}','YYYY-MM-DD') 
                       AND T.TESTPOSITION IN(SELECT NAME FROM QA_MANTAINCE_TYPE WHERE FPYCOUNT = 1) AND K.CLIENTCODE = '01'", Date0);
                await Task.Run(() =>
                {
                    DataTable dt = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, sql);
                    if (dt.Rows.Count>0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            /// 送修数
                            string strSenNum = "0";
                            //维修数
                            string strRepNum = "0";
                            //通过数
                            string strGoodNum = "0";
                            //投入
                            string strGoodNum1 = "0";
                            //不良数
                            string strErrNum = "0";
                            //报废WORK20171226021
                            string strBufNum = "0";
                            string strworkcode = dt.Rows[i][0].ToString();
                            string strLinecode = dt.Rows[i][1].ToString();
                            string P_station = dt.Rows[i][2].ToString();
                            string P_tablename = strworkcode.ToString().Replace('-', '_'); 
                            strselect = string.Format(@"SELECT COUNT(*)  FROM QA_IPQC_SUBREPAIR A WHERE A.GONGWEI = 'MMI2'
                            AND EXISTS(SELECT WORKJOB_CODE FROM WORK_WORKJOB B WHERE B.WORKJOB_CODE= A.COMMAND_CODE AND B.CLIENTCODE= '01')
                            AND A.SENDDATE BETWEEN TO_DATE('{2}', 'YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{3}', 'YYYY-MM-DD HH24:MI:SS')  AND A.LINE_CODE='{1}'
                            AND A.COMMAND_CODE = '{0}'", strworkcode, strLinecode, strDate1, strDate2);
                            DataTable dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strselect);
                            if (dtm.Rows.Count > 0)
                            {
                                strSenNum = dtm.Rows[0][0].ToString();
                            }
                            //维修数据
                            strselect = string.Format(@"SELECT COUNT(*)  FROM QA_IPQC_SUBREPAIR A WHERE A.GONGWEI = 'MMI2'
                            AND EXISTS(SELECT WORKJOB_CODE FROM WORK_WORKJOB B WHERE B.WORKJOB_CODE= A.COMMAND_CODE AND B.CLIENTCODE= '01')
                            AND A.REPAIRDATE BETWEEN TO_DATE('{2}', 'YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{3}', 'YYYY-MM-DD HH24:MI:SS') AND A.LINE_CODE='{1}'
                            AND A.COMMAND_CODE = '{0}'", strworkcode, strLinecode, strDate1, strDate2);
                            dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strselect);
                            if (dtm.Rows.Count > 0)
                            {
                                strRepNum = dtm.Rows[0][0].ToString();
                            }
                            //通过数
                            strselect = string.Format("SELECT COUNT(DISTINCT BARCODE) FROM {0} A WHERE   SCAN_TIME BETWEEN   TO_DATE('{3}','YYYY/MM/DD HH24:MI:SS') AND  TO_DATE('{4}','YYYY/MM/DD HH24:MI:SS') AND TESTPOSITION='{1}'   AND A.SCAN_POSITION='F'" +
                                          " AND LINE_CODE2='{2}' ", P_tablename, P_station, strLinecode, strDate1, strDate2);
                            dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strselect);
                            if (dtm.Rows.Count > 0)
                            {
                                strGoodNum = dtm.Rows[0][0].ToString();
                            }

                            //投入数
                            strselect = string.Format(@" SELECT COUNT(DISTINCT BARCODE) FROM {5} B WHERE SCAN_TIME BETWEEN
                            TO_DATE('{0}', 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') 
                            AND TESTPOSITION = '{4}'  AND B.SCAN_POSITION = 'F' AND LINE_CODE2 = '{2}'  AND NOT EXISTS(SELECT A.REPAIR_CARD_CODE FROM QA_IPQC_SUBREPAIR A WHERE A.GONGWEI= 'MMI2'
                            AND EXISTS(SELECT WORKJOB_CODE FROM WORK_WORKJOB B WHERE B.WORKJOB_CODE= A.COMMAND_CODE AND B.CLIENTCODE= '01')
                            AND(A.REPAIRDATE BETWEEN TO_DATE('{0}', 'YYYY-MM-DD HH24:MI:SS') AND  TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS')
                            OR A.SENDDATE BETWEEN TO_DATE('{0}', 'YYYY-MM-DD HH24:MI:SS') AND  TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS')) AND A.LINE_CODE = '{2}' AND A.COMMAND_CODE = '{3}' AND B.BARCODE=A.REPAIR_CARD_CODE )", strDate1, strDate2, strLinecode, strworkcode, P_station, P_tablename);
                            dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strselect);
                            if (dtm.Rows.Count > 0)
                            {
                                strGoodNum1 = dtm.Rows[0][0].ToString();
                            }
                            //strErrNum
                            strselect = string.Format(@" SELECT COUNT(DISTINCT(A.REPAIR_CARD_CODE))  FROM QA_IPQC_SUBREPAIR A WHERE A.GONGWEI = 'MMI2'
                                                 AND EXISTS(SELECT WORKJOB_CODE FROM WORK_WORKJOB B WHERE B.WORKJOB_CODE= A.COMMAND_CODE AND B.CLIENTCODE= '01')
                                                 AND(A.REPAIRDATE BETWEEN TO_DATE('{0}', 'YYYY-MM-DD HH24:MI:SS') AND  TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS')
                                                 OR A.SENDDATE BETWEEN TO_DATE('{0}', 'YYYY-MM-DD HH24:MI:SS') AND  TO_DATE('{1}', 'YYYY-MM-DD HH24:MI:SS')) AND A.LINE_CODE = '{2}' AND A.COMMAND_CODE = '{3}'", strDate1, strDate2, strLinecode, strworkcode, P_station, P_tablename);
                            dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strselect);
                            if (dtm.Rows.Count > 0)
                            {
                                strErrNum = dtm.Rows[0][0].ToString();
                            }
                            var Plan_Line = "";
                            var MAT_MODEL = "";
                            var EMS_MAT_ID = "";
                            var Center_Order_Id = "";
                            var datends = "";
                            var datends1 = "";
                            var strsql12 = string.Format("SELECT T.LINE,COALESCE(GET_MODEL(T.ITEM_CODE),(GET_T100MODEL(T.ITEM_CODE,1)),TO_CHAR(REPLACE((SELECT PRODUCT_MODEL FROM ODM_MODEL WHERE  BOM=T.ITEM_CODE),'/','')))ITEM_CODES,T.ITEM_CODE,(CASE WHEN WORKORDER IS NULL THEN 'NA' ELSE WORKORDER END),TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS'),SYSDATE FROM WORK_WORKJOB T WHERE  T.WORKJOB_CODE='{0}'", strworkcode);
                            dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsql12);
                            if (dtm.Rows.Count > 0)
                            {
                                Plan_Line = dtm.Rows[0][0].ToString();
                                MAT_MODEL = dtm.Rows[0][1].ToString();
                                EMS_MAT_ID = dtm.Rows[0][2].ToString();
                                Center_Order_Id = dtm.Rows[0][3].ToString();
                                datends = dtm.Rows[0][4].ToString();
                                datends1 = dtm.Rows[0][5].ToString();
                            }
                            strGoodNum1 = Convert.ToString(Convert.ToInt32(strGoodNum1) + Convert.ToInt32(strErrNum));

                            strselect = string.Format(@"SELECT COUNT(*)  FROM QA_IPQC_SUBREPAIR A WHERE A.GONGWEI = 'MMI2'
                            AND EXISTS(SELECT WORKJOB_CODE FROM WORK_WORKJOB B WHERE B.WORKJOB_CODE= A.COMMAND_CODE AND B.CLIENTCODE= '01')
                            AND A.REPAIRDATE BETWEEN TO_DATE('{2}', 'YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{3}', 'YYYY-MM-DD HH24:MI:SS')  AND A.LINE_CODE='{1}'
                            AND A.COMMAND_CODE = '{0}'  AND A.RES = '报废'", strworkcode, strLinecode, strDate1, strDate2);
                            dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strselect);
                            if (dtm.Rows.Count > 0)
                            {
                                strBufNum = dtm.Rows[0][0].ToString();
                            }
                            string ISLINE = "CHINO-E";
                            strsql12 = "SELECT  WORKSHOP2 FROM WORKPRODUCE T WHERE  LINE='" + strLinecode + "'";
                            dtm = OracleHelper.ExecuteDataTable1(OracleHelper.OracleConnection, strsql12);
                            if (dtm.Rows.Count > 0)
                            {
                                ISLINE = dtm.Rows[0][0].ToString();
                            }
                            if (string.IsNullOrEmpty(ISLINE))
                            {
                                ISLINE = "CHINO-E";
                            }
                            DataRow dr = exid.NewRow();
                            dr = exid.NewRow();
                            //dr[0] = "";
                            dr[1] = "";
                            dr[2] = "";
                            dr[3] = "";
                            dr[4] = "CHINO-E";
                            dr[5] = Center_Order_Id;
                            dr[6] = strworkcode;
                            dr[7] = "MMI2";
                            dr[8] = Date3;
                            dr[9] = Date4;
                            dr[10] = "";
                            dr[11] = Plan_Line;
                            dr[12] = strLinecode;
                            dr[13] = strGoodNum1;//一次投入数 
                            dr[14] = strGoodNum1;//投入数 
                            dr[15] = strGoodNum;//通过数
                            dr[16] = strBufNum;//报废数
                            dr[17] = strRepNum;//维修数 P_BFNUMBER
                            dr[18] = strRepNum;//维修通过数
                            dr[19] = Convert.ToString(Convert.ToInt32(strRepNum) - Convert.ToInt32(strBufNum));//维修重投数
                            dr[20] = "0";//返工数
                            dr[21] = "0";//返工后通过数
                            dr[22] = "0";//返工后投入数
                            dr[23] = strErrNum;//不良数
                            dr[24] = strErrNum;//首次不良数 
                            dr[25] = Convert.ToString(Convert.ToInt32(strGoodNum1) - Convert.ToInt32(strErrNum)); //一次性通过数 
                            dr[26] = "0";//误测数 
                            dr[27] = MAT_MODEL;
                            dr[28] = EMS_MAT_ID;
                            dr[29] = EMS_MAT_ID;
                            dr[30] = "MESMMI2";
                            dr[31] = datends;
                            dr[32] = "";
                            dr[33] = "";
                            dr[34] = "";
                            dr[35] = datends1;
                            dr[36] = 0;
                            dr[37] = "";
                            dr[38] = null;//CHINO-E
                            dr[39] = ISLINE;
                            if (strGoodNum1 != "0")
                            {
                                exid.Rows.Add(dr);
                            }
                        }
                    }
                });
            }
            int totalRow = 0;
            if (exid.Rows.Count > 0)
            {
                totalRow = exid.Rows.Count;
                string TableName = "[HWOUTPUT].[dbo].[First_Pass_Yield_Data]";
                AppHelper.dBtnInsert(exid, TableName, totalRow);
            }
            //throw new NotImplementedException();
            errMessage.success = true;
            errMessage.Err = "执行完成Hw_RepairMes:" + totalRow;
            return errMessage;
        }
    } 
}
