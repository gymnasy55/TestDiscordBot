using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using MyDiscordBot.Handlers.Dialogue;
using MyDiscordBot.Handlers.Dialogue.Steps;

namespace MyDiscordBot.Commands
{
    public class LolCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Returns Pong")]
        //[RequireCategories(ChannelCheckMode.Any, "TEXT")]
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
            var message = await interactivity.WaitForReactionAsync(x => x.Channel == ctx.Channel && x.User == ctx.User)
                .ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync(message.Result.Emoji);
        }

        [Command("dialogue")]
        public async Task Dialogue(CommandContext ctx)
        {
            var inputStep = new TextStep("Enter smth interesting", null, 5);
            var funnyStep = new IntStep("MUR MUR MUR", null, maxValue: 100);

            var input = string.Empty;
            var value = 0;

            inputStep.OnValidResult += result =>
            {
                input = result;
                if (result == "SupDvach!") inputStep.SetNextStep(funnyStep);
            };

            funnyStep.OnValidResult += result => value = result;

            var userChannel = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);
            var inputDialogueHandler = new DialogueHandler(ctx.Client, userChannel, ctx.User, inputStep);
            var succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);
            if (!succeeded) return;
            await ctx.Channel.SendMessageAsync(input).ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync($"{value}").ConfigureAwait(false);
        }
    }
}