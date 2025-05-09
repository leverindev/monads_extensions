# Functional Programming Monads Extensions

This library provides useful functional programming monads: **Option** (also known as Maybe)  and **Result**.

## Motivation

The **Maybe** and **Result** monads are handy patterns from the world of functional programming. They are present in all functional languages and in many others (e.g., Rust).

### Maybe
The **Maybe** monad (aka **Option**) is a design pattern that allows a chain of function calls to be interrupted if any function in the chain fails to return a meaningful result.

```
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

```
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

```
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

```
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
```
Entity result = provider.Get(id).UnwrapOrException("Entity not found");
```

Return *null*:
```
Entity? result = provider.Get(id).UnwrapOrDefault();
```

Retrieve the value, but leave the option to handle the error:
```
Entity? result = provider.Get(id).UnwrapOrElse(error =>
{
    Console.WriteLine(error);
    return null;
});
```

### Usage example

Additionally, the library provides extension methods for constructing processing chains (**Map**, **Do**, **Bind**).

Let's assume we need to implement a method that parses a string into a number, and if the number is natural, then squares it, otherwise (invalid string or non-natural number) returns -1, and also logs an error to the console if the string is invalid.

```
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
