using CLI.Actions;
using CLI.Actions.Archivation;
using System.CommandLine;

namespace CLI;

class Program
{
	static async Task<int> Main(string[] args)
	{
		var url = new Option<string>("--url", new string[] { "-u" })
		{
			Description = "Адрес сервера с \"потрясающими\" файлами",
			DefaultValueFactory = _ => "http://localhost",
			Recursive = true,
			Arity = ArgumentArity.ExactlyOne
		};
		url.Validators.Add(result =>
		{
			var value = result.GetValue(url);
			if (!Uri.TryCreate(value, UriKind.Absolute, out var uriResult)
			    || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
			{
				result.AddError("Указанный адрес сервера имеет неверный формат, ожидаемый формат - http(s)://host:port");
			}
		});
		var destinationFolder = new Option<string>("--destination", new string[] { "-d" })
		{
			Description = "Путь, по которому будет сохранён архив",
			DefaultValueFactory = _ => ".",
			Arity = ArgumentArity.ExactlyOne
		};
		destinationFolder.Validators.Add(result =>
		{
			if (!Path.Exists(result.GetValue(destinationFolder)))
				result.AddError("Не удалось найти указанную директорию.");
		});

		var listCommand = new Command("list", "Получить список \"потрясающих\" файлов")
		{
			Options = { url }
		};
		listCommand.SetAction(async result => await new ListCommandAction().InvokeAsync(result));

		var initializeArchivationProcessCommand = new Command("init", "Инициализировать процесс создания архива")
		{
			Arguments = { new Argument<string[]>("fileNames") { Arity = ArgumentArity.OneOrMore } }
		};
		initializeArchivationProcessCommand.SetAction(async result => await new InitializeCommandAction().InvokeAsync(result));

		var archivationProcessStatusCommand = new Command("status", "Получить статус процесса создания архива")
		{
			Arguments = { new Argument<string>("processId") }
		};
		archivationProcessStatusCommand.SetAction(async result => await new StatusCommandAction().InvokeAsync(result));

		var downloadArchiveCommand = new Command("download", "Скачать созданный архив")
		{
			Arguments = { new Argument<string>("processId") },
			Options = { destinationFolder }
		};
		downloadArchiveCommand.SetAction(async result => await new DownloadCommandAction().InvokeAsync(result));

		var archivationCommand = new Command("archivation", "Управление задачами архивации файлов")
		{
			Options = { url },
			Subcommands = { initializeArchivationProcessCommand, archivationProcessStatusCommand, downloadArchiveCommand }
		};
		archivationCommand.SetAction(async _ => await archivationCommand.Parse("-h").InvokeAsync());

		var rootCommand = new RootCommand("CLI для управления \"потрясающими\" файлами")
		{
			Subcommands = { listCommand, archivationCommand }
		};
		rootCommand.SetAction(async result => await new AsciiHelpCommandAction().InvokeAsync(result));

		var parserConfiguration = new ParserConfiguration { EnablePosixBundling = true };
		var parseResult = rootCommand.Parse(args, parserConfiguration);
		if (parseResult.Errors.Count == 0) return await parseResult.InvokeAsync();
		Console.WriteLine(parseResult.Errors[0]);
		return 1;
	}
}