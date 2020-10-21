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
        public List<HHVacancyModel> VacanciesInDetail { get; }
        public Dictionary<Area, List<HHVacancyModel>> VacanciesByRegionWithSalary { get; }
        public List<Area> Areas { get; }
    }

    public class VacancyService : IVacancyService // TODO: подумать надо ли перенести это в Classes
    {
        private IRequestServices _requestServices;
        private string _hhVacancyRequest = "https://api.hh.ru/vacancies";
        private int vacanciesPerPage = 100;
        private List<HHVacancyModel> _vacancies;
        private List<HHVacancyModel> _vacanciesInDetail;
        public List<HHVacancyModel> VacanciesInDetail
        {
            get
            {
                if (!_vacanciesInDetail.Any())
                    foreach (var vacancy in _vacancies)
                    {
                        _vacanciesInDetail.Add(GetVacancy(int.Parse(vacancy.id)).GetAwaiter().GetResult());
                    }

                return _vacanciesInDetail;
            }
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


        private Dictionary<Area, List<HHVacancyModel>> _vacanciesByRegionWithSalary;
        public Dictionary<Area, List<HHVacancyModel>> VacanciesByRegionWithSalary
        {
            get
            {
                if (_vacanciesByRegionWithSalary != null && _vacanciesByRegionWithSalary.Any())
                    return _vacanciesByRegionWithSalary;

                _vacanciesByRegionWithSalary = new Dictionary<Area, List<HHVacancyModel>>();
                Areas.ForEach(x => _vacanciesByRegionWithSalary.Add(x, new List<HHVacancyModel>()));
                foreach (var vac in VacanciesWithSalary)
                {
                    _vacanciesByRegionWithSalary[vac.area].Add(vac);
                }

                return _vacanciesByRegionWithSalary;
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
