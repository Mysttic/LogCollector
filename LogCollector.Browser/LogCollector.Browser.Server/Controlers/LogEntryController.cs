using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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
	public async Task<ActionResult<PagedResult<BaseLogEntryDto>>> GetPagedLogs(
		[FromQuery] LogQueryParameters logQueryParameters)
	{
		var pagedLogEntryResult = await _logEntryRepository.GetAllAsync<BaseLogEntryDto>(logQueryParameters);

		return Ok(new { pagedLogEntryResult.TotalCount, pagedLogEntryResult });
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<BaseLogEntryDto>> GetLog(int id)
	{
		var log = await _logEntryRepository.GetLogDetailsAsync(id);

		return Ok(log);
	}
}