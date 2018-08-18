using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBPlatform_v1._0.Models.Companies
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Country()
        {
            
        }
        public Country(string name)
        {
            Name = name;
        }
    }
    
}