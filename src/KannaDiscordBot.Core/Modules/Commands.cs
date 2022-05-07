using Discord;
using Discord.Commands;

namespace KannaDiscordBot.Core.Modules;

public class Commands : ModuleBase<SocketCommandContext>
{
    [Command("ping")]
    public async Task Ping()
    {
        await ReplyAsync("pong");
    }

    [Command("hug")]
    public Task Hug(IGuildUser user = null)
    {
        throw new NotImplementedException();
    }

    [Command("ban")]
    public async Task Ban(IUser user = null, [Remainder] string reason = null)
    {
        if (user == null)
        {
            await ReplyAsync("Please specify a user");
            return;
        }
        await Context.Guild.AddBanAsync(user, 1, reason ?? "Not specified");

        EmbedBuilder embedBuilder = new EmbedBuilder()
            .WithDescription($":white_check_mark: {user.Mention} was banned\n**Reason**{reason ?? "Not specified"}")
            .WithFooter(footer =>
            {
                footer.WithText("User banned").WithIconUrl("https://i.imgur.com/6Bi17B3.png");
            });

        Embed embed = embedBuilder.Build();
        await ReplyAsync(embed: embed);
    }
}