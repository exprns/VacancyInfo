using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.IO;

namespace VacancyInfo.Services
{
    public class RequestServices : IRequestServices
    {
        public Stream requestValue { get; set; }
        public bool GetPullRequestsError { get; set; }
        private IHttpClientFactory _httpClientFactory;
        public RequestServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async void SendRequest(HttpRequestMessage request, string clientName)
        {
            var client = _httpClientFactory.CreateClient(clientName);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                requestValue = responseStream;
            }
            else
            {
                GetPullRequestsError = true;
                requestValue = Stream.Null;
            }
        }
    }

    public interface IRequestServices
    {
        public void SendRequest(HttpRequestMessage request, string clientName);
        public bool GetPullRequestsError { get; set; }
        public Stream requestValue { get; set; }
    }
}
