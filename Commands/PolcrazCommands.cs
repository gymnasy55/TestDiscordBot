using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using YoutubeExplode;

namespace MyDiscordBot.Commands
{
    public class PolcrazCommands : BaseCommandModule
    {
        [Command("roll")]
        [Description("Returns random value")]
        public async Task Roll(CommandContext ctx,
            [Description("First Value")] int first,
            [Description("Second Value")] int second = 0)
        {
            var user = ctx.Member.Mention;
            var rndValue = new Random().Next(first < second ? first : second, (first > second ? first : second) + 1);
            await ctx.Channel.SendMessageAsync($"{user} {rndValue}");
        }

        [Command("github")]
        [Description("Returns our repositories list")]
        public async Task Github(CommandContext ctx)
        {
            var message = new DiscordEmbedBuilder
            {
                Title = "Repositories",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail {Url = ctx.Client.CurrentUser.AvatarUrl},
            };
            var user = ctx.Member.Mention;
            var list = Repositories.GetRepositories("https://api.github.com/users/gymnasy55/repos?sort=updated").Result;
            await ctx.Channel.SendMessageAsync($"{user}").ConfigureAwait(false);
            var kekW = "```\n# Heading 1\n";
            foreach (var repository in list)
            {
                kekW += $"{repository.Name}: [link goto]({repository.Url})\n";
            }

            kekW += "```";
            await ctx.Channel.SendMessageAsync($"{kekW}").ConfigureAwait(false);

        }

        [Command("search")]
        [Description("Returns youtube search list")]
        public async Task Search(CommandContext ctx, string query)
        {
            var youtube = new YoutubeClient();
            var k = await youtube.Search.GetVideosAsync(query, 0, 1);
            var message = "```markdown\n";
            foreach (var video1 in k)
            {
                message += $"{video1.Title} | {video1.Author} | {video1.Duration}\n";
            }

            message += "```";
            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }
    }
}