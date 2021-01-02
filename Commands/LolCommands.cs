﻿using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

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
    }
}