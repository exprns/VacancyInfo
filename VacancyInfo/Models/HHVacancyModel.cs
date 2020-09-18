using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace VacancyInfo
{
    public class HHVacancyModel
    {
 string id { get; set; }// Идентификатор вакансии
string description { get; set; } // Описание вакансии, содержит html
        string branded_description { get; set; }//  	Брендированное описание вакансии
        string[] key_skills { get; set; }//Информация о ключевых навыках, заявленных в вакансии.Список может быть пустым.
        // string key_skills[].name   { get; set; }// название ключевого навыка
    object schedule { get; set; }// График работы.Элемент справочника schedule
   // schedule.id { get; set; }//  Идентификатор графика работы
                                    // schedule.name    Название графика работы
    bool accept_handicapped { get; set; }// Указание, что вакансия доступна для соискателей с инвалидностью
    bool accept_kids { get; set; }//Указание, что вакансия доступна для соискателей от 14 лет
        object experience   //Требуемый опыт работы. Элемент справочника experience
// experience.id  Идентификатор требуемого опыта работы
// experience.nameНазвание требуемого опыта работы
object address { get; set; }//Адрес вакансии
        string alternate_url { get; set; }//Ссылка на представление вакансии на сайте
        string apply_alternate_url { get; set; }// Ссылка на отклик на вакансию на сайте
        string code { get; set; }//	Внутренний код вакансии работадателя
        object department { get; set; } //Департамент, от имени которого размещается вакансия (если данная возможность доступна для компании). Работодатели могут запросить справочник департаментов.
                                        // department.idИдентификатор департамента
                                        // department.nameНазвание департамента
        object employment { get; set; } //Тип занятости. Элемент справочника employment.
                                        // employment.id  Идентификатор типа занятости
                                        // employment.name  Название типа занятости
        object salary { get; set; } // 	Оклад
                                    // salary.from 	Нижняя граница вилки оклада
                                    // salary.to 	Верняя граница вилки оклада
                                    // salary.gross 	Признак того что оклад указан до вычета налогов. В случае если не указано - null.
                                    // salary.currencyИдентификатор валюты оклада (справочник currency).
        bool archived { get; set; }// Находится ли данная вакансия в архиве
        string name { get; set; } // Название вакансии
        object insider_interview { get; set; }  //	Интервью о жизни в компании
        object area { get; set; } //  Регион размещения вакансии
                                  // area.idИдентификатор региона
                                  // area.name  Название региона
                                  // area.url   Url получения информации о регионе
        string created_at { get; set; }//Дата и время создания вакансии
        string published_at { get; set; } // Дата и время публикации вакансии
        object employer { get; set; } // Короткое представление работодателя. Описание полей смотрите в информации о работодателе.
                                      // employer.blacklistedДобавлены ли все вакансии работодателя в список скрытых
        bool response_letter_required { get; set; } // Обязательно ли заполнять сообщение при отклике на вакансию
        object type { get; set; }  // Тип вакансии.Элемент справочника vacancy_type.
                                   // type.id Идентификатор типа вакансии
                                   // type.name  Название типа вакансии
        bool has_test { get; set; }// Информация о наличии прикрепленного тестового задании к вакансии. В случае присутствия теста - true.
        string response_url { get; set; }// 	На вакансии с типом direct нельзя откликнуться на сайте hh.ru, у этих вакансий в ключе response_url выдаётся URL внешнего сайта (чаще всего это сайт работодателя с формой отклика).
        object test { get; set; }// 	Информация о прикрепленном тестовом задании к вакансии.В случае отсутствия теста — null. В данный момент отклик на вакансии с обязательным тестом через API невозможен.
                                 //test.required  Обязательно ли заполнение теста для отклика
        string[] specialization // Специализации. Элементы справочника specializations
// specializations[].id   Идентификатор специализации
// specializations[].name Название специализации
// specializations[].profarea_id  Идентификатор профессиональной области, в которую входит специализация
// specializations[].profarea_nameНазвание профессиональной области, в которую входит специализация
object contacts { get; set; }//   	Контактная информация
        object billing_type { get; set; }//   Биллинговый тип вакансии. Элемент справочника vacancy_billing_type.
                                         // billing_type.idИдентификатор биллингового типа вакансии
                                         // billing_type.name    Название биллингового типа вакансии
        bool allow_messages { get; set; } // Включена ли возможность соискателю писать сообщения работодателю, после приглашения/отклика на вакансию
        bool premium { get; set; } // Является ли данная вакансия премиум-вакансией
        string[]  driver_license_types { get; set; } // Список требуемых категорий водительских прав. Список может быть пустым.
        // driver_license_types[].id // Категория водительских прав. Элемент справочника driver_license_types
bool accept_incomplete_resumes { get; set; }// Разрешен ли отклик на вакансию неполным резюме
        object working_days { get; set; } // 	Рабочие дни. Элемент справочника working_days
                                          // working_days.idИдентификатор рабочих дней
                                          // working_days.name  Название рабочих дней
                                          // working_time_intervals    	Временные интервалы работы.Элемент справочника working_time_intervals
                                          // working_time_intervals.id  Идентификатор временного интервала работы
                                          // working_time_intervals.nameНазвание временного интервала работы
        object working_time_modes { get; set; } //	Режимы времени работы.Элемент справочника working_time_modes
                                                //  working_time_modes.id  Идентификатор режима времени работы
       // string working_time_modes.name // Название режима времени работы
bool accept_temporary { get; set; }

        public static async Task<List<HHVacancyModel>> ConvertFromStreamAsync(Stream stream)
        {
            return await JsonSerializer.DeserializeAsync
                <List<HHVacancyModel>>(stream);
        }
    }

}
