using AutoMapper;

public class LogEntryProfile : Profile
{
	public LogEntryProfile()
	{
		CreateMap<CreateLogEntryDto, LogEntry>().ReverseMap();
		CreateMap<LogEntryDto, LogEntry>().ReverseMap();

	}
}
