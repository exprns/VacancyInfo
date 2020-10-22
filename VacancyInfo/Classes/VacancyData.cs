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
        Task<List<HHVacancyModel>> GetVacanciesInDetail();
        decimal GetAverageSalary();
        Dictionary<int, decimal> GetRegionsSalaries();
        decimal GetRegionAverageSalary(int areaId);
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

        public async Task<List<HHVacancyModel>> GetVacanciesInDetail() => await _vacancyService.GetVacanciesInDetail(); // TODO: думаю что стоит убрать отсюда все ф-и, которые сквозно вызывают функции из ваканси сервиса

        public List<Area> GetAreas() => _vacancyService.Areas;
        public Dictionary<int, List<HHVacancyModel>> GetVacanciesByRegionWithSalary() => _vacancyService.VacanciesByRegionWithSalary;
        public Dictionary<int, List<HHVacancyModel>> GetVacanciesByRegion() => _vacancyService.VacanciesByRegion;
        public decimal GetAverageSalary() => GetAverageSalary(_vacancyService.VacanciesWithSalary);
        public decimal GetRegionAverageSalary(int areaId) => GetAverageSalary(_vacancyService.VacanciesByRegionWithSalary[areaId]);
        public List<HHVacancyModel> GetRegionVacancies(int areaId) => _vacancyService.VacanciesByRegion[areaId];


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
                avgSalaryByReg.Add(regionVacancies.Key, GetRegionAverageSalary(regionVacancies.Key));
            }
            return avgSalaryByReg;
        }
    }
}