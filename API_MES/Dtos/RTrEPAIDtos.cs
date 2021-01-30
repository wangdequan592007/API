using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Dtos
{
    public class RTrEPAIDtos
    {
        public bool success { get; set; }
        public int AllCount { get; set; }
        public int RECOUNT { get; internal set; }
        public int SEOKCOUNT { get; internal set; }
        public int REOKCOUNT { get; internal set; }
        public int RTOKCOUNT { get; internal set; }
    }
}
