using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VG.Pm.PmDb.Shared
{
    public interface IChangeLog
    {
        public string? ChangeLogJson { get; set; }
    }
}
