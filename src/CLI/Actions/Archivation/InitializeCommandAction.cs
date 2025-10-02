using CLI.Endpoints;
using Refit;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace CLI.Actions.Archivation;

public class InitializeCommandAction : AsynchronousCommandLineAction
{
	public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = new CancellationToken())
	{
		var fileNames = parseResult.GetRequiredValue<string[]>("fileNames");
		var url = parseResult.GetRequiredValue<string>("--url");
		var awesomeFilesAPI = RestService.For<IAwesomeFilesAPI>(url);
		try
		{
			var processId = await awesomeFilesAPI.InitializeArchivationProcess(fileNames);
			Console.WriteLine($"Id процесса создания архива: {processId}");
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