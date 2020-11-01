using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacancyInfo.Models.HHModels;

namespace VacancyInfo.Models
{
    public class KeySkillStats
    {
        public Key_Skills KeySkill { get; set; }
        public decimal Price { get; set; }
        public decimal FrequencyInPercent { get; set; }
    }
}
