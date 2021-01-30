using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_ORACLE
{
    [Table("SFBA_T")]
    public class SFBA_T
    {
        [MaxLength(40)]
        [Column("SFBA006")]
        public string SFBA006 { get; set; }
        [MaxLength(20)]
        [Column("SFBADOCNO")]
        public string SFBADOCNO { get; set; }
        [Column("SFBASEQ")] 
        public int? SFBASEQ { get; set; }
        [Column("SFBAENT")]
        public int? SFBAENT { get; set; }
    }
}
