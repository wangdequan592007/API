using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE.Model.TMES
{
    [Table("MTL_SUB_ATTEMPER")]
    public class MTL_SUB_ATTEMPER
    {
        [MaxLength(64)]
        [Column("ATTEMPTER_CODE")]
        public string ATTEMPTER_CODE { get; set; }
        [MaxLength(64)]
        [Column("TESTPOSITION")]
        public string TESTPOSITION { get; set; }
        [Column("SERIAL_NUMBER")]
        [MaxLength(5)]
        public int? SERIAL_NUMBER { get; set; }
        [MaxLength(64)]
        [Column("WORK_CODE")]
        public string WORK_CODE { get; set; }
    }
}
