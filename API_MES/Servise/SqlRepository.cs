using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_MES.Data;
using API_MES.Dtos;
using EF_SQL;
using Microsoft.EntityFrameworkCore;
using API_MES.Helper;
namespace API_MES.Servise
{
    public class SqlRepository : ISqlRepository
    {
        private readonly SqlDbContext _sqlDbContext;
        //private readonly IPropertyMappingService _propertyMappingService;

        public SqlRepository(SqlDbContext sqlDbContext)
        {
            _sqlDbContext = sqlDbContext ?? throw new ArgumentNullException(nameof(sqlDbContext));
            //_propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public void AddCompany(Company company)
        {
           
            _sqlDbContext.Companies.Add(company);
        }

        public void AddDA_BOM(DA_BOM dA_BOM)
        {
            if (dA_BOM == null)
            {
                throw new ArgumentNullException(nameof(dA_BOM));
            } 
            dA_BOM.Id = Guid.NewGuid();
            dA_BOM.CRT_DATE = DateTime.Now;
            _sqlDbContext.DA_BOMs.Add(dA_BOM);
        }

        public async Task<IEnumerable<Company>> GetCompany()
        {
            return await _sqlDbContext.Companies.ToListAsync();
        }

        public async Task<DA_BOM> GetDA_BOM(string Boms)
        { 
            //return await _sqlDbContext.DA_BOMs.Where(x=>x.BOM==Boms).ToListAsync();
            return await _sqlDbContext.DA_BOMs
                .FirstOrDefaultAsync(x => x.BOM == Boms);
        }

        public async Task<bool> SaveAsync()
        {
            return await _sqlDbContext.SaveChangesAsync() >= 0;
        }

        public void UpdateDA_BOM(DA_BOM dA_BOM)
        {
            //throw new NotImplementedException();
        }

        public async Task<DA_ELSEGROSS> GtDA_ELSEGROSS(DateTime dateTime, int ty,string cl,string code)
        {
             
            if (!string.IsNullOrWhiteSpace(code))
            {
                return await _sqlDbContext.DA_ELSEGROSSs
                .FirstOrDefaultAsync(x => x.DT_DATE == dateTime && x.DT_MODEL == ty && x.DT_CLIENTNAME == cl&&x.DT_CLIENTCODE==code);
            }
            return await _sqlDbContext.DA_ELSEGROSSs
               .FirstOrDefaultAsync(x => x.DT_DATE == dateTime&&x.DT_MODEL==ty&&x.DT_CLIENTNAME==cl);
        }

        public void AddDA_ELSEGROSS(DA_ELSEGROSS dA_ELSEGROSS)
        {
            if(dA_ELSEGROSS == null)
            {
                throw new ArgumentNullException(nameof(dA_ELSEGROSS));
            }
            dA_ELSEGROSS.Id = Guid.NewGuid();
            dA_ELSEGROSS.CRT_DATE = DateTime.Now;
            _sqlDbContext.DA_ELSEGROSSs.Add(dA_ELSEGROSS);
        }

        public void UpdateDA_ELSEGROSS(DA_ELSEGROSS dA_ELSEGROSS)
        {
            //throw new NotImplementedException();
        }

        public void AddDA_GROSS(DA_GROSS dA_GROSS)
        {
            if (dA_GROSS == null)
            {
                throw new ArgumentNullException(nameof(dA_GROSS));
            }
            dA_GROSS.Id = Guid.NewGuid();
            dA_GROSS.CRT_DATE = DateTime.Now;
            _sqlDbContext.DA_GROSSs.Add(dA_GROSS);
        }

        public void AddDA_PAYPERSON(DA_PAYPERSON dA_PAYPERSON)
        {
            if (dA_PAYPERSON == null)
            {
                throw new ArgumentNullException(nameof(dA_PAYPERSON));
            }
            dA_PAYPERSON.Id = Guid.NewGuid();
            dA_PAYPERSON.CRT_DATE = DateTime.Now;
            _sqlDbContext.DA_PAYPERSONs.Add(dA_PAYPERSON);
        }

        public void AddDA_PAYLOSS(DA_PAYLOSS dA_PAYLOSS)
        {
            if (dA_PAYLOSS == null)
            {
                throw new ArgumentNullException(nameof(dA_PAYLOSS));
            }
            dA_PAYLOSS.Id = Guid.NewGuid();
            dA_PAYLOSS.CRT_DATE = DateTime.Now;
            _sqlDbContext.DA_PAYLOSSs.Add(dA_PAYLOSS);
        }

        public void AddDA_PAYMONTH(DA_PAYMONTH dA_PAYMONTH)
        {
            if (dA_PAYMONTH == null)
            {
                throw new ArgumentNullException(nameof(dA_PAYMONTH));
            }
            dA_PAYMONTH.Id = Guid.NewGuid();
            dA_PAYMONTH.CRT_DATE = DateTime.Now;
            _sqlDbContext.DA_PAYMONTHs.Add(dA_PAYMONTH);
        }

        public async Task<IEnumerable<DA_GROSS>> GetGROSS(string MO, string CLIENTNAME, string MANAGER, string DATE1, string DATE2)
        {
            var items = _sqlDbContext.DA_GROSSs as IQueryable<DA_GROSS>;
            if (!string.IsNullOrWhiteSpace(MO))
            {
                items = items.Where(x => x.DT_MO == MO.Trim());
            }
            if (!string.IsNullOrWhiteSpace(CLIENTNAME))
            {
                items = items.Where(x => x.DT_CLIENTNAME == CLIENTNAME.Trim());
            }
            if (!string.IsNullOrWhiteSpace(MANAGER))
            {
                items = items.Where(x => x.DT_MANAGER == MANAGER.Trim());
            }
            if (!string.IsNullOrWhiteSpace(DATE1))
            {
                var date1 = Convert.ToDateTime(DATE1);
                var date2 = Convert.ToDateTime(DATE2);
                items = items.Where(x => x.DT_DATE >= date1&& x.DT_DATE <= date2);
            }
            return await items.ToListAsync();
        }

        public void DeleteGROSS(DA_GROSS  dA_GROSS)
        {
            _sqlDbContext.DA_GROSSs.Remove(dA_GROSS);
        }

        public async Task<DA_GROSS> GetGROSS(string Guids)
        {
            var item=_sqlDbContext.DA_GROSSs.Where(x => x.Id.ToString() == Guids).FirstOrDefaultAsync(); 
            return await item;
        }

        public async Task<IEnumerable<DA_ELSEGROSS>> GetELSEGROSS(string cLIENTNAME, string mODEL, string dATE1, string dATE2)
        {
            var items = _sqlDbContext.DA_ELSEGROSSs as IQueryable<DA_ELSEGROSS>;
            if (!string.IsNullOrWhiteSpace(mODEL))
            {
                items = items.Where(x => x.DT_MODEL.ToString() == mODEL);
            }
            if (!string.IsNullOrWhiteSpace(cLIENTNAME))
            {
                items = items.Where(x => x.DT_CLIENTNAME == cLIENTNAME);
            }
            if (!string.IsNullOrWhiteSpace(dATE1))
            {
                var date1 = Convert.ToDateTime(dATE1);
                var date2 = Convert.ToDateTime(dATE2);
                items = items.Where(x => x.DT_DATE >= date1 && x.DT_DATE <= date2);
            }
            return await items.ToListAsync();
        }

        public async Task<DA_ELSEGROSS> GetELSEGROSS(string Guids)
        {
            var item = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.Id.ToString() == Guids).FirstOrDefaultAsync();
            return await item;
        }

        public void DeleteELSEGROSS(DA_ELSEGROSS dA_GROSS)
        {
            _sqlDbContext.DA_ELSEGROSSs.Remove(dA_GROSS);
        }

        public async Task<IEnumerable<DA_PAYPERSON>> GetPAYPERSON(string cLIENTNAME, string mODEL, string dATE1, string dATE2)
        {
            var items = _sqlDbContext.DA_PAYPERSONs as IQueryable<DA_PAYPERSON>;
            if (!string.IsNullOrWhiteSpace(mODEL))
            {
                items = items.Where(x => x.DT_MODEL.ToString() == mODEL);
            }
            if (!string.IsNullOrWhiteSpace(cLIENTNAME))
            {
                items = items.Where(x => x.DT_CLIENTNAME == cLIENTNAME);
            }
            if (!string.IsNullOrWhiteSpace(dATE1))
            {
                var date1 = Convert.ToDateTime(dATE1);
                var date2 = Convert.ToDateTime(dATE2);
                items = items.Where(x => x.DT_DATE >= date1 && x.DT_DATE <= date2);
            }
            return await items.ToListAsync();
        }

        public async Task<IEnumerable<DA_PAYLOSS>> GetPAYLOSS(string cLIENTNAME, string mODEL, string dATE1, string dATE2)
        {
            var items = _sqlDbContext.DA_PAYLOSSs as IQueryable<DA_PAYLOSS>;
            if (!string.IsNullOrWhiteSpace(mODEL))
            {
                items = items.Where(x => x.DT_MODEL.ToString() == mODEL);
            }
            if (!string.IsNullOrWhiteSpace(cLIENTNAME))
            {
                items = items.Where(x => x.DT_CLIENTNAME == cLIENTNAME);
            }
            if (!string.IsNullOrWhiteSpace(dATE1))
            {
                var date1 = Convert.ToDateTime(dATE1);
                var date2 = Convert.ToDateTime(dATE2);
                items = items.Where(x => x.DT_DATE >= date1 && x.DT_DATE <= date2);
            }
            return await items.ToListAsync();
        }

        public async Task<IEnumerable<DA_PAYMONTH>> GetPAYMONTH(string cLIENTNAME, string mODEL, string dATE1, string dATE2)
        {
            var items = _sqlDbContext.DA_PAYMONTHs as IQueryable<DA_PAYMONTH>;
            if (!string.IsNullOrWhiteSpace(mODEL))
            {
                items = items.Where(x => x.DT_MODEL.ToString() == mODEL);
            }
            if (!string.IsNullOrWhiteSpace(cLIENTNAME))
            {
                items = items.Where(x => x.DT_CLIENTNAME == cLIENTNAME);
            }
            if (!string.IsNullOrWhiteSpace(dATE1))
            {
                var date1 = Convert.ToDateTime(dATE1);
                var date2 = Convert.ToDateTime(dATE2);
                items = items.Where(x => x.DT_DATE >= date1 && x.DT_DATE <= date2);
            }
            return await items.ToListAsync();
        }

        public async Task<DA_PAYPERSON> GetPAYPERSON(string Guids)
        {
            var item = _sqlDbContext.DA_PAYPERSONs.Where(x => x.Id.ToString() == Guids).FirstOrDefaultAsync();
            return await item;
        }

        public void DeletePAYPERSON(DA_PAYPERSON dA_GROSS)
        {
            _sqlDbContext.DA_PAYPERSONs.Remove(dA_GROSS);
        }

        public async Task<DA_PAYLOSS> GetPAYLOSS(string Guids)
        {
            var item = _sqlDbContext.DA_PAYLOSSs.Where(x => x.Id.ToString() == Guids).FirstOrDefaultAsync();
            return await item;
        }

        public void DeletePAYLOSS(DA_PAYLOSS dA_GROSS)
        {
            _sqlDbContext.DA_PAYLOSSs.Remove(dA_GROSS);
        }

        public async Task<DA_PAYMONTH> GetPAYMONTH(string Guids)
        {
            var item = _sqlDbContext.DA_PAYMONTHs.Where(x => x.Id.ToString() == Guids).FirstOrDefaultAsync();
            return await item;
        }

        public void DeletePAYMONTH(DA_PAYMONTH dA_GROSS)
        {
            _sqlDbContext.DA_PAYMONTHs.Remove(dA_GROSS);
        }

        public async Task<IEnumerable<DA_BOM>> GetBom(string Boms, string dATE1, string dATE2,string cLientname)
        {
            var items = _sqlDbContext.DA_BOMs as IQueryable<DA_BOM>;
            if (!string.IsNullOrWhiteSpace(Boms))
            {
                items = items.Where(x => x.BOM.ToString() == Boms);
            }
            if (!string.IsNullOrWhiteSpace(cLientname))
            {
                items = items.Where(x => x.CLIENTNAME == cLientname);
            }
            if (!string.IsNullOrWhiteSpace(dATE1))
            {
                var date1 = Convert.ToDateTime(dATE1+" 00:00:00");
                var date2 = Convert.ToDateTime(dATE2 + " 23:59:59");
                items = items.Where(x => x.CRT_DATE >= date1 && x.CRT_DATE <= date2);
            }
            return await items.Take(100).ToListAsync();
        }

        public async Task<DA_BOM> GetPrice(string Guids)
        {
            var item = _sqlDbContext.DA_BOMs.Where(x => x.Id.ToString() == Guids).FirstOrDefaultAsync();
            return await item;
        }

        public void DeletePrice(DA_BOM dA_BOM)
        {
            _sqlDbContext.DA_BOMs.Remove(dA_BOM);
        }
        public async Task<IEnumerable<DA_CLENTNAME>> GET_CLENTNAME(string NAME)
        {
            var items = _sqlDbContext.DA_CLENTNAME as IQueryable<DA_CLENTNAME>;
            if (!string.IsNullOrWhiteSpace(NAME))
            {
                items = items.Where(x => x.NAME.ToString() == NAME);
            }
            return await items.Take(100).OrderBy(x=>x.CODE).ToListAsync();
        }
        public async Task<IEnumerable<DA_CLENTNAME>> GET_CLENTNAMEBU1(string NAME)
        {
            var items = _sqlDbContext.DA_CLENTNAME as IQueryable<DA_CLENTNAME>;
            if (!string.IsNullOrWhiteSpace(NAME))
            {
                items = items.Where(x => x.NAME.ToString() == NAME);
            }
            return await items.Where(x=>x.BU==1).Take(100).OrderBy(x => x.CODE).ToListAsync();
        }
        public async Task<DA_CLENTNAME> GET_CLENTNAME1(string NAME)
        {
            var items = _sqlDbContext.DA_CLENTNAME as IQueryable<DA_CLENTNAME>;
            if (!string.IsNullOrWhiteSpace(NAME))
            {
                items = items.Where(x => x.NAME.ToString() == NAME);
            }
            return await items.FirstOrDefaultAsync();
        }

        public void AddDA_CLENTNAME(DA_CLENTNAME dA_CLENTNAME)
        {
            _sqlDbContext.DA_CLENTNAME.Add(dA_CLENTNAME);
        }

        public void UpdateDA_CLENTNAME(DA_CLENTNAME dA_CLENTNAME)
        {
            //throw new NotImplementedException();
        }

        public void DeleteCLENTNAME(DA_CLENTNAME dA_)
        {
            _sqlDbContext.DA_CLENTNAME.Remove(dA_);
        }

        public async Task<IEnumerable<DA_VIEW>> getDA_VIEW(string dATE1, string cLIENTNAME)
        {
            List<DA_VIEW> dA_VIEWs = new List<DA_VIEW>();
            List<DA_VIEW> dA_VIEWs1 = new List<DA_VIEW>();// 
            List<DA_VIEW> dA_VIEWs2 = new List<DA_VIEW>();//收入统计
            List<DA_VIEW> dA_VIEWs3 = new List<DA_VIEW>();//支出统计
            List<DA_VIEW> dA_VIEWsIns = new List<DA_VIEW>();//入库总数
            #region
            double t01 = 0;
            double t02 = 0;
            double t03 = 0;
            double t04 = 0;
            double t05 = 0;
            double t06 = 0;
            double t07 = 0;
            double t08 = 0;
            double t09 = 0;
            double t10 = 0;
            double t11 = 0;
            double t12 = 0;
            double t13 = 0;
            double t14 = 0;
            double t15 = 0;
            double t16 = 0;
            double t17 = 0;
            double t18 = 0;
            double t19 = 0;
            double t20 = 0;
            double t21 = 0;
            double t22 = 0;
            double t23 = 0;
            double t24 = 0;
            double t25 = 0;
            double t26 = 0;
            double t27 = 0;
            double t28 = 0;
            double t29 = 0;
            double t30 = 0;
            double t31 = 0;
            double ttal = 0;
            double tO01 = 0;
            double tO02 = 0;
            double tO03 = 0;
            double tO04 = 0;
            double tO05 = 0;
            double tO06 = 0;
            double tO07 = 0;
            double tO08 = 0;
            double tO09 = 0;
            double tO10 = 0;
            double tO11 = 0;
            double tO12 = 0;
            double tO13 = 0;
            double tO14 = 0;
            double tO15 = 0;
            double tO16 = 0;
            double tO17 = 0;
            double tO18 = 0;
            double tO19 = 0;
            double tO20 = 0;
            double tO21 = 0;
            double tO22 = 0;
            double tO23 = 0;
            double tO24 = 0;
            double tO25 = 0;
            double tO26 = 0;
            double tO27 = 0;
            double tO28 = 0;
            double tO29 = 0;
            double tO30 = 0;
            double tO31 = 0;
            double tOtal = 0;
            #endregion
            var DA_ = await _sqlDbContext.DA_CLENTNAME.Where(x => x.BU == 1).OrderBy(x => x.CODE).ToListAsync(); 
            var DAtable= await _sqlDbContext.DT_INSTORE.Where(x => x.DT_DATE.Value.ToString("yyyy-MM") == dATE1).Where(x=>x.DT_NAME!= "KD组件"&&x.DT_NAME!= "KD散料").ToListAsync();
            if (DA_.Count>0)
            {
                var dtos = from q in DAtable
                           from p in _sqlDbContext.DA_BOMs
                           where q.DT_BOM == p.BOM
                           select new DA_GROSSDtos
                           {
                               DT_BOM = q.DT_BOM,
                               DT_CLIENTCODE = p.CLIENTCODE,
                               DT_CLIENTNAME = p.CLIENTNAME,
                               DT_STORENB = q.DT_COUNT.ToString(),
                               DT_DATE = q.DT_DATE.Value.ToString("yyyy-MM-dd"),
                               DT_PRICE = q.DT_PRICE,
                               DT_MODELNAME = p.MODEL,
                           };
                IEnumerable<DA_GROSSDtos> query = dtos.DistinctBy(p => p.DT_MODELNAME);
                var DA_GROSSs = query.ToList();
                foreach (var item in DA_)
                {
                    #region
                    DA_VIEW dA_VIEW = new DA_VIEW();
                    DA_VIEW dA_VIEW1 = new DA_VIEW();//主营收入
                    DA_VIEW dA_VIEW2 = new DA_VIEW();//其他收入（元）
                    DA_VIEW dA_VIEW3 = new DA_VIEW();// 收入小计
                    var CliemtName = item.NAME;
                    dA_VIEW.TYPES = CliemtName;
                    dA_VIEW1.TYPES = CliemtName;
                    dA_VIEW2.TYPES = CliemtName;
                    dA_VIEW3.TYPES = CliemtName;
                    dA_VIEW.TYPES1 = "整机入库数";
                    dA_VIEW1.TYPES1 = "主营收入（元）";
                    dA_VIEW2.TYPES1 = "其他收入（元）";
                    dA_VIEW3.TYPES1 = "收入小计（元）";
                    dA_VIEW.MODEL = "";
                    dA_VIEW1.MODEL = "整机";
                    dA_VIEW2.MODEL = "";
                    dA_VIEW3.MODEL = "";
                    var Dates1 = dATE1 + "-01";
                    var valGross1 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates1);
                    var sumGross1 = valGross1.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_1 = sumGross1.ToString();
                    var priceGross1 = valGross1.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_1 = priceGross1.ToString();
                    var valelseGross1 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                    var sumrlseGross1 = valelseGross1.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_1 = sumrlseGross1.ToString();
                    dA_VIEW3.DT_1 = (priceGross1 + sumrlseGross1).ToString();

                    var Dates2 = dATE1 + "-02";
                    var valGross2 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates2);
                    var sumGross2 = valGross2.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_2 = sumGross2.ToString();
                    var priceGross2 = valGross2.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_2 = priceGross2.ToString();
                    var valelseGross2 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates2);
                    var sumrlseGross2 = valelseGross2.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_2 = sumrlseGross2.ToString();
                    dA_VIEW3.DT_2 = (priceGross2 + sumrlseGross2).ToString();

                    var Dates3 = dATE1 + "-03";
                    var valGross3 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates3);
                    var sumGross3 = valGross3.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_3 = sumGross3.ToString();
                    var priceGross3 = valGross3.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_3 = priceGross3.ToString();
                    var valelseGross3 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates3);
                    var sumrlseGross3 = valelseGross3.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_3 = sumrlseGross3.ToString();
                    dA_VIEW3.DT_3 = (priceGross3 + sumrlseGross3).ToString();

                    var Dates4 = dATE1 + "-04";
                    var valGross4 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates4);
                    var sumGross4 = valGross4.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_4 = sumGross4.ToString();
                    var priceGross4 = valGross4.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_4 = priceGross4.ToString();
                    var valelseGross4 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates4);
                    var sumrlseGross4 = valelseGross4.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_4 = sumrlseGross4.ToString();
                    dA_VIEW3.DT_4 = (priceGross4 + sumrlseGross4).ToString();

                    var Dates5 = dATE1 + "-05";
                    var valGross5 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates5);
                    var sumGross5 = valGross5.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_5 = sumGross5.ToString();
                    var priceGross5 = valGross5.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_5 = priceGross5.ToString();
                    var valelseGross5 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates5);
                    var sumrlseGross5 = valelseGross5.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_5 = sumrlseGross5.ToString();
                    dA_VIEW3.DT_5 = (priceGross5 + sumrlseGross5).ToString();

                    var Dates6 = dATE1 + "-06";
                    var valGross6 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates6);
                    var sumGross6 = valGross6.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_6 = sumGross6.ToString();
                    var priceGross6 = valGross6.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_6 = priceGross6.ToString();
                    var valelseGross6 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates6);
                    var sumrlseGross6 = valelseGross6.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_6 = sumrlseGross6.ToString();
                    dA_VIEW3.DT_6 = (priceGross6 + sumrlseGross6).ToString();

                    var Dates7 = dATE1 + "-07";
                    var valGross7 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates7);
                    var sumGross7 = valGross7.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_7 = sumGross7.ToString();
                    var priceGross7 = valGross7.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_7 = priceGross7.ToString();
                    var valelseGross7 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates7);
                    var sumrlseGross7 = valelseGross7.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_7 = sumrlseGross7.ToString();
                    dA_VIEW3.DT_7 = (priceGross7 + sumrlseGross7).ToString();

                    var Dates8 = dATE1 + "-08";
                    var valGross8 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates8);
                    var sumGross8 = valGross8.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_8 = sumGross8.ToString();
                    var priceGross8 = valGross8.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_8 = priceGross8.ToString();
                    var valelseGross8 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates8);
                    var sumrlseGross8 = valelseGross8.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_8 = sumrlseGross8.ToString();
                    dA_VIEW3.DT_8 = (priceGross8 + sumrlseGross8).ToString();

                    var Dates9 = dATE1 + "-09";
                    var valGross9 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates9);
                    var sumGross9 = valGross9.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_9 = sumGross9.ToString();
                    var priceGross9 = valGross9.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_9 = priceGross9.ToString();
                    var valelseGross9 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates9);
                    var sumrlseGross9 = valelseGross9.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_9 = sumrlseGross9.ToString();
                    dA_VIEW3.DT_9 = (priceGross9 + sumrlseGross9).ToString();

                    var Dates10 = dATE1 + "-10";
                    var valGross10 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates10);
                    var sumGross10 = valGross10.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_10 = sumGross10.ToString();
                    var priceGross10 = valGross10.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_10 = priceGross10.ToString();
                    var valelseGross10 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates10);
                    var sumrlseGross10 = valelseGross10.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_10 = sumrlseGross10.ToString();
                    dA_VIEW3.DT_10 = (priceGross10 + sumrlseGross10).ToString();

                    var Dates11 = dATE1 + "-11";
                    var valGross11 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates11);
                    var sumGross11 = valGross11.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_11 = sumGross11.ToString();
                    var priceGross11 = valGross11.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_11 = priceGross11.ToString();
                    var valelseGross11 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates11);
                    var sumrlseGross11 = valelseGross11.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_11 = sumrlseGross11.ToString();
                    dA_VIEW3.DT_11 = (priceGross11 + sumrlseGross11).ToString();

                    string Dates12 = dATE1 + "-12";
                    var valGross12 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates12);
                    var sumGross12 = valGross12.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_12 = sumGross12.ToString();
                    var priceGross12 = valGross12.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_12 = priceGross12.ToString();
                    var valelseGross12 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates12);
                    var sumrlseGross12 = valelseGross12.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_12 = sumrlseGross12.ToString();
                    dA_VIEW3.DT_12 = (priceGross12 + sumrlseGross12).ToString();

                    var Dates13 = dATE1 + "-13";
                    var valGross13 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates13);
                    var sumGross13 = valGross13.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_13 = sumGross13.ToString();
                    var priceGross13 = valGross13.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_13 = priceGross13.ToString();
                    var valelseGross13 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates13);
                    var sumrlseGross13 = valelseGross13.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_13 = sumrlseGross13.ToString();
                    dA_VIEW3.DT_13 = (priceGross13 + sumrlseGross13).ToString();

                    var Dates14 = dATE1 + "-14";
                    var valGross14 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates14);
                    var sumGross14 = valGross14.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_14 = sumGross14.ToString();
                    var priceGross14 = valGross14.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_14 = priceGross14.ToString();
                    var valelseGross14 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates14);
                    var sumrlseGross14 = valelseGross14.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_14 = sumrlseGross14.ToString();
                    dA_VIEW3.DT_14 = (priceGross14 + sumrlseGross14).ToString();

                    var Dates15 = dATE1 + "-15";
                    var valGross15 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates15);
                    var sumGross15 = valGross15.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_15 = sumGross15.ToString();
                    var priceGross15 = valGross15.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_15 = priceGross15.ToString();
                    var valelseGross15 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates15);
                    var sumrlseGross15 = valelseGross15.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_15 = sumrlseGross15.ToString();
                    dA_VIEW3.DT_15 = (priceGross15 + sumrlseGross15).ToString();

                    var Dates16 = dATE1 + "-16";
                    var valGross16 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates16);
                    var sumGross16 = valGross16.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_16 = sumGross16.ToString();
                    var priceGross16 = valGross16.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_16 = priceGross16.ToString();
                    var valelseGross16 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates16);
                    var sumrlseGross16 = valelseGross16.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_16 = sumrlseGross16.ToString();
                    dA_VIEW3.DT_16 = (priceGross16 + sumrlseGross16).ToString();

                    var Dates17 = dATE1 + "-17";
                    var valGross17 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates17);
                    var sumGross17 = valGross17.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_17 = sumGross17.ToString();
                    var priceGross17 = valGross17.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_17 = priceGross17.ToString();
                    var valelseGross17 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates17);
                    var sumrlseGross17 = valelseGross17.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_17 = sumrlseGross17.ToString();
                    dA_VIEW3.DT_17 = (priceGross17 + sumrlseGross17).ToString();

                    var Dates18 = dATE1 + "-18";
                    var valGross18 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates18);
                    var sumGross18 = valGross18.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_18 = sumGross18.ToString();
                    var priceGross18 = valGross18.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_18 = priceGross18.ToString();
                    var valelseGross18 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates18);
                    var sumrlseGross18 = valelseGross18.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_18 = sumrlseGross18.ToString();
                    dA_VIEW3.DT_18 = (priceGross18 + sumrlseGross18).ToString();

                    var Dates19 = dATE1 + "-19";
                    var valGross19 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates19);
                    var sumGross19 = valGross19.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_19 = sumGross19.ToString();
                    var priceGross19 = valGross19.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_19 = priceGross19.ToString();
                    var valelseGross19 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates19);
                    var sumrlseGross19 = valelseGross19.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_19 = sumrlseGross19.ToString();
                    dA_VIEW3.DT_19 = (priceGross19 + sumrlseGross19).ToString();

                    var Dates20 = dATE1 + "-20";
                    var valGross20 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates20);
                    var sumGross20 = valGross20.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_20 = sumGross20.ToString();
                    var priceGross20 = valGross20.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_20 = priceGross20.ToString();
                    var valelseGross20 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates20);
                    var sumrlseGross20 = valelseGross20.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_20 = sumrlseGross20.ToString();
                    dA_VIEW3.DT_20 = (priceGross20 + sumrlseGross20).ToString();

                    var Dates21 = dATE1 + "-21";
                    var valGross21 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates21);
                    var sumGross21 = valGross21.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_21 = sumGross21.ToString();
                    var priceGross21 = valGross21.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_21 = priceGross21.ToString();
                    var valelseGross21 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates21);
                    var sumrlseGross21 = valelseGross21.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_21 = sumrlseGross21.ToString();
                    dA_VIEW3.DT_21 = (priceGross21 + sumrlseGross21).ToString();

                    var Dates22 = dATE1 + "-22";
                    var valGross22 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates22);
                    var sumGross22 = valGross22.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_22 = sumGross22.ToString();
                    var priceGross22 = valGross22.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_22 = priceGross22.ToString();
                    var valelseGross22 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates22);
                    var sumrlseGross22 = valelseGross22.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_22 = sumrlseGross22.ToString();
                    dA_VIEW3.DT_22 = (priceGross22 + sumrlseGross22).ToString();

                    var Dates23 = dATE1 + "-23";
                    var valGross23 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates23);
                    var sumGross23 = valGross23.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_23 = sumGross23.ToString();
                    var priceGross23 = valGross23.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_23 = priceGross23.ToString();
                    var valelseGross23 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates23);
                    var sumrlseGross23 = valelseGross23.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_23 = sumrlseGross23.ToString();
                    dA_VIEW3.DT_23 = (priceGross23 + sumrlseGross23).ToString();

                    var Dates24 = dATE1 + "-24";
                    var valGross24 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates24);
                    var sumGross24 = valGross24.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_24 = sumGross24.ToString();
                    var priceGross24 = valGross24.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_24 = priceGross24.ToString();
                    var valelseGross24 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates24);
                    var sumrlseGross24 = valelseGross24.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_24 = sumrlseGross24.ToString();
                    dA_VIEW3.DT_24 = (priceGross24 + sumrlseGross24).ToString();

                    var Dates25 = dATE1 + "-25";
                    var valGross25 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates25);
                    var sumGross25 = valGross25.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_25 = sumGross25.ToString();
                    var priceGross25 = valGross25.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_25 = priceGross25.ToString();
                    var valelseGross25 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates25);
                    var sumrlseGross25 = valelseGross25.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_25 = sumrlseGross25.ToString();
                    dA_VIEW3.DT_25 = (priceGross25 + sumrlseGross25).ToString();
                    var Dates26 = dATE1 + "-26";
                    var valGross26 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates26);
                    var sumGross26 = valGross26.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_26 = sumGross26.ToString();
                    var priceGross26 = valGross26.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_26 = priceGross26.ToString();
                    var valelseGross26 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates26);
                    var sumrlseGross26 = valelseGross26.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_26 = sumrlseGross26.ToString();
                    dA_VIEW3.DT_26 = (priceGross26 + sumrlseGross26).ToString();

                    var Dates27 = dATE1 + "-27";
                    var valGross27 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates27);
                    var sumGross27 = valGross27.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_27 = sumGross27.ToString();
                    var priceGross27 = valGross27.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_27 = priceGross27.ToString();
                    var valelseGross27 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates27);
                    var sumrlseGross27 = valelseGross27.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_27 = sumrlseGross27.ToString();
                    dA_VIEW3.DT_27 = (priceGross27 + sumrlseGross27).ToString();

                    var Dates28 = dATE1 + "-28";
                    var valGross28 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates28);
                    var sumGross28 = valGross28.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_28 = sumGross28.ToString();
                    var priceGross28 = valGross28.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_28 = priceGross28.ToString();
                    var valelseGross28 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates28);
                    var sumrlseGross28 = valelseGross28.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_28 = sumrlseGross28.ToString();
                    dA_VIEW3.DT_28 = (priceGross28 + sumrlseGross28).ToString();

                    var Dates29 = dATE1 + "-29";
                    var valGross29 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates29);
                    var sumGross29 = valGross29.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_29 = sumGross29.ToString();
                    var priceGross29 = valGross29.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_29 = priceGross29.ToString();
                    var valelseGross29 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates29);
                    var sumrlseGross29 = valelseGross29.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_29 = sumrlseGross29.ToString();
                    dA_VIEW3.DT_29 = (priceGross29 + sumrlseGross29).ToString();

                    var Dates30 = dATE1 + "-30";
                    var valGross30 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates30);
                    var sumGross30 = valGross30.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_30 = sumGross30.ToString();
                    var priceGross30 = valGross30.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_30 = priceGross30.ToString();
                    var valelseGross30 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates30);
                    var sumrlseGross30 = valelseGross30.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_30 = sumrlseGross30.ToString();
                    dA_VIEW3.DT_30 = (priceGross30 + sumrlseGross30).ToString();

                    var Dates31 = dATE1 + "-31";
                    var valGross31 = dtos.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE == Dates31);
                    var sumGross31 = valGross31.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    dA_VIEW.DT_31 = sumGross31.ToString();
                    var priceGross31 = valGross31.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                    dA_VIEW1.DT_31 = priceGross31.ToString();
                    var valelseGross31 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates31);
                    var sumrlseGross31 = valelseGross31.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                    dA_VIEW2.DT_31 = sumrlseGross31.ToString();
                    dA_VIEW3.DT_31 = (priceGross31 + sumrlseGross31).ToString();

                    dA_VIEW.TOTAL = (sumGross1 + sumGross2 + sumGross3 + sumGross4 + sumGross5 + sumGross6 + sumGross7 + sumGross8 + sumGross9 + sumGross10 + sumGross11 + sumGross12 + sumGross13 + sumGross14 + sumGross15 + sumGross16 + sumGross17 + sumGross18 + sumGross19 + sumGross20 + sumGross21 + sumGross22 + sumGross23 + sumGross24 + sumGross25 + sumGross26 + sumGross27 + sumGross28 + sumGross29 + sumGross30 + sumGross31).ToString();
                    dA_VIEW1.TOTAL = (priceGross1 + priceGross2 + priceGross3 + priceGross4 + priceGross5 + priceGross6 + priceGross7 + priceGross8 + priceGross9 + priceGross10 + priceGross11 + priceGross12 + priceGross13 + priceGross14 + priceGross15 + priceGross16 + priceGross17 + priceGross18 + priceGross19 + priceGross20 + priceGross21 + priceGross22 + priceGross23 + priceGross24 + priceGross25 + priceGross26 + priceGross27 + priceGross28 + priceGross29 + priceGross30 + priceGross31).ToString();
                    dA_VIEW2.TOTAL = (sumrlseGross1 + sumrlseGross2 + sumrlseGross3 + sumrlseGross4 + sumrlseGross5 + sumrlseGross6 + sumrlseGross7 + sumrlseGross8 + sumrlseGross9 + sumrlseGross10 + sumrlseGross11 + sumrlseGross12 + sumrlseGross13 + sumrlseGross14 + sumrlseGross15 + sumrlseGross16 + sumrlseGross17 + sumrlseGross18 + sumrlseGross19 + sumrlseGross20 + sumrlseGross21 + sumrlseGross22 + sumrlseGross23 + sumrlseGross24 + sumrlseGross25 + sumrlseGross26 + sumrlseGross27 + sumrlseGross28 + sumrlseGross29 + sumrlseGross30 + sumrlseGross31).ToString();
                    dA_VIEW3.TOTAL = (Convert.ToDouble(dA_VIEW1.TOTAL) + Convert.ToDouble(dA_VIEW2.TOTAL)).ToString();
                    dA_VIEWs.Add(dA_VIEW);
                    dA_VIEWs.Add(dA_VIEW1);
                    dA_VIEWs.Add(dA_VIEW2);
                    dA_VIEWs.Add(dA_VIEW3);
                    dA_VIEWs2.Add(dA_VIEW3);
                    dA_VIEWsIns.Add(dA_VIEW);
                    t01 += Convert.ToDouble(dA_VIEW3.DT_1);
                    t02 += Convert.ToDouble(dA_VIEW3.DT_2);
                    t03 += Convert.ToDouble(dA_VIEW3.DT_3);
                    t04 += Convert.ToDouble(dA_VIEW3.DT_4);
                    t05 += Convert.ToDouble(dA_VIEW3.DT_5);
                    t06 += Convert.ToDouble(dA_VIEW3.DT_6);
                    t07 += Convert.ToDouble(dA_VIEW3.DT_7);
                    t08 += Convert.ToDouble(dA_VIEW3.DT_8);
                    t09 += Convert.ToDouble(dA_VIEW3.DT_9);
                    t10 += Convert.ToDouble(dA_VIEW3.DT_10);
                    t11 += Convert.ToDouble(dA_VIEW3.DT_11);
                    t12 += Convert.ToDouble(dA_VIEW3.DT_12);
                    t13 += Convert.ToDouble(dA_VIEW3.DT_13);
                    t14 += Convert.ToDouble(dA_VIEW3.DT_14);
                    t15 += Convert.ToDouble(dA_VIEW3.DT_15);
                    t16 += Convert.ToDouble(dA_VIEW3.DT_16);
                    t17 += Convert.ToDouble(dA_VIEW3.DT_17);
                    t18 += Convert.ToDouble(dA_VIEW3.DT_18);
                    t19 += Convert.ToDouble(dA_VIEW3.DT_19);
                    t20 += Convert.ToDouble(dA_VIEW3.DT_20);
                    t21 += Convert.ToDouble(dA_VIEW3.DT_21);
                    t22 += Convert.ToDouble(dA_VIEW3.DT_22);
                    t23 += Convert.ToDouble(dA_VIEW3.DT_23);
                    t24 += Convert.ToDouble(dA_VIEW3.DT_24);
                    t25 += Convert.ToDouble(dA_VIEW3.DT_25);
                    t26 += Convert.ToDouble(dA_VIEW3.DT_26);
                    t27 += Convert.ToDouble(dA_VIEW3.DT_27);
                    t28 += Convert.ToDouble(dA_VIEW3.DT_28);
                    t29 += Convert.ToDouble(dA_VIEW3.DT_29);
                    t30 += Convert.ToDouble(dA_VIEW3.DT_30);
                    t31 += Convert.ToDouble(dA_VIEW3.DT_31);
                    ttal += Convert.ToDouble(dA_VIEW3.TOTAL);
                    #endregion
                }
            }
            var DAtable1 = await _sqlDbContext.DT_INSTORE.Where(x => x.DT_DATE.Value.ToString("yyyy-MM") == dATE1).Where(x => x.DT_NAME== "KD组件").ToListAsync();
            var DAtable2 = await _sqlDbContext.DT_INSTORE.Where(x => x.DT_DATE.Value.ToString("yyyy-MM") == dATE1).Where(x => x.DT_NAME == "KD散料").ToListAsync();
            var DA_1 = await _sqlDbContext.DA_CLENTNAME.Where(x => x.BU == 2).OrderBy(x => x.CODE).ToArrayAsync();
            foreach (var item in DA_1)
            {
                DA_VIEW dA_VIEW1 = new DA_VIEW();//主营收入 KD入库数
                DA_VIEW dA_VIEW2 = new DA_VIEW();//主营收入
                DA_VIEW dA_VIEW3 = new DA_VIEW();//主营收入 KD入库数
                DA_VIEW dA_VIEW4 = new DA_VIEW();
                DA_VIEW dA_VIEW5 = new DA_VIEW();
                DA_VIEW dA_VIEWT1 = new DA_VIEW();
                var CliemtName = item.NAME;
                dA_VIEW1.TYPES = CliemtName;
                dA_VIEW2.TYPES = CliemtName;
                dA_VIEW3.TYPES = CliemtName;
                dA_VIEW4.TYPES = CliemtName;
                dA_VIEW5.TYPES = CliemtName;
                dA_VIEWT1.TYPES = CliemtName;
                dA_VIEW1.TYPES1 = "KD入库数";
                dA_VIEW3.TYPES1 = "KD入库数";
                dA_VIEW2.TYPES1 = "收入（元)";
                dA_VIEW4.TYPES1 = "收入（元)";
                dA_VIEW5.TYPES1 = "收入小计（元）";
                dA_VIEW1.MODEL = "组件";
                dA_VIEW3.MODEL = "散料";
                dA_VIEW2.MODEL = "组件";
                dA_VIEW4.MODEL = "散料";
                dA_VIEW5.MODEL = "";
                var Dates1 = dATE1 + "-01";
                var valGross1 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross1 = Math.Round(valGross1.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)),2);
                dA_VIEW1.DT_1 = sumGross1.ToString();
                var priceGross1 = Math.Round(valGross1.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))),2);
                dA_VIEW2.DT_1 = priceGross1.ToString();
                var valGrosst1 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst1 = Math.Round(valGrosst1.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)),2);
                dA_VIEW3.DT_1 = sumGrosst1.ToString();
                var priceGrosst1 = Math.Round(valGrosst1.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))),2);
                dA_VIEW4.DT_1 = priceGrosst1.ToString(); 

                Dates1 = dATE1 + "-02";
                var valGross2 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross2 = Math.Round(valGross2.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_2 = sumGross2.ToString();
                var priceGross2 = Math.Round(valGross2.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_2 = priceGross2.ToString();
                var valGrosst2 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst2 = Math.Round(valGrosst2.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_2 = sumGrosst2.ToString();
                var priceGrosst2 = Math.Round(valGrosst2.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_2 = priceGrosst2.ToString();

                Dates1 = dATE1 + "-03";
                var valGross3 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross3 = Math.Round(valGross3.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_3 = sumGross3.ToString();
                var priceGross3 = Math.Round(valGross3.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_3 = priceGross3.ToString();
                var valGrosst3 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst3 = Math.Round(valGrosst3.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_3 = sumGrosst3.ToString();
                var priceGrosst3 = Math.Round(valGrosst3.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_3 = priceGrosst3.ToString();

                Dates1 = dATE1 + "-04";
                var valGross4 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross4 = Math.Round(valGross4.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_4 = sumGross4.ToString();
                var priceGross4 = Math.Round(valGross4.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_4 = priceGross4.ToString();
                var valGrosst4 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst4 = Math.Round(valGrosst4.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_4 = sumGrosst4.ToString();
                var priceGrosst4 = Math.Round(valGrosst4.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_4 = priceGrosst4.ToString();

                Dates1 = dATE1 + "-05";
                var valGross5 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross5 = Math.Round(valGross5.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_5 = sumGross5.ToString();
                var priceGross5 = Math.Round(valGross5.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_5 = priceGross5.ToString();
                var valGrosst5 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst5 = Math.Round(valGrosst5.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_5 = sumGrosst5.ToString();
                var priceGrosst5 = Math.Round(valGrosst5.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_5 = priceGrosst5.ToString();

                Dates1 = dATE1 + "-06";
                var valGross6 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross6 = Math.Round(valGross6.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_6 = sumGross6.ToString();
                var priceGross6 = Math.Round(valGross6.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_6 = priceGross6.ToString();
                var valGrosst6 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst6 = Math.Round(valGrosst6.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_6 = sumGrosst6.ToString();
                var priceGrosst6 = Math.Round(valGrosst6.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_6 = priceGrosst6.ToString();

                Dates1 = dATE1 + "-07";
                var valGross7 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross7 = Math.Round(valGross7.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_7 = sumGross7.ToString();
                var priceGross7 = Math.Round(valGross7.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_7 = priceGross7.ToString();
                var valGrosst7 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst7 = Math.Round(valGrosst7.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_7 = sumGrosst7.ToString();
                var priceGrosst7 = Math.Round(valGrosst7.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_7 = priceGrosst7.ToString();

                Dates1 = dATE1 + "-08";
                var valGross8 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross8 = Math.Round(valGross8.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_8 = sumGross8.ToString();
                var priceGross8 = Math.Round(valGross8.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_8 = priceGross8.ToString();
                var valGrosst8 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst8 = Math.Round(valGrosst8.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_8 = sumGrosst8.ToString();
                var priceGrosst8 = Math.Round(valGrosst8.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_8 = priceGrosst8.ToString();

                Dates1 = dATE1 + "-09";
                var valGross9 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross9 = Math.Round(valGross9.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_9 = sumGross9.ToString();
                var priceGross9 = Math.Round(valGross9.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_9 = priceGross9.ToString();
                var valGrosst9 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst9 = Math.Round(valGrosst9.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_9 = sumGrosst9.ToString();
                var priceGrosst9 = Math.Round(valGrosst9.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_9 = priceGrosst1.ToString();

                Dates1 = dATE1 + "-10";
                var valGross10 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross10 = Math.Round(valGross10.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_10 = sumGross10.ToString();
                var priceGross10 = Math.Round(valGross10.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_10 = priceGross10.ToString();
                var valGrosst10 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst10 = Math.Round(valGrosst10.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_10 = sumGrosst10.ToString();
                var priceGrosst10 = Math.Round(valGrosst10.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_10 = priceGrosst10.ToString();

                Dates1 = dATE1 + "-11";
                var valGross11 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross11 = Math.Round(valGross11.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_11 = sumGross11.ToString();
                var priceGross11 = Math.Round(valGross11.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_11 = priceGross11.ToString();
                var valGrosst11 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst11 = Math.Round(valGrosst11.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_11 = sumGrosst11.ToString();
                var priceGrosst11 = Math.Round(valGrosst11.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_11 = priceGrosst11.ToString();

                Dates1 = dATE1 + "-12";
                var valGross12 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross12 = Math.Round(valGross12.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_12 = sumGross12.ToString();
                var priceGross12 = Math.Round(valGross12.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_12 = priceGross12.ToString();
                var valGrosst12 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst12 = Math.Round(valGrosst12.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_12 = sumGrosst12.ToString();
                var priceGrosst12 = Math.Round(valGrosst12.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_12 = priceGrosst12.ToString();

                Dates1 = dATE1 + "-13";
                var valGross13 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross13 = Math.Round(valGross13.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_13 = sumGross13.ToString();
                var priceGross13 = Math.Round(valGross13.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_13 = priceGross13.ToString();
                var valGrosst13 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst13 = Math.Round(valGrosst13.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_13 = sumGrosst13.ToString();
                var priceGrosst13 = Math.Round(valGrosst13.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_13 = priceGrosst13.ToString();

                Dates1 = dATE1 + "-14";
                var valGross14 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross14 = Math.Round(valGross14.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_14 = sumGross14.ToString();
                var priceGross14 = Math.Round(valGross14.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_14 = priceGross14.ToString();
                var valGrosst14 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst14 = Math.Round(valGrosst14.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_14 = sumGrosst14.ToString();
                var priceGrosst14 = Math.Round(valGrosst14.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_14 = priceGrosst14.ToString();

                Dates1 = dATE1 + "-15";
                var valGross15 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross15 = Math.Round(valGross15.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_15 = sumGross15.ToString();
                var priceGross15 = Math.Round(valGross15.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_15 = priceGross15.ToString();
                var valGrosst15 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst15 = Math.Round(valGrosst15.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_15 = sumGrosst15.ToString();
                var priceGrosst15 = Math.Round(valGrosst15.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_15 = priceGrosst15.ToString();

                Dates1 = dATE1 + "-16";
                var valGross16 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross16 = Math.Round(valGross16.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_16 = sumGross16.ToString();
                var priceGross16 = Math.Round(valGross16.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_16 = priceGross16.ToString();
                var valGrosst16 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst16 = Math.Round(valGrosst16.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_16 = sumGrosst16.ToString();
                var priceGrosst16 = Math.Round(valGrosst16.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_16 = priceGrosst16.ToString();

                Dates1 = dATE1 + "-17";
                var valGross17 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross17 = Math.Round(valGross17.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_17 = sumGross17.ToString();
                var priceGross17 = Math.Round(valGross17.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_17 = priceGross17.ToString();
                var valGrosst17 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst17 = Math.Round(valGrosst17.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_17 = sumGrosst17.ToString();
                var priceGrosst17 = Math.Round(valGrosst17.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_17 = priceGrosst17.ToString();

                Dates1 = dATE1 + "-18";
                var valGross18 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross18 = Math.Round(valGross18.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_18 = sumGross18.ToString();
                var priceGross18 = Math.Round(valGross18.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_18 = priceGross18.ToString();
                var valGrosst18 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst18 = Math.Round(valGrosst18.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_18 = sumGrosst18.ToString();
                var priceGrosst18 = Math.Round(valGrosst18.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_18 = priceGrosst18.ToString();

                Dates1 = dATE1 + "-19";
                var valGross19 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross19 = Math.Round(valGross19.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_19 = sumGross19.ToString();
                var priceGross19 = Math.Round(valGross19.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_19 = priceGross19.ToString();
                var valGrosst19 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst19 = Math.Round(valGrosst19.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_19 = sumGrosst19.ToString();
                var priceGrosst19 = Math.Round(valGrosst19.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_19 = priceGrosst19.ToString();

                Dates1 = dATE1 + "-20";
                var valGross20 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross20 = Math.Round(valGross20.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_20 = sumGross20.ToString();
                var priceGross20 = Math.Round(valGross20.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_20 = priceGross20.ToString();
                var valGrosst20 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst20 = Math.Round(valGrosst20.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_20 = sumGrosst20.ToString();
                var priceGrosst20 = Math.Round(valGrosst20.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_20 = priceGrosst20.ToString();

                Dates1 = dATE1 + "-21";
                var valGross21 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross21 = Math.Round(valGross21.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_21 = sumGross21.ToString();
                var priceGross21 = Math.Round(valGross21.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_21 = priceGross21.ToString();
                var valGrosst21 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst21 = Math.Round(valGrosst21.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_21 = sumGrosst21.ToString();
                var priceGrosst21 = Math.Round(valGrosst21.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_21 = priceGrosst21.ToString();

                Dates1 = dATE1 + "-22";
                var valGross22 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross22 = Math.Round(valGross22.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_22 = sumGross22.ToString();
                var priceGross22 = Math.Round(valGross22.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_22 = priceGross22.ToString();
                var valGrosst22 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst22 = Math.Round(valGrosst22.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_22 = sumGrosst22.ToString();
                var priceGrosst22 = Math.Round(valGrosst22.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_22 = priceGrosst22.ToString();

                Dates1 = dATE1 + "-23";
                var valGross23 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross23 = Math.Round(valGross23.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_23 = sumGross23.ToString();
                var priceGross23 = Math.Round(valGross23.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_23 = priceGross23.ToString();
                var valGrosst23 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst23 = Math.Round(valGrosst23.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_23 = sumGrosst23.ToString();
                var priceGrosst23 = Math.Round(valGrosst23.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_23 = priceGrosst23.ToString();

                Dates1 = dATE1 + "-24";
                var valGross24 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross24 = Math.Round(valGross24.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_24 = sumGross24.ToString();
                var priceGross24 = Math.Round(valGross24.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_24 = priceGross24.ToString();
                var valGrosst24 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst24 = Math.Round(valGrosst24.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_24 = sumGrosst24.ToString();
                var priceGrosst24 = Math.Round(valGrosst24.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_24 = priceGrosst24.ToString();

                Dates1 = dATE1 + "-25";
                var valGross25 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross25 = Math.Round(valGross25.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_25 = sumGross25.ToString();
                var priceGross25 = Math.Round(valGross25.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_25 = priceGross25.ToString();
                var valGrosst25 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst25 = Math.Round(valGrosst25.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_25 = sumGrosst25.ToString();
                var priceGrosst25 = Math.Round(valGrosst25.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_25 = priceGrosst25.ToString();

                Dates1 = dATE1 + "-26";
                var valGross26 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross26 = Math.Round(valGross26.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_26 = sumGross26.ToString();
                var priceGross26 = Math.Round(valGross26.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_26 = priceGross26.ToString();
                var valGrosst26 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst26 = Math.Round(valGrosst26.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_26 = sumGrosst26.ToString();
                var priceGrosst26 = Math.Round(valGrosst26.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_26 = priceGrosst26.ToString();

                Dates1 = dATE1 + "-27";
                var valGross27 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross27 = Math.Round(valGross27.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_27 = sumGross27.ToString();
                var priceGross27 = Math.Round(valGross27.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_27 = priceGross27.ToString();
                var valGrosst27 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst27 = Math.Round(valGrosst27.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_27 = sumGrosst27.ToString();
                var priceGrosst27 = Math.Round(valGrosst27.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_27 = priceGrosst27.ToString();

                Dates1 = dATE1 + "-28";
                var valGross28 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross28 = Math.Round(valGross28.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_28 = sumGross28.ToString();
                var priceGross28 = Math.Round(valGross28.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_28 = priceGross28.ToString();
                var valGrosst28 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst28 = Math.Round(valGrosst28.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_28 = sumGrosst28.ToString();
                var priceGrosst28 = Math.Round(valGrosst28.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_28 = priceGrosst28.ToString();

                Dates1 = dATE1 + "-29";
                var valGross29 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross29 = Math.Round(valGross29.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_29 = sumGross29.ToString();
                var priceGross29 = Math.Round(valGross29.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_29 = priceGross29.ToString();
                var valGrosst29 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst29 = Math.Round(valGrosst29.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_29 = sumGrosst29.ToString();
                var priceGrosst29 = Math.Round(valGrosst29.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_29 = priceGrosst29.ToString();

                Dates1 = dATE1 + "-30";
                var valGross30 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross30 = Math.Round(valGross30.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_30 = sumGross30.ToString();
                var priceGross30 = Math.Round(valGross30.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_30 = priceGross30.ToString();
                var valGrosst30 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst30 = Math.Round(valGrosst30.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_30 = sumGrosst30.ToString();
                var priceGrosst30 = Math.Round(valGrosst30.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_30 = priceGrosst30.ToString();

                Dates1 = dATE1 + "-31";
                var valGross31 = DAtable1.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross31 = Math.Round(valGross31.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW1.DT_31 = sumGross31.ToString();
                var priceGross31 = Math.Round(valGross31.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW2.DT_31 = priceGross31.ToString();
                var valGrosst31 = DAtable2.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst31 = Math.Round(valGrosst31.Sum(x => x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)), 2);
                dA_VIEW3.DT_31 = sumGrosst31.ToString();
                var priceGrosst31 = Math.Round(valGrosst31.Sum(x => (x.DT_COUNT == "" ? 0 : Convert.ToDouble(x.DT_COUNT)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_31 = priceGrosst31.ToString();

                dA_VIEW5.DT_1 = (priceGross1 + priceGrosst1).ToString();
                dA_VIEW5.DT_2 = (priceGross2 + priceGrosst2).ToString();
                dA_VIEW5.DT_3 = (priceGross3 + priceGrosst3).ToString();
                dA_VIEW5.DT_4 = (priceGross4 + priceGrosst4).ToString();
                dA_VIEW5.DT_5 = (priceGross5 + priceGrosst5).ToString();
                dA_VIEW5.DT_6 = (priceGross6 + priceGrosst6).ToString();
                dA_VIEW5.DT_7 = (priceGross7 + priceGrosst7).ToString();
                dA_VIEW5.DT_8 = (priceGross8 + priceGrosst8).ToString();
                dA_VIEW5.DT_9 = (priceGross9 + priceGrosst9).ToString();
                dA_VIEW5.DT_10 = (priceGross10 + priceGrosst10).ToString();
                dA_VIEW5.DT_11 = (priceGross11 + priceGrosst11).ToString();
                dA_VIEW5.DT_12 = (priceGross12 + priceGrosst12).ToString();
                dA_VIEW5.DT_13 = (priceGross13 + priceGrosst13).ToString();
                dA_VIEW5.DT_14 = (priceGross14 + priceGrosst14).ToString();
                dA_VIEW5.DT_15 = (priceGross15 + priceGrosst15).ToString();
                dA_VIEW5.DT_16 = (priceGross16 + priceGrosst16).ToString();
                dA_VIEW5.DT_17 = (priceGross17 + priceGrosst17).ToString();
                dA_VIEW5.DT_18 = (priceGross18 + priceGrosst18).ToString();
                dA_VIEW5.DT_19 = (priceGross19 + priceGrosst19).ToString();
                dA_VIEW5.DT_20 = (priceGross20 + priceGrosst20).ToString();
                dA_VIEW5.DT_21 = (priceGross21 + priceGrosst21).ToString();
                dA_VIEW5.DT_22 = (priceGross22 + priceGrosst22).ToString();
                dA_VIEW5.DT_23 = (priceGross23 + priceGrosst23).ToString();
                dA_VIEW5.DT_24 = (priceGross24 + priceGrosst24).ToString();
                dA_VIEW5.DT_25 = (priceGross25 + priceGrosst25).ToString();
                dA_VIEW5.DT_26 = (priceGross26 + priceGrosst26).ToString();
                dA_VIEW5.DT_27 = (priceGross27 + priceGrosst27).ToString();
                dA_VIEW5.DT_28 = (priceGross28 + priceGrosst28).ToString();
                dA_VIEW5.DT_29 = (priceGross29 + priceGrosst29).ToString();
                dA_VIEW5.DT_30 = (priceGross30 + priceGrosst30).ToString();
                dA_VIEW5.DT_31 = (priceGross31 + priceGrosst31).ToString();
                dA_VIEWT1.DT_1 = (sumGross1 + sumGrosst1).ToString();
                dA_VIEWT1.DT_2 = (sumGross2 + sumGrosst2).ToString();
                dA_VIEWT1.DT_3 = (sumGross3 + sumGrosst3).ToString();
                dA_VIEWT1.DT_4 = (sumGross4 + sumGrosst4).ToString();
                dA_VIEWT1.DT_5 = (sumGross5 + sumGrosst5).ToString();
                dA_VIEWT1.DT_6 = (sumGross6 + sumGrosst6).ToString();
                dA_VIEWT1.DT_7 = (sumGross7 + sumGrosst7).ToString();
                dA_VIEWT1.DT_8 = (sumGross8 + sumGrosst8).ToString();
                dA_VIEWT1.DT_9 = (sumGross9 + sumGrosst9).ToString();
                dA_VIEWT1.DT_10 = (sumGross10 + sumGrosst10).ToString();
                dA_VIEWT1.DT_11 = (sumGross11 + sumGrosst11).ToString();
                dA_VIEWT1.DT_12 = (sumGross12 + sumGrosst12).ToString();
                dA_VIEWT1.DT_13 = (sumGross13 + sumGrosst13).ToString();
                dA_VIEWT1.DT_14 = (sumGross14 + sumGrosst14).ToString();
                dA_VIEWT1.DT_15 = (sumGross15 + sumGrosst15).ToString();
                dA_VIEWT1.DT_16 = (sumGross16 + sumGrosst16).ToString();
                dA_VIEWT1.DT_17 = (sumGross17 + sumGrosst17).ToString();
                dA_VIEWT1.DT_18 = (sumGross18 + sumGrosst18).ToString();
                dA_VIEWT1.DT_19 = (sumGross19 + sumGrosst19).ToString();
                dA_VIEWT1.DT_20 = (sumGross20 + sumGrosst20).ToString();
                dA_VIEWT1.DT_21 = (sumGross21 + sumGrosst21).ToString();
                dA_VIEWT1.DT_22 = (sumGross22 + sumGrosst22).ToString();
                dA_VIEWT1.DT_23 = (sumGross23 + sumGrosst23).ToString();
                dA_VIEWT1.DT_24 = (sumGross24 + sumGrosst24).ToString();
                dA_VIEWT1.DT_25 = (sumGross25 + sumGrosst25).ToString();
                dA_VIEWT1.DT_26 = (sumGross26 + sumGrosst26).ToString();
                dA_VIEWT1.DT_27 = (sumGross27 + sumGrosst27).ToString();
                dA_VIEWT1.DT_28 = (sumGross28 + sumGrosst28).ToString();
                dA_VIEWT1.DT_29 = (sumGross29 + sumGrosst29).ToString();
                dA_VIEWT1.DT_30 = (sumGross30 + sumGrosst30).ToString();
                dA_VIEWT1.DT_31 = (sumGross31 + sumGrosst31).ToString();
               
                dA_VIEW1.TOTAL = (sumGross1 + sumGross2 + sumGross3 + sumGross4 + sumGross5 + sumGross6 + sumGross7 + sumGross8 + sumGross9 + sumGross10 + sumGross11 + sumGross12 + sumGross13 + sumGross14 + sumGross15 + sumGross16 + sumGross17 + sumGross18 + sumGross19 + sumGross20 + sumGross21 + sumGross22 + sumGross23 + sumGross24 + sumGross25 + sumGross26 + sumGross27 + sumGross28 + sumGross29 + sumGross30 + sumGross31).ToString();
                dA_VIEW2.TOTAL = (priceGross1 + priceGross2 + priceGross3 + priceGross4 + priceGross5 + priceGross6 + priceGross7 + priceGross8 + priceGross9 + priceGross10 + priceGross11 + priceGross12 + priceGross13 + priceGross14 + priceGross15 + priceGross16 + priceGross17 + priceGross18 + priceGross19 + priceGross20 + priceGross21 + priceGross22 + priceGross23 + priceGross24 + priceGross25 + priceGross26 + priceGross27 + priceGross28 + priceGross29 + priceGross30 + priceGross31).ToString();
                dA_VIEW3.TOTAL = (sumGrosst1 + sumGrosst2 + sumGrosst3 + sumGrosst4 + sumGrosst5 + sumGrosst6 + sumGrosst7 + sumGrosst8 + sumGrosst9 + sumGrosst10 + sumGrosst11 + sumGrosst12 + sumGrosst13 + sumGrosst14 + sumGrosst15 + sumGrosst16 + sumGrosst17 + sumGrosst18 + sumGrosst19 + sumGrosst20 + sumGrosst21 + sumGrosst22 + sumGrosst23 + sumGrosst24 + sumGrosst25 + sumGrosst26 + sumGrosst27 + sumGrosst28 + sumGrosst29 + sumGrosst30 + sumGrosst31).ToString();
                dA_VIEW4.TOTAL = (priceGrosst1 + priceGrosst2 + priceGrosst3 + priceGrosst4 + priceGrosst5 + priceGrosst6 + priceGrosst7 + priceGrosst8 + priceGrosst9 + priceGrosst10 + priceGrosst11 + priceGrosst12 + priceGrosst13 + priceGrosst14 + priceGrosst15 + priceGrosst16 + priceGrosst17 + priceGrosst18 + priceGrosst19 + priceGrosst20 + priceGrosst21 + priceGrosst22 + priceGrosst23 + priceGrosst24 + priceGrosst25 + priceGrosst26 + priceGrosst27 + priceGrosst28 + priceGrosst29 + priceGrosst30 + priceGrosst31).ToString();
                dA_VIEW5.TOTAL = (Convert.ToDouble(dA_VIEW2.TOTAL) + Convert.ToDouble(dA_VIEW4.TOTAL)).ToString(); 
                dA_VIEWT1.TOTAL = (Convert.ToDouble(dA_VIEW1.TOTAL) + Convert.ToDouble(dA_VIEW3.TOTAL)).ToString();
                dA_VIEWs.Add(dA_VIEW1);
                dA_VIEWs.Add(dA_VIEW3);
                dA_VIEWs.Add(dA_VIEW2);
                dA_VIEWs.Add(dA_VIEW4);
                dA_VIEWs.Add(dA_VIEW5);
                dA_VIEWs2.Add(dA_VIEW5);
                dA_VIEWsIns.Add(dA_VIEWT1);
                t01 += Convert.ToDouble(dA_VIEW5.DT_1);
                t02 += Convert.ToDouble(dA_VIEW5.DT_2);
                t03 += Convert.ToDouble(dA_VIEW5.DT_3);
                t04 += Convert.ToDouble(dA_VIEW5.DT_4);
                t05 += Convert.ToDouble(dA_VIEW5.DT_5);
                t06 += Convert.ToDouble(dA_VIEW5.DT_6);
                t07 += Convert.ToDouble(dA_VIEW5.DT_7);
                t08 += Convert.ToDouble(dA_VIEW5.DT_8);
                t09 += Convert.ToDouble(dA_VIEW5.DT_9);
                t10 += Convert.ToDouble(dA_VIEW5.DT_10);
                t11 += Convert.ToDouble(dA_VIEW5.DT_11);
                t12 += Convert.ToDouble(dA_VIEW5.DT_12);
                t13 += Convert.ToDouble(dA_VIEW5.DT_13);
                t14 += Convert.ToDouble(dA_VIEW5.DT_14);
                t15 += Convert.ToDouble(dA_VIEW5.DT_15);
                t16 += Convert.ToDouble(dA_VIEW5.DT_16);
                t17 += Convert.ToDouble(dA_VIEW5.DT_17);
                t18 += Convert.ToDouble(dA_VIEW5.DT_18);
                t19 += Convert.ToDouble(dA_VIEW5.DT_19);
                t20 += Convert.ToDouble(dA_VIEW5.DT_20);
                t21 += Convert.ToDouble(dA_VIEW5.DT_21);
                t22 += Convert.ToDouble(dA_VIEW5.DT_22);
                t23 += Convert.ToDouble(dA_VIEW5.DT_23);
                t24 += Convert.ToDouble(dA_VIEW5.DT_24);
                t25 += Convert.ToDouble(dA_VIEW5.DT_25);
                t26 += Convert.ToDouble(dA_VIEW5.DT_26);
                t27 += Convert.ToDouble(dA_VIEW5.DT_27);
                t28 += Convert.ToDouble(dA_VIEW5.DT_28);
                t29 += Convert.ToDouble(dA_VIEW5.DT_29);
                t30 += Convert.ToDouble(dA_VIEW5.DT_30);
                t31 += Convert.ToDouble(dA_VIEW5.DT_31);
                ttal += Convert.ToDouble(dA_VIEW5.TOTAL);
            }
            DA_VIEW dA_VIEWT = new DA_VIEW
            {
                TYPES = "汇总",
                TYPES1 = "收入小计（元）",
                MODEL = "",
                DT_1 = t01.ToString(),
                DT_2 = t02.ToString(),
                DT_3 = t03.ToString(),
                DT_4 = t04.ToString(),
                DT_5 = t05.ToString(),
                DT_6 = t06.ToString(),
                DT_7 = t07.ToString(),
                DT_8 = t08.ToString(),
                DT_9 = t09.ToString(),
                DT_10 = t10.ToString(),
                DT_11 = t11.ToString(),
                DT_12 = t12.ToString(),
                DT_13 = t13.ToString(),
                DT_14 = t14.ToString(),
                DT_15 = t15.ToString(),
                DT_16 = t16.ToString(),
                DT_17 = t17.ToString(),
                DT_18 = t18.ToString(),
                DT_19 = t19.ToString(),
                DT_20 = t20.ToString(),
                DT_21 = t21.ToString(),
                DT_22 = t22.ToString(),
                DT_23 = t23.ToString(),
                DT_24 = t24.ToString(),
                DT_25 = t25.ToString(),
                DT_26 = t26.ToString(),
                DT_27 = t27.ToString(),
                DT_28 = t28.ToString(),
                DT_29 = t29.ToString(),
                DT_30 = t30.ToString(),
                DT_31 = t31.ToString(),
                TOTAL = ttal.ToString()
            };
            dA_VIEWs.Add(dA_VIEWT);
            //人工及料耗费用支出
            //获取时薪
            DA_BOM HourPices = await GetDA_BOM("直接人员时薪");
            double HourPice = HourPices.PRICE == "" ? 0 : Convert.ToDouble(HourPices.PRICE);
            var DA_2 = await _sqlDbContext.DA_CLENTNAME.OrderBy(x => x.CODE).ToArrayAsync();
            //支出
            foreach (var item in DA_2)
            {
                DA_VIEW dA_VIEW1 = new DA_VIEW();//直接作业人员
                DA_VIEW dA_VIEW2 = new DA_VIEW();//直接员工费用
                DA_VIEW dA_VIEW3 = new DA_VIEW();//机物料消耗  
                DA_VIEW dA_VIEW4 = new DA_VIEW();//超损物料费
                DA_VIEW dA_VIEW5 = new DA_VIEW();//单台生产费用
                DA_VIEW dA_VIEW6 = new DA_VIEW();//支出汇总
                var CliemtName = item.NAME;
                dA_VIEW1.TYPES = "人工及料耗费用支出";
                dA_VIEW2.TYPES = "人工及料耗费用支出";
                dA_VIEW3.TYPES = "人工及料耗费用支出";
                dA_VIEW4.TYPES = "人工及料耗费用支出";
                dA_VIEW5.TYPES = "人工及料耗费用支出";
                dA_VIEW6.TYPES = "支出汇总";
                dA_VIEW1.TYPES1 = CliemtName;
                dA_VIEW2.TYPES1 = CliemtName;
                dA_VIEW3.TYPES1 = CliemtName;
                dA_VIEW4.TYPES1 = CliemtName;
                dA_VIEW5.TYPES1 = CliemtName;
                dA_VIEW6.TYPES1 = CliemtName;
                dA_VIEW1.MODEL = "直接作业人员";
                dA_VIEW2.MODEL = "直接员工费用";
                dA_VIEW3.MODEL = "机物料消耗";
                dA_VIEW4.MODEL = "超损物料费";
                dA_VIEW5.MODEL = "单台生产费用";
                dA_VIEW6.MODEL = CliemtName;
                var Dates1 = dATE1 + "-01";
                var INSTORE = dA_VIEWsIns.Where(x => x.TYPES == CliemtName).FirstOrDefault();
                var valGross1 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                double sumGross1 = Math.Round(valGross1.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)),2);
                dA_VIEW1.DT_1 = sumGross1.ToString();
                double sumPiceGross1 = Math.Round(valGross1.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_1 = sumPiceGross1.ToString();
                //机物料消耗
                var valDA_PAYLOSS1 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS1 = Math.Round(valDA_PAYLOSS1.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_1 = sumDA_PAYLOSS1.ToString();
                var valPAYLOSS1 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1).Where(x => x.DT_NAME == "超损物料损耗");
                double sumPAYLOSS1 = Math.Round(valPAYLOSS1.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_1 = sumPAYLOSS1.ToString();
                double OneCount1 = INSTORE.DT_1== "0" ? 0 : Math.Round((sumDA_PAYLOSS1 + sumPiceGross1 + sumPAYLOSS1) / Convert.ToDouble(INSTORE.DT_1), 2);
                dA_VIEW5.DT_1 = OneCount1.ToString();
                dA_VIEW6.DT_1 = (sumDA_PAYLOSS1 + sumPiceGross1 + sumPAYLOSS1).ToString();

                var Dates2 = dATE1 + "-02";
                var valGross2 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates2);
                var sumGross2 = Math.Round(valGross2.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_2 = sumGross2.ToString();
                var dd = valGross2.ToList();
                var sumPiceGross2 = Math.Round(valGross2.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_2 = sumPiceGross2.ToString();
                var valDA_PAYLOSS2 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates2).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS2 = Math.Round(valDA_PAYLOSS2.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_2 = sumDA_PAYLOSS2.ToString();
                var valPAYLOSS2 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates2).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS2 = Math.Round(valPAYLOSS2.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_2 = sumPAYLOSS2.ToString();
                var OneCount2 = INSTORE.DT_2 == "0" ? 0 : Math.Round((sumDA_PAYLOSS2 + sumPiceGross2 + sumPAYLOSS2) / Convert.ToDouble(INSTORE.DT_2), 2);
                dA_VIEW5.DT_2 = OneCount2.ToString();
                dA_VIEW6.DT_2 = (sumDA_PAYLOSS2 + sumPiceGross2 + sumPAYLOSS2).ToString();

                var Dates3 = dATE1 + "-03";
                var valGross3 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates3);
                var sumGross3 = Math.Round(valGross3.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_3 = sumGross3.ToString();
                var sumPiceGross3 = Math.Round(valGross3.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_3 = sumPiceGross3.ToString();
                var valDA_PAYLOSS3 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates3).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS3 = Math.Round(valDA_PAYLOSS3.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_3 = sumDA_PAYLOSS3.ToString();
                var valPAYLOSS3 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates3).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS3 = Math.Round(valPAYLOSS3.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_3 = sumPAYLOSS3.ToString();
                var OneCount3 = INSTORE.DT_3 == "0" ? 0 : Math.Round((sumDA_PAYLOSS3 + sumPiceGross3 + sumPAYLOSS3) / Convert.ToDouble(INSTORE.DT_3), 2);
                dA_VIEW5.DT_3 = OneCount3.ToString();
                dA_VIEW6.DT_3 = (sumDA_PAYLOSS3 + sumPiceGross3 + sumPAYLOSS3).ToString();

                var Dates4 = dATE1 + "-04";
                var valGross4 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates4);
                var sumGross4 = Math.Round(valGross4.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_4 = sumGross4.ToString();
                var sumPiceGross4 = Math.Round(valGross4.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_4 = sumPiceGross4.ToString();
                var valDA_PAYLOSS4 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates4).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS4 = Math.Round(valDA_PAYLOSS4.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_4 = sumDA_PAYLOSS4.ToString();
                var valPAYLOSS4 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates4).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS4 = Math.Round(valPAYLOSS4.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_4 = sumPAYLOSS4.ToString(); 
                var OneCount4 = INSTORE.DT_4 == "0" ? 0 : Math.Round((sumDA_PAYLOSS4 + sumPiceGross4 + sumPAYLOSS4) / Convert.ToDouble(INSTORE.DT_4), 2);
                dA_VIEW5.DT_4 = OneCount4.ToString();
                dA_VIEW6.DT_4 = (sumDA_PAYLOSS4 + sumPiceGross4 + sumPAYLOSS4).ToString();

                var Dates5 = dATE1 + "-05";
                var valGross5 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates5);
                var sumGross5 = Math.Round(valGross5.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_5 = sumGross5.ToString();
                var sumPiceGross5 = Math.Round(valGross5.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_5 = sumPiceGross5.ToString();
                var valDA_PAYLOSS5 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates5).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS5 = Math.Round(valDA_PAYLOSS5.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_5 = sumDA_PAYLOSS5.ToString();
                var valPAYLOSS5 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates5).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS5 = Math.Round(valPAYLOSS5.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_5 = sumPAYLOSS5.ToString();
                var OneCount5 = INSTORE.DT_5 == "0"? 0 : Math.Round((sumDA_PAYLOSS5 + sumPiceGross5 + sumPAYLOSS5) / Convert.ToDouble(INSTORE.DT_6), 2);
                dA_VIEW5.DT_5 = OneCount5.ToString();
                dA_VIEW6.DT_5 = (sumDA_PAYLOSS5 + sumPiceGross5 + sumPAYLOSS5).ToString();

                var Dates6 = dATE1 + "-06";
                var valGross6 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates6);
                var sumGross6 = Math.Round(valGross6.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_6 = sumGross6.ToString();
                var sumPiceGross6 = Math.Round(valGross6.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_6 = sumPiceGross6.ToString();
                var valDA_PAYLOSS6 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates6).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS6 = Math.Round(valDA_PAYLOSS6.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_6 = sumDA_PAYLOSS6.ToString();
                var valPAYLOSS6 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates6).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS6 = Math.Round(valPAYLOSS6.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_6 = sumPAYLOSS6.ToString();
                var OneCount6 = INSTORE.DT_6 == "0"? 0 : Math.Round((sumDA_PAYLOSS6 + sumPiceGross6 + sumPAYLOSS6) / Convert.ToDouble(INSTORE.DT_6), 2);
                dA_VIEW5.DT_6 = OneCount6.ToString();
                dA_VIEW6.DT_6 = (sumDA_PAYLOSS6 + sumPiceGross6 + sumPAYLOSS6).ToString();

                var Dates7 = dATE1 + "-07";
                var valGross7 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates7);
                var sumGross7 = Math.Round(valGross7.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_7 = sumGross7.ToString();
                var sumPiceGross7 = Math.Round(valGross7.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_7 = sumPiceGross7.ToString();
                var valDA_PAYLOSS7 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates7).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS7 = Math.Round(valDA_PAYLOSS7.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_7 = sumDA_PAYLOSS7.ToString();
                var valPAYLOSS7 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates7).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS7 = Math.Round(valPAYLOSS7.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_7 = sumPAYLOSS7.ToString();
                var OneCount7 = INSTORE.DT_7 == "0" ? 0 : Math.Round((sumDA_PAYLOSS7 + sumPiceGross7 + sumPAYLOSS7) / Convert.ToDouble(INSTORE.DT_7), 2);
                dA_VIEW5.DT_7 = OneCount7.ToString();
                dA_VIEW6.DT_7 = (sumDA_PAYLOSS7 + sumPiceGross7 + sumPAYLOSS7).ToString();

                var Dates8 = dATE1 + "-08";
                var valGross8 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates8);
                var sumGross8 = Math.Round(valGross8.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_8 = sumGross8.ToString();
                var sumPiceGross8 = Math.Round(valGross8.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_8 = sumPiceGross8.ToString();
                var valDA_PAYLOSS8 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates8).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS8 = Math.Round(valDA_PAYLOSS8.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_8 = sumDA_PAYLOSS8.ToString();
                var valPAYLOSS8 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates8).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS8 = Math.Round(valPAYLOSS8.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_8 = sumPAYLOSS8.ToString();
                var OneCount8 = INSTORE.DT_8 == "0" ? 0 : Math.Round((sumDA_PAYLOSS8 + sumPiceGross8 + sumPAYLOSS8) / Convert.ToDouble(INSTORE.DT_8), 2);
                dA_VIEW5.DT_8 = OneCount8.ToString();
                dA_VIEW6.DT_8 = (sumDA_PAYLOSS8 + sumPiceGross8 + sumPAYLOSS8).ToString();

                var Dates9 = dATE1 + "-09";
                var valGross9 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates9);
                var sumGross9 = Math.Round(valGross9.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_9 = sumGross9.ToString();
                var sumPiceGross9 = Math.Round(valGross9.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_9 = sumPiceGross9.ToString();
                var valDA_PAYLOSS9 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates9).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS9 = Math.Round(valDA_PAYLOSS9.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_9 = sumDA_PAYLOSS9.ToString();
                var valPAYLOSS9 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates9).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS9 = Math.Round(valPAYLOSS9.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_9 = sumPAYLOSS9.ToString(); 
                var OneCount9 = INSTORE.DT_9 == "0" ? 0 : Math.Round((sumDA_PAYLOSS9 + sumPiceGross9 + sumPAYLOSS9) / Convert.ToDouble(INSTORE.DT_9), 2);
                dA_VIEW5.DT_9 = OneCount9.ToString();
                dA_VIEW6.DT_9 = (sumDA_PAYLOSS9 + sumPiceGross9 + sumPAYLOSS9).ToString();

                var Dates10 = dATE1 + "-10";
                var valGross10 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates10);
                var sumGross10 = Math.Round(valGross10.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_10 = sumGross10.ToString();
                var sumPiceGross10 = Math.Round(valGross10.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_10 = sumPiceGross10.ToString();
                var valDA_PAYLOSS10 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates10).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS10 = Math.Round(valDA_PAYLOSS10.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_10 = sumDA_PAYLOSS10.ToString();
                var valPAYLOSS10 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates10).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS10 = Math.Round(valPAYLOSS10.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_10 = sumPAYLOSS10.ToString(); 
                var OneCount10 = INSTORE.DT_10 == "0" ? 0 : Math.Round((sumDA_PAYLOSS10 + sumPiceGross10 + sumPAYLOSS10) / Convert.ToDouble(INSTORE.DT_10), 2);
                dA_VIEW5.DT_10 = OneCount10.ToString();
                dA_VIEW6.DT_10 = (sumDA_PAYLOSS10 + sumPiceGross10 + sumPAYLOSS10).ToString();

                var Dates11 = dATE1 + "-11";
                var valGross11 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates11);
                var sumGross11 = Math.Round(valGross11.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_11 = sumGross11.ToString();
                var sumPiceGross11 = Math.Round(valGross11.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_11 = sumPiceGross11.ToString();
                var valDA_PAYLOSS11 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates11).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS11 = Math.Round(valDA_PAYLOSS11.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_11 = sumDA_PAYLOSS11.ToString();
                var valPAYLOSS11 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates11).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS11 = Math.Round(valPAYLOSS11.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_11 = sumPAYLOSS11.ToString(); 
                var OneCount11 = INSTORE.DT_11 == "0" ? 0 : Math.Round((sumDA_PAYLOSS11 + sumPiceGross11 + sumPAYLOSS11) / Convert.ToDouble(INSTORE.DT_11), 2);
                dA_VIEW5.DT_11 = OneCount11.ToString();
                dA_VIEW6.DT_11 = (sumDA_PAYLOSS11 + sumPiceGross11 + sumPAYLOSS11).ToString();

                var Dates12 = dATE1 + "-12";
                var valGross12 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates12);
                var sumGross12 = Math.Round(valGross12.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_12 = sumGross12.ToString();
                var sumPiceGross12 = Math.Round(valGross12.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_12 = sumPiceGross12.ToString();
                var valDA_PAYLOSS12 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates12).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS12 = Math.Round(valDA_PAYLOSS12.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_12 = sumDA_PAYLOSS12.ToString();
                var valPAYLOSS12 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates12).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS12 = Math.Round(valPAYLOSS12.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_12 = sumPAYLOSS12.ToString(); 
                var OneCount12 = INSTORE.DT_12 == "0" ? 0 : Math.Round((sumDA_PAYLOSS12 + sumPiceGross12 + sumPAYLOSS12) / Convert.ToDouble(INSTORE.DT_12), 2);
                dA_VIEW5.DT_12 = OneCount12.ToString();
                dA_VIEW6.DT_12 = (sumDA_PAYLOSS12 + sumPiceGross12 + sumPAYLOSS12).ToString();

                var Dates13 = dATE1 + "-13";
                var valGross13 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates13);
                var sumGross13 = Math.Round(valGross13.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_13 = sumGross13.ToString();
                var sumPiceGross13 = Math.Round(valGross13.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_13 = sumPiceGross13.ToString();
                var valDA_PAYLOSS13 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates13).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS13 = Math.Round(valDA_PAYLOSS13.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_13 = sumDA_PAYLOSS13.ToString();
                var valPAYLOSS13 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates13).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS13 = Math.Round(valPAYLOSS13.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_13 = sumPAYLOSS13.ToString(); 
                var OneCount13 = INSTORE.DT_13== "0" ? 0 : Math.Round((sumDA_PAYLOSS13 + sumPiceGross13 + sumPAYLOSS13) / Convert.ToDouble(INSTORE.DT_13), 2);
                dA_VIEW5.DT_13 = OneCount13.ToString();
                dA_VIEW6.DT_13 = (sumDA_PAYLOSS13 + sumPiceGross13 + sumPAYLOSS13).ToString();

                var Dates14 = dATE1 + "-14";
                var valGross14 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates14);
                var sumGross14 = Math.Round(valGross14.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_14 = sumGross14.ToString();
                var sumPiceGross14 = Math.Round(valGross14.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_14 = sumPiceGross14.ToString();
                var valDA_PAYLOSS14 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates14).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS14 = Math.Round(valDA_PAYLOSS14.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_14 = sumDA_PAYLOSS14.ToString();
                var valPAYLOSS14 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates14).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS14 = Math.Round(valPAYLOSS14.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_14 = sumPAYLOSS14.ToString(); 
                var OneCount14 = INSTORE.DT_14 == "0" ? 0 : Math.Round((sumDA_PAYLOSS14 + sumPiceGross14 + sumPAYLOSS14) / Convert.ToDouble(INSTORE.DT_14), 2);
                dA_VIEW5.DT_14 = OneCount14.ToString();
                dA_VIEW6.DT_14 = (sumDA_PAYLOSS14 + sumPiceGross14 + sumPAYLOSS14).ToString();

                var Dates15 = dATE1 + "-15";
                var valGross15 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates15);
                var sumGross15 = Math.Round(valGross15.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_15 = sumGross15.ToString();
                var sumPiceGross15 = Math.Round(valGross15.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_15 = sumPiceGross15.ToString();
                var valDA_PAYLOSS15 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates15).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS15 = Math.Round(valDA_PAYLOSS15.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_15 = sumDA_PAYLOSS15.ToString();
                var valPAYLOSS15 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates15).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS15 = Math.Round(valPAYLOSS15.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_15 = sumPAYLOSS15.ToString(); 
                var OneCount15 = INSTORE.DT_15 == "0" ? 0 : Math.Round((sumDA_PAYLOSS15 + sumPiceGross15 + sumPAYLOSS15) / Convert.ToDouble(INSTORE.DT_15), 2);
                dA_VIEW5.DT_15 = OneCount15.ToString();
                dA_VIEW6.DT_15 = (sumDA_PAYLOSS15 + sumPiceGross15 + sumPAYLOSS15).ToString();

                var Dates16 = dATE1 + "-16";
                var valGross16 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates16);
                var sumGross16 = Math.Round(valGross16.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_16 = sumGross1.ToString();
                var sumPiceGross16 = Math.Round(valGross16.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_16 = sumPiceGross16.ToString();
                var valDA_PAYLOSS16 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates16).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS16 = Math.Round(valDA_PAYLOSS16.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_16 = sumDA_PAYLOSS16.ToString();
                var valPAYLOSS16 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates16).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS16 = Math.Round(valPAYLOSS16.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_16 = sumPAYLOSS16.ToString(); 
                var OneCount16 = INSTORE.DT_16 == "0" ? 0 : Math.Round((sumDA_PAYLOSS16 + sumPiceGross16 + sumPAYLOSS16) / Convert.ToDouble(INSTORE.DT_16), 2);
                dA_VIEW5.DT_16 = OneCount16.ToString();
                dA_VIEW6.DT_16 = (sumDA_PAYLOSS16 + sumPiceGross16 + sumPAYLOSS16).ToString();

                var Dates17 = dATE1 + "-17";
                var valGross17 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates17);
                var sumGross17 = Math.Round(valGross17.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_17 = sumGross17.ToString();
                var sumPiceGross17 = Math.Round(valGross17.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_17 = sumPiceGross17.ToString();
                var valDA_PAYLOSS17 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates17).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS17 = Math.Round(valDA_PAYLOSS17.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_17 = sumDA_PAYLOSS17.ToString();
                var valPAYLOSS17 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates17).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS17 = Math.Round(valPAYLOSS17.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_17 = sumPAYLOSS17.ToString(); 
                var OneCount17 = INSTORE.DT_17 == "0" ? 0 : Math.Round((sumDA_PAYLOSS17 + sumPiceGross17 + sumPAYLOSS17) / Convert.ToDouble(INSTORE.DT_17), 2);
                dA_VIEW5.DT_17 = OneCount17.ToString();
                dA_VIEW6.DT_17 = (sumDA_PAYLOSS17 + sumPiceGross17 + sumPAYLOSS17).ToString();

                var Dates18 = dATE1 + "-18";
                var valGross18 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates18);
                var sumGross18 = Math.Round(valGross18.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_18 = sumGross18.ToString();
                var sumPiceGross18 = Math.Round(valGross18.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_18 = sumPiceGross18.ToString();
                var valDA_PAYLOSS18 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates18).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS18 = Math.Round(valDA_PAYLOSS18.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_18 = sumDA_PAYLOSS18.ToString();
                var valPAYLOSS18 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates18).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS18 = Math.Round(valPAYLOSS18.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_18 = sumPAYLOSS18.ToString(); 
                var OneCount18 = INSTORE.DT_18 == "0" ? 0 : Math.Round((sumDA_PAYLOSS18 + sumPiceGross18 + sumPAYLOSS18) / Convert.ToDouble(INSTORE.DT_18), 2);
                dA_VIEW5.DT_18 = OneCount18.ToString();
                dA_VIEW6.DT_18 = (sumDA_PAYLOSS18 + sumPiceGross18 + sumPAYLOSS18).ToString();

                var Dates19 = dATE1 + "-19";
                var valGross19 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates19);
                var sumGross19 = Math.Round(valGross19.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_19 = sumGross19.ToString();
                var sumPiceGross19 = Math.Round(valGross19.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_19 = sumPiceGross19.ToString();
                var valDA_PAYLOSS19 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates19).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS19 = Math.Round(valDA_PAYLOSS19.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_19 = sumDA_PAYLOSS19.ToString();
                var valPAYLOSS19 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates19).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS19 = Math.Round(valPAYLOSS19.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_19 = sumPAYLOSS19.ToString(); 
                var OneCount19 = INSTORE.DT_19 == "0" ? 0 : Math.Round((sumDA_PAYLOSS19 + sumPiceGross19 + sumPAYLOSS19) / Convert.ToDouble(INSTORE.DT_19), 2);
                dA_VIEW5.DT_19 = OneCount19.ToString();
                dA_VIEW6.DT_19 = (sumDA_PAYLOSS19 + sumPiceGross19 + sumPAYLOSS19).ToString();

                var Dates20 = dATE1 + "-20";
                var valGross20 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates20);
                var sumGross20 = Math.Round(valGross20.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_20 = sumGross20.ToString();
                var sumPiceGross20 = Math.Round(valGross20.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_20 = sumPiceGross20.ToString();
                var valDA_PAYLOSS20 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates20).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS20 = Math.Round(valDA_PAYLOSS20.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_20 = sumDA_PAYLOSS20.ToString();
                var valPAYLOSS20 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates20).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS20 = Math.Round(valPAYLOSS20.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_20 = sumPAYLOSS20.ToString(); 
                var OneCount20 = INSTORE.DT_20 == "0" ? 0 : Math.Round((sumDA_PAYLOSS20 + sumPiceGross20 + sumPAYLOSS20) / Convert.ToDouble(INSTORE.DT_20), 2);
                dA_VIEW5.DT_20 = OneCount20.ToString();
                dA_VIEW6.DT_20 = (sumDA_PAYLOSS20 + sumPiceGross20 + sumPAYLOSS20).ToString();

                var Dates21 = dATE1 + "-21";
                var valGross21 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates21);
                var sumGross21 = Math.Round(valGross21.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_21 = sumGross21.ToString();
                var sumPiceGross21 = Math.Round(valGross21.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_21 = sumPiceGross21.ToString();
                var valDA_PAYLOSS21 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates21).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS21 = Math.Round(valDA_PAYLOSS21.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_21 = sumDA_PAYLOSS21.ToString();
                var valPAYLOSS21 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates21).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS21 = Math.Round(valPAYLOSS21.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_21 = sumPAYLOSS21.ToString(); 
                var OneCount21 = INSTORE.DT_21 == "0" ? 0 : Math.Round((sumDA_PAYLOSS21 + sumPiceGross21 + sumPAYLOSS21) / Convert.ToDouble(INSTORE.DT_21), 2);
                dA_VIEW5.DT_21 = OneCount21.ToString();
                dA_VIEW6.DT_21 = (sumDA_PAYLOSS21 + sumPiceGross21 + sumPAYLOSS21).ToString();

                var Dates22 = dATE1 + "-22";
                var valGross22 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates22);
                var sumGross22 = Math.Round(valGross22.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_22 = sumGross22.ToString();
                var sumPiceGross22 = Math.Round(valGross22.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_22 = sumPiceGross22.ToString();
                var valDA_PAYLOSS22 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates22).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS22 = Math.Round(valDA_PAYLOSS22.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_22 = sumDA_PAYLOSS22.ToString();
                var valPAYLOSS22 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates22).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS22 = Math.Round(valPAYLOSS22.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_22 = sumPAYLOSS22.ToString(); 
                var OneCount22 = INSTORE.DT_22 == "0" ? 0 : Math.Round((sumDA_PAYLOSS22 + sumPiceGross22 + sumPAYLOSS22) / Convert.ToDouble(INSTORE.DT_22), 2);
                dA_VIEW5.DT_22 = OneCount22.ToString();
                dA_VIEW6.DT_22 = (sumDA_PAYLOSS22 + sumPiceGross22 + sumPAYLOSS22).ToString();

                var Dates23 = dATE1 + "-23";
                var valGross23 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates23);
                var sumGross23 = Math.Round(valGross23.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_23 = sumGross23.ToString();
                var sumPiceGross23 = Math.Round(valGross23.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_23 = sumPiceGross23.ToString();
                var valDA_PAYLOSS23 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates23).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS23 = Math.Round(valDA_PAYLOSS23.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_23 = sumDA_PAYLOSS23.ToString();
                var valPAYLOSS23 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates23).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS23 = Math.Round(valPAYLOSS23.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_23 = sumPAYLOSS23.ToString(); 
                var OneCount23 = INSTORE.DT_23== "0" ? 0 : Math.Round((sumDA_PAYLOSS23 + sumPiceGross23 + sumPAYLOSS23) / Convert.ToDouble(INSTORE.DT_23), 2);
                dA_VIEW5.DT_23 = OneCount23.ToString();
                dA_VIEW6.DT_23 = (sumDA_PAYLOSS23 + sumPiceGross23 + sumPAYLOSS23).ToString();

                var Dates24 = dATE1 + "-24";
                var valGross24 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates24);
                var sumGross24 = Math.Round(valGross24.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_24 = sumGross24.ToString();
                var sumPiceGross24 = Math.Round(valGross24.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_24 = sumPiceGross24.ToString();
                var valDA_PAYLOSS24 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates24).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS24 = Math.Round(valDA_PAYLOSS24.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_24 = sumDA_PAYLOSS24.ToString();
                var valPAYLOSS24 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates24).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS24 = Math.Round(valPAYLOSS24.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_24 = sumPAYLOSS24.ToString(); 
                var OneCount24 = INSTORE.DT_24 == "0" ? 0 : Math.Round((sumDA_PAYLOSS24 + sumPiceGross24 + sumPAYLOSS24) / Convert.ToDouble(INSTORE.DT_24), 2);
                dA_VIEW5.DT_24 = OneCount24.ToString();
                dA_VIEW6.DT_24 = (sumDA_PAYLOSS24 + sumPiceGross24 + sumPAYLOSS24).ToString();

                var Dates25 = dATE1 + "-25";
                var valGross25 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates25);
                var sumGross25 = Math.Round(valGross25.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_25 = sumGross25.ToString();
                var sumPiceGross25 = Math.Round(valGross25.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_25 = sumPiceGross25.ToString();
                var valDA_PAYLOSS25 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates25).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS25 = Math.Round(valDA_PAYLOSS25.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_25 = sumDA_PAYLOSS25.ToString();
                var valPAYLOSS25 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates25).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS25 = Math.Round(valPAYLOSS25.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_25 = sumPAYLOSS25.ToString(); 
                var OneCount25 = INSTORE.DT_25 == "0" ? 0 : Math.Round((sumDA_PAYLOSS25 + sumPiceGross25 + sumPAYLOSS25) / Convert.ToDouble(INSTORE.DT_25), 2);
                dA_VIEW5.DT_25 = OneCount25.ToString();
                dA_VIEW6.DT_25 = (sumDA_PAYLOSS25 + sumPiceGross25 + sumPAYLOSS25).ToString();

                var Dates26 = dATE1 + "-26";
                var valGross26 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates26);
                var sumGross26 = Math.Round(valGross26.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_26 = sumGross26.ToString();
                var sumPiceGross26 = Math.Round(valGross26.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_26 = sumPiceGross26.ToString();
                var valDA_PAYLOSS26 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates26).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS26 = Math.Round(valDA_PAYLOSS26.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_26 = sumDA_PAYLOSS26.ToString();
                var valPAYLOSS26 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates26).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS26 = Math.Round(valPAYLOSS26.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_26 = sumPAYLOSS26.ToString();
                var OneCount26 = INSTORE.DT_26 == "0" ? 0 : Math.Round((sumDA_PAYLOSS26 + sumPiceGross26 + sumPAYLOSS26) / Convert.ToDouble(INSTORE.DT_26), 2);
                dA_VIEW5.DT_26 = OneCount26.ToString();
                dA_VIEW6.DT_26 = (sumDA_PAYLOSS26 + sumPiceGross26 + sumPAYLOSS26).ToString();

                var Dates27 = dATE1 + "-27";
                var valGross27 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates27);
                var sumGross27 = Math.Round(valGross27.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_27 = sumGross27.ToString();
                var sumPiceGross27 = Math.Round(valGross27.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_27 = sumPiceGross27.ToString();
                var valDA_PAYLOSS27 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates27).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS27 = Math.Round(valDA_PAYLOSS27.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_27 = sumDA_PAYLOSS27.ToString();
                var valPAYLOSS27 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates27).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS27 = Math.Round(valPAYLOSS27.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_27 = sumPAYLOSS27.ToString(); 
                var OneCount27 = INSTORE.DT_27 == "0" ? 0 : Math.Round((sumDA_PAYLOSS27 + sumPiceGross27 + sumPAYLOSS27) / Convert.ToDouble(INSTORE.DT_27), 2);
                dA_VIEW5.DT_27 = OneCount27.ToString();
                dA_VIEW6.DT_27 = (sumDA_PAYLOSS27 + sumPiceGross27 + sumPAYLOSS27).ToString();

                var Dates28 = dATE1 + "-28";
                var valGross28 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates28);
                var sumGross28 = Math.Round(valGross28.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_28 = sumGross28.ToString();
                var sumPiceGross28 = Math.Round(valGross28.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_28 = sumPiceGross28.ToString();
                var valDA_PAYLOSS28 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates28).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS28 = Math.Round(valDA_PAYLOSS28.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_28 = sumDA_PAYLOSS28.ToString();
                var valPAYLOSS28 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates28).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS28 = Math.Round(valPAYLOSS28.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_28 = sumPAYLOSS28.ToString(); 
                var OneCount28 = INSTORE.DT_28 == "0" ? 0 : Math.Round((sumDA_PAYLOSS28 + sumPiceGross28 + sumPAYLOSS28) / Convert.ToDouble(INSTORE.DT_28), 2);
                dA_VIEW5.DT_28 = OneCount28.ToString();
                dA_VIEW6.DT_28 = (sumDA_PAYLOSS28 + sumPiceGross28 + sumPAYLOSS28).ToString();

                var Dates29 = dATE1 + "-29";
                var valGross29 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates29);
                var sumGross29 = Math.Round(valGross29.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_29 = sumGross29.ToString();
                var sumPiceGross29 = Math.Round(valGross29.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_29 = sumPiceGross29.ToString();
                var valDA_PAYLOSS29 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates29).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS29 = Math.Round(valDA_PAYLOSS29.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_29 = sumDA_PAYLOSS29.ToString();
                var valPAYLOSS29 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates29).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS29 = Math.Round(valPAYLOSS29.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_29 = sumPAYLOSS29.ToString(); 
                var OneCount29 = INSTORE.DT_29 == "0" ? 0 : Math.Round((sumDA_PAYLOSS29 + sumPiceGross29 + sumPAYLOSS29) / Convert.ToDouble(INSTORE.DT_29), 2);
                dA_VIEW5.DT_29 = OneCount29.ToString();
                dA_VIEW6.DT_29 = (sumDA_PAYLOSS29 + sumPiceGross29 + sumPAYLOSS29).ToString();

                var Dates30 = dATE1 + "-30";
                var valGross30 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates30);
                var sumGross30 = Math.Round(valGross30.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_30 = sumGross30.ToString();
                var sumPiceGross30 = Math.Round(valGross30.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_30 = sumPiceGross30.ToString();
                var valDA_PAYLOSS30 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates30).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS30 = Math.Round(valDA_PAYLOSS30.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_30 = sumDA_PAYLOSS30.ToString();
                var valPAYLOSS30 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates30).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS30 = Math.Round(valPAYLOSS30.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_30 = sumPAYLOSS30.ToString(); 
                var OneCount30 = INSTORE.DT_30== "0" ? 0 : Math.Round((sumDA_PAYLOSS30 + sumPiceGross30 + sumPAYLOSS30) / Convert.ToDouble(INSTORE.DT_30), 2);
                dA_VIEW5.DT_30 = OneCount30.ToString();
                dA_VIEW6.DT_30 = (sumDA_PAYLOSS30 + sumPiceGross30 + sumPAYLOSS30).ToString();

                var Dates31 = dATE1 + "-31";
                var valGross31 = _sqlDbContext.DA_PAYPERSONs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates31);
                var sumGross31 = Math.Round(valGross31.Sum(x => x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)), 2);
                dA_VIEW1.DT_31 = sumGross31.ToString();
                var sumPiceGross31 = Math.Round(valGross31.Sum(x => (x.DT_INDIRECTWORKTIME == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTWORKTIME)) * (x.DT_DIRECTHOUR == "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_INDIRECTMOUTH == "" ? 0 : Convert.ToDouble(x.DT_INDIRECTMOUTH))), 2);
                dA_VIEW2.DT_31 = sumPiceGross31.ToString();
                var valDA_PAYLOSS31 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates31).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS31 = Math.Round(valDA_PAYLOSS31.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW3.DT_31 = sumDA_PAYLOSS31.ToString();
                var valPAYLOSS31 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates31).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS31 = Math.Round(valPAYLOSS31.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE))), 2);
                dA_VIEW4.DT_31 = sumPAYLOSS31.ToString(); 
                var OneCount31 = INSTORE.DT_31 == "0" ? 0 : Math.Round((sumDA_PAYLOSS31 + sumPiceGross31 + sumPAYLOSS31) / Convert.ToDouble(INSTORE.DT_31), 2);
                dA_VIEW5.DT_31 = OneCount31.ToString();
                dA_VIEW6.DT_31 = (sumDA_PAYLOSS31 + sumPiceGross31 + sumPAYLOSS31).ToString();

                dA_VIEW1.TOTAL = (sumGross1 + sumGross2 + sumGross3 + sumGross4 + sumGross5 + sumGross6 + sumGross7 + sumGross8 + sumGross9 + sumGross10 + sumGross11 + sumGross12 + sumGross13 + sumGross14 + sumGross15 + sumGross16 + sumGross17 + sumGross18 + sumGross19 + sumGross20 + sumGross21 + sumGross22 + sumGross23 + sumGross24 + sumGross25 + sumGross26 + sumGross27 + sumGross28 + sumGross29 + sumGross30 + sumGross31).ToString();
                dA_VIEW2.TOTAL = (sumPiceGross1 + sumPiceGross2 + sumPiceGross3 + sumPiceGross4 + sumPiceGross5 + sumPiceGross6 + sumPiceGross7 + sumPiceGross8 + sumPiceGross9 + sumPiceGross10 + sumPiceGross11 + sumPiceGross12 + sumPiceGross13 + sumPiceGross14 + sumPiceGross15 + sumPiceGross16 + sumPiceGross17 + sumPiceGross18 + sumPiceGross19 + sumPiceGross20 + sumPiceGross21 + sumPiceGross22 + sumPiceGross23 + sumPiceGross24 + sumPiceGross25 + sumPiceGross26 + sumPiceGross27 + sumPiceGross28 + sumPiceGross29 + sumPiceGross30 + sumPiceGross31).ToString();
                dA_VIEW3.TOTAL = (sumDA_PAYLOSS1 + sumDA_PAYLOSS2 + sumDA_PAYLOSS3 + sumDA_PAYLOSS4 + sumDA_PAYLOSS5 + sumDA_PAYLOSS6 + sumDA_PAYLOSS7 + sumDA_PAYLOSS8 + sumDA_PAYLOSS9 + sumDA_PAYLOSS10 + sumDA_PAYLOSS11 + sumDA_PAYLOSS12 + sumDA_PAYLOSS13 + sumDA_PAYLOSS14 + sumDA_PAYLOSS15 + sumDA_PAYLOSS16 + sumDA_PAYLOSS17 + sumDA_PAYLOSS18 + sumDA_PAYLOSS19 + sumDA_PAYLOSS20 + sumDA_PAYLOSS21 + sumDA_PAYLOSS22 + sumDA_PAYLOSS23 + sumDA_PAYLOSS24 + sumDA_PAYLOSS25 + sumDA_PAYLOSS26 + sumDA_PAYLOSS27 + sumDA_PAYLOSS28 + sumDA_PAYLOSS29 + sumDA_PAYLOSS30 + sumDA_PAYLOSS31).ToString();
                dA_VIEW4.TOTAL = (sumPAYLOSS1 + sumPAYLOSS2 + sumPAYLOSS3 + sumPAYLOSS4 + sumPAYLOSS5 + sumPAYLOSS6 + sumPAYLOSS7 + sumPAYLOSS8 + sumPAYLOSS9 + sumPAYLOSS10 + sumPAYLOSS11 + sumPAYLOSS12 + sumPAYLOSS13 + sumPAYLOSS14 + sumPAYLOSS15 + sumPAYLOSS16 + sumPAYLOSS17 + sumPAYLOSS18 + sumPAYLOSS19 + sumPAYLOSS20 + sumPAYLOSS21 + sumPAYLOSS22 + sumPAYLOSS23 + sumPAYLOSS24 + sumPAYLOSS25 + sumPAYLOSS26 + sumPAYLOSS27 + sumPAYLOSS28 + sumPAYLOSS29 + sumPAYLOSS30 + sumPAYLOSS31).ToString();
                dA_VIEW5.TOTAL = Math.Round(OneCount1 + OneCount2 + OneCount3 + OneCount4 + OneCount5 + OneCount6 + OneCount7 + OneCount8 + OneCount9 + OneCount10 + OneCount11 + OneCount12 + OneCount13 + OneCount14 + OneCount15 + OneCount16 + OneCount17 + OneCount18 + OneCount19 + OneCount20 + OneCount21 + OneCount22 + OneCount23 + OneCount24 + OneCount25 + OneCount26 + OneCount27 + OneCount28 + OneCount29 + OneCount30 + OneCount31, 2).ToString();
                dA_VIEW6.TOTAL = Math.Round(Convert.ToDouble(dA_VIEW2.TOTAL) + Convert.ToDouble(dA_VIEW3.TOTAL) + Convert.ToDouble(dA_VIEW4.TOTAL), 2).ToString();
                dA_VIEWs.Add(dA_VIEW1);
                dA_VIEWs.Add(dA_VIEW2);
                dA_VIEWs.Add(dA_VIEW3);
                dA_VIEWs.Add(dA_VIEW4);
                dA_VIEWs.Add(dA_VIEW5);
                dA_VIEWs1.Add(dA_VIEW6);
            }
            #region
            DA_VIEW dA_VIEW11 = new DA_VIEW();//间接人员费用(平摊):
            DA_VIEW dA_VIEW12 = new DA_VIEW();//公摊费用支出合计:
            DA_VIEW dA_VIEW13 = new DA_VIEW();//平台部门费用支出合计: 
            dA_VIEW11.TYPES = "间接人员费用(平摊):";
            dA_VIEW12.TYPES = "公摊费用支出合计:";
            dA_VIEW13.TYPES = "平台部门费用支出合计:";
            dA_VIEW11.TYPES1 = "";
            dA_VIEW12.TYPES1 = "";
            dA_VIEW13.TYPES1 = "";
            dA_VIEW11.MODEL = "";
            dA_VIEW12.MODEL = "";
            dA_VIEW13.MODEL = "";
            DateTime date1 = Convert.ToDateTime(dATE1);
            int pYear = date1.Year;
            int pMonth = date1.Month;
            int vMax = DateTime.DaysInMonth(pYear, pMonth);
            var paymonth = _sqlDbContext.DA_PAYMONTHs.Where(x => x.DT_DATE.Value.ToString("yyyy-MM") == dATE1);
            var person = paymonth.Where(x => x.DT_NAME == "间接人员月薪");
            double personTal = person.Sum(x => x.DT_EXPEND == "" ? 0 : Convert.ToDouble(x.DT_EXPEND));
            var gongtang = paymonth.Where(x => x.DT_NAME == "公摊费用");
            double gongtangTal = gongtang.Sum(x => x.DT_EXPEND == "" ? 0 : Convert.ToDouble(x.DT_EXPEND));
            var pingtai = paymonth.Where(x => x.DT_NAME == "平台部门费用");
            double pingtaiTal = pingtai.Sum(x => x.DT_EXPEND == "" ? 0 : Convert.ToDouble(x.DT_EXPEND));
            DateTime firstDayOfThisMonth = new DateTime(pYear, pMonth, 1);
            DateTime lastDayOfThisMonth = new DateTime(pYear, pMonth, DateTime.DaysInMonth(pYear, pMonth));
            int TotalWeeks1 = TotalWeeks(firstDayOfThisMonth, lastDayOfThisMonth, DayOfWeek.Sunday);
            int OnWorkDay = vMax;// - TotalWeeks1;
            double Ctperson = Math.Round(personTal / OnWorkDay, 2);
            double Ctgongtang = Math.Round(gongtangTal / OnWorkDay, 2);
            double Ctpingtai = Math.Round(pingtaiTal / OnWorkDay, 2);
            var Tdate1 = Convert.ToDateTime(dATE1 + "-01");
            switch (Tdate1.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_1 = Ctperson.ToString();
                    dA_VIEW12.DT_1 = Ctgongtang.ToString();
                    dA_VIEW13.DT_1 = Ctpingtai.ToString();
                    break;
            }
            var Tdate2 = Convert.ToDateTime(dATE1 + "-02");
            switch (Tdate2.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_2 = Ctperson.ToString();
                    dA_VIEW12.DT_2 = Ctgongtang.ToString();
                    dA_VIEW13.DT_2 = Ctpingtai.ToString();
                    break;
            }
            var Tdate3 = Convert.ToDateTime(dATE1 + "-03");
            switch (Tdate3.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_3 = Ctperson.ToString();
                    dA_VIEW12.DT_3 = Ctgongtang.ToString();
                    dA_VIEW13.DT_3 = Ctpingtai.ToString();
                    break;
            }
            var Tdate4 = Convert.ToDateTime(dATE1 + "-04");
            switch (Tdate4.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_4 = Ctperson.ToString();
                    dA_VIEW12.DT_4 = Ctgongtang.ToString();
                    dA_VIEW13.DT_4 = Ctpingtai.ToString();
                    break;
            }
            var Tdate5 = Convert.ToDateTime(dATE1 + "-05");
            switch (Tdate5.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_5 = Ctperson.ToString();
                    dA_VIEW12.DT_5 = Ctgongtang.ToString();
                    dA_VIEW13.DT_5 = Ctpingtai.ToString();
                    break;
            }
            var Tdate6 = Convert.ToDateTime(dATE1 + "-06");
            switch (Tdate6.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_6 = Ctperson.ToString();
                    dA_VIEW12.DT_6 = Ctgongtang.ToString();
                    dA_VIEW13.DT_6 = Ctpingtai.ToString();
                    break;
            }
            var Tdate7 = Convert.ToDateTime(dATE1 + "-07");
            switch (Tdate7.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_7 = Ctperson.ToString();
                    dA_VIEW12.DT_7 = Ctgongtang.ToString();
                    dA_VIEW13.DT_7 = Ctpingtai.ToString();
                    break;
            }
            var Tdate8 = Convert.ToDateTime(dATE1 + "-08");
            switch (Tdate8.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_8 = Ctperson.ToString();
                    dA_VIEW12.DT_8 = Ctgongtang.ToString();
                    dA_VIEW13.DT_8 = Ctpingtai.ToString();
                    break;
            }
            var Tdate9 = Convert.ToDateTime(dATE1 + "-09");
            switch (Tdate9.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_9 = Ctperson.ToString();
                    dA_VIEW12.DT_9 = Ctgongtang.ToString();
                    dA_VIEW13.DT_9 = Ctpingtai.ToString();
                    break;
            }
            var Tdate10 = Convert.ToDateTime(dATE1 + "-10");
            switch (Tdate10.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_10 = Ctperson.ToString();
                    dA_VIEW12.DT_10 = Ctgongtang.ToString();
                    dA_VIEW13.DT_10 = Ctpingtai.ToString();
                    break;
            }
            var Tdate11 = Convert.ToDateTime(dATE1 + "-11");
            switch (Tdate11.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_11 = Ctperson.ToString();
                    dA_VIEW12.DT_11 = Ctgongtang.ToString();
                    dA_VIEW13.DT_11 = Ctpingtai.ToString();
                    break;
            }
            var Tdate12 = Convert.ToDateTime(dATE1 + "-12");
            switch (Tdate12.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_12 = Ctperson.ToString();
                    dA_VIEW12.DT_12 = Ctgongtang.ToString();
                    dA_VIEW13.DT_12 = Ctpingtai.ToString();
                    break;
            }
            var Tdate13 = Convert.ToDateTime(dATE1 + "-13");
            switch (Tdate13.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_13 = Ctperson.ToString();
                    dA_VIEW12.DT_13 = Ctgongtang.ToString();
                    dA_VIEW13.DT_13 = Ctpingtai.ToString();
                    break;
            }
            var Tdate14 = Convert.ToDateTime(dATE1 + "-14");
            switch (Tdate14.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_14 = Ctperson.ToString();
                    dA_VIEW12.DT_14 = Ctgongtang.ToString();
                    dA_VIEW13.DT_14 = Ctpingtai.ToString();
                    break;
            }
            var Tdate15 = Convert.ToDateTime(dATE1 + "-15");
            switch (Tdate15.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_15 = Ctperson.ToString();
                    dA_VIEW12.DT_15 = Ctgongtang.ToString();
                    dA_VIEW13.DT_15 = Ctpingtai.ToString();
                    break;
            }
            var Tdate16 = Convert.ToDateTime(dATE1 + "-16");
            switch (Tdate16.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_16 = Ctperson.ToString();
                    dA_VIEW12.DT_16 = Ctgongtang.ToString();
                    dA_VIEW13.DT_16 = Ctpingtai.ToString();
                    break;
            }
            var Tdate17 = Convert.ToDateTime(dATE1 + "-17");
            switch (Tdate17.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_17 = Ctperson.ToString();
                    dA_VIEW12.DT_17 = Ctgongtang.ToString();
                    dA_VIEW13.DT_17 = Ctpingtai.ToString();
                    break;
            }
            var Tdate18 = Convert.ToDateTime(dATE1 + "-18");
            switch (Tdate18.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_18 = Ctperson.ToString();
                    dA_VIEW12.DT_18 = Ctgongtang.ToString();
                    dA_VIEW13.DT_18 = Ctpingtai.ToString();
                    break;
            }
            var Tdate19 = Convert.ToDateTime(dATE1 + "-19");
            switch (Tdate19.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_19 = Ctperson.ToString();
                    dA_VIEW12.DT_19 = Ctgongtang.ToString();
                    dA_VIEW13.DT_19 = Ctpingtai.ToString();
                    break;
            }
            var Tdate20 = Convert.ToDateTime(dATE1 + "-20");
            switch (Tdate20.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_20 = Ctperson.ToString();
                    dA_VIEW12.DT_20 = Ctgongtang.ToString();
                    dA_VIEW13.DT_20 = Ctpingtai.ToString();
                    break;
            }
            var Tdate21 = Convert.ToDateTime(dATE1 + "-21");
            switch (Tdate21.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_21 = Ctperson.ToString();
                    dA_VIEW12.DT_21 = Ctgongtang.ToString();
                    dA_VIEW13.DT_21 = Ctpingtai.ToString();
                    break;
            }
            var Tdate22 = Convert.ToDateTime(dATE1 + "-22");
            switch (Tdate22.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_22 = Ctperson.ToString();
                    dA_VIEW12.DT_22 = Ctgongtang.ToString();
                    dA_VIEW13.DT_22 = Ctpingtai.ToString();
                    break;
            }
            var Tdate23 = Convert.ToDateTime(dATE1 + "-23");
            switch (Tdate23.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_23 = Ctperson.ToString();
                    dA_VIEW12.DT_23 = Ctgongtang.ToString();
                    dA_VIEW13.DT_23 = Ctpingtai.ToString();
                    break;
            }
            var Tdate24 = Convert.ToDateTime(dATE1 + "-24");
            switch (Tdate24.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_24 = Ctperson.ToString();
                    dA_VIEW12.DT_24 = Ctgongtang.ToString();
                    dA_VIEW13.DT_24 = Ctpingtai.ToString();
                    break;
            }
            var Tdate25 = Convert.ToDateTime(dATE1 + "-25");
            switch (Tdate25.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_25 = Ctperson.ToString();
                    dA_VIEW12.DT_25 = Ctgongtang.ToString();
                    dA_VIEW13.DT_25 = Ctpingtai.ToString();
                    break;
            }
            var Tdate26 = Convert.ToDateTime(dATE1 + "-26");
            switch (Tdate26.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_26 = Ctperson.ToString();
                    dA_VIEW12.DT_26 = Ctgongtang.ToString();
                    dA_VIEW13.DT_26 = Ctpingtai.ToString();
                    break;
            }
            var Tdate27 = Convert.ToDateTime(dATE1 + "-27");
            switch (Tdate27.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_27 = Ctperson.ToString();
                    dA_VIEW12.DT_27 = Ctgongtang.ToString();
                    dA_VIEW13.DT_27 = Ctpingtai.ToString();
                    break;
            }
            var Tdate28 = Convert.ToDateTime(dATE1 + "-28");
            switch (Tdate28.DayOfWeek)
            { 
                default:
                    dA_VIEW11.DT_28 = Ctperson.ToString();
                    dA_VIEW12.DT_28 = Ctgongtang.ToString();
                    dA_VIEW13.DT_28 = Ctpingtai.ToString();
                    break;
            }
            if (vMax >= 29)
            {
                var Tdate29 = Convert.ToDateTime(dATE1 + "-29");
                switch (Tdate29.DayOfWeek)
                { 
                    default:
                        dA_VIEW11.DT_29 = Ctperson.ToString();
                        dA_VIEW12.DT_29 = Ctgongtang.ToString();
                        dA_VIEW13.DT_29 = Ctpingtai.ToString();
                        break;
                }
            }
            else
            {
                dA_VIEW11.DT_29 = "0";
                dA_VIEW12.DT_29 = "0";
                dA_VIEW13.DT_29 = "0";
            }
            if (vMax >= 30)
            {
                var Tdate30 = Convert.ToDateTime(dATE1 + "-30");
                switch (Tdate30.DayOfWeek)
                { 
                    default:
                        dA_VIEW11.DT_30 = Ctperson.ToString();
                        dA_VIEW12.DT_30 = Ctgongtang.ToString();
                        dA_VIEW13.DT_30 = Ctpingtai.ToString();
                        break;
                }
            }
            else
            {
                dA_VIEW11.DT_30 = "0";
                dA_VIEW12.DT_30 = "0";
                dA_VIEW13.DT_30 = "0";
            }
            if (vMax >= 31)
            {
                var Tdate31 = Convert.ToDateTime(dATE1 + "-31");
                switch (Tdate31.DayOfWeek)
                { 
                    default:
                        dA_VIEW11.DT_31 = Ctperson.ToString();
                        dA_VIEW12.DT_31 = Ctgongtang.ToString();
                        dA_VIEW13.DT_31 = Ctpingtai.ToString();
                        break;
                }
            }
            else
            {
                dA_VIEW11.DT_31 = "0";
                dA_VIEW12.DT_31 = "0";
                dA_VIEW13.DT_31 = "0";
            }

            dA_VIEW11.TOTAL = personTal.ToString();
            dA_VIEW12.TOTAL = gongtangTal.ToString();
            dA_VIEW13.TOTAL = pingtaiTal.ToString();
            dA_VIEWs.Add(dA_VIEW11);
            dA_VIEWs.Add(dA_VIEW12);
            dA_VIEWs.Add(dA_VIEW13);
            #endregion
            foreach (var item in DA_2)
            {
                DA_VIEW dA_VIEW1 = new DA_VIEW();//直接作业人员 
                var CliemtName = item.NAME;
                dA_VIEW1.TYPES = "项目支出小计:";
                dA_VIEW1.TYPES1 = "";
                dA_VIEW1.MODEL = CliemtName;
                //SUM(E23: E25) + SUM(E$43:E$44) * 0.3 + E$42 * 0.25 
                var dtms = dA_VIEWs1.Where(x => x.MODEL == CliemtName).FirstOrDefault();
                var dtmos = dA_VIEWs2.Where(x => x.MODEL == CliemtName).FirstOrDefault();
                double Ct1 = 0;
                var Dates1 = dATE1 + "-01";
                var Tdates1 = Convert.ToDateTime(Dates1);
                Ct1 = Math.Round(dtms.DT_1 == "" ? 0 : Convert.ToDouble(dtms.DT_1) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
               
                dA_VIEW1.DT_1 = Ct1.ToString();

                double Ct2 = 0;
                Dates1 = dATE1 + "-02";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct2 = Math.Round(dtms.DT_2 == "" ? 0 : Convert.ToDouble(dtms.DT_2) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
             
                dA_VIEW1.DT_2 = Ct2.ToString();

                double Ct3 = 0;
                Dates1 = dATE1 + "-03";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct3 = Math.Round(dtms.DT_3 == "" ? 0 : Convert.ToDouble(dtms.DT_3) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
              
                dA_VIEW1.DT_3 = Ct3.ToString();

                double Ct4 = 0;
                Dates1 = dATE1 + "-04";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct4 = Math.Round(dtms.DT_4 == "" ? 0 : Convert.ToDouble(dtms.DT_4) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_4 = Ct4.ToString();

                double Ct5 = 0;
                Dates1 = dATE1 + "-05";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct5 = Math.Round(dtms.DT_5 == "" ? 0 : Convert.ToDouble(dtms.DT_5) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_5 = Ct5.ToString();
                double Ct6 = 0;
                Dates1 = dATE1 + "-06";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct6 = Math.Round(dtms.DT_6 == "" ? 0 : Convert.ToDouble(dtms.DT_6) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_6 = Ct6.ToString();

                double Ct7 = 0;
                Dates1 = dATE1 + "-07";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct7 = Math.Round(dtms.DT_7 == "" ? 0 : Convert.ToDouble(dtms.DT_7) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_7 = Ct7.ToString();

                double Ct8 = 0;
                Dates1 = dATE1 + "-08";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct8 = Math.Round(dtms.DT_8 == "" ? 0 : Convert.ToDouble(dtms.DT_8) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_8 = Ct8.ToString();

                double Ct9 = 0;
                Dates1 = dATE1 + "-09";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct9 = Math.Round(dtms.DT_9 == "" ? 0 : Convert.ToDouble(dtms.DT_9) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_9 = Ct9.ToString();

                double Ct10 = 0;
                Dates1 = dATE1 + "-10";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct10 = Math.Round(dtms.DT_10 == "" ? 0 : Convert.ToDouble(dtms.DT_10) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_10 = Ct10.ToString();

                double Ct11 = 0;
                Dates1 = dATE1 + "-11";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct11 = Math.Round(dtms.DT_11 == "" ? 0 : Convert.ToDouble(dtms.DT_11) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_11 = Ct11.ToString();

                double Ct12 = 0;
                Dates1 = dATE1 + "-12";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct12 = Math.Round(dtms.DT_12 == "" ? 0 : Convert.ToDouble(dtms.DT_12) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_12 = Ct12.ToString();

                double Ct13 = 0;
                Dates1 = dATE1 + "-13";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct13 = Math.Round(dtms.DT_13 == "" ? 0 : Convert.ToDouble(dtms.DT_13) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_13 = Ct13.ToString();

                double Ct14 = 0;
                Dates1 = dATE1 + "-14";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct14 = Math.Round(dtms.DT_14 == "" ? 0 : Convert.ToDouble(dtms.DT_14) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_14 = Ct14.ToString();

                double Ct15 = 0;
                Dates1 = dATE1 + "-15";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct15 = Math.Round(dtms.DT_15 == "" ? 0 : Convert.ToDouble(dtms.DT_15) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_15 = Ct15.ToString();

                double Ct16 = 0;
                Dates1 = dATE1 + "-16";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct16 = Math.Round(dtms.DT_16 == "" ? 0 : Convert.ToDouble(dtms.DT_16) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_16 = Ct16.ToString();

                double Ct17 = 0;
                Dates1 = dATE1 + "-17";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct17 = Math.Round(dtms.DT_17 == "" ? 0 : Convert.ToDouble(dtms.DT_17) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_17 = Ct17.ToString();

                double Ct18 = 0;
                Dates1 = dATE1 + "-18";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct18 = Math.Round(dtms.DT_18 == "" ? 0 : Convert.ToDouble(dtms.DT_18) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_18 = Ct18.ToString();

                double Ct19 = 0;
                Dates1 = dATE1 + "-19";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct19 = Math.Round(dtms.DT_19 == "" ? 0 : Convert.ToDouble(dtms.DT_19) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_19 = Ct19.ToString();

                double Ct20 = 0;
                Dates1 = dATE1 + "-20";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct20 = Math.Round(dtms.DT_20 == "" ? 0 : Convert.ToDouble(dtms.DT_20) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_20 = Ct20.ToString();

                double Ct21 = 0;
                Dates1 = dATE1 + "-21";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct21 = Math.Round(dtms.DT_21 == "" ? 0 : Convert.ToDouble(dtms.DT_21) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_21 = Ct21.ToString();

                double Ct22 = 0;
                Dates1 = dATE1 + "-22";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct22 = Math.Round(dtms.DT_22 == "" ? 0 : Convert.ToDouble(dtms.DT_22) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_22 = Ct22.ToString();

                double Ct23 = 0;
                Dates1 = dATE1 + "-23";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct23 = Math.Round(dtms.DT_23 == "" ? 0 : Convert.ToDouble(dtms.DT_23) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_23 = Ct23.ToString();

                double Ct24 = 0;
                Dates1 = dATE1 + "-24";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct24 = Math.Round(dtms.DT_24 == "" ? 0 : Convert.ToDouble(dtms.DT_24) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_24 = Ct24.ToString();

                double Ct25 = 0;
                Dates1 = dATE1 + "-25";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct25 = Math.Round(dtms.DT_25 == "" ? 0 : Convert.ToDouble(dtms.DT_25) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_25 = Ct25.ToString();

                double Ct26 = 0;
                Dates1 = dATE1 + "-26";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct26 = Math.Round(dtms.DT_26 == "" ? 0 : Convert.ToDouble(dtms.DT_26) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_26 = Ct26.ToString();

                double Ct27 = 0;
                Dates1 = dATE1 + "-27";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct27 = Math.Round(dtms.DT_27 == "" ? 0 : Convert.ToDouble(dtms.DT_27) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_27 = Ct27.ToString();

                double Ct28 = 0;
                Dates1 = dATE1 + "-28";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct28 = Math.Round(dtms.DT_28 == "" ? 0 : Convert.ToDouble(dtms.DT_28) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                dA_VIEW1.DT_28 = Ct28.ToString();

                double Ct29 = 0;
                Dates1 = dATE1 + "-29";
                if (vMax >= 29)
                {
                    Tdates1 = Convert.ToDateTime(Dates1);
                    Ct29 = Math.Round(dtms.DT_29 == "" ? 0 : Convert.ToDouble(dtms.DT_29) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                }
                dA_VIEW1.DT_29 = Ct29.ToString();

                double Ct30 = 0;
                Dates1 = dATE1 + "-30";
                if (vMax >= 30)
                {
                    Tdates1 = Convert.ToDateTime(Dates1);
                    Ct30 = Math.Round(dtms.DT_30 == "" ? 0 : Convert.ToDouble(dtms.DT_30) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                }
                dA_VIEW1.DT_30 = Ct30.ToString();

                double Ct31 = 0;
                Dates1 = dATE1 + "-31";
                if (vMax >= 31)
                {
                    Tdates1 = Convert.ToDateTime(Dates1);
                    Ct31 = Math.Round(dtms.DT_31 == "" ? 0 : Convert.ToDouble(dtms.DT_31) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2);
                }
                dA_VIEW1.DT_31 = Ct31.ToString();
                dA_VIEW1.TOTAL = Math.Round(Ct1 + Ct2 + Ct3 + Ct4 + Ct5 + Ct6 + Ct7 + Ct8 + Ct9 + Ct10 + Ct11 + Ct12 + Ct13 + Ct14 + Ct15 + Ct16 + Ct17 + Ct18 + Ct19 + Ct20 + Ct21 + Ct22 + Ct23 + Ct24 + Ct25 + Ct26 + Ct27 + Ct28 + Ct29 + Ct30 + Ct31, 2).ToString();
                dA_VIEWs.Add(dA_VIEW1);
                dA_VIEWs3.Add(dA_VIEW1);
                tO01 += Ct1;
                tO02 += Ct2;
                tO03 += Ct3;
                tO04 += Ct4;
                tO05 += Ct5;
                tO06 += Ct6;
                tO07 += Ct7;
                tO08 += Ct8;
                tO09 += Ct9;
                tO10 += Ct10;
                tO11 += Ct11;
                tO12 += Ct12;
                tO13 += Ct13;
                tO14 += Ct14;
                tO15 += Ct15;
                tO16 += Ct16;
                tO17 += Ct17;
                tO18 += Ct18;
                tO19 += Ct19;
                tO20 += Ct20;
                tO21 += Ct21;
                tO22 += Ct22;
                tO23 += Ct23;
                tO24 += Ct24;
                tO25 += Ct25;
                tO26 += Ct26;
                tO27 += Ct27;
                tO28 += Ct28;
                tO29 += Ct29;
                tO30 += Ct30;
                tO31 += Ct31;
                tOtal += Math.Round(dA_VIEW1.TOTAL == "" ? 0 : Convert.ToDouble(dA_VIEW1.TOTAL), 2);
            }
            DA_VIEW dA_VIEWTT1 = new DA_VIEW
            {
                TYPES = "生产部支出合计:",
                TYPES1 = "",
                MODEL = "",
                DT_1 = Math.Round(tO01, 2).ToString(),
                DT_2 = Math.Round(tO02, 2).ToString(),
                DT_3 = Math.Round(tO03, 2).ToString(),
                DT_4 = Math.Round(tO04, 2).ToString(),
                DT_5 = Math.Round(tO05, 2).ToString(),
                DT_6 = Math.Round(tO06, 2).ToString(),
                DT_7 = Math.Round(tO07, 2).ToString(),
                DT_8 = Math.Round(tO08, 2).ToString(),
                DT_9 = Math.Round(tO09, 2).ToString(),
                DT_10 = Math.Round(tO10, 2).ToString(),
                DT_11 = Math.Round(tO11, 2).ToString(),
                DT_12 = Math.Round(tO12, 2).ToString(),
                DT_13 = Math.Round(tO13, 2).ToString(),
                DT_14 = Math.Round(tO14, 2).ToString(),
                DT_15 = Math.Round(tO15, 2).ToString(),
                DT_16 = Math.Round(tO16, 2).ToString(),
                DT_17 = Math.Round(tO17, 2).ToString(),
                DT_18 = Math.Round(tO18, 2).ToString(),
                DT_19 = Math.Round(tO19, 2).ToString(),
                DT_20 = Math.Round(tO20, 2).ToString(),
                DT_21 = Math.Round(tO21, 2).ToString(),
                DT_22 = Math.Round(tO22, 2).ToString(),
                DT_23 = Math.Round(tO23, 2).ToString(),
                DT_24 = Math.Round(tO24, 2).ToString(),
                DT_25 = Math.Round(tO25, 2).ToString(),
                DT_26 = Math.Round(tO26, 2).ToString(),
                DT_27 = Math.Round(tO27, 2).ToString(),
                DT_28 = Math.Round(tO28, 2).ToString(),
                DT_29 = Math.Round(tO29, 2).ToString(),
                DT_30 = Math.Round(tO30, 2).ToString(),
                DT_31 = Math.Round(tO31, 2).ToString(),
                TOTAL = Math.Round(tOtal, 2).ToString()
            };//生产部支出合计
            dA_VIEWs.Add(dA_VIEWTT1);
            foreach (var item in DA_2)
            {
                //List<DA_VIEW> dA_VIEWs1 = new List<DA_VIEW>();//支出统计
                //List<DA_VIEW> dA_VIEWs2 = new List<DA_VIEW>();//收入统计
                DA_VIEW dA_VIEW1 = new DA_VIEW();//直接作业人员 
                var CliemtName = item.NAME;
                dA_VIEW1.TYPES = "盈亏(元)";
                dA_VIEW1.TYPES1 = "";
                dA_VIEW1.MODEL = CliemtName;
                //SUM(E23: E25) + SUM(E$43:E$44) * 0.3 + E$42 * 0.25 
                var dtms = dA_VIEWs3.Where(x => x.MODEL == CliemtName).FirstOrDefault();
                var dtmos = dA_VIEWs2.Where(x => x.TYPES == CliemtName).FirstOrDefault();
                dA_VIEW1.DT_1 = Math.Round((dtmos.DT_1 == "" ? 0 : Convert.ToDouble(dtmos.DT_1)) - (dtms.DT_1 == "" ? 0 : Convert.ToDouble(dtms.DT_1)), 2).ToString();
                dA_VIEW1.DT_2 = Math.Round((dtmos.DT_2 == "" ? 0 : Convert.ToDouble(dtmos.DT_2)) - (dtms.DT_2 == "" ? 0 : Convert.ToDouble(dtms.DT_2)), 2).ToString();
                dA_VIEW1.DT_3 = Math.Round((dtmos.DT_3 == "" ? 0 : Convert.ToDouble(dtmos.DT_3)) - (dtms.DT_3 == "" ? 0 : Convert.ToDouble(dtms.DT_3)), 2).ToString();
                dA_VIEW1.DT_4 = Math.Round((dtmos.DT_4 == "" ? 0 : Convert.ToDouble(dtmos.DT_4)) - (dtms.DT_4 == "" ? 0 : Convert.ToDouble(dtms.DT_4)), 2).ToString();
                dA_VIEW1.DT_5 = Math.Round((dtmos.DT_5 == "" ? 0 : Convert.ToDouble(dtmos.DT_5)) - (dtms.DT_5 == "" ? 0 : Convert.ToDouble(dtms.DT_5)), 2).ToString();
                dA_VIEW1.DT_6 = Math.Round((dtmos.DT_6 == "" ? 0 : Convert.ToDouble(dtmos.DT_6)) - (dtms.DT_6 == "" ? 0 : Convert.ToDouble(dtms.DT_6)), 2).ToString();
                dA_VIEW1.DT_7 = Math.Round((dtmos.DT_7 == "" ? 0 : Convert.ToDouble(dtmos.DT_7)) - (dtms.DT_7 == "" ? 0 : Convert.ToDouble(dtms.DT_7)), 2).ToString();
                dA_VIEW1.DT_8 = Math.Round((dtmos.DT_8 == "" ? 0 : Convert.ToDouble(dtmos.DT_8)) - (dtms.DT_8 == "" ? 0 : Convert.ToDouble(dtms.DT_8)), 2).ToString();
                dA_VIEW1.DT_9 = Math.Round((dtmos.DT_9 == "" ? 0 : Convert.ToDouble(dtmos.DT_9)) - (dtms.DT_9 == "" ? 0 : Convert.ToDouble(dtms.DT_9)), 2).ToString();
                dA_VIEW1.DT_10 = Math.Round((dtmos.DT_10 == "" ? 0 : Convert.ToDouble(dtmos.DT_10)) - (dtms.DT_10 == "" ? 0 : Convert.ToDouble(dtms.DT_10)), 2).ToString();
                dA_VIEW1.DT_11 = Math.Round((dtmos.DT_11 == "" ? 0 : Convert.ToDouble(dtmos.DT_11)) - (dtms.DT_11 == "" ? 0 : Convert.ToDouble(dtms.DT_11)), 2).ToString();
                dA_VIEW1.DT_12 = Math.Round((dtmos.DT_12 == "" ? 0 : Convert.ToDouble(dtmos.DT_12)) - (dtms.DT_12 == "" ? 0 : Convert.ToDouble(dtms.DT_12)), 2).ToString();
                dA_VIEW1.DT_13 = Math.Round((dtmos.DT_13 == "" ? 0 : Convert.ToDouble(dtmos.DT_13)) - (dtms.DT_13 == "" ? 0 : Convert.ToDouble(dtms.DT_13)), 2).ToString();
                dA_VIEW1.DT_14 = Math.Round((dtmos.DT_14 == "" ? 0 : Convert.ToDouble(dtmos.DT_14)) - (dtms.DT_14 == "" ? 0 : Convert.ToDouble(dtms.DT_14)), 2).ToString();
                dA_VIEW1.DT_15 = Math.Round((dtmos.DT_15 == "" ? 0 : Convert.ToDouble(dtmos.DT_15)) - (dtms.DT_15 == "" ? 0 : Convert.ToDouble(dtms.DT_15)), 2).ToString();
                dA_VIEW1.DT_16 = Math.Round((dtmos.DT_16 == "" ? 0 : Convert.ToDouble(dtmos.DT_16)) - (dtms.DT_16 == "" ? 0 : Convert.ToDouble(dtms.DT_16)), 2).ToString();
                dA_VIEW1.DT_17 = Math.Round((dtmos.DT_17 == "" ? 0 : Convert.ToDouble(dtmos.DT_17)) - (dtms.DT_17 == "" ? 0 : Convert.ToDouble(dtms.DT_17)), 2).ToString();
                dA_VIEW1.DT_18 = Math.Round((dtmos.DT_18 == "" ? 0 : Convert.ToDouble(dtmos.DT_18)) - (dtms.DT_18 == "" ? 0 : Convert.ToDouble(dtms.DT_18)), 2).ToString();
                dA_VIEW1.DT_19 = Math.Round((dtmos.DT_19 == "" ? 0 : Convert.ToDouble(dtmos.DT_19)) - (dtms.DT_19 == "" ? 0 : Convert.ToDouble(dtms.DT_19)), 2).ToString();
                dA_VIEW1.DT_20 = Math.Round((dtmos.DT_20 == "" ? 0 : Convert.ToDouble(dtmos.DT_20)) - (dtms.DT_20 == "" ? 0 : Convert.ToDouble(dtms.DT_20)), 2).ToString();
                dA_VIEW1.DT_21 = Math.Round((dtmos.DT_21 == "" ? 0 : Convert.ToDouble(dtmos.DT_21)) - (dtms.DT_21 == "" ? 0 : Convert.ToDouble(dtms.DT_21)), 2).ToString();
                dA_VIEW1.DT_22 = Math.Round((dtmos.DT_22 == "" ? 0 : Convert.ToDouble(dtmos.DT_22)) - (dtms.DT_22 == "" ? 0 : Convert.ToDouble(dtms.DT_22)), 2).ToString();
                dA_VIEW1.DT_23 = Math.Round((dtmos.DT_23 == "" ? 0 : Convert.ToDouble(dtmos.DT_23)) - (dtms.DT_23 == "" ? 0 : Convert.ToDouble(dtms.DT_23)), 2).ToString();
                dA_VIEW1.DT_24 = Math.Round((dtmos.DT_24 == "" ? 0 : Convert.ToDouble(dtmos.DT_24)) - (dtms.DT_24 == "" ? 0 : Convert.ToDouble(dtms.DT_24)), 2).ToString();
                dA_VIEW1.DT_25 = Math.Round((dtmos.DT_25 == "" ? 0 : Convert.ToDouble(dtmos.DT_25)) - (dtms.DT_25 == "" ? 0 : Convert.ToDouble(dtms.DT_25)), 2).ToString();
                dA_VIEW1.DT_26 = Math.Round((dtmos.DT_26 == "" ? 0 : Convert.ToDouble(dtmos.DT_26)) - (dtms.DT_26 == "" ? 0 : Convert.ToDouble(dtms.DT_26)), 2).ToString();
                dA_VIEW1.DT_27 = Math.Round((dtmos.DT_27 == "" ? 0 : Convert.ToDouble(dtmos.DT_27)) - (dtms.DT_27 == "" ? 0 : Convert.ToDouble(dtms.DT_27)), 2).ToString();
                dA_VIEW1.DT_28 = Math.Round((dtmos.DT_28 == "" ? 0 : Convert.ToDouble(dtmos.DT_28)) - (dtms.DT_28 == "" ? 0 : Convert.ToDouble(dtms.DT_28)), 2).ToString();
                dA_VIEW1.DT_29 = Math.Round((dtmos.DT_29 == "" ? 0 : Convert.ToDouble(dtmos.DT_29)) - (dtms.DT_29 == "" ? 0 : Convert.ToDouble(dtms.DT_29)), 2).ToString();
                dA_VIEW1.DT_30 = Math.Round((dtmos.DT_30 == "" ? 0 : Convert.ToDouble(dtmos.DT_30)) - (dtms.DT_30 == "" ? 0 : Convert.ToDouble(dtms.DT_30)), 2).ToString();
                dA_VIEW1.DT_31 = Math.Round((dtmos.DT_31 == "" ? 0 : Convert.ToDouble(dtmos.DT_31)) - (dtms.DT_31 == "" ? 0 : Convert.ToDouble(dtms.DT_31)), 2).ToString();
                dA_VIEW1.TOTAL = Math.Round((dtmos.TOTAL == "" ? 0 : Convert.ToDouble(dtmos.TOTAL)) - (dtms.TOTAL == "" ? 0 : Convert.ToDouble(dtms.TOTAL)), 2).ToString();
                dA_VIEWs.Add(dA_VIEW1);
            };
            //生产部盈亏（元）：
            DA_VIEW dA_VIEWTT2 = new DA_VIEW
            {
                TYPES = "生产部盈亏（元）:",
                TYPES1 = "",
                MODEL = "",

                DT_1 = Math.Round(t01 - tO01, 2).ToString(),
                DT_2 = Math.Round(t02 - tO02, 2).ToString(),
                DT_3 = Math.Round(t03 - tO03, 2).ToString(),
                DT_4 = Math.Round(t04 - tO04, 2).ToString(),
                DT_5 = Math.Round(t05 - tO05, 2).ToString(),
                DT_6 = Math.Round(t06 - tO06, 2).ToString(),
                DT_7 = Math.Round(t07 - tO07, 2).ToString(),
                DT_8 = Math.Round(t08 - tO08, 2).ToString(),
                DT_9 = Math.Round(t09 - tO09, 2).ToString(),
                DT_10 = Math.Round(t10 - tO10, 2).ToString(),
                DT_11 = Math.Round(t11 - tO11, 2).ToString(),
                DT_12 = Math.Round(t12 - tO12, 2).ToString(),
                DT_13 = Math.Round(t13 - tO13, 2).ToString(),
                DT_14 = Math.Round(t14 - tO14, 2).ToString(),
                DT_15 = Math.Round(t15 - tO15, 2).ToString(),
                DT_16 = Math.Round(t16 - tO16, 2).ToString(),
                DT_17 = Math.Round(t17 - tO17, 2).ToString(),
                DT_18 = Math.Round(t18 - tO18, 2).ToString(),
                DT_19 = Math.Round(t19 - tO19, 2).ToString(),
                DT_20 = Math.Round(t20 - tO20, 2).ToString(),
                DT_21 = Math.Round(t21 - tO21, 2).ToString(),
                DT_22 = Math.Round(t22 - tO22, 2).ToString(),
                DT_23 = Math.Round(t23 - tO23, 2).ToString(),
                DT_24 = Math.Round(t24 - tO24, 2).ToString(),
                DT_25 = Math.Round(t25 - tO25, 2).ToString(),
                DT_26 = Math.Round(t26 - tO26, 2).ToString(),
                DT_27 = Math.Round(t27 - tO27, 2).ToString(),
                DT_28 = Math.Round(t28 - tO28, 2).ToString(),
                DT_29 = Math.Round(t29 - tO29, 2).ToString(),
                DT_30 = Math.Round(t30 - tO30, 2).ToString(),
                DT_31 = Math.Round(t31 - tO31, 2).ToString(),
                TOTAL = Math.Round(ttal - tOtal, 2).ToString()
            };
            dA_VIEWs.Add(dA_VIEWTT2);
            return dA_VIEWs;
        }

        public async Task<IEnumerable<DA_VIEW>> GetDA_VIEWN1(string dATE1)
        {
            #region
            double t01 = 0;
            double t02 = 0;
            double t03 = 0;
            double t04 = 0;
            double t05 = 0;
            double t06 = 0;
            double t07 = 0;
            double t08 = 0;
            double t09 = 0;
            double t10 = 0;
            double t11 = 0;
            double t12 = 0;
            double t13 = 0;
            double t14 = 0;
            double t15 = 0;
            double t16 = 0;
            double t17 = 0;
            double t18 = 0;
            double t19 = 0;
            double t20 = 0;
            double t21 = 0;
            double t22 = 0;
            double t23 = 0;
            double t24 = 0;
            double t25 = 0;
            double t26 = 0;
            double t27 = 0;
            double t28 = 0;
            double t29 = 0;
            double t30 = 0;
            double t31 = 0;
            double ttal = 0;
            double tO01 = 0;
            double tO02 = 0;
            double tO03 = 0;
            double tO04 = 0;
            double tO05 = 0;
            double tO06 = 0;
            double tO07 = 0;
            double tO08 = 0;
            double tO09 = 0;
            double tO10 = 0;
            double tO11 = 0;
            double tO12 = 0;
            double tO13 = 0;
            double tO14 = 0;
            double tO15 = 0;
            double tO16 = 0;
            double tO17 = 0;
            double tO18 = 0;
            double tO19 = 0;
            double tO20 = 0;
            double tO21 = 0;
            double tO22 = 0;
            double tO23 = 0;
            double tO24 = 0;
            double tO25 = 0;
            double tO26 = 0;
            double tO27 = 0;
            double tO28 = 0;
            double tO29 = 0;
            double tO30 = 0;
            double tO31 = 0;
            double tOtal = 0;
            #endregion
            var DA_ =await _sqlDbContext.DA_CLENTNAME.Where(x=>x.BU==1).OrderBy(x => x.CODE).ToArrayAsync(); 
            List<DA_VIEW> dA_VIEWs = new List<DA_VIEW>();
            List<DA_VIEW> dA_VIEWs1 = new List<DA_VIEW>();// 
            List<DA_VIEW> dA_VIEWs2 = new List<DA_VIEW>();//收入统计
            List<DA_VIEW> dA_VIEWs3 = new List<DA_VIEW>();//支出统计
            foreach (var item in DA_)
            {
                #region
                DA_VIEW dA_VIEW = new DA_VIEW();
                DA_VIEW dA_VIEW1 = new DA_VIEW();//主营收入
                DA_VIEW dA_VIEW2 = new DA_VIEW();//其他收入（元）
                DA_VIEW dA_VIEW3 = new DA_VIEW();// 收入小计
                var CliemtName = item.NAME;
                dA_VIEW.TYPES = CliemtName;
                dA_VIEW1.TYPES = CliemtName;
                dA_VIEW2.TYPES = CliemtName;
                dA_VIEW3.TYPES = CliemtName;
                dA_VIEW.TYPES1 = "整机入库数";
                dA_VIEW1.TYPES1 = "主营收入（元）";
                dA_VIEW2.TYPES1 = "其他收入（元）";
                dA_VIEW3.TYPES1 = "收入小计（元）";
                dA_VIEW.MODEL = "";
                dA_VIEW1.MODEL = "整机";
                dA_VIEW2.MODEL = "";
                dA_VIEW3.MODEL = "";
                var Dates1 = dATE1 + "-01";
                var valGross1 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName&&x.DT_DATE.Value.ToString("yyyy-MM-dd")== Dates1);
                var sumGross1 = valGross1.Sum(x => x.DT_STORENB==""?0:Convert.ToDouble(x.DT_STORENB));  
                dA_VIEW.DT_1 = sumGross1.ToString();
                var priceGross1 = valGross1.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_1 = priceGross1.ToString();
                var valelseGross1 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumrlseGross1 = valelseGross1.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_1 = sumrlseGross1.ToString();
                dA_VIEW3.DT_1 =(priceGross1 + sumrlseGross1).ToString();

                var Dates2 = dATE1 + "-02";
                var valGross2= _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates2);
                var sumGross2 = valGross2.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)); 
                dA_VIEW.DT_2 = sumGross2.ToString();
                var priceGross2 = valGross2.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_2= priceGross2.ToString();
                var valelseGross2 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates2);
                var sumrlseGross2 = valelseGross2.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_2 = sumrlseGross2.ToString();
                dA_VIEW3.DT_2 = (priceGross2 + sumrlseGross2).ToString();

                var Dates3 = dATE1 + "-03";
                var valGross3 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates3);
                var sumGross3 = valGross3.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)); 
                dA_VIEW.DT_3 = sumGross3.ToString();
                var priceGross3 = valGross3.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_3 = priceGross3.ToString();
                var valelseGross3 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates3);
                var sumrlseGross3 = valelseGross3.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_3 = sumrlseGross3.ToString();
                dA_VIEW3.DT_3 = (priceGross3 + sumrlseGross3).ToString();

                var Dates4 = dATE1 + "-04";
                var valGross4 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates4);
                var sumGross4 = valGross4.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_4 = sumGross4.ToString();
                var priceGross4 = valGross4.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_4 = priceGross4.ToString();
                var valelseGross4 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates4);
                var sumrlseGross4 = valelseGross4.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_4 = sumrlseGross4.ToString();
                dA_VIEW3.DT_4 = (priceGross4 + sumrlseGross4).ToString();

                var Dates5 = dATE1 + "-05";
                var valGross5 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates5);
                var sumGross5 = valGross5.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_5 = sumGross5.ToString();
                var priceGross5 = valGross5.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_5 = priceGross5.ToString();
                var valelseGross5 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates5);
                var sumrlseGross5 = valelseGross5.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_5 = sumrlseGross5.ToString();
                dA_VIEW3.DT_5 = (priceGross5 + sumrlseGross5).ToString();

                var Dates6 = dATE1 + "-06";
                var valGross6 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates6);
                var sumGross6 = valGross6.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_6 = sumGross6.ToString();
                var priceGross6 = valGross6.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_6 = priceGross6.ToString();
                var valelseGross6 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates6);
                var sumrlseGross6 = valelseGross6.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_6= sumrlseGross6.ToString();
                dA_VIEW3.DT_6 = (priceGross6 + sumrlseGross6).ToString();

                var Dates7 = dATE1 + "-07";
                var valGross7 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates7);
                var sumGross7 = valGross7.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_7 = sumGross7.ToString();
                var priceGross7 = valGross7.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_7 = priceGross7.ToString();
                var valelseGross7 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates7);
                var sumrlseGross7 = valelseGross7.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_7 = sumrlseGross7.ToString();
                dA_VIEW3.DT_7 = (priceGross7 + sumrlseGross7).ToString();

                var Dates8 = dATE1 + "-08";
                var valGross8 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates8);
                var sumGross8 = valGross8.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_8 = sumGross8.ToString();
                var priceGross8 = valGross8.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_8 = priceGross8.ToString();
                var valelseGross8 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates8);
                var sumrlseGross8 = valelseGross8.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_8 = sumrlseGross8.ToString();
                dA_VIEW3.DT_8 = (priceGross8 + sumrlseGross8).ToString();

                var Dates9 = dATE1 + "-09";
                var valGross9 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates9);
                var sumGross9 = valGross9.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_9 = sumGross9.ToString();
                var priceGross9 = valGross9.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_9 = priceGross9.ToString();
                var valelseGross9 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates9);
                var sumrlseGross9 = valelseGross9.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_9 = sumrlseGross9.ToString();
                dA_VIEW3.DT_9 = (priceGross9 + sumrlseGross9).ToString();

                var Dates10 = dATE1 + "-10";
                var valGross10 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates10);
                var sumGross10 = valGross10.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_10 = sumGross10.ToString();
                var priceGross10 = valGross10.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_10 = priceGross10.ToString();
                var valelseGross10 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates10);
                var sumrlseGross10 = valelseGross10.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_10 = sumrlseGross10.ToString();
                dA_VIEW3.DT_10 = (priceGross10 + sumrlseGross10).ToString();

                var Dates11 = dATE1 + "-11";
                var valGross11 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates11);
                var sumGross11 = valGross11.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_11 = sumGross11.ToString();
                var priceGross11 = valGross11.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_11 = priceGross11.ToString();
                var valelseGross11 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates11);
                var sumrlseGross11 = valelseGross11.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_11 = sumrlseGross11.ToString();
                dA_VIEW3.DT_11 = (priceGross11 + sumrlseGross11).ToString();

                string Dates12 = dATE1 + "-12";
                IQueryable<DA_GROSS> valGross12 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates12);
                var sumGross12 = valGross12.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_12 = sumGross12.ToString();
                var priceGross12 = valGross12.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_12 = priceGross12.ToString();
                var valelseGross12 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates12);
                var sumrlseGross12= valelseGross12.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_12 = sumrlseGross12.ToString();
                dA_VIEW3.DT_12 = (priceGross12 + sumrlseGross12).ToString();

                var Dates13 = dATE1 + "-13";
                var valGross13 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates13);
                var sumGross13 = valGross13.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_13 = sumGross13.ToString();
                var priceGross13 = valGross13.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_13 = priceGross13.ToString();
                var valelseGross13 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates13);
                var sumrlseGross13 = valelseGross13.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_13 = sumrlseGross13.ToString();
                dA_VIEW3.DT_13 = (priceGross13 + sumrlseGross13).ToString();

                var Dates14 = dATE1 + "-14";
                var valGross14 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates14);
                var sumGross14 = valGross14.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_14 = sumGross14.ToString();
                var priceGross14 = valGross14.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_14 = priceGross14.ToString();
                var valelseGross14 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates14);
                var sumrlseGross14 = valelseGross14.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_14 = sumrlseGross14.ToString();
                dA_VIEW3.DT_14 = (priceGross14 + sumrlseGross14).ToString();

                var Dates15 = dATE1 + "-15";
                var valGross15 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates15);
                var sumGross15 = valGross15.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_15 = sumGross15.ToString();
                var priceGross15 = valGross15.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_15 = priceGross15.ToString();
                var valelseGross15 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates15);
                var sumrlseGross15 = valelseGross15.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_15 = sumrlseGross15.ToString();
                dA_VIEW3.DT_15 = (priceGross15 + sumrlseGross15).ToString();

                var Dates16 = dATE1 + "-16";
                var valGross16 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates16);
                var sumGross16 = valGross16.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_16 = sumGross16.ToString();
                var priceGross16 = valGross16.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_16 = priceGross16.ToString();
                var valelseGross16 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates16);
                var sumrlseGross16 = valelseGross16.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_16 = sumrlseGross16.ToString();
                dA_VIEW3.DT_16 = (priceGross16 + sumrlseGross16).ToString();

                var Dates17 = dATE1 + "-17";
                var valGross17 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates17);
                var sumGross17 = valGross17.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_17= sumGross17.ToString();
                var priceGross17 = valGross17.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_17 = priceGross17.ToString();
                var valelseGross17 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates17);
                var sumrlseGross17 = valelseGross17.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_17 = sumrlseGross17.ToString();
                dA_VIEW3.DT_17 = (priceGross17 + sumrlseGross17).ToString();

                var Dates18 = dATE1 + "-18";
                var valGross18 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates18);
                var sumGross18 = valGross18.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_18 = sumGross18.ToString();
                var priceGross18 = valGross18.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_18 = priceGross18.ToString();
                var valelseGross18 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates18);
                var sumrlseGross18 = valelseGross18.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_18 = sumrlseGross18.ToString();
                dA_VIEW3.DT_18 = (priceGross18 + sumrlseGross18).ToString();

                var Dates19 = dATE1 + "-19";
                var valGross19 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates19);
                var sumGross19 = valGross19.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_19 = sumGross19.ToString();
                var priceGross19 = valGross19.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_19 = priceGross19.ToString();
                var valelseGross19 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates19);
                var sumrlseGross19 = valelseGross19.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_19 = sumrlseGross19.ToString();
                dA_VIEW3.DT_19 = (priceGross19 + sumrlseGross19).ToString();

                var Dates20 = dATE1 + "-20";
                var valGross20 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates20);
                var sumGross20 = valGross20.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_20 = sumGross20.ToString();
                var priceGross20 = valGross20.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_20 = priceGross20.ToString();
                var valelseGross20 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates20);
                var sumrlseGross20 = valelseGross20.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_20 = sumrlseGross20.ToString();
                dA_VIEW3.DT_20 = (priceGross20 + sumrlseGross20).ToString();

                var Dates21 = dATE1 + "-21";
                var valGross21 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates21);
                var sumGross21 = valGross21.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_21 = sumGross21.ToString();
                var priceGross21 = valGross21.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_21 = priceGross21.ToString();
                var valelseGross21 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates21);
                var sumrlseGross21 = valelseGross21.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_21 = sumrlseGross21.ToString();
                dA_VIEW3.DT_21 = (priceGross21 + sumrlseGross21).ToString();

                var Dates22 = dATE1 + "-22";
                var valGross22 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates22);
                var sumGross22 = valGross22.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_22 = sumGross22.ToString();
                var priceGross22 = valGross22.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_22 = priceGross22.ToString();
                var valelseGross22 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates22);
                var sumrlseGross22 = valelseGross22.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_22 = sumrlseGross22.ToString();
                dA_VIEW3.DT_22 = (priceGross22 + sumrlseGross22).ToString();

                var Dates23 = dATE1 + "-23";
                var valGross23= _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates23);
                var sumGross23 = valGross23.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_23 = sumGross23.ToString();
                var priceGross23 = valGross23.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_23 = priceGross23.ToString();
                var valelseGross23 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates23);
                var sumrlseGross23 = valelseGross23.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_23 = sumrlseGross23.ToString();
                dA_VIEW3.DT_23 = (priceGross23 + sumrlseGross23).ToString();

                var Dates24 = dATE1 + "-24";
                var valGross24 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates24);
                var sumGross24 = valGross24.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_24 = sumGross24.ToString();
                var priceGross24 = valGross24.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_24 = priceGross24.ToString();
                var valelseGross24 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates24);
                var sumrlseGross24= valelseGross24.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_24 = sumrlseGross24.ToString();
                dA_VIEW3.DT_24 = (priceGross24 + sumrlseGross24).ToString();

                var Dates25 = dATE1 + "-25";
                var valGross25 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates25);
                var sumGross25 = valGross25.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_25 = sumGross25.ToString();
                var priceGross25 = valGross25.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_25 = priceGross25.ToString();
                var valelseGross25 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates25);
                var sumrlseGross25 = valelseGross25.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_25 = sumrlseGross25.ToString();
                dA_VIEW3.DT_25 = (priceGross25 + sumrlseGross25).ToString();
                var Dates26 = dATE1 + "-26";
                var valGross26 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates26);
                var sumGross26= valGross26.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_26 = sumGross26.ToString();
                var priceGross26 = valGross26.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_26 = priceGross26.ToString();
                var valelseGross26 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates26);
                var sumrlseGross26 = valelseGross26.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_26 = sumrlseGross26.ToString();
                dA_VIEW3.DT_26 = (priceGross26 + sumrlseGross26).ToString();

                var Dates27 = dATE1 + "-27";
                var valGross27 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates27);
                var sumGross27 = valGross27.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_27 = sumGross27.ToString();
                var priceGross27 = valGross27.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_27 = priceGross27.ToString();
                var valelseGross27 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates27);
                var sumrlseGross27 = valelseGross27.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_27 = sumrlseGross27.ToString();
                dA_VIEW3.DT_27 = (priceGross27 + sumrlseGross27).ToString();

                var Dates28 = dATE1 + "-28";
                var valGross28= _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates28);
                var sumGross28 = valGross28.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_28 = sumGross28.ToString();
                var priceGross28 = valGross28.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_28 = priceGross28.ToString();
                var valelseGross28 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates28);
                var sumrlseGross28 = valelseGross28.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_28 = sumrlseGross28.ToString();
                dA_VIEW3.DT_28 = (priceGross28 + sumrlseGross28).ToString();

                var Dates29 = dATE1 + "-29";
                var valGross29= _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates29);
                var sumGross29 = valGross29.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_29 = sumGross29.ToString();
                var priceGross29 = valGross29.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_29 = priceGross29.ToString();
                var valelseGross29 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates29);
                var sumrlseGross29 = valelseGross29.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_29 = sumrlseGross29.ToString();
                dA_VIEW3.DT_29 = (priceGross29 + sumrlseGross29).ToString();

                var Dates30 = dATE1 + "-30";
                var valGross30 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates30);
                var sumGross30 = valGross30.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_30 = sumGross30.ToString();
                var priceGross30 = valGross30.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_30 = priceGross30.ToString();
                var valelseGross30 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates30);
                var sumrlseGross30 = valelseGross30.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_30 = sumrlseGross30.ToString();
                dA_VIEW3.DT_30 = (priceGross30 + sumrlseGross30).ToString();

                var Dates31 = dATE1 + "-31";
                var valGross31 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates31);
                var sumGross31 = valGross31.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW.DT_31 = sumGross31.ToString();
                var priceGross31 = valGross31.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW1.DT_31 = priceGross31.ToString();
                var valelseGross31 = _sqlDbContext.DA_ELSEGROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates31);
                var sumrlseGross31 = valelseGross31.Sum(x => x.DT_VALUE == "" ? 0 : Convert.ToDouble(x.DT_VALUE));
                dA_VIEW2.DT_31 = sumrlseGross31.ToString();
                dA_VIEW3.DT_31 = (priceGross31+ sumrlseGross31).ToString();

                dA_VIEW.TOTAL = (sumGross1 + sumGross2 + sumGross3 + sumGross4 + sumGross5 + sumGross6 + sumGross7 + sumGross8 + sumGross9 + sumGross10 + sumGross11 + sumGross12 + sumGross13 + sumGross14 + sumGross15 + sumGross16 + sumGross17 + sumGross18 + sumGross19 + sumGross20 + sumGross21 + sumGross22 + sumGross23 + sumGross24 + sumGross25 + sumGross26 + sumGross27 + sumGross28 + sumGross29 + sumGross30 + sumGross31).ToString();
                dA_VIEW1.TOTAL = (priceGross1 + priceGross2 + priceGross3 + priceGross4 + priceGross5 + priceGross6 + priceGross7 + priceGross8 + priceGross9 + priceGross10 + priceGross11 + priceGross12 + priceGross13 + priceGross14 + priceGross15 + priceGross16 + priceGross17 + priceGross18 + priceGross19 + priceGross20 + priceGross21 + priceGross22 + priceGross23 + priceGross24 + priceGross25 + priceGross26 + priceGross27 + priceGross28 + priceGross29 + priceGross30 + priceGross31).ToString();
                dA_VIEW2.TOTAL = (sumrlseGross1 + sumrlseGross2 + sumrlseGross3 + sumrlseGross4 + sumrlseGross5 + sumrlseGross6 + sumrlseGross7 + sumrlseGross8 + sumrlseGross9 + sumrlseGross10 + sumrlseGross11 + sumrlseGross12 + sumrlseGross13 + sumrlseGross14 + sumrlseGross15 + sumrlseGross16 + sumrlseGross17 + sumrlseGross18 + sumrlseGross19 + sumrlseGross20 + sumrlseGross21 + sumrlseGross22 + sumrlseGross23 + sumrlseGross24 + sumrlseGross25 + sumrlseGross26 + sumrlseGross27 + sumrlseGross28 + sumrlseGross29 + sumrlseGross30 + sumrlseGross31).ToString();
                dA_VIEW3.TOTAL =(Convert.ToDouble(dA_VIEW1.TOTAL)+ Convert.ToDouble(dA_VIEW2.TOTAL)).ToString();
                dA_VIEWs.Add(dA_VIEW);
                dA_VIEWs.Add(dA_VIEW1);
                dA_VIEWs.Add(dA_VIEW2);
                dA_VIEWs.Add(dA_VIEW3);
                dA_VIEWs2.Add(dA_VIEW3);
                t01 += Convert.ToDouble(dA_VIEW3.DT_1);
                t02 += Convert.ToDouble(dA_VIEW3.DT_2);
                t03 += Convert.ToDouble(dA_VIEW3.DT_3);
                t04 += Convert.ToDouble(dA_VIEW3.DT_4);
                t05 += Convert.ToDouble(dA_VIEW3.DT_5);
                t06 += Convert.ToDouble(dA_VIEW3.DT_6);
                t07 += Convert.ToDouble(dA_VIEW3.DT_7);
                t08 += Convert.ToDouble(dA_VIEW3.DT_8);
                t09 += Convert.ToDouble(dA_VIEW3.DT_9);
                t10 += Convert.ToDouble(dA_VIEW3.DT_10);
                t11 += Convert.ToDouble(dA_VIEW3.DT_11);
                t12 += Convert.ToDouble(dA_VIEW3.DT_12);
                t13 += Convert.ToDouble(dA_VIEW3.DT_13);
                t14 += Convert.ToDouble(dA_VIEW3.DT_14);
                t15 += Convert.ToDouble(dA_VIEW3.DT_15);
                t16 += Convert.ToDouble(dA_VIEW3.DT_16);
                t17 += Convert.ToDouble(dA_VIEW3.DT_17);
                t18 += Convert.ToDouble(dA_VIEW3.DT_18);
                t19 += Convert.ToDouble(dA_VIEW3.DT_19);
                t20 += Convert.ToDouble(dA_VIEW3.DT_20);
                t21 += Convert.ToDouble(dA_VIEW3.DT_21);
                t22 += Convert.ToDouble(dA_VIEW3.DT_22);
                t23 += Convert.ToDouble(dA_VIEW3.DT_23);
                t24 += Convert.ToDouble(dA_VIEW3.DT_24);
                t25 += Convert.ToDouble(dA_VIEW3.DT_25);
                t26 += Convert.ToDouble(dA_VIEW3.DT_26);
                t27 += Convert.ToDouble(dA_VIEW3.DT_27);
                t28 += Convert.ToDouble(dA_VIEW3.DT_28);
                t29 += Convert.ToDouble(dA_VIEW3.DT_29);
                t30 += Convert.ToDouble(dA_VIEW3.DT_30);
                t31 += Convert.ToDouble(dA_VIEW3.DT_31);
                ttal += Convert.ToDouble(dA_VIEW3.TOTAL);
                #endregion  
            }
            var DA_1 = await _sqlDbContext.DA_CLENTNAME.Where(x => x.BU == 2).OrderBy(x => x.CODE).ToArrayAsync();
            foreach (var item in DA_1)
            {
                DA_VIEW dA_VIEW1 = new DA_VIEW();//主营收入 KD入库数
                DA_VIEW dA_VIEW2 = new DA_VIEW();//主营收入
                DA_VIEW dA_VIEW3 = new DA_VIEW();//主营收入 KD入库数
                DA_VIEW dA_VIEW4 = new DA_VIEW();
                DA_VIEW dA_VIEW5 = new DA_VIEW();
                var CliemtName = item.NAME;
                dA_VIEW1.TYPES = CliemtName;
                dA_VIEW2.TYPES = CliemtName;
                dA_VIEW3.TYPES = CliemtName;
                dA_VIEW4.TYPES = CliemtName;
                dA_VIEW5.TYPES = CliemtName;
                dA_VIEW1.TYPES1 = "KD入库数";
                dA_VIEW3.TYPES1 = "KD入库数";
                dA_VIEW2.TYPES1 = "收入（元)"; 
                dA_VIEW4.TYPES1 = "收入（元)";
                dA_VIEW5.TYPES1 = "收入小计（元）";
                dA_VIEW1.MODEL = "组件";
                dA_VIEW3.MODEL = "散料";
                dA_VIEW2.MODEL = "组件";
                dA_VIEW4.MODEL = "散料";
                dA_VIEW5.MODEL = "";
                var Dates1 = dATE1 + "-01";
                var valGross1 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName&&x.DT_NAME=="组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGross1 = valGross1.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_1 = sumGross1.ToString(); 
                var priceGross1 = valGross1.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_1 = priceGross1.ToString();
                var valGrosst1 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumGrosst1 = valGrosst1.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_1 = sumGrosst1.ToString();
                var priceGrosst1 = valGrosst1.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_1 = priceGrosst1.ToString();
                


                var Dates2 = dATE1 + "-02";
                var valGross2 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates2);
                var sumGross2 = valGross2.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_2 = sumGross2.ToString();
                var priceGross2 = valGross2.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_2 = priceGross2.ToString();
                var valGrosst2 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates2);
                var sumGrosst2 = valGrosst2.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_2 = sumGrosst2.ToString();
                var priceGrosst2 = valGrosst2.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_2 = priceGrosst2.ToString();

                var Dates3 = dATE1 + "-03";
                var valGross3 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates3);
                var sumGross3 = valGross3.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_3 = sumGross3.ToString();
                var priceGross3 = valGross3.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_3 = priceGross3.ToString();
                var valGrosst3 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates3);
                var sumGrosst3 = valGrosst3.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_3 = sumGrosst3.ToString();
                var priceGrosst3 = valGrosst3.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_3 = priceGrosst3.ToString();

                var Dates4 = dATE1 + "-04";
                var valGross4 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates4);
                var sumGross4 = valGross4.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_4 = sumGross4.ToString();
                var priceGross4 = valGross4.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_4 = priceGross4.ToString();
                var valGrosst4 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates4);
                var sumGrosst4 = valGrosst4.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_4 = sumGrosst4.ToString();
                var priceGrosst4 = valGrosst4.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_4 = priceGrosst4.ToString();

                var Dates5 = dATE1 + "-05";
                var valGross5 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates5);
                var sumGross5 = valGross5.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_5 = sumGross5.ToString();
                var priceGross5 = valGross5.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_5  = priceGross5.ToString();
                var valGrosst5 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates5);
                var sumGrosst5 = valGrosst5.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_5 = sumGrosst5.ToString();
                var priceGrosst5 = valGrosst5.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_5 = priceGrosst5.ToString();

                var Dates6 = dATE1 + "-01";
                var valGross6 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates6);
                var sumGross6 = valGross6.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_6 = sumGross6.ToString();
                var priceGross6 = valGross6.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_6 = priceGross6.ToString();
                var valGrosst6 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates6);
                var sumGrosst6 = valGrosst6.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_6 = sumGrosst6.ToString();
                var priceGrosst6 = valGrosst6.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_6 = priceGrosst6.ToString();

                var Dates7 = dATE1 + "-01";
                var valGross7 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates7);
                var sumGross7 = valGross7.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_7 = sumGross7.ToString();
                var priceGross7 = valGross7.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_7 = priceGross7.ToString();
                var valGrosst7 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates7);
                var sumGrosst7 = valGrosst7.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_7 = sumGrosst7.ToString();
                var priceGrosst7 = valGrosst7.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_7 = priceGrosst7.ToString();

                var Dates8 = dATE1 + "-08";
                var valGross8 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates8);
                var sumGross8 = valGross8.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_8 = sumGross8.ToString();
                var priceGross8 = valGross8.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_8 = priceGross8.ToString();
                var valGrosst8 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates8);
                var sumGrosst8 = valGrosst8.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_8 = sumGrosst8.ToString();
                var priceGrosst8 = valGrosst8.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_8 = priceGrosst8.ToString();

                var Dates9 = dATE1 + "-09";
                var valGross9 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates9);
                var sumGross9 = valGross9.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_9 = sumGross9.ToString();
                var priceGross9 = valGross9.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_9 = priceGross9.ToString();
                var valGrosst9 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates9);
                var sumGrosst9 = valGrosst9.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_9 = sumGrosst9.ToString();
                var priceGrosst9 = valGrosst9.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_9 = priceGrosst9.ToString();

                var Dates10 = dATE1 + "-10";
                var valGross10 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates10);
                var sumGross10 = valGross10.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_10 = sumGross10.ToString();
                var priceGross10 = valGross10.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_10 = priceGross10.ToString();
                var valGrosst10 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates10);
                var sumGrosst10 = valGrosst10.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_10 = sumGrosst10.ToString();
                var priceGrosst10 = valGrosst10.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_10 = priceGrosst10.ToString();

                var Dates11 = dATE1 + "-11";
                var valGross11 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates11);
                var sumGross11 = valGross11.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_11 = sumGross11.ToString();
                var priceGross11 = valGross11.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_11 = priceGross11.ToString();
                var valGrosst11 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates11);
                var sumGrosst11 = valGrosst11.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_11 = sumGrosst11.ToString();
                var priceGrosst11 = valGrosst11.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_11 = priceGrosst11.ToString();

                var Dates12 = dATE1 + "-12";
                var valGross12 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates12);
                var sumGross12 = valGross12.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_12 = sumGross12.ToString();
                var priceGross12 = valGross12.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_12 = priceGross12.ToString();
                var valGrosst12 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates12);
                var sumGrosst12 = valGrosst12.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_12 = sumGrosst12.ToString();
                var priceGrosst12 = valGrosst12.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_12 = priceGrosst12.ToString();

                var Dates13 = dATE1 + "-13";
                var valGross13 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates13);
                var sumGross13 = valGross13.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_13 = sumGross13.ToString();
                var priceGross13 = valGross13.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_13 = priceGross13.ToString();
                var valGrosst13 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates13);
                var sumGrosst13 = valGrosst13.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_13 = sumGrosst13.ToString();
                var priceGrosst13= valGrosst13.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_13 = priceGrosst13.ToString();

                var Dates14 = dATE1 + "-01";
                var valGross14 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates14);
                var sumGross14 = valGross14.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_14 = sumGross14.ToString();
                var priceGross14 = valGross14.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_14 = priceGross14.ToString();
                var valGrosst14 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates14);
                var sumGrosst14 = valGrosst14.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_14 = sumGrosst14.ToString();
                var priceGrosst14 = valGrosst14.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_14 = priceGrosst14.ToString();

                var Dates15 = dATE1 + "-15";
                var valGross15 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates15);
                var sumGross15 = valGross15.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_15 = sumGross15.ToString();
                var priceGross15 = valGross15.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_15 = priceGross15.ToString();
                var valGrosst15 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates15);
                var sumGrosst15 = valGrosst15.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_15 = sumGrosst15.ToString();
                var priceGrosst15 = valGrosst15.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_15 = priceGrosst15.ToString();

                var Dates16 = dATE1 + "-16";
                var valGross16 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates16);
                var sumGross16 = valGross16.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_16 = sumGross16.ToString();
                var priceGross16 = valGross16.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_16 = priceGross16.ToString();
                var valGrosst16 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates16);
                var sumGrosst16 = valGrosst16.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_16 = sumGrosst16.ToString();
                var priceGrosst16 = valGrosst16.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_16 = priceGrosst16.ToString();

                var Dates17 = dATE1 + "-17";
                var valGross17 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates17);
                var sumGross17 = valGross17.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_17 = sumGross17.ToString();
                var priceGross17= valGross17.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_17 = priceGross17.ToString();
                var valGrosst17 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates17);
                var sumGrosst17 = valGrosst17.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_17 = sumGrosst17.ToString();
                var priceGrosst17 = valGrosst17.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_17 = priceGrosst17.ToString();

                var Dates18 = dATE1 + "-18";
                var valGross18 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates18);
                var sumGross18 = valGross18.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_18 = sumGross18.ToString();
                var priceGross18 = valGross18.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_18= priceGross18.ToString();
                var valGrosst18 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates18);
                var sumGrosst18 = valGrosst18.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_18 = sumGrosst18.ToString();
                var priceGrosst18 = valGrosst18.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_18 = priceGrosst18.ToString();

                var Dates19 = dATE1 + "-19";
                var valGross19 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates19);
                var sumGross19 = valGross19.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_19 = sumGross19.ToString();
                var priceGross19 = valGross19.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_19 = priceGross19.ToString();
                var valGrosst19 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates19);
                var sumGrosst19 = valGrosst19.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_19 = sumGrosst19.ToString();
                var priceGrosst19 = valGrosst19.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_19 = priceGrosst19.ToString();

                var Dates20 = dATE1 + "-20";
                var valGross20 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates20);
                var sumGross20 = valGross20.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_20 = sumGross20.ToString();
                var priceGross20 = valGross20.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_20 = priceGross20.ToString();
                var valGrosst20 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates20);
                var sumGrosst20 = valGrosst20.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_20= sumGrosst20.ToString();
                var priceGrosst20 = valGrosst20.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_20 = priceGrosst20.ToString();

                var Dates21 = dATE1 + "-21";
                var valGross21 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates21);
                var sumGross21 = valGross21.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_21 = sumGross21.ToString();
                var priceGross21 = valGross21.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_21 = priceGross21.ToString();
                var valGrosst21 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates21);
                var sumGrosst21 = valGrosst21.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_21 = sumGrosst21.ToString();
                var priceGrosst21 = valGrosst21.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_21 = priceGrosst21.ToString();

                var Dates22 = dATE1 + "-22";
                var valGross22 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates22);
                var sumGross22 = valGross22.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_22 = sumGross22.ToString();
                var priceGross22 = valGross22.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_22 = priceGross22.ToString();
                var valGrosst22 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates22);
                var sumGrosst22 = valGrosst22.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_22 = sumGrosst22.ToString();
                var priceGrosst22 = valGrosst22.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_22 = priceGrosst22.ToString();

                var Dates23 = dATE1 + "-23";
                var valGross23 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates23);
                var sumGross23 = valGross23.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_23 = sumGross23.ToString();
                var priceGross23 = valGross23.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_23 = priceGross23.ToString();
                var valGrosst23 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates23);
                var sumGrosst23 = valGrosst23.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_23 = sumGrosst23.ToString();
                var priceGrosst23 = valGrosst23.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_23 = priceGrosst23.ToString();

                var Dates24 = dATE1 + "-24";
                var valGross24 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates24);
                var sumGross24 = valGross24.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_24 = sumGross24.ToString();
                var priceGross24 = valGross24.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_24 = priceGross24.ToString();
                var valGrosst24 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates24);
                var sumGrosst24 = valGrosst24.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_24 = sumGrosst24.ToString();
                var priceGrosst24 = valGrosst24.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_24 = priceGrosst24.ToString();

                var Dates25 = dATE1 + "-25";
                var valGross25 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates25);
                var sumGross25 = valGross25.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_25 = sumGross25.ToString();
                var priceGross25 = valGross25.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_25 = priceGross25.ToString();
                var valGrosst25 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates25);
                var sumGrosst25 = valGrosst25.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_25 = sumGrosst25.ToString();
                var priceGrosst25 = valGrosst25.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_25 = priceGrosst25.ToString();

                var Dates26 = dATE1 + "-26";
                var valGross26 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates26);
                var sumGross26 = valGross26.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_26 = sumGross26.ToString();
                var priceGross26 = valGross26.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_26 = priceGross26.ToString();
                var valGrosst26 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates26);
                var sumGrosst26 = valGrosst26.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_26 = sumGrosst26.ToString();
                var priceGrosst26 = valGrosst26.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_26 = priceGrosst26.ToString();

                var Dates27 = dATE1 + "-27";
                var valGross27 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates27);
                var sumGross27 = valGross27.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_27 = sumGross27.ToString();
                var priceGross27 = valGross27.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_27 = priceGross27.ToString();
                var valGrosst27 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates27);
                var sumGrosst27 = valGrosst27.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_27 = sumGrosst27.ToString();
                var priceGrosst27 = valGrosst27.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_27 = priceGrosst27.ToString();

                var Dates28 = dATE1 + "-28";
                var valGross28 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates28);
                var sumGross28 = valGross28.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_28 = sumGross28.ToString();
                var priceGross28 = valGross28.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_28 = priceGross28.ToString();
                var valGrosst28 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates28);
                var sumGrosst28 = valGrosst28.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_28 = sumGrosst28.ToString();
                var priceGrosst28 = valGrosst28.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_28 = priceGrosst28.ToString();

                var Dates29 = dATE1 + "-29";
                var valGross29 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates29);
                var sumGross29 = valGross29.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_29 = sumGross29.ToString();
                var priceGross29 = valGross29.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_29 = priceGross29.ToString();
                var valGrosst29 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates29);
                var sumGrosst29 = valGrosst29.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_29 = sumGrosst29.ToString();
                var priceGrosst29 = valGrosst29.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_29 = priceGrosst29.ToString();

                var Dates30 = dATE1 + "-30";
                var valGross30 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates30);
                var sumGross30 = valGross30.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_30 = sumGross30.ToString();
                var priceGross30 = valGross30.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_30 = priceGross30.ToString();
                var valGrosst30 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates30);
                var sumGrosst30 = valGrosst30.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_30 = sumGrosst30.ToString();
                var priceGrosst30 = valGrosst30.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_30 = priceGrosst30.ToString();

                var Dates31 = dATE1 + "-31";
                var valGross31 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "组件入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates31);
                var sumGross31 = valGross31.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW1.DT_31 = sumGross31.ToString();
                var priceGross31 = valGross31.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW2.DT_31 = priceGross31.ToString();
                var valGrosst31 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_NAME == "散料入库" && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates31);
                var sumGrosst31 = valGrosst31.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                dA_VIEW3.DT_31 = sumGrosst31.ToString();
                var priceGrosst31 = valGrosst31.Sum(x => (x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_31 = priceGrosst31.ToString();
                dA_VIEW5.DT_1 = (priceGross1 + priceGrosst1).ToString();
                dA_VIEW5.DT_2 = (priceGross2 + priceGrosst2).ToString();
                dA_VIEW5.DT_3 = (priceGross3 + priceGrosst3).ToString();
                dA_VIEW5.DT_4 = (priceGross4 + priceGrosst4).ToString();
                dA_VIEW5.DT_5 = (priceGross5 + priceGrosst5).ToString();
                dA_VIEW5.DT_6 = (priceGross6 + priceGrosst6).ToString();
                dA_VIEW5.DT_7 = (priceGross7 + priceGrosst7).ToString();
                dA_VIEW5.DT_8 = (priceGross8 + priceGrosst8).ToString();
                dA_VIEW5.DT_9 = (priceGross9 + priceGrosst9).ToString();
                dA_VIEW5.DT_10 = (priceGross10 + priceGrosst10).ToString();
                dA_VIEW5.DT_11 = (priceGross11 + priceGrosst11).ToString();
                dA_VIEW5.DT_12 = (priceGross12 + priceGrosst12).ToString();
                dA_VIEW5.DT_13 = (priceGross13 + priceGrosst13).ToString();
                dA_VIEW5.DT_14 = (priceGross14 + priceGrosst14).ToString();
                dA_VIEW5.DT_15 = (priceGross15 + priceGrosst15).ToString();
                dA_VIEW5.DT_16 = (priceGross16 + priceGrosst16).ToString();
                dA_VIEW5.DT_17 = (priceGross17 + priceGrosst17).ToString();
                dA_VIEW5.DT_18 = (priceGross18 + priceGrosst18).ToString();
                dA_VIEW5.DT_19 = (priceGross19 + priceGrosst19).ToString();
                dA_VIEW5.DT_20 = (priceGross20 + priceGrosst20).ToString();
                dA_VIEW5.DT_21 = (priceGross21 + priceGrosst21).ToString();
                dA_VIEW5.DT_22 = (priceGross22 + priceGrosst22).ToString();
                dA_VIEW5.DT_23 = (priceGross23 + priceGrosst23).ToString();
                dA_VIEW5.DT_24 = (priceGross24 + priceGrosst24).ToString();
                dA_VIEW5.DT_25 = (priceGross25 + priceGrosst25).ToString();
                dA_VIEW5.DT_26 = (priceGross26 + priceGrosst26).ToString();
                dA_VIEW5.DT_27 = (priceGross27 + priceGrosst27).ToString();
                dA_VIEW5.DT_28 = (priceGross28 + priceGrosst28).ToString();
                dA_VIEW5.DT_29 = (priceGross29 + priceGrosst29).ToString();
                dA_VIEW5.DT_30 = (priceGross30 + priceGrosst30).ToString();
                dA_VIEW5.DT_31 = (priceGross31 + priceGrosst31).ToString();
                dA_VIEW1.TOTAL = (sumGross1 + sumGross2 + sumGross3 + sumGross4 + sumGross5 + sumGross6 + sumGross7 + sumGross8 + sumGross9 + sumGross10 + sumGross11 + sumGross12 + sumGross13 + sumGross14 + sumGross15 + sumGross16 + sumGross17 + sumGross18 + sumGross19 + sumGross20 + sumGross21 + sumGross22 + sumGross23 + sumGross24 + sumGross25 + sumGross26 + sumGross27 + sumGross28 + sumGross29 + sumGross30 + sumGross31).ToString();
                dA_VIEW2.TOTAL = (priceGross1 + priceGross2 + priceGross3 + priceGross4 + priceGross5 + priceGross6 + priceGross7 + priceGross8 + priceGross9 + priceGross10 + priceGross11 + priceGross12 + priceGross13 + priceGross14 + priceGross15 + priceGross16 + priceGross17 + priceGross18 + priceGross19 + priceGross20 + priceGross21 + priceGross22 + priceGross23 + priceGross24 + priceGross25 + priceGross26 + priceGross27 + priceGross28 + priceGross29 + priceGross30 + priceGross31).ToString();
                dA_VIEW3.TOTAL = (sumGrosst1 + sumGrosst2 + sumGrosst3 + sumGrosst4 + sumGrosst5 + sumGrosst6 + sumGrosst7 + sumGrosst8 + sumGrosst9 + sumGrosst10 + sumGrosst11 + sumGrosst12 + sumGrosst13 + sumGrosst14 + sumGrosst15 + sumGrosst16 + sumGrosst17 + sumGrosst18 + sumGrosst19 + sumGrosst20 + sumGrosst21 + sumGrosst22 + sumGrosst23 + sumGrosst24 + sumGrosst25 + sumGrosst26 + sumGrosst27 + sumGrosst28 + sumGrosst29 + sumGrosst30 + sumGrosst31).ToString();
                dA_VIEW4.TOTAL = (priceGrosst1 + priceGrosst2 + priceGrosst3 + priceGrosst4 + priceGrosst5 + priceGrosst6 + priceGrosst7 + priceGrosst8 + priceGrosst9 + priceGrosst10 + priceGrosst11 + priceGrosst12 + priceGrosst13 + priceGrosst14 + priceGrosst15 + priceGrosst16 + priceGrosst17 + priceGrosst18 + priceGrosst19 + priceGrosst20 + priceGrosst21 + priceGrosst22 + priceGrosst23 + priceGrosst24 + priceGrosst25 + priceGrosst26 + priceGrosst27 + priceGrosst28 + priceGrosst29 + priceGrosst30 + priceGrosst31).ToString();
                dA_VIEW5.TOTAL = (Convert.ToDouble(dA_VIEW2.TOTAL) + Convert.ToDouble(dA_VIEW4.TOTAL)).ToString();
                dA_VIEWs.Add(dA_VIEW1); 
                dA_VIEWs.Add(dA_VIEW3);
                dA_VIEWs.Add(dA_VIEW2);
                dA_VIEWs.Add(dA_VIEW4);
                dA_VIEWs.Add(dA_VIEW5);
                dA_VIEWs2.Add(dA_VIEW5);
                t01 += Convert.ToDouble(dA_VIEW5.DT_1);
                t02 += Convert.ToDouble(dA_VIEW5.DT_2);
                t03 += Convert.ToDouble(dA_VIEW5.DT_3);
                t04 += Convert.ToDouble(dA_VIEW5.DT_4);
                t05 += Convert.ToDouble(dA_VIEW5.DT_5);
                t06 += Convert.ToDouble(dA_VIEW5.DT_6);
                t07 += Convert.ToDouble(dA_VIEW5.DT_7);
                t08 += Convert.ToDouble(dA_VIEW5.DT_8);
                t09 += Convert.ToDouble(dA_VIEW5.DT_9);
                t10 += Convert.ToDouble(dA_VIEW5.DT_10);
                t11 += Convert.ToDouble(dA_VIEW5.DT_11);
                t12 += Convert.ToDouble(dA_VIEW5.DT_12);
                t13 += Convert.ToDouble(dA_VIEW5.DT_13);
                t14 += Convert.ToDouble(dA_VIEW5.DT_14);
                t15 += Convert.ToDouble(dA_VIEW5.DT_15);
                t16 += Convert.ToDouble(dA_VIEW5.DT_16);
                t17 += Convert.ToDouble(dA_VIEW5.DT_17);
                t18 += Convert.ToDouble(dA_VIEW5.DT_18);
                t19 += Convert.ToDouble(dA_VIEW5.DT_19);
                t20 += Convert.ToDouble(dA_VIEW5.DT_20);
                t21 += Convert.ToDouble(dA_VIEW5.DT_21);
                t22 += Convert.ToDouble(dA_VIEW5.DT_22);
                t23 += Convert.ToDouble(dA_VIEW5.DT_23);
                t24 += Convert.ToDouble(dA_VIEW5.DT_24);
                t25 += Convert.ToDouble(dA_VIEW5.DT_25);
                t26 += Convert.ToDouble(dA_VIEW5.DT_26);
                t27 += Convert.ToDouble(dA_VIEW5.DT_27);
                t28 += Convert.ToDouble(dA_VIEW5.DT_28);
                t29 += Convert.ToDouble(dA_VIEW5.DT_29);
                t30 += Convert.ToDouble(dA_VIEW5.DT_30);
                t31 += Convert.ToDouble(dA_VIEW5.DT_31);
                ttal += Convert.ToDouble(dA_VIEW5.TOTAL);
            }
            DA_VIEW dA_VIEWT = new DA_VIEW
            {
                TYPES = "汇总",
                TYPES1 = "收入小计（元）",
                MODEL = "",
                DT_1 = t01.ToString(),
                DT_2 = t02.ToString(),
                DT_3 = t03.ToString(),
                DT_4 = t04.ToString(),
                DT_5 = t05.ToString(),
                DT_6 = t06.ToString(),
                DT_7 = t07.ToString(),
                DT_8 = t08.ToString(),
                DT_9 = t09.ToString(),
                DT_10= t10.ToString(),
                DT_11 = t11.ToString(),
                DT_12 = t12.ToString(),
                DT_13 = t13.ToString(),
                DT_14 = t14.ToString(),
                DT_15 = t15.ToString(),
                DT_16 = t16.ToString(),
                DT_17 = t17.ToString(),
                DT_18 = t18.ToString(),
                DT_19 = t19.ToString(),
                DT_20 = t20.ToString(),
                DT_21 = t21.ToString(),
                DT_22 = t22.ToString(),
                DT_23 = t23.ToString(),
                DT_24 = t24.ToString(),
                DT_25 = t25.ToString(),
                DT_26 = t26.ToString(),
                DT_27 = t27.ToString(),
                DT_28 = t28.ToString(),
                DT_29 = t29.ToString(),
                DT_30 = t30.ToString(),
                DT_31 = t31.ToString(),
                TOTAL = ttal.ToString()
            };
            dA_VIEWs.Add(dA_VIEWT);
            //人工及料耗费用支出
            //获取时薪
            DA_BOM HourPices = await GetDA_BOM("直接人员时薪");
            double HourPice = HourPices.PRICE==""?0:Convert.ToDouble(HourPices.PRICE);
            var DA_2 = await _sqlDbContext.DA_CLENTNAME.OrderBy(x=>x.CODE).ToArrayAsync();
            
            foreach (var item in DA_2)
            {
                DA_VIEW dA_VIEW1 = new DA_VIEW();//直接作业人员
                DA_VIEW dA_VIEW2 = new DA_VIEW();//直接员工费用
                DA_VIEW dA_VIEW3 = new DA_VIEW();//机物料消耗  
                DA_VIEW dA_VIEW4 = new DA_VIEW();//超损物料费
                DA_VIEW dA_VIEW5 = new DA_VIEW();//单台生产费用
                DA_VIEW dA_VIEW6 = new DA_VIEW();//支出汇总
                var CliemtName = item.NAME;
                dA_VIEW1.TYPES = "人工及料耗费用支出";
                dA_VIEW2.TYPES = "人工及料耗费用支出";
                dA_VIEW3.TYPES = "人工及料耗费用支出";
                dA_VIEW4.TYPES = "人工及料耗费用支出";
                dA_VIEW5.TYPES = "人工及料耗费用支出";
                dA_VIEW6.TYPES = "支出汇总";
                dA_VIEW1.TYPES1 = CliemtName;
                dA_VIEW2.TYPES1 = CliemtName;
                dA_VIEW3.TYPES1 = CliemtName;
                dA_VIEW4.TYPES1 = CliemtName;
                dA_VIEW5.TYPES1 = CliemtName;
                dA_VIEW6.TYPES1 = CliemtName;
                dA_VIEW1.MODEL = "直接作业人员";
                dA_VIEW2.MODEL = "直接员工费用";
                dA_VIEW3.MODEL = "机物料消耗";
                dA_VIEW4.MODEL = "超损物料费";
                dA_VIEW5.MODEL = "单台生产费用";
                dA_VIEW6.MODEL = CliemtName;
                var Dates1 = dATE1 + "-01";
                var valGross1 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                double sumGross1 = valGross1.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_1 = sumGross1.ToString();
                double sumPiceGross1 = valGross1.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK))* (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR))* (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_1 = sumPiceGross1.ToString();
                //机物料消耗
                var valDA_PAYLOSS1= _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1).Where(x=>x.DT_NAME== "生产辅料消耗" || x.DT_NAME== "低值易耗消耗");
                var sumDA_PAYLOSS1 = valDA_PAYLOSS1.Sum(x=> (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS))*(x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_1 = sumDA_PAYLOSS1.ToString();
                var valPAYLOSS1 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1).Where(x => x.DT_NAME == "超损物料损耗");
                double sumPAYLOSS1 = valPAYLOSS1.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_1 = sumPAYLOSS1.ToString();
                var valZJGross1 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates1);
                var sumZJGross1 = valZJGross1.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                double OneCount1 = sumZJGross1 == 0 ? 0 : Math.Round((sumDA_PAYLOSS1 + sumPiceGross1 + sumPAYLOSS1) / sumZJGross1, 2);
                dA_VIEW5.DT_1 = OneCount1.ToString();
                dA_VIEW6.DT_1 = (sumDA_PAYLOSS1 + sumPiceGross1 + sumPAYLOSS1).ToString();

                var Dates2 = dATE1 + "-02";
                var valGross2 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates2);
                var sumGross2 = valGross2.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_2 = sumGross2.ToString();
                var dd = valGross2.ToList();
                var sumPiceGross2 = valGross2.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_2 = sumPiceGross2.ToString();
                var valDA_PAYLOSS2 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates2).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS2 = valDA_PAYLOSS2.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_2 = sumDA_PAYLOSS2.ToString();
                var valPAYLOSS2 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates2).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS2 = valPAYLOSS2.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_2 = sumPAYLOSS2.ToString();
                var valZJGross2 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates2);
                var sumZJGross2 = valZJGross2.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount2 = sumZJGross2 == 0 ? 0 : Math.Round((sumDA_PAYLOSS2 + sumPiceGross2 + sumPAYLOSS2) / sumZJGross2, 2);
                dA_VIEW5.DT_2 = OneCount2.ToString();
                dA_VIEW6.DT_2 = (sumDA_PAYLOSS2 + sumPiceGross2 + sumPAYLOSS2).ToString();

                var Dates3 = dATE1 + "-03";
                var valGross3 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates3);
                var sumGross3 = valGross3.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_3 = sumGross3.ToString();
                var sumPiceGross3 = valGross3.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_3 = sumPiceGross3.ToString();
                var valDA_PAYLOSS3 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates3).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS3 = valDA_PAYLOSS3.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_3 = sumDA_PAYLOSS3.ToString();
                var valPAYLOSS3 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates3).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS3 = valPAYLOSS3.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_3 = sumPAYLOSS3.ToString();
                var valZJGross3 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates3);
                var sumZJGross3 = valZJGross3.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount3 = sumZJGross3 == 0 ? 0 : Math.Round((sumDA_PAYLOSS3 + sumPiceGross3 + sumPAYLOSS3) / sumZJGross3, 2);
                dA_VIEW5.DT_3 = OneCount3.ToString();
                dA_VIEW6.DT_3 = (sumDA_PAYLOSS3 + sumPiceGross3 + sumPAYLOSS3).ToString();

                var Dates4 = dATE1 + "-04";
                var valGross4 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates4);
                var sumGross4 = valGross4.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_4 = sumGross4.ToString();
                var sumPiceGross4 = valGross4.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_4 = sumPiceGross4.ToString();
                var valDA_PAYLOSS4 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates4).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS4 = valDA_PAYLOSS4.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_4 = sumDA_PAYLOSS4.ToString();
                var valPAYLOSS4 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates4).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS4 = valPAYLOSS4.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_4 = sumPAYLOSS4.ToString();
                var valZJGross4 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates4);
                var sumZJGross4 = valZJGross4.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount4 = sumZJGross4 == 0 ? 0 : Math.Round((sumDA_PAYLOSS4 + sumPiceGross4 + sumPAYLOSS4) / sumZJGross4, 2);
                dA_VIEW5.DT_4 = OneCount4.ToString();
                dA_VIEW6.DT_4 = (sumDA_PAYLOSS4 + sumPiceGross4 + sumPAYLOSS4).ToString();

                var Dates5 = dATE1 + "-05";
                var valGross5 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates5);
                var sumGross5 = valGross5.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_5 = sumGross5.ToString();
                var sumPiceGross5 = valGross5.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_5 = sumPiceGross5.ToString();
                var valDA_PAYLOSS5 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates5).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS5 = valDA_PAYLOSS5.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_5 = sumDA_PAYLOSS5.ToString();
                var valPAYLOSS5 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates5).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS5 = valPAYLOSS5.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_5 = sumPAYLOSS5.ToString();
                var valZJGross5 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates5);
                var sumZJGross5 = valZJGross5.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount5 = sumZJGross5 == 0 ? 0 : Math.Round((sumDA_PAYLOSS5 + sumPiceGross5 + sumPAYLOSS5) / sumZJGross5, 2);
                dA_VIEW5.DT_5 = OneCount5.ToString();
                dA_VIEW6.DT_5 = (sumDA_PAYLOSS5 + sumPiceGross5 + sumPAYLOSS5).ToString();

                var Dates6 = dATE1 + "-06";
                var valGross6 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates6);
                var sumGross6 = valGross6.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_6 = sumGross6.ToString();
                var sumPiceGross6 = valGross6.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_6 = sumPiceGross6.ToString();
                var valDA_PAYLOSS6 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates6).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS6 = valDA_PAYLOSS6.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_6 = sumDA_PAYLOSS6.ToString();
                var valPAYLOSS6 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates6).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS6 = valPAYLOSS6.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_6 = sumPAYLOSS6.ToString();
                var valZJGross6 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates6);
                var sumZJGross6 = valZJGross6.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount6= sumZJGross6 == 0 ? 0 : Math.Round((sumDA_PAYLOSS6 + sumPiceGross6 + sumPAYLOSS6) / sumZJGross6, 2);
                dA_VIEW5.DT_6 = OneCount6.ToString();
                dA_VIEW6.DT_6 = (sumDA_PAYLOSS6 + sumPiceGross6 + sumPAYLOSS6).ToString();

                var Dates7 = dATE1 + "-07";
                var valGross7 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates7);
                var sumGross7 = valGross7.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_7 = sumGross7.ToString();
                var sumPiceGross7 = valGross7.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_7 = sumPiceGross7.ToString();
                var valDA_PAYLOSS7 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates7).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS7 = valDA_PAYLOSS7.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_7 = sumDA_PAYLOSS7.ToString();
                var valPAYLOSS7 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates7).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS7 = valPAYLOSS7.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_7 = sumPAYLOSS7.ToString();
                var valZJGross7 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates7);
                var sumZJGross7 = valZJGross7.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount7 = sumZJGross7 == 0 ? 0 : Math.Round((sumDA_PAYLOSS7 + sumPiceGross7 + sumPAYLOSS7) / sumZJGross7, 2);
                dA_VIEW5.DT_7 = OneCount7.ToString();
                dA_VIEW6.DT_7 = (sumDA_PAYLOSS7 + sumPiceGross7 + sumPAYLOSS7).ToString();

                var Dates8 = dATE1 + "-08";
                var valGross8 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates8);
                var sumGross8 = valGross8.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_8 = sumGross8.ToString();
                var sumPiceGross8 = valGross8.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_8 = sumPiceGross8.ToString();
                var valDA_PAYLOSS8 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates8).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS8 = valDA_PAYLOSS8.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_8 = sumDA_PAYLOSS8.ToString();
                var valPAYLOSS8 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates8).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS8 = valPAYLOSS8.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_8 = sumPAYLOSS8.ToString();
                var valZJGross8 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates8);
                var sumZJGross8 = valZJGross8.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount8 = sumZJGross8 == 0 ? 0 : Math.Round((sumDA_PAYLOSS8 + sumPiceGross8 + sumPAYLOSS8) / sumZJGross8, 2);
                dA_VIEW5.DT_8 = OneCount8.ToString();
                dA_VIEW6.DT_8 = (sumDA_PAYLOSS8 + sumPiceGross8 + sumPAYLOSS8).ToString();

                var Dates9 = dATE1 + "-09";
                var valGross9 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates9);
                var sumGross9 = valGross9.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_9 = sumGross9.ToString();
                var sumPiceGross9 = valGross9.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_9 = sumPiceGross9.ToString();
                var valDA_PAYLOSS9 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates9).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS9 = valDA_PAYLOSS9.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_9 = sumDA_PAYLOSS9.ToString();
                var valPAYLOSS9 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates9).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS9 = valPAYLOSS9.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_9 = sumPAYLOSS9.ToString();
                var valZJGross9 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates9);
                var sumZJGross9 = valZJGross9.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount9 = sumZJGross9 == 0 ? 0 : Math.Round((sumDA_PAYLOSS9 + sumPiceGross9 + sumPAYLOSS9) / sumZJGross9, 2);
                dA_VIEW5.DT_9 = OneCount9.ToString();
                dA_VIEW6.DT_9 = (sumDA_PAYLOSS9 + sumPiceGross9 + sumPAYLOSS9).ToString();

                var Dates10 = dATE1 + "-10";
                var valGross10 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates10);
                var sumGross10 = valGross10.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_10 = sumGross10.ToString();
                var sumPiceGross10= valGross10.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_10= sumPiceGross10.ToString();
                var valDA_PAYLOSS10 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates10).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS10 = valDA_PAYLOSS10.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_10 = sumDA_PAYLOSS10.ToString();
                var valPAYLOSS10 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates10).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS10 = valPAYLOSS10.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_10 = sumPAYLOSS10.ToString();
                var valZJGross10 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates10);
                var sumZJGross10 = valZJGross10.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount10 = sumZJGross10 == 0 ? 0 : Math.Round((sumDA_PAYLOSS10 + sumPiceGross10 + sumPAYLOSS10) / sumZJGross10, 2);
                dA_VIEW5.DT_10 = OneCount10.ToString();
                dA_VIEW6.DT_10 = (sumDA_PAYLOSS10 + sumPiceGross10 + sumPAYLOSS10).ToString();

                var Dates11 = dATE1 + "-11";
                var valGross11 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates11);
                var sumGross11 = valGross11.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_11 = sumGross11.ToString();
                var sumPiceGross11 = valGross11.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_11 = sumPiceGross11.ToString();
                var valDA_PAYLOSS11 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates11).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS11 = valDA_PAYLOSS11.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_11 = sumDA_PAYLOSS11.ToString();
                var valPAYLOSS11 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates11).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS11 = valPAYLOSS11.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_11 = sumPAYLOSS11.ToString();
                var valZJGross11 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates11);
                var sumZJGross11= valZJGross11.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount11 = sumZJGross11 == 0 ? 0 : Math.Round((sumDA_PAYLOSS11 + sumPiceGross11 + sumPAYLOSS11) / sumZJGross11, 2);
                dA_VIEW5.DT_11 = OneCount11.ToString();
                dA_VIEW6.DT_11 = (sumDA_PAYLOSS11 + sumPiceGross11 + sumPAYLOSS11).ToString();

                var Dates12 = dATE1 + "-12";
                var valGross12 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates12);
                var sumGross12 = valGross12.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_12 = sumGross12.ToString();
                var sumPiceGross12 = valGross12.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_12 = sumPiceGross12.ToString();
                var valDA_PAYLOSS12 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates12).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS12 = valDA_PAYLOSS12.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_12 = sumDA_PAYLOSS12.ToString();
                var valPAYLOSS12 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates12).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS12 = valPAYLOSS12.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_12 = sumPAYLOSS12.ToString();
                var valZJGross12 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates12);
                var sumZJGross12 = valZJGross12.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount12 = sumZJGross12 == 0 ? 0 : Math.Round((sumDA_PAYLOSS12 + sumPiceGross12+ sumPAYLOSS12) / sumZJGross12, 2);
                dA_VIEW5.DT_12 = OneCount12.ToString();
                dA_VIEW6.DT_12 = (sumDA_PAYLOSS12 + sumPiceGross12 + sumPAYLOSS12).ToString();

                var Dates13 = dATE1 + "-13";
                var valGross13 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates13);
                var sumGross13 = valGross13.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_13 = sumGross13.ToString();
                var sumPiceGross13 = valGross13.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_13 = sumPiceGross13.ToString();
                var valDA_PAYLOSS13 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates13).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS13 = valDA_PAYLOSS13.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_13 = sumDA_PAYLOSS13.ToString();
                var valPAYLOSS13 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates13).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS13 = valPAYLOSS13.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_13 = sumPAYLOSS13.ToString();
                var valZJGross13 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates13);
                var sumZJGross13 = valZJGross13.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount13 = sumZJGross13 == 0 ? 0 : Math.Round((sumDA_PAYLOSS13 + sumPiceGross13 + sumPAYLOSS13) / sumZJGross13, 2);
                dA_VIEW5.DT_13 = OneCount13.ToString();
                dA_VIEW6.DT_13 = (sumDA_PAYLOSS13 + sumPiceGross13 + sumPAYLOSS13).ToString();

                var Dates14 = dATE1 + "-14";
                var valGross14 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates14);
                var sumGross14 = valGross14.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_14 = sumGross14.ToString();
                var sumPiceGross14 = valGross14.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_14 = sumPiceGross14.ToString();
                var valDA_PAYLOSS14 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates14).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS14 = valDA_PAYLOSS14.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_14 = sumDA_PAYLOSS14.ToString();
                var valPAYLOSS14 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates14).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS14 = valPAYLOSS14.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_14 = sumPAYLOSS14.ToString();
                var valZJGross14 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates14);
                var sumZJGross14 = valZJGross14.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount14 = sumZJGross14 == 0 ? 0 : Math.Round((sumDA_PAYLOSS14 + sumPiceGross14 + sumPAYLOSS14) / sumZJGross14, 2);
                dA_VIEW5.DT_14 = OneCount14.ToString();
                dA_VIEW6.DT_14 = (sumDA_PAYLOSS14 + sumPiceGross14 + sumPAYLOSS14).ToString();

                var Dates15 = dATE1 + "-15";
                var valGross15 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates15);
                var sumGross15 = valGross15.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_15 = sumGross15.ToString();
                var sumPiceGross15 = valGross15.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_15 = sumPiceGross15.ToString();
                var valDA_PAYLOSS15 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates15).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS15 = valDA_PAYLOSS15.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_15 = sumDA_PAYLOSS15.ToString();
                var valPAYLOSS15 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates15).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS15 = valPAYLOSS15.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_15= sumPAYLOSS15.ToString();
                var valZJGross15 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates15);
                var sumZJGross15 = valZJGross15.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount15 = sumZJGross15 == 0 ? 0 : Math.Round((sumDA_PAYLOSS15 + sumPiceGross15 + sumPAYLOSS15) / sumZJGross15, 2);
                dA_VIEW5.DT_15 = OneCount15.ToString();
                dA_VIEW6.DT_15 = (sumDA_PAYLOSS15 + sumPiceGross15 + sumPAYLOSS15).ToString();

                var Dates16 = dATE1 + "-16";
                var valGross16 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates16);
                var sumGross16 = valGross16.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_16 = sumGross1.ToString();
                var sumPiceGross16 = valGross16.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_16 = sumPiceGross16.ToString();
                var valDA_PAYLOSS16 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates16).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS16 = valDA_PAYLOSS16.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_16 = sumDA_PAYLOSS16.ToString();
                var valPAYLOSS16 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates16).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS16 = valPAYLOSS16.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_16 = sumPAYLOSS16.ToString();
                var valZJGross16 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates16);
                var sumZJGross16 = valZJGross16.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount16 =sumZJGross16 == 0 ? 0 : Math.Round((sumDA_PAYLOSS16 + sumPiceGross16 + sumPAYLOSS16) / sumZJGross16, 2);
                dA_VIEW5.DT_16 = OneCount16.ToString();
                dA_VIEW6.DT_16 = (sumDA_PAYLOSS16 + sumPiceGross16 + sumPAYLOSS16).ToString();

                var Dates17 = dATE1 + "-17";
                var valGross17 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates17);
                var sumGross17 = valGross17.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_17 = sumGross17.ToString();
                var sumPiceGross17 = valGross17.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_17 = sumPiceGross17.ToString();
                var valDA_PAYLOSS17 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates17).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS17 = valDA_PAYLOSS17.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_17 = sumDA_PAYLOSS17.ToString();
                var valPAYLOSS17 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates17).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS17 = valPAYLOSS17.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_17 = sumPAYLOSS17.ToString();
                var valZJGross17 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates17);
                var sumZJGross17 = valZJGross17.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount17 = sumZJGross17 == 0 ? 0 : Math.Round((sumDA_PAYLOSS17 + sumPiceGross17 + sumPAYLOSS17) / sumZJGross17, 2);
                dA_VIEW5.DT_17 = OneCount17.ToString();
                dA_VIEW6.DT_17 = (sumDA_PAYLOSS17 + sumPiceGross17 + sumPAYLOSS17).ToString();

                var Dates18 = dATE1 + "-18";
                var valGross18 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates18);
                var sumGross18 = valGross18.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_18 = sumGross18.ToString();
                var sumPiceGross18 = valGross18.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_18 = sumPiceGross18.ToString();
                var valDA_PAYLOSS18 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates18).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS18 = valDA_PAYLOSS18.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_18 = sumDA_PAYLOSS18.ToString();
                var valPAYLOSS18 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates18).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS18 = valPAYLOSS18.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_18 = sumPAYLOSS18.ToString();
                var valZJGross18 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates18);
                var sumZJGross18 = valZJGross18.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount18 = sumZJGross18 == 0 ? 0 : Math.Round((sumDA_PAYLOSS18 + sumPiceGross18 + sumPAYLOSS18) / sumZJGross18, 2);
                dA_VIEW5.DT_18 = OneCount18.ToString();
                dA_VIEW6.DT_18 = (sumDA_PAYLOSS18 + sumPiceGross18 + sumPAYLOSS18).ToString();

                var Dates19 = dATE1 + "-19";
                var valGross19 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates19);
                var sumGross19 = valGross19.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_19 = sumGross19.ToString();
                var sumPiceGross19 = valGross19.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_19 = sumPiceGross19.ToString();
                var valDA_PAYLOSS19 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates19).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS19 = valDA_PAYLOSS19.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_19 = sumDA_PAYLOSS19.ToString();
                var valPAYLOSS19 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates19).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS19 = valPAYLOSS19.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_19 = sumPAYLOSS19.ToString();
                var valZJGross19 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates19);
                var sumZJGross19 = valZJGross19.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount19 = sumZJGross19 == 0 ? 0 : Math.Round((sumDA_PAYLOSS19 + sumPiceGross19+ sumPAYLOSS19) / sumZJGross19, 2);
                dA_VIEW5.DT_19 = OneCount19.ToString();
                dA_VIEW6.DT_19 = (sumDA_PAYLOSS19 + sumPiceGross19 + sumPAYLOSS19).ToString();

                var Dates20 = dATE1 + "-20";
                var valGross20 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates20);
                var sumGross20 = valGross20.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_20 = sumGross20.ToString();
                var sumPiceGross20 = valGross20.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_20= sumPiceGross20.ToString();
                var valDA_PAYLOSS20 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates20).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS20 = valDA_PAYLOSS20.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_20 = sumDA_PAYLOSS20.ToString();
                var valPAYLOSS20 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates20).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS20 = valPAYLOSS20.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_20 = sumPAYLOSS20.ToString();
                var valZJGross20 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates20);
                var sumZJGross20 = valZJGross20.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount20 =sumZJGross20 == 0 ? 0 : Math.Round((sumDA_PAYLOSS20 + sumPiceGross20 + sumPAYLOSS20) / sumZJGross20, 2);
                dA_VIEW5.DT_20 = OneCount20.ToString();
                dA_VIEW6.DT_20 = (sumDA_PAYLOSS20 + sumPiceGross20 + sumPAYLOSS20).ToString();

                var Dates21 = dATE1 + "-21";
                var valGross21 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates21);
                var sumGross21 = valGross21.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_21 = sumGross21.ToString();
                var sumPiceGross21 = valGross21.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_21 = sumPiceGross21.ToString();
                var valDA_PAYLOSS21 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates21).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS21 = valDA_PAYLOSS21.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_21 = sumDA_PAYLOSS21.ToString();
                var valPAYLOSS21 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates21).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS21 = valPAYLOSS21.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_21 = sumPAYLOSS21.ToString();
                var valZJGross21 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates21);
                var sumZJGross21 = valZJGross21.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount21 = sumZJGross21 == 0 ? 0 : Math.Round((sumDA_PAYLOSS21 + sumPiceGross21 + sumPAYLOSS21) / sumZJGross21, 2);
                dA_VIEW5.DT_21 = OneCount21.ToString();
                dA_VIEW6.DT_21 = (sumDA_PAYLOSS21 + sumPiceGross21 + sumPAYLOSS21).ToString();

                var Dates22 = dATE1 + "-22";
                var valGross22 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates22);
                var sumGross22 = valGross22.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_22 = sumGross22.ToString();
                var sumPiceGross22 = valGross22.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_22 = sumPiceGross22.ToString();
                var valDA_PAYLOSS22 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates22).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS22 = valDA_PAYLOSS22.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_22 = sumDA_PAYLOSS22.ToString();
                var valPAYLOSS22 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates22).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS22 = valPAYLOSS22.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_22 = sumPAYLOSS22.ToString();
                var valZJGross22 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates22);
                var sumZJGross22 = valZJGross22.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount22 = sumZJGross22 == 0 ? 0 : Math.Round((sumDA_PAYLOSS22 + sumPiceGross22 + sumPAYLOSS22) / sumZJGross22, 2);
                dA_VIEW5.DT_22= OneCount22.ToString();
                dA_VIEW6.DT_22 = (sumDA_PAYLOSS22 + sumPiceGross22 + sumPAYLOSS22).ToString();

                var Dates23 = dATE1 + "-23";
                var valGross23 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates23);
                var sumGross23 = valGross23.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_23 = sumGross23.ToString();
                var sumPiceGross23 = valGross23.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_23 = sumPiceGross23.ToString();
                var valDA_PAYLOSS23 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates23).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS23 = valDA_PAYLOSS23.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_23 = sumDA_PAYLOSS23.ToString();
                var valPAYLOSS23 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates23).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS23 = valPAYLOSS23.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_23 = sumPAYLOSS23.ToString();
                var valZJGross23 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates23);
                var sumZJGross23 = valZJGross23.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount23= sumZJGross23 == 0 ? 0 : Math.Round((sumDA_PAYLOSS23 + sumPiceGross23 + sumPAYLOSS23) / sumZJGross23, 2);
                dA_VIEW5.DT_23 = OneCount23.ToString();
                dA_VIEW6.DT_23 = (sumDA_PAYLOSS23 + sumPiceGross23 + sumPAYLOSS23).ToString();

                var Dates24 = dATE1 + "-24";
                var valGross24 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates24);
                var sumGross24 = valGross24.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_24 = sumGross24.ToString();
                var sumPiceGross24 = valGross24.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_24 = sumPiceGross24.ToString();
                var valDA_PAYLOSS24 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates24).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS24 = valDA_PAYLOSS24.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_24 = sumDA_PAYLOSS24.ToString();
                var valPAYLOSS24 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates24).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS24 = valPAYLOSS24.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_24 = sumPAYLOSS24.ToString();
                var valZJGross24 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates24);
                var sumZJGross24 = valZJGross24.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount24 = sumZJGross24 == 0 ? 0 : Math.Round((sumDA_PAYLOSS24 + sumPiceGross24 + sumPAYLOSS24) / sumZJGross24, 2);
                dA_VIEW5.DT_24 = OneCount24.ToString();
                dA_VIEW6.DT_24 = (sumDA_PAYLOSS24 + sumPiceGross24 + sumPAYLOSS24).ToString();

                var Dates25 = dATE1 + "-25";
                var valGross25 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates25);
                var sumGross25 = valGross25.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_25 = sumGross25.ToString();
                var sumPiceGross25 = valGross25.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_25 = sumPiceGross25.ToString();
                var valDA_PAYLOSS25 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates25).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS25 = valDA_PAYLOSS25.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_25 = sumDA_PAYLOSS25.ToString();
                var valPAYLOSS25 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates25).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS25 = valPAYLOSS25.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_25 = sumPAYLOSS25.ToString();
                var valZJGross25 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates25);
                var sumZJGross25 = valZJGross25.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount25 = sumZJGross25 == 0 ? 0 : Math.Round((sumDA_PAYLOSS25 + sumPiceGross25 + sumPAYLOSS25) / sumZJGross25, 2);
                dA_VIEW5.DT_25 = OneCount25.ToString();
                dA_VIEW6.DT_25 = (sumDA_PAYLOSS25 + sumPiceGross25 + sumPAYLOSS25).ToString();

                var Dates26 = dATE1 + "-26";
                var valGross26 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates26);
                var sumGross26 = valGross26.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_26 = sumGross26.ToString();
                var sumPiceGross26 = valGross26.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_26 = sumPiceGross26.ToString();
                var valDA_PAYLOSS26 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates26).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS26 = valDA_PAYLOSS26.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_26 = sumDA_PAYLOSS26.ToString();
                var valPAYLOSS26 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates26).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS26 = valPAYLOSS26.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_26 = sumPAYLOSS26.ToString();
                var valZJGross26 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates26);
                var OneCount26 = valZJGross26.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)) == 0 ? 0 : Math.Round((sumDA_PAYLOSS26 + sumPiceGross26 + sumPAYLOSS26) / valZJGross26.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)), 2);
                dA_VIEW5.DT_26 = OneCount26.ToString();
                dA_VIEW6.DT_26 = (sumDA_PAYLOSS26 + sumPiceGross26 + sumPAYLOSS26).ToString();

                var Dates27 = dATE1 + "-27";
                var valGross27 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates27);
                var sumGross27 = valGross27.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_27 = sumGross27.ToString();
                var sumPiceGross27 = valGross27.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_27 = sumPiceGross27.ToString();
                var valDA_PAYLOSS27 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates27).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS27 = valDA_PAYLOSS27.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_27 = sumDA_PAYLOSS27.ToString();
                var valPAYLOSS27 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates27).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS27 = valPAYLOSS27.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_27 = sumPAYLOSS27.ToString();
                var valZJGross27 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates27);
                var sumZJGross27 = valZJGross27.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount27 = sumZJGross27 == 0 ? 0 : Math.Round((sumDA_PAYLOSS27 + sumPiceGross27 + sumPAYLOSS27) / sumZJGross27, 2);
                dA_VIEW5.DT_27 = OneCount27.ToString();
                dA_VIEW6.DT_27 = (sumDA_PAYLOSS27 + sumPiceGross27 + sumPAYLOSS27).ToString();

                var Dates28 = dATE1 + "-28";
                var valGross28 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates28);
                var sumGross28 = valGross28.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_28 = sumGross28.ToString();
                var sumPiceGross28 = valGross28.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_28 = sumPiceGross28.ToString();
                var valDA_PAYLOSS28 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates28).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS28 = valDA_PAYLOSS28.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_28 = sumDA_PAYLOSS28.ToString();
                var valPAYLOSS28 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates28).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS28 = valPAYLOSS28.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_28 = sumPAYLOSS28.ToString();
                var valZJGross28 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates28);
                var sumZJGross28 = valZJGross28.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount28 = sumZJGross28 == 0 ? 0 : Math.Round((sumDA_PAYLOSS28 + sumPiceGross28 + sumPAYLOSS28) / sumZJGross28, 2);
                dA_VIEW5.DT_28 = OneCount28.ToString();
                dA_VIEW6.DT_28 = (sumDA_PAYLOSS28 + sumPiceGross28 + sumPAYLOSS28).ToString();

                var Dates29 = dATE1 + "-29";
                var valGross29 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates29);
                var sumGross29 = valGross29.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_29 = sumGross29.ToString();
                var sumPiceGross29 = valGross29.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_29 = sumPiceGross29.ToString();
                var valDA_PAYLOSS29 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates29).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS29 = valDA_PAYLOSS29.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_29 = sumDA_PAYLOSS29.ToString();
                var valPAYLOSS29 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates29).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS29 = valPAYLOSS29.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_29 = sumPAYLOSS29.ToString();
                var valZJGross29 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates29);
                var sumZJGross29 = valZJGross29.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount29 = sumZJGross29 == 0 ? 0 : Math.Round((sumDA_PAYLOSS29 + sumPiceGross29 + sumPAYLOSS29) / sumZJGross29, 2);
                dA_VIEW5.DT_29 = OneCount29.ToString();
                dA_VIEW6.DT_29 = (sumDA_PAYLOSS29 + sumPiceGross29 + sumPAYLOSS29).ToString();

                var Dates30 = dATE1 + "-30";
                var valGross30 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates30);
                var sumGross30 = valGross30.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_30 = sumGross30.ToString();
                var sumPiceGross30 = valGross30.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_30 = sumPiceGross30.ToString();
                var valDA_PAYLOSS30 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates30).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS30 = valDA_PAYLOSS30.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_30 = sumDA_PAYLOSS30.ToString();
                var valPAYLOSS30 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates30).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS30 = valPAYLOSS30.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_30 = sumPAYLOSS30.ToString();
                var valZJGross30 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates30);
                var sumZJGross30 = valZJGross30.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount30 = sumZJGross30 == 0 ? 0 : Math.Round((sumDA_PAYLOSS30 + sumPiceGross30 + sumPAYLOSS30) / sumZJGross30, 2);
                dA_VIEW5.DT_30 = OneCount30.ToString();
                dA_VIEW6.DT_30 = (sumDA_PAYLOSS30 + sumPiceGross30 + sumPAYLOSS30).ToString();

                var Dates31 = dATE1 + "-31";
                var valGross31 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates31);
                var sumGross31 = valGross31.Sum(x => x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK));
                dA_VIEW1.DT_31 = sumGross31.ToString();
                var sumPiceGross31 = valGross31.Sum(x => (x.DT_ONWORK == "" ? 0 : Convert.ToDouble(x.DT_ONWORK)) * (x.DT_DIRECTHOUR== "" ? HourPice : Convert.ToDouble(x.DT_DIRECTHOUR)) * (x.DT_PERSONHOUR == "" ? 0 : Convert.ToDouble(x.DT_PERSONHOUR)));
                dA_VIEW2.DT_31 = sumPiceGross31.ToString();
                var valDA_PAYLOSS31 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates31).Where(x => x.DT_NAME == "生产辅料消耗" || x.DT_NAME == "低值易耗消耗");
                var sumDA_PAYLOSS31 = valDA_PAYLOSS31.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW3.DT_31 = sumDA_PAYLOSS31.ToString();
                var valPAYLOSS31 = _sqlDbContext.DA_PAYLOSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates31).Where(x => x.DT_NAME == "超损物料损耗");
                var sumPAYLOSS31 = valPAYLOSS31.Sum(x => (x.DT_LOSS == "" ? 0 : Convert.ToDouble(x.DT_LOSS)) * (x.DT_PRICE == "" ? 0 : Convert.ToDouble(x.DT_PRICE)));
                dA_VIEW4.DT_31 = sumPAYLOSS31.ToString();
                var valZJGross31 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM-dd") == Dates31);
                var sumZJGross31 = valZJGross31.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                var OneCount31 = sumZJGross31 == 0 ? 0 : Math.Round((sumDA_PAYLOSS31 + sumPiceGross31 + sumPAYLOSS31) / sumZJGross31, 2);
                dA_VIEW5.DT_31 = OneCount31.ToString();
                dA_VIEW6.DT_31 = (sumDA_PAYLOSS31 + sumPiceGross31 + sumPAYLOSS31).ToString();

                dA_VIEW1.TOTAL = (sumGross1+ sumGross2 + sumGross3 + sumGross4 + sumGross5 + sumGross6 + sumGross7 + sumGross8 + sumGross9 + sumGross10 + sumGross11 + sumGross12 + sumGross13 + sumGross14 + sumGross15 + sumGross16 + sumGross17 + sumGross18 + sumGross19 + sumGross20 + sumGross21 + sumGross22 + sumGross23 + sumGross24 + sumGross25 + sumGross26 + sumGross27 + sumGross28 + sumGross29 + sumGross30 + sumGross31).ToString();
                dA_VIEW2.TOTAL = (sumPiceGross1+ sumPiceGross2 + sumPiceGross3 + sumPiceGross4 + sumPiceGross5 + sumPiceGross6 + sumPiceGross7 + sumPiceGross8 + sumPiceGross9 + sumPiceGross10 + sumPiceGross11 + sumPiceGross12 + sumPiceGross13 + sumPiceGross14 + sumPiceGross15 + sumPiceGross16 + sumPiceGross17 + sumPiceGross18 + sumPiceGross19 + sumPiceGross20 + sumPiceGross21 + sumPiceGross22 + sumPiceGross23 + sumPiceGross24 + sumPiceGross25 + sumPiceGross26 + sumPiceGross27 + sumPiceGross28 + sumPiceGross29 + sumPiceGross30 + sumPiceGross31).ToString();
                dA_VIEW3.TOTAL =(sumDA_PAYLOSS1+ sumDA_PAYLOSS2 + sumDA_PAYLOSS3 + sumDA_PAYLOSS4 + sumDA_PAYLOSS5 + sumDA_PAYLOSS6 + sumDA_PAYLOSS7 + sumDA_PAYLOSS8 + sumDA_PAYLOSS9 + sumDA_PAYLOSS10 + sumDA_PAYLOSS11 + sumDA_PAYLOSS12 + sumDA_PAYLOSS13 + sumDA_PAYLOSS14 + sumDA_PAYLOSS15 + sumDA_PAYLOSS16 + sumDA_PAYLOSS17 + sumDA_PAYLOSS18 + sumDA_PAYLOSS19 + sumDA_PAYLOSS20 + sumDA_PAYLOSS21 + sumDA_PAYLOSS22 + sumDA_PAYLOSS23 + sumDA_PAYLOSS24 + sumDA_PAYLOSS25 + sumDA_PAYLOSS26 + sumDA_PAYLOSS27 + sumDA_PAYLOSS28 + sumDA_PAYLOSS29 + sumDA_PAYLOSS30 + sumDA_PAYLOSS31).ToString();
                dA_VIEW4.TOTAL=(sumPAYLOSS1+ sumPAYLOSS2 + sumPAYLOSS3 + sumPAYLOSS4 + sumPAYLOSS5 + sumPAYLOSS6 + sumPAYLOSS7 + sumPAYLOSS8 + sumPAYLOSS9 + sumPAYLOSS10 + sumPAYLOSS11 + sumPAYLOSS12 + sumPAYLOSS13 + sumPAYLOSS14 + sumPAYLOSS15 + sumPAYLOSS16 + sumPAYLOSS17 + sumPAYLOSS18 + sumPAYLOSS19 + sumPAYLOSS20 + sumPAYLOSS21 + sumPAYLOSS22 + sumPAYLOSS23 + sumPAYLOSS24 + sumPAYLOSS25 + sumPAYLOSS26 + sumPAYLOSS27 + sumPAYLOSS28 + sumPAYLOSS29 + sumPAYLOSS30 + sumPAYLOSS31).ToString();
                dA_VIEW5.TOTAL = Math.Round(OneCount1 + OneCount2 + OneCount3 + OneCount4 + OneCount5 + OneCount6 + OneCount7 + OneCount8 + OneCount9 + OneCount10 + OneCount11 + OneCount12 + OneCount13 + OneCount14 + OneCount15 + OneCount16 + OneCount17 + OneCount18 + OneCount19 + OneCount20 + OneCount21 + OneCount22 + OneCount23 + OneCount24 + OneCount25 + OneCount26 + OneCount27 + OneCount28 + OneCount29 + OneCount30 + OneCount31,2).ToString();
                dA_VIEW6.TOTAL = Math.Round(Convert.ToDouble(dA_VIEW2.TOTAL)+ Convert.ToDouble(dA_VIEW3.TOTAL)+ Convert.ToDouble(dA_VIEW4.TOTAL), 2).ToString();
                dA_VIEWs.Add(dA_VIEW1);
                dA_VIEWs.Add(dA_VIEW2);
                dA_VIEWs.Add(dA_VIEW3);
                dA_VIEWs.Add(dA_VIEW4);
                dA_VIEWs.Add(dA_VIEW5);
                dA_VIEWs1.Add(dA_VIEW6);
            }
            #region
            DA_VIEW dA_VIEW11 = new DA_VIEW();//间接人员费用(平摊):
            DA_VIEW dA_VIEW12 = new DA_VIEW();//公摊费用支出合计:
            DA_VIEW dA_VIEW13 = new DA_VIEW();//平台部门费用支出合计: 
            dA_VIEW11.TYPES = "间接人员费用(平摊):";
            dA_VIEW12.TYPES = "公摊费用支出合计:";
            dA_VIEW13.TYPES = "平台部门费用支出合计:"; 
            dA_VIEW11.TYPES1 = ""; 
            dA_VIEW12.TYPES1 = "";
            dA_VIEW13.TYPES1 = "";
            dA_VIEW11.MODEL = ""; 
            dA_VIEW12.MODEL = "";
            dA_VIEW13.MODEL = "";
            DateTime date1 = Convert.ToDateTime(dATE1);
            int pYear = date1.Year;
            int pMonth = date1.Month; 
            int vMax = DateTime.DaysInMonth(pYear, pMonth);
            var paymonth = _sqlDbContext.DA_PAYMONTHs.Where(x => x.DT_DATE.Value.ToString("yyyy-MM") == dATE1);
            var person = paymonth.Where(x => x.DT_NAME == "间接人员月薪");
            double personTal = person.Sum(x => x.DT_EXPEND == "" ? 0 : Convert.ToDouble(x.DT_EXPEND));
            var gongtang= paymonth.Where(x => x.DT_NAME == "公摊费用");
            double gongtangTal = gongtang.Sum(x => x.DT_EXPEND == "" ? 0 : Convert.ToDouble(x.DT_EXPEND));
            var pingtai= paymonth.Where(x => x.DT_NAME == "平台部门费用");
            double pingtaiTal = pingtai.Sum(x => x.DT_EXPEND == "" ? 0 : Convert.ToDouble(x.DT_EXPEND)); 
            DateTime firstDayOfThisMonth = new DateTime(pYear, pMonth, 1);
            DateTime lastDayOfThisMonth = new DateTime(pYear, pMonth, DateTime.DaysInMonth(pYear, pMonth));
            int TotalWeeks1 = TotalWeeks(firstDayOfThisMonth, lastDayOfThisMonth, DayOfWeek.Sunday);
            int OnWorkDay = vMax - TotalWeeks1;
            double Ctperson = Math.Round(personTal/ OnWorkDay, 2);
            double Ctgongtang = Math.Round(gongtangTal / OnWorkDay, 2);
            double Ctpingtai = Math.Round(pingtaiTal / OnWorkDay, 2); 
            var Tdate1 = Convert.ToDateTime(dATE1 + "-01");
            switch (Tdate1.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_1 = "0";
                    dA_VIEW12.DT_1 = "0";
                    dA_VIEW13.DT_1 = "0";
                    break;
                default:
                    dA_VIEW11.DT_1 = Ctperson.ToString();
                    dA_VIEW12.DT_1 = Ctgongtang.ToString();
                    dA_VIEW13.DT_1 = Ctpingtai.ToString();
                    break;
            }  
            var Tdate2 = Convert.ToDateTime(dATE1 + "-02");
            switch (Tdate2.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_2 = "0";
                    dA_VIEW12.DT_2 = "0";
                    dA_VIEW13.DT_2 = "0";
                    break;
                default:
                    dA_VIEW11.DT_2 = Ctperson.ToString();
                    dA_VIEW12.DT_2 = Ctgongtang.ToString();
                    dA_VIEW13.DT_2 = Ctpingtai.ToString();
                    break;
            } 
            var Tdate3 = Convert.ToDateTime(dATE1 + "-03");
            switch (Tdate3.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_3 = "0";
                    dA_VIEW12.DT_3 = "0";
                    dA_VIEW13.DT_3 = "0";
                    break;
                default:
                    dA_VIEW11.DT_3 = Ctperson.ToString();
                    dA_VIEW12.DT_3 = Ctgongtang.ToString();
                    dA_VIEW13.DT_3 = Ctpingtai.ToString();
                    break;
            } 
            var Tdate4 = Convert.ToDateTime(dATE1 + "-04");
            switch (Tdate4.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_4 = "0";
                    dA_VIEW12.DT_4 = "0";
                    dA_VIEW13.DT_4 = "0";
                    break;
                default:
                    dA_VIEW11.DT_4 = Ctperson.ToString();
                    dA_VIEW12.DT_4 = Ctgongtang.ToString();
                    dA_VIEW13.DT_4 = Ctpingtai.ToString();
                    break;
            } 
            var Tdate5 = Convert.ToDateTime(dATE1 + "-05");
            switch (Tdate5.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_5 = "0";
                    dA_VIEW12.DT_5 = "0";
                    dA_VIEW13.DT_5 = "0";
                    break;
                default:
                    dA_VIEW11.DT_5 = Ctperson.ToString();
                    dA_VIEW12.DT_5 = Ctgongtang.ToString();
                    dA_VIEW13.DT_5 = Ctpingtai.ToString();
                    break;
            } 
            var Tdate6 = Convert.ToDateTime(dATE1 + "-06");
            switch (Tdate6.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_6 = "0";
                    dA_VIEW12.DT_6 = "0";
                    dA_VIEW13.DT_6 = "0";
                    break;
                default:
                    dA_VIEW11.DT_6 = Ctperson.ToString();
                    dA_VIEW12.DT_6 = Ctgongtang.ToString();
                    dA_VIEW13.DT_6 = Ctpingtai.ToString();
                    break;
            } 
            var Tdate7 = Convert.ToDateTime(dATE1 + "-07");
            switch (Tdate7.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_7 = "0";
                    dA_VIEW12.DT_7 = "0";
                    dA_VIEW13.DT_7 = "0";
                    break;
                default:
                    dA_VIEW11.DT_7 = Ctperson.ToString();
                    dA_VIEW12.DT_7 = Ctgongtang.ToString();
                    dA_VIEW13.DT_7 = Ctpingtai.ToString();
                    break;
            } 
            var Tdate8 = Convert.ToDateTime(dATE1 + "-08");
            switch (Tdate8.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_8 = "0";
                    dA_VIEW12.DT_8 = "0";
                    dA_VIEW13.DT_8 = "0";
                    break;
                default:
                    dA_VIEW11.DT_8 = Ctperson.ToString();
                    dA_VIEW12.DT_8 = Ctgongtang.ToString();
                    dA_VIEW13.DT_8 = Ctpingtai.ToString();
                    break;
            } 
            var Tdate9= Convert.ToDateTime(dATE1 + "-09");
            switch (Tdate9.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_9 = "0";
                    dA_VIEW12.DT_9 = "0";
                    dA_VIEW13.DT_9 = "0";
                    break;
                default:
                    dA_VIEW11.DT_9 = Ctperson.ToString();
                    dA_VIEW12.DT_9 = Ctgongtang.ToString();
                    dA_VIEW13.DT_9 = Ctpingtai.ToString();
                    break;
            } 
            var Tdate10 = Convert.ToDateTime(dATE1 + "-10");
            switch (Tdate10.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_10 = "0";
                    dA_VIEW12.DT_10 = "0";
                    dA_VIEW13.DT_10 = "0";
                    break;
                default:
                    dA_VIEW11.DT_10 = Ctperson.ToString();
                    dA_VIEW12.DT_10 = Ctgongtang.ToString();
                    dA_VIEW13.DT_10 = Ctpingtai.ToString();
                    break;
            } 
            var Tdate11 = Convert.ToDateTime(dATE1 + "-11");
            switch (Tdate11.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_11 = "0";
                    dA_VIEW12.DT_11 = "0";
                    dA_VIEW13.DT_11 = "0";
                    break;
                default:
                    dA_VIEW11.DT_11 = Ctperson.ToString();
                    dA_VIEW12.DT_11 = Ctgongtang.ToString();
                    dA_VIEW13.DT_11 = Ctpingtai.ToString();
                    break;
            } 
            var Tdate12 = Convert.ToDateTime(dATE1 + "-12");
            switch (Tdate12.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_12 = "0";
                    dA_VIEW12.DT_12 = "0";
                    dA_VIEW13.DT_12 = "0";
                    break;
                default:
                    dA_VIEW11.DT_12 = Ctperson.ToString();
                    dA_VIEW12.DT_12 = Ctgongtang.ToString();
                    dA_VIEW13.DT_12 = Ctpingtai.ToString();
                    break;
            }
            var Tdate13 = Convert.ToDateTime(dATE1 + "-13");
            switch (Tdate13.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_13 = "0";
                    dA_VIEW12.DT_13 = "0";
                    dA_VIEW13.DT_13 = "0";
                    break;
                default:
                    dA_VIEW11.DT_13 = Ctperson.ToString();
                    dA_VIEW12.DT_13 = Ctgongtang.ToString();
                    dA_VIEW13.DT_13 = Ctpingtai.ToString();
                    break;
            }
            var Tdate14 = Convert.ToDateTime(dATE1 + "-14");
            switch (Tdate14.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_14 = "0";
                    dA_VIEW12.DT_14 = "0";
                    dA_VIEW13.DT_14 = "0";
                    break;
                default:
                    dA_VIEW11.DT_14 = Ctperson.ToString();
                    dA_VIEW12.DT_14= Ctgongtang.ToString();
                    dA_VIEW13.DT_14 = Ctpingtai.ToString();
                    break;
            }
            var Tdate15 = Convert.ToDateTime(dATE1 + "-15");
            switch (Tdate15.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_15 = "0";
                    dA_VIEW12.DT_15 = "0";
                    dA_VIEW13.DT_15 = "0";
                    break;
                default:
                    dA_VIEW11.DT_15 = Ctperson.ToString();
                    dA_VIEW12.DT_15 = Ctgongtang.ToString();
                    dA_VIEW13.DT_15 = Ctpingtai.ToString();
                    break;
            }
            var Tdate16 = Convert.ToDateTime(dATE1 + "-16");
            switch (Tdate16.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_16 = "0";
                    dA_VIEW12.DT_16 = "0";
                    dA_VIEW13.DT_16 = "0";
                    break;
                default:
                    dA_VIEW11.DT_16 = Ctperson.ToString();
                    dA_VIEW12.DT_16 = Ctgongtang.ToString();
                    dA_VIEW13.DT_16 = Ctpingtai.ToString();
                    break;
            } 
            var Tdate17 = Convert.ToDateTime(dATE1 + "-17");
            switch (Tdate17.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_17 = "0";
                    dA_VIEW12.DT_17 = "0";
                    dA_VIEW13.DT_17 = "0";
                    break;
                default:
                    dA_VIEW11.DT_17 = Ctperson.ToString();
                    dA_VIEW12.DT_17 = Ctgongtang.ToString();
                    dA_VIEW13.DT_17 = Ctpingtai.ToString();
                    break;
            }
            var Tdate18 = Convert.ToDateTime(dATE1 + "-18");
            switch (Tdate18.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_18 = "0";
                    dA_VIEW12.DT_18 = "0";
                    dA_VIEW13.DT_18 = "0";
                    break;
                default:
                    dA_VIEW11.DT_18 = Ctperson.ToString();
                    dA_VIEW12.DT_18 = Ctgongtang.ToString();
                    dA_VIEW13.DT_18 = Ctpingtai.ToString();
                    break;
            }
            var Tdate19 = Convert.ToDateTime(dATE1 + "-19");
            switch (Tdate19.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_19 = "0";
                    dA_VIEW12.DT_19 = "0";
                    dA_VIEW13.DT_19 = "0";
                    break;
                default:
                    dA_VIEW11.DT_19 = Ctperson.ToString();
                    dA_VIEW12.DT_19 = Ctgongtang.ToString();
                    dA_VIEW13.DT_19 = Ctpingtai.ToString();
                    break;
            } 
            var Tdate20 = Convert.ToDateTime(dATE1 + "-20");
            switch (Tdate20.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_20 = "0";
                    dA_VIEW12.DT_20 = "0";
                    dA_VIEW13.DT_20 = "0";
                    break;
                default:
                    dA_VIEW11.DT_20 = Ctperson.ToString();
                    dA_VIEW12.DT_20 = Ctgongtang.ToString();
                    dA_VIEW13.DT_20 = Ctpingtai.ToString();
                    break;
            }
            var Tdate21 = Convert.ToDateTime(dATE1 + "-21");
            switch (Tdate21.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_21 = "0";
                    dA_VIEW12.DT_21 = "0";
                    dA_VIEW13.DT_21 = "0";
                    break;
                default:
                    dA_VIEW11.DT_21 = Ctperson.ToString();
                    dA_VIEW12.DT_21 = Ctgongtang.ToString();
                    dA_VIEW13.DT_21 = Ctpingtai.ToString();
                    break;
            }
            var Tdate22 = Convert.ToDateTime(dATE1 + "-22");
            switch (Tdate22.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_22 = "0";
                    dA_VIEW12.DT_22 = "0";
                    dA_VIEW13.DT_22 = "0";
                    break;
                default:
                    dA_VIEW11.DT_22 = Ctperson.ToString();
                    dA_VIEW12.DT_22 = Ctgongtang.ToString();
                    dA_VIEW13.DT_22 = Ctpingtai.ToString();
                    break;
            }
            var Tdate23 = Convert.ToDateTime(dATE1 + "-23");
            switch (Tdate23.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_23 = "0";
                    dA_VIEW12.DT_23 = "0";
                    dA_VIEW13.DT_23 = "0";
                    break;
                default:
                    dA_VIEW11.DT_23 = Ctperson.ToString();
                    dA_VIEW12.DT_23 = Ctgongtang.ToString();
                    dA_VIEW13.DT_23 = Ctpingtai.ToString();
                    break;
            }
            var Tdate24 = Convert.ToDateTime(dATE1 + "-24");
            switch (Tdate24.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_24 = "0";
                    dA_VIEW12.DT_24 = "0";
                    dA_VIEW13.DT_24 = "0";
                    break;
                default:
                    dA_VIEW11.DT_24= Ctperson.ToString();
                    dA_VIEW12.DT_24 = Ctgongtang.ToString();
                    dA_VIEW13.DT_24= Ctpingtai.ToString();
                    break;
            }
            var Tdate25 = Convert.ToDateTime(dATE1 + "-25");
            switch (Tdate25.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_25 = "0";
                    dA_VIEW12.DT_25 = "0";
                    dA_VIEW13.DT_25 = "0";
                    break;
                default:
                    dA_VIEW11.DT_25 = Ctperson.ToString();
                    dA_VIEW12.DT_25 = Ctgongtang.ToString();
                    dA_VIEW13.DT_25 = Ctpingtai.ToString();
                    break;
            }
            var Tdate26 = Convert.ToDateTime(dATE1 + "-26");
            switch (Tdate26.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_26 = "0";
                    dA_VIEW12.DT_26 = "0";
                    dA_VIEW13.DT_26 = "0";
                    break;
                default:
                    dA_VIEW11.DT_26 = Ctperson.ToString();
                    dA_VIEW12.DT_26 = Ctgongtang.ToString();
                    dA_VIEW13.DT_26 = Ctpingtai.ToString();
                    break;
            }
            var Tdate27 = Convert.ToDateTime(dATE1 + "-27");
            switch (Tdate27.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_27 = "0";
                    dA_VIEW12.DT_27 = "0";
                    dA_VIEW13.DT_27 = "0";
                    break;
                default:
                    dA_VIEW11.DT_27 = Ctperson.ToString();
                    dA_VIEW12.DT_27 = Ctgongtang.ToString();
                    dA_VIEW13.DT_27 = Ctpingtai.ToString();
                    break;
            }
            var Tdate28 = Convert.ToDateTime(dATE1 + "-28");
            switch (Tdate28.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dA_VIEW11.DT_28 = "0";
                    dA_VIEW12.DT_28 = "0";
                    dA_VIEW13.DT_28 = "0";
                    break;
                default:
                    dA_VIEW11.DT_28 = Ctperson.ToString();
                    dA_VIEW12.DT_28 = Ctgongtang.ToString();
                    dA_VIEW13.DT_28 = Ctpingtai.ToString();
                    break;
            }
            if (vMax>=29)
            {
                var Tdate29 = Convert.ToDateTime(dATE1 + "-29");
                switch (Tdate29.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        dA_VIEW11.DT_29 = "0";
                        dA_VIEW12.DT_29 = "0";
                        dA_VIEW13.DT_29 = "0";
                        break;
                    default:
                        dA_VIEW11.DT_29 = Ctperson.ToString();
                        dA_VIEW12.DT_29 = Ctgongtang.ToString();
                        dA_VIEW13.DT_29 = Ctpingtai.ToString();
                        break;
                }
            }
            else
            {
                dA_VIEW11.DT_29 = "0";
                dA_VIEW12.DT_29 = "0";
                dA_VIEW13.DT_29 = "0";
            }
            if (vMax >= 30)
            {
                var Tdate30 = Convert.ToDateTime(dATE1 + "-30");
                switch (Tdate30.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        dA_VIEW11.DT_30 = "0";
                        dA_VIEW12.DT_30 = "0";
                        dA_VIEW13.DT_30 = "0";
                        break;
                    default:
                        dA_VIEW11.DT_30 = Ctperson.ToString();
                        dA_VIEW12.DT_30 = Ctgongtang.ToString();
                        dA_VIEW13.DT_30 = Ctpingtai.ToString();
                        break;
                }
            }
            else
            {
                dA_VIEW11.DT_30 = "0";
                dA_VIEW12.DT_30 = "0";
                dA_VIEW13.DT_30 = "0";
            }
            if (vMax >= 31)
            {
                var Tdate31 = Convert.ToDateTime(dATE1 + "-31");
                switch (Tdate31.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        dA_VIEW11.DT_31 = "0";
                        dA_VIEW12.DT_31 = "0";
                        dA_VIEW13.DT_31 = "0";
                        break;
                    default:
                        dA_VIEW11.DT_31 = Ctperson.ToString();
                        dA_VIEW12.DT_31 = Ctgongtang.ToString();
                        dA_VIEW13.DT_31 = Ctpingtai.ToString();
                        break;
                }
            }
            else
            {
                dA_VIEW11.DT_31 = "0";
                dA_VIEW12.DT_31 = "0";
                dA_VIEW13.DT_31 = "0";
            }

            dA_VIEW11.TOTAL = personTal.ToString();
            dA_VIEW12.TOTAL = gongtangTal.ToString();
            dA_VIEW13.TOTAL = pingtaiTal.ToString();
            dA_VIEWs.Add(dA_VIEW11);
            dA_VIEWs.Add(dA_VIEW12);
            dA_VIEWs.Add(dA_VIEW13);
            #endregion
            foreach (var item in DA_2)
            {
                DA_VIEW dA_VIEW1 = new DA_VIEW();//直接作业人员 
                var CliemtName = item.NAME;
                dA_VIEW1.TYPES = "项目支出小计:"; 
                dA_VIEW1.TYPES1 = ""; 
                dA_VIEW1.MODEL = CliemtName;
                //SUM(E23: E25) + SUM(E$43:E$44) * 0.3 + E$42 * 0.25 
                var dtms = dA_VIEWs1.Where(x => x.MODEL == CliemtName).FirstOrDefault();
                var dtmos= dA_VIEWs2.Where(x => x.MODEL == CliemtName).FirstOrDefault();
                double Ct1 = 0;
                var Dates1 = dATE1 + "-01";
                var Tdates1 = Convert.ToDateTime(Dates1);
                Ct1 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_1 == "" ? 0 : Convert.ToDouble(dtms.DT_1),
                    _ => Math.Round(dtms.DT_1 == "" ? 0 : Convert.ToDouble(dtms.DT_1) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_1 = Ct1.ToString();

                double Ct2 = 0;
                Dates1 = dATE1 + "-02";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct2 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_2 == "" ? 0 : Convert.ToDouble(dtms.DT_2),
                    _ => Math.Round(dtms.DT_2 == "" ? 0 : Convert.ToDouble(dtms.DT_2) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_2= Ct2.ToString();

                double Ct3 = 0;
                Dates1 = dATE1 + "-03";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct3 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_3 == "" ? 0 : Convert.ToDouble(dtms.DT_3),
                    _ => Math.Round(dtms.DT_3 == "" ? 0 : Convert.ToDouble(dtms.DT_3) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_3 = Ct3.ToString();

                double Ct4 = 0;
                Dates1 = dATE1 + "-04";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct4 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_4 == "" ? 0 : Convert.ToDouble(dtms.DT_4),
                    _ => Math.Round(dtms.DT_4 == "" ? 0 : Convert.ToDouble(dtms.DT_4) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_4 = Ct4.ToString();

                double Ct5 = 0;
                Dates1 = dATE1 + "-05";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct5 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_5 == "" ? 0 : Convert.ToDouble(dtms.DT_5),
                    _ => Math.Round(dtms.DT_5 == "" ? 0 : Convert.ToDouble(dtms.DT_5) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_5 = Ct5.ToString();
                double Ct6 = 0;
                Dates1 = dATE1 + "-06";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct6 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_6 == "" ? 0 : Convert.ToDouble(dtms.DT_6),
                    _ => Math.Round(dtms.DT_6 == "" ? 0 : Convert.ToDouble(dtms.DT_6) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_6 = Ct6.ToString();

                double Ct7 = 0;
                Dates1 = dATE1 + "-07";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct7 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_7 == "" ? 0 : Convert.ToDouble(dtms.DT_7),
                    _ => Math.Round(dtms.DT_7 == "" ? 0 : Convert.ToDouble(dtms.DT_7) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_7 = Ct7.ToString();

                double Ct8 = 0;
                Dates1 = dATE1 + "-08";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct8 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_8 == "" ? 0 : Convert.ToDouble(dtms.DT_8),
                    _ => Math.Round(dtms.DT_8 == "" ? 0 : Convert.ToDouble(dtms.DT_8) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_8 = Ct8.ToString();

                double Ct9 = 0;
                Dates1 = dATE1 + "-09";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct9 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_9 == "" ? 0 : Convert.ToDouble(dtms.DT_9),
                    _ => Math.Round(dtms.DT_9 == "" ? 0 : Convert.ToDouble(dtms.DT_9) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_9 = Ct9.ToString();

                double Ct10 = 0;
                Dates1 = dATE1 + "-10";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct10 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_10 == "" ? 0 : Convert.ToDouble(dtms.DT_10),
                    _ => Math.Round(dtms.DT_10 == "" ? 0 : Convert.ToDouble(dtms.DT_10) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_10 = Ct10.ToString();

                double Ct11 = 0;
                Dates1 = dATE1 + "-11";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct11 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_11 == "" ? 0 : Convert.ToDouble(dtms.DT_11),
                    _ => Math.Round(dtms.DT_11 == "" ? 0 : Convert.ToDouble(dtms.DT_11) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_11 = Ct11.ToString();

                double Ct12 = 0;
                Dates1 = dATE1 + "-12";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct12 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_12 == "" ? 0 : Convert.ToDouble(dtms.DT_12),
                    _ => Math.Round(dtms.DT_12 == "" ? 0 : Convert.ToDouble(dtms.DT_12) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_12 = Ct12.ToString();

                double Ct13 = 0;
                Dates1 = dATE1 + "-13";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct13 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_13 == "" ? 0 : Convert.ToDouble(dtms.DT_13),
                    _ => Math.Round(dtms.DT_13 == "" ? 0 : Convert.ToDouble(dtms.DT_13) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_13 = Ct13.ToString();

                double Ct14 = 0;
                Dates1 = dATE1 + "-14";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct14 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_14 == "" ? 0 : Convert.ToDouble(dtms.DT_14),
                    _ => Math.Round(dtms.DT_14 == "" ? 0 : Convert.ToDouble(dtms.DT_14) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_14= Ct14.ToString();

                double Ct15 = 0;
                Dates1 = dATE1 + "-15";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct15 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_15 == "" ? 0 : Convert.ToDouble(dtms.DT_15),
                    _ => Math.Round(dtms.DT_15 == "" ? 0 : Convert.ToDouble(dtms.DT_15) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_15 = Ct15.ToString();

                double Ct16 = 0;
                Dates1 = dATE1 + "-16";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct16 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_16 == "" ? 0 : Convert.ToDouble(dtms.DT_16),
                    _ => Math.Round(dtms.DT_16 == "" ? 0 : Convert.ToDouble(dtms.DT_16) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_16 = Ct16.ToString();

                double Ct17 = 0;
                Dates1 = dATE1 + "-17";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct17 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_17 == "" ? 0 : Convert.ToDouble(dtms.DT_17),
                    _ => Math.Round(dtms.DT_17 == "" ? 0 : Convert.ToDouble(dtms.DT_17) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_17 = Ct17.ToString();

                double Ct18 = 0;
                Dates1 = dATE1 + "-18";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct18 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_18 == "" ? 0 : Convert.ToDouble(dtms.DT_18),
                    _ => Math.Round(dtms.DT_18 == "" ? 0 : Convert.ToDouble(dtms.DT_18) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_18 = Ct18.ToString();

                double Ct19 = 0;
                Dates1 = dATE1 + "-19";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct19 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_19 == "" ? 0 : Convert.ToDouble(dtms.DT_19),
                    _ => Math.Round(dtms.DT_19 == "" ? 0 : Convert.ToDouble(dtms.DT_19) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_19 = Ct19.ToString();

                double Ct20 = 0;
                Dates1 = dATE1 + "-20";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct20 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_20 == "" ? 0 : Convert.ToDouble(dtms.DT_20),
                    _ => Math.Round(dtms.DT_20 == "" ? 0 : Convert.ToDouble(dtms.DT_20) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_20 = Ct20.ToString();

                double Ct21 = 0;
                Dates1 = dATE1 + "-21";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct21 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_21 == "" ? 0 : Convert.ToDouble(dtms.DT_21),
                    _ => Math.Round(dtms.DT_21 == "" ? 0 : Convert.ToDouble(dtms.DT_21) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_21 = Ct21.ToString();

                double Ct22 = 0;
                Dates1 = dATE1 + "-22";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct22 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_22 == "" ? 0 : Convert.ToDouble(dtms.DT_22),
                    _ => Math.Round(dtms.DT_22 == "" ? 0 : Convert.ToDouble(dtms.DT_22) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_22 = Ct22.ToString();

                double Ct23 = 0;
                Dates1 = dATE1 + "-23";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct23 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_23 == "" ? 0 : Convert.ToDouble(dtms.DT_23),
                    _ => Math.Round(dtms.DT_23 == "" ? 0 : Convert.ToDouble(dtms.DT_23) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_23 = Ct23.ToString();

                double Ct24 = 0;
                Dates1 = dATE1 + "-24";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct24 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_24 == "" ? 0 : Convert.ToDouble(dtms.DT_24),
                    _ => Math.Round(dtms.DT_24 == "" ? 0 : Convert.ToDouble(dtms.DT_24) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_24 = Ct24.ToString();

                double Ct25 = 0;
                Dates1 = dATE1 + "-25";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct25 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_25 == "" ? 0 : Convert.ToDouble(dtms.DT_25),
                    _ => Math.Round(dtms.DT_25 == "" ? 0 : Convert.ToDouble(dtms.DT_25) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_25 = Ct25.ToString();

                double Ct26 = 0;
                Dates1 = dATE1 + "-26";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct26 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_26 == "" ? 0 : Convert.ToDouble(dtms.DT_26),
                    _ => Math.Round(dtms.DT_26 == "" ? 0 : Convert.ToDouble(dtms.DT_26) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_26 = Ct26.ToString();

                double Ct27 = 0;
                Dates1 = dATE1 + "-27";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct27 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_27 == "" ? 0 : Convert.ToDouble(dtms.DT_27),
                    _ => Math.Round(dtms.DT_27 == "" ? 0 : Convert.ToDouble(dtms.DT_27) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_27 = Ct27.ToString();

                double Ct28 = 0;
                Dates1 = dATE1 + "-28";
                Tdates1 = Convert.ToDateTime(Dates1);
                Ct28 = Tdates1.DayOfWeek switch
                {
                    DayOfWeek.Sunday => dtms.DT_28 == "" ? 0 : Convert.ToDouble(dtms.DT_28),
                    _ => Math.Round(dtms.DT_28 == "" ? 0 : Convert.ToDouble(dtms.DT_28) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                };
                dA_VIEW1.DT_28 = Ct28.ToString();

                double Ct29 = 0;
                Dates1 = dATE1 + "-29";
                if (vMax >= 29)
                {
                    Tdates1 = Convert.ToDateTime(Dates1);
                    Ct29 = Tdates1.DayOfWeek switch
                    {
                        DayOfWeek.Sunday => dtms.DT_29 == "" ? 0 : Convert.ToDouble(dtms.DT_29),
                        _ => Math.Round(dtms.DT_29 == "" ? 0 : Convert.ToDouble(dtms.DT_29) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                    };
                }
                dA_VIEW1.DT_29 = Ct29.ToString();

                double Ct30 = 0;
                Dates1 = dATE1 + "-30";
                if (vMax >= 30)
                {
                    Tdates1 = Convert.ToDateTime(Dates1);
                    Ct30 = Tdates1.DayOfWeek switch
                    {
                        DayOfWeek.Sunday => dtms.DT_30 == "" ? 0 : Convert.ToDouble(dtms.DT_30),
                        _ => Math.Round(dtms.DT_30 == "" ? 0 : Convert.ToDouble(dtms.DT_30) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                    };
                }
                dA_VIEW1.DT_30 = Ct30.ToString();

                double Ct31 = 0;
                Dates1 = dATE1 + "-31";
                if (vMax >= 31)
                {
                    Tdates1 = Convert.ToDateTime(Dates1);
                    Ct31 = Tdates1.DayOfWeek switch
                    {
                        DayOfWeek.Sunday => dtms.DT_31 == "" ? 0 : Convert.ToDouble(dtms.DT_31),
                        _ => Math.Round(dtms.DT_31 == "" ? 0 : Convert.ToDouble(dtms.DT_31) + Ctperson * (item.PAY1 == "" ? 1 : Convert.ToDouble(item.PAY1)) + Ctgongtang * (item.PAY2 == "" ? 1 : Convert.ToDouble(item.PAY2)) + Ctpingtai * (item.PAY3 == "" ? 1 : Convert.ToDouble(item.PAY3)), 2),
                    };
                }
                dA_VIEW1.DT_31 = Ct31.ToString();
                dA_VIEW1.TOTAL= Math.Round(Ct1+ Ct2 + Ct3 + Ct4 + Ct5 + Ct6 + Ct7 + Ct8 + Ct9 + Ct10 + Ct11 + Ct12 + Ct13 + Ct14 + Ct15 + Ct16 + Ct17 + Ct18 + Ct19 + Ct20 + Ct21 + Ct22 + Ct23 + Ct24 + Ct25 + Ct26 + Ct27 + Ct28 + Ct29 + Ct30 + Ct31,2).ToString();
                dA_VIEWs.Add(dA_VIEW1);
                dA_VIEWs3.Add(dA_VIEW1);
                tO01 += Ct1;
                tO02 += Ct2;
                tO03 += Ct3;
                tO04 += Ct4;
                tO05 += Ct5;
                tO06 += Ct6;
                tO07 += Ct7;
                tO08 += Ct8;
                tO09 += Ct9;
                tO10 += Ct10;
                tO11 += Ct11;
                tO12 += Ct12;
                tO13 += Ct13;
                tO14 += Ct14;
                tO15 += Ct15;
                tO16 += Ct16;
                tO17 += Ct17;
                tO18 += Ct18;
                tO19 += Ct19;
                tO20 += Ct20;
                tO21 += Ct21;
                tO22 += Ct22;
                tO23 += Ct23;
                tO24 += Ct24;
                tO25 += Ct25;
                tO26 += Ct26;
                tO27 += Ct27;
                tO28 += Ct28;
                tO29 += Ct29;
                tO30 += Ct30;
                tO31 += Ct31;
                tOtal += Math.Round(dA_VIEW1.TOTAL == "" ? 0 : Convert.ToDouble(dA_VIEW1.TOTAL),2);
            }
            DA_VIEW dA_VIEWTT1 = new DA_VIEW
            {
                TYPES = "生产部支出合计:",
                TYPES1 = "",
                MODEL = "",
                DT_1 = Math.Round(tO01, 2).ToString(),
                DT_2 = Math.Round(tO02, 2).ToString(),
                DT_3 = Math.Round(tO03, 2).ToString(),
                DT_4 = Math.Round(tO04, 2).ToString(),
                DT_5 = Math.Round(tO05, 2).ToString(),
                DT_6 = Math.Round(tO06, 2).ToString(),
                DT_7 = Math.Round(tO07, 2).ToString(),
                DT_8 = Math.Round(tO08, 2).ToString(),
                DT_9 = Math.Round(tO09, 2).ToString(),
                DT_10 = Math.Round(tO10, 2).ToString(),
                DT_11 = Math.Round(tO11, 2).ToString(),
                DT_12 = Math.Round(tO12, 2).ToString(),
                DT_13 = Math.Round(tO13, 2).ToString(),
                DT_14 = Math.Round(tO14, 2).ToString(),
                DT_15 = Math.Round(tO15, 2).ToString(),
                DT_16 = Math.Round(tO16, 2).ToString(),
                DT_17 = Math.Round(tO17, 2).ToString(),
                DT_18 = Math.Round(tO18, 2).ToString(),
                DT_19 = Math.Round(tO19, 2).ToString(),
                DT_20 = Math.Round(tO20, 2).ToString(),
                DT_21 = Math.Round(tO21, 2).ToString(),
                DT_22 = Math.Round(tO22, 2).ToString(),
                DT_23 = Math.Round(tO23, 2).ToString(),
                DT_24 = Math.Round(tO24, 2).ToString(),
                DT_25 = Math.Round(tO25, 2).ToString(),
                DT_26 = Math.Round(tO26, 2).ToString(),
                DT_27 = Math.Round(tO27, 2).ToString(),
                DT_28 = Math.Round(tO28, 2).ToString(),
                DT_29 = Math.Round(tO29, 2).ToString(),
                DT_30 = Math.Round(tO30, 2).ToString(),
                DT_31 = Math.Round(tO31, 2).ToString(),
                TOTAL = Math.Round(tOtal, 2).ToString()
            };//生产部支出合计
            dA_VIEWs.Add(dA_VIEWTT1);
            foreach (var item in DA_2)
            {
                //List<DA_VIEW> dA_VIEWs1 = new List<DA_VIEW>();//支出统计
                //List<DA_VIEW> dA_VIEWs2 = new List<DA_VIEW>();//收入统计
                DA_VIEW dA_VIEW1 = new DA_VIEW();//直接作业人员 
                var CliemtName = item.NAME;
                dA_VIEW1.TYPES = "盈亏(元)";
                dA_VIEW1.TYPES1 = "";
                dA_VIEW1.MODEL = CliemtName;
                //SUM(E23: E25) + SUM(E$43:E$44) * 0.3 + E$42 * 0.25 
                var dtms = dA_VIEWs3.Where(x => x.MODEL == CliemtName).FirstOrDefault();
                var dtmos = dA_VIEWs2.Where(x => x.TYPES == CliemtName).FirstOrDefault();
                dA_VIEW1.DT_1 = Math.Round((dtmos.DT_1 == "" ? 0 : Convert.ToDouble(dtmos.DT_1)) - (dtms.DT_1 == "" ? 0 : Convert.ToDouble(dtms.DT_1)), 2).ToString();
                dA_VIEW1.DT_2 = Math.Round((dtmos.DT_2 == "" ? 0 : Convert.ToDouble(dtmos.DT_2)) - (dtms.DT_2 == "" ? 0 : Convert.ToDouble(dtms.DT_2)), 2).ToString();
                dA_VIEW1.DT_3 = Math.Round((dtmos.DT_3 == "" ? 0 : Convert.ToDouble(dtmos.DT_3)) - (dtms.DT_3 == "" ? 0 : Convert.ToDouble(dtms.DT_3)), 2).ToString();
                dA_VIEW1.DT_4 = Math.Round((dtmos.DT_4 == "" ? 0 : Convert.ToDouble(dtmos.DT_4)) - (dtms.DT_4 == "" ? 0 : Convert.ToDouble(dtms.DT_4)), 2).ToString();
                dA_VIEW1.DT_5 = Math.Round((dtmos.DT_5 == "" ? 0 : Convert.ToDouble(dtmos.DT_5)) - (dtms.DT_5 == "" ? 0 : Convert.ToDouble(dtms.DT_5)), 2).ToString();
                dA_VIEW1.DT_6 = Math.Round((dtmos.DT_6 == "" ? 0 : Convert.ToDouble(dtmos.DT_6)) - (dtms.DT_6 == "" ? 0 : Convert.ToDouble(dtms.DT_6)), 2).ToString();
                dA_VIEW1.DT_7 = Math.Round((dtmos.DT_7 == "" ? 0 : Convert.ToDouble(dtmos.DT_7)) - (dtms.DT_7 == "" ? 0 : Convert.ToDouble(dtms.DT_7)), 2).ToString();
                dA_VIEW1.DT_8 = Math.Round((dtmos.DT_8 == "" ? 0 : Convert.ToDouble(dtmos.DT_8)) - (dtms.DT_8 == "" ? 0 : Convert.ToDouble(dtms.DT_8)), 2).ToString();
                dA_VIEW1.DT_9 = Math.Round((dtmos.DT_9 == "" ? 0 : Convert.ToDouble(dtmos.DT_9)) - (dtms.DT_9 == "" ? 0 : Convert.ToDouble(dtms.DT_9)), 2).ToString();
                dA_VIEW1.DT_10 = Math.Round((dtmos.DT_10 == "" ? 0 : Convert.ToDouble(dtmos.DT_10)) - (dtms.DT_10 == "" ? 0 : Convert.ToDouble(dtms.DT_10)), 2).ToString();
                dA_VIEW1.DT_11 = Math.Round((dtmos.DT_11 == "" ? 0 : Convert.ToDouble(dtmos.DT_11)) - (dtms.DT_11 == "" ? 0 : Convert.ToDouble(dtms.DT_11)), 2).ToString();
                dA_VIEW1.DT_12 = Math.Round((dtmos.DT_12 == "" ? 0 : Convert.ToDouble(dtmos.DT_12)) - (dtms.DT_12 == "" ? 0 : Convert.ToDouble(dtms.DT_12)), 2).ToString();
                dA_VIEW1.DT_13 = Math.Round((dtmos.DT_13 == "" ? 0 : Convert.ToDouble(dtmos.DT_13)) - (dtms.DT_13 == "" ? 0 : Convert.ToDouble(dtms.DT_13)), 2).ToString();
                dA_VIEW1.DT_14 = Math.Round((dtmos.DT_14 == "" ? 0 : Convert.ToDouble(dtmos.DT_14)) - (dtms.DT_14 == "" ? 0 : Convert.ToDouble(dtms.DT_14)), 2).ToString();
                dA_VIEW1.DT_15 = Math.Round((dtmos.DT_15 == "" ? 0 : Convert.ToDouble(dtmos.DT_15)) - (dtms.DT_15 == "" ? 0 : Convert.ToDouble(dtms.DT_15)), 2).ToString();
                dA_VIEW1.DT_16 = Math.Round((dtmos.DT_16 == "" ? 0 : Convert.ToDouble(dtmos.DT_16)) - (dtms.DT_16 == "" ? 0 : Convert.ToDouble(dtms.DT_16)), 2).ToString();
                dA_VIEW1.DT_17 = Math.Round((dtmos.DT_17 == "" ? 0 : Convert.ToDouble(dtmos.DT_17)) - (dtms.DT_17 == "" ? 0 : Convert.ToDouble(dtms.DT_17)), 2).ToString();
                dA_VIEW1.DT_18 = Math.Round((dtmos.DT_18 == "" ? 0 : Convert.ToDouble(dtmos.DT_18)) - (dtms.DT_18 == "" ? 0 : Convert.ToDouble(dtms.DT_18)), 2).ToString();
                dA_VIEW1.DT_19 = Math.Round((dtmos.DT_19 == "" ? 0 : Convert.ToDouble(dtmos.DT_19)) - (dtms.DT_19 == "" ? 0 : Convert.ToDouble(dtms.DT_19)), 2).ToString();
                dA_VIEW1.DT_20 = Math.Round((dtmos.DT_20 == "" ? 0 : Convert.ToDouble(dtmos.DT_20)) - (dtms.DT_20 == "" ? 0 : Convert.ToDouble(dtms.DT_20)), 2).ToString();
                dA_VIEW1.DT_21 = Math.Round((dtmos.DT_21 == "" ? 0 : Convert.ToDouble(dtmos.DT_21)) - (dtms.DT_21 == "" ? 0 : Convert.ToDouble(dtms.DT_21)), 2).ToString();
                dA_VIEW1.DT_22 = Math.Round((dtmos.DT_22 == "" ? 0 : Convert.ToDouble(dtmos.DT_22)) - (dtms.DT_22 == "" ? 0 : Convert.ToDouble(dtms.DT_22)), 2).ToString();
                dA_VIEW1.DT_23 = Math.Round((dtmos.DT_23 == "" ? 0 : Convert.ToDouble(dtmos.DT_23)) - (dtms.DT_23 == "" ? 0 : Convert.ToDouble(dtms.DT_23)), 2).ToString();
                dA_VIEW1.DT_24 = Math.Round((dtmos.DT_24 == "" ? 0 : Convert.ToDouble(dtmos.DT_24)) - (dtms.DT_24 == "" ? 0 : Convert.ToDouble(dtms.DT_24)), 2).ToString();
                dA_VIEW1.DT_25 = Math.Round((dtmos.DT_25 == "" ? 0 : Convert.ToDouble(dtmos.DT_25)) - (dtms.DT_25 == "" ? 0 : Convert.ToDouble(dtms.DT_25)), 2).ToString();
                dA_VIEW1.DT_26 = Math.Round((dtmos.DT_26 == "" ? 0 : Convert.ToDouble(dtmos.DT_26)) - (dtms.DT_26 == "" ? 0 : Convert.ToDouble(dtms.DT_26)), 2).ToString();
                dA_VIEW1.DT_27 = Math.Round((dtmos.DT_27 == "" ? 0 : Convert.ToDouble(dtmos.DT_27)) - (dtms.DT_27 == "" ? 0 : Convert.ToDouble(dtms.DT_27)), 2).ToString();
                dA_VIEW1.DT_28 = Math.Round((dtmos.DT_28 == "" ? 0 : Convert.ToDouble(dtmos.DT_28)) - (dtms.DT_28 == "" ? 0 : Convert.ToDouble(dtms.DT_28)), 2).ToString();
                dA_VIEW1.DT_29 = Math.Round((dtmos.DT_29 == "" ? 0 : Convert.ToDouble(dtmos.DT_29)) - (dtms.DT_29 == "" ? 0 : Convert.ToDouble(dtms.DT_29)), 2).ToString();
                dA_VIEW1.DT_30 = Math.Round((dtmos.DT_30 == "" ? 0 : Convert.ToDouble(dtmos.DT_30)) - (dtms.DT_30 == "" ? 0 : Convert.ToDouble(dtms.DT_30)), 2).ToString();
                dA_VIEW1.DT_31 = Math.Round((dtmos.DT_31 == "" ? 0 : Convert.ToDouble(dtmos.DT_31)) - (dtms.DT_31 == "" ? 0 : Convert.ToDouble(dtms.DT_31)), 2).ToString();
                dA_VIEW1.TOTAL = Math.Round((dtmos.TOTAL == "" ? 0 : Convert.ToDouble(dtmos.TOTAL)) - (dtms.TOTAL == "" ? 0 : Convert.ToDouble(dtms.TOTAL)), 2).ToString();
                dA_VIEWs.Add(dA_VIEW1);
            };
            //生产部盈亏（元）：
            DA_VIEW dA_VIEWTT2 = new DA_VIEW
            {
                TYPES = "生产部盈亏（元）:",
                TYPES1 = "",
                MODEL = "",
               
                DT_1 = Math.Round(t01 - tO01, 2).ToString(),
                DT_2 = Math.Round(t02 - tO02, 2).ToString(),
                DT_3 = Math.Round(t03 - tO03, 2).ToString(),
                DT_4 = Math.Round(t04 - tO04, 2).ToString(),
                DT_5 = Math.Round(t05 - tO05, 2).ToString(),
                DT_6 = Math.Round(t06 - tO06, 2).ToString(),
                DT_7 = Math.Round(t07 - tO07, 2).ToString(),
                DT_8 = Math.Round(t08 - tO08, 2).ToString(),
                DT_9 = Math.Round(t09 - tO09, 2).ToString(),
                DT_10 = Math.Round(t10 - tO10, 2).ToString(),
                DT_11 = Math.Round(t11 - tO11, 2).ToString(),
                DT_12 = Math.Round(t12 - tO12, 2).ToString(),
                DT_13 = Math.Round(t13 - tO13, 2).ToString(),
                DT_14 = Math.Round(t14 - tO14, 2).ToString(),
                DT_15 = Math.Round(t15 - tO15, 2).ToString(),
                DT_16 = Math.Round(t16 - tO16, 2).ToString(),
                DT_17 = Math.Round(t17 - tO17, 2).ToString(),
                DT_18 = Math.Round(t18 - tO18, 2).ToString(),
                DT_19 = Math.Round(t19 - tO19, 2).ToString(),
                DT_20 = Math.Round(t20 - tO20, 2).ToString(),
                DT_21 = Math.Round(t21 - tO21, 2).ToString(),
                DT_22 = Math.Round(t22 - tO22, 2).ToString(),
                DT_23 = Math.Round(t23 - tO23, 2).ToString(),
                DT_24 = Math.Round(t24 - tO24, 2).ToString(),
                DT_25 = Math.Round(t25 - tO25, 2).ToString(),
                DT_26 = Math.Round(t26 - tO26, 2).ToString(),
                DT_27 = Math.Round(t27 - tO27, 2).ToString(),
                DT_28 = Math.Round(t28 - tO28, 2).ToString(),
                DT_29 = Math.Round(t29 - tO29, 2).ToString(),
                DT_30 = Math.Round(t30 - tO30, 2).ToString(),
                DT_31 = Math.Round(t31 - tO31, 2).ToString(),
                TOTAL = Math.Round(ttal - tOtal, 2).ToString()
            };
            dA_VIEWs.Add(dA_VIEWTT2);
            return dA_VIEWs; 
        }
        public async Task<IEnumerable<DA_VIEW>> getDA_VIEWMODEL(string dATE1, string cLIENTNAME)
        {
            List<DA_VIEW> LidA_VIEWs = new List<DA_VIEW>();
            List<DA_VIEW> Lids = new List<DA_VIEW>();
            var DA_ = await _sqlDbContext.DA_CLENTNAME.Where(x => x.BU == 1).OrderBy(x => x.CODE).ToArrayAsync();
            foreach (var item in DA_)
            {
                List<DA_VIEW> LiTs = new List<DA_VIEW>();
                var CliemtName = item.NAME;
                var valGross1 = _sqlDbContext.DT_INSTORE.Where(x => x.DT_NAME == "整机入库" && x.DT_DATE.Value.ToString("yyyy-MM") == dATE1);
                var dtos = from q in valGross1
                           from p in _sqlDbContext.DA_BOMs
                           where q.DT_BOM == p.BOM && p.CLIENTNAME== CliemtName
                           select new DA_GROSSDtos
                           {
                               DT_BOM = q.DT_BOM,
                               DT_CLIENTCODE = p.CLIENTCODE,
                               DT_CLIENTNAME = p.CLIENTNAME,
                               DT_STORENB = q.DT_COUNT,
                               DT_DATE = q.DT_DATE.Value.ToString("yyyy-MM-dd"),
                               DT_PRICE = q.DT_PRICE,
                               DT_MODELNAME = p.MODEL,
                           };
                IEnumerable<DA_GROSSDtos> query = dtos.DistinctBy(p => p.DT_MODELNAME);
                var table = dtos.ToList();
                var Gl = query.ToList().Select(x => x.DT_MODELNAME);
                foreach (var item1 in Gl)
                {
                    var Moel = await GetDA_BOMPrice(item1);
                    string DT_MODELNAME1 = item1;
                    var dA_VIEW1 = new DA_VIEW
                    {
                        TYPES = CliemtName,//项目
                        TYPES1 = "整机入库",
                        MODEL = item1,
                        MODEL1 = Moel.PRICE
                    };
                    var newQuery = dtos.Where(x => x.DT_MODELNAME == DT_MODELNAME1).GroupBy(p => new { p.DT_DATE, p.DT_CLIENTCODE, p.DT_MODELNAME })
                    .Select(p => new
                    {
                        p.Key.DT_DATE,
                        p.Key.DT_CLIENTCODE,
                        p.Key.DT_MODELNAME,
                        DT_STORENB = p.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)).ToString(),
                    });
                    List<string> Val1 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-01")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_1 = Val1.Count == 0 ? "0" : Val1[0].ToString();

                    List<string> Val2 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-02")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_2 = Val2.Count == 0 ? "0" : Val2[0].ToString();

                    List<string> Val3 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-03")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_3 = Val3.Count == 0 ? "0" : Val3[0].ToString();

                    List<string> Val4 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-04")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_4 = Val4.Count == 0 ? "0" : Val4[0].ToString();

                    List<string> Val5 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-05")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_5 = Val5.Count == 0 ? "0" : Val5[0].ToString();

                    List<string> Val6 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-06")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_6 = Val6.Count == 0 ? "0" : Val6[0].ToString();

                    List<string> Val7 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-07")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_7 = Val7.Count == 0 ? "0" : Val7[0].ToString();

                    List<string> Val8 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-08")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_8 = Val8.Count == 0 ? "0" : Val8[0].ToString();

                    List<string> Val9 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-09")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_9 = Val9.Count == 0 ? "0" : Val9[0].ToString();

                    List<string> Val10 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-10")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_10 = Val10.Count == 0 ? "0" : Val10[0].ToString();

                    List<string> Val11 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-11")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_11 = Val11.Count == 0 ? "0" : Val11[0].ToString();

                    List<string> Val12 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-12")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_12 = Val12.Count == 0 ? "0" : Val12[0].ToString();

                    List<string> Val13 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-13")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_13 = Val13.Count == 0 ? "0" : Val13[0].ToString();

                    List<string> Val14 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-14")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_14 = Val14.Count == 0 ? "0" : Val14[0].ToString();

                    List<string> Val15 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-15")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_15 = Val15.Count == 0 ? "0" : Val15[0].ToString();

                    List<string> Val16 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-16")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_16 = Val16.Count == 0 ? "0" : Val16[0].ToString();

                    List<string> Val17 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-17")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_17 = Val17.Count == 0 ? "0" : Val17[0].ToString();

                    List<string> Val18 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-18")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_18 = Val18.Count == 0 ? "0" : Val18[0].ToString();

                    List<string> Val19 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-19")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_19 = Val19.Count == 0 ? "0" : Val19[0].ToString();

                    List<string> Val20 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-20")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_20 = Val20.Count == 0 ? "0" : Val20[0].ToString();

                    List<string> Val21 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-21")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_21 = Val21.Count == 0 ? "0" : Val21[0].ToString();

                    List<string> Val22 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-22")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_22 = Val22.Count == 0 ? "0" : Val22[0].ToString();

                    List<string> Val23 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-23")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_23 = Val23.Count == 0 ? "0" : Val23[0].ToString();

                    List<string> Val24 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-24")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_24 = Val24.Count == 0 ? "0" : Val24[0].ToString();

                    List<string> Val25 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-25")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_25 = Val25.Count == 0 ? "0" : Val25[0].ToString();

                    List<string> Val26 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-26")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_26 = Val26.Count == 0 ? "0" : Val26[0].ToString();

                    List<string> Val27 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-27")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_27 = Val27.Count == 0 ? "0" : Val27[0].ToString();

                    List<string> Val28 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-28")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_28 = Val28.Count == 0 ? "0" : Val28[0].ToString();

                    List<string> Val29 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-29")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_29 = Val29.Count == 0 ? "0" : Val29[0].ToString();

                    List<string> Val30 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-30")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_30 = Val30.Count == 0 ? "0" : Val30[0].ToString();

                    List<string> Val31 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-31")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_31 = Val31.Count == 0 ? "0" : Val31[0].ToString();
                    dA_VIEW1.TOTAL = (Convert.ToInt32(dA_VIEW1.DT_1) + Convert.ToInt32(dA_VIEW1.DT_2) + Convert.ToInt32(dA_VIEW1.DT_3) + Convert.ToInt32(dA_VIEW1.DT_4) + Convert.ToInt32(dA_VIEW1.DT_5) + Convert.ToInt32(dA_VIEW1.DT_6) + Convert.ToInt32(dA_VIEW1.DT_7)
                        + Convert.ToInt32(dA_VIEW1.DT_8) + Convert.ToInt32(dA_VIEW1.DT_9) + Convert.ToInt32(dA_VIEW1.DT_10) + Convert.ToInt32(dA_VIEW1.DT_11) + Convert.ToInt32(dA_VIEW1.DT_12) + Convert.ToInt32(dA_VIEW1.DT_13) + Convert.ToInt32(dA_VIEW1.DT_14) + Convert.ToInt32(dA_VIEW1.DT_15)
                        + Convert.ToInt32(dA_VIEW1.DT_16) + Convert.ToInt32(dA_VIEW1.DT_17) + Convert.ToInt32(dA_VIEW1.DT_18) + Convert.ToInt32(dA_VIEW1.DT_19) + Convert.ToInt32(dA_VIEW1.DT_20) + Convert.ToInt32(dA_VIEW1.DT_21) + Convert.ToInt32(dA_VIEW1.DT_22)
                        + Convert.ToInt32(dA_VIEW1.DT_23) + Convert.ToInt32(dA_VIEW1.DT_24) + Convert.ToInt32(dA_VIEW1.DT_25) + Convert.ToInt32(dA_VIEW1.DT_26) + Convert.ToInt32(dA_VIEW1.DT_27) + Convert.ToInt32(dA_VIEW1.DT_28) + Convert.ToInt32(dA_VIEW1.DT_29)
                        + Convert.ToInt32(dA_VIEW1.DT_30) + Convert.ToInt32(dA_VIEW1.DT_31)).ToString();
                    LidA_VIEWs.Add(dA_VIEW1);
                    LiTs.Add(dA_VIEW1);
                }

                var valGross4 = _sqlDbContext.DT_INSTORE.Where(x => x.DT_NAME == "其他入库" && x.DT_DATE.Value.ToString("yyyy-MM") == dATE1);
                var dtos4 = from q in valGross4
                           from p in _sqlDbContext.DA_BOMs
                           where q.DT_BOM == p.BOM && p.CLIENTNAME == CliemtName
                           select new DA_GROSSDtos
                           {
                               DT_BOM = q.DT_BOM,
                               DT_CLIENTCODE = p.CLIENTCODE,
                               DT_CLIENTNAME = p.CLIENTNAME,
                               DT_STORENB = q.DT_COUNT,
                               DT_DATE = q.DT_DATE.Value.ToString("yyyy-MM-dd"),
                               DT_PRICE = q.DT_PRICE,
                               DT_MODELNAME = p.MODEL,
                           };
                IEnumerable<DA_GROSSDtos> query4 = dtos4.DistinctBy(p => p.DT_MODELNAME);
                var table4 = dtos4.ToList();
                var Gl4 = query4.ToList().Select(x => x.DT_MODELNAME);
                foreach (var item1 in Gl4)
                {
                    var Moel = await GetDA_BOMPrice(item1);
                    string DT_MODELNAME1 = item1;
                    var dA_VIEW1 = new DA_VIEW
                    {
                        TYPES = CliemtName,//项目
                        TYPES1 = "其他入库",
                        MODEL = item1,
                        MODEL1 = Moel.PRICE
                    };
                    var newQuery = dtos4.Where(x => x.DT_MODELNAME == DT_MODELNAME1).GroupBy(p => new { p.DT_DATE, p.DT_CLIENTCODE, p.DT_MODELNAME })
                    .Select(p => new
                    {
                        p.Key.DT_DATE,
                        p.Key.DT_CLIENTCODE,
                        p.Key.DT_MODELNAME,
                        DT_STORENB = p.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)).ToString(),
                    });
                    List<string> Val1 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-01")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_1 = Val1.Count == 0 ? "0" : Val1[0].ToString();

                    List<string> Val2 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-02")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_2 = Val2.Count == 0 ? "0" : Val2[0].ToString();

                    List<string> Val3 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-03")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_3 = Val3.Count == 0 ? "0" : Val3[0].ToString();

                    List<string> Val4 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-04")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_4 = Val4.Count == 0 ? "0" : Val4[0].ToString();

                    List<string> Val5 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-05")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_5 = Val5.Count == 0 ? "0" : Val5[0].ToString();

                    List<string> Val6 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-06")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_6 = Val6.Count == 0 ? "0" : Val6[0].ToString();

                    List<string> Val7 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-07")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_7 = Val7.Count == 0 ? "0" : Val7[0].ToString();

                    List<string> Val8 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-08")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_8 = Val8.Count == 0 ? "0" : Val8[0].ToString();

                    List<string> Val9 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-09")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_9 = Val9.Count == 0 ? "0" : Val9[0].ToString();

                    List<string> Val10 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-10")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_10 = Val10.Count == 0 ? "0" : Val10[0].ToString();

                    List<string> Val11 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-11")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_11 = Val11.Count == 0 ? "0" : Val11[0].ToString();

                    List<string> Val12 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-12")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_12 = Val12.Count == 0 ? "0" : Val12[0].ToString();

                    List<string> Val13 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-13")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_13 = Val13.Count == 0 ? "0" : Val13[0].ToString();

                    List<string> Val14 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-14")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_14 = Val14.Count == 0 ? "0" : Val14[0].ToString();

                    List<string> Val15 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-15")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_15 = Val15.Count == 0 ? "0" : Val15[0].ToString();

                    List<string> Val16 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-16")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_16 = Val16.Count == 0 ? "0" : Val16[0].ToString();

                    List<string> Val17 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-17")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_17 = Val17.Count == 0 ? "0" : Val17[0].ToString();

                    List<string> Val18 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-18")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_18 = Val18.Count == 0 ? "0" : Val18[0].ToString();

                    List<string> Val19 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-19")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_19 = Val19.Count == 0 ? "0" : Val19[0].ToString();

                    List<string> Val20 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-20")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_20 = Val20.Count == 0 ? "0" : Val20[0].ToString();

                    List<string> Val21 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-21")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_21 = Val21.Count == 0 ? "0" : Val21[0].ToString();

                    List<string> Val22 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-22")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_22 = Val22.Count == 0 ? "0" : Val22[0].ToString();

                    List<string> Val23 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-23")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_23 = Val23.Count == 0 ? "0" : Val23[0].ToString();

                    List<string> Val24 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-24")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_24 = Val24.Count == 0 ? "0" : Val24[0].ToString();

                    List<string> Val25 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-25")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_25 = Val25.Count == 0 ? "0" : Val25[0].ToString();

                    List<string> Val26 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-26")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_26 = Val26.Count == 0 ? "0" : Val26[0].ToString();

                    List<string> Val27 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-27")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_27 = Val27.Count == 0 ? "0" : Val27[0].ToString();

                    List<string> Val28 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-28")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_28 = Val28.Count == 0 ? "0" : Val28[0].ToString();

                    List<string> Val29 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-29")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_29 = Val29.Count == 0 ? "0" : Val29[0].ToString();

                    List<string> Val30 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-30")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_30 = Val30.Count == 0 ? "0" : Val30[0].ToString();

                    List<string> Val31 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-31")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_31 = Val31.Count == 0 ? "0" : Val31[0].ToString();
                    dA_VIEW1.TOTAL = (Convert.ToInt32(dA_VIEW1.DT_1) + Convert.ToInt32(dA_VIEW1.DT_2) + Convert.ToInt32(dA_VIEW1.DT_3) + Convert.ToInt32(dA_VIEW1.DT_4) + Convert.ToInt32(dA_VIEW1.DT_5) + Convert.ToInt32(dA_VIEW1.DT_6) + Convert.ToInt32(dA_VIEW1.DT_7)
                        + Convert.ToInt32(dA_VIEW1.DT_8) + Convert.ToInt32(dA_VIEW1.DT_9) + Convert.ToInt32(dA_VIEW1.DT_10) + Convert.ToInt32(dA_VIEW1.DT_11) + Convert.ToInt32(dA_VIEW1.DT_12) + Convert.ToInt32(dA_VIEW1.DT_13) + Convert.ToInt32(dA_VIEW1.DT_14) + Convert.ToInt32(dA_VIEW1.DT_15)
                        + Convert.ToInt32(dA_VIEW1.DT_16) + Convert.ToInt32(dA_VIEW1.DT_17) + Convert.ToInt32(dA_VIEW1.DT_18) + Convert.ToInt32(dA_VIEW1.DT_19) + Convert.ToInt32(dA_VIEW1.DT_20) + Convert.ToInt32(dA_VIEW1.DT_21) + Convert.ToInt32(dA_VIEW1.DT_22)
                        + Convert.ToInt32(dA_VIEW1.DT_23) + Convert.ToInt32(dA_VIEW1.DT_24) + Convert.ToInt32(dA_VIEW1.DT_25) + Convert.ToInt32(dA_VIEW1.DT_26) + Convert.ToInt32(dA_VIEW1.DT_27) + Convert.ToInt32(dA_VIEW1.DT_28) + Convert.ToInt32(dA_VIEW1.DT_29)
                        + Convert.ToInt32(dA_VIEW1.DT_30) + Convert.ToInt32(dA_VIEW1.DT_31)).ToString();
                    LidA_VIEWs.Add(dA_VIEW1);
                    LiTs.Add(dA_VIEW1);
                }


                var valGross2 = _sqlDbContext.DT_INSTORE.Where(x => x.DT_DATE.Value.ToString("yyyy-MM") == dATE1 && x.DT_NAME == "KD组件");
                var dtos2 = from q in valGross2
                            from p in _sqlDbContext.DA_BOMs
                            where q.DT_BOM == p.BOM && p.CLIENTNAME == CliemtName
                            select new DA_GROSSDtos
                            {
                                DT_BOM = q.DT_BOM,
                                DT_CLIENTCODE = p.CLIENTCODE,
                                DT_CLIENTNAME = p.CLIENTNAME,
                                DT_STORENB = q.DT_COUNT,
                                DT_DATE = q.DT_DATE.Value.ToString("yyyy-MM-dd"),
                                DT_PRICE = q.DT_PRICE,
                                DT_MODELNAME = p.MODEL,
                            };
                var query2 = dtos2.DistinctBy(p => p.DT_MODELNAME);
                var Gl2 = query2.ToList().Select(x => x.DT_MODELNAME);
                foreach (var item1 in Gl2)
                {
                    var Moel = await GetDA_BOMPrice(item1);
                    string DT_MODELNAME1 = item1;
                    DA_VIEW dA_VIEW1 = new DA_VIEW
                    {
                        TYPES = CliemtName,//项目
                        TYPES1 = "CKD/SKD",
                        MODEL = item1,
                        MODEL1 = Moel.PRICE
                    };
                    var newQuery = dtos2.Where(x => x.DT_MODELNAME == DT_MODELNAME1).GroupBy(p => new { p.DT_DATE, p.DT_CLIENTCODE, p.DT_MODELNAME })
                    .Select(p => new
                    {
                        p.Key.DT_DATE,
                        p.Key.DT_CLIENTCODE,
                        p.Key.DT_MODELNAME,
                        DT_STORENB = p.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)).ToString(),
                    });
                    List<string> Val1 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-01")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_1 = Val1.Count == 0 ? "0" : Val1[0].ToString();

                    List<string> Val2 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-02")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_2 = Val2.Count == 0 ? "0" : Val2[0].ToString();

                    List<string> Val3 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-03")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_3 = Val3.Count == 0 ? "0" : Val3[0].ToString();

                    List<string> Val4 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-04")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_4 = Val4.Count == 0 ? "0" : Val4[0].ToString();

                    List<string> Val5 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-05")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_5 = Val5.Count == 0 ? "0" : Val5[0].ToString();

                    List<string> Val6 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-06")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_6 = Val6.Count == 0 ? "0" : Val6[0].ToString();

                    List<string> Val7 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-07")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_7 = Val7.Count == 0 ? "0" : Val7[0].ToString();

                    List<string> Val8 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-08")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_8 = Val8.Count == 0 ? "0" : Val8[0].ToString();

                    List<string> Val9 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-09")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_9 = Val9.Count == 0 ? "0" : Val9[0].ToString();

                    List<string> Val10 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-10")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_10 = Val10.Count == 0 ? "0" : Val10[0].ToString();

                    List<string> Val11 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-11")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_11 = Val11.Count == 0 ? "0" : Val11[0].ToString();

                    List<string> Val12 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-12")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_12 = Val12.Count == 0 ? "0" : Val12[0].ToString();

                    List<string> Val13 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-13")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_13 = Val13.Count == 0 ? "0" : Val13[0].ToString();

                    List<string> Val14 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-14")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_14 = Val14.Count == 0 ? "0" : Val14[0].ToString();

                    List<string> Val15 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-15")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_15 = Val15.Count == 0 ? "0" : Val15[0].ToString();

                    List<string> Val16 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-16")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_16 = Val16.Count == 0 ? "0" : Val16[0].ToString();

                    List<string> Val17 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-17")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_17 = Val17.Count == 0 ? "0" : Val17[0].ToString();

                    List<string> Val18 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-18")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_18 = Val18.Count == 0 ? "0" : Val18[0].ToString();

                    List<string> Val19 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-19")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_19 = Val19.Count == 0 ? "0" : Val19[0].ToString();

                    List<string> Val20 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-20")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_20 = Val20.Count == 0 ? "0" : Val20[0].ToString();

                    List<string> Val21 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-21")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_21 = Val21.Count == 0 ? "0" : Val21[0].ToString();

                    List<string> Val22 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-22")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_22 = Val22.Count == 0 ? "0" : Val22[0].ToString();

                    List<string> Val23 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-23")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_23 = Val23.Count == 0 ? "0" : Val23[0].ToString();

                    List<string> Val24 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-24")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_24 = Val24.Count == 0 ? "0" : Val24[0].ToString();

                    List<string> Val25 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-25")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_25 = Val25.Count == 0 ? "0" : Val25[0].ToString();

                    List<string> Val26 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-26")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_26 = Val26.Count == 0 ? "0" : Val26[0].ToString();

                    List<string> Val27 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-27")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_27 = Val27.Count == 0 ? "0" : Val27[0].ToString();

                    List<string> Val28 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-28")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_28 = Val28.Count == 0 ? "0" : Val28[0].ToString();

                    List<string> Val29 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-29")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_29 = Val29.Count == 0 ? "0" : Val29[0].ToString();

                    List<string> Val30 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-30")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_30 = Val30.Count == 0 ? "0" : Val30[0].ToString();

                    List<string> Val31 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-31")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_31 = Val31.Count == 0 ? "0" : Val31[0].ToString();
                    dA_VIEW1.TOTAL = (Convert.ToInt32(dA_VIEW1.DT_1) + Convert.ToInt32(dA_VIEW1.DT_2) + Convert.ToInt32(dA_VIEW1.DT_3) + Convert.ToInt32(dA_VIEW1.DT_4) + Convert.ToInt32(dA_VIEW1.DT_5) + Convert.ToInt32(dA_VIEW1.DT_6) + Convert.ToInt32(dA_VIEW1.DT_7)
                        + Convert.ToInt32(dA_VIEW1.DT_8) + Convert.ToInt32(dA_VIEW1.DT_9) + Convert.ToInt32(dA_VIEW1.DT_10) + Convert.ToInt32(dA_VIEW1.DT_11) + Convert.ToInt32(dA_VIEW1.DT_12) + Convert.ToInt32(dA_VIEW1.DT_13) + Convert.ToInt32(dA_VIEW1.DT_14) + Convert.ToInt32(dA_VIEW1.DT_15)
                        + Convert.ToInt32(dA_VIEW1.DT_16) + Convert.ToInt32(dA_VIEW1.DT_17) + Convert.ToInt32(dA_VIEW1.DT_18) + Convert.ToInt32(dA_VIEW1.DT_19) + Convert.ToInt32(dA_VIEW1.DT_20) + Convert.ToInt32(dA_VIEW1.DT_21) + Convert.ToInt32(dA_VIEW1.DT_22)
                        + Convert.ToInt32(dA_VIEW1.DT_23) + Convert.ToInt32(dA_VIEW1.DT_24) + Convert.ToInt32(dA_VIEW1.DT_25) + Convert.ToInt32(dA_VIEW1.DT_26) + Convert.ToInt32(dA_VIEW1.DT_27) + Convert.ToInt32(dA_VIEW1.DT_28) + Convert.ToInt32(dA_VIEW1.DT_29)
                        + Convert.ToInt32(dA_VIEW1.DT_30) + Convert.ToInt32(dA_VIEW1.DT_31)).ToString();
                    LidA_VIEWs.Add(dA_VIEW1);
                    LiTs.Add(dA_VIEW1);
                }
                var valGross3 = _sqlDbContext.DT_INSTORE.Where(x => x.KD_USER == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM") == dATE1 && x.DT_NAME == "KD散料");
                var dtos3 = from q in valGross3
                            from p in _sqlDbContext.DA_BOMs
                            where q.DT_BOM == p.BOM
                            select new DA_GROSSDtos
                            {
                                DT_BOM = q.DT_BOM,
                                DT_CLIENTCODE = q.KD_USER,
                                DT_CLIENTNAME = q.KD_USER,
                                DT_STORENB = q.DT_COUNT,
                                DT_DATE = q.DT_DATE.Value.ToString("yyyy-MM-dd"),
                                DT_PRICE = q.DT_PRICE,
                                DT_MODELNAME = p.MODEL,
                            };
                var dd = valGross3.ToList();
                if (dd.Count > 0)
                {

                    var query3 = dtos3.DistinctBy(p => p.DT_CLIENTNAME);
                    var query33 = query3.ToList();
                    var Gl3 = query3.Select(x => x.DT_BOM).ToList();
                    foreach (var item1 in Gl3)
                    {
                        var Moel = await GetDA_BOMPrice1(item1);
                        string DT_MODELNAME1 = item1;
                        DA_VIEW dA_VIEW1 = new DA_VIEW
                        {
                            TYPES = CliemtName,//项目
                            TYPES1 = "散料",
                            MODEL = "/",
                            MODEL1 = Moel.PRICE
                        };
                        var newQuery = query3.GroupBy(p => new { p.DT_DATE, p.DT_CLIENTCODE})
                        .Select(p => new
                        {
                            p.Key.DT_DATE,
                            p.Key.DT_CLIENTCODE, 
                            DT_STORENB = p.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)).ToString(),
                        });
                        var dds = newQuery.ToList();
                        List<string> Val1 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-01")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_1 = Val1.Count == 0 ? "0" : Val1[0].ToString();

                        List<string> Val2 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-02")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_2 = Val2.Count == 0 ? "0" : Val2[0].ToString();

                        List<string> Val3 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-03")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_3 = Val3.Count == 0 ? "0" : Val3[0].ToString();

                        List<string> Val4 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-04")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_4 = Val4.Count == 0 ? "0" : Val4[0].ToString();

                        List<string> Val5 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-05")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_5 = Val5.Count == 0 ? "0" : Val5[0].ToString();

                        List<string> Val6 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-06")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_6 = Val6.Count == 0 ? "0" : Val6[0].ToString();

                        List<string> Val7 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-07")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_7 = Val7.Count == 0 ? "0" : Val7[0].ToString();

                        List<string> Val8 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-08")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_8 = Val8.Count == 0 ? "0" : Val8[0].ToString();

                        List<string> Val9 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-09")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_9 = Val9.Count == 0 ? "0" : Val9[0].ToString();

                        List<string> Val10 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-10")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_10 = Val10.Count == 0 ? "0" : Val10[0].ToString();

                        List<string> Val11 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-11")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_11 = Val11.Count == 0 ? "0" : Val11[0].ToString();

                        List<string> Val12 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-12")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_12 = Val12.Count == 0 ? "0" : Val12[0].ToString();

                        List<string> Val13 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-13")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_13 = Val13.Count == 0 ? "0" : Val13[0].ToString();

                        List<string> Val14 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-14")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_14 = Val14.Count == 0 ? "0" : Val14[0].ToString();

                        List<string> Val15 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-15")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_15 = Val15.Count == 0 ? "0" : Val15[0].ToString();

                        List<string> Val16 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-16")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_16 = Val16.Count == 0 ? "0" : Val16[0].ToString();

                        List<string> Val17 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-17")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_17 = Val17.Count == 0 ? "0" : Val17[0].ToString();

                        List<string> Val18 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-18")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_18 = Val18.Count == 0 ? "0" : Val18[0].ToString();

                        List<string> Val19 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-19")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_19 = Val19.Count == 0 ? "0" : Val19[0].ToString();

                        List<string> Val20 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-20")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_20 = Val20.Count == 0 ? "0" : Val20[0].ToString();

                        List<string> Val21 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-21")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_21 = Val21.Count == 0 ? "0" : Val21[0].ToString();

                        List<string> Val22 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-22")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_22 = Val22.Count == 0 ? "0" : Val22[0].ToString();

                        List<string> Val23 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-23")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_23 = Val23.Count == 0 ? "0" : Val23[0].ToString();

                        List<string> Val24 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-24")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_24 = Val24.Count == 0 ? "0" : Val24[0].ToString();

                        List<string> Val25 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-25")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_25 = Val25.Count == 0 ? "0" : Val25[0].ToString();

                        List<string> Val26 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-26")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_26 = Val26.Count == 0 ? "0" : Val26[0].ToString();

                        List<string> Val27 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-27")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_27 = Val27.Count == 0 ? "0" : Val27[0].ToString();

                        List<string> Val28 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-28")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_28 = Val28.Count == 0 ? "0" : Val28[0].ToString();

                        List<string> Val29 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-29")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_29 = Val29.Count == 0 ? "0" : Val29[0].ToString();

                        List<string> Val30 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-30")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_30 = Val30.Count == 0 ? "0" : Val30[0].ToString();

                        List<string> Val31 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-31")).Select(x => x.DT_STORENB).ToList();
                        dA_VIEW1.DT_31 = Val31.Count == 0 ? "0" : Val31[0].ToString();
                        dA_VIEW1.TOTAL = (Convert.ToInt32(dA_VIEW1.DT_1) + Convert.ToInt32(dA_VIEW1.DT_2) + Convert.ToInt32(dA_VIEW1.DT_3) + Convert.ToInt32(dA_VIEW1.DT_4) + Convert.ToInt32(dA_VIEW1.DT_5) + Convert.ToInt32(dA_VIEW1.DT_6) + Convert.ToInt32(dA_VIEW1.DT_7)
                            + Convert.ToInt32(dA_VIEW1.DT_8) + Convert.ToInt32(dA_VIEW1.DT_9) + Convert.ToInt32(dA_VIEW1.DT_10) + Convert.ToInt32(dA_VIEW1.DT_11) + Convert.ToInt32(dA_VIEW1.DT_12) + Convert.ToInt32(dA_VIEW1.DT_13) + Convert.ToInt32(dA_VIEW1.DT_14) + Convert.ToInt32(dA_VIEW1.DT_15)
                            + Convert.ToInt32(dA_VIEW1.DT_16) + Convert.ToInt32(dA_VIEW1.DT_17) + Convert.ToInt32(dA_VIEW1.DT_18) + Convert.ToInt32(dA_VIEW1.DT_19) + Convert.ToInt32(dA_VIEW1.DT_20) + Convert.ToInt32(dA_VIEW1.DT_21) + Convert.ToInt32(dA_VIEW1.DT_22)
                            + Convert.ToInt32(dA_VIEW1.DT_23) + Convert.ToInt32(dA_VIEW1.DT_24) + Convert.ToInt32(dA_VIEW1.DT_25) + Convert.ToInt32(dA_VIEW1.DT_26) + Convert.ToInt32(dA_VIEW1.DT_27) + Convert.ToInt32(dA_VIEW1.DT_28) + Convert.ToInt32(dA_VIEW1.DT_29)
                            + Convert.ToInt32(dA_VIEW1.DT_30) + Convert.ToInt32(dA_VIEW1.DT_31)).ToString();
                        LidA_VIEWs.Add(dA_VIEW1);
                        LiTs.Add(dA_VIEW1);
                    }
                }
                //var prices = valGross3.ToList();
                //if (prices.Count > 0)
                //{
                //    var price = prices[0].DT_PRICE;

                //    var sum1 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-01")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum2 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-02")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum3 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-03")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum4 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-04")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum5 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-05")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum6 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-06")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum7 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-07")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum8 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-08")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum9 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-09")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum10 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-10")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum11 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-11")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum12 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-12")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum13 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-13")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum14 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-14")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum15 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-15")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum16 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-16")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum17 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-17")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum18 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-18")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum19 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-19")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum20 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-20")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum21 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-21")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum22 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-22")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum23 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-23")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum24 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-24")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum25 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-25")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum26 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-26")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum27 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-27")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum28 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-28")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum29 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-29")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum30 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-30")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var sum31 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-31")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                //    var tall = sum1 + sum2 + sum3 + sum4 + sum5 + sum6 + sum7 + sum8 + sum9 + sum10 + sum11 + sum12 + sum13 + sum14 + sum15 + sum16 + sum17 + sum18 + sum19 + sum20 + sum21 + sum22 + sum23 + sum24 + sum25 + sum26 + sum27 + sum28 + sum29 + sum30 + sum31;
                //    DA_VIEW dA_VIEW1 = new DA_VIEW
                //    {
                //        TYPES = CliemtName,//项目
                //        TYPES1 = "散料",
                //        MODEL = CliemtName + "散料",
                //        MODEL1 = price,
                //        DT_1 = sum1.ToString(),
                //        DT_2 = sum2.ToString(),
                //        DT_3 = sum3.ToString(),
                //        DT_4 = sum4.ToString(),
                //        DT_5 = sum5.ToString(),
                //        DT_6 = sum6.ToString(),
                //        DT_7 = sum7.ToString(),
                //        DT_8 = sum8.ToString(),
                //        DT_9 = sum9.ToString(),
                //        DT_10 = sum10.ToString(),
                //        DT_11 = sum11.ToString(),
                //        DT_12 = sum12.ToString(),
                //        DT_13 = sum13.ToString(),
                //        DT_14 = sum14.ToString(),
                //        DT_15 = sum15.ToString(),
                //        DT_16 = sum16.ToString(),
                //        DT_17 = sum17.ToString(),
                //        DT_18 = sum18.ToString(),
                //        DT_19 = sum19.ToString(),
                //        DT_20 = sum20.ToString(),
                //        DT_21 = sum21.ToString(),
                //        DT_22 = sum22.ToString(),
                //        DT_23 = sum23.ToString(),
                //        DT_24 = sum24.ToString(),
                //        DT_25 = sum25.ToString(),
                //        DT_26 = sum26.ToString(),
                //        DT_27 = sum27.ToString(),
                //        DT_28 = sum28.ToString(),
                //        DT_29 = sum29.ToString(),
                //        DT_30 = sum30.ToString(),
                //        DT_31 = sum31.ToString(),
                //        TOTAL = tall.ToString(),
                //    };
                //    LidA_VIEWs.Add(dA_VIEW1);
                //    LiTs.Add(dA_VIEW1);
                //}
                //huizong
                if (LiTs.Count > 0)
                {
                    DA_VIEW dA_VIEW1 = new DA_VIEW
                    {
                        TYPES = CliemtName,//项目
                        TYPES1 = "项目入库合计",
                        MODEL = "",
                        MODEL1 = "",
                        DT_1 = LiTs.Sum(x => x.DT_1 == "" ? 0 : Convert.ToDouble(x.DT_1))
                                   .ToString(),
                        DT_2 = LiTs.Sum(x => x.DT_2 == "" ? 0 : Convert.ToDouble(x.DT_2)).ToString(),
                        DT_3 = LiTs.Sum(x => x.DT_3 == "" ? 0 : Convert.ToDouble(x.DT_3)).ToString(),
                        DT_4 = LiTs.Sum(x => x.DT_4 == "" ? 0 : Convert.ToDouble(x.DT_4)).ToString(),
                        DT_5 = LiTs.Sum(x => x.DT_5 == "" ? 0 : Convert.ToDouble(x.DT_5)).ToString(),
                        DT_6 = LiTs.Sum(x => x.DT_6 == "" ? 0 : Convert.ToDouble(x.DT_6)).ToString(),
                        DT_7 = LiTs.Sum(x => x.DT_7 == "" ? 0 : Convert.ToDouble(x.DT_7)).ToString(),
                        DT_8 = LiTs.Sum(x => x.DT_8 == "" ? 0 : Convert.ToDouble(x.DT_8)).ToString(),
                        DT_9 = LiTs.Sum(x => x.DT_9 == "" ? 0 : Convert.ToDouble(x.DT_9)).ToString(),
                        DT_10 = LiTs.Sum(x => x.DT_10 == "" ? 0 : Convert.ToDouble(x.DT_10)).ToString(),
                        DT_11 = LiTs.Sum(x => x.DT_11 == "" ? 0 : Convert.ToDouble(x.DT_11)).ToString(),
                        DT_12 = LiTs.Sum(x => x.DT_12 == "" ? 0 : Convert.ToDouble(x.DT_12)).ToString(),
                        DT_13 = LiTs.Sum(x => x.DT_13 == "" ? 0 : Convert.ToDouble(x.DT_13)).ToString(),
                        DT_14 = LiTs.Sum(x => x.DT_14 == "" ? 0 : Convert.ToDouble(x.DT_14)).ToString(),
                        DT_15 = LiTs.Sum(x => x.DT_15 == "" ? 0 : Convert.ToDouble(x.DT_15)).ToString(),
                        DT_16 = LiTs.Sum(x => x.DT_16 == "" ? 0 : Convert.ToDouble(x.DT_16)).ToString(),
                        DT_17 = LiTs.Sum(x => x.DT_17 == "" ? 0 : Convert.ToDouble(x.DT_17)).ToString(),
                        DT_18 = LiTs.Sum(x => x.DT_18 == "" ? 0 : Convert.ToDouble(x.DT_18)).ToString(),
                        DT_19 = LiTs.Sum(x => x.DT_19 == "" ? 0 : Convert.ToDouble(x.DT_19)).ToString(),
                        DT_20 = LiTs.Sum(x => x.DT_20 == "" ? 0 : Convert.ToDouble(x.DT_20)).ToString(),
                        DT_21 = LiTs.Sum(x => x.DT_21 == "" ? 0 : Convert.ToDouble(x.DT_21)).ToString(),
                        DT_22 = LiTs.Sum(x => x.DT_22 == "" ? 0 : Convert.ToDouble(x.DT_22)).ToString(),
                        DT_23 = LiTs.Sum(x => x.DT_23 == "" ? 0 : Convert.ToDouble(x.DT_23)).ToString(),
                        DT_24 = LiTs.Sum(x => x.DT_24 == "" ? 0 : Convert.ToDouble(x.DT_24)).ToString(),
                        DT_25 = LiTs.Sum(x => x.DT_25 == "" ? 0 : Convert.ToDouble(x.DT_25)).ToString(),
                        DT_26 = LiTs.Sum(x => x.DT_26 == "" ? 0 : Convert.ToDouble(x.DT_26)).ToString(),
                        DT_27 = LiTs.Sum(x => x.DT_27 == "" ? 0 : Convert.ToDouble(x.DT_27)).ToString(),
                        DT_28 = LiTs.Sum(x => x.DT_28 == "" ? 0 : Convert.ToDouble(x.DT_28)).ToString(),
                        DT_29 = LiTs.Sum(x => x.DT_29 == "" ? 0 : Convert.ToDouble(x.DT_29)).ToString(),
                        DT_30 = LiTs.Sum(x => x.DT_30 == "" ? 0 : Convert.ToDouble(x.DT_30)).ToString(),
                        DT_31 = LiTs.Sum(x => x.DT_31 == "" ? 0 : Convert.ToDouble(x.DT_31)).ToString(),
                        TOTAL = LiTs.Sum(x => x.TOTAL == "" ? 0 : Convert.ToDouble(x.TOTAL)).ToString(),
                    };
                    LidA_VIEWs.Add(dA_VIEW1);
                    Lids.Add(dA_VIEW1);
                }
            }
            //生产部当日/月入库合计
            if (Lids.Count > 0)
            {
                DA_VIEW dA_VIEW1 = new DA_VIEW
                {
                    TYPES = "生产部当日/月入库合计",//项目
                    TYPES1 = "",
                    MODEL = "",
                    MODEL1 = "",
                    DT_1 = Lids.Sum(x => x.DT_1 == "" ? 0 : Convert.ToDouble(x.DT_1))
                               .ToString(),
                    DT_2 = Lids.Sum(x => x.DT_2 == "" ? 0 : Convert.ToDouble(x.DT_2)).ToString(),
                    DT_3 = Lids.Sum(x => x.DT_3 == "" ? 0 : Convert.ToDouble(x.DT_3)).ToString(),
                    DT_4 = Lids.Sum(x => x.DT_4 == "" ? 0 : Convert.ToDouble(x.DT_4)).ToString(),
                    DT_5 = Lids.Sum(x => x.DT_5 == "" ? 0 : Convert.ToDouble(x.DT_5)).ToString(),
                    DT_6 = Lids.Sum(x => x.DT_6 == "" ? 0 : Convert.ToDouble(x.DT_6)).ToString(),
                    DT_7 = Lids.Sum(x => x.DT_7 == "" ? 0 : Convert.ToDouble(x.DT_7)).ToString(),
                    DT_8 = Lids.Sum(x => x.DT_8 == "" ? 0 : Convert.ToDouble(x.DT_8)).ToString(),
                    DT_9 = Lids.Sum(x => x.DT_9 == "" ? 0 : Convert.ToDouble(x.DT_9)).ToString(),
                    DT_10 = Lids.Sum(x => x.DT_10 == "" ? 0 : Convert.ToDouble(x.DT_10)).ToString(),
                    DT_11 = Lids.Sum(x => x.DT_11 == "" ? 0 : Convert.ToDouble(x.DT_11)).ToString(),
                    DT_12 = Lids.Sum(x => x.DT_12 == "" ? 0 : Convert.ToDouble(x.DT_12)).ToString(),
                    DT_13 = Lids.Sum(x => x.DT_13 == "" ? 0 : Convert.ToDouble(x.DT_13)).ToString(),
                    DT_14 = Lids.Sum(x => x.DT_14 == "" ? 0 : Convert.ToDouble(x.DT_14)).ToString(),
                    DT_15 = Lids.Sum(x => x.DT_15 == "" ? 0 : Convert.ToDouble(x.DT_15)).ToString(),
                    DT_16 = Lids.Sum(x => x.DT_16 == "" ? 0 : Convert.ToDouble(x.DT_16)).ToString(),
                    DT_17 = Lids.Sum(x => x.DT_17 == "" ? 0 : Convert.ToDouble(x.DT_17)).ToString(),
                    DT_18 = Lids.Sum(x => x.DT_18 == "" ? 0 : Convert.ToDouble(x.DT_18)).ToString(),
                    DT_19 = Lids.Sum(x => x.DT_19 == "" ? 0 : Convert.ToDouble(x.DT_19)).ToString(),
                    DT_20 = Lids.Sum(x => x.DT_20 == "" ? 0 : Convert.ToDouble(x.DT_20)).ToString(),
                    DT_21 = Lids.Sum(x => x.DT_21 == "" ? 0 : Convert.ToDouble(x.DT_21)).ToString(),
                    DT_22 = Lids.Sum(x => x.DT_22 == "" ? 0 : Convert.ToDouble(x.DT_22)).ToString(),
                    DT_23 = Lids.Sum(x => x.DT_23 == "" ? 0 : Convert.ToDouble(x.DT_23)).ToString(),
                    DT_24 = Lids.Sum(x => x.DT_24 == "" ? 0 : Convert.ToDouble(x.DT_24)).ToString(),
                    DT_25 = Lids.Sum(x => x.DT_25 == "" ? 0 : Convert.ToDouble(x.DT_25)).ToString(),
                    DT_26 = Lids.Sum(x => x.DT_26 == "" ? 0 : Convert.ToDouble(x.DT_26)).ToString(),
                    DT_27 = Lids.Sum(x => x.DT_27 == "" ? 0 : Convert.ToDouble(x.DT_27)).ToString(),
                    DT_28 = Lids.Sum(x => x.DT_28 == "" ? 0 : Convert.ToDouble(x.DT_28)).ToString(),
                    DT_29 = Lids.Sum(x => x.DT_29 == "" ? 0 : Convert.ToDouble(x.DT_29)).ToString(),
                    DT_30 = Lids.Sum(x => x.DT_30 == "" ? 0 : Convert.ToDouble(x.DT_30)).ToString(),
                    DT_31 = Lids.Sum(x => x.DT_31 == "" ? 0 : Convert.ToDouble(x.DT_31)).ToString(),
                    TOTAL = Lids.Sum(x => x.TOTAL == "" ? 0 : Convert.ToDouble(x.TOTAL)).ToString(),
                };
                LidA_VIEWs.Add(dA_VIEW1);
            }
            return LidA_VIEWs;
        }
        public async Task<IEnumerable<DA_VIEW>> getDA_VIEWMODELOLD(string dATE1, string cLIENTNAME) {
            List<DA_VIEW> LidA_VIEWs = new List<DA_VIEW>();
            List<DA_VIEW> Lids = new List<DA_VIEW>();
            var DA_ = await _sqlDbContext.DA_CLENTNAME.Where(x => x.BU == 1).OrderBy(x => x.CODE).ToArrayAsync();
            foreach (var item in DA_)
            {
                List<DA_VIEW> LiTs = new List<DA_VIEW>();
                var CliemtName = item.NAME; 
                var valGross1 =_sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTNAME == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM") == dATE1);
                var dtos =   from q in valGross1
                                                from p in _sqlDbContext.DA_BOMs
                                                where q.DT_BOM == p.BOM 
                                                select new DA_GROSSDtos
                                                {
                                                    DT_BOM = q.DT_BOM,
                                                    DT_CLIENTCODE = q.DT_CLIENTCODE,
                                                    DT_CLIENTNAME = q.DT_CLIENTNAME,
                                                    DT_STORENB = q.DT_STORENB,
                                                    DT_DATE = q.DT_DATE.Value.ToString("yyyy-MM-dd"),
                                                    DT_PRICE = q.DT_PRICE,
                                                    DT_MODELNAME = p.MODEL,  
                                                };
                IEnumerable<DA_GROSSDtos> query = dtos.DistinctBy(p => p.DT_MODELNAME);

                var Gl = query.ToList().Select(x=>x.DT_MODELNAME);
                foreach (var item1 in Gl)
                {
                    var Moel =await GetDA_BOMPrice(item1);
                    string DT_MODELNAME1 = item1;
                    var dA_VIEW1 = new DA_VIEW
                    {
                        TYPES = CliemtName,//项目
                        TYPES1 = "整机",
                        MODEL = item1,
                        MODEL1 = Moel.PRICE
                    };
                    var newQuery = dtos.Where(x=>x.DT_MODELNAME== DT_MODELNAME1).GroupBy(p => new { p.DT_DATE, p.DT_CLIENTCODE, p.DT_MODELNAME })
                    .Select(p => new
                    {
                        p.Key.DT_DATE,
                        p.Key.DT_CLIENTCODE,
                        p.Key.DT_MODELNAME,
                        DT_STORENB = p.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)).ToString(),
                    }); 
                    List<string> Val1 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-01")).Select(x=>x.DT_STORENB).ToList();
                    dA_VIEW1.DT_1 = Val1.Count==0?"0":Val1[0].ToString();

                    List<string> Val2 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-02")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_2 = Val2.Count == 0 ? "0" : Val2[0].ToString();

                    List<string> Val3 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-03")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_3 = Val3.Count == 0 ? "0" : Val3[0].ToString();

                    List<string> Val4 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-04")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_4= Val4.Count == 0 ? "0" : Val4[0].ToString();

                    List<string> Val5 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-05")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_5 = Val5.Count == 0 ? "0" : Val5[0].ToString();

                    List<string> Val6 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-06")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_6 = Val6.Count == 0 ? "0" : Val6[0].ToString();

                    List<string> Val7 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-07")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_7 = Val7.Count == 0 ? "0" : Val7[0].ToString();

                    List<string> Val8 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-08")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_8 = Val8.Count == 0 ? "0" : Val8[0].ToString();

                    List<string> Val9 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-09")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_9 = Val9.Count == 0 ? "0" : Val9[0].ToString();

                    List<string> Val10 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-10")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_10 = Val10.Count == 0 ? "0" : Val10[0].ToString();

                    List<string> Val11 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-11")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_11 = Val11.Count == 0 ? "0" : Val11[0].ToString();

                    List<string> Val12 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-12")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_12 = Val12.Count == 0 ? "0" : Val12[0].ToString();

                    List<string> Val13 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-13")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_13 = Val13.Count == 0 ? "0" : Val13[0].ToString();

                    List<string> Val14 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-14")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_14 = Val14.Count == 0 ? "0" : Val14[0].ToString();

                    List<string> Val15 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-15")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_15 = Val15.Count == 0 ? "0" : Val15[0].ToString();

                    List<string> Val16 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-16")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_16 = Val16.Count == 0 ? "0" : Val16[0].ToString();

                    List<string> Val17 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-17")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_17 = Val17.Count == 0 ? "0" : Val17[0].ToString();

                    List<string> Val18 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-18")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_18 = Val18.Count == 0 ? "0" : Val18[0].ToString();

                    List<string> Val19 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-19")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_19 = Val19.Count == 0 ? "0" : Val19[0].ToString();

                    List<string> Val20 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-20")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_20 = Val20.Count == 0 ? "0" : Val20[0].ToString();

                    List<string> Val21 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-21")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_21 = Val21.Count == 0 ? "0" : Val21[0].ToString();

                    List<string> Val22 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-22")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_22 = Val22.Count == 0 ? "0" : Val22[0].ToString();

                    List<string> Val23 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-23")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_23 = Val23.Count == 0 ? "0" : Val23[0].ToString();

                    List<string> Val24 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-24")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_24 = Val24.Count == 0 ? "0" : Val24[0].ToString();

                    List<string> Val25 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-25")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_25 = Val25.Count == 0 ? "0" : Val25[0].ToString();

                    List<string> Val26 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-26")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_26 = Val26.Count == 0 ? "0" : Val26[0].ToString();

                    List<string> Val27 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-27")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_27 = Val27.Count == 0 ? "0" : Val27[0].ToString();

                    List<string> Val28 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-28")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_28 = Val28.Count == 0 ? "0" : Val28[0].ToString();

                    List<string> Val29 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-29")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_29 = Val29.Count == 0 ? "0" : Val29[0].ToString();

                    List<string> Val30 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-30")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_30 = Val30.Count == 0 ? "0" : Val30[0].ToString();

                    List<string> Val31 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-31")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_31 = Val31.Count == 0 ? "0" : Val31[0].ToString();
                    dA_VIEW1.TOTAL = (Convert.ToInt32(dA_VIEW1.DT_1) + Convert.ToInt32(dA_VIEW1.DT_2) + Convert.ToInt32(dA_VIEW1.DT_3) + Convert.ToInt32(dA_VIEW1.DT_4) + Convert.ToInt32(dA_VIEW1.DT_5) + Convert.ToInt32(dA_VIEW1.DT_6) + Convert.ToInt32(dA_VIEW1.DT_7)
                        + Convert.ToInt32(dA_VIEW1.DT_8) + Convert.ToInt32(dA_VIEW1.DT_9) + Convert.ToInt32(dA_VIEW1.DT_10) + Convert.ToInt32(dA_VIEW1.DT_11) + Convert.ToInt32(dA_VIEW1.DT_12) + Convert.ToInt32(dA_VIEW1.DT_13) + Convert.ToInt32(dA_VIEW1.DT_14) + Convert.ToInt32(dA_VIEW1.DT_15)
                        + Convert.ToInt32(dA_VIEW1.DT_16) + Convert.ToInt32(dA_VIEW1.DT_17) + Convert.ToInt32(dA_VIEW1.DT_18) + Convert.ToInt32(dA_VIEW1.DT_19) + Convert.ToInt32(dA_VIEW1.DT_20) + Convert.ToInt32(dA_VIEW1.DT_21) + Convert.ToInt32(dA_VIEW1.DT_22)
                        + Convert.ToInt32(dA_VIEW1.DT_23) + Convert.ToInt32(dA_VIEW1.DT_24) + Convert.ToInt32(dA_VIEW1.DT_25) + Convert.ToInt32(dA_VIEW1.DT_26) + Convert.ToInt32(dA_VIEW1.DT_27) + Convert.ToInt32(dA_VIEW1.DT_28) + Convert.ToInt32(dA_VIEW1.DT_29)
                        + Convert.ToInt32(dA_VIEW1.DT_30) + Convert.ToInt32(dA_VIEW1.DT_31)).ToString();
                    LidA_VIEWs.Add(dA_VIEW1);
                    LiTs.Add(dA_VIEW1);
                }
                var valGross2 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTCODE == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM") == dATE1&&x.DT_NAME=="组件入库"); 
                var dtos2= from q in valGross2
                           from p in _sqlDbContext.DA_BOMs
                           where q.DT_BOM == p.BOM
                           select new DA_GROSSDtos
                           {
                               DT_BOM = q.DT_BOM,
                               DT_CLIENTCODE = q.DT_CLIENTCODE,
                               DT_CLIENTNAME = q.DT_CLIENTNAME,
                               DT_STORENB = q.DT_STORENB,
                               DT_DATE = q.DT_DATE.Value.ToString("yyyy-MM-dd"),
                               DT_PRICE = q.DT_PRICE,
                               DT_MODELNAME = p.MODEL,
                           };
                var query2 = dtos2.DistinctBy(p => p.DT_MODELNAME); 
                var Gl2 = query2.ToList().Select(x => x.DT_MODELNAME);
                foreach (var item1 in Gl2)
                {
                    var Moel = await GetDA_BOMPrice(item1);
                    string DT_MODELNAME1 = item1;
                    DA_VIEW dA_VIEW1 = new DA_VIEW
                    {
                        TYPES = CliemtName,//项目
                        TYPES1 = "CKD/SKD",
                        MODEL = item1,
                        MODEL1 = Moel.PRICE
                    };
                    var newQuery = dtos2.Where(x => x.DT_MODELNAME == DT_MODELNAME1).GroupBy(p => new { p.DT_DATE, p.DT_CLIENTCODE, p.DT_MODELNAME })
                    .Select(p => new
                    {
                        p.Key.DT_DATE,
                        p.Key.DT_CLIENTCODE,
                        p.Key.DT_MODELNAME,
                        DT_STORENB = p.Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB)).ToString(),
                    });
                    List<string> Val1 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-01")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_1 = Val1.Count == 0 ? "0" : Val1[0].ToString();

                    List<string> Val2 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-02")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_2 = Val2.Count == 0 ? "0" : Val2[0].ToString();

                    List<string> Val3 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-03")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_3 = Val3.Count == 0 ? "0" : Val3[0].ToString();

                    List<string> Val4 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-04")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_4 = Val4.Count == 0 ? "0" : Val4[0].ToString();

                    List<string> Val5 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-05")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_5 = Val5.Count == 0 ? "0" : Val5[0].ToString();

                    List<string> Val6 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-06")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_6 = Val6.Count == 0 ? "0" : Val6[0].ToString();

                    List<string> Val7 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-07")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_7 = Val7.Count == 0 ? "0" : Val7[0].ToString();

                    List<string> Val8 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-08")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_8 = Val8.Count == 0 ? "0" : Val8[0].ToString();

                    List<string> Val9 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-09")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_9 = Val9.Count == 0 ? "0" : Val9[0].ToString();

                    List<string> Val10 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-10")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_10 = Val10.Count == 0 ? "0" : Val10[0].ToString();

                    List<string> Val11 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-11")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_11 = Val11.Count == 0 ? "0" : Val11[0].ToString();

                    List<string> Val12 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-12")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_12 = Val12.Count == 0 ? "0" : Val12[0].ToString();

                    List<string> Val13 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-13")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_13 = Val13.Count == 0 ? "0" : Val13[0].ToString();

                    List<string> Val14 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-14")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_14 = Val14.Count == 0 ? "0" : Val14[0].ToString();

                    List<string> Val15 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-15")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_15 = Val15.Count == 0 ? "0" : Val15[0].ToString();

                    List<string> Val16 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-16")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_16 = Val16.Count == 0 ? "0" : Val16[0].ToString();

                    List<string> Val17 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-17")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_17 = Val17.Count == 0 ? "0" : Val17[0].ToString();

                    List<string> Val18 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-18")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_18 = Val18.Count == 0 ? "0" : Val18[0].ToString();

                    List<string> Val19 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-19")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_19 = Val19.Count == 0 ? "0" : Val19[0].ToString();

                    List<string> Val20 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-20")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_20 = Val20.Count == 0 ? "0" : Val20[0].ToString();

                    List<string> Val21 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-21")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_21 = Val21.Count == 0 ? "0" : Val21[0].ToString();

                    List<string> Val22 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-22")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_22 = Val22.Count == 0 ? "0" : Val22[0].ToString();

                    List<string> Val23 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-23")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_23 = Val23.Count == 0 ? "0" : Val23[0].ToString();

                    List<string> Val24 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-24")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_24 = Val24.Count == 0 ? "0" : Val24[0].ToString();

                    List<string> Val25 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-25")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_25 = Val25.Count == 0 ? "0" : Val25[0].ToString();

                    List<string> Val26 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-26")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_26 = Val26.Count == 0 ? "0" : Val26[0].ToString();

                    List<string> Val27 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-27")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_27 = Val27.Count == 0 ? "0" : Val27[0].ToString();

                    List<string> Val28 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-28")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_28 = Val28.Count == 0 ? "0" : Val28[0].ToString();

                    List<string> Val29 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-29")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_29 = Val29.Count == 0 ? "0" : Val29[0].ToString();

                    List<string> Val30 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-30")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_30 = Val30.Count == 0 ? "0" : Val30[0].ToString();

                    List<string> Val31 = newQuery.Where(x => x.DT_DATE == (dATE1 + "-31")).Select(x => x.DT_STORENB).ToList();
                    dA_VIEW1.DT_31 = Val31.Count == 0 ? "0" : Val31[0].ToString();
                    dA_VIEW1.TOTAL = (Convert.ToInt32(dA_VIEW1.DT_1) + Convert.ToInt32(dA_VIEW1.DT_2) + Convert.ToInt32(dA_VIEW1.DT_3) + Convert.ToInt32(dA_VIEW1.DT_4) + Convert.ToInt32(dA_VIEW1.DT_5) + Convert.ToInt32(dA_VIEW1.DT_6) + Convert.ToInt32(dA_VIEW1.DT_7)
                        + Convert.ToInt32(dA_VIEW1.DT_8) + Convert.ToInt32(dA_VIEW1.DT_9) + Convert.ToInt32(dA_VIEW1.DT_10) + Convert.ToInt32(dA_VIEW1.DT_11) + Convert.ToInt32(dA_VIEW1.DT_12) + Convert.ToInt32(dA_VIEW1.DT_13) + Convert.ToInt32(dA_VIEW1.DT_14) + Convert.ToInt32(dA_VIEW1.DT_15)
                        + Convert.ToInt32(dA_VIEW1.DT_16) + Convert.ToInt32(dA_VIEW1.DT_17) + Convert.ToInt32(dA_VIEW1.DT_18) + Convert.ToInt32(dA_VIEW1.DT_19) + Convert.ToInt32(dA_VIEW1.DT_20) + Convert.ToInt32(dA_VIEW1.DT_21) + Convert.ToInt32(dA_VIEW1.DT_22)
                        + Convert.ToInt32(dA_VIEW1.DT_23) + Convert.ToInt32(dA_VIEW1.DT_24) + Convert.ToInt32(dA_VIEW1.DT_25) + Convert.ToInt32(dA_VIEW1.DT_26) + Convert.ToInt32(dA_VIEW1.DT_27) + Convert.ToInt32(dA_VIEW1.DT_28) + Convert.ToInt32(dA_VIEW1.DT_29)
                        + Convert.ToInt32(dA_VIEW1.DT_30) + Convert.ToInt32(dA_VIEW1.DT_31)).ToString();
                    LidA_VIEWs.Add(dA_VIEW1);
                    LiTs.Add(dA_VIEW1);
                }
                var valGross3 = _sqlDbContext.DA_GROSSs.Where(x => x.DT_CLIENTCODE == CliemtName && x.DT_DATE.Value.ToString("yyyy-MM") == dATE1 && x.DT_NAME == "散料入库");
               
                var prices = valGross3.ToList();
                if (prices.Count>0)
                {
                    var price = prices[0].DT_PRICE;
                    
                    var sum1 = valGross3.Where(x =>x.DT_DATE.Value.ToString("yyyy-MM-dd")==(dATE1+"-01")).Sum(x =>x.DT_STORENB==""?0:Convert.ToDouble(x.DT_STORENB));
                    var sum2 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-02")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum3 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-03")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum4 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-04")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum5 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-05")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum6 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-06")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum7 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-07")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum8 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-08")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum9 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-09")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum10 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-10")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum11 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-11")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum12 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-12")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum13 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-13")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum14 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-14")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum15 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-15")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum16 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-16")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum17 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-17")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum18 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-18")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum19 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-19")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum20 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-20")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum21 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-21")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum22 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-22")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum23 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-23")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum24 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-24")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum25 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-25")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum26 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-26")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum27 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-27")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum28 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-28")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum29 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-29")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum30 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-30")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var sum31 = valGross3.Where(x => x.DT_DATE.Value.ToString("yyyy-MM-dd") == (dATE1 + "-31")).Sum(x => x.DT_STORENB == "" ? 0 : Convert.ToDouble(x.DT_STORENB));
                    var tall = sum1 + sum2 + sum3 + sum4 + sum5 + sum6 + sum7 + sum8 + sum9 + sum10 + sum11 + sum12 + sum13 + sum14 + sum15 + sum16 + sum17 + sum18 + sum19 + sum20 + sum21 + sum22 + sum23 + sum24 + sum25 + sum26 + sum27 + sum28 + sum29 + sum30 + sum31;
                    DA_VIEW dA_VIEW1 = new DA_VIEW
                    {
                        TYPES = CliemtName,//项目
                        TYPES1 = "散料",
                        MODEL = CliemtName + "散料",
                        MODEL1 = price,
                        DT_1 = sum1.ToString(),
                        DT_2 = sum2.ToString(),
                        DT_3 = sum3.ToString(),
                        DT_4 = sum4.ToString(),
                        DT_5 = sum5.ToString(),
                        DT_6 = sum6.ToString(),
                        DT_7 = sum7.ToString(),
                        DT_8 = sum8.ToString(),
                        DT_9 = sum9.ToString(),
                        DT_10 = sum10.ToString(),
                        DT_11 = sum11.ToString(),
                        DT_12 = sum12.ToString(),
                        DT_13 = sum13.ToString(),
                        DT_14 = sum14.ToString(),
                        DT_15 = sum15.ToString(),
                        DT_16 = sum16.ToString(),
                        DT_17 = sum17.ToString(),
                        DT_18 = sum18.ToString(),
                        DT_19 = sum19.ToString(),
                        DT_20 = sum20.ToString(),
                        DT_21 = sum21.ToString(),
                        DT_22 = sum22.ToString(),
                        DT_23 = sum23.ToString(),
                        DT_24 = sum24.ToString(),
                        DT_25 = sum25.ToString(),
                        DT_26 = sum26.ToString(),
                        DT_27 = sum27.ToString(),
                        DT_28 = sum28.ToString(),
                        DT_29 = sum29.ToString(),
                        DT_30 = sum30.ToString(),
                        DT_31 = sum31.ToString(),
                        TOTAL = tall.ToString(),
                    };
                    LidA_VIEWs.Add(dA_VIEW1);
                    LiTs.Add(dA_VIEW1);
                }
                //huizong
                if (LiTs.Count>0)
                {
                    DA_VIEW dA_VIEW1 = new DA_VIEW
                    {
                        TYPES = CliemtName,//项目
                        TYPES1 = "项目入库合计",
                        MODEL = "",
                        MODEL1 = "",
                        DT_1 = LiTs.Sum(x => x.DT_1 == "" ? 0 : Convert.ToDouble(x.DT_1))
                                   .ToString(),
                        DT_2 = LiTs.Sum(x => x.DT_2 == "" ? 0 : Convert.ToDouble(x.DT_2)).ToString(),
                        DT_3 = LiTs.Sum(x => x.DT_3 == "" ? 0 : Convert.ToDouble(x.DT_3)).ToString(),
                        DT_4 = LiTs.Sum(x => x.DT_4 == "" ? 0 : Convert.ToDouble(x.DT_4)).ToString(),
                        DT_5 = LiTs.Sum(x => x.DT_5 == "" ? 0 : Convert.ToDouble(x.DT_5)).ToString(),
                        DT_6 = LiTs.Sum(x => x.DT_6 == "" ? 0 : Convert.ToDouble(x.DT_6)).ToString(),
                        DT_7 = LiTs.Sum(x => x.DT_7 == "" ? 0 : Convert.ToDouble(x.DT_7)).ToString(),
                        DT_8 = LiTs.Sum(x => x.DT_8 == "" ? 0 : Convert.ToDouble(x.DT_8)).ToString(),
                        DT_9 = LiTs.Sum(x => x.DT_9 == "" ? 0 : Convert.ToDouble(x.DT_9)).ToString(),
                        DT_10 = LiTs.Sum(x => x.DT_10 == "" ? 0 : Convert.ToDouble(x.DT_10)).ToString(),
                        DT_11 = LiTs.Sum(x => x.DT_11 == "" ? 0 : Convert.ToDouble(x.DT_11)).ToString(),
                        DT_12 = LiTs.Sum(x => x.DT_12 == "" ? 0 : Convert.ToDouble(x.DT_12)).ToString(),
                        DT_13 = LiTs.Sum(x => x.DT_13 == "" ? 0 : Convert.ToDouble(x.DT_13)).ToString(),
                        DT_14 = LiTs.Sum(x => x.DT_14 == "" ? 0 : Convert.ToDouble(x.DT_14)).ToString(),
                        DT_15 = LiTs.Sum(x => x.DT_15 == "" ? 0 : Convert.ToDouble(x.DT_15)).ToString(),
                        DT_16 = LiTs.Sum(x => x.DT_16 == "" ? 0 : Convert.ToDouble(x.DT_16)).ToString(),
                        DT_17 = LiTs.Sum(x => x.DT_17 == "" ? 0 : Convert.ToDouble(x.DT_17)).ToString(),
                        DT_18 = LiTs.Sum(x => x.DT_18 == "" ? 0 : Convert.ToDouble(x.DT_18)).ToString(),
                        DT_19 = LiTs.Sum(x => x.DT_19 == "" ? 0 : Convert.ToDouble(x.DT_19)).ToString(),
                        DT_20 = LiTs.Sum(x => x.DT_20 == "" ? 0 : Convert.ToDouble(x.DT_20)).ToString(),
                        DT_21 = LiTs.Sum(x => x.DT_21 == "" ? 0 : Convert.ToDouble(x.DT_21)).ToString(),
                        DT_22 = LiTs.Sum(x => x.DT_22 == "" ? 0 : Convert.ToDouble(x.DT_22)).ToString(),
                        DT_23 = LiTs.Sum(x => x.DT_23 == "" ? 0 : Convert.ToDouble(x.DT_23)).ToString(),
                        DT_24 = LiTs.Sum(x => x.DT_24 == "" ? 0 : Convert.ToDouble(x.DT_24)).ToString(),
                        DT_25 = LiTs.Sum(x => x.DT_25 == "" ? 0 : Convert.ToDouble(x.DT_25)).ToString(),
                        DT_26 = LiTs.Sum(x => x.DT_26 == "" ? 0 : Convert.ToDouble(x.DT_26)).ToString(),
                        DT_27 = LiTs.Sum(x => x.DT_27 == "" ? 0 : Convert.ToDouble(x.DT_27)).ToString(),
                        DT_28 = LiTs.Sum(x => x.DT_28 == "" ? 0 : Convert.ToDouble(x.DT_28)).ToString(),
                        DT_29 = LiTs.Sum(x => x.DT_29 == "" ? 0 : Convert.ToDouble(x.DT_29)).ToString(),
                        DT_30 = LiTs.Sum(x => x.DT_30 == "" ? 0 : Convert.ToDouble(x.DT_30)).ToString(),
                        DT_31 = LiTs.Sum(x => x.DT_31 == "" ? 0 : Convert.ToDouble(x.DT_31)).ToString(),
                        TOTAL = LiTs.Sum(x => x.TOTAL == "" ? 0 : Convert.ToDouble(x.TOTAL)).ToString(),
                    };
                    LidA_VIEWs.Add(dA_VIEW1);
                    Lids.Add(dA_VIEW1);
                }
            }
            //生产部当日/月入库合计
            if (Lids.Count > 0)
            {
                DA_VIEW dA_VIEW1 = new DA_VIEW
                {
                    TYPES = "生产部当日/月入库合计",//项目
                    TYPES1 = "",
                    MODEL = "",
                    MODEL1 = "",
                    DT_1 = Lids.Sum(x => x.DT_1 == "" ? 0 : Convert.ToDouble(x.DT_1))
                               .ToString(),
                    DT_2 = Lids.Sum(x => x.DT_2 == "" ? 0 : Convert.ToDouble(x.DT_2)).ToString(),
                    DT_3 = Lids.Sum(x => x.DT_3 == "" ? 0 : Convert.ToDouble(x.DT_3)).ToString(),
                    DT_4 = Lids.Sum(x => x.DT_4 == "" ? 0 : Convert.ToDouble(x.DT_4)).ToString(),
                    DT_5 = Lids.Sum(x => x.DT_5 == "" ? 0 : Convert.ToDouble(x.DT_5)).ToString(),
                    DT_6 = Lids.Sum(x => x.DT_6 == "" ? 0 : Convert.ToDouble(x.DT_6)).ToString(),
                    DT_7 = Lids.Sum(x => x.DT_7 == "" ? 0 : Convert.ToDouble(x.DT_7)).ToString(),
                    DT_8 = Lids.Sum(x => x.DT_8 == "" ? 0 : Convert.ToDouble(x.DT_8)).ToString(),
                    DT_9 = Lids.Sum(x => x.DT_9 == "" ? 0 : Convert.ToDouble(x.DT_9)).ToString(),
                    DT_10 = Lids.Sum(x => x.DT_10 == "" ? 0 : Convert.ToDouble(x.DT_10)).ToString(),
                    DT_11 = Lids.Sum(x => x.DT_11 == "" ? 0 : Convert.ToDouble(x.DT_11)).ToString(),
                    DT_12 = Lids.Sum(x => x.DT_12 == "" ? 0 : Convert.ToDouble(x.DT_12)).ToString(),
                    DT_13 = Lids.Sum(x => x.DT_13 == "" ? 0 : Convert.ToDouble(x.DT_13)).ToString(),
                    DT_14 = Lids.Sum(x => x.DT_14 == "" ? 0 : Convert.ToDouble(x.DT_14)).ToString(),
                    DT_15 = Lids.Sum(x => x.DT_15 == "" ? 0 : Convert.ToDouble(x.DT_15)).ToString(),
                    DT_16 = Lids.Sum(x => x.DT_16 == "" ? 0 : Convert.ToDouble(x.DT_16)).ToString(),
                    DT_17 = Lids.Sum(x => x.DT_17 == "" ? 0 : Convert.ToDouble(x.DT_17)).ToString(),
                    DT_18 = Lids.Sum(x => x.DT_18 == "" ? 0 : Convert.ToDouble(x.DT_18)).ToString(),
                    DT_19 = Lids.Sum(x => x.DT_19 == "" ? 0 : Convert.ToDouble(x.DT_19)).ToString(),
                    DT_20 = Lids.Sum(x => x.DT_20 == "" ? 0 : Convert.ToDouble(x.DT_20)).ToString(),
                    DT_21 = Lids.Sum(x => x.DT_21 == "" ? 0 : Convert.ToDouble(x.DT_21)).ToString(),
                    DT_22 = Lids.Sum(x => x.DT_22 == "" ? 0 : Convert.ToDouble(x.DT_22)).ToString(),
                    DT_23 = Lids.Sum(x => x.DT_23 == "" ? 0 : Convert.ToDouble(x.DT_23)).ToString(),
                    DT_24 = Lids.Sum(x => x.DT_24 == "" ? 0 : Convert.ToDouble(x.DT_24)).ToString(),
                    DT_25 = Lids.Sum(x => x.DT_25 == "" ? 0 : Convert.ToDouble(x.DT_25)).ToString(),
                    DT_26 = Lids.Sum(x => x.DT_26 == "" ? 0 : Convert.ToDouble(x.DT_26)).ToString(),
                    DT_27 = Lids.Sum(x => x.DT_27 == "" ? 0 : Convert.ToDouble(x.DT_27)).ToString(),
                    DT_28 = Lids.Sum(x => x.DT_28 == "" ? 0 : Convert.ToDouble(x.DT_28)).ToString(),
                    DT_29 = Lids.Sum(x => x.DT_29 == "" ? 0 : Convert.ToDouble(x.DT_29)).ToString(),
                    DT_30 = Lids.Sum(x => x.DT_30 == "" ? 0 : Convert.ToDouble(x.DT_30)).ToString(),
                    DT_31 = Lids.Sum(x => x.DT_31 == "" ? 0 : Convert.ToDouble(x.DT_31)).ToString(),
                    TOTAL = Lids.Sum(x => x.TOTAL == "" ? 0 : Convert.ToDouble(x.TOTAL)).ToString(),
                };
                LidA_VIEWs.Add(dA_VIEW1); 
            }
            return LidA_VIEWs; 
        }

        #region 统计一段时间内有多少个星期几
        ///<summary> 
        ///统计一段时间内有多少个星期几 
        ///</summary> 
        ///<param name="AStart">开始日期</param> 
        ///<param name="AEnd">结束日期</param> 
        ///<param name="AWeek">星期几</param> 
        ///<returns>返回个数</returns> 
        public static int TotalWeeks(DateTime AStart, DateTime AEnd, DayOfWeek AWeek)
        {
            TimeSpan vTimeSpan = new TimeSpan(AEnd.Ticks - AStart.Ticks);
            int Result = (int)vTimeSpan.TotalDays / 7;
            for (int i = 0; i <= vTimeSpan.TotalDays % 7; i++)
                if (AStart.AddDays(i).DayOfWeek == AWeek)
                    return Result + 1;
            return Result;
        }

        public async Task<IEnumerable<DA_CLENTCODE>> GetDA_CLENTCODEs()
        {
            return await _sqlDbContext.DA_CLENTCODE.ToListAsync(); 
        }

        public async Task<IEnumerable<DA_STAORE>> GetDA_STAOREs()
        {
            return await _sqlDbContext.DA_STAORE.ToListAsync();
        }

        public async Task<DA_BOM> GetDA_BOMPrice(string Boms)
        {
            return await _sqlDbContext.DA_BOMs
                .FirstOrDefaultAsync(x => x.MODEL == Boms);
        }
        public async Task<DA_BOM> GetDA_BOMPrice1(string Boms)
        {
            return await _sqlDbContext.DA_BOMs
                .FirstOrDefaultAsync(x => x.BOM == Boms);
        }
        public void ADD_INSTORE(DT_INSTORE dT_INSTORE)
        {
            _sqlDbContext.DT_INSTORE.Add(dT_INSTORE);
        }

        public async Task<IEnumerable<DT_INSTORE>> GetINSTAL(string cLIENTNAME, string mODEL, string dATE1, string dATE2)
        {
            var items = _sqlDbContext.DT_INSTORE as IQueryable<DT_INSTORE>;
            if (!string.IsNullOrWhiteSpace(mODEL))
            {
                items = items.Where(x => x.DT_MODEL.ToString() == mODEL);
            }
            if (!string.IsNullOrWhiteSpace(cLIENTNAME))
            {
                items = items.Where(x => x.DT_NAME == cLIENTNAME);
            }
            if (!string.IsNullOrWhiteSpace(dATE1))
            {
                var date1 = Convert.ToDateTime(dATE1);
                var date2 = Convert.ToDateTime(dATE2);
                items = items.Where(x => x.DT_DATE >= date1 && x.DT_DATE <= date2);
            }
            return await items.ToListAsync();
        }

        public async Task<DT_INSTORE> GetINSTORE(string Guids)
        {
            var item = _sqlDbContext.DT_INSTORE.Where(x => x.ID.ToString() == Guids).FirstOrDefaultAsync();
            return await item;
        }

        public void DE_INSTORE(DT_INSTORE dA_GROSS)
        {
            _sqlDbContext.DT_INSTORE.Remove(dA_GROSS);
        }

        public void AddDA_DATEINSTORE(DA_DATEINSTORE dA_DATEINSTORE)
        {
            if (dA_DATEINSTORE == null)
            {
                throw new ArgumentNullException(nameof(dA_DATEINSTORE));
            }
            dA_DATEINSTORE.ID = Guid.NewGuid();
            dA_DATEINSTORE.CRT_DATE = DateTime.Now;
            _sqlDbContext.DA_DATEINSTORE.Add(dA_DATEINSTORE);
        }

        public void AddDA_ATTENDANCE(DA_ATTENDANCE dA_PAYMONTH)
        {
            _sqlDbContext.DA_ATTENDANCE.Add(dA_PAYMONTH);
        }
        #endregion
    }
}
