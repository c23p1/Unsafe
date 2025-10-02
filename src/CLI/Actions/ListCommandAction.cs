using CLI.Endpoints;
using Refit;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace CLI.Actions;

public class ListCommandAction : AsynchronousCommandLineAction
{
	public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = new CancellationToken())
	{
		var url = parseResult.GetRequiredValue<string>("--url");
		var awesomeFilesAPI = RestService.For<IAwesomeFilesAPI>(url);
		try
		{
			Console.WriteLine(await awesomeFilesAPI.GetFiles());
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