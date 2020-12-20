using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacancyInfo.Models;
using VacancyInfo.Models.HHModels;
using VacancyInfo.Services;

namespace VacancyInfo.Classes
{
    static public class SalaryInfo
    {
        static public decimal GetAverageSalary(List<HHVacancyModel> vacancies)
        {
            // TODO: добавить получение стоимости доллара и евро
            decimal avgFrom = vacancies.Sum(x => x.salary.from.Value) / vacancies.Count; // TODO: добавить тут тесты
            decimal avgTo = vacancies.Sum(x => x.salary.to.Value) / vacancies.Count;

            return (avgFrom + avgTo) / 2;
        }

        static public Dictionary<int, decimal> GetRegionsSalaries(Dictionary<int, List<HHVacancyModel>> vacanciesByRegionWithSalary)
        {
            Dictionary<int, decimal> avgSalaryByReg = new Dictionary<int, decimal>();// TODO: и мб тут
            foreach (var regionVacancies in vacanciesByRegionWithSalary)
            {
                avgSalaryByReg.Add(regionVacancies.Key, GetAverageSalary(regionVacancies.Value));
            }
            return avgSalaryByReg;
        }
    }
}