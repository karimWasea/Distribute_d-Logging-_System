using Distributed__Logging__System_.Application._LogEntry.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distributed__Logging__System_.Application._LogEntry
{
    public interface ILogEntryRepository
    {
        
            Task<IEnumerable<LogEntryDto>> GetLogEntriesAsync(LogEntrySearchDto searchDto);
            Task<LogEntryDto> GetByIdAsync(int id);
            Task AddAsync(LogEntryCreateDto logEntry);
            Task UpdateAsync(LogEntryUpdateDto logEntry);
            Task DeleteAsync(int id);
         

    }
}
