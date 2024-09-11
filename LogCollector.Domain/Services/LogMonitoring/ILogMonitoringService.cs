public interface ILogMonitoringService
{
	Task CheckLogsAsync();

	Task HandleMonitoringAlertAsync(Monitor monitor, List<LogEntry> monitorLogs);
}