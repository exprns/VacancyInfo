using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace VacancyInfo.Services
{
    public interface IJsonService
    {
        public string JsonSerialize<T>(T objToSerialize);
        public string JsonSerializeAllUnicode<T>(T requestBody);
    }
    public class JsonService : IJsonService
    {
        private JsonSerializerOptions _allUnicodeOpt = new JsonSerializerOptions()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
        };
        public string JsonSerialize<T>(T objToSerialize) => JsonSerializer.Serialize<T>(objToSerialize);
        public string JsonSerializeAllUnicode<T>(T objToSerialize) => JsonSerializer.Serialize<T>(objToSerialize, _allUnicodeOpt);
    }
}
