using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eshop.Domain.Entities.Goods;
using WebGrease.Css.Extensions;

namespace Eshop.Models
{
    public class GoodEditView
    {
        public Good Good { get; set; }
        public List<int> PropertiesIds { get; set; }
        public Dictionary<string,int?> GoodSpecifications { get; set; }
        public Dictionary<string,SelectList> GoodListSpecifications { get; set; }

        public GoodEditView(Good g, IEnumerable<Property>properties)
        {
            Good = g;
            PropertiesIds = new List<int>();
            GoodListSpecifications = new Dictionary<string, SelectList>(); 
            GoodSpecifications = new Dictionary<string, int?>();
            foreach (var prop in properties)
            {
                PropertiesIds.Add(prop.Id);
                var spec = Good.Specifications.FirstOrDefault(s => s.Property.Id == prop.Id);
                if(spec==null) GoodSpecifications.Add(prop.Name,null);
                else GoodSpecifications.Add(prop.Name,spec.Id);

                GoodListSpecifications.Add(prop.Name, new SelectList(prop.Specifications, "Id", "Name", spec?.Id ?? -1));

            }

        }

        public GoodEditView()
        {
        }
    }
}