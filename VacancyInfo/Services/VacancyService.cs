using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Task<List<HHVacancyModel>> GetVacanciesInDetail();
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
        public async Task<List<HHVacancyModel>> GetVacanciesInDetail()
        {
            if (!_vacanciesInDetail.Any())
                foreach (var vacancy in _vacancies)
                {
                     _vacanciesInDetail.Add(await GetVacancy(int.Parse(vacancy.id)));
                }

            return _vacanciesInDetail;
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
                Areas.ForEach(x => _vacanciesByRegionWithSalary.Add(int.Parse(x.id), new List<HHVacancyModel>()));
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

        public VacancyService(IRequestServices requestServices)
        {
            _requestServices = requestServices;
            _vacancies = new List<HHVacancyModel>();
            _vacanciesInDetail = new List<HHVacancyModel>();
        }

        public async Task<List<HHVacancyModel>> GetVacancies(string vacancyName, string region, int page = 0)
        {
            string requestBody = _hhVacancyRequest + "?text=" + vacancyName;
            if(region != "")
            {
                requestBody += "&&" + region;
            }
            requestBody += "&per_page=" + vacanciesPerPage;

            var responce = await _requestServices.SendRequest(requestBody);

            if (!_requestServices.GetPullRequestsError)
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

        private List<HHVacancyModel> _vacanciesWithSalary;
        public List<HHVacancyModel> VacanciesWithSalary
        {
            get
            {
                if(_vacanciesWithSalary == null) 
                    _vacanciesWithSalary = _vacancies.Where(x => x.salary?.from.HasValue == true && x.salary?.to.HasValue == true).ToList();                   
                return _vacanciesWithSalary;
            }        
        }

        private async Task<HHVacancyModel> GetVacancy(int id)
        {
            string requestBody = _hhVacancyRequest+"/"+id;
            var responce = await _requestServices.SendRequest(requestBody);
            if (!_requestServices.GetPullRequestsError)
                return await HHVacancyModel.ConvertFromStreamAsync(responce);
            return HHVacancyModel.NullObject();
        }
    }
}
