using CLI.Endpoints;
using Refit;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace CLI.Actions.Archivation;

public class DownloadCommandAction : AsynchronousCommandLineAction
{
	public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = new CancellationToken())
	{
		var processId = parseResult.GetRequiredValue<string>("processId");
		var url = parseResult.GetRequiredValue<string>("--url");
		var destination = parseResult.GetRequiredValue<string>("--destination");
		var awesomeFilesAPI = RestService.For<IAwesomeFilesAPI>(url);
		try
		{
			Console.WriteLine("Начата загрузка архива");
			var content = await awesomeFilesAPI.DownloadByProcessId(processId);
			var fileBytes = await content.ReadAsByteArrayAsync(cancellationToken);
			var archiveFilePath = Path.Combine(destination, "AwesomeArchive.zip");
			await File.WriteAllBytesAsync(archiveFilePath, fileBytes, cancellationToken);
			Console.WriteLine("Архив успешно сохранён");
		}
		catch (UnauthorizedAccessException)
		{
			Console.WriteLine("Недостаточно прав для сохранения файла в указанную директорию");
			return 1;
		}
		catch (HttpRequestException)
		{
			Console.WriteLine("Не удалось подключиться к серверу. Попробуйте изменить url.");
			return 1;
		}
		catch (ValidationApiException validationException)
		{
			Console.WriteLine(validationException.Content!.Title);
			Console.WriteLine(validationException.Content!.Detail);
		}
		return 0;
	}
}