public class BaseMonitorDto : MonitorDto
{
	public int Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
	public IList<BaseAlertDto>? Alerts { get; set; }
}