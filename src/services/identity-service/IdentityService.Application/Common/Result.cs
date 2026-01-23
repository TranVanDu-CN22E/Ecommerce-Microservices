namespace IdentityService.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public IReadOnlyList<Error> Errors { get; }
        protected Result(bool isSuccess, IReadOnlyList<Error> error)
        {
            IsSuccess = isSuccess;
            Errors = error;
        }
        public static Result Success()
            => new(true, Array.Empty<Error>());
        public static Result<T> Failure<T>(IReadOnlyList<Error> errors)
            => Result<T>.Failure(errors);

    }
    public class Result<T> : Result
    {
        private readonly T? _value;
        public T Value =>
            IsSuccess
                ? _value!
                : throw new InvalidOperationException("Cannot access Value when result is failure.");
        protected Result(T? value, bool isSuccess, IReadOnlyList<Error> errors)
            : base(isSuccess, errors)
        {
            _value = value;
        }
        public static Result<T> Success(T value)
            => new(value, true, Array.Empty<Error>());
        public static Result<T> Failure(IReadOnlyList<Error> errors)
            => new(default, false, errors);
    }
}
