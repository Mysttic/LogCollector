﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

[Route("api/[controller]")]
[ApiController]
public class MonitorController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IMonitorRepository _monitorRepository;

	public MonitorController(IMapper mapper, IMonitorRepository monitorRepository)
	{
		_mapper = mapper;
		_monitorRepository = monitorRepository;
	}

	// GET: api/<MonitorController>
	[HttpGet]
	public async Task<ActionResult<PagedResult<BaseMonitorDto>>> GetPagedMonitors(
		[FromQuery] MonitorQueryParameters monitorQueryParameters)
	{
		var pagedMonitorResult = await _monitorRepository.GetAllAsync<BaseMonitorDto>(monitorQueryParameters);

		return Ok(new { pagedMonitorResult.TotalCount, pagedMonitorResult });
	}

	// GET api/<MonitorController>/5
	[HttpGet("{id}")]
	public async Task<ActionResult<BaseMonitorDto>> GetMonitor(int id)
	{
		var monitor = await _monitorRepository.GetMonitorDetailsAsync(id);

		return Ok(monitor);
	}

	// POST api/<MonitorController>
	[HttpPost]
	public async Task<IActionResult> PostMonitor([FromBody] MonitorDto monitorDto)
	{
		var monitor = await _monitorRepository.AddAsync<MonitorDto, Monitor>(monitorDto);

		return Created($"/api/Monitor/{monitor.Id}", monitor);
	}

	// PUT api/<MonitorController>/5
	[HttpPut("{id}")]
	public async Task<IActionResult> PutMonitor(int id, [FromBody] MonitorDto monitorDto)
	{
		try
		{
			await _monitorRepository.UpdateAsync(id, monitorDto);
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!await _monitorRepository.ExistsAsync(id))
				return NotFound();
			else
				throw;
		}

		return NoContent();
	}

	// DELETE api/<MonitorController>/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteMonitor(int id)
	{
		await _monitorRepository.DeleteAsync(id);

		return NoContent();
	}

	private async Task<bool> MonitorExistsAsync(int id)
	{
		return await _monitorRepository.ExistsAsync(id);
	}
}