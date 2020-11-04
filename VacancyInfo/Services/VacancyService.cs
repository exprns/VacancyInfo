using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VacancyInfo.Models;
using VacancyInfo.Models.HHModels;

namespace VacancyInfo.Services
{    public interface IVacancyService
    {
        public Task<List<HHVacancyModel>> GetVacancies(string vacancyName, string region, int page = 0);
        public List<HHVacancyModel> VacanciesWithSalary { get; }
        public List<HHVacancyModel> Vacancies { get; }
        public List<HHVacancyModel> VacanciesInDetail { get; }

        public Task<List<HHVacancyModel>> GetVacanciesInDetail(IEnumerable<int> vacancyIDs);
        public Dictionary<int, List<HHVacancyModel>> VacanciesByRegionWithSalary { get; }
        public Dictionary<int, List<HHVacancyModel>> VacanciesByRegion { get; }
        public List<Area> Areas { get; }
    }

    public class VacancyService : IVacancyService // TODO: подумать надо ли перенести это в Classes
    {
        private IRequestServices _requestServices;
        private string _hhVacancyRequest = "https://api.hh.ru/vacancies";
        private int vacanciesPerPage = 100;
        private List<HHVacancyModel> _vacancies;
        private List<HHVacancyModel> _vacanciesInDetail;
        public List<HHVacancyModel> Vacancies => _vacancies;
        public List<HHVacancyModel> VacanciesInDetail => _vacanciesInDetail;

        public VacancyService(IRequestServices requestServices)
        {
            _requestServices = requestServices;
            _vacancies = new List<HHVacancyModel>();
            _vacanciesInDetail = new List<HHVacancyModel>();
        }

        private List<Area> _areas;
        public List<Area> Areas
        {
            get
            {
                if (_areas != null)
                    return _areas;
                _areas = _vacancies.Select(x=>x.area).GroupBy(p => int.Parse(p.id))
                          .Select(g => g.First())
                          .ToList();
                return _areas;
            }
        }


        private Dictionary<int, List<HHVacancyModel>> _vacanciesByRegionWithSalary;
        public Dictionary<int, List<HHVacancyModel>> VacanciesByRegionWithSalary
        {
            get
            {
                if (_vacanciesByRegionWithSalary != null && _vacanciesByRegionWithSalary.Any())
                    return _vacanciesByRegionWithSalary;

                _vacanciesByRegionWithSalary = new Dictionary<int, List<HHVacancyModel>>();
                Areas.ForEach(x => _vacanciesByRegionWithSalary.Add(int.Parse(x.id), new List<HHVacancyModel>())); // TODO: по-моему тут можно упростить
                foreach (var vac in VacanciesWithSalary)
                {
                    _vacanciesByRegionWithSalary[int.Parse(vac.area.id)].Add(vac);
                }

                return _vacanciesByRegionWithSalary;
            }
        }


        private Dictionary<int, List<HHVacancyModel>> _vacanciesByRegion;
        public Dictionary<int, List<HHVacancyModel>> VacanciesByRegion
        {
            get
            {
                if (_vacanciesByRegion != null && _vacanciesByRegion.Any())
                    return _vacanciesByRegion;

                _vacanciesByRegion = new Dictionary<int, List<HHVacancyModel>>();
                Areas.ForEach(x => _vacanciesByRegion.Add(int.Parse(x.id), new List<HHVacancyModel>()));
                foreach (var vac in _vacancies)
                {
                    _vacanciesByRegion[int.Parse(vac.area.id)].Add(vac);
                }

                return _vacanciesByRegion;
            }
        }

        private List<HHVacancyModel> _vacanciesWithSalary;
        public List<HHVacancyModel> VacanciesWithSalary
        {
            get
            {
                if (_vacanciesWithSalary == null)
                    _vacanciesWithSalary = _vacancies.Where(x => x.salary?.from.HasValue == true && x.salary?.to.HasValue == true).ToList();
                return _vacanciesWithSalary;
            }
        }

        public async Task<List<HHVacancyModel>> GetVacancies(string vacancyName, string region, int page = 0)
        {
            string requestBody = _hhVacancyRequest + "?text=" + vacancyName;
            if(region != "")
            {
                requestBody += "&&" + region; // TODO: переделать vacancies?text=велосипедист2&area=28
            }
            requestBody += "&per_page=" + vacanciesPerPage;

            var responce = await _requestServices.SendRequest(requestBody);

            if (responce != Stream.Null)
            {
                var items = await Items.ConvertFromStreamAsync(responce);
                if (items.pages != 0 && page < items.pages)
                {
                    _vacancies.AddRange(items.items);
                    await GetVacancies(vacancyName, region, ++page);
                }
            }
            else
            {
                throw new Exception();
            }
            return _vacancies;
            
        }

        public async Task<List<HHVacancyModel>> GetVacanciesInDetail(IEnumerable<int> vacancyIDs)
        {
            _vacanciesInDetail = new List<HHVacancyModel>();
            foreach (var vacancyID in vacancyIDs)
            {
                _vacanciesInDetail.Add(await GetVacancy(vacancyID));
            }

            return _vacanciesInDetail;
        }

        private async Task<HHVacancyModel> GetVacancy(int id)
        {
            string requestBody = _hhVacancyRequest+"/"+id;
            var responce = await _requestServices.SendRequest(requestBody);
            if (responce != Stream.Null)
                return await HHVacancyModel.ConvertFromStreamAsync(responce);
            return HHVacancyModel.NullObject();
        }
    }
}
