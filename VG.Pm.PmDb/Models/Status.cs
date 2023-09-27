using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VG.Pm.PmDb.Models
{
    [Table("Status")]
    public class Status
    {
        [Key]
        public int StatusId { get; set; }
        public string Title { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
        public string? ChangeLogJson { get; set; }
        public int OrderId { get; set; }
    }
}
