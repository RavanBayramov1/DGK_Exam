namespace ExamSystem.Common;

public record ServiceResult
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    protected ServiceResult(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static ServiceResult Success() => new(true, null);
    public static ServiceResult Failure(Error error) => new(false, error);
    public static implicit operator ServiceResult(Error error) => Failure(error);
}

public record ServiceResult<T> : ServiceResult
{
    public T? Data { get; }

    protected ServiceResult(T data) : base(true, null) => Data = data;
    protected ServiceResult(Error error) : base(false, error) { }

    public static ServiceResult<T> Success(T data) => new(data);
    public static new ServiceResult<T> Failure(Error error) => new(error);
    public static implicit operator ServiceResult<T>(T data) => new(data);
    public static implicit operator ServiceResult<T>(Error error) => new(error);
}