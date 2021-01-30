using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Entities
{
    public class UserModel
    { /// <summary>
      /// 用户名
      /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 所属角色
        /// </summary>
        public List<string> BeRoles { get; set; }
    }
}
