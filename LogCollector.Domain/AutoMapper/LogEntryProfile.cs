using AutoMapper;

public class LogEntryProfile : Profile
{
	public LogEntryProfile()
	{
		CreateMap<CreateLogEntryDto, LogEntry>().ReverseMap();
		CreateMap<LogEntryDto, LogEntry>().ReverseMap();

		CreateMap<CreateMonitorDto, Monitor>().ReverseMap();
		CreateMap<UpdateMonitorDto, Monitor>().ReverseMap();
		CreateMap<MonitorDto, Monitor>().ReverseMap();

		CreateMap<CreateAlertDto, Alert>().ReverseMap();
		CreateMap<UpdateAlertDto, Alert>().ReverseMap();
		CreateMap<AlertDto, Alert>().ReverseMap();

	}
}
