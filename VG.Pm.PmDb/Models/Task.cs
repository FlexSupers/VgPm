using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VG.Pm.PmDb.Shared;

namespace VG.Pm.PmDb.Models
{
    [Table("Task")]
    public class Task : IChangeLog
    {
        [Key]
        public int TaskId { get; set; }
        public string Title { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
        public string? ChangeLogJson { get; set; }
        public int ProjectId { get; set; }
        public int StatusId { get; set; }
        public string Description { get; set; }
        public int TaskTypeId { get; set; }
    }
}
