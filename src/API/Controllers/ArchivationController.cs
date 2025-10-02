using Application.Interfaces.Services;
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("archivation")]
public class ArchivationController : ControllerBase
{
	private readonly IFileProcessingService _fileProcessingService;

	public ArchivationController(IFileProcessingService fileProcessingService)
	{
		_fileProcessingService = fileProcessingService;
	}

	[HttpPost("initialize")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<string>> InitializeArchivationProcess(string[] fileNames)
	{
		var result = await _fileProcessingService.AddArchivationProcessAsync(fileNames);
		if (result.Succeeded)
		{
			return Ok(result.Value);
		}
		else
		{
			return Problem(title: "При обработке запроса произошла ошибка", detail: result.Error!.Description,
				statusCode: result.Error.Code);
		}
	}

	[HttpGet("getStatus/{processId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<string> GetProcessStatusByProcessId([FromRoute] string processId)
	{
		var result = _fileProcessingService.GetStatus(processId);
		if (result.Succeeded)
		{
			return Ok(result.Value.GetDisplayName());
		}
		else
		{
			return Problem(title: "При обработке запроса произошла ошибка", detail: result.Error!.Description,
				statusCode: result.Error.Code);
		}
	}

	[HttpGet("download/{processId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult DownloadByProcessId([FromRoute] string processId)
	{
		var result = _fileProcessingService.GetResult(processId);
		if (result.Succeeded)
		{
			return File(result.Value, "application/zip", "AwesomeArchive.zip");
		}
		else
		{
			return Problem(title: "При обработке запроса произошла ошибка", detail: result.Error!.Description,
				statusCode: result.Error!.Code);
		}
	}
}