using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF_ORACLE.Model.TMES
{
    [Table("ODM_BEATE")]
    public class ODM_BEATE
    {
        [Key]
        [MaxLength(64)]
        [Column("LINECODE")]
        public string LINECODE { get; set; }
        [MaxLength(50)]
        [Column("INPUTMAN")]
        public string INPUTMAN { get; set; }
        [Column("INPUTDATE", TypeName = "DATE")]
        public DateTime? INPUTDATE { get; set; }
        [MaxLength(64)]
        [Column("BEATE_MIN")]
        public string BEATE_MIN { get; set; }
        [MaxLength(64)]
        [Column("BEATE_NUM")]
        public string BEATE_NUM { get; set; }
        [MaxLength(64)]
        [Column("BEATE_MAX")]
        public string BEATE_MAX { get; set; }
        [MaxLength(64)]
        [Column("BEATE_TOP")]
        public string BEATE_TOP { get; set; }
    }
}
