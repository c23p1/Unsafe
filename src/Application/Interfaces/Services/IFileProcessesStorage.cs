using Application.Common.Results;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IFileProcessesStorage
{
	void Add(FileProcessingProcess process);
	GenericResult<FileProcessingProcess> GetById(string id);
}