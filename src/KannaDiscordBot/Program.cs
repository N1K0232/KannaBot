using KannaDiscordBot.Core;

namespace KannaDiscordBot;

public class Program
{
    public async static Task Main()
    {
        string token = Environment.GetEnvironmentVariable("Token");
        KannaBot bot = new(token);
        await bot.RunAsync();
    }
}