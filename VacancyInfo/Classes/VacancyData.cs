using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacancyInfo.Models;
using VacancyInfo.Services;

namespace VacancyInfo.Classes
{
    public class VacancyData
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

        public decimal GetAverageSalary()
        {
            var vacanciesWithSalary = _vacancyService.VacanciesWithSalary;
            decimal avgFrom = vacanciesWithSalary.Sum(x => x.salary.from.Value) / vacanciesWithSalary.Count;
            decimal avgTo = vacanciesWithSalary.Sum(x => x.salary.to.Value) / vacanciesWithSalary.Count;

            return (avgFrom + avgTo) / 2;
        }
                 
    }
}
