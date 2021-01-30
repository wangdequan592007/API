using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("BARCODEREMP")]
    public class BARCODEREMP
    {
        [Key] 
        [MaxLength(64)]
        [Column("BARCODE")]
        public string BARCODE { get; set; }
        [MaxLength(64)]
        [Column("WORKCODE")]
        public string WORKCODE { get; set; }
        [MaxLength(64)]
        [Column("LINKSN")]
        public string LINKSN { get; set; }
    }
}
