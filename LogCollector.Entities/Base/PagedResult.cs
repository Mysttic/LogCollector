﻿public class PagedResult<T>
{
	public List<T>? Items { get; set; }
	public int TotalCount { get; set; }
	public int PageNumber { get; set; }
	public int RecordNumber { get; set; }
}
