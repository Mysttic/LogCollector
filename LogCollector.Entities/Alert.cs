using System.ComponentModel.DataAnnotations.Schema;

public class Alert : RecordBase
{
	public string Message { get; set; } = "";
	[ForeignKey(nameof(MonitorId))]
	public int MonitorId { get; set; }
	public Monitor Monitor { get; set; }
}
