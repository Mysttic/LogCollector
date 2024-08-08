using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Dynamic.Core;
using System.Text.Json;

public class LogMonitoringService
{
	private readonly LogCollectorDbContext _logCollectorDbContext;
	private readonly IConfiguration _configuration;
	private readonly IBackgroundJobClient _backgroundJobClient;
	private readonly IEmailService _emailService;
	private readonly ISMSService _smsService;
	private readonly ICustomApiCallService _customApiCallService;

	public LogMonitoringService(LogCollectorDbContext logCollectorDbContext, 
		IConfiguration configuration, 
		IBackgroundJobClient backgroundJobClient, 
		IEmailService emailService,
		ISMSService smsService,
		ICustomApiCallService customApiCallService)
	{
		_logCollectorDbContext = logCollectorDbContext;
		_configuration = configuration;
		_backgroundJobClient = backgroundJobClient;
		_emailService = emailService;
		_smsService = smsService;
		_customApiCallService = customApiCallService;
	}

	public async Task CheckLogsAsync()
	{
		List<LogEntry> logEntries = await _logCollectorDbContext.Logs.AsNoTracking().ToListAsync();
		if (logEntries.Count == 0)
		{
			Console.WriteLine("No logs to check");
			return;
		}
		foreach (Monitor monitor in _logCollectorDbContext.Monitors.AsNoTracking().Where(m => !string.IsNullOrEmpty(m.Query)))
		{
			List<LogEntry> monitorLogs = logEntries.AsQueryable().Where(l=>l.CreatedAt > monitor.LastInvoke).Where(monitor.Query).ToList();
			if (monitorLogs.Count() > 0) // monitor.Threshold) // można na przyszłość dodać jakiś warunek kiedy ma reagować na określoną ilość logów
			{
				HandleMonitoringAlert(monitor, monitorLogs);
			}
		}
		await _logCollectorDbContext.SaveChangesAsync();

	}

	public void HandleMonitoringAlert(Monitor monitor, List<LogEntry> monitorLogs)
	{
		var alert = new Alert
		{
			MonitorId = monitor.Id,
			Message = $"Monitor {monitor.Name} has detected {monitorLogs.Count} logs using query {monitor.Query}",
			Content = JsonSerializer.Serialize(monitorLogs),
		};
		_logCollectorDbContext.Alerts.Add(alert);
		monitor.LastInvoke = DateTime.Now;
		_logCollectorDbContext.Monitors.Update(monitor);		
		HandleMonitorAlertAction(alert, monitor);

	}

	public void HandleMonitorAlertAction(Alert alert, Monitor monitor)
	{
		if (monitor.Action == MonitorAction.SendEmail.ToString())
			_backgroundJobClient.Enqueue(() => _emailService.SendEmailAsync(monitor.Email_Address, monitor.Email_Subject, alert.Message));
		else if (monitor.Action == MonitorAction.SendSms.ToString())
			_backgroundJobClient.Enqueue(() => _smsService.SendSMSAsync(monitor.SMS_PhoneNumber, alert.Message));
		else if (monitor.Action == MonitorAction.CustomApiCall.ToString())
			_backgroundJobClient.Enqueue(() => _customApiCallService.SendCustomApiCallAsync(monitor.CustomApiCall_Url, monitor.CustomApiCall_AuthKey, alert.Message));
		else
			Console.WriteLine($"Unknown action {monitor.Action}");
	}
}
