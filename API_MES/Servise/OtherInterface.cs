using API_MES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_MES.Servise
{
    public interface OtherInterface
    {
        Task<ErrMessage> DEL_LENOVOSFTP();
    }
}
