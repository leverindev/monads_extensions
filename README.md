[![Build](https://github.com/leverindev/monads_extensions/actions/workflows/dotnet.yml/badge.svg)](https://github.com/leverindev/monads_extensions/actions/workflows/dotnet.yml)

# Functional Programming Monads Extensions

This library provides useful functional programming monads: **Option** (also known as Maybe)  and **Result**.

## Motivation

The **Maybe** and **Result** monads are handy patterns from the world of functional programming. They are present in all functional languages and in many others (e.g., Rust).

### Maybe
The **Maybe** monad (aka **Option**) is a design pattern that allows a chain of function calls to be interrupted if any function in the chain fails to return a meaningful result.

```csharp
protected Option<int> Parse(string input)
{
    return int.TryParse(input, out var value)
        ? Option.Some(value)
        : Option.None;
}
```

### Result

Let's look closer at the **Result** monad, which allows you to handle errors in a functional style.

#### Traditional approach

Suppose we have a method that returns an entity by ID.

```csharp
public interface IEntitiesProvider
{
    Entity Get(int id);
}
```

But what to do if an entity with the requested ID wasn't found?

We can return a *null*, and the calling code has to process such a case. Or we can throw an exception, and the only hope remains that the user of this method reads the documentation (if, of course, there is one).

Both options are acceptable, but there is no single approach that would be forced by a programming language.

#### Functional programming way

The method returns a structure that can contain either a success or an error. And after pattern matching was introduced in C#, using this approach is quite natural.

The interface *IEntitiesProvider* and its implementation could look like this:

```csharp
public interface IEntitiesProvider
{
    Result<Entity, string> Get(int id);
}

public class EntitiesProvider : IEntitiesProvider
{
    private readonly IDictionary<int, Entity> _entities = new Dictionary<int, Entity>();

    public Result<Entity, string> Get(int id)
    {
        return _entities.TryGetValue(id, out var entity)
            ? Result.Ok(entity)
            : Result.Error($"Entity with ID {id} wasn't found");
    }
}
```

And the code that uses *IEntitiesProvider* looks like this:

```csharp
public void UseEntitiesProvider(IEntitiesProvider provider, int id)
{
    Result<Entity, string> result = provider.Get(id);
    switch (result)
    {
        case { HasValue: true, Value: var value }:
            // Do something with the value
            Console.WriteLine(value);
            break;
        case { HasValue: false, Error: var error }:
            Console.WriteLine(error);
            break;
    }
}
```

If this seems too difficult or cumbersome because of a lot of boilerplate code, thanks to extension methods we can still use traditional approaches:

Throw an exception:
```csharp
Entity result = provider.Get(id).UnwrapOrException("Entity not found");
```

Return *null*:
```csharp
Entity? result = provider.Get(id).UnwrapOrDefault();
```

Retrieve the value, but leave the option to handle the error:
```csharp
Entity? result = provider.Get(id).UnwrapOrElse(error =>
{
    Console.WriteLine(error);
    return null;
});
```

### Usage example

Additionally, the library provides extension methods for constructing processing chains (**Map**, **Do**, **Bind**).

Let's assume we need to implement a method that parses a string into a number, and if the number is natural, then squares it, otherwise (invalid string or non-natural number) returns -1, and also logs an error to the console if the string is invalid.

```csharp
public int SquareOfStrNumber(string input, int expectedValue)
{
    return ParseInt32(input)
        .Do(LogError)
        .Bind(ToIntermediateResult)
        .Bind(CheckNatural)
        .Bind(Square)
        .UnwrapOrElse(ToIntermediateResult)
        .Map(Unwrap);
}

private Result<int, string> ParseInt32(string input)
{
    if (int.TryParse(input, out var value))
    {
        return Result.Ok(value);
    }

    return Result.Error("Invalid input");
}

private void LogError<T>(Result<T, string> result)
{
    if (!result.HasValue)
    {
        Console.WriteLine(result.Error);
    }
}

private IntermediateResult<int> CheckNatural(IntermediateResult<int> result)
{
    if (result.Completed)
    {
        return result;
    }

    return result.Value < 1
        ? new IntermediateResult<int>(-1, true)
        : new IntermediateResult<int>(result.Value, false);
}

private IntermediateResult<int> Square(IntermediateResult<int> result)
{
    return result.Completed
        ? result
        : new IntermediateResult<int>(result.Value * 2, true);
}

private int Unwrap(IntermediateResult<int> result)
{
    return result.Completed ? result.Value : -1;
}

private readonly struct IntermediateResult<T>(T value, bool completed)
{
    public T Value { get; } = value;

    public bool Completed { get; } = completed;
}
```

### Pipelines

Pipelines represent a technique where data is transformed through a sequence of functions, with the output of one function serving as the input for the next.

#### Example
```csharp
var database = new DatabaseContext();

var transaction = new UserBalanceTransaction { UserId = 1, Amount = 100 };

var state = new UpdateUserBalanceState { Transaction = transaction };

var pipeline = new GetUserHandler(database)
    .PipeTo(new UpdateStateHandler<User>(state))
    .PipeTo(new GetUserBalanceHandler(database))
    .PipeTo(new UpdateStateHandler<UserBalance>(state))
    .PipeTo(x => x.Bind(b => state))
    .PipeTo(new ModifyBalanceHandler());

var balanceResult = pipeline.Execute(transaction.UserId);

if (balanceResult.IsValue(out var balance, out var error))
{
    Console.WriteLine($"Balance: {balance}");
}
else
{
    Console.WriteLine($"Update balance error: {error}");
}

///////////////////////////////////////////////////////////////////////////

// Operation state:
public class UpdateUserBalanceState
{
    public required UserBalanceTransaction Transaction { get; init; }

    public User? User { get; set; }

    public UserBalance? UserBalance { get; set; }
}

// Handlers:
public class GetUserHandler(DatabaseContext database) : IPipeline<long, Result<User, string>>
{
    public Result<User, string> Execute(long userId)
    {
        var user = database.Users.FirstOrDefault(x => x.Id == userId);
        if (user != null)
        {
            return Result.Ok(user);
        }

        return Result.Error("User not found");
    }
}

public class GetUserBalanceHandler(DatabaseContext database) : IPipeline<Result<User, string>, Result<UserBalance, string>>
{
    public Result<UserBalance, string> Execute(Result<User, string> input)
    {
        if (input.IsError(out var user, out var error))
        {
            return Result.Error(error);
        }

        var userBalance = database.UserBalances.FirstOrDefault(x => x.Id == user.Id);
        if (userBalance != null)
        {
            return Result.Ok(userBalance);
        }

        return Result.Error($"User balance with id {user.Id} not found");
    }
}

public class ModifyBalanceHandler : IPipeline<Result<UpdateUserBalanceState, string>, Result<decimal, string>>
{
    public Result<decimal, string> Execute(Result<UpdateUserBalanceState, string> input)
    {
        if (input.IsError(out var state, out var error))
        {
            return Result.Error(error);
        }

        var newBalance = state.UserBalance.Balance + state.Transaction.Amount;
        if (newBalance < 0)
        {
            return Result.Error("Insufficient funds");
        }

        state.UserBalance.Balance = newBalance;

        return Result.Ok(state.UserBalance.Balance);
    }
}

public class UpdateStateHandler<T>(UpdateUserBalanceState state) : IPipeline<Result<T, string>, Result<T, string>>
{
    public Result<T, string> Execute(Result<T, string> input)
    {
        if (input.IsError(out var value, out var error))
        {
            return Result.Error(error);
        }

        switch (value)
        {
            case User user:
                state.User = user;
                break;
            case UserBalance userBalance:
                state.UserBalance = userBalance;
                break;
        }

        return input;
    }
}
```

The asynchronous version also exists.
