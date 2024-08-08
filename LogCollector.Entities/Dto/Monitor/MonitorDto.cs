public class MonitorDto
{
	public bool IsActive { get; set; } = true;
	public string? Name { get; set; }
	public string? Description { get; set; }
	public string? Query { get; set; }
	public string Action { get; set; } = "";
	public DateTime LastInvoke { get; set; }

	public string? Email_Address { get; set; }
	public string? Email_Subject { get; set; }

	public string? SMS_PhoneNumber { get; set; }

	public string? CustomApiCall_Url { get; set; }
	public string? CustomApiCall_AuthKey { get; set; }
}
