using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_MES.Model
{
    [Table("Employee")]
    public class Employee23
    {
        [Key]
        public Guid DepartmentId { get; set; }
        public string Code { get; set; }
        public string CnName { get; set; }
    }
}
