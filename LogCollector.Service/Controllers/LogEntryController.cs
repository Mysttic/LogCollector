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

    [HttpPost]
    public async Task<IActionResult> PostLog(CreateLogEntryDto logEntryPost)
	{
		var logEntry = await _logEntryRepository.AddAsync<CreateLogEntryDto, LogEntry>(logEntryPost);

		return Created($"/api/LogEntry/{logEntry.Id}", logEntry);
	}
}
