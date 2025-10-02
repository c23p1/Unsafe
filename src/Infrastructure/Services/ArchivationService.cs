using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using System.IO.Compression;

namespace Infrastructure.Services;

public class ArchivationService : IArchivationService
{
	private readonly string _filesPath;

	public ArchivationService(IConfiguration configuration)
	{
		_filesPath = configuration["AwesomeFilesPath"]!;
	}

	public async Task HandleProcessAsync(FileProcessingProcess process)
	{
		try
		{
			if (process.Status != ProcessStatus.Queued)
				throw new InvalidOperationException("Процесс имеет неверный статус");
			process.Status = ProcessStatus.Running;
			using var memoryStream = new MemoryStream();
			using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
			{
				foreach (var fileName in process.FileNames)
				{
					var entry = archive.CreateEntry(fileName, CompressionLevel.SmallestSize);
					await using var entryStream = entry.Open();
					var path = Path.Combine(_filesPath, fileName);
					await using var fileStream = File.OpenRead(path);
					await fileStream.CopyToAsync(entryStream);
				}
			}
			process.Result = memoryStream.ToArray();
		}
		catch
		{
			process.Status = ProcessStatus.Aborted;
			throw;
		}
		finally
		{
			process.Status = ProcessStatus.Completed;
		}
	}
}