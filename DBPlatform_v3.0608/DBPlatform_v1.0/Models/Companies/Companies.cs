using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBPlatform_v1._0.Models
{
    public class Company
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "The Name can not be empty")]
        public string name { get; set; }
        public int employees_min { get; set; }
        public int employees_max { get; set; }
        public string employees_prooflink { get; set; }
        public int revenue_min { get; set; }
        public int revenue_max { get; set; }
        public string revenue_prooflink { get; set; }
        public string domain { get; set; }
        public string website { get; set; }
        public DateTime DateCreated{get; set; }
        public DateTime DateModified { get; set; }
        public int? parent_id { get; set; }
        public int? PrimaryIndustryId { get; set; }
        public Industry PrimaryIndustry { get; set; }

        public string Employees()
        {
            return getMinMax(this.employees_min, this.employees_max);

        }

        public ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Industry> Industries { get; set; }
        public Company()
        {
            DateCreated = DateModified = DateTime.UtcNow;
            Industries = new List<Industry>();
            Contacts = new List<Contact>();
        }
        public void getCompanyChanges(Company newCompany)
        {

            this.name = newCompany.name;
            this.PrimaryIndustryId = newCompany.PrimaryIndustryId;
            this.employees_min = newCompany.employees_min;
            this.employees_max = newCompany.employees_max;
            this.employees_prooflink = newCompany.employees_prooflink;
            this.revenue_min = newCompany.revenue_min;
            this.revenue_max = newCompany.revenue_max;
            this.revenue_prooflink = newCompany.revenue_prooflink;
            this.domain = newCompany.domain;
            this.website = newCompany.website;
            this.parent_id = newCompany.parent_id;

        }
        public bool IfMinMaxFitsCriteria(int? min, int? max)
        {
            if (max == 0)
            {
                if (min == 0) return true;
                if (min <= this.employees_min) return true;
            }
            else
            {
                if (employees_min >=min && employees_max <=max ) return true;
            }
            return false;

        }
        public void SetDomain(string email)
        {
            int indexAt = email.IndexOf('@');
            if (indexAt != -1)
            {
                domain = email.Substring(indexAt + 1);
            }
        }
        public void SetMinMaxEmployees(string minMaxstr)
        {
            var minMax = RetrieveMinMax(minMaxstr);
            employees_min = minMax[0];
            employees_max = minMax[1];
        }

        public void SetMinMaxRevenue(string minMaxstr)
        {
            var minMax = RetrieveMinMax(minMaxstr);
            revenue_min = minMax[0];
            revenue_max = minMax[1];
        }

        private static int[] RetrieveMinMax(string minMax)
        {
            int[] values = new int[2];
            try
            {
                if (!String.IsNullOrEmpty(minMax))
                {
                    var splitedValues = minMax.Split('-');
                    if (splitedValues.Length == 1)
                    {
                        var indexOfPlus = splitedValues[0].IndexOf('+');
                        if (indexOfPlus != -1)
                        {
                            var val = Convert.ToInt32(splitedValues[0].Substring(0, indexOfPlus));
                            values[0] = val;
                            values[1] = 0;
                        }
                        else
                        {
                            values[0] = Convert.ToInt32(splitedValues[0]);
                            values[1] = Convert.ToInt32(splitedValues[0]);
                        }
                    }
                    else
                    {
                        int min = Convert.ToInt32(splitedValues[0]);
                        int max = Convert.ToInt32(splitedValues[1]);

                        if (max < min && max != 0)
                        {
                            max = min;
                        }

                        values[0] = min;
                        values[1] = max;
                    }

                    return values;
                }


            }
            catch (Exception)
            {
                // ignored
            }
            values[0] = 0;
            values[1] = 0;

            return values;
        }
        public string GetEmployees()
        {
            return getMinMax(this.employees_min, this.employees_max);
        }
        public string GetRevenue()
        {
            return getMinMax(this.revenue_min, this.revenue_max);
        }
        private string getMinMax(int min, int max)
        {
            if (min == 0 && max == 0) return "0";
            else if (max == 0)
            {
                return min + "+";
            }
            else if (min == max) return min.ToString();
            else return min.ToString() + '-' + max;

        }
        public bool isMinMaxGood()
        {
            if (employees_max < employees_min && employees_max != 0 || revenue_max < revenue_min && revenue_max != 0) return false;
            return true;
        }
    }

    public enum SearchType { Exact, ByParts, All }
    public class CompaniesSearchView
    {
        public string searchType { get; set; }
        public IEnumerable<Company> companies { get; set; }
        public List<SelectListItem> companiesList { get; set; }
        public IEnumerable<string> SelectedCompanies { get; set; }
        [Display(Name = "Company name")]
        public string searchName { get; set; }

        public CompaniesSearchView()
        {

            companies = new List<Company>();
        }

    }
    public class CompanyEditView
    {

        public Company company { get; set; }
        public int[] selectedIndustries { get; set; }
        public SelectList selectListIndustries { get; set; }
        public IEnumerable<Industry> industries { get; set; }
        public SelectList selectListCompanies { get; set; }

        public CompanyEditView(Company company, IEnumerable<Industry> DBIndustries, IEnumerable<Company> DBCompanies)
        {

            this.company = company;
            this.selectListIndustries = new SelectList(DBIndustries, "Id", "Name");
            this.selectListCompanies = new SelectList(DBCompanies.Where(comp => comp.name != company.name), "Id", "name", company.parent_id);
            this.industries = DBIndustries;
        }
        public void getMissingData(IEnumerable<Industry> DBIndustries, IEnumerable<Company> DBCompanies)
        {
            this.selectListIndustries = new SelectList(DBIndustries, "Id", "Name");
            this.selectListCompanies = new SelectList(DBCompanies, "Id", "name");
            this.industries = DBIndustries;
        }
        public CompanyEditView()
        {
            industries = new List<Industry>();
        }
    }
}