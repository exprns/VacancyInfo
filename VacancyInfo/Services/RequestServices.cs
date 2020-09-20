﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

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

        public async Task<Stream> SendRequest(HttpRequestMessage request, string clientName)
        {
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
        public Task<Stream> SendRequest(HttpRequestMessage request, string clientName);
        public bool GetPullRequestsError { get; set; }
        public Stream Result { get; set; }
    }
}
