# T4T.CQRS

![](https://github.com/team4talent/t4t-cqrs/workflows/develop/badge.svg?branch=develop)

Contains infrastructure and fluent extensions to get started using the CQRS pattern.

This repository contains the following projects:

- T4T.CQRS
- T4T.CQRS.Api
- T4T.CQRS.Autofac

## Packages

`T4T.CQRS` is the core package that contains unopiniated infrastructure to build application with CQRS.

`T4T.CQRS.Api` adds utility classes to easily incorporate T4T.CQRS in an API application.

`T4T.CQRS.Autofac` is a provider for T4T.CQRS that uses Autofac to wire up implementations.

## Installation

> No packages have yet been published.

T4T.CQRS will be available on [NuGet](https://www.nuget.org) soon 🤞:

```shell
dotnet add package T4T.CQRS
dotnet add package T4T.CQRS.Api
dotnet add package T4T.CQRS.Autofac
```

## Usage

See the sample to see a fully working application using all three packages. Comments are added throughout to show options and explain usages.

The following demonstrates the quickest way to get started:

1. Load the `CQRSModule` from `T4T.CQRS.Autofac`, this module will automatically register all the handlers found in the given assemblies:

```csharp
builder.RegisterModule(new CQRSModule());
```

2. Create your Queries and Commands, these are simple POCOs:

```csharp
public class GetTodosQuery
{

}
```

```csharp
public class GetTodosQueryResult : ExecutionResult
{
    public IEnumerable<TodoItem> Todos { get; set; }
}
```

```csharp
public class MarkTodoAsDoneCommand
{
    public int Id { get; set; }
}
```

3. Create your handlers:

```csharp
public class GetTodosQueryHandler : IQueryHandler<GetTodosQuery, GetTodosQueryResult>
{
    public Task<GetTodosQueryResult> Handle(
            GetTodosQuery query,
            CancellationToken cancellationToken = default)
    {

    }
}
```

```csharp
public class MarkTodoItemAsDoneCommandHandler : ICommandHandler<MarkTodoItemAsDoneCommand>
{
    public Task<ExecutionResult> Handle(
            MarkTodoItemAsDoneCommand command,
            CancellationToken cancellationToken = default)
    {

    }
}
```

4. Use it!

```csharp
[HttpGet]
public async Task<IActionResult> Get(CancellationToken token = default)
{
    var query = new GetTodosQuery();
    var result = await HandleQuery<GetTodosQuery, GetTodosQueryResult>(query, token);
    return result.ToActionResult();
}
```

```csharp
[HttpPost("{id}/markasdone")]
public async Task<IActionResult> MarkAsDone(int id, CancellationToken token = default)
{
    var command = new MarkTodoItemAsDoneCommand(id);
    var result = await HandleCommand(command, token);
    return result.ToActionResult();
}
```
