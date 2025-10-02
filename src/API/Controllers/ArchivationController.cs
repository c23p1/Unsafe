using Application.Interfaces.Services;
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers;

[ApiController]
[Route("archivation")]
public class ArchivationController : ControllerBase
{
	private readonly IFileProcessingService _fileProcessingService;
	private readonly IMemoryCache _memoryCache;

	public ArchivationController(IFileProcessingService fileProcessingService, IMemoryCache memoryCache)
	{
		_fileProcessingService = fileProcessingService;
		_memoryCache = memoryCache;
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
		byte[] content;
		if (!_memoryCache.TryGetValue(processId, out content!))
		{
			var result = _fileProcessingService.GetResult(processId);
			if (result.Succeeded)
			{
				content = result.Value;
				_memoryCache.Set(processId, content, new TimeSpan(0, 30, 0));
			}
			else
			{
				return Problem(title: "При обработке запроса произошла ошибка", detail: result.Error!.Description,
					statusCode: result.Error!.Code);
			}
		}
		return File(content, "application/zip", "AwesomeArchive.zip");
	}
}