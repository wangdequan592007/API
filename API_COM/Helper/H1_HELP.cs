using API_COM.Modelclass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_COM.Helper
{
    public class H1_HELP
    {
        public UNI_BSS_HEAD UNI_BSS_HEAD { get;  set; }
        public UNI_BSS_BODY UNI_BSS_BODY { get; set; }
        public UNI_BSS_ATTACHED UNI_BSS_ATTACHED { get; set; }
    }
    public class UNI_BSS_HEAD
    {
        public string APP_ID { get; set; }
        public string TIMESTAMP { get; set; }
        public string TRANS_ID { get; set; }
        public string TOKEN { get; set; }
        public List<RESERVED> RESERVED { get; set; } 
    }
    public class UNI_BSS_BODY
    {
        public RECEIVE_DELIVERY_REQ RECEIVE_DELIVERY_REQ { get; set; }
    }
    public class RECEIVE_DELIVERY_REQ
    {
        public receiveDelivery data { get; set; }
        
    }
    public class RESERVED
    {
        public string RESERVED_VALUE { get; set; }
        public string RESERVED_ID { get; set; }
    }
    public class UNI_BSS_ATTACHED
    {
        public string MEDIA_INFO { get; set; }
    }
}
