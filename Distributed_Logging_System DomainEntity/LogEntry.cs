﻿namespace Distributed_Logging_System_DomainEntity
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string Service { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty; 
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
      
    }

}
