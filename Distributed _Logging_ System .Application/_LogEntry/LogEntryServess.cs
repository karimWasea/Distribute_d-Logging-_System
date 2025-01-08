using AutoMapper;
using Distributed__Logging__System.Utalites;
using Distributed__Logging__System_.Application._LogEntry.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Distributed_Logging_System_DomainEntity;
using Microsoft.EntityFrameworkCore;
using Distributed__Logging__System_.Application._LogEntry;

namespace Distributed_Logging_System_.Application._LogEntry
{
    public class LogEntryServess : ILogEntryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LogEntryServess> _logger;
        private readonly IMapper _mapper;

        // Constructor with logger and AutoMapper dependency injection
        public LogEntryServess(ILogger<LogEntryServess> logger, IMapper mapper, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _context = applicationDbContext;
        }

        // Add a log entry
        public async Task AddAsync(LogEntryCreateDto logEntryDto)
        {
            try
            {
                var newLogEntry = _mapper.Map<LogEntry>(logEntryDto);
                _context.logEntries.Add(newLogEntry);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Log entry added: {newLogEntry.Id}");
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error occurred while adding log entry.");
            }
        }

        // Delete a log entry by ID
        public async Task DeleteAsync(int id)
        {
            try
            {
                var logEntry = await _context.logEntries.FirstOrDefaultAsync(x => x.Id == id);
                if (logEntry == null)
                {
                    _logger.LogWarning($"Log entry with ID {id} not found for deletion.");
                    throw new NotFoundException($"Log entry with ID {id} not found.");
                }

                _context.logEntries.Remove(logEntry);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Log entry deleted: {id}");
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error occurred while deleting log entry.");
            }
        }

        // Get a log entry by ID
        public async Task<LogEntryDto> GetByIdAsync(int id)
        {
            try
            {
                var logEntry = await _context.logEntries.FirstOrDefaultAsync(x => x.Id == id);
                if (logEntry == null)
                {
                    _logger.LogWarning($"Log entry with ID {id} not found.");
                    throw new NotFoundException($"Log entry with ID {id} not found.");
                }

                _logger.LogInformation($"Log entry retrieved: {logEntry.Id}");
                return _mapper.Map<LogEntryDto>(logEntry);
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error occurred while retrieving log entry.");
            }
            return null; // Fallback to null in case of error
        }

        // Get all log entries or filter by search criteria
        public async Task<IEnumerable<LogEntryDto>> GetlogEntriesAsync(LogEntrySearchDto searchDto)
        {
            try
            {
                var query = _context.logEntries.AsQueryable();

                // Apply filters based on the search criteria
                if (!string.IsNullOrEmpty(searchDto.Service))
                {
                    query = query.Where(x => x.Service.Contains(searchDto.Service));
                }

                if (!string.IsNullOrEmpty(searchDto.Level))
                {
                    query = query.Where(x => x.Level.Contains(searchDto.Level));
                }

                var logEntries = await query.ToListAsync();
                _logger.LogInformation($"Retrieved {logEntries.Count} log entries.");
                return _mapper.Map<IEnumerable<LogEntryDto>>(logEntries);
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error occurred while retrieving log entries.");
            }
            return Enumerable.Empty<LogEntryDto>(); // Return empty collection on error
        }

        public async Task<IEnumerable<LogEntryDto>> GetLogEntriesAsync(LogEntrySearchDto searchDto)
        {
            try
            {
                // Start with all log entries from the database
                var query = _context.logEntries.AsQueryable();

                // Apply filters based on the search criteria
                if (!string.IsNullOrEmpty(searchDto.Service))
                {
                    query = query.Where(x => x.Service.Contains(searchDto.Service));
                }

                if (!string.IsNullOrEmpty(searchDto.Level))
                {
                    query = query.Where(x => x.Level.Contains(searchDto.Level));
                }

                if (searchDto.StartTimestamp.HasValue)
                {
                    query = query.Where(x => x.Timestamp >= searchDto.StartTimestamp.Value);
                }

                if (searchDto.EndTimestamp.HasValue)
                {
                    query = query.Where(x => x.Timestamp <= searchDto.EndTimestamp.Value);
                }

                // Get the total number of records for pagination
                var totalRecords = await query.CountAsync();

                // Fetch the log entries, applying pagination
                var logEntries = await query
                    .OrderBy(e => e.Service)  // Sorting by Service or another relevant field
                    .Skip((searchDto.PageNumber - 1) * searchDto.PageSize)
                    .Take(searchDto.PageSize)
                    .ToListAsync();

                // Map the results to LogEntryDto using AutoMapper
                var logEntryDtos = _mapper.Map<IEnumerable<LogEntryDto>>(logEntries);

                // Log the count of retrieved log entries
                _logger.LogInformation($"Retrieved {logEntryDtos.Count()} log entries on page {searchDto.PageNumber} of {totalRecords} total records.");

                return logEntryDtos;
            }
            catch (Exception ex)
            {
                // Log the error and handle exceptions
                _logger.LogError(ex, "Error occurred while retrieving log entries.");
                return Enumerable.Empty<LogEntryDto>();  // Return an empty collection in case of an error
            }
        }



        // Update an existing log entry
        public async Task UpdateAsync(LogEntryUpdateDto logEntryDto)
        {
            try
            {
                var existingLogEntry = await _context.logEntries.FirstOrDefaultAsync(x => x.Id == logEntryDto.Id);
                if (existingLogEntry == null)
                {
                    _logger.LogWarning($"Log entry with ID {logEntryDto.Id} not found for update.");
                    throw new NotFoundException($"Log entry with ID {logEntryDto.Id} not found.");
                }

                _mapper.Map(logEntryDto, existingLogEntry);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Log entry updated: {existingLogEntry.Id}");
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error occurred while updating log entry.");
            }
        }

        // Centralized error handling method
        private void HandleError(Exception ex, string message)
        {
            _logger.LogError(ex, message);
            throw new ApplicationException(message, ex);
        }
    }
}
