using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF_ORACLE
{
    [Table("ODM_PRESSURETIME")]
    public class ODM_PRESSURETIME
    {
        [Key]
        [MaxLength(80)]
        [Column("CLIENCODE")]
        public string CLIENCODE { get; set; }
        [Column("DTTIME")]
        [MaxLength(50)]
        public string DTTIME { get; set; }
        [Column("NUMCODE")]
        public int? NUMCODE { get; set; }
        [Column("WGSTATION")]
        public int? WGSTATION { get; set; }
        [Column("MODEL")]
        public int? MODEL { get; set; }

    }
}
