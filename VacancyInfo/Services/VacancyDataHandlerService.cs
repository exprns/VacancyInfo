using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacancyInfo.Models;
using VacancyInfo.Models.HHModels;

namespace VacancyInfo.Services
{
    public interface IVacancyDataHandlerService
    {
        public List<Area> GetAreas(List<HHVacancyModel> vacancies);
        public List<HHVacancyModel> GetRegionVacancies(List<HHVacancyModel> vacs, int regId);
        public List<HHVacancyModel> GetVacanciesWithSalary(List<HHVacancyModel> vacancies);

    }

    public class VacancyDataHandlerService: IVacancyDataHandlerService
    {
        public List<Area> GetAreas(List<HHVacancyModel> vacancies)
        {
            return vacancies.Select(x => x.area).GroupBy(p => int.Parse(p.id))
              .Select(g => g.First())
              .ToList();
        }

        public List<HHVacancyModel> GetRegionVacancies(List<HHVacancyModel> vacs,int regId)
        {
            return vacs.Where(x=> int.Parse(x.area.id) == regId).ToList();
        }

        public List<HHVacancyModel> GetVacanciesWithSalary(List<HHVacancyModel> vacancies)
        {
            return vacancies.Where(x => x.salary?.from.HasValue == true && x.salary?.to.HasValue == true).ToList();
        }
    }
}
