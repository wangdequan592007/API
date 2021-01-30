using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF_ORACLE.Model.TMES
{
    [Table("RE_MODEL")] 
     public class RE_MODEL
    {
        [Key]
        [MaxLength(50)]
        [Column("CODE")]
        public string CODE { get; set; }
        [MaxLength(240)]
        [Column("NAME")]
        public string NAME { get; set; }
    }
}
