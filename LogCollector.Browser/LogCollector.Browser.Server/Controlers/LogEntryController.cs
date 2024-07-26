using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class LogEntryController : Controller
{
	private readonly IMapper _mapper;
	private readonly ILogEntryRepository _logEntryRepository;

	public LogEntryController(IMapper mapper, ILogEntryRepository logEntryRepository)
	{
		_mapper = mapper;
		_logEntryRepository = logEntryRepository;
	}

	[HttpGet]
	public async Task<ActionResult<PagedResult<LogEntryDto>>> GetPagedLogs(
		[FromQuery] QueryParameters queryParameters)
	{
		var pagedLogEntryResult = await _logEntryRepository.GetAllAsync<LogEntryDto>(queryParameters);

		return Ok(new { pagedLogEntryResult.TotalCount, pagedLogEntryResult });
	}
}

