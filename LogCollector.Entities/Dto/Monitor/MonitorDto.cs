public class MonitorDto : BaseMonitorDto
{
	public int Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
	public virtual IList<AlertDto> Alerts { get; set; }

}
