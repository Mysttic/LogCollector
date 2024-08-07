public interface IMonitorQueryParameters : IQueryParameters
{
	string? IsActive { get; set; }
	string? Name { get; set; }
	string? Action { get; set; }
	string? Query { get; set; }
}
