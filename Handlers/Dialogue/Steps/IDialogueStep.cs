﻿using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace MyDiscordBot.Handlers.Dialogue.Steps
{
    public interface IDialogueStep
    {
        Action<DiscordMessage> OnMessageAdded { get; set; }
        IDialogueStep NextStep { get; }
        Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);
    }
}