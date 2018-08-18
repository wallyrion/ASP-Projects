using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBPlatform_v1._0.Models.ViewHelpers
{
    public class CompanyViewHelper
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ContactCount { get;set; }
        public string Industry{get; set; }
        public string Employees { get; set; }
        public string EmployeesProoflink { get; set; }
        public string Revenue { get; set; }
        public string RevenueProoflink { get; set; }
        public string Domain { get; set; }
        public string DateCreated { get; set; }

        public CompanyViewHelper(Company comp)
        {
            Id = comp.Id;
            Name = comp.name;
            ContactCount = comp.Contacts?.Count ?? 0;
            Industry = comp.PrimaryIndustry != null ? comp.PrimaryIndustry.Name : "";
            Employees = comp.GetEmployees();
            EmployeesProoflink = comp.employees_prooflink;
            Revenue = comp.GetRevenue();
            RevenueProoflink = comp.revenue_prooflink;
            Domain = comp.domain;
            DateCreated = comp.DateCreated.ToShortDateString();
        }
    }
}