public class BaseMonitorDto
{
	public bool IsActive { get; set; } = true;
	public string? Name { get; set; }
	public string? Description { get; set; }
	public string? Query { get; set; }
	public string Action { get; set; } = "";
}
