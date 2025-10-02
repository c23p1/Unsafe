namespace Application.Common.Results;

public record Error
{
	public int Code { get; }
	public string Description { get; }

	private Error(int code, string description)
	{
		Code = code;
		Description = description;
	}

	public static Error InvalidOperation(string message) => new(400, $"Недопустимая операция ({message})");
	public static Error EntityNotFound(string entityName, string id) =>
		new(404, $"Сущность ({entityName}) с Id ({id}) не найдена");
	public static Error FileNotFound(string path) =>
		new(404, $"Не удалось найти указанный файл ({path})");
}