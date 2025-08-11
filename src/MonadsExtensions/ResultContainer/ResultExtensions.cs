using System;
using System.Threading.Tasks;
using MonadsExtensions.Extensions;
using MonadsExtensions.ResultContainer.Models;

namespace MonadsExtensions.ResultContainer;

public static class Result
{
    public static IntermediateOk<TValue> Ok<TValue>(TValue value) => new (value);

    public static IntermediateError<TError> Error<TError>(TError error) => new (error);

    public static Result<TValue, TError> ToResult<TValue, TError>(this TValue value) => Ok(value);

    public static Result<TValue, TError> ToError<TValue, TError>(this TError error) => Error(error);

    public static TValue UnwrapOrElse<TValue, TError>(this Result<TValue, TError> result, Func<TError, TValue> onError) =>
        result.Match(value => value, onError);

    public static TValue UnwrapOrDefault<TValue, TError>(this Result<TValue, TError> result) =>
        result.UnwrapOrElse(_ => default);

    public static TValue UnwrapOrException<TValue, TError>(this Result<TValue, TError> result, Exception exception)
    {
        return result.Match(
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
        return result.Match(ProcessSuccess, ProcessError);

        Result<TOutResult, TOutError> ProcessSuccess(TInputResult x) => Ok(x.Map(onSuccess));
        Result<TOutResult, TOutError> ProcessError(TInputError x) => Error(x.Map(onError));
    }

    public static Result<TOutResult, TError> Bind<TInputResult, TOutResult, TError>(
        this Result<TInputResult, TError> result,
        Func<TInputResult, TOutResult> onSuccess)
    {
        return result.Bind(onSuccess, error => error);
    }

    public static Task<Result<TOutResult, TOutError>> BindAsync<TInputResult, TInputError, TOutResult, TOutError>(
        this Task<Result<TInputResult, TInputError>> resultTask,
        Func<TInputResult, TOutResult> onSuccess,
        Func<TInputError, TOutError> onError)
    {
        return resultTask?.ContinueWith(task => Bind(task.Result, onSuccess, onError));
    }

    public static Task<Result<TOutResult, TError>> BindAsync<TInputResult, TOutResult, TError>(
        this Task<Result<TInputResult, TError>> resultTask,
        Func<TInputResult, TOutResult> onSuccess)
    {
        return resultTask.BindAsync(onSuccess, error => error);
    }

    public static async Task<Result<TOutResult, TOutError>> BindAsync<TInputResult, TInputError, TOutResult, TOutError>(
        this Task<Result<TInputResult, TInputError>> resultTask,
        Func<TInputResult, Task<TOutResult>> onSuccess,
        Func<TInputError, Task<TOutError>> onError)
    {
        var result = await resultTask.ConfigureAwait(false);

        if (result.IsValue(out var value, out var error))
        {
            var outResult = await onSuccess(value).ConfigureAwait(false);

            return Ok(outResult);
        }

        var outError = await onError(error).ConfigureAwait(false);

        return Error(outError);
    }

    public static async Task<Result<TOutResult, TError>> BindAsync<TInputResult, TOutResult, TError>(
        this Task<Result<TInputResult, TError>> resultTask,
        Func<TInputResult, Task<TOutResult>> onSuccess)
    {
        var result = await resultTask.ConfigureAwait(false);

        if (result.IsValue(out var value, out var error))
        {
            var outResult = await onSuccess(value).ConfigureAwait(false);

            return Ok(outResult);
        }

        return Error(error);
    }
}
