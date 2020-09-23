using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacancyInfo.Models
{
    public class Specialization
    {
        public string id { get; set; }
        public string name { get; set; }
        public int? profarea_id { get; set; }
        public string profarea_name { get; set; }
    }
}
