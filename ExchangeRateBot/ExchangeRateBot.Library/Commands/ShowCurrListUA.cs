﻿using ExchangeRateBot.Library.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExchangeRateBot.Library.Commands
{
    public class ShowCurrListUA : IShowCurrListUA
    {
        private readonly IChatMessageSender _chatMessageSender;
        private readonly string _supportedCurrencies;
        private readonly string _name;

        public ShowCurrListUA(IChatMessageSender chatMessageSender)
        {
            _name = "/SHOWCURRLISTUA";
            _chatMessageSender = chatMessageSender;
            _supportedCurrencies = GetValuesFromEnum();

            string GetValuesFromEnum()
            {
                StringBuilder stringBuilder = new StringBuilder();
                var currencies = (string[])Enum.GetNames(typeof(SupportedCurrenciesUA));

                for (int i = 0; i <= currencies.Length - 1; i++)
                {
                    if (i != currencies.Length - 1)
                    {
                        stringBuilder.Append($"{ currencies[i] },\n");
                    }
                    else
                    {
                        stringBuilder.Append($"{ currencies[i] }");
                    }
                }

                return stringBuilder.ToString();
            }
        }

        public bool Contains(string command)
        {
            return command.Contains($"@{ BotSettings.Name }") && command.Contains(this._name);
        }

        public async Task Execute(Message message, ITelegramBotClient telegramBotClient)
        {
            await _chatMessageSender.SendShowCurrListMessage(message, _supportedCurrencies, telegramBotClient);
        }
    }
}