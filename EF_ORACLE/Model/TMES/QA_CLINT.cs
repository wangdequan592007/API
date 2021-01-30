using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF_ORACLE.Model.TMES
{
    [Table("QA_CLINT")]
    public class QA_CLINT
    {
        [Key]
        [MaxLength(50)]
        [Column("CODE")]
        public string CODE { get; set; }
        [MaxLength(50)]
        [Column("NAME")]
        public string NAME { get; set; }
    }
}
