using System;
using System.ComponentModel.DataAnnotations;

namespace API_MES.Model
{
    public class IMGFIRST
    {
        public Guid ID { get; set; }
        public string MO { get; set; }
        public string LINE { get; set; }
        public string MTYPE { get; set; }
        public string IMGNAME { get; set; }
    }
}
