using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

namespace VacancyInfo.Services
{
    public interface IRequestServices
    {
        public Task<Stream> SendRequest(string requestBody);
    }

    public class RequestService : IRequestServices
    {
        private HttpClient _client;

        public RequestService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        public async Task<Stream> SendRequest(string requestBody)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestBody);

            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                return responseStream;
            }
            else
            {
                return Stream.Null;
            }
        }
    }
}
