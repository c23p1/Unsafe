using Application.Common.Results;
using Application.Interfaces.Services;
using Domain.Entities;
using System.Collections.Concurrent;

namespace Infrastructure.Services;

public class FileProcessesStorage : IFileProcessesStorage
{
	private readonly ConcurrentDictionary<string, FileProcessingProcess> _processes = new();

	public void Add(FileProcessingProcess process)
	{
		_processes[process.Id] = process;
	}

	public GenericResult<FileProcessingProcess> GetById(string id)
	{
		if (!_processes.TryGetValue(id, out var process))
			return GenericResult<FileProcessingProcess>.Failure(Error.EntityNotFound(nameof(FileProcessingProcess), id));
		return GenericResult<FileProcessingProcess>.Success(process);
	}
}