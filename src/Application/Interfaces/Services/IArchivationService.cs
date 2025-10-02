using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IArchivationService
{
	Task HandleProcessAsync(FileProcessingProcess process);
}