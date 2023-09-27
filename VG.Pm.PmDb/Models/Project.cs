using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VG.Pm.PmDb.Models
{
    [Table("Project")]
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public string Title { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
        public string? ChangeLogJson { get; set; }

    }
}
