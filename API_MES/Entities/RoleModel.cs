using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Entities
{
    public class RoleModel
    {
        public string RoleName { get; set; }
        public List<OneApiModel> Apis { get; set; }
    }
}
