using Application.Common.Results;
using Domain.Enums;

namespace Application.Interfaces.Services;

public interface IFileProcessingService
{
	Task<GenericResult<string>> AddArchivationProcessAsync(string[] fileNames);
	GenericResult<ProcessStatus> GetStatus(string processId);
	GenericResult<byte[]> GetResult(string processId);
}