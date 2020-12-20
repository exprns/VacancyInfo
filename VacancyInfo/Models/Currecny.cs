using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacancyInfo.Models
{
    public class Currecny
    {
        public int Id { get; set; }
        public int NumCode { get; set; }
        public string CharCode { get; set; }
        public int Nominal { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }

    }
}
