using BudgetTracker.Domain.Common.Errors;

namespace BudgetTracker.Domain.Common;

public class Result
{
    
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new ArgumentException("Error should be None when success is true");
        } 
        if (!isSuccess && error == Error.None)
        {
            throw new ArgumentException("Error should not be None when success is false");       
        }
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public bool IsSuccess { get;}
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    
    public static Result Success=> new(isSuccess: true, error: Error.None);
    public static Result Failure(Error error)=> new (isSuccess: false, error: error);

}

public class Result<T> : Result
{
    private readonly T? _value;
    public T Value=> IsSuccess? _value!: throw new InvalidOperationException("Value should not be accessed when result is failure");
    private Result(T value) : base(true, Error.None)
    {
        _value = value;
    }

    private Result(Error error) : base(false, error)
    {
        _value = default;
    }


    public new static Result<T> Success(T value) => new(value);
    public new static Result<T> Failure(Error error)=> new(error);
}