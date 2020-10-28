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
        decimal GetAverageSalary(List<HHVacancyModel> vacancies);
        Dictionary<int, decimal> GetRegionsSalaries(Dictionary<int, List<HHVacancyModel>> vacanciesByRegionWithSalary);
    }

    public class VacancyData : IVacancyData
    {

        public decimal GetAverageSalary(List<HHVacancyModel> vacancies)
        {
            decimal avgFrom = vacancies.Sum(x => x.salary.from.Value) / vacancies.Count;
            decimal avgTo = vacancies.Sum(x => x.salary.to.Value) / vacancies.Count;

            return (avgFrom + avgTo) / 2;
        }

        public Dictionary<int, decimal> GetRegionsSalaries(Dictionary<int, List<HHVacancyModel>> vacanciesByRegionWithSalary)
        {
            Dictionary<int, decimal> avgSalaryByReg = new Dictionary<int, decimal>();
            foreach(var regionVacancies in vacanciesByRegionWithSalary)
            {
                avgSalaryByReg.Add(regionVacancies.Key, GetAverageSalary(regionVacancies.Value));
            }
            return avgSalaryByReg;
        }
    }
}