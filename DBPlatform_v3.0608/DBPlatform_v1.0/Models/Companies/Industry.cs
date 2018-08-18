using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBPlatform_v1._0.Models.Alias;


namespace DBPlatform_v1._0.Models
{
    public class Industry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<IndustryAlias> Aliases { get; set; }
        public Industry()
        {
            Aliases = new List<IndustryAlias>();
            Companies = new List<Company>();
        }
        public Industry(string name)
        {
            Name = name;
            Companies = new List<Company>();
            Aliases = new List<IndustryAlias>();
        }

    }
}