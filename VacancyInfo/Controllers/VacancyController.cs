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
        private IVacancyDataHandlerService _vacancyDataHandlerService;

        public VacancyController(IVacancyService vacancyService, IJsonService jsonService, IVacancyDataHandlerService service)
        {
            _vacancyService = vacancyService;
            _jsonService = jsonService;
            _vacancyDataHandlerService = service;
        }

        // GET api/Vacancy/Vacancies?name=5
        [HttpGet("Vacancies")]
        public async Task<List<HHVacancyModel>> GetVacanciesAsync( string name, string region = "")
        {
            var vacs = await _vacancyService.GetVacancies(name, region);
            return vacs; // TODO: думаю тут сделать ответ о том что данные получены и можно строить графики
        }

        // GET api/Vacancy/VacanciesInDetail
        [HttpGet("VacanciesInDetail")]
        public async Task<List<HHVacancyModel>> GetVacanciesInDetailAsync()
        {
            var vacIds = _vacancyDataHandlerService.GetVacanciesWithSalary(_vacancyService.Vacancies).GetRange(1, 50).Select(_=>int.Parse(_.id));
            return await _vacancyService.GetVacanciesInDetail(vacIds);
        }

        // TODO: всё что ниже - вынести в отдельные контроллеры
        // GET api/Vacancy/GetAvarageSalary
        [HttpGet("GetAvarageSalary", Name = "getAvarageSalary")]
        public decimal GetAverageSalary([FromServices] IVacancyDataHandlerService service)
        {
            return SalaryInfo.GetAverageSalary(service.GetVacanciesWithSalary(_vacancyService.Vacancies));

        }
        // GET api/Vacancy/GetAverageRegionSalary?areaId=2
        [HttpGet("GetAverageRegionSalary", Name = "getAverageRegionSalary")]
        public decimal GetAverageRegionSalary(int areaId)
        {
            var vacs = _vacancyDataHandlerService.GetRegionVacancies(_vacancyDataHandlerService.GetVacanciesWithSalary(_vacancyService.Vacancies), areaId);
            if (!vacs.Any()) return -1;
            return SalaryInfo.GetAverageSalary(vacs);
        }

        // GET api/Vacancy/GetAreasJson
        [HttpGet("GetAreasJson", Name = "getAreasJson")]
        public string GetAreasJson()
        {
            return _jsonService.JsonSerializeAllUnicode(_vacancyDataHandlerService.GetAreas(_vacancyService.Vacancies));
        }

        // GET api/Vacancy/GetRegionVacanciesJson?areaId=5
        [HttpGet("GetRegionVacanciesJson", Name = "getRegionVacanciesJson")]
        public string GetRegionVacanciesJson(int areaId)
        {
            var vacs = _vacancyDataHandlerService.GetRegionVacancies(_vacancyService.Vacancies, areaId);
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
