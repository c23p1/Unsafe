using System.CommandLine;
using System.CommandLine.Invocation;

namespace CLI.Actions;

internal class AsciiHelpCommandAction : AsynchronousCommandLineAction
{
	public override async Task<int> InvokeAsync(ParseResult parseResult,
		CancellationToken cancellationToken = new CancellationToken())
	{
		if (parseResult.CommandResult.Command is not RootCommand rootCommand || parseResult.Tokens.Count != 0)
			return await Task.FromResult(0);
		Console.WriteLine("     _                                         _____ _ _           \n    / \\__      _____  ___  ___  _ __ ___   ___|  ___(_) | ___  ___ \n   / _ \\ \\ /\\ / / _ \\/ __|/ _ \\| '_ ` _ \\ / _ \\ |_  | | |/ _ \\/ __|\n  / ___ \\ V  V /  __/\\__ \\ (_) | | | | | |  __/  _| | | |  __/\\__ \\\n /_/   \\_\\_/\\_/ \\___||___/\\___/|_| |_| |_|\\___|_|   |_|_|\\___||___/\n  __________________                                                \n | By Kaspersky Lab | \n  ------------------");
		await rootCommand.Parse("-h").InvokeAsync(cancellationToken: cancellationToken);
		return await Task.FromResult(0);
	}
}