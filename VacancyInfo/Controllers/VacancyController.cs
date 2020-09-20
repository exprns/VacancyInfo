using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using VacancyInfo.Services;

namespace VacancyInfo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private string _hhName = "https://api.hh.ru/vacancies";
        private IRequestServices _requestServices;

        public VacancyController(IRequestServices requestServices)
        {
            _requestServices = requestServices;
        }

        // GET api/<VacancyController>/5
        [HttpGet("{vacancyName}")]
        public async System.Threading.Tasks.Task<string> GetAsync(string vacancyName)
        {
            string requestBody = _hhName + "?text=" + vacancyName;
            var request = new HttpRequestMessage(HttpMethod.Get, requestBody);
            request.Headers.Add("Accept", "application/vnd.github.v3+json"); //  нужно ли это вообще???????????
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample"); 
            string clientName = "hh";
            var responce =  await _requestServices.SendRequest(request, clientName);
            if (!_requestServices.GetPullRequestsError)
            {
                using (FileStream fs = new FileStream("requestAnswer.json", FileMode.OpenOrCreate))
                {
                    responce.Seek(0,SeekOrigin.Begin);
                    responce.CopyTo(fs);
                    fs.Close();
                }
                return "VseNorm";
                //return _requestServices.Result;
            }
            else
            {
                //return new List<HHVacancyModel>();
                return "ErrorRequest";
            }
            
        }
    }
}
