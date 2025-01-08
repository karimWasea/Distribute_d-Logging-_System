using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distributed__Logging__System_.Application._LogEntry.Dtos
{
    public class LogEntrySearchDto : pagedseach
    {
        public string? Service { get; set; } = string.Empty;
        public string? Level { get; set; } = string.Empty;
        public DateTime? StartTimestamp { get; set; }
        public DateTime? EndTimestamp { get; set; }
        public string? Message { get; set; } = string.Empty;

        // Pagination properties

    }

    public class pagedseach
    {
        public int PageNumber { get; set; } = 1;  // Default to first page
        public int PageSize { get; set; } = 10;   // Default to 10 results per page    }
    }
}
