using Application.Interfaces.Services;
using Infrastructure.Services;
using Infrastructure.Services.BackgroundProcessing;
using Serilog;

namespace API;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.Host.UseSerilog((context, configuration) =>
		{
			configuration.ReadFrom.Configuration(context.Configuration);
		});
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Services.AddSingleton<IFileProcessesStorage, FileProcessesStorage>();
		builder.Services.AddTransient<IFilesService, FilesService>();
		builder.Services.AddTransient<IFileProcessingService, FileProcessingService>();
		builder.Services.AddTransient<IArchivationService, ArchivationService>();

		builder.Services.AddMemoryCache();

		builder.Services.AddHostedService<BackgroundProcessingService>();
		builder.Services.AddSingleton(new BackgroundWorkQueue(100));

		builder.Services.AddControllers();
		var app = builder.Build();
		app.UseSerilogRequestLogging();
		app.UseSwagger();
		app.UseSwaggerUI();
		app.UseHttpsRedirection();
		app.MapControllers();
		app.Run();
	}
}