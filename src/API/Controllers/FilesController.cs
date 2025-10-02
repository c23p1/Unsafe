using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("files")]
public class FilesController : ControllerBase
{
	private readonly IFilesService _filesService;

	public FilesController(IFilesService filesService)
	{
		_filesService = filesService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult GetAllFileNames() =>
		Ok(_filesService.GetAllFileNames());
}