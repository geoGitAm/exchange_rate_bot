﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExchangeRateBot.Library.Commands
{
    /// <summary>
    /// Represents a command interface.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Represents command type.
        /// </summary>
        CommandType CommandType { get; }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="message">Chat message.</param>
        /// <param name="telegramBotClient">Telergam bot client.</param>
        Task ExecuteAsync(Message message, ITelegramBotClient telegramBotClient);
    }
}
