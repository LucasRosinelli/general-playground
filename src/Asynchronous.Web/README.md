# Asynchronous calls
This project intends to show the ability of cancel a request through `CancellationToken` in Web projects, in this case a **Web API**.

There are four endpoints available:
- /Sample/DoPropagateAbort - Uses the cancellation token to cancel a request with an exception.
- /Sample/DoPropagateGracefully - Uses the cancellation token to cancel a request gracefully.
- /Sample/DoNotPropagateToCalled - Ignores the existing cancellation token as the called method doesn't receive it. The called method continues its execution in background.
- /Sample/DoNotPropagate - There is no cancellation token specified in the controller action, so the called method continues its execution in background.

> I recommend to use **Postman** (or other similar tool) that allows you to cancel a given request as *Cancel* option available in *Swagger interface* do not cancel your request.

## Build
```shell
dotnet build ./GeneralPlayground.sln
```

## Run
```shell
dotnet run --project ./src/Asynchronous.Web/Asynchronous.Web.csproj
```

## Call
```
https://localhost:5001/Sample/ACTION/EXECUTION_TIME
```

Replace:
- `ACTION` - the action name you intend to verify
- `EXECUTION_TIME` - the execution time in milliseconds

You can open `https://localhost:5001/swagger` for more information and description.
