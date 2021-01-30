using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_MES.Dtos;

namespace API_MES.Entities
{
    public class RT_MESSAGE
    {
        public string Err { get; set; }
        public bool success { get; set; }
        public List<DA_ODM_PRESSURE> dA_ODM_PRESSURE { get; set; }
        public List<DA_ODM_PRESSFREE> dA_ODM_PRESSFREE { get; internal set; }
    }
}
