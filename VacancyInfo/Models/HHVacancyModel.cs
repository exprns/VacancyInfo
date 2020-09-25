using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VacancyInfo.Models;
using VacancyInfo.Models.HHModels;

namespace VacancyInfo.Models
{
    public class HHVacancyModel
    {
        public string id { get; set; }// Идентификатор вакансии
        public string description { get; set; } // Описание вакансии, содержит html
        public string branded_description { get; set; }//  	Брендированное описание вакансии
        // TODO: key_skills проверить
        public Key_Skills[] key_skills { get; set; }//Информация о ключевых навыках, заявленных в вакансии.Список может быть пустым. 
        // public string key_skills[].name   { get; set; }// название ключевого навыка
        public Schedule schedule { get; set; }// График работы.Элемент справочника schedule
        // schedule.id { get; set; }//  Идентификатор графика работы
                                    // schedule.name    Название графика работы
        public bool accept_handicapped { get; set; }// Указание, что вакансия доступна для соискателей с инвалидностью
        public bool accept_kids { get; set; }//Указание, что вакансия доступна для соискателей от 14 лет
        public Experience experience { get; set; } //Требуемый опыт работы. Элемент справочника experience
                                        // experience.id  Идентификатор требуемого опыта работы
                                        // experience.nameНазвание требуемого опыта работы
        public object address { get; set; }//Адрес вакансии
        public string alternate_url { get; set; }//Ссылка на представление вакансии на сайте
        public string apply_alternate_url { get; set; }// Ссылка на отклик на вакансию на сайте
        public string code { get; set; }//	Внутренний код вакансии работадателя
        public Departament department { get; set; } //Департамент, от имени которого размещается вакансия (если данная возможность доступна для компании). Работодатели могут запросить справочник департаментов.
                                        // department.idИдентификатор департамента
                                        // department.nameНазвание департамента
        public Employment Employment { get; set; } //Тип занятости. Элемент справочника employment.
                                        // employment.id  Идентификатор типа занятости
                                        // employment.name  Название типа занятости
        public Salary salary { get; set; } // 	Оклад
                                    // salary.from 	Нижняя граница вилки оклада
                                    // salary.to 	Верняя граница вилки оклада
                                    // salary.gross 	Признак того что оклад указан до вычета налогов. В случае если не указано - null.
                                    // salary.currency Идентификатор валюты оклада (справочник currency).
        public bool archived { get; set; }// Находится ли данная вакансия в архиве
        public string name { get; set; } // Название вакансии
        public object insider_interview { get; set; }  //	Интервью о жизни в компании
        public Area area { get; set; } //  Регион размещения вакансии
                                  // area.idИдентификатор региона
                                  // area.name  Название региона
                                  // area.url   Url получения информации о регионе
        public string created_at { get; set; }//Дата и время создания вакансии
        public string published_at { get; set; } // Дата и время публикации вакансии
        public Employer employer { get; set; } // Короткое представление работодателя. Описание полей смотрите в информации о работодателе.
                                      // employer.blacklisted Добавлены ли все вакансии работодателя в список скрытых
        public bool response_letter_required { get; set; } // Обязательно ли заполнять сообщение при отклике на вакансию
        public HHType type { get; set; }  // Тип вакансии.Элемент справочника vacancy_type.
                                   // type.id Идентификатор типа вакансии
                                   // type.name  Название типа вакансии
        public bool has_test { get; set; }// Информация о наличии прикрепленного тестового задании к вакансии. В случае присутствия теста - true.
        public string response_url { get; set; }// 	На вакансии с типом direct нельзя откликнуться на сайте hh.ru, у этих вакансий в ключе response_url выдаётся URL внешнего сайта (чаще всего это сайт работодателя с формой отклика).
        public object test { get; set; }// 	Информация о прикрепленном тестовом задании к вакансии.В случае отсутствия теста — null. В данный момент отклик на вакансии с обязательным тестом через API невозможен.
                                 //test.required  Обязательно ли заполнение теста для отклика
        public Specialization[] specialization { get; set; }// Специализации. Элементы справочника specializations
                                             // specializations[].id   Идентификатор специализации
                                             // specializations[].name Название специализации
                                             // specializations[].profarea_id  Идентификатор профессиональной области, в которую входит специализация
                                             // specializations[].profarea_name Название профессиональной области, в которую входит специализация
        public object contacts { get; set; }//   	Контактная информация
        public Billing_type billing_type { get; set; }//   Биллинговый тип вакансии. Элемент справочника vacancy_billing_type.
                                         // billing_type.id Идентификатор биллингового типа вакансии
                                         // billing_type.name    Название биллингового типа вакансии
        public bool allow_messages { get; set; } // Включена ли возможность соискателю писать сообщения работодателю, после приглашения/отклика на вакансию
        public bool premium { get; set; } // Является ли данная вакансия премиум-вакансией
        public string[]  driver_license_types { get; set; } // Список требуемых категорий водительских прав. Список может быть пустым.
        // driver_license_types[].id // Категория водительских прав. Элемент справочника driver_license_types
        public bool accept_incomplete_resumes { get; set; }// Разрешен ли отклик на вакансию неполным резюме



        public object working_days { get; set; } // 	Рабочие дни. Элемент справочника working_days
                                                       // working_days.id Идентификатор рабочих дней
                                                       // working_days.name  Название рабочих дней
        public object working_time_intervals { get; set; }//  	Временные интервалы работы.Элемент справочника working_time_intervals
                                                                          // working_time_intervals.id  Идентификатор временного интервала работы
                                                                          // working_time_intervals.name Название временного интервала работы
        public object working_time_modes { get; set; } //	Режимы времени работы.Элемент справочника working_time_modes
                                                                   //  working_time_modes.id  Идентификатор режима времени работы
                                                                   // public string working_time_modes.name // Название режима времени работы

        public bool accept_temporary { get; set; }

        public static async Task<HHVacancyModel> ConvertFromStreamAsync(Stream stream)
        {
            return await JsonSerializer.DeserializeAsync
                <HHVacancyModel>(stream);
        }

        public static HHVacancyModel NullObject() => new HHVacancyModel();

        public bool IsNull(HHVacancyModel model) => model == new HHVacancyModel();
    }

}
