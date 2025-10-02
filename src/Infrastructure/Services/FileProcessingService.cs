using Application.Common.Results;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Services.BackgroundProcessing;

namespace Infrastructure.Services;

public class FileProcessingService : IFileProcessingService
{
	private readonly IFilesService _filesService;
	private readonly IFileProcessesStorage _fileProcessesStorage;
	private readonly IArchivationService _archivationService;
	private readonly BackgroundWorkQueue _backgroundWorkQueue;

	public FileProcessingService(IFilesService filesService, IFileProcessesStorage fileProcessesStorage,
		IArchivationService archivationService, BackgroundWorkQueue backgroundWorkQueue)
	{
		_filesService = filesService;
		_fileProcessesStorage = fileProcessesStorage;
		_archivationService = archivationService;
		_backgroundWorkQueue = backgroundWorkQueue;
	}

	public async Task<GenericResult<string>> AddArchivationProcessAsync(string[] fileNames)
	{
		if (fileNames.Length == 0)
			return GenericResult<string>.Failure(Error.InvalidOperation("Необходимо указать как минимум 1 файл для архивации"));
		fileNames = fileNames.Distinct().ToArray();
		var availableFileNames = _filesService.GetAllFileNames();
		foreach (var fileName in fileNames)
		{
			if (!availableFileNames.Contains(fileName))
				return GenericResult<string>.Failure(Error.FileNotFound(fileName));
		}
		var process = new FileProcessingProcess
		{
			FileNames = fileNames
		};
		_fileProcessesStorage.Add(process);
		process.Status = ProcessStatus.Queued;
		await _backgroundWorkQueue.EnqueueAsync(async _ => await _archivationService.HandleProcessAsync(process));
		return GenericResult<string>.Success(process.Id);
	}

	public GenericResult<ProcessStatus> GetStatus(string processId)
	{
		var result = _fileProcessesStorage.GetById(processId);
		if (!result.Succeeded)
			return GenericResult<ProcessStatus>.Failure(result.Error!);
		return GenericResult<ProcessStatus>.Success(result.Value.Status);
	}

	public GenericResult<byte[]> GetResult(string processId)
	{
		var result = _fileProcessesStorage.GetById(processId);
		if (!result.Succeeded)
			return GenericResult<byte[]>.Failure(result.Error!);
		if (result.Value.Status != ProcessStatus.Completed)
			return GenericResult<byte[]>.Failure(Error.InvalidOperation(
				"Получение результата процесса возможно только после его успешного завершения"));
		return GenericResult<byte[]>.Success(result.Value.Result!);
	}
}