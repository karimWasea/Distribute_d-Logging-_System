using Distributed__Logging__System_.Application._LogEntry;
using Distributed__Logging__System_.Application._LogEntry.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Distribute_d_Logging__System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
     [Authorize]
    public class LogEntryController : ControllerBase
    {

        private readonly ILogEntryRepository _logEntryRepository;
        private readonly S3Service _s3Service;

        // Constructor to inject the repository
        public LogEntryController(ILogEntryRepository logEntryRepository , S3Service s3Service)
        {
            _s3Service = s3Service;

            _logEntryRepository = logEntryRepository;
        }

       

            [HttpGet]
        public async Task<ActionResult<IEnumerable<LogEntryDto>>> GetAll( LogEntrySearchDto logEntrySearchDto)
        {
            try
            {
                // Retrieve all log entries with optional filters and pagination
                //await _s3Service.UploadLogAsync(logEntrySearchDto.Service, logEntrySearchDto.Level, logEntrySearchDto
                //.Level);

                var logEntries = await _logEntryRepository.GetLogEntriesAsync(logEntrySearchDto);

                return Ok(logEntries);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a meaningful response
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

      


        [HttpGet]
        [Route("logs/{logFileName}")]
        public async Task<IActionResult> RetrieveLog(string logFileName)
        {
            try
            {
                // Retrieve log from S3
                var logContent = await _s3Service.DownloadLogAsync(logFileName);
                return Ok(logContent);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving log: {ex.Message}");
            }
        }









        // GET: api/LogEntry/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogEntryDto>> GetLogEntry(int id)
        {
            var logEntry = await _logEntryRepository.GetByIdAsync(id);

            if (logEntry == null)
            {
                return NotFound(new { Message = $"Log entry with ID {id} not found." });
            }

            return Ok(logEntry);
        }

        // POST: api/LogEntry
        [HttpPost]
        public async Task<ActionResult<LogEntryDto>> CreateLogEntry( LogEntryCreateDto logEntryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _logEntryRepository.AddAsync(logEntryDto);
            return CreatedAtAction(nameof(GetLogEntry),   logEntryDto);
        }

        // PUT: api/LogEntry/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLogEntry(LogEntryUpdateDto logEntryDto)
        {
           

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingLogEntry = await _logEntryRepository.GetByIdAsync(logEntryDto.Id);
            if (existingLogEntry == null)
            {
                return NotFound(new { Message = $"Log entry with ID {logEntryDto.Id} not found." });
            }

            await _logEntryRepository.UpdateAsync(logEntryDto);
            return NoContent();
        }

        // DELETE: api/LogEntry/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogEntry(int id)
        {
            var logEntry = await _logEntryRepository.GetByIdAsync(id);
            if (logEntry == null)
            {
                return NotFound(new { Message = $"Log entry with ID {id} not found." });
            }

            await _logEntryRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
