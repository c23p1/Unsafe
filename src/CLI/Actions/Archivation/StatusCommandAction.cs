using CLI.Endpoints;
using Refit;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace CLI.Actions.Archivation;

public class StatusCommandAction : AsynchronousCommandLineAction
{
	public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = new CancellationToken())
	{
		var processId = parseResult.GetRequiredValue<string>("processId");
		var url = parseResult.GetRequiredValue<string>("--url");
		var awesomeFilesAPI = RestService.For<IAwesomeFilesAPI>(url);
		try
		{
			var status = await awesomeFilesAPI.GetProcessStatusByProcessId(processId);
			Console.WriteLine($"Статус процесса: {status}");
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