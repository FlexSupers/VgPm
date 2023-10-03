using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VG.Pm.PmDb.Models
{
    [Table("TaskType")]
    public class TaskType
    {
        [Key]
        public int TaskTypeId { get; set; }
        public string Title { get; set; }
        public string? ColorPicker { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
        public string? ChangeLogJson { get; set; }
    }
}
