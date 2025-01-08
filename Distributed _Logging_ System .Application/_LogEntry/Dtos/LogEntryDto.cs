using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distributed__Logging__System_.Application._LogEntry.Dtos
{
    public class LogEntryDto
    {
        public int Id { get; set; }
        public string Service { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
