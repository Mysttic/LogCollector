public class QueryParameters
{
	private int _pageSize = 15;
	public int StartIndex { get; set; }
	public int PageNumber { get; set; }
	public string? DeviceId { get; set; }
	public string? ApplicationName { get; set; }
	public string? IpAddress { get; set; }
	public string? LogMessage { get; set; }


	public int PageSize
	{
		get { return _pageSize; }
		set
		{
			_pageSize = value;
		}
	}
}