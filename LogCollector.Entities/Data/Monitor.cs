public class Monitor
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public string? Query { get; set; }
	public string Action { get; set; } = "";
	public virtual IList<Alert> Alerts { get; set; }
}
