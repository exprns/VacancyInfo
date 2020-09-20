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
        public Stream Result { get; set; }
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
                Result = responseStream;
            }
            else
            {
                GetPullRequestsError = true;
                Result = Stream.Null;
            }
        }
    }

    public interface IRequestServices
    {
        public void SendRequest(HttpRequestMessage request, string clientName);
        public bool GetPullRequestsError { get; set; }
        public Stream Result { get; set; }
    }
}
