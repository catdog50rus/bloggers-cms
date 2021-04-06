using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pds.Data.Entities
{
    public class LogRecord : EntityBase
    {
        public string Message { get; set; }

        public string Exception { get; set; }

        public string CallSite { get; set; }

        public string Logger { get; set; }

        public string Level { get; set; }
    }
}
