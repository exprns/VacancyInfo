using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VacancyInfo.Models;

namespace VacancyInfo.Services
{
    public interface ICurrencyService
    {
        private string GetCurreciesRequest
        {
            get => "http://www.cbr.ru/scripts/XML_daily.asp?";
        }
    }

    public class CurrencyService : ICurrencyService
    {
        private string GetCurreciesRequest { get; }
        private List<Currecny> _currecnies;

        private IRequestServices _requestService;

        public CurrencyService(IRequestServices requestService)
        {
            _requestService = requestService;
            var requestResult = SendRequest();
            var xmlCurrencies = GetXmlFromRequest(requestResult.Result);
            _currecnies = new List<Currecny>();
            foreach(XmlDocument xmlCurrency in xmlCurrencies.DocumentElement)
            {
                Currecny cur = new Currecny()
                {
                    Id = int.Parse(xmlCurrency.Value),
                    Name = xmlCurrencies.GetElementsByTagName(nameof(cur.Name)).Item(0).Value,
                    CharCode = xmlCurrencies.GetElementsByTagName(nameof(cur.CharCode)).Item(0).Value,
                    Nominal = int.Parse(xmlCurrencies.GetElementsByTagName(nameof(cur.Nominal)).Item(0).Value),
                    NumCode = int.Parse(xmlCurrencies.GetElementsByTagName(nameof(cur.NumCode)).Item(0).Value),
                    Value = double.Parse(xmlCurrencies.GetElementsByTagName(nameof(cur.Value)).Item(0).Value)
                };
                _currecnies.Add(cur);
            }
        }

        public double GetCurrencyValue(string curName) 
        {
            var resultCur = _currecnies.FirstOrDefault();
            if (resultCur != null)
                return resultCur.Value;
            throw new Exception(); // TODO: Заменить в будущем на возвращение нуля
                     
        }

        private async Task<Stream> SendRequest() 
        {
            return await _requestService.SendRequest(GetCurreciesRequest);
        }

        private XmlDocument GetXmlFromRequest(Stream stream)
        {
            XmlDocument xmlCurrencies = new XmlDocument();
            xmlCurrencies.Load(stream);
            return xmlCurrencies;
        }
    }
}
