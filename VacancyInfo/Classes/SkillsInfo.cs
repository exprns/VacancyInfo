using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacancyInfo.Models;
using VacancyInfo.Models.HHModels;

namespace VacancyInfo.Classes
{
    interface ISkillsInfo
    {
        public List<Key_Skills> GetKeySkills(List<HHVacancyModel> vacancies);
        public List<KeySkillStats> GetKeySkillsWithStats(List<HHVacancyModel> vacancies);

    }

    static public class SkillsInfo // TODO: придумать как заменить info на что-то попонятнее
    {
        static public List<Key_Skills> GetKeySkills(List<HHVacancyModel> vacancies) // если будет долго делаться, то можно сделать через хеш таблицу
        {
            List<Key_Skills> skills = new List<Key_Skills>();
            foreach(HHVacancyModel vac in vacancies)
            {
                if (vac.key_skills == null)
                    continue;
                foreach(Key_Skills skill in vac.key_skills)
                {
                    if (!skills.Any(x => x.name.ToLower() == skill.name.ToLower()))
                        skills.Add(skill);
                }
            }
            return skills;
        }

        static public List<KeySkillStats> GetKeySkillsWithStats(List<HHVacancyModel> vacancies) // если будет долго делаться, то можно сделать через хеш таблицу
        {
            Dictionary<Key_Skills, List<HHVacancyModel>> vacanciesWithSkill = new Dictionary<Key_Skills, List<HHVacancyModel>>();
            foreach (HHVacancyModel vac in vacancies)
            {
                if (vac.key_skills == null)
                    continue;
                foreach (Key_Skills skill in vac.key_skills)
                {
                    var existedSkill = vacanciesWithSkill.Keys.FirstOrDefault(x => x.name.ToLower() == skill.name.ToLower());
                    if (existedSkill == null)
                        vacanciesWithSkill.Add(skill, new List<HHVacancyModel>() { vac });
                    else
                        vacanciesWithSkill[existedSkill].Add(vac);
                }
            }
            List<KeySkillStats> skillWithStats = new List<KeySkillStats>();
            foreach(var skillAndVacs in vacanciesWithSkill)
            {
                skillWithStats.Add(GetSkillWithStats(skillAndVacs.Key, skillAndVacs.Value, vacancies.Count));
            }
            return skillWithStats;
        }

        static private KeySkillStats GetSkillWithStats(Key_Skills skill, List<HHVacancyModel> vacanciesHasSkill, int allVacanciesCnt)
        {
            return new KeySkillStats() { 
                KeySkill = skill, 
                Price = SalaryInfo.GetAverageSalary(vacanciesHasSkill),
                FrequencyInPercent = decimal.Divide(vacanciesHasSkill.Count,allVacanciesCnt) * 100 // TODO: найти другой способ приведения
            };
        }

    }
}
