using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBPlatform_v1._0.Models
{
    public class ExportList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string ContactsIds { get; set; }
        public string IndustriesIds { get;set; }
        public string CountriesIds { get; set; }
        public string JobAreaIds{get; set; }
        public string JobLevelIds { get; set; }

        public ExportList()
        {
            Date = DateTime.UtcNow;
        }

    }
}