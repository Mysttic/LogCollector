using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class LogEntryController : Controller
{
	private readonly LogCollectorDbContext _context;

	public LogEntryController(LogCollectorDbContext context)
	{
		_context = context;
	}

	[HttpGet]
	public async Task<IActionResult> GetLogs(
		[FromQuery] string deviceId = "",
		[FromQuery] string applicationName = "",
		[FromQuery] string ipAddress = "",
		[FromQuery] string message = "",
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 10)
	{
		if (page < 1 || pageSize < 1)
		{
			return BadRequest("Page and pageSize must be greater than 0.");
		}

		var query = _context.Logs.AsQueryable();

		if (!string.IsNullOrEmpty(ipAddress))
		{
			query = query.Where(log => log.IpAddress == ipAddress);
		}

		if (!string.IsNullOrEmpty(applicationName))
		{
			query = query.Where(log => log.ApplicationName == applicationName);
		}

		var totalLogs = await query.CountAsync();
		var logs = await query
			.OrderByDescending(log => log.Timestamp)
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();

		return Ok(new { totalLogs, logs });
	}
}

