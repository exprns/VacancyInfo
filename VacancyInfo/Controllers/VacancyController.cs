﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VacancyInfo.Classes;
using VacancyInfo.Models;
using VacancyInfo.Services;

namespace VacancyInfo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private VacancyData _vacancyData;
        private IJsonService _jsonService;

        public VacancyController(IVacancyService vacancyService, IJsonService jsonService)
        {
            _vacancyData = new VacancyData(vacancyService);
            _jsonService = jsonService;
        }

        // GET api/Vacancy/Vacancies?name=5
        [HttpGet("Vacancies")]
        public async Task<List<HHVacancyModel>> GetVacanciesAsync(string name)
        {
            return await _vacancyData.GetVacanciesAsync(name); // TODO: думаю тут сделать ответ о том что данные получены и можно строить графики
        }

        // GET api/Vacancy/VacanciesInDetail
        [HttpGet("VacanciesInDetail")]
        public async Task<List<HHVacancyModel>> GetVacanciesInDetailAsync()
        {
            return await _vacancyData.GetVacanciesInDetail();
        }

        // GET api/Vacancy/GetAvarageSalary
        [HttpGet("GetAvarageSalary",Name ="getAvarageSalary")]
        public decimal GetAverageSalary()
        {
            return _vacancyData.GetAverageSalary();
        }

        // GET api/Vacancy/GetAreasJson
        [HttpGet("GetAreasJson", Name = "getAreasJson")]
        public string GetAreasJson()
        {
            var areas = _vacancyData.GetAreas();
            return _jsonService.JsonSerializeAllUnicode(areas);
        }

        // GET api/Vacancy/GetAverageRegionSalary?areaId=2
        [HttpGet("GetAverageRegionSalary", Name = "getAverageRegionSalary")]
        public decimal GetAverageRegionSalary(int areaId)
        {
            var areas = _vacancyData.GetAreas();
            var regSalaries = _vacancyData.GetRegionAverageSalary(areaId);
            return regSalaries;
        }

        // GET api/Vacancy/GetRegionVacanciesJson?areaId=5
        [HttpGet("GetRegionVacanciesJson", Name = "getRegionVacanciesJson")]
        public string GetRegionVacanciesJson(int areaId)
        {
            var vacs = _vacancyData.GetRegionVacancies(areaId);
            return _jsonService.JsonSerializeAllUnicode(vacs);
        }

    }
}
