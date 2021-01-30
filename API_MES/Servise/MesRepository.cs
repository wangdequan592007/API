using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API_MES.Dtos;
using API_MES.Entities;
using API_MES.Helper;
using EF_ORACLE;
using EF_ORACLE.Model;
using EF_ORACLE.Model.TMES;
using Microsoft.EntityFrameworkCore;

namespace API_MES.Servise
{
    public class MesRepository : IMesRepository
    {
        private readonly MESContext _context;
        //private readonly IPropertyMappingService _propertyMappingService;
        public MesRepository(MESContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //_propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService)); 
        }

        public void AddFQA_FIRSTARTICLELINE(FQA_FIRSTARTICLELINE fQA_FIRSTARTICLELINE)
        {
            //using (MESContext dbContext = new MESContext())
            //{
            //    {
            //        try
            //        {
            //            string sql = "Update SysUser Set  Name='Ricahrd老师-小王子' WHERE Id=@Id";
            //            SqlParameter parameter = new SqlParameter("@Id", 1);
            //            dbContext.Database.ExecuteSqlRaw(sql, parameter);
            //        }
            //        catch (Exception ex)
            //        {
            //            throw ex;
            //        }
            //    }
            //}
            //try
            //{
            //    string sql = "Update SysUser Set  Name='Ricahrd老师-小王子' WHERE Id=@Id";
            //    SqlParameter parameter = new SqlParameter("@Id", 1);
            //    _context.Database.ExecuteSqlRaw(sql, parameter);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            _context.fQA_FIRSTARTICLELINEs.Add(fQA_FIRSTARTICLELINE);
        }
        public void AddFQA_FIRSTARTICLE(FQA_FIRSTARTICLE fQA_FIRSTARTICLELINE)
        {
            _context.fQA_FIRSTARTICLEs.Add(fQA_FIRSTARTICLELINE);
        }
        public async Task<IEnumerable<FQA_FIRSTARTICLE>> GetICMo1(string MO)
        {
            if (MO == null)
            {
                throw new ArgumentNullException(nameof(MO));
            } 
            return await _context.fQA_FIRSTARTICLEs.Where(x => x.MO == MO)
                                                   .ToListAsync();
        }

        public async Task<IEnumerable<FQA_FIRSTARTICLELINE>> GetICMo2(string MO,string LINE)
        {
            if (MO == null)
            {
                throw new ArgumentNullException(nameof(MO));
            } 
            return await _context.fQA_FIRSTARTICLELINEs.Where(x => x.MO == MO&&x.LINE==LINE)
                                                   .ToListAsync();
        }

        public async Task<IEnumerable<WORKPRODUCE>> GetLINE(int nb)
        {
            if (nb==1)
            {
                return await _context.wORKPRODUCEs.Where(x => x.LINE.Contains("TP-A") || x.LINE.Contains("TP3") || x.LINE.Contains("TP-B")).ToListAsync();
            }
            if (nb == 2)
            {
                return await _context.wORKPRODUCEs.Where(x => x.LINE.Contains("TP") || x.LINE.Contains("AY")).Where(x=>x.WORKSHOP2== "CHINO-E").OrderBy(x=>x.LINE).ToListAsync();
            }
            if (nb == 3)
            {
                return await _context.wORKPRODUCEs.Where(x =>  x.LINE.Contains("点胶")).OrderBy(x => x.LINE).ToListAsync();
            }
            return await _context.wORKPRODUCEs.ToListAsync();
        }

        public async Task<IEnumerable<FQA_MIDTIME>> GetMidTimeAsync(string ITEMCODE, string TPROCESS, string DTIME1, string DTIME2)
        {
            var items = _context.MidTimes as IQueryable<FQA_MIDTIME>; 
            if (!string.IsNullOrWhiteSpace(ITEMCODE))
            {
                items = items.Where(x => x.ITEMCODE == ITEMCODE.Trim()); 
            }
            if (!string.IsNullOrWhiteSpace(TPROCESS))
            {
                items = items.Where(x => x.DTYPE == Convert.ToInt32(TPROCESS.Trim())); 
            }
            if (!string.IsNullOrWhiteSpace(DTIME1))
            {
                items = items.Where(x => x.INPUTDATE >= Convert.ToDateTime(DTIME1.Trim()) && x.INPUTDATE <= Convert.ToDateTime(DTIME2.Trim()));
            }
            return await items.OrderBy(x => x.INPUTDATE)
                .ToListAsync();
        }

        public async Task<IEnumerable<ModelMO>> GetMO(string mo)
        {
            //var listCt = _context.ModelMOs.Where(x => x.WORKJOB_CODE.Contains(mo)).Count();
            //if (listCt > 0)
            //原生sql查询
           
            if (!string.IsNullOrWhiteSpace(mo))
            { 
                //var query = await (from b in _appContext.ModelMOs where b.WORKJOB_CODE == mo select b).ToListAsync();
                return await _context.ModelMOs.Where(x => x.WORKJOB_CODE.Contains(mo)||x.WORKORDER.Contains(mo)).OrderByDescending(x => x.INPUTDATE).Take(50).ToListAsync();
            }
            else
            {
                return await _context.ModelMOs.OrderByDescending(x => x.INPUTDATE).Take(50).ToListAsync();
            }
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void UpdateFQA_FIRSTARTICLELINE(FQA_FIRSTARTICLELINE fQA_FIRSTARTICLELINE)
        {
            //throw new NotImplementedException();
            // _context.fQA_FIRSTARTICLELINEs.Update(fQA_FIRSTARTICLELINE);
            //_context.fQA_FIRSTARTICLELINEs.Attach(fQA_FIRSTARTICLELINE);//把user附加到上下文来管理
        }
        public void UpdateFQA_FIRSTARTICLE(FQA_FIRSTARTICLE fQA_FIRSTARTICLE)
        {
            //throw new NotImplementedException();
            // _context.fQA_FIRSTARTICLEs.Update(fQA_FIRSTARTICLE);
           // _context.fQA_FIRSTARTICLEs.Attach(fQA_FIRSTARTICLE);//把user附加到上下文来管理

        }

        public void DeleteFQA_FIRSTARTICLE(FQA_FIRSTARTICLE fQA_FIRSTARTICLE)
        {
            _context.fQA_FIRSTARTICLEs.Remove(fQA_FIRSTARTICLE);
        }

        public void DeleteFQA_FIRSTARTICLELINE(FQA_FIRSTARTICLELINE fQA_FIRSTARTICLELINE)
        {
            _context.fQA_FIRSTARTICLELINEs.Remove(fQA_FIRSTARTICLELINE);
        }

        public async Task<IEnumerable<FQA_MIDTIME>> GetFQA_MIDTIMEAsync(FQA_MIDTIME model)
        {
           return await _context.MidTimes.Where(x => x.ITEMCODE == model.ITEMCODE).ToListAsync();

        }

        public async Task<FQA_MIDTIME> GetFQA_MIDTIMEFirstAsync(string model)
        {
            return await _context.MidTimes.FirstOrDefaultAsync(x => x.ITEMCODE == model);
        }

        public void AddFQA_MIDTIME(FQA_MIDTIME fQA_MIDTIME)
        {
            _context.MidTimes.Add(fQA_MIDTIME); 
        }

        public void UpdateFQA_MIDTIME(FQA_MIDTIME fQA_MIDTIME)
        {
            //throw new NotImplementedException(); 
            _context.MidTimes.Update(fQA_MIDTIME);
        }

        public void DeleteFQA_MIDTIME(FQA_MIDTIME fQA_MIDTIME)
        {
            _context.MidTimes.Remove(fQA_MIDTIME);
        }

        public void AddLOCKMO(LOCKMO lOCKMO)
        {
            _context.lOCKMOs.Add(lOCKMO);
        }

        public void AddFQA_FIRSTARTICLEIMG(FQA_FIRSTARTICLEIMG fQA_FIRSTARTICLEIMG)
        {
            _context.fQA_FIRSTARTICLEIMGs.Add(fQA_FIRSTARTICLEIMG);
        }

        public  bool  DeleteFQA_FIRSTARTICLEIMG(string MO, string LINE, string T)
        {
            int intype = Convert.ToInt32(T);
            var fQA_FIRSTARTICLEIMG = _context.fQA_FIRSTARTICLEIMGs.Where(x => x.MO == MO && x.LINE == LINE && x.MTYPE == intype).ToList();
            if (fQA_FIRSTARTICLEIMG != null)
            {
                foreach (FQA_FIRSTARTICLEIMG item in fQA_FIRSTARTICLEIMG)
                {
                    _context.fQA_FIRSTARTICLEIMGs.Remove(item);
                }
                return true;
            }
            else
            {
                return false;
            } 
            
        }

        public async Task<IEnumerable<FQA_FIRSTARTICLEIMG>> GetfQA_FIRSTARTICLEIMG(string MO, string LINE, string T)
        {
            int intype = Convert.ToInt32(T);
            return await _context.fQA_FIRSTARTICLEIMGs.Where(x => x.MO == MO && x.LINE == LINE && x.MTYPE == intype).ToListAsync();
        }

        public async Task<FQA_FIRSTARTICLEIMG> GetfQA_FIRSTARTICLEIMGAsync(string IMGLIST)
        {
            return await _context.fQA_FIRSTARTICLEIMGs.FirstOrDefaultAsync(x => x.IMGLIST == IMGLIST);
        }

        public void DeleteFQA_FIRSTARTICLEIMGls(FQA_FIRSTARTICLEIMG fQA_FIRSTARTICLEIMG)
        {
            _context.fQA_FIRSTARTICLEIMGs.Remove(fQA_FIRSTARTICLEIMG);
        }

        public async Task<IEnumerable<ODM_IMEILINKNETCODE>> GetLISTIMEIL(string MO)
        {

            if (string.IsNullOrWhiteSpace(MO))
            {
                return null;
            }
            List<ODM_IMEILINKNETCODE> LioDM_IMEILINKNETCODEs = new List<ODM_IMEILINKNETCODE>();
            await Task.Run(() =>
            { 
                string sql = $"SELECT PHYSICSNO,WORKORDER,SN,IMEI2,MEID FROM ODM_IMEILINKNETCODE X WHERE x.WORKORDER = '{MO}'";
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        ODM_IMEILINKNETCODE oDM_IMEILINKNETCODE = new ODM_IMEILINKNETCODE();
                        oDM_IMEILINKNETCODE.WORKORDER = dataTable.Rows[i]["WORKORDER"].ToString();
                        oDM_IMEILINKNETCODE.PHYSICSNO = dataTable.Rows[i]["PHYSICSNO"].ToString();
                        oDM_IMEILINKNETCODE.IMEI2 = dataTable.Rows[i]["IMEI2"].ToString();
                        oDM_IMEILINKNETCODE.MEID = dataTable.Rows[i]["MEID"].ToString();
                        oDM_IMEILINKNETCODE.SN = dataTable.Rows[i]["SN"].ToString();
                        LioDM_IMEILINKNETCODEs.Add(oDM_IMEILINKNETCODE);
                    }
                    
                }
                else
                {
                    LioDM_IMEILINKNETCODEs = null;
                }
            });
            return LioDM_IMEILINKNETCODEs;
            //return await _context.oDM_IMEILINKNETCODEs.Where(x => x.WORKORDER == MO).ToListAsync();
        }

        public async Task<BARCODEREMP> GetBARCODEREMP(string MO, string SN)
        {
            string sql = $"SELECT BARCODE,WORKCODE,LINKSN FROM BARCODEREMP X WHERE 1=1";
            BARCODEREMP bARCODEREMPs = new BARCODEREMP();
            if (string.IsNullOrWhiteSpace(SN))
            {
                return null; 
            }
            await Task.Run(() =>
            { 
                sql += $" AND BARCODE='{SN}'";
                if (!string.IsNullOrWhiteSpace(MO))
                {
                    sql += $" AND WORKCODE='{MO}'";
                }
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    bARCODEREMPs.WORKCODE = dataTable.Rows[0]["WORKCODE"].ToString();
                    bARCODEREMPs.BARCODE = dataTable.Rows[0]["BARCODE"].ToString();
                    bARCODEREMPs.LINKSN = dataTable.Rows[0]["LINKSN"].ToString();
                }
                else
                {
                    bARCODEREMPs = null;
                }
            }); 
            return bARCODEREMPs;
            //if (string.IsNullOrWhiteSpace(SN))
            //{
            //    return null;
            //}
            //var ls= _context.bARCODEREMPs.Where(x => x.BARCODE == SN);
            //if (!string.IsNullOrWhiteSpace(MO))
            //{
            //    ls = ls.Where(x => x.WORKCODE == MO);
            //}
            //if (ls==null)
            //{
            //    return null;
            //}
            //return await ls.FirstOrDefaultAsync();
        }

        public async Task<ODM_IMEILINKNETCODE> GetODM_IMEILINKNETCODE(string MO, string SN)
        {
            if (string.IsNullOrWhiteSpace(SN))
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(MO))
            {
                return null;
            }
            ODM_IMEILINKNETCODE oDM_IMEILINKNETCODE = new ODM_IMEILINKNETCODE();
            await Task.Run(() =>
            {
                List<ODM_IMEILINKNETCODE> LioDM_IMEILINKNETCODEs = new List<ODM_IMEILINKNETCODE>();
                string sql = $"SELECT PHYSICSNO,WORKORDER,SN,IMEI2,MEID FROM ODM_IMEILINKNETCODE X WHERE (x.SN = '{SN}' OR x.PHYSICSNO = '{SN}' OR x.IMEI2 = '{SN}' OR x.MEID = '{SN}') AND x.WORKORDER = '{MO}'";
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    oDM_IMEILINKNETCODE.WORKORDER = dataTable.Rows[0]["WORKORDER"].ToString();
                    oDM_IMEILINKNETCODE.PHYSICSNO = dataTable.Rows[0]["PHYSICSNO"].ToString();
                    oDM_IMEILINKNETCODE.IMEI2 = dataTable.Rows[0]["IMEI2"].ToString();
                    oDM_IMEILINKNETCODE.MEID = dataTable.Rows[0]["MEID"].ToString();
                    oDM_IMEILINKNETCODE.SN = dataTable.Rows[0]["SN"].ToString();
                }
                else
                {
                    oDM_IMEILINKNETCODE = null;
                }
            });
            return oDM_IMEILINKNETCODE;
            //var ls = _context.oDM_IMEILINKNETCODEs.Where(x => x.WORKORDER == MO).Where(x=>x.SN==SN||x.PHYSICSNO==SN||x.IMEI2==SN||x.MEID==SN);
            //return await ls.FirstOrDefaultAsync();

        }

        public void AddFQA_FIRSTARTICLELINELOG(FQA_FIRSTARTICLELINELOG fQA_FIRSTARTICLELINELOG)
        {
            _context.fQA_FIRSTARTICLELINELOGs.Add(fQA_FIRSTARTICLELINELOG);
        }

        public async Task<IEnumerable<LOCKMO>> GetODMOLOCK(string MO, string LINE, string DATE1, string DATE2)
        {
            //throw new NotImplementedException();
            var items = _context.lOCKMOs as IQueryable<LOCKMO>;
            if (!string.IsNullOrWhiteSpace(MO))
            {
                items = items.Where(x => x.MO == MO);
            }
            if (!string.IsNullOrWhiteSpace(LINE))
            {
                items = items.Where(x => x.LINE == LINE);
            }
            if (!string.IsNullOrWhiteSpace(DATE1))
            {
                items = items.Where(x => x.CRT_DATE >= Convert.ToDateTime(DATE1) && x.CRT_DATE <= Convert.ToDateTime(DATE2));
            }
           // var ddd= _context.lOCKMOs.ToListAsync();
            return await items.OrderBy(x => x.CRT_DATE)
                .ToListAsync();
        }

        public async Task<IEnumerable<FQA_FIRSTARTICLEDtos>> FIRSTARTICLEs(string MO, string BOM, string DATE1, string DATE2,string MTYPE)
        {
            var items = _context.fQA_FIRSTARTICLEs as IQueryable<FQA_FIRSTARTICLE>;
            if (!string.IsNullOrWhiteSpace(MO))
            {
                items = items.Where(x => x.MO == MO);
            }
            if (!string.IsNullOrWhiteSpace(DATE1))
            {
                items = items.Where(x => x.CREAT_DT >= Convert.ToDateTime(DATE1) && x.CREAT_DT <= Convert.ToDateTime(DATE2));
            }
            if (!string.IsNullOrWhiteSpace(MTYPE))
            {
                items = items.Where(x => x.MTYPE == Convert.ToInt32(MTYPE));
            }
            var items1 = _context.ModelMOs as IQueryable<ModelMO>;  
            if (!string.IsNullOrWhiteSpace(MO))
            {
                items1 = items1.Where(x => x.WORKJOB_CODE == MO);
            }
            if (!string.IsNullOrWhiteSpace(BOM))
            {
                items1 = items1.Where(x => x.ITEM_CODE == BOM);
            }
            
            var FQA_FIRSTARTICLEDtos = from q in items
                             from p in items1
                             where q.MO == p.WORKJOB_CODE
                             select new FQA_FIRSTARTICLEDtos
                             {MO= q.MO,BOM= p.ITEM_CODE,MTYPE= q.MTYPE,MODEL= q.MODEL,CREAT_DT=q.CREAT_DT,CREAT_US=q.CREAT_US,NOTES=q.NOTES};

            return await FQA_FIRSTARTICLEDtos.ToListAsync();
        }

        public async Task<IEnumerable<EDI_SSCCINFOLOG>> GetEDI_SSCCINFOLOG(string SSCC, string PALLET, string PASSORNG, string DATE1, string DATE2)
        {
            var items = _context.EDI_SSCCINFOLOG as IQueryable<EDI_SSCCINFOLOG>;
            if (!string.IsNullOrWhiteSpace(SSCC))
            {
                items = items.Where(x => x.SSCC == SSCC.Trim());
            }
            if (!string.IsNullOrWhiteSpace(PALLET))
            {
                items = items.Where(x => x.BOXSN == PALLET.Trim());
            }
            if (!string.IsNullOrWhiteSpace(PASSORNG))
            {
                items = items.Where(x => x.RESULT == PASSORNG.Trim());
            }
            if (!string.IsNullOrWhiteSpace(DATE1))
            {
                items = items.Where(x => x.CRT_DATE >= Convert.ToDateTime(DATE1.Trim()+" 00:00:00") && x.CRT_DATE <= Convert.ToDateTime(DATE2.Trim()+" 23:59:59"));
            }
            return await items.Take(500).ToListAsync();
        }

        public async Task<ODM_MODEL> GetODM_MODEL(string bom)
        {
            var dd1= await _context.ODM_MODEL.FirstOrDefaultAsync(x => x.BOM == bom);
            if (dd1==null)
            {
                if (bom.Contains("K01"))
                {
                    bom = bom.Substring(3);
                }
                dd1 = await _context.ODM_MODEL.FirstOrDefaultAsync(x => x.BOM == bom);
            }
            return dd1;
        }

        public void AddODM_TEMPERATUREBOARD(ODM_TEMPERATUREBOARD oDM_TEMPERATUREBOARD)
        {
            _context.ODM_TEMPERATUREBOARD.Add(oDM_TEMPERATUREBOARD);
        }

        public void AddODM_TEMPERATUREBOARD_DTL(ODM_TEMPERATUREBOARD_DTL oDM_TEMPERATUREBOARD_DTL)
        {
            _context.ODM_TEMPERATUREBOARD_DTL.Add(oDM_TEMPERATUREBOARD_DTL);
        }

        public async Task<ODM_TEMPERATUREBOARD> GetODM_TEMPERATUREBOARD(string SN)
        {
            return await _context.ODM_TEMPERATUREBOARD.Where(x => x.SN == SN).FirstOrDefaultAsync(); 
        }

        public void UpdateTEMPERATUREBOARD(ODM_TEMPERATUREBOARD oDM_TEMPERATUREBOARD)
        {
           // throw new NotImplementedException();
        }

        public async Task<IEnumerable<ODM_TEMPERATUREBOARD>> GetTEMPERATUREBOARD(string SN)
        {
            var items = _context.ODM_TEMPERATUREBOARD as IQueryable<ODM_TEMPERATUREBOARD>;
            if (!string.IsNullOrWhiteSpace(SN))
            {
                items = items.Where(x => x.SN == SN);
            }
            return await items.ToListAsync();
        }
        public async Task<IEnumerable<ODM_TEMPERATUREBOARD_DTL>> GetODM_TEMPERATUREBOARD_DTL(string SN)
        {
            var items = _context.ODM_TEMPERATUREBOARD_DTL as IQueryable<ODM_TEMPERATUREBOARD_DTL>;
            if (!string.IsNullOrWhiteSpace(SN))
            {
                items = items.Where(x => x.SN == SN);
            }
            return await items.ToListAsync();
        } 
        public async Task<IEnumerable<ODM_TEMPERATUREBOARD_DTOS>> GetODM_TEMPERATUREBOARD_DTOS(string SN)
        {
            //var result = persons.Join(cities, p => p.CityID, c => c.ID, (p, c) => new { PersonName = p.Name, CityName = c.Name });
            //  var result = _context.ODM_TEMPERATUREBOARD_DTL.GroupJoin(_context.ODM_TEMPERATUREBOARD, p => p.SN, c => c.SN, (p, cs) => new { PersonName = p.SN, Citys = cs });
            var tb1 = _context.ODM_TEMPERATUREBOARD as IQueryable<ODM_TEMPERATUREBOARD>;
            if (!string.IsNullOrWhiteSpace(SN))
            {
                tb1 = tb1.Where(x => x.SN == SN);
            }
            var tb2 = _context.ODM_TEMPERATUREBOARD_DTL as IQueryable<ODM_TEMPERATUREBOARD_DTL>;
            if (!string.IsNullOrWhiteSpace(SN))
            {
                tb2 = tb2.Where(x => x.SN == SN);
            }
            var dtos = tb1.Join(
                tb2, q=>q.SN,p=>p.SN,(q,p)=>
            new ODM_TEMPERATUREBOARD_DTOS
            {
                SN=q.SN, 
                FSTATUS = q.FSTATUS,
                FCOUNT=q.FCOUNT,
                FSTATUS_DTL=p.FSTATUS,
                CRTUSER = p.CRTUSER,
                CRTTIME = p.CRTTIME,
                REMART = p.REMART,
                DID = p.DID,
            });
            //var dtos1 = from q in _context.ODM_TEMPERATUREBOARD_DTL
            //            from p in _context.ODM_TEMPERATUREBOARD
            //            where q.SN == p.SN
            //           select new ODM_TEMPERATUREBOARD_DTOS
            //           {
            //               SN = q.SN,
            //               FSTATUS_DTL = q.FSTATUS,
            //               FCOUNT=p.FCOUNT,
            //               FSTATUS=p.FSTATUS,
            //               REMART=q.REMART,
            //               CRTTIME=q.CRTTIME,
            //               CRTUSER=q.CRTUSER
            //           }; 
            //var ddd1 = await dtos1.ToListAsync();
            var ddd = await dtos.ToListAsync();
            return ddd;
        }

        public void AddFQA_LINETIMEOW(FQA_LINETIMEOW lOCKMO)
        {
            _context.FQA_LINETIMEOW.Add(lOCKMO);
        } 
        public async Task<IEnumerable<FQA_LINETIMEOW>> GetFQA_LINETIMEOW(string LINE)
        {
            return await _context.FQA_LINETIMEOW.Where(x=>x.LINE==LINE).ToListAsync();
        }
        public bool DelFQA_LINETIMEOW(string id)
        {
            FQA_LINETIMEOW fQA_LINETIMEOW = _context.FQA_LINETIMEOW.Where(x => x.ID == id).FirstOrDefault();
            if (fQA_LINETIMEOW != null)
            {
                _context.FQA_LINETIMEOW.Remove(fQA_LINETIMEOW);
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool DelODM_SIDKEYTEMP()
        {
            string sql = @"DELETE ODM_SIDKEYTEMP";
            //_context.Database.ExecuteSqlCommand(sql);
            //_context.SaveChanges();
            int index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
            return true;
        }
        public async Task<IEnumerable<FQA_CK_SNLOG>> GetFQA_CK_SNLOG(string MO, string SN, string LINE, string TY, string DATE1, string DATE2, string ISC,string ISOK,string Ems)
        {
            IQueryable<FQA_CK_SNLOG> items = _context.FQA_CK_SNLOG;
            IQueryable<FQA_CK_SN> items1 = _context.FQA_CK_SN;
            //var dd1 = await items.FirstOrDefaultAsync();
            if (!string.IsNullOrWhiteSpace(SN))
            {
                items = items.Where(x => x.SN == SN);
                items1 = items1.Where(x => x.SN == SN);
            } 
            if (!string.IsNullOrWhiteSpace(MO))
            {
                items = items.Where(x => x.MO == MO);
                items1 = items1.Where(x => x.MO == MO);
            } 
            if (!string.IsNullOrWhiteSpace(ISOK))
            {
                items = items.Where(x => x.ISOK == Convert.ToInt32(ISOK));
                items1 = items1.Where(x => x.ISOK == Convert.ToInt32(ISOK));
            }
            if (!string.IsNullOrWhiteSpace(LINE))
            {
                items = items.Where(x => x.LINE == LINE);
                items1 = items1.Where(x => x.LINE == LINE);
            }
            if (!string.IsNullOrWhiteSpace(ISC))
            {
                if (ISC == "true")
                {
                    items = items.Where(x => x.CREATEDATE >= Convert.ToDateTime(DATE1.Trim()) && x.CREATEDATE <= Convert.ToDateTime(DATE2.Trim()));
                    items1 = items1.Where(x => x.CREATEDATE >= Convert.ToDateTime(DATE1.Trim()) && x.CREATEDATE <= Convert.ToDateTime(DATE2.Trim()));
                } 
            }
            
            if (!string.IsNullOrWhiteSpace(TY))
            {
                items1= items1.Where(x=>x.TLL== Convert.ToInt32(TY));
                //var dds =  items1.ToList();
                var list = items1.Select(u => u.TGUID).ToList();
                items = items.Where(u => list.Contains(u.TGUID)); 
            }
            if (!string.IsNullOrWhiteSpace(Ems))
            {
                if (Ems != "ZN")
                {
                    var molist = _context.ModelMOs.Where(x => x.EMS_CODE == Ems || x.CLIENTCODE == Ems);
                    //List<string> list = molist.Select(u => u.WORKJOB_CODE).ToList();
                    //items = items.Where(u => list.Contains(u.MO));
                    IQueryable<FQA_CK_SNLOG> dtos1 = from q in items
                                from p in molist
                                where q.MO == p.WORKJOB_CODE
                                select new FQA_CK_SNLOG
                                {
                                    SN = q.SN,
                                    CREATEDATE=q.CREATEDATE,
                                    PCNAME=q.PCNAME,
                                    SUBID=q.SUBID,
                                    TGUID=q.TGUID,
                                    TLL=q.TLL,
                                    T_DES=q.T_DES,
                                    ISOK=q.ISOK,
                                    LINE=q.LINE,
                                    MO=q.MO
                                };
                    //var dd = dtos1.ToListAsync();
                    //var cc = (IEnumerable<FQA_CK_SNLOG>)dd;
                    return dtos1;
                }
               
            }
            return await items.OrderBy(x=>x.CREATEDATE).ToListAsync();
        }
        public async Task<IEnumerable<FQA_CK_SNLOGV>> GetFQA_CK_SNIMEILOG(string MO, string SN, string IMEI, string LINE, string TY, string DATE1, string DATE2, string ISC, string ISOK, string Ems, string Ts)
        {
            List<FQA_CK_SNLOGV> lis = new List<FQA_CK_SNLOGV>();
            await Task.Run(() =>
            {
                string sql = @" SELECT TGUID, B.MO,B.SN,C.PHYSICSNO IMEI,LINE,ISOK,PCNAME,CREATEDATE,TLL,T_DES,SUBID  FROM FQA_CK_SNLOG B
                                LEFT JOIN ODM_IMEILINKNETCODE C ON C.SN = B.SN WHERE 1 =1";
                if (Ts=="1")
                {
                    sql = @" SELECT TGUID, B.MO,B.SN,C.PHYSICSNO IMEI,LINE,ISOK,PCNAME,CREATEDATE,TLL,T_DES,SUBID  FROM FQA_CK_SN B
                                LEFT JOIN ODM_IMEILINKNETCODE C ON C.SN = B.SN WHERE 1 =1";
                }
                if (!string.IsNullOrWhiteSpace(MO))
                {
                    sql += $" AND MO='{MO}'";
                }
                if (!string.IsNullOrWhiteSpace(SN))
                {
                    sql += $" AND B.SN='{SN}'";
                }
                if (!string.IsNullOrWhiteSpace(IMEI))
                {
                    sql += $" AND C.PHYSICSNO='{IMEI}'";
                }
                if (!string.IsNullOrWhiteSpace(LINE))
                {
                    sql += $" AND LINE='{LINE}'";
                }
                if (!string.IsNullOrWhiteSpace(ISOK))
                {
                    sql += $" AND ISOK='{ISOK}'";
                }
                if (!string.IsNullOrWhiteSpace(ISC))
                {
                    if (ISC == "true")
                    {
                        sql += $" AND CREATEDATE BETWEEN TO_DATE('{DATE1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{DATE2}','YYYY-MM-DD HH24:MI:SS')"; 
                    }
                }
                if (!string.IsNullOrWhiteSpace(TY))
                {
                    sql += $" AND EXISTS(SELECT A.TGUID FROM FQA_CK_SN A WHERE A.TGUID= B.TGUID AND A.TLL={TY})"; 
                }
                if (!string.IsNullOrWhiteSpace(Ems))
                {
                    sql += $" AND EXISTS(SELECT A1.WORKJOB_CODE FROM WORK_WORKJOB A1 WHERE A1.WORKJOB_CODE= B.MO AND (A1.EMS_CODE='{Ems}' OR CLIENTCODE='{Ems}'))";
                }
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        FQA_CK_SNLOGV selModel = new FQA_CK_SNLOGV
                        {
                            TGUID = dataTable.Rows[i]["TGUID"].ToString(),
                            MO = dataTable.Rows[i]["MO"].ToString(),
                            SN = dataTable.Rows[i]["SN"].ToString(),
                            IMEI = dataTable.Rows[i]["IMEI"].ToString(),
                            LINE = dataTable.Rows[i]["LINE"].ToString(),
                            ISOK = dataTable.Rows[i]["ISOK"].ToString(),
                            PCNAME = dataTable.Rows[i]["PCNAME"].ToString(),
                            CREATEDATE = dataTable.Rows[i]["CREATEDATE"].ToString(),
                            TLL = dataTable.Rows[i]["TLL"].ToString(),
                            T_DES = dataTable.Rows[i]["T_DES"].ToString(),
                            SUBID = dataTable.Rows[i]["SUBID"].ToString(),
                        };
                        lis.Add(selModel);
                    }

                }
                else
                {
                    lis = null;
                }
            });
            return lis;
        }
        public void AddODM_SIDKEYTEMP(ODM_SIDKEYTEMP oDM_SIDKEYTEMP)
        {
            _context.ODM_SIDKEYTEMP.Add(oDM_SIDKEYTEMP);
        }

        public async Task<IEnumerable<ODM_SIDKEYTEMP>> ListODM_SIDKEYTEMP()
        {

            var dtos1 = from q in _context.ODM_SIDKEYTEMP
                        from p in _context.ODM_SIDKEY
                        where q.SID == p.SID
                        select new ODM_SIDKEYTEMP
                        {
                            SN = q.SN,
                            SID = q.SID
                        };
            return await dtos1.ToListAsync();
        }

        public void AddODM_SIDKEYLOG(ODM_SIDKEYLOG oDM_SIDKEYLOG)
        {
            _context.ODM_SIDKEYLOG.Add(oDM_SIDKEYLOG);
        }
        public async Task<IEnumerable<ODM_SIDKEYTEMP>> GetODM_SIDKEYTEMP()
        {
            return await _context.ODM_SIDKEYTEMP.ToListAsync();
        }

        public bool SaveODM_SIDKEYTEMP()
        {
            string sql = @"INSERT INTO ODM_SIDKEY SELECT * FROM ODM_SIDKEYTEMP";
            //_context.Database.ExecuteSqlCommand(sql);
            //_context.SaveChanges();
            int index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
            return true;
        }

        public async Task<ODM_SIDKEY> GetODM_SIDKEYLOG(string Sid)
        {
            var ls = _context.ODM_SIDKEY.Where(x => x.SID == Sid);
            return await ls.FirstOrDefaultAsync();
        }

        public int GetODM_SIDKEYCT()
        {
            var flag3 = _context.ODM_SIDKEY.Where(m => m.ISUSER == 0).Count();

            return flag3;
            //throw new NotImplementedException();
        }

        public async Task<ODM_IMEILINKNETCODE> GetSNBY_IMEILINKNETCODE(string SN)
        {
            ODM_IMEILINKNETCODE oDM_IMEILINKNETCODE = new ODM_IMEILINKNETCODE();
            await Task.Run(() =>
            {
                List<ODM_IMEILINKNETCODE> LioDM_IMEILINKNETCODEs = new List<ODM_IMEILINKNETCODE>();
                string sql = $"SELECT PHYSICSNO,WORKORDER,SN,IMEI2,MEID FROM ODM_IMEILINKNETCODE X WHERE x.SN = '{SN}' OR x.PHYSICSNO = '{SN}' OR x.IMEI2 = '{SN}' OR x.MEID = '{SN}'";
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    oDM_IMEILINKNETCODE.WORKORDER = dataTable.Rows[0]["WORKORDER"].ToString();
                    oDM_IMEILINKNETCODE.PHYSICSNO = dataTable.Rows[0]["PHYSICSNO"].ToString();
                    oDM_IMEILINKNETCODE.IMEI2 = dataTable.Rows[0]["IMEI2"].ToString();
                    oDM_IMEILINKNETCODE.MEID = dataTable.Rows[0]["MEID"].ToString();
                    oDM_IMEILINKNETCODE.SN = dataTable.Rows[0]["SN"].ToString();
                }
                else
                {
                    oDM_IMEILINKNETCODE= null;
                } 
            });
            return oDM_IMEILINKNETCODE;

            //var ls = _context.oDM_IMEILINKNETCODEs.AsNoTracking().Where(x => x.SN == SN || x.PHYSICSNO == SN || x.IMEI2 == SN || x.MEID == SN);
            //var ls = _context.oDM_IMEILINKNETCODEs.AsNoTracking().Where(x => x.PHYSICSNO == SN);
            //var a = await ls.FirstOrDefaultAsync();
        }

        public void AddODM_LOCKREWORK(ODM_LOCKREWORK oDM_LOCKREWORK)
        {
            _context.ODM_LOCKREWORK.Add(oDM_LOCKREWORK);
        }

        public async Task<MESTOOL> GetMESTOOL(string TOOL)
        {
            return await _context.MESTOOL.Where(x => x.TOOL == TOOL).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ODM_LOCKREWORK>> GetODM_LOCKREWORK(string SN, string MO, string ISCK, string DATE1, string DATE2)
        {
            var items = _context.ODM_LOCKREWORK as IQueryable<ODM_LOCKREWORK>;
            if (!string.IsNullOrWhiteSpace(SN))
            {
                items = items.Where(x => x.SN == SN.Trim());
            }
            if (!string.IsNullOrWhiteSpace(MO))
            {
                items = items.Where(x => x.MO == MO.Trim());
            }
            if (!string.IsNullOrWhiteSpace(ISCK))
            {
                if (ISCK=="true")
                {
                    items = items.Where(x => x.CRT_DATE >= Convert.ToDateTime(DATE1.Trim()) && x.CRT_DATE <= Convert.ToDateTime(DATE2.Trim()));
                }
            } 
            return await items.Take(500).ToListAsync();
        }

        public async Task<ODM_LOCKREWORK> GetODM_LOCKREWORKDID(string DID)
        {
            return await _context.ODM_LOCKREWORK.Where(x => x.UDID == DID).FirstOrDefaultAsync();
        }

        public void UpdateODM_LOCKREWORK(ODM_LOCKREWORK oDM_LOCKREWORK)
        {
            //--throw new NotImplementedException();
        }

        public async Task<MTL_SUB_ATTEMPER> GetMTL_SUB_ATTEMPER(string MO, string TEop)
        {
            return await _context.MTL_SUB_ATTEMPER.Where(x => x.ATTEMPTER_CODE == MO&&x.TESTPOSITION==TEop).FirstOrDefaultAsync();
        }

        public async Task<FQA_OBARESHUOTIME> GetFQA_OBARESHUOTIME(string Itemcode)
        {
            return await _context.FQA_OBARESHUOTIME.Where(x => x.ITEMCODE == Itemcode).FirstOrDefaultAsync();
        }

        public async Task<FQA_OBAITEMTIME> GetFQA_OBAITEMTIME(string Itemcode)
        {
            return await _context.FQA_OBAITEMTIME.Where(x => x.ITEMCODE == Itemcode).FirstOrDefaultAsync();
        }

        public void AddFQA_OBARESHUOTIME(FQA_OBARESHUOTIME fQA_OBARESHUOTIME)
        {
            _context.FQA_OBARESHUOTIME.Add(fQA_OBARESHUOTIME);
        }

        public void UpdateFQA_OBARESHUOTIME(FQA_OBARESHUOTIME fQA_OBARESHUOTIME)
        {
            //throw new NotImplementedException();
        }

        public async Task<IEnumerable<FQA_OBARESHUOTIME>> ListFQA_OBARESHUOTIME(string BOM, string DATE1, string DATE2)
        {
            var items = _context.FQA_OBARESHUOTIME as IQueryable<FQA_OBARESHUOTIME>;
            if (!string.IsNullOrWhiteSpace(BOM))
            {
                items = items.Where(x => x.ITEMCODE == BOM);
            }
            if (!string.IsNullOrWhiteSpace(DATE1))
            {
                items = items.Where(x => x.CRT_DATE >= Convert.ToDateTime(DATE1.Trim()) && x.CRT_DATE <= Convert.ToDateTime(DATE2.Trim()));
            }
            return await items.Take(200).ToListAsync();
        }

        public void DeleteFQA_OBARESHUOTIME(FQA_OBARESHUOTIME fQA_OBARESHUOTIME)
        {
            _context.FQA_OBARESHUOTIME.Remove(fQA_OBARESHUOTIME);
        }

        public async Task<IEnumerable<ODM_BEATE>> GetBEATELINE(int nb)
        {
            return await _context.ODM_BEATE.ToListAsync();
        }
        public async Task<IEnumerable<ODM_BEATEDtos>> GetBEATELINEMODEL()
        {
            List<ODM_BEATEDtos> LiBEATEDtos = new List<ODM_BEATEDtos>();
            
            await Task.Run(() =>
            {
               
                string sql = $"SELECT DISTINCT(CODE) ID,T.CODE NAME FROM ODM_STATION T";
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        ODM_BEATEDtos BEATEDtos = new ODM_BEATEDtos();
                        BEATEDtos.ID = dataTable.Rows[i]["ID"].ToString();
                        BEATEDtos.NAME = dataTable.Rows[i]["NAME"].ToString();
                        LiBEATEDtos.Add(BEATEDtos);

                    }
                 
                }
                else
                {
                    LiBEATEDtos = null;
                }
            });
            return LiBEATEDtos; 

        }

        public async Task<ErrMessage> ErrMessage(string SN1, string SN2, string SN3, int TY)
        {
            ErrMessage errMessage = new ErrMessage();
            bool bl = true;
            return await Task.Run(async () =>
            {
                switch (TY)
                {
                    case 1:
                        if (!string.IsNullOrEmpty(SN1))
                        {
                            SN1 = SN1.Trim();
                        }
                        else
                        {
                            errMessage = new ErrMessage { Err = "传入条码为空", success = true };
                            goto EndLG;
                        }
                        string Sn = AppHelper.GetSnByImei(SN1);
                        if (!string.IsNullOrWhiteSpace(Sn))
                        {
                            SN1 = Sn;
                        } 
                        string sql = $"SELECT MO,LINE FROM FQA_CK_SNLOG A WHERE A.SN='{SN1}' AND INSTR(T_DES,'中件推送') > 0 AND ISOK=0";
                        DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                        if (dataTable == null)
                        {
                            errMessage = (new ErrMessage { Err = "数据库连接失效", success = true });
                            goto EndLG;
                        }
                        if (dataTable.Rows.Count > 0)
                        {
                            string MO = dataTable.Rows[0]["MO"].ToString();
                            string STATION = "包装投料";
                            if (dataTable.Rows.Count == 1)
                            {
                                if (!AppHelper.ISDBG(MO))
                                {
                                    string subid = AppHelper.GetSubidByMo(MO, STATION);
                                    if (!string.IsNullOrWhiteSpace(subid))
                                    {
                                        var did = Guid.NewGuid().ToString();
                                        ODM_LOCKREWORK oDM_LOCKREWORK = new ODM_LOCKREWORK
                                        {
                                            SUBID = Convert.ToInt32(subid),
                                            UDID = did,
                                            SN = SN1,
                                            CRT_DATE = DateTime.Now,
                                            MO = MO,
                                            LOCKIN = 0,
                                            STATION = STATION,
                                            CRT_USER = SN2
                                        };
                                        AddODM_LOCKREWORK(oDM_LOCKREWORK);
                                        bl = false;
                                    }
                                }
                            }
                            errMessage = (new ErrMessage { Err = "中件推送,请录入报表", success = false });
                            sql = $"UPDATE FQA_CK_SNLOG A SET ISOK=1 WHERE A.SN='{SN1}'  AND ISOK=0";
                            int index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
                            sql = $"UPDATE FQA_CK_SN  A SET ISOK=1 WHERE A.SN='{SN1}'  AND ISOK=0";
                            index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
                            goto EndLG;
                        }
                        sql = $"SELECT * FROM FQA_CK_SNLOG A WHERE A.SN='{SN1}' AND INSTR(T_DES,'工单埋尾') > 0 AND ISOK=0";
                        dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                        if (dataTable == null)
                        {
                            errMessage = (new ErrMessage { Err = "数据库连接失效", success = true });
                            goto EndLG;
                        }
                        if (dataTable.Rows.Count > 0)
                        {
                            string MO = dataTable.Rows[0]["MO"].ToString();
                            string STATION = "包装投料";
                            if (dataTable.Rows.Count == 1)
                            {
                                if (!AppHelper.ISDBG(MO))
                                {
                                    string subid = AppHelper.GetSubidByMo(MO, STATION);

                                    if (!string.IsNullOrWhiteSpace(subid))
                                    {
                                        var did = Guid.NewGuid().ToString();

                                        ODM_LOCKREWORK oDM_LOCKREWORK = new ODM_LOCKREWORK
                                        {
                                            SUBID = Convert.ToInt32(subid),
                                            UDID = did,
                                            SN = SN1,
                                            CRT_DATE = DateTime.Now,
                                            MO = MO,
                                            LOCKIN = 0,
                                            STATION = STATION,
                                            CRT_USER = SN2
                                        };
                                        AddODM_LOCKREWORK(oDM_LOCKREWORK);
                                        bl = false;
                                    }
                                }
                            }
                            errMessage = (new ErrMessage { Err = "末件推送,请录入报表", success = false });
                            sql = $"UPDATE FQA_CK_SNLOG A SET ISOK=1 WHERE A.SN='{SN1}'  AND ISOK=0";
                            int index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
                            sql = $"UPDATE FQA_CK_SN  A SET ISOK=1 WHERE A.SN='{SN1}'  AND ISOK=0";
                            index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
                            goto EndLG;
                        }
                        sql = $"SELECT * FROM FQA_CK_SNLOG A WHERE A.SN='{SN1}' AND INSTR(T_DES,'开线收线时间段首台机器') > 0 AND ISOK=0";
                        dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                        if (dataTable == null)
                        {
                            errMessage = (new ErrMessage { Err = "数据库连接失效", success = true });
                            goto EndLG;
                        }
                        if (dataTable.Rows.Count > 0)
                        {
                            errMessage = (new ErrMessage { Err = "首件推送,请录入报表", success = false });
                            sql = $"UPDATE FQA_CK_SNLOG A SET ISOK=1 WHERE A.SN='{SN1}'  AND ISOK=0"; 
                            int index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
                            sql = $"UPDATE FQA_CK_SN  A SET ISOK=1 WHERE A.SN='{SN1}'  AND ISOK=0";
                            index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
                            goto EndLG;
                        } 
                        sql = $"SELECT MO,LINE FROM FQA_CK_SNLOG A WHERE A.SN='{SN1}' AND ISOK=0";
                        dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                        if (dataTable == null)
                        {
                            errMessage = (new ErrMessage { Err = "数据库连接失效", success = true });
                            goto EndLG;
                        }
                        if (dataTable.Rows.Count > 0)
                        {
                            sql = $"UPDATE FQA_CK_SNLOG A SET ISOK=1 WHERE A.SN='{SN1}'  AND ISOK=0";
                            int index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
                            sql = $"UPDATE FQA_CK_SN  A SET ISOK=1 WHERE A.SN='{SN1}'  AND ISOK=0";
                            index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
                        }
                        errMessage = (new ErrMessage { Err = "OK", success = true });
                        goto EndLG;
                    default:
                        break;
                };
                 EndLG:;
                if (!bl)
                {
                    _ = await SaveAsync();
                }
                return errMessage;
            });
          
            
        }

        public async Task<IEnumerable<RE_MODEL>> GetRE_MODEL()
        {
            return await _context.RE_MODEL.OrderBy(x=>x.CODE).ToListAsync();
        }

        public async Task<IEnumerable<RE_BUG_TYPE>> GetRE_BUG_TYPE(string CODE, string MODEL, string TYPE)
        {
            var items = _context.RE_BUG_TYPE as IQueryable<RE_BUG_TYPE>;
            if (!string.IsNullOrWhiteSpace(CODE))
            {
                items = items.Where(x => x.CODE2 == CODE);
            }
            if (!string.IsNullOrWhiteSpace(MODEL))
            {
                items = items.Where(x => x.REASONTYPE == MODEL);
            }
            if (!string.IsNullOrWhiteSpace(TYPE))
            {
                items = items.Where(x => x.DESCRIBE.Contains(TYPE));
            }
            return await items.OrderBy(x => x.CODE).ToListAsync(); 
        }

        public async Task<ErrMessage> GetRE_CODE(string cODE, string mODEL, string tYPE)
        {
            ErrMessage errMessage = new ErrMessage();
            await Task.Run(() =>
            {
                errMessage.success = false;
                string sql = $@"WITH TS AS (SELECT  MAX(T.CODE)CODE  FROM RE_BUG_TYPE T WHERE T.REASONTYPE='{mODEL}' AND T.CODE2='{cODE}')
                          SELECT '{mODEL}' || '-' || LPAD(TO_CHAR(TO_NUMBER(NVL(SUBSTR（CODE, INSTR(CODE, '-') + 1), 0)) + 1),4,0)CODE FROM TS A";
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable==null)
                {
                    errMessage.success = false;
                    errMessage.Err = "数据库链接失败";
                    goto ENDLOG;
                }
                if (dataTable.Rows.Count > 0)
                {
                    errMessage.success = true;
                    errMessage.Err = dataTable.Rows[0]["CODE"].ToString();
                }
                ENDLOG:;
            }); 
           return errMessage; 
        } 
        public async Task<ErrMessage> ADD_RE_BUG_TYPE(string v_CLIENNAME, string v_CLIENCODE, string v_REASONTYPE, string v_CODE, string v_NAME, string v_DESCRIBE, string v_BUG_TYPE, string v_WORKCODE)
        {
            ErrMessage errMessage = new ErrMessage();
            List<RE_BUG_TYPE> Listre = await _context.RE_BUG_TYPE.Where(x => x.CODE == v_CODE && x.CODE2 == v_CLIENCODE).ToListAsync();
            if (Listre.Count > 0)
            {
                RE_BUG_TYPE rE_BUG_TYPE = new RE_BUG_TYPE();
                string ID = Listre[0].ID;
                rE_BUG_TYPE = await _context.RE_BUG_TYPE.Where(x => x.CODE == v_CODE && x.CODE2 == v_CLIENCODE).FirstOrDefaultAsync();
                rE_BUG_TYPE.CODE = v_CODE;
                rE_BUG_TYPE.NAME = v_NAME;
                rE_BUG_TYPE.DESCRIBE = v_DESCRIBE;
                rE_BUG_TYPE.BUG_TYPE = v_BUG_TYPE;
                rE_BUG_TYPE.CLASS = "N";
                rE_BUG_TYPE.REASONTYPE = v_REASONTYPE;
                rE_BUG_TYPE.CODE2 = v_CLIENCODE;
                rE_BUG_TYPE.NAME2 = v_CLIENNAME;
                rE_BUG_TYPE.WORKCODE = v_WORKCODE;
                _context.RE_BUG_TYPE.Update(rE_BUG_TYPE);
                await SaveAsync();
                errMessage.Err = "更新完成";
                errMessage.success = true;

            }
            else
            {
                RE_BUG_TYPE rE_BUG_TYPE = new RE_BUG_TYPE();
                string ISHW = "0";
                if (v_CLIENCODE == "01")
                {
                    ISHW = "1";
                }
                rE_BUG_TYPE.ISHW = ISHW;
                rE_BUG_TYPE.CODE = v_CODE;
                rE_BUG_TYPE.NAME = v_NAME;
                rE_BUG_TYPE.DESCRIBE = v_DESCRIBE;
                rE_BUG_TYPE.BUG_TYPE = v_BUG_TYPE;
                rE_BUG_TYPE.CLASS = "N";
                rE_BUG_TYPE.REASONTYPE = v_REASONTYPE;
                rE_BUG_TYPE.CODE2 = v_CLIENCODE;
                rE_BUG_TYPE.NAME2 = v_CLIENNAME;
                rE_BUG_TYPE.WORKCODE = v_WORKCODE;
                _context.RE_BUG_TYPE.Add(rE_BUG_TYPE);
                await SaveAsync();
                errMessage.Err = "保存完成";
                errMessage.success = true;
            }
            return errMessage;
        }

        public async Task<ErrMessage> CHA_RE_BUG_TYPE(string v_ID, string v_NAME, string v_DESCRIBE, string v_BUG_TYPE)
        {
            ErrMessage errMessage = new ErrMessage();
            RE_BUG_TYPE rE_BUG_TYPE = await _context.RE_BUG_TYPE.Where(x => x.ID == v_ID).FirstOrDefaultAsync();
            if (rE_BUG_TYPE!=null)
            {
                rE_BUG_TYPE.NAME = v_NAME;
                rE_BUG_TYPE.DESCRIBE = v_DESCRIBE;
                rE_BUG_TYPE.BUG_TYPE = v_BUG_TYPE;
                _context.RE_BUG_TYPE.Update(rE_BUG_TYPE);
                await SaveAsync();
                errMessage.Err = "当前不良代码修改完成";
                errMessage.success = true;
            }
            else
            {
                errMessage.Err = "当前不良代码不存在";
                errMessage.success = false;
            }
            return errMessage;
        }

        public async Task<ErrMessage> DEL_RE_BUG_TYPE(string v_ID)
        {
            ErrMessage errMessage = new ErrMessage();
            RE_BUG_TYPE rE_BUG_TYPE = await _context.RE_BUG_TYPE.Where(x => x.ID == v_ID).FirstOrDefaultAsync();
            if (rE_BUG_TYPE != null)
            { 
                _context.RE_BUG_TYPE.Remove(rE_BUG_TYPE);
                await SaveAsync();
                errMessage.Err = "当前不良代码删除完成";
                errMessage.success = true;
            }
            else
            {
                errMessage.Err = "当前不良代码不存在";
                errMessage.success = false;
            }
            return errMessage;
        }

        public async Task<ErrMessage> COPY_RE_BUG_TYPE(string v_CLIENT, string v_CLIENT1, string v_REASONTYPE)
        {
            ErrMessage errMessage = new ErrMessage();
            QA_CLINT qA_CLINT =await _context.QA_CLINT.Where(x => x.CODE == v_CLIENT).FirstOrDefaultAsync();
            string Name1 = qA_CLINT.NAME;
            string sql = $"INSERT INTO RE_BUG_TYPE( CODE,NAME,BUG_TYPE,DESCRIBE,CLASS,TESTPOSITION,WORKCODE,REASONTYPE,CODE2,NAME2,ID) SELECT CODE,NAME,BUG_TYPE,DESCRIBE,CLASS,TESTPOSITION,WORKCODE,REASONTYPE,'{v_CLIENT}'CODE2,'{Name1}'NAME2,SYS_GUID()ID FROM RE_BUG_TYPE WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(v_CLIENT1))
            {
                sql += $" AND CODE2='{v_CLIENT1}'";
            }
            if (!string.IsNullOrWhiteSpace(v_REASONTYPE))
            {
                sql += $" AND REASONTYPE='{v_REASONTYPE}'";
            }
            await Task.Run(() =>
            {
                int dc = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
            });
            errMessage.success = true;
            errMessage.Err = "复制完成";
            return errMessage;
        }

        public async Task<IEnumerable<SelModel>> FQA_ERRTY(string code)
        {
            List<SelModel> lis = new List<SelModel>();
            await Task.Run(() =>
            {
                string sql = "SELECT DISTINCT(A.ERRTYPE)ERRTYPE FROM FQA_OBA_ERRCODE A WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(code))
                {
                    sql += $" AND CODE='{code}'";
                }
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        SelModel selModel = new SelModel
                        {
                            CODE = dataTable.Rows[i]["ERRTYPE"].ToString(),
                            NAME = dataTable.Rows[i]["ERRTYPE"].ToString()
                        };
                        lis.Add(selModel);
                    }

                }
                else
                {
                    lis = null;
                }
            });
            return lis;
        }

        public async Task<IEnumerable<SelModel>> FQA_ERRTYCODE(string Tcode, string code)
        {
            List<SelModel> lis = new List<SelModel>();
            await Task.Run(() =>
            {
                string sql = "SELECT ERRDESC FROM FQA_OBA_ERRCODE A WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(Tcode))
                {
                    sql += $" AND ERRTYPE='{Tcode}'";
                }
                if (!string.IsNullOrWhiteSpace(code))
                {
                    sql += $" AND CODE='{code}'";
                }
                sql += " ORDER BY ERRDESC";
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        SelModel selModel = new SelModel
                        {
                            CODE = dataTable.Rows[i]["ERRDESC"].ToString(),
                            NAME = dataTable.Rows[i]["ERRDESC"].ToString()
                        };
                        lis.Add(selModel);
                    }

                }
                else
                {
                    lis = null;
                }
            });
            return lis;
        }

        public ErrMessage FQA_OBA_PASS(string v_ID)
        {
            ErrMessage errMessage = new ErrMessage();
            if (string.IsNullOrWhiteSpace(v_ID))
            {
                errMessage.Err = "Pass";
                errMessage.success = true;
                goto EED;
            }
            #region MyRegion
            //string sql = $@"SELECT A.ERRDESC,T.IMEI,A.ERRTYPE FROM FQA_OBA_CHECK_INFO T LEFT OUTER JOIN FQA_OBA_ERRCODE A ON INSTR(T.CRIBE, A.ERRDESC) > 0
            //            WHERE T.ID = '{v_ID}' AND T.RESULT = 'NG' AND A.ERROK = 0";
            //DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            //if (dataTable==null)
            //{
            //    errMessage.Err = "数据库连接失败";
            //    errMessage.success = false;
            //    goto EED;
            //}
            //if (dataTable.Rows.Count>0)
            //{
            //    string ERRDESC = dataTable.Rows[0]["ERRDESC"].ToString();
            //    string IMEI = dataTable.Rows[0]["IMEI"].ToString();
            //    string ERRTYPE = dataTable.Rows[0]["ERRTYPE"].ToString();
            //    errMessage.Err = $"当前抽检批次{v_ID}包含{ERRTYPE}缺陷【{ERRDESC}】IMEI:【{IMEI}】,请拒收";
            //    errMessage.success = false;
            //    goto EED;
            //} 
            #endregion
            string sql = $@"WITH TS AS(
                    SELECT A.ERRDESC,A.ERROK,COUNT(*)CT FROM FQA_OBA_CHECK_INFO T LEFT OUTER JOIN FQA_OBA_ERRCODE A ON INSTR(T.CRIBE, A.ERRDESC) > 0 WHERE T.ID = '{v_ID}' AND T.RESULT = 'NG' GROUP BY ERRDESC,ERROK
                    )SELECT* FROM TS WHERE CT >= ERROK";
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable == null)
            {
                errMessage.Err = "数据库连接失败";
                errMessage.success = false;
                goto EED;
            }
            if (dataTable.Rows.Count > 0)
            {
                string ERRDESC = dataTable.Rows[0]["ERRDESC"].ToString();
                string CT = dataTable.Rows[0]["CT"].ToString();
                errMessage.Err = $"当前抽检批次【{v_ID}】包含{CT}个【{ERRDESC}】缺陷,请拒收";
                errMessage.success = false;
                goto EED;
            }
            errMessage.Err = "Pass";
            errMessage.success = true;
        EED:;
            return errMessage;
        }

        public async Task<IEnumerable<Model.FQA_OBA_SAMPLE>> GetFQA_OBA_SAMPLE(string iD)
        {
            List<Model.FQA_OBA_SAMPLE> lis = new List<Model.FQA_OBA_SAMPLE>();
            await Task.Run(() =>
            {
                string sql = $"SELECT ID,MIDCARTONID,IMEI,SN,IMEI2,MEID,INPUT_DATE,FQA_OBA_ID FROM FQA_OBA_SAMPLE A WHERE A.ID='{iD}'"; 
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        Model.FQA_OBA_SAMPLE selModel = new Model.FQA_OBA_SAMPLE
                        {
                            ID = dataTable.Rows[i]["ID"].ToString(),
                            MIDCARTONID = dataTable.Rows[i]["MIDCARTONID"].ToString(),
                            IMEI = dataTable.Rows[i]["IMEI"].ToString(),
                            SN = dataTable.Rows[i]["SN"].ToString(),
                            IMEI2 = dataTable.Rows[i]["IMEI2"].ToString(),
                            MEID = dataTable.Rows[i]["MEID"].ToString(),
                            INPUT_DATE = dataTable.Rows[i]["INPUT_DATE"].ToString(),
                            FQA_OBA_ID = dataTable.Rows[i]["FQA_OBA_ID"].ToString(),
                        };
                        lis.Add(selModel);
                    }

                }
                else
                {
                    lis = null;
                }
            });
            return lis;
        }

        public bool UpdateFQA_CK_SN(string SN, string USER)
        {
            List<string> LsSql = new List<string>();
            string sql = $"UPDATE FQA_CK_SN  SET ISOK=1 WHERE SN='{SN}'";
            LsSql.Add(sql);
            //int n1=OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql); 
            sql = $"UPDATE FQA_CK_SNLOG  SET ISOK=1 WHERE SN='{SN}'";
            LsSql.Add(sql);
            //n1 = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql); 
            sql = $"INSERT INTO FQA_CK_SNINFOR( SELECT SUBID,MO,SN,LINE,ISOK,'{SN}',SYSDATE,TLL,'取消抽检',TGUID FROM(SELECT * FROM FQA_CK_SNLOG A WHERE A.SN='{SN}' ORDER BY A.CREATEDATE DESC) WHERE ROWNUM=1)";
            LsSql.Add(sql);
            bool n1 = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, LsSql);
            return true;
        }

        public async Task<IEnumerable<EDI_850appendix>> G_850appendix(string po)
        {
            List<EDI_850appendix> lis = new List<EDI_850appendix>();
            string sql = $"SELECT PONUMBER,SHIPEARLYSTATUS,STOP_RELEASE,STOP_SHIP,SHIP_TO_NAME1,MATERIALDES,SHIP_TO_ADDRESS1 FROM EDI_850APPENDIX T WHERE PONUMBER LIKE'{po}%'";
            await Task.Run(() =>
            {
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        EDI_850appendix eDI_850Appendix = new EDI_850appendix
                        {
                            PONUMBER = dataTable.Rows[i]["PONUMBER"].ToString(),
                            SHIPEARLYSTATUS = dataTable.Rows[i]["SHIPEARLYSTATUS"].ToString(),
                            STOP_RELEASE = dataTable.Rows[i]["STOP_RELEASE"].ToString(),
                            STOP_SHIP = dataTable.Rows[i]["STOP_SHIP"].ToString(),
                            SHIP_TO_NAME1 = dataTable.Rows[i]["SHIP_TO_NAME1"].ToString(),
                            MATERIALDES = dataTable.Rows[i]["MATERIALDES"].ToString(),
                            SHIP_TO_ADDRESS1 = dataTable.Rows[i]["SHIP_TO_ADDRESS1"].ToString()
                        };
                        lis.Add(eDI_850Appendix);
                    }

                }
            });
            return lis;
        }

        public void Up850appendix(string pONUMBER, string sHIPEARLYSTATUS, string sTOP_RELEASE, string sTOP_SHIP)
        {
            string sql = $"UPDATE EDI_850APPENDIX SET SHIPEARLYSTATUS='{sHIPEARLYSTATUS}',STOP_RELEASE='{sTOP_RELEASE}',STOP_SHIP='{sTOP_SHIP}' WHERE PONUMBER='{pONUMBER}'";
            int index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
        }

        public async Task<ODM_PRESSUREMODEL> G_ODM_PRESSUREMODEL(string cLIENCODE)
        {
            ODM_PRESSUREMODEL oDM_PRESSUREMODEL = new ODM_PRESSUREMODEL();
            string sql = $"SELECT CLIENCODE,TOPCODE,NUMCODE FROM ODM_PRESSUREMODEL T WHERE CLIENCODE ='{cLIENCODE}'";
            await Task.Run(() =>
            {
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    oDM_PRESSUREMODEL.CLIENCODE = dataTable.Rows[0]["CLIENCODE"].ToString();
                    oDM_PRESSUREMODEL.NUMCODE = dataTable.Rows[0]["NUMCODE"].ToString();
                    oDM_PRESSUREMODEL.TOPCODE = dataTable.Rows[0]["TOPCODE"].ToString();
                }
            });
            return oDM_PRESSUREMODEL;
        }

        public void AddODM_TESTFAILNUM(ODM_TESTFAILNUM oDM_TESTFAILNUM)
        {
            _context.oDM_TESTFAILNUMs.Add(oDM_TESTFAILNUM);
        }

        public void UpdateODM_TESTFAILNUM(ODM_TESTFAILNUM oDM_TESTFAILNUM)
        {
            //throw new NotImplementedException();
        }

        public async Task<ODM_TESTFAILNUM> GetODM_TESTFAILNUM(string STATION)
        {
            return await _context.oDM_TESTFAILNUMs.Where(x => x.STATION == STATION).FirstOrDefaultAsync(); 
        }

        public async Task<IEnumerable<ODM_TESTFAILNUM>> GetLIST_TESTFAILNUM()
        {
            //List<SelModel> lis = new List<SelModel>();
            List<ODM_TESTFAILNUM> lss =await _context.oDM_TESTFAILNUMs.ToListAsync(); 
            return lss;
        }

        public async Task<ErrMessage> DelODM_TESTFAILNUM(string sTATION)
        {
            ErrMessage errMessage = new ErrMessage();
            ODM_TESTFAILNUM fQA_LINETIMEOW =await _context.oDM_TESTFAILNUMs.Where(x => x.STATION == sTATION).FirstOrDefaultAsync();
            if (fQA_LINETIMEOW != null)
            {
                _context.oDM_TESTFAILNUMs.Remove(fQA_LINETIMEOW);
                errMessage.success = true;
                errMessage.Err = "删除完成";
                _context.SaveChanges();
            }
            else
            {
                errMessage.success=false;
                errMessage.Err = "删除失败";
            }
            return errMessage;
        }

        public async Task<IEnumerable<ORT_BARCODE_NEW>> GetOrt_barcode(string v_MO, string v_SN, string v_EMS_M2, string v_DATE1, string v_DATE2, string v_EMS_M1)
        {
            List<ORT_BARCODE_NEW> lis = new List<ORT_BARCODE_NEW>();
            if (!string.IsNullOrWhiteSpace(v_MO))
            {
                v_MO = v_MO.Trim();
            }
            if (!string.IsNullOrWhiteSpace(v_SN))
            {
                v_SN = v_SN.Trim();
            }
            if (!string.IsNullOrWhiteSpace(v_EMS_M2))
            {
                v_EMS_M2 = v_EMS_M2.Trim();
            }
            if (!string.IsNullOrWhiteSpace(v_DATE1))
            {
                v_DATE1 = v_DATE1.Trim();
            }
            if (!string.IsNullOrWhiteSpace(v_DATE2))
            {
                v_DATE2 = v_DATE2.Trim();
            }
            if (!string.IsNullOrWhiteSpace(v_EMS_M1))
            {
                v_EMS_M1 = v_EMS_M1.Trim();
            }
            string sql = "SELECT ID,MO,BARCODE,LINKSN,PERSON,CRT_DATE,ISOK,MODEL,KEYPART,RESYS_DATE,REPERSON FROM ORT_BARCODE_NEW T WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(v_MO))
            {
                sql += $" AND MO='{v_MO}'";
            } 
            if (!string.IsNullOrWhiteSpace(v_SN))
            {
                string SN2 = AppHelper.GetSnByImei(v_SN);
                if (!string.IsNullOrWhiteSpace(SN2))
                {
                    sql += $" AND BARCODE='{SN2}'";
                }
                else
                {
                    sql += $" AND BARCODE='{v_SN}'";
                } 
            }
            if (!string.IsNullOrWhiteSpace(v_EMS_M2))
            {
                sql += $" AND CRT_DATE BETWEEN TO_DATE('{v_DATE1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{v_DATE2}','YYYY-MM-DD HH24:MI:SS')";
            }
            if (!string.IsNullOrWhiteSpace(v_EMS_M1))
            {
                sql += $" AND ISOK='{v_EMS_M1}'";
            }
            await Task.Run(() =>
            { 
                DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        ORT_BARCODE_NEW  bARCODE_NEW = new ORT_BARCODE_NEW
                        {
                            ID=dataTable.Rows[i]["ID"].ToString(),
                            MO = dataTable.Rows[i]["MO"].ToString(),
                            BARCODE = dataTable.Rows[i]["BARCODE"].ToString(),
                            LINKSN = dataTable.Rows[i]["LINKSN"].ToString(),
                            PERSON = dataTable.Rows[i]["PERSON"].ToString(),
                            CRT_DATE = dataTable.Rows[i]["CRT_DATE"].ToString(),
                            ISOK = dataTable.Rows[i]["ISOK"].ToString(),
                            MODEL = dataTable.Rows[i]["MODEL"].ToString(),
                            KEYPART = dataTable.Rows[i]["KEYPART"].ToString(),
                            RESYS_DATE = dataTable.Rows[i]["RESYS_DATE"].ToString(),
                            REPERSON = dataTable.Rows[i]["REPERSON"].ToString(),
                        };
                        lis.Add(bARCODE_NEW);
                    } 
                }
                else
                {
                    lis = null;
                }
            });
            return lis;
        }

        public async Task<ErrMessage> UpDateORT(string iD, string pCUSER, string oRT)
        {
            ErrMessage errMessage = new ErrMessage();
            await Task.Run(() =>
            {
                string sql = "UPDATE ORT_BARCODE_NEW T SET ISOK=0,RESYS_DATE=SYSDATE,REPERSON='{pCUSER}'  WHERE ID='{iD}'";
                if (oRT=="N")
                {
                    errMessage.Err = "ORT取消";
                    sql = $"UPDATE ORT_BARCODE_NEW T SET ISOK=1,RESYS_DATE=SYSDATE,REPERSON='{pCUSER}'  WHERE ID='{iD}'";
                    int index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
                }
                else
                {
                    errMessage.Err = "ORT校验";
                    sql = $"UPDATE ORT_BARCODE_NEW T SET ISOK=0,RESYS_DATE=NULL,REPERSON='{pCUSER}'  WHERE ID='{iD}'";
                    int index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, sql);
                }
            });
            errMessage.success = true;
            return errMessage;
        }

        public int GetODM_SIDKEYCTBYMODEL(string code)
        {
            string sql = "SELECT COUNT(*) FROM ODM_SIDKEY A  WHERE A.ISUSER=0";
            if (!string.IsNullOrWhiteSpace(code))
            {
                sql += $" AND A.PRODUCT_MODEL='{code}'";
            } 
            DataTable dataTable = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dataTable.Rows.Count>0)
            {
                return Convert.ToInt32(dataTable.Rows[0][0].ToString());
            }
            return 0;
        }

        public async Task<IEnumerable<Model.SFAA_T>> GetSFAA_T(string Mo)
        {
            List<Model.SFAA_T> lis = new List<Model.SFAA_T>();
            string sql = @"SELECT T.SFAADOCNO,T.SFAA010,A.IMAAL003 ,T.SFAA012,T.SFAA009 FROM SFAA_T T
                          LEFT JOIN IMAAL_T A  ON A.IMAAL001 = T.SFAA010 WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(Mo))
            {
                sql += $" AND SFAADOCNO LIKE'%{Mo}%'";
            }
            sql = $"WITH TS AS({sql}) SELECT * FROM TS WHERE ROWNUM<50";
            await Task.Run(() =>
            {
                DataTable dataTable = OracleHelper.ExecuteDataTable(UserInfo.OracleConnectionStringT100, sql);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        Model.SFAA_T bARCODE_NEW = new Model.SFAA_T
                        {
                            SFAADOCNO = dataTable.Rows[i]["SFAADOCNO"].ToString(),
                            SFAA010 = dataTable.Rows[i]["SFAA010"].ToString(),
                            IMAAL003 = dataTable.Rows[i]["IMAAL003"].ToString(),
                            SFAA012 = dataTable.Rows[i]["SFAA012"].ToString(),
                            SFAA009 = dataTable.Rows[i]["SFAA009"].ToString(),
                        };
                        lis.Add(bARCODE_NEW);
                    }
                }
                else
                {
                    lis = null;
                }
            });
            return lis; 
        }

        public Task<ErrMessage> mULCHING21(string bARCODE, string cLIENTCODE,string mO,string T100CT)
        {
            ErrMessage err = new ErrMessage();
            string msg = string.Empty;
            //await Task.Run(() =>
            //{
            string sql = $"SELECT COUNT(*) FROM ODM_PRESSURE WHERE MO='{mO}' AND TYPE=3";
            DataTable dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dt.Rows.Count > 0)
            {
                int CT = string.IsNullOrWhiteSpace(T100CT) ? 0 : Convert.ToInt32(T100CT);
                int Ct1 = Convert.ToInt32(dt.Rows[0][0].ToString());
                if (Ct1 >= CT)
                {
                    err.success = false;
                    err.Err = "当前点胶工单已投入完成,投入数" + CT + ",请切换工单";
                    return Task.FromResult(err);
                }
            }
            string strselect = string.Format("SELECT LINKSN  FROM ODM_PRESSURE WHERE BARCODE='{0}' AND TYPE=4", bARCODE);
            DataTable dt1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselect);
            if (dt1 == null)
            {
                msg = OracleHelper.OracleErrMsg;
                err.success = false;
                err.Err = msg;
                return Task.FromResult(err);
            }
            if (dt1.Rows.Count > 0)
            {
                msg = string.Format("扫描条码{0}已经静置扫描,不能重复保压", bARCODE, dt1.Rows[0][0].ToString());
                err.success = false;
                err.Err = msg;
                return Task.FromResult(err);
            }
            strselect = string.Format("SELECT LINKSN  FROM ODM_PRESSURE WHERE BARCODE='{0}' AND TYPE=3", bARCODE);
            dt1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselect);
            if (dt1.Rows.Count > 0)
            {
                msg = string.Format("扫描条码{0}已经绑定保压盒条码{1},不能重复绑定", bARCODE, dt1.Rows[0][0].ToString());
                err.success = false;
                err.Err = msg;
                return Task.FromResult(err);
            }
            strselect = string.Format("SELECT TO_CHAR(CREAT_TIME,'YYYY-MMDD HH24:MI:SS')TI  FROM ODM_PRESSURE WHERE BARCODE='{0}' AND TYPE=2", bARCODE);
            dt1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselect);
            if (dt1.Rows.Count > 0)
            {
                err.success = true;
                err.Err = bARCODE + " PASS";
                return Task.FromResult(err);
            }
            string sql4 = string.Format("SELECT T.TOPCODE,T.NUMCODE FROM ODM_PRESSUREMODEL T WHERE T.CLIENCODE='{0}'", cLIENTCODE);
            DataTable dataTable3 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql4);
            if (dataTable3.Rows.Count > 0)
            {
                string str1 = dataTable3.Rows[0][0].ToString();
                string str2 = dataTable3.Rows[0][1].ToString();
                int length1 = bARCODE.Length;
                if (str2.Contains(","))
                {
                    bool flag = false;
                    string str3 = str2;
                    char[] chArray = new char[1] { ',' };
                    foreach (string str4 in str3.Split(chArray))
                    {
                        if (Convert.ToInt32(str4) == length1)
                            flag = true;
                    }
                    if (!flag)
                    {
                        msg = string.Format("扫描条码{0}长度{1}要与当前配置长度{2}相等", bARCODE, (object)length1, (object)str2);
                        err.success = false;
                        err.Err = msg;
                        return Task.FromResult(err);
                    }
                }
                else
                {
                    int int32 = Convert.ToInt32(str2);
                    if (length1 != int32)
                    {
                        msg = string.Format("扫描条码{0}长度{1}要与当前配置长度{2}相等", bARCODE, (object)length1, (object)int32);
                        err.success = false;
                        err.Err = msg;
                        return Task.FromResult(err);
                    }
                }
                if (str1.Contains(","))
                {
                    bool flag = false;
                    string str3 = str1;
                    char[] chArray = new char[1] { ',' };
                    foreach (string str4 in str3.Split(chArray))
                    {
                        int length2 = str4.Length;
                        if (length2 <= length1 && bARCODE.Substring(0, length2) == str4)
                            flag = true;
                    }
                    if (flag)
                    {
                        msg = bARCODE + "PASS";
                        err.success = true;
                        err.Err = msg;
                        return Task.FromResult(err);
                    }
                    msg = string.Format("扫描条码{0}不符号当前配置前缀{1}规则", bARCODE, (object)str1);
                    err.success = false;
                    err.Err = msg;
                    return Task.FromResult(err);
                }
                else
                {
                    int length3 = str1.Length;
                    if (length3 > length1)
                    {
                        msg = string.Format("扫描条码{0}长度{1}要大于当前配置前缀长度{2}", bARCODE, (object)length1, (object)length3);
                        err.success = false;
                        err.Err = msg;
                        return Task.FromResult(err);
                    }
                    if (bARCODE.Substring(0, length3) != str1)
                    {
                        msg = string.Format("扫描条码{0}不符号当前配置前缀{1}规则", bARCODE, (object)str1);
                        err.success = false;
                        err.Err = msg;
                        return Task.FromResult(err);
                    }
                    msg = bARCODE + "PASS";
                    err.success = true;
                    err.Err = msg;
                    return Task.FromResult(err);
                }
            }
            else
            {
                msg = string.Format("当前客户保压前缀设定的LCM条码规则未设定", cLIENTCODE);
                err.success = false;
                err.Err = msg;
                return Task.FromResult(err);
            }
            //});
            err.success = true;
            err.Err = bARCODE + " PASS";
            return Task.FromResult(err);
        }

        public Task<ErrMessage> mULCHING33(string bARCODE, string cLIENTCODE)
        {
            ErrMessage err = new ErrMessage();
            string msg = string.Empty;
            //await Task.Run(() =>
            //{ });
            string sql = "SELECT LOCK_STATUS FROM ODM_BYH_LOCK_INFO WHERE SN='" + bARCODE + "' AND LOCK_STATUS=1";
                DataTable dt  = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dt.Rows.Count > 0)
                {
                    err.success = false;
                    err.Err = "操作失败:保压盒[" + bARCODE + "]已锁定,禁止使用！";
                    return Task.FromResult(err);  
                }
                //保压盒子判断
                sql = string.Format("SELECT T.BARCODE FROM ODM_PRESSURE T WHERE (LINKSN='{0}' OR TOP='{0}') AND TYPE=3 AND LINKING=1", bARCODE);
                dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
                if (dt.Rows.Count > 0)
                {
                    msg = string.Format("当前保压盒{0}当前工序静置投入扫描，请静置投入扫描！", bARCODE);
                    err.success = false;
                    err.Err = msg;
                    return Task.FromResult(err);
                }
                err.success = true;
                err.Err = bARCODE + " PASS";
                return Task.FromResult(err);
            //});
            //err.success = true;
            //err.Err = bARCODE + " PASS";
            //return err;
        }

        public Task<ErrMessage> mULCHING3(string bARCODE, string cLIENTCODE, string lCMBARCODE, string tOPBARCODE,string UserID,string mO,string lINE, string T100CT)
        {
            //LCM判断
            ErrMessage err = new ErrMessage();
            Task<ErrMessage> msg1 = mULCHING21(lCMBARCODE, cLIENTCODE, mO, T100CT);
            if (!msg1.Result.success)
            {
                err.success = false;
                err.Err = msg1.Result.Err;
                return Task.FromResult(err);
            }
            Task<ErrMessage> msg2 = mULCHING33(lCMBARCODE, cLIENTCODE);
            if (!msg1.Result.success)
            {
                err.success = false;
                err.Err = msg1.Result.Err;
                return Task.FromResult(err);
            }
            #region MyRegion
            //string sql = $"SELECT COUNT(*) FROM ODM_PRESSURE WHERE MO='{mO}' AND TYPE=3";
            //DataTable dt = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            //if (dt.Rows.Count > 0)
            //{
            //    int  CT = string.IsNullOrWhiteSpace(T100CT) ? 0 : Convert.ToInt32(T100CT);
            //    int Ct1 = Convert.ToInt32(dt.Rows[0][0].ToString());
            //    if (Ct1 >= CT)
            //    {
            //        err.success = false;
            //        err.Err = "当前点胶工单已投入完成,投入数"+ CT+",请切换工单";
            //        return Task.FromResult(err);
            //    }
            //} 
            #endregion
            string strinsert = string.Format("INSERT INTO ODM_PRESSURE(BARCODE,TYPE,CREAT_PERSON,LINKSN,LINKING,CLIENTCODE,TOP,MO,LINE)VALUES('{0}','{1}','{2}','{3}',1,'{4}','{5}','{6}','{7}')", lCMBARCODE, 3, UserID, bARCODE, cLIENTCODE, tOPBARCODE, mO,lINE);
            int index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, strinsert);
            if (index > 0)
            {
                err.success = true;
                err.Err = bARCODE + " PASS";
                return Task.FromResult(err);
            }
            else
            { 
                err.success = false;
                err.Err = "保压扫描插入失败:" + OracleHelper.OracleErrMsg;
                return Task.FromResult(err);
            } 
        }

        public Task<ErrMessage> mULCHING4(string bARCODE, string hOUR,string UserID)
        {
            ErrMessage err = new ErrMessage();
            string msg = string.Empty;
            string bacode = bARCODE;
            string strselect = string.Format("SELECT BARCODE  FROM ODM_PRESSURE WHERE (LINKSN='{0}' OR TOP='{0}') AND TYPE=3 AND LINKING=1", bARCODE);
            DataTable dt1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselect);
            if (dt1.Rows.Count > 0)
            {
                bARCODE = dt1.Rows[0][0].ToString();
            }
            strselect = string.Format("SELECT TO_CHAR(CREAT_TIME,'YYYY-MMDD HH24:MI:SS')TI,CLIENTCODE,MO,LINE  FROM ODM_PRESSURE WHERE BARCODE='{0}' AND TYPE=3", bARCODE);
            dt1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselect);
            if (dt1.Rows.Count > 0)
            {
                string Clientcode = dt1.Rows[0][1].ToString();
                string Mo = dt1.Rows[0][2].ToString();
                string Line = dt1.Rows[0][3].ToString();
                strselect = string.Format("SELECT LINKSN  FROM ODM_PRESSURE WHERE BARCODE='{0}' AND TYPE=4", bARCODE);
                dt1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselect);
                if (dt1.Rows.Count > 0)
                {
                    msg = string.Format("条码{0}已经静置扫描,不能重复扫描", bacode);
                    err.success = false;
                    err.Err = msg;
                    return Task.FromResult(err);
                }
                strselect = string.Format("SELECT TO_CHAR(CREAT_TIME,'YYYY-MM-DD HH24:MI:SS') EE,SYSDATE,SYSDATE+({1}/24) FROM ODM_PRESSURE  T WHERE  T.TYPE=3 AND (SYSDATE-T.CREAT_TIME)<({1}/24)  AND T.BARCODE='{0}' ", bARCODE, hOUR);
                dt1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, strselect);
                if (dt1.Rows.Count > 0)
                {
                    msg = string.Format("条码{0}前工序保压开始时间{1}需要{2}小时后才能进行静置扫描", bacode, dt1.Rows[0][0].ToString(), hOUR);
                    err.success = false;
                    err.Err = msg;
                    return Task.FromResult(err);
                }
                string strinsert = string.Format("INSERT INTO ODM_PRESSURE(BARCODE,TYPE,CREAT_PERSON,LINKSN,LINKING,CLIENTCODE,TOP,MO,LINE)VALUES('{0}','{1}','{2}','{3}',0,'{4}','{5}','{6}','{7}')", bARCODE,4, UserID, bacode, Clientcode, "", Mo, Line);
                int index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, strinsert);
                if (index > 0)
                {
                    strinsert = string.Format("UPDATE ODM_PRESSURE SET LINKING=0 WHERE BARCODE='{0}' AND TYPE=3 AND LINKING=1", bARCODE);
                    index = OracleHelper.ExecuteNonQuery(OracleHelper.OracleConnection, strinsert);
                    err.success = true;
                    err.Err = bARCODE + " PASS";
                    return Task.FromResult(err);
                }
                else
                {
                    err.success = false;
                    err.Err = "保压扫描插入失败:" + OracleHelper.OracleErrMsg;
                    return Task.FromResult(err);
                }
            }
            else
            {
                msg = string.Format("条码{0}前工序保压扫描未投入", bacode);
                err.success = false;
                err.Err = msg;
                return Task.FromResult(err);
            }
           throw new NotImplementedException();
        }

        public async Task<IEnumerable<ODM_PACKINGELSE>> G_ODM_PACKINGELSE(string v_MO, string dATE1, string sID, string cARTONID)
        {
            List<ODM_PACKINGELSE> PackingelseLis = new List<ODM_PACKINGELSE>();
            string sql = "SELECT T.WORKORDER,T.MIDCARTONID,T.IMEI,T.RECORDDATE,T.STATION FROM ODM_PACKINGELSE T WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(v_MO))
            {
                sql += $" AND WORKORDER='{v_MO}'";
            }
            if (!string.IsNullOrWhiteSpace(sID))
            {
                sql += $" AND IMEI='{sID}'";
            }
            if (!string.IsNullOrWhiteSpace(cARTONID))
            {
                sql += $" AND MIDCARTONID='{cARTONID}'";
            }
            if (!string.IsNullOrWhiteSpace(dATE1))
            {
                sql += $" AND TO_CHAR(RECORDDATE,'YYYY-MM-DD')='{dATE1}'";
            }
            DataTable dt1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    ODM_PACKINGELSE dM_PACKINGELSE = new ODM_PACKINGELSE
                    {
                        WORKORDER = dt1.Rows[i]["WORKORDER"].ToString(),
                        MIDCARTONID = dt1.Rows[i]["MIDCARTONID"].ToString(),
                        IMEI = dt1.Rows[i]["IMEI"].ToString(),
                        STATION = dt1.Rows[i]["STATION"].ToString(),
                        RECORDDATE = dt1.Rows[i]["RECORDDATE"].ToString()
                    };
                    PackingelseLis.Add(dM_PACKINGELSE);
                }  
            }
            return await Task.FromResult(PackingelseLis);
            //return PackingelseLis;
            //throw new NotImplementedException();
        }

        public   Task<RT_MESSAGE> GetODM_PRESSURE(string mO, string sN, string dATE1, string dATE2, string sTATION, string lINE, string cLIENTCODE)
        {
            RT_MESSAGE rT_MESSAGE = new RT_MESSAGE();
            List<DA_ODM_PRESSURE> LsdA_ODM_s = new List<DA_ODM_PRESSURE>();
            string sql = "SELECT  ROWNUM ID, A.MO,A.LINE,A.BARCODE,DECODE(A.TYPE,1,'中框扫描',2,'中框LCM关联',3,'保压扫描',4,'静置扫描',5,'外观检查')TYPE,A.CREAT_TIME,A.CREAT_PERSON,A.LINKSN SEG1,A.LCM SEG2,A.TOP SEG3, (SELECT  NAME FROM QA_CLINT WHERE CODE=A.CLIENTCODE)CLIENTCODE FROM ODM_PRESSURE A WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(mO))
            {
                sql += $" AND MO='{mO}'";
            }
            if (!string.IsNullOrWhiteSpace(lINE))
            {
                sql += $" AND LINE='{lINE}'";
            }
            if (!string.IsNullOrWhiteSpace(sTATION)&& sTATION!="0")
            {
                sql += $" AND TYPE='{sTATION}'";
            }
            if (!string.IsNullOrWhiteSpace(cLIENTCODE))
            {
                sql += $" AND CLIENTCODE='{cLIENTCODE}'";
            }
            if (!string.IsNullOrWhiteSpace(dATE1))
            {
                sql += $" AND CREAT_TIME BETWEEN TO_DATE('{dATE1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{dATE2}','YYYY-MM-DD HH24:MI:SS')";
            }
            if (!string.IsNullOrWhiteSpace(sN))
            {
                sql += $" AND (BARCODE='{sN}' OR LINKSN='{sN}' OR LCM='{sN}' OR TOP='{sN}' )";
            }
            DataTable dt1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DA_ODM_PRESSURE Da_odm_pressure = new DA_ODM_PRESSURE
                    {
                        MO = dt1.Rows[i]["MO"].ToString(),
                        LINE = dt1.Rows[i]["LINE"].ToString(),
                        TYPE = dt1.Rows[i]["TYPE"].ToString(),
                        BARCODE = dt1.Rows[i]["BARCODE"].ToString(),
                        CREAT_TIME = dt1.Rows[i]["CREAT_TIME"].ToString(),
                        CREAT_PERSON = dt1.Rows[i]["CREAT_PERSON"].ToString(),
                        SEG1 = dt1.Rows[i]["SEG1"].ToString(),
                        SEG2 = dt1.Rows[i]["SEG2"].ToString(),
                        SEG3 = dt1.Rows[i]["SEG3"].ToString(),
                        CLIENTCODE = dt1.Rows[i]["CLIENTCODE"].ToString(),
                        ID = dt1.Rows[i]["ID"].ToString(),
                    };
                    LsdA_ODM_s.Add(Da_odm_pressure);
                }
                rT_MESSAGE.success = true;
                rT_MESSAGE.Err = $"查询完成，共{dt1.Rows.Count}条记录";
                rT_MESSAGE.dA_ODM_PRESSURE = LsdA_ODM_s;
            }
            else
            {
                rT_MESSAGE.success = false;
                rT_MESSAGE.Err = "查询完成，无数据记录 "+System.DateTime.Now;

            }
            return   Task.FromResult(rT_MESSAGE); 
        }

        public Task<RT_MESSAGE> GetODM_PRESSUREFREE(string sN, string dATE1, string dATE2, string iD_USER)
        {
            RT_MESSAGE rT_MESSAGE = new RT_MESSAGE();
            List<DA_ODM_PRESSFREE> LsdA_ODM_s = new List<DA_ODM_PRESSFREE>();
            string sql = "SELECT T.BARCODE,DECODE(T.TYPE,1,'中框扫描',2,'中框LCM关联',3,'保压扫描',4,'静置扫描',5,'外观检查')TYPE,T.CREAT_TIME,T.CREAT_PERSON,T.LINKSN,T.FRR_TIME,T.CRT_USER FROM ODM_PRESSURELOG T WHERE 1=1";
            if (!string.IsNullOrEmpty(sN))
            {
                sql += $" AND (BARCODE='{sN}' OR LINKSN='{sN}')";
            }
            if (!string.IsNullOrEmpty(dATE1))
            {
                sql += $" AND FRR_TIME BETWEEN TO_DATE('{dATE1}','YYYY-MM-DD HH24:MI:SS') AND TO_DATE('{dATE2}','YYYY-MM-DD HH24:MI:SS')";
            }
            if (!string.IsNullOrEmpty(iD_USER))
            {
                sql += $" AND  CRT_USER='{iD_USER}' ";
            }
            DataTable dt1 = OracleHelper.ExecuteDataTable(OracleHelper.OracleConnection, sql);
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DA_ODM_PRESSFREE Da_odm_pressure = new DA_ODM_PRESSFREE
                    {
                        BARCODE = dt1.Rows[i]["BARCODE"].ToString(),
                        TYPE = dt1.Rows[i]["TYPE"].ToString(),
                        CREAT_TIME = dt1.Rows[i]["CREAT_TIME"].ToString(),
                        CREAT_PERSON = dt1.Rows[i]["CREAT_PERSON"].ToString(),
                        LINKSN = dt1.Rows[i]["LINKSN"].ToString(),
                        FRR_TIME = dt1.Rows[i]["FRR_TIME"].ToString(),
                        CRT_USER = dt1.Rows[i]["CRT_USER"].ToString(), 
                    };
                    LsdA_ODM_s.Add(Da_odm_pressure);
                }
                rT_MESSAGE.success = true;
                rT_MESSAGE.Err = $"查询完成，共{dt1.Rows.Count}条记录";
                rT_MESSAGE.dA_ODM_PRESSFREE = LsdA_ODM_s;
            }
            else
            {
                rT_MESSAGE.success = false;
                rT_MESSAGE.Err = "查询完成，无数据记录 " + System.DateTime.Now;

            }
            return Task.FromResult(rT_MESSAGE);
            throw new NotImplementedException();
        }
    }
}
