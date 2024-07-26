using AutoMapper;

public class LogEntryProfile : Profile
{
	public LogEntryProfile()
	{
		CreateMap<LogEntryPost, LogEntry>();
	}
}
