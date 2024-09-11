public class Monitor : RecordBase
{
	public bool IsActive { get; set; } = true;
	public string? Name { get; set; }
	public string? Description { get; set; }
	public string? Query { get; set; }
	public string Action { get; set; } = "";
	public DateTime LastInvoke { get; set; }
	public virtual IList<Alert> Alerts { get; set; } = new List<Alert>();

	//EmailMonitor
	public string? Email_Address { get; set; }

	public string? Email_Subject { get; set; }

	//SMS
	public string? SMS_PhoneNumber { get; set; }

	//CustomApiCall
	public string? CustomApiCall_Url { get; set; }

	public string? CustomApiCall_AuthKey { get; set; }
}