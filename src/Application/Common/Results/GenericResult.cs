namespace Application.Common.Results;

public record GenericResult<T> : Result
{
	public T Value => Succeeded
		? _value!
		: throw new InvalidOperationException("Значение не может быть получено при наличии ошибки");
	private readonly T? _value;

	protected GenericResult(T value)
	{
		_value = value;
	}
	protected GenericResult(Error error) : base(error) { }

	public static GenericResult<T> Success(T value) => new(value);
	public new static GenericResult<T> Failure(Error error) => new(error);
}