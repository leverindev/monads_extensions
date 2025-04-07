using MonadsExtensions.ResultContainer.Models;

namespace MonadsExtensions.ResultContainer
{
    public static class Result
    {
        public static IntermediateOk<TValue> Ok<TValue>(TValue value) => new IntermediateOk<TValue>(value);

        public static IntermediateError<TError> Error<TError>(TError error) => new IntermediateError<TError>(error);

        public static Result<TValue, TError> ToResult<TValue, TError>(this TValue value) => Ok(value);

        public static Result<TValue, TError> ToError<TValue, TError>(this TError error) => Error(error);
    }
}
