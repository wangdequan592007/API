using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API_MES.Dtos;
using API_MES.Entities;
using EF_ORACLE.Model;
using EF_ORACLE.Model.TMES;
using Microsoft.AspNetCore.Mvc;

namespace API_MES.Servise
{
    public interface IMesRepository
    {
        Task<BARCODEREMP> GetBARCODEREMP(string MO,string SN);
        Task<ODM_IMEILINKNETCODE> GetODM_IMEILINKNETCODE(string MO, string SN);
        Task<ODM_IMEILINKNETCODE> GetSNBY_IMEILINKNETCODE(string SN);
        Task<IEnumerable<ODM_IMEILINKNETCODE>> GetLISTIMEIL(string MO);
        Task<IEnumerable<FQA_FIRSTARTICLE>> GetICMo1(string MO);
        Task<IEnumerable<FQA_FIRSTARTICLELINE>> GetICMo2(string MO, string LINE);
        Task<IEnumerable<LOCKMO>> GetODMOLOCK(string MO, string LINE, string DATE1, string DATE2);
        Task<IEnumerable<FQA_FIRSTARTICLEDtos>> FIRSTARTICLEs(string MO, string BOM, string DATE1, string DATE2, string MTYPE);
        Task<bool> SaveAsync();
        Task<IEnumerable<FQA_MIDTIME>> GetMidTimeAsync(string ITEMCODE, string TPROCESS, string DTIME1,string DTIME2);
        Task<IEnumerable<ModelMO>> GetMO(string mo);
        //lINE  ODM_MODEL
        Task<IEnumerable<WORKPRODUCE>> GetLINE(int nb);

        Task<IEnumerable<ODM_BEATE>> GetBEATELINE(int nb);

        void AddFQA_FIRSTARTICLELINE(FQA_FIRSTARTICLELINE  fQA_FIRSTARTICLELINE);
        void UpdateFQA_FIRSTARTICLELINE(FQA_FIRSTARTICLELINE  fQA_FIRSTARTICLELINE);
        void AddFQA_FIRSTARTICLE(FQA_FIRSTARTICLE  fQA_FIRSTARTICLE);
        void UpdateFQA_FIRSTARTICLE(FQA_FIRSTARTICLE  fQA_FIRSTARTICLE);
        void DeleteFQA_FIRSTARTICLE(FQA_FIRSTARTICLE  fQA_FIRSTARTICLE);
        void DeleteFQA_FIRSTARTICLELINE(FQA_FIRSTARTICLELINE  fQA_FIRSTARTICLELINE);
        Task<IEnumerable<FQA_MIDTIME>> GetFQA_MIDTIMEAsync(FQA_MIDTIME model);
        Task<FQA_MIDTIME> GetFQA_MIDTIMEFirstAsync(string model);
        void AddFQA_MIDTIME(FQA_MIDTIME fQA_MIDTIME);
        void AddLOCKMO(LOCKMO  lOCKMO);
        void AddFQA_FIRSTARTICLELINELOG(FQA_FIRSTARTICLELINELOG  fQA_FIRSTARTICLELINELOG);
        void UpdateFQA_MIDTIME(FQA_MIDTIME  fQA_MIDTIME);
        void DeleteFQA_MIDTIME(FQA_MIDTIME  fQA_MIDTIME);
        void AddFQA_FIRSTARTICLEIMG(FQA_FIRSTARTICLEIMG  fQA_FIRSTARTICLEIMG);
        bool DeleteFQA_FIRSTARTICLEIMG(string MO,string LINE,string T);
        Task<IEnumerable<FQA_FIRSTARTICLEIMG>> GetfQA_FIRSTARTICLEIMG(string MO, string LINE, string T);
        Task<FQA_FIRSTARTICLEIMG> GetfQA_FIRSTARTICLEIMGAsync(string IMGLIST);
        void DeleteFQA_FIRSTARTICLEIMGls(FQA_FIRSTARTICLEIMG  fQA_FIRSTARTICLEIMG);
        Task<IEnumerable<EDI_SSCCINFOLOG>>GetEDI_SSCCINFOLOG(string SSCC, string PALLET, string PASSORNG, string DATE1, string DATE2);
        Task<ODM_MODEL> GetODM_MODEL(string bom);
        void AddODM_TEMPERATUREBOARD(ODM_TEMPERATUREBOARD  oDM_TEMPERATUREBOARD);
        void AddODM_TEMPERATUREBOARD_DTL(ODM_TEMPERATUREBOARD_DTL  oDM_TEMPERATUREBOARD_DTL);
        Task<ODM_TEMPERATUREBOARD> GetODM_TEMPERATUREBOARD(string SN);
        Task<IEnumerable<ODM_TEMPERATUREBOARD>> GetTEMPERATUREBOARD(string SN);
        Task<IEnumerable<ODM_TEMPERATUREBOARD_DTL>> GetODM_TEMPERATUREBOARD_DTL(string SN);
        Task<IEnumerable<ODM_TEMPERATUREBOARD_DTOS>> GetODM_TEMPERATUREBOARD_DTOS(string SN);
        void UpdateTEMPERATUREBOARD(ODM_TEMPERATUREBOARD oDM_TEMPERATUREBOARD);
        void AddFQA_LINETIMEOW(FQA_LINETIMEOW  fQA_LINETIMEOW);
        bool DelFQA_LINETIMEOW(string id);
        Task<IEnumerable<FQA_LINETIMEOW>> GetFQA_LINETIMEOW(string LINE);
        Task<IEnumerable<FQA_CK_SNLOG>> GetFQA_CK_SNLOG(string MO, string SN, string LINE, string TY, string DATE1, string DATE2, string ISC,string ISOK,string Ems);
        Task<IEnumerable<FQA_CK_SNLOGV>> GetFQA_CK_SNIMEILOG(string MO, string SN, string IMEI, string LINE, string TY, string DATE1, string DATE2, string ISC, string ISOK, string Ems, string Ts);
        bool DelODM_SIDKEYTEMP();
        void AddODM_SIDKEYTEMP(ODM_SIDKEYTEMP oDM_SIDKEYTEMP);
        Task<IEnumerable<ODM_SIDKEYTEMP>> ListODM_SIDKEYTEMP();
        Task<IEnumerable<ODM_SIDKEYTEMP>> GetODM_SIDKEYTEMP();
        void AddODM_SIDKEYLOG(ODM_SIDKEYLOG oDM_SIDKEYLOG);
        Task<ODM_SIDKEY> GetODM_SIDKEYLOG(string Sid);
        int GetODM_SIDKEYCT();
        bool SaveODM_SIDKEYTEMP();
        void AddODM_LOCKREWORK(ODM_LOCKREWORK  oDM_LOCKREWORK);
        Task<MESTOOL> GetMESTOOL(string TOOL);
        Task<IEnumerable<ODM_LOCKREWORK>> GetODM_LOCKREWORK(string SN,string MO,string ISCK,string DATE1, string DATE2);
        Task<ODM_LOCKREWORK> GetODM_LOCKREWORKDID(string DID);
        void UpdateODM_LOCKREWORK(ODM_LOCKREWORK  oDM_LOCKREWORK);
        Task<MTL_SUB_ATTEMPER> GetMTL_SUB_ATTEMPER(string MO,string TEop); 
        Task<FQA_OBARESHUOTIME> GetFQA_OBARESHUOTIME(string Itemcode);
        Task<FQA_OBAITEMTIME> GetFQA_OBAITEMTIME(string Itemcode); 
        void AddFQA_OBARESHUOTIME(FQA_OBARESHUOTIME  fQA_OBARESHUOTIME);
        void UpdateFQA_OBARESHUOTIME(FQA_OBARESHUOTIME fQA_OBARESHUOTIME);
        Task<IEnumerable<FQA_OBARESHUOTIME>> ListFQA_OBARESHUOTIME(string BOM,string DATE1,string DATE2);
        void DeleteFQA_OBARESHUOTIME(FQA_OBARESHUOTIME fQA_OBARESHUOTIME);
        Task<IEnumerable<ODM_BEATEDtos>> GetBEATELINEMODEL();
        Task<ErrMessage> ErrMessage(string SN1, string SN2, string SN3, int TY);
        Task<IEnumerable<RE_MODEL>> GetRE_MODEL();
        Task<IEnumerable<RE_BUG_TYPE>> GetRE_BUG_TYPE(string CODE, string MODEL, string TYPE);
        Task<ErrMessage> GetRE_CODE(string cODE, string mODEL, string tYPE);
        Task<ErrMessage> ADD_RE_BUG_TYPE(string v_CLIENNAME, string v_CLIENCODE, string v_REASONTYPE, string v_CODE, string v_NAME, string v_DESCRIBE,string v_BUG_TYPE, string v_WORKCODE);
        Task<ErrMessage> CHA_RE_BUG_TYPE(string v_ID, string v_NAME, string v_DESCRIBE, string v_BUG_TYPE);
        Task<ErrMessage> DEL_RE_BUG_TYPE(string v_ID);
        Task<ErrMessage> COPY_RE_BUG_TYPE(string v_CLIENT, string v_CLIENT1, string v_REASONTYPE); 
        Task<IEnumerable<SelModel>> FQA_ERRTY(string code);
        Task<IEnumerable<SelModel>> FQA_ERRTYCODE(string Tcode,string code);
        ErrMessage FQA_OBA_PASS(string v_ID);
        Task<IEnumerable<Model.FQA_OBA_SAMPLE>> GetFQA_OBA_SAMPLE(string iD);
        bool UpdateFQA_CK_SN(string SN, string USER);
        Task<IEnumerable<EDI_850appendix>> G_850appendix(string po);
        void Up850appendix(string pONUMBER, string sHIPEARLYSTATUS, string sTOP_RELEASE, string sTOP_SHIP);

        Task<ODM_PRESSUREMODEL> G_ODM_PRESSUREMODEL(string cLIENCODE);

        void AddODM_TESTFAILNUM(ODM_TESTFAILNUM  oDM_TESTFAILNUM);
        void UpdateODM_TESTFAILNUM(ODM_TESTFAILNUM oDM_TESTFAILNUM);
        Task<ODM_TESTFAILNUM> GetODM_TESTFAILNUM(string STATION); 
        Task<IEnumerable<ODM_TESTFAILNUM>> GetLIST_TESTFAILNUM();
        Task<ErrMessage> DelODM_TESTFAILNUM(string sTATION);
        Task<IEnumerable<ORT_BARCODE_NEW>> GetOrt_barcode(string v_MO, string v_SN, string v_EMS_M2, string v_DATE1, string v_DATE2, string v_EMS_M1);
        Task<ErrMessage> UpDateORT(string iD, string pCUSER, string oRT);
        int GetODM_SIDKEYCTBYMODEL(string code);
        Task<IEnumerable<Model.SFAA_T>> GetSFAA_T(string Mo);
        Task<ErrMessage> mULCHING21(string bARCODE, string cLIENTCODE, string mO, string T100CT);
        Task<ErrMessage> mULCHING33(string bARCODE, string cLIENTCODE);
        Task<ErrMessage> mULCHING3(string bARCODE, string cLIENTCODE, string lCMBARCODE, string tOPBARCODE, string UserID, string mO, string lINE,string T100CT);
        Task<ErrMessage> mULCHING4(string bARCODE, string hOUR,string UserID);
        Task<IEnumerable<ODM_PACKINGELSE>> G_ODM_PACKINGELSE(string v_MO, string dATE1, string sID, string cARTONID);
        Task<RT_MESSAGE> GetODM_PRESSURE(string mO, string sN, string dATE1, string dATE2, string sTATION, string lINE, string cLIENTCODE);
        Task<RT_MESSAGE> GetODM_PRESSUREFREE(string sN, string dATE1, string dATE2, string iD_USER);
    }
}
