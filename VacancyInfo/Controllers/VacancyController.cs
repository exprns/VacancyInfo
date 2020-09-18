using System.Collections.Generic;
using System.Net.Http;
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
        public List<HHVacancyModel> Get(string vacancyName)
        {
            string requestBody = _hhName + "?text" + vacancyName;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestBody);
            string clientName = "hh";
            _requestServices.SendRequest(request, clientName);
            if (!_requestServices.GetPullRequestsError)
            {
                return _requestServices.result;
            }
            else
            {
                return new List<HHVacancyModel>();
            }
            
        }
    }
}
