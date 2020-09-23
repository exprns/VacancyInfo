using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacancyInfo.Models.HHModels
{
    public class Employer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string alternate_url { get; set; }
        public object logo_urls { get; set; }
        public string vacancies_url { get; set; }
        public bool? trusted { get; set; }


    }
}
