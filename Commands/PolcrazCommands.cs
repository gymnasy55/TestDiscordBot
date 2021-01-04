using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace MyDiscordBot.Commands
{
    public class PolcrazCommands : BaseCommandModule
    {
        [Command("roll"), Description("Returns random value")]
        public async Task Roll(CommandContext ctx,
            [Description("First Value")] int first,
            [Description("Second Value")] int second = 0)
        {
            var user = ctx.Member.Mention;
            var rndValue = new Random().Next(first < second ? first : second, (first > second ? first : second) + 1);
            await ctx.Channel.SendMessageAsync($"{user} {rndValue}");
        }

        [Command("github"), Description("Returns our repositories list")]
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

        [Command("search"), Description("Returns youtube search list")]
        public async Task Search(CommandContext ctx, string query)
        {
            var youtube = new YoutubeClient();
            var k = await youtube.Search.GetVideosAsync(query, 0, 1);
            var message = k.Aggregate("```markdown\n", (current, video1) => current + $"{video1.Title} | {video1.Author} | {video1.Duration}\n");
            message += "```";
            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }

        [Command("play"), Description("Play a song")]
        public async Task Play(CommandContext ctx, string query)
        {
            var youtube = new YoutubeClient();
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(query);
            var streamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();
            await ctx.Guild.CreateVoiceChannelAsync("123");
        }

        [Command("joinvoice"), Description("Connect bot to your voice channel")]
        public async Task Join(CommandContext ctx, DiscordChannel chn = null)
        {
            //check whether VNext is enabled
            var vnext = ctx.Client.GetVoiceNext();
            if (vnext == null)
            {
                //not enabled
                await ctx.RespondAsync("VNext is not enabled or configured.");
                return;
            }

            //check whether we aren't already connected
            var vnc = vnext.GetConnection(ctx.Guild);
            if (vnc != null)
            {
                //already connected
                await ctx.RespondAsync("Already connected in this guild.");
                return;
            }

            //get member's voice state
            var vstat = ctx.Member?.VoiceState;
            if (vstat?.Channel == null && chn == null)
            {
                //they did not specify a channel and are not in one
                await ctx.RespondAsync("You are not in a voice channel.");
                return;
            }

            //channel not specified, use user's
            if (chn == null)
                chn = vstat.Channel;

            //connect
            vnc = await vnext.ConnectAsync(chn);
            await ctx.RespondAsync($"Connected to `{chn.Name}`");
        }

        [Command("leavevoice"), Description("Connect bot to your voice channel")]
        public async Task Leave(CommandContext ctx)
        {
            // check whether VNext is enabled
            var vnext = ctx.Client.GetVoiceNext();
            if (vnext == null)
            {
                // not enabled
                await ctx.RespondAsync("VNext is not enabled or configured.");
                return;
            }

            // check whether we are connected
            var vnc = vnext.GetConnection(ctx.Guild);
            if (vnc == null)
            {
                // not connected
                await ctx.RespondAsync("Not connected in this guild.");
                return;
            }

            // disconnect
            vnc.Disconnect();
            await ctx.RespondAsync("Disconnected");
        }
    }
}