using API_MES.Dtos;
using API_MES.Entities;
using API_MES.Model;
using EF_ORACLE;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Servise
{
    public interface IRepairRepository
    {
        Task<IEnumerable<REPAIDtos>> GetRepairList(string code, string date1, string date2);
        Task<RTrEPAIDtos> GetRepairListCount(string code, string date1, string date2);
        System.Data.DataTable GetRepairListTB(string code, string date1, string date2);
        Task<ErrMessage> G_XMDATEY(string code);
        Task<ErrMessage> G_REPairDATa(string code, string date);
        Task<IEnumerable<REPAIDtos>> GetRepairListByDate(string code, string date1);
        Task<RTrEPAIDtos> GetRepairListCountByTable(string code, string date1, string date2);
        Task<IEnumerable<REPAIDtos>> GetRepairListByDateByTable(string code, string date1, string date2);

        Task<IEnumerable<SenDataList>> GetSenDataListByTable(string code, string date1, string date2);
        Task<ListDataList> GetDataListByTable(string code, string date1, string date2);
        Task<ErrMessage> G_PressureTime(string code);
        Task<ErrMessage> G_PressureWg(string code, string sn,string user);
        Task<ErrMessage> AddPressuretime(string cLIENCODE, string dTTIME, string nUMCODE, string wGSTATION);
        Task<ODM_PRESSURETIME> GetODM_PRESSURETIME(string code);
        Task<ErrMessage> G_HwReDATE(string code);
        Task<ErrMessage> Hw_RepairIms(string date1);
        Task<ErrMessage> Hw_RepairMes(string date1);
        Task<DT_BEAT> BEAT_OUTTT(string lINE, string cODE, string mODEL);
        bool IODM_BEATE(string LINE);
        Task<ErrMessage> Hw_FtyIms(string date1);
        Task<ErrMessage> Hw_FtyMes(string date1);
        Task<ErrMessage> Hw_FtyMesMmi2(string date1);
    }
}
