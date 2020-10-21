using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacancyInfo.Models;
using VacancyInfo.Models.HHModels;
using VacancyInfo.Services;

namespace VacancyInfo.Classes
{
    public interface IVacancyData
    {
        Task<List<HHVacancyModel>> GetVacanciesAsync(string name, string region = "");
        List<HHVacancyModel> GetVacanciesInDetail();
        decimal GetAverageSalary();
        Dictionary<Area, decimal> GetRegionsSalaries();
        decimal GetRegionSalary(Area area);
    }

    public class VacancyData : IVacancyData
    {
        private IVacancyService _vacancyService;

        public VacancyData(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }

        public async Task<List<HHVacancyModel>> GetVacanciesAsync(string name, string region = "")
        {
            await _vacancyService.GetVacancies(name, region);
            return new List<HHVacancyModel>(); // TODO: думаю тут сделать ответ о том что данные получены и можно строить графики
        }

        public List<HHVacancyModel> GetVacanciesInDetail() => _vacancyService.VacanciesInDetail;

        public List<Area> GetAreas() => _vacancyService.Areas;
        public Dictionary<Area, List<HHVacancyModel>> GetVacanciesByRegionWithSalary() => _vacancyService.VacanciesByRegionWithSalary;
        public decimal GetAverageSalary() => GetAverageSalary(_vacancyService.VacanciesWithSalary);
        
        private decimal GetAverageSalary(List<HHVacancyModel> vacancies)
        {
            decimal avgFrom = vacancies.Sum(x => x.salary.from.Value) / vacancies.Count;
            decimal avgTo = vacancies.Sum(x => x.salary.to.Value) / vacancies.Count;

            return (avgFrom + avgTo) / 2;
        }

        public Dictionary<Area, decimal> GetRegionsSalaries()
        {
            Dictionary<Area, decimal> avgSalaryByReg = new Dictionary<Area, decimal>();
            foreach(var regionVacancies in _vacancyService.VacanciesByRegionWithSalary)
            {
                avgSalaryByReg.Add(regionVacancies.Key, GetRegionSalary(regionVacancies.Key));
            }
            return avgSalaryByReg;
        }

        public decimal GetRegionSalary(Area area) => GetAverageSalary(_vacancyService.VacanciesByRegionWithSalary[area]);
    }
}