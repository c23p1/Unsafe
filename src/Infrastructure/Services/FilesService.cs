using Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class FilesService : IFilesService
{
	private readonly string _filesPath;

	public FilesService(IConfiguration configuration)
	{
		_filesPath = configuration["AwesomeFilesPath"]!;
	}

	public List<string> GetAllFileNames()
	{
		return Directory.GetFiles(_filesPath).Select(Path.GetFileName).ToList()!;
	}
}