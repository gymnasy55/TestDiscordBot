using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;

namespace MyDiscordBot.Commands
{
    public class LolCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Returns Pong")]
        public async Task Ping(CommandContext ctx)
        {
            var user = ctx.Member.Mention;
            await ctx.Channel.SendMessageAsync($"{user} Pong").ConfigureAwait(false);
        }

        [Command("add")]
        [Description("Adds two numbers")]
        public async Task Add(CommandContext ctx,
            [Description("First number")] int first,
            [Description("Second number")] int second)
        {
            var user = ctx.Member.Mention;
            await ctx.Channel.SendMessageAsync($"{user} {first + second}").ConfigureAwait(false);
        }

        [Command("respondmessage")]
        public async Task RespondMessage(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync(message.Result.Content);
        }

        [Command("respondemoji")]
        public async Task RespondEmoji(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var message = await interactivity.WaitForReactionAsync(x => x.Channel == ctx.Channel && x.User == ctx.User).ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync(message.Result.Emoji);
        }
    }
}