using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EF_SQL;
using Microsoft.EntityFrameworkCore;
namespace API_MES.Servise
{
    public interface ISqlRepository
    {
        Task<bool> SaveAsync();
        void AddCompany(Company  company);
        Task<IEnumerable<Company>> GetCompany();
        void AddDA_BOM(DA_BOM  dA_BOM);
        Task<DA_BOM> GetDA_BOM(string Boms);
        Task<DA_BOM> GetDA_BOMPrice(string Boms);
        void UpdateDA_BOM(DA_BOM dA_BOM);
        Task<DA_BOM> GetPrice(string Guids);
        void DeletePrice(DA_BOM  dA_BOM);
        Task<IEnumerable<DA_BOM>> GetBom(string Boms, string dATE1, string dATE2,string clientname);
        Task<DA_ELSEGROSS> GtDA_ELSEGROSS(DateTime dateTime,int ty,string cl,string code);
        void AddDA_ELSEGROSS(DA_ELSEGROSS  dA_ELSEGROSS);
        void UpdateDA_ELSEGROSS(DA_ELSEGROSS dA_ELSEGROSS);
        void AddDA_GROSS(DA_GROSS dA_GROSS);
        void AddDA_PAYPERSON(DA_PAYPERSON  dA_PAYPERSON);
        void AddDA_PAYLOSS(DA_PAYLOSS  dA_PAYLOSS);
        void AddDA_PAYMONTH(DA_PAYMONTH  dA_PAYMONTH);
        Task<IEnumerable<DA_GROSS>> GetGROSS(string MO, string CLIENTNAME, string MANAGER, string DATE1, string DATE2);
        void DeleteGROSS(DA_GROSS  dA_GROSS);
        Task<DA_GROSS> GetGROSS(string Guids);
        Task<IEnumerable<DA_ELSEGROSS>> GetELSEGROSS(string cLIENTNAME, string mODEL, string dATE1, string dATE2);
        Task<DA_ELSEGROSS> GetELSEGROSS(string Guids);
        void DeleteELSEGROSS(DA_ELSEGROSS dA_GROSS);
        Task<IEnumerable<DA_PAYPERSON>> GetPAYPERSON(string cLIENTNAME, string mODEL, string dATE1, string dATE2);
        Task<IEnumerable<DA_PAYLOSS>> GetPAYLOSS(string cLIENTNAME, string mODEL, string dATE1, string dATE2);
        Task<IEnumerable<DA_PAYMONTH>> GetPAYMONTH(string cLIENTNAME, string mODEL, string dATE1, string dATE2);
        Task<DA_PAYPERSON> GetPAYPERSON(string Guids);
        void DeletePAYPERSON(DA_PAYPERSON dA_GROSS);
        Task<DA_PAYLOSS> GetPAYLOSS(string Guids);
        void DeletePAYLOSS(DA_PAYLOSS dA_GROSS);
        Task<DA_PAYMONTH> GetPAYMONTH(string Guids);
        void DeletePAYMONTH(DA_PAYMONTH dA_GROSS);
        Task<IEnumerable<DA_VIEW>> getDA_VIEW(string dATE1, string cLIENTNAME);
        Task<IEnumerable<DA_VIEW>> getDA_VIEWMODEL(string dATE1, string cLIENTNAME);
        Task<IEnumerable<DA_CLENTNAME>> GET_CLENTNAME  (string NAME);
        Task<IEnumerable<DA_CLENTNAME>> GET_CLENTNAMEBU1(string NAME);
        Task<DA_CLENTNAME> GET_CLENTNAME1(string NAME);
        void AddDA_CLENTNAME(DA_CLENTNAME dA_CLENTNAME);
        void UpdateDA_CLENTNAME(DA_CLENTNAME dA_CLENTNAME);
        void DeleteCLENTNAME(DA_CLENTNAME dA_);
        Task<IEnumerable<DA_CLENTCODE>> GetDA_CLENTCODEs();
        Task<IEnumerable<DA_STAORE>> GetDA_STAOREs();
        void ADD_INSTORE(DT_INSTORE  dT_INSTORE);
        Task<IEnumerable<DT_INSTORE>> GetINSTAL(string cLIENTNAME, string mODEL, string dATE1, string dATE2);
        void DE_INSTORE(DT_INSTORE dA_);
        Task<DT_INSTORE> GetINSTORE(string guids);
        void AddDA_DATEINSTORE(DA_DATEINSTORE  dA_DATEINSTORE);
        void AddDA_ATTENDANCE(DA_ATTENDANCE dA_PAYMONTH);
    }
}
