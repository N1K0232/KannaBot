using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace KannaDiscordBot.Core;

public class KannaBot
{
    private DiscordSocketClient socketClient;
    private CommandService commandService;
    private IServiceProvider serviceProvider;

    private readonly string token;

    public KannaBot(string token)
    {
        this.token = token;

        socketClient = null;
        commandService = null;
        serviceProvider = null;
    }

    public async Task RunAsync()
    {
        socketClient = new DiscordSocketClient();
        commandService = new CommandService();
        var services = new ServiceCollection()
            .AddSingleton(socketClient)
            .AddSingleton(commandService);

        serviceProvider = services.BuildServiceProvider();

        socketClient.Log += SocketClient_Log;
        await RegisterCommandsAsync();
        await socketClient.LoginAsync(TokenType.Bot, token);
        await socketClient.StartAsync();
        await Task.Delay(-1);
    }

    private Task SocketClient_Log(LogMessage logMessage)
    {
        Console.WriteLine(logMessage);
        return Task.CompletedTask;
    }

    private async Task RegisterCommandsAsync()
    {
        socketClient.MessageReceived += HandleCommandAsync;
        await commandService.AddModulesAsync(Assembly.GetExecutingAssembly(), serviceProvider);
    }
    private async Task HandleCommandAsync(SocketMessage socketMessage)
    {
        var message = socketMessage as SocketUserMessage;
        var context = new SocketCommandContext(socketClient, message);
        if (message.Author.IsBot) return;

        int argPos = 0;
        if (message.HasStringPrefix("!", ref argPos))
        {
            var result = await commandService.ExecuteAsync(context, argPos, serviceProvider);
            if (!result.IsSuccess)
            {
                Console.WriteLine(result.ErrorReason);
            }
            if (result.Error.Equals(CommandError.UnmetPrecondition))
            {
                await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}