namespace CleanArchitecture.Domain.Common;

public class Result<T>
{
	public bool Success { get; set; }
	public string Message { get; set; } = "";
	public T Data { get; set; }

	public static Result<T> Fail(string errorMessage)
	{
		return new Result<T> { Success = false, Message = errorMessage };
	}

	public static Result<T> Complete(T data, string message)
	{
		return new Result<T> { Success = true, Data = data, Message = message };
	}
}