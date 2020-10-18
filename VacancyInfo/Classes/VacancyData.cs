using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacancyInfo.Models;
using VacancyInfo.Services;

namespace VacancyInfo.Classes
{
    public interface IVacancyData
    {
        Task<List<HHVacancyModel>> GetVacanciesAsync(string name, string region = "");
        Task<List<HHVacancyModel>> GetVacanciesInDetailAsync();
        decimal GetAverageSalary();
        Dictionary<int, decimal> GetRegionsSalaries();
        decimal GetRegionSalary(int regionID);
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

        public async Task<List<HHVacancyModel>> GetVacanciesInDetailAsync()
        {
            return _vacancyService.VacanciesInDetail;
        }

        
        public decimal GetAverageSalary() => GetAverageSalary(_vacancyService.VacanciesWithSalary);
        

        private decimal GetAverageSalary(List<HHVacancyModel> vacancies)
        {
            decimal avgFrom = vacancies.Sum(x => x.salary.from.Value) / vacancies.Count;
            decimal avgTo = vacancies.Sum(x => x.salary.to.Value) / vacancies.Count;

            return (avgFrom + avgTo) / 2;
        }

        public Dictionary<int, decimal> GetRegionsSalaries()
        {
            Dictionary<int, decimal> avgSalaryByReg = new Dictionary<int, decimal>();
            foreach(var regionVacancies in _vacancyService.VacanciesByRegionWithSalary)
            {
                avgSalaryByReg.Add(regionVacancies.Key, GetRegionSalary(regionVacancies.Key));
            }
            return avgSalaryByReg;
        }

        public decimal GetRegionSalary(int regionID) => GetAverageSalary(_vacancyService.VacanciesByRegionWithSalary[regionID]);
    }
}
