public class Monitor : RecordBase
{
	public bool IsActive { get; set; } = true;
	public string? Name { get; set; }
	public string? Description { get; set; }
	public string? Query { get; set; }
	public string Action { get; set; } = "";
	public DateTime LastInvoke { get; set; }
	public virtual IList<Alert> Alerts { get; set; } = new List<Alert>();
}
