using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace VacancyInfo.Models
{
    public class Items
    {
        public List<HHVacancyModel> items { get; set; } // HHVacancyModel[]
        public int? found { get; set; }
        public int? pages { get; set; }
        public int? per_page { get; set; }
        public int? page { get; set; }
        public object clusters { get; set; }
        public object arguments { get; set; }
        public string alternate_url { get; set; }

        public static async Task<Items> ConvertFromStreamAsync(Stream stream)
        {
            return await JsonSerializer.DeserializeAsync
                <Items>(stream);
        }
    }

}
