using AutoMapper;

using Distributed__Logging__System_.Application._LogEntry.Dtos;

using Distributed_Logging_System_DomainEntity;


namespace Distributed__Logging__System_.Application._LogEntry.Dtos
{
    public class LogEntryMappingProfile : Profile
    {
        public LogEntryMappingProfile()
        {
            // Map from LogEntryCreateDto to LogEntryDto (for creating a log entry)
            CreateMap<LogEntryCreateDto, LogEntry>()
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => DateTime.UtcNow)); // If needed to set the timestamp automatically

            // Map from LogEntryDto to LogEntryUpdateDto (for updating a log entry)
            CreateMap<LogEntry, LogEntryUpdateDto>().ReverseMap();
            CreateMap<LogEntry, LogEntryDto>();

            // You can add more mappings if needed
        }
    }
}
