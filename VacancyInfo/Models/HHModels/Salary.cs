using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacancyInfo.Models.HHModels
{
    public class Salary
    {
        public int? from { get; set; }
        public int? to { get; set; }
        public bool? gross { get; set; }
        public string currency { get; set; }
    }
}
