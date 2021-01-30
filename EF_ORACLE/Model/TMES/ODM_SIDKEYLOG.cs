using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("ODM_SIDKEYLOG")]
    public class ODM_SIDKEYLOG
    {
        
        [Column("FILENAME")]
        [MaxLength(50)]
        public string FILENAME { get; set; }
        [Column("SNCOUNT")]
        [MaxLength(50)]
        public string SNCOUNT { get; set; }
        [Key]
        [Column("KEY")]
        [MaxLength(50)]
        public string KEY { get; set; }
        [Column("CREATE_USER")]
        [MaxLength(50)]
        public string CREATE_USER { get; set; }
        [Column("CREATE_DATE", TypeName = "DATE")]
        public DateTime? CREATE_DATE { get; set; }
        [Column("PRODUCT_MODEL")]
        [MaxLength(50)]
        public string PRODUCT_MODEL { get; set; }
    }
}
