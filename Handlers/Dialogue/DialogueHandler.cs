﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using MyDiscordBot.Handlers.Dialogue.Steps;

namespace MyDiscordBot.Handlers.Dialogue
{
    public class DialogueHandler
    {
        private readonly DiscordClient _client;
        private readonly DiscordChannel _channel;
        private readonly DiscordUser _user;
        private IDialogueStep _currentStep;

        public DialogueHandler(DiscordClient client, DiscordChannel channel, DiscordUser user, IDialogueStep startingStep)
        {
            _client = client;
            _channel = channel;
            _user = user;
            _currentStep = startingStep;
        }

        private readonly List<DiscordMessage> _messages = new List<DiscordMessage>();

        public async Task<bool> ProcessDialogue()
        {
            while (_currentStep != null)
            {
                _currentStep.OnMessageAdded += (message) => _messages.Add(message);
                var canceled = await _currentStep.ProcessStep(_client, _channel, _user).ConfigureAwait(false);
                if (canceled)
                {
                    await DeleteMessages().ConfigureAwait(false);
                    var cancelEmbed = new DiscordEmbedBuilder
                    {
                        Title="The Dialogue Has Successfully Been Cancelled",
                        Description = _user.Mention,
                        Color = DiscordColor.Blurple
                    };
                    await _channel.SendMessageAsync(embed: cancelEmbed).ConfigureAwait(false);
                    return false;
                }

                _currentStep = _currentStep.NextStep;
            }

            await DeleteMessages().ConfigureAwait(false);
            return true;
        }

        private async Task DeleteMessages()
        {
            if(_channel.IsPrivate) return;
            foreach (var message in _messages)
            {
                await message.DeleteAsync().ConfigureAwait(false);
            }
        }
    }
}