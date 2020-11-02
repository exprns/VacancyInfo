using Microsoft.AspNetCore.Mvc;
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
        private IJsonService _jsonService;
        private IVacancyService _vacancyService;

        public VacancyController(IVacancyService vacancyService, IJsonService jsonService)
        {
            _vacancyService = vacancyService;
            _jsonService = jsonService;
        }

        // GET api/Vacancy/Vacancies?name=5
        [HttpGet("Vacancies")]
        public async Task<List<HHVacancyModel>> GetVacanciesAsync(string name, string region = "")
        {
            return await _vacancyService.GetVacancies(name, region); // TODO: думаю тут сделать ответ о том что данные получены и можно строить графики
        }

        // GET api/Vacancy/VacanciesInDetail
        [HttpGet("VacanciesInDetail")]
        public async Task<List<HHVacancyModel>> GetVacanciesInDetailAsync()
        {
            return await _vacancyService.GetVacanciesInDetail(_vacancyService.VacanciesWithSalary.GetRange(1,50));
        }

        // GET api/Vacancy/GetAvarageSalary
        [HttpGet("GetAvarageSalary",Name ="getAvarageSalary")]
        public decimal GetAverageSalary()
        {
            return SalaryInfo.GetAverageSalary(_vacancyService.VacanciesWithSalary);
        }

        // GET api/Vacancy/GetAreasJson
        [HttpGet("GetAreasJson", Name = "getAreasJson")]
        public string GetAreasJson()
        {
            return _jsonService.JsonSerializeAllUnicode(_vacancyService.Areas);
        }

        // GET api/Vacancy/GetAverageRegionSalary?areaId=2
        [HttpGet("GetAverageRegionSalary", Name = "getAverageRegionSalary")]
        public decimal GetAverageRegionSalary(int areaId)
        {
            var vacs = _vacancyService.VacanciesByRegionWithSalary[areaId];
            if (!vacs.Any()) return -1;
            return SalaryInfo.GetAverageSalary(_vacancyService.VacanciesByRegionWithSalary[areaId]);
        }

        // GET api/Vacancy/GetRegionVacanciesJson?areaId=5
        [HttpGet("GetRegionVacanciesJson", Name = "getRegionVacanciesJson")]
        public string GetRegionVacanciesJson(int areaId)
        {
            var vacs = _vacancyService.VacanciesByRegion[areaId];
            return _jsonService.JsonSerializeAllUnicode(vacs);
        }

        // GET api/Vacancy/GetSkillsJson
        [HttpGet("GetSkillsJson", Name = "GetSkillsJson")]
        public string GetSkillsJson()
        {
            var skills = SkillsInfo.GetKeySkills(_vacancyService.VacanciesInDetail);
            return _jsonService.JsonSerializeAllUnicode(skills);
        }

        // GET api/Vacancy/GetSkillsWithStatsJson
        [HttpGet("GetSkillsWithStatsJson", Name = "GetSkillsWithStatsJson")]
        public string GetSkillsWithStatsJson()
        {
            var skills = SkillsInfo.GetKeySkillsWithStats(_vacancyService.VacanciesInDetail);
            return _jsonService.JsonSerializeAllUnicode(skills);
        }

    }
}
