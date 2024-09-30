# Telegramper

> A new [iteration](https://leaf-amber-dca.notion.site/Pipeliner-31f0aac674da40f18e09f9dd366498d2) is coming

---

It is framework similar to a ASP.Net Core. Framework contains services, middlewares, configuration, controllers(executors) and other.

> The framework is under development, so unexpected errors, changes in functionality, and names are possible! I would be grateful if you could report any bugs or functionality you need.

## Installation
To install the framework, you can use NuGet.org https://www.nuget.org/packages/Telegramper.

## Learn more about the framework
1. [About UpdateContext (similar to HttpContext)](https://github.com/GineTik/Telegramper-TelegramFramework/blob/master/Documentation/Core/UpdateContext.md)
1. [Configuration bot in Program.cs (Services, Middlewares, etc.)](https://github.com/GineTik/Telegramper-TelegramFramework/blob/master/Documentation/Core/ConfigurationOfFramework.md)
1. [Executors and attributes](https://github.com/GineTik/Telegramper-TelegramFramework/tree/master/Documentation/Executors)
1. [Session](https://github.com/GineTik/Telegramper-TelegramFramework/tree/master/Documentation/Sessions)

### Chapters coming soon
- Dialog (comleted, documentation in process)
- Examples of projects written on the Telegramper framework

## Donates
If you want to support to me, you can do it send me a crypto: BNB Smart Chain 0x0aB4b9A02bcaF822c3AeA91bc566aEA4194e1cEF

## Quick start
```cs
internal class Program
{
   static void Main(string[] args)
   {
       var builder = new BotApplicationBuilder();
       builder.ConfigureApiKey("your api key");
       builder.ReceiverOptions.ConfigureAllowedUpdates(UpdateType.Message, UpdateType.CallbackQuery); // default is UpdateType.Message
       builder.Services.AddExecutors(); // identical to the controller in ASP.Net Core
   
       var app = builder.Build();
       app.UseExecutors();
       app.RunPolling(); // webhooks are not implemented, but in the future you will be able to, for example, change polling to webhooks and vice versa
   }
}

public class BasicExecutor : Executor
{
    [TargetCommand] // identical to [TargetCommand("start")]
    public async Task Start()
    {
        var sender = UpdateContext.User.ToString();
        await Client.SendTextMessageAsync($"You are {sender}"); // send a text response
    }

    [TargetCommand("echo, command2")]
    [EmptyParameterSeparator] // remove separator, by default is space(" ")
    public async Task Echo(string phrase) // more about the parameters later 
    {
        await Client.SendTextMessageAsync(phrase);
    }
}
```
