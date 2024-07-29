using AutoMapper;

public class LogEntryProfile : Profile
{
	public LogEntryProfile()
	{
		CreateMap<LogEntryDto, LogEntry>().ReverseMap();
		CreateMap<BaseLogEntryDto, LogEntry>().ReverseMap();

		CreateMap<MonitorDto, Monitor>().ReverseMap();
		CreateMap<MonitorDto, Monitor>().ReverseMap();
		CreateMap<BaseMonitorDto, Monitor>().ReverseMap();

		CreateMap<AlertDto, Alert>().ReverseMap();
		CreateMap<AlertDto, Alert>().ReverseMap();
		CreateMap<BaseAlertDto, Alert>().ReverseMap();

	}
}
