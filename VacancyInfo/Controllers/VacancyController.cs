using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacancyInfo.Services;

namespace VacancyInfo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private IVacancyService _vacancyService;
        private IRequestServices _requestServices;

        public VacancyController(IRequestServices requestServices, IVacancyService vacancyService)
        {
            _requestServices = requestServices;
            _vacancyService = vacancyService;
        }

        // GET api/Vacancy/GetVacancies?name=5
        [HttpGet("GetVacancies")]
        public async Task<List<HHVacancyModel>> GetVacanciesAsync(string name)
        {
            await _vacancyService.GetVacancies(name);
            return new List<HHVacancyModel>(); // TODO: думаю тут сделать ответ о том что данные получены и можно строить графики
        }

        // GET api/Vacancy/GetAvarageSalary
        [HttpGet("GetAvarageSalary",Name ="getAvarageSalary")]
        public decimal GetAverageSalary()
        {
            return _vacancyService.GetAverageSalary();
        }


    }
}
