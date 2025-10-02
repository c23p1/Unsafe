namespace Application.Common.Results;

public record Result
{
	public bool Succeeded { get; }
	public Error? Error { get; }

	protected Result()
	{
		Succeeded = true;
	}
	protected Result(Error error)
	{
		Succeeded = false;
		Error = error;
	}

	public static Result Success() => new();
	public static Result Failure(Error error) => new(error);
}