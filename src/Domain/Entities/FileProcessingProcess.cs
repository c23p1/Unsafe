using Domain.Enums;

namespace Domain.Entities;

public class FileProcessingProcess
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public ProcessStatus Status { get; set; } = ProcessStatus.Created;
	public required string[] FileNames { get; init; }
	public byte[]? Result { get; set; }
}