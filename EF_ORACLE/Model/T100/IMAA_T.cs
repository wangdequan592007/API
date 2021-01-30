using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE
{
    [Table("IMAA_T")]
    public class IMAA_T
    {
        [MaxLength(40)]
        [Column("IMAA001")]
        public string IMAA001 { get; set; }
        [MaxLength(4000)]
        [Column("IMAAUD009")]
        public string IMAAUD009 { get; set; }
        [MaxLength(4000)]
        [Column("IMAAUD010")]
        public string IMAAUD010 { get; set; } 
        [Column("IMAAENT")]
        public int? IMAAENT { get; set; }

        
    }

}
