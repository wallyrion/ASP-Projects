using System.Collections.Generic;

namespace Eshop.Models
{
    public class PropertyView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SpecificationView> Specifications {get; set; }

        public PropertyView()
        {
            Specifications = new List<SpecificationView>();
        }
    }
}