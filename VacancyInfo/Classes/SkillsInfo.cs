﻿using System;
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
                    if (!skills.Any(x => x.name == skill.name))
                        skills.Add(skill);
                }
            }
            return skills;
        }

        static public List<KeySkillStats> GetKeySkillsWithStats(List<HHVacancyModel> vacancies) // если будет долго делаться, то можно сделать через хеш таблицу
        {
            List<KeySkillStats> skillWithStats = new List<KeySkillStats>();
            Dictionary<Key_Skills, List<HHVacancyModel>> vacanciesWithSkill = new Dictionary<Key_Skills, List<HHVacancyModel>>();
            foreach (HHVacancyModel vac in vacancies)
            {
                if (vac.key_skills == null)
                    continue;
                foreach (Key_Skills skill in vac.key_skills)
                {
                    if (!vacanciesWithSkill.Keys.Contains(skill))
                        vacanciesWithSkill.Add(skill, new List<HHVacancyModel>() { vac });
                    else
                        vacanciesWithSkill[skill].Add(vac);
                }
            }
            return skillWithStats;
        }
    }
}
