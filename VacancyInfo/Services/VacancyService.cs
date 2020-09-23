using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace VacancyInfo.Services
{
    public class VacancyService : IVacancyService
    {
        private IRequestServices _requestServices;
        private string _hhName = "https://api.hh.ru/vacancies";
        private int vacanciesPerPage = 100;
        private List<HHVacancyModel> _vacancies;

        public VacancyService(IRequestServices requestServices)
        {
            _requestServices = requestServices;
            _vacancies = new List<HHVacancyModel>();
        }

        public async Task<List<HHVacancyModel>> GetVacancies(string vacancyName, int page = 0)
        {
            string requestBody = _hhName + "?text=" + vacancyName;
            requestBody += "&per_page=" + vacanciesPerPage;
            var request = new HttpRequestMessage(HttpMethod.Get, requestBody);
            request.Headers.Add("Accept", "application/vnd.github.v3+json"); //  нужно ли это вообще???????????
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var responce = await _requestServices.SendRequest(request);

            if (!_requestServices.GetPullRequestsError)
            {
                try
                {
                    var items = await HHVacancyModel.ConvertFromStreamAsync(responce);
                    if (items.pages != 0 && page < items.pages)
                    {
                        _vacancies.AddRange(items.items);
                        await GetVacancies(vacancyName, page + 1);
                    }
                }
                catch (Exception ex)
                {

                }
                return _vacancies;
            }
            else
            {
                return new List<HHVacancyModel>();
            }
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

        public decimal GetAverageSalary()
        {
            var vacanciesWithSalary = VacanciesWithSalary;
            decimal avgFrom = vacanciesWithSalary.Sum(x => x.salary.from.Value) / vacanciesWithSalary.Count;
            decimal avgTo = vacanciesWithSalary.Sum(x => x.salary.to.Value) / vacanciesWithSalary.Count;

            return (avgFrom + avgTo) / 2;
        }

    }

    public interface IVacancyService
    {
        public Task<List<HHVacancyModel>> GetVacancies(string vacancyName, int page = 0);
        public decimal GetAverageSalary();
        public List<HHVacancyModel> VacanciesWithSalary { get; }
    }
}
