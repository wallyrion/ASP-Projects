using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBPlatform_v1._0.Models.Companies;

namespace DBPlatform_v1._0.Models
{
    public class GenerateList
    {
        public string ListName { get; set; }
        public IEnumerable<Industry> Industries { get; set; }
        public IEnumerable<JobLevel> JobLevels { get; set; }
        public IEnumerable<JobArea> JobAreas { get; set; }
        public IEnumerable<Country> Countries { get; set; }

    }
}