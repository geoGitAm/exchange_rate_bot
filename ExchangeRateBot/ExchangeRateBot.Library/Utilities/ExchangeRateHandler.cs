﻿using ExchangeRateBot.Library.Models;
using ExchangeRateBot.Library.Observers.ExchangeRateHandlerObservers;
using ExchangeRateBot.Library.Observers.ExchangeRateObservers;
using ExchangeRateBot.Library.Strategies;
using ExchangeRateBot.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ExchangeRateBot.Library.Utilities
{
    /// <summary>
    /// Represents an exchange rate handler.
    /// </summary>
    public class ExchangeRateHandler : IExchangeRateHandler, IExchangeRateHandlerSubject
    {
        private readonly IEnumerable<IExchangeRateHandlerObserver> _availableObservers;
        private List<IExchangeRateHandlerObserver> _observers;

        public IExchangeRateRequest Request { get; set; }
        public IExchangeRateHandlerStrategy Strategy { get; set; }

        public ExchangeRateHandler(IEnumerable<IExchangeRateHandlerObserver> availableObservers)
        {
            _observers = new List<IExchangeRateHandlerObserver>();
            _availableObservers = availableObservers;

            AttachObservers();
        }

        public async Task<IExchangeRate> GetExchangeRate()
        {
            Notify();

            var exchangeRate = await Strategy.ExecuteAsync(Request);

            return exchangeRate;
        }

        public void SetNewRequest(string requestMessage)
        {
            var requestDetails = requestMessage.Split(' ');
            IExchangeRateRequest inputRequest = Factory.CreateExchangeRequest();

            if (requestDetails.Length == 4)
            {
                inputRequest.Currency = requestDetails[2];
                inputRequest.Date = DateTime.ParseExact(
                        requestDetails[3], "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                inputRequest.Country = "UA";

                Request = inputRequest;
            }
            else
            {
                inputRequest.Currency = requestDetails[2];
                inputRequest.Date = DateTime.ParseExact(
                        requestDetails[3], "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture
                        );
                inputRequest.Country = requestDetails[4];

                Request = inputRequest;
            }
        }

        public void Attach(IExchangeRateHandlerObserver observer)
        {
            if (_observers.Contains(observer) == false)
            {
                _observers.Add(observer);
            }
        }

        public void Detach(IExchangeRateHandlerObserver observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        public void Notify()
        {
            if (_observers.Count > 0)
            {
                foreach (var observer in _observers)
                {
                    observer.Update(this);
                }
            }
        }

        private void AttachObservers()
        {
            foreach (var observer in _availableObservers)
            {
                if (_observers.Contains(observer) == false)
                {
                    _observers.Add(observer);
                }
            }
        }
    }
}
