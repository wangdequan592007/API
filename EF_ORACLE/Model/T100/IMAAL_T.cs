using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.T100
{
    [Table("IMAAL_T")]
    public class IMAAL_T
    { 
        [MaxLength(40)]
        [Column("IMAAL001")]
        public string IMAAL001 { get; set; }
        [MaxLength(4000)]
        [Column("IMAAL003")]
        public string IMAAL003 { get; set; }
        [MaxLength(4000)]
        [Column("IMAAL004")]
        public string IMAAL004 { get; set; }  
        [MaxLength(6)]
        [Column("IMAAL002")]
        public string IMAAL002 { get; set; }
        [Column("IMAALENT")]
        public int? IMAALENT { get; set; }
    }
}
