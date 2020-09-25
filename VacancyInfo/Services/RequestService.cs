using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

namespace VacancyInfo.Services
{
    public class RequestService : IRequestServices
    {
        public Stream Result { get; set; }
        public bool GetPullRequestsError { get; set; }
        private IHttpClientFactory _httpClientFactory;

        public RequestService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Stream> SendRequest(string requestBody)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestBody);

            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                return responseStream;
            }
            else
            {
                GetPullRequestsError = true;
                return Stream.Null;
            }
        }
    }

    public interface IRequestServices
    {
        public Task<Stream> SendRequest(string requestBody);
        public bool GetPullRequestsError { get; set; }
        public Stream Result { get; set; }
    }
}
