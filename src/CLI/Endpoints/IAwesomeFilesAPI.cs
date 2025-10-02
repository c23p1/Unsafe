using Refit;

namespace CLI.Endpoints;

public interface IAwesomeFilesAPI
{
	[Get("/files")]
	Task<string> GetFiles();

	[Post("/archivation/initialize")]
	Task<string> InitializeArchivationProcess(string[] fileNames);
	[Get("/archivation/getStatus/{processId}")]
	Task<string> GetProcessStatusByProcessId(string processId);
	[Get("/archivation/download/{processId}")]
	Task<Stream> DownloadByProcessId(string processId);
}