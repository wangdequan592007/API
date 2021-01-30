using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF_ORACLE.Model.TMES
{
    [Table("ODM_TESTFAILNUM")]
    public class ODM_TESTFAILNUM
    {
        [Key]
        [MaxLength(50)]
        [Column("STATION")]
        public string STATION { get; set; }

        [Column("FAILNUM")]
        public int? FAILNUM { get; set; }
    }
}
