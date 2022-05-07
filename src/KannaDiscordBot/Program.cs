using KannaDiscordBot.Core;
using System.Text;

namespace KannaDiscordBot;

public class Program
{
    public async static Task Main()
    {
        string encodedAccessToken = Environment.GetEnvironmentVariable("AccessToken");
        byte[] bytes = Convert.FromBase64String(encodedAccessToken);
        string accessToken = Encoding.UTF8.GetString(bytes);
        KannaBot bot = new(accessToken);
        await bot.RunAsync();
    }
}