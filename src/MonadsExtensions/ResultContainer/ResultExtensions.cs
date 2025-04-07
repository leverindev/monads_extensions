using System;
using MonadsExtensions.Extensions;
using MonadsExtensions.ResultContainer.Models;

namespace MonadsExtensions.ResultContainer
{
    public static class Result
    {
        public static IntermediateOk<TValue> Ok<TValue>(TValue value) => new IntermediateOk<TValue>(value);

        public static IntermediateError<TError> Error<TError>(TError error) => new IntermediateError<TError>(error);

        public static Result<TValue, TError> ToResult<TValue, TError>(this TValue value) => Ok(value);

        public static Result<TValue, TError> ToError<TValue, TError>(this TError error) => Error(error);

        public static TValue UnwrapOrElse<TValue, TError>(this Result<TValue, TError> result, Func<TError, TValue> onError) =>
            result.Bind(value => value, onError);

        public static TValue UnwrapOrDefault<TValue, TError>(this Result<TValue, TError> result) =>
            result.UnwrapOrElse(error => default);

        public static TValue UnwrapOrException<TValue, TError>(this Result<TValue, TError> result, Exception exception)
        {
            return result.Bind(
                value => value,
                error =>
                {
                    exception.Data.Add("Error", error);

                    throw exception;
                });
        }

        public static TValue UnwrapOrException<TValue, TError>(this Result<TValue, TError> result, string message) =>
            result.UnwrapOrException(new Exception(message));

        public static Result<TOutResult, TOutError> Bind<TInputResult, TInputError, TOutResult, TOutError>(
            this Result<TInputResult, TInputError> result,
            Func<TInputResult, TOutResult> onSuccess,
            Func<TInputError, TOutError> onError)
        {
            return result.Bind(ProcessSuccess, ProcessError);

            Result<TOutResult, TOutError> ProcessSuccess(TInputResult x) => Ok(x.Map(onSuccess));
            Result<TOutResult, TOutError> ProcessError(TInputError x) => Error(x.Map(onError));
        }

        public static Result<TOutResult, TError> Bind<TInputResult, TOutResult, TError>(
            this Result<TInputResult, TError> result,
            Func<TInputResult, TOutResult> onSuccess)
        {
            return result.Bind(onSuccess, error => error);
        }
    }
}
