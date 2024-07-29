﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

[Route("api/[controller]")]
[ApiController]
public class AlertController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IAlertRepository _alertRepository;

    public AlertController(IMapper mapper, IAlertRepository alertRepository )
    {
        _mapper = mapper;
		_alertRepository = alertRepository;
    }

    // GET: api/<AlertController>
    [HttpGet]
	public async Task<ActionResult<PagedResult<AlertDto>>> GetPagedAlerts(
		[FromQuery] AlertQueryParameters alertQueryParameters)
	{
		var pagedAlertResult = await _alertRepository.GetAllAsync<AlertDto>(alertQueryParameters);

		return Ok(new { pagedAlertResult.TotalCount, pagedAlertResult });
	}

	// GET api/<AlertController>/5
	[HttpGet("{id}")]
	public async Task<ActionResult<AlertDto>> GetAlert(int id)
	{
		var alertResult = await _alertRepository.GetAsync<AlertDto>(id);

		return Ok(alertResult);
	}

	// POST api/<AlertController>
	[HttpPost]
	public async Task<IActionResult> PostAlert([FromBody] CreateAlertDto createAlertDto)
	{
		var alert = await _alertRepository.AddAsync<CreateAlertDto, Alert>(createAlertDto);
		
		return Created($"/api/Alert/{alert.Id}", alert);

	}

	// PUT api/<AlertController>/5
	[HttpPut("{id}")]
	public async Task<IActionResult> PutAlert(int id, [FromBody] UpdateAlertDto updateAlertDto)
	{
		try
		{

			await _alertRepository.UpdateAsync(id, updateAlertDto);
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!await _alertRepository.ExistsAsync(id))
			{
				return NotFound();
			}
			else
			{
				throw;
			}
		}

		return NoContent();
	}

	// DELETE api/<AlertController>/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteAlert(int id)
	{
		await _alertRepository.DeleteAsync(id);

		return NoContent();
	}
}
