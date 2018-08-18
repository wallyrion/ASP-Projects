using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBPlatform_v1._0.Models;
using DBPlatform_v1._0.Models.Companies;

namespace DBPlatform_v1._0.Helpers
{
    public static class GSheetHelper
    {
        public static List<ContactHelper> GetContactInfoFromGSheet(string gSheetUrl)
        {
            var s = Helpers.GoogleSheetData.getDataFromSheetlink(gSheetUrl);
            if (!s.Any())
            {
                return null;
            }
            var values = s.ToList();
            var firstRow = values[0].ToList();

            int firstNameColumn = firstRow.IndexOf("first_name"),
                lastNameColumn = firstRow.IndexOf("last_name"),
                companyColumn = firstRow.IndexOf("company"),
                titleColumn = firstRow.IndexOf("title"),
                countryColumn = firstRow.IndexOf("country"),
                emailColumn = firstRow.IndexOf("email"),
                employeesColumn = firstRow.IndexOf("employees"),
                employeesProoflinkColumn = firstRow.IndexOf("employees_prooflink"),
                revenueColumn = firstRow.IndexOf("revenue"),
                revenueProoflinkColumn = firstRow.IndexOf("revenue_prooflink"),
                prooflinkColumn = firstRow.IndexOf("prooflink"),
                industryColumn = firstRow.IndexOf("industry");


            if (emailColumn == -1 || firstNameColumn == -1 || lastNameColumn == -1 ||
                companyColumn == -1 || titleColumn == -1 || countryColumn == -1 || prooflinkColumn == -1 ||
                employeesColumn == -1 || employeesProoflinkColumn == -1 ||
                revenueColumn == -1 || revenueProoflinkColumn == -1 || industryColumn == -1)
            {
                return null;
            }

            var contacts = new List<ContactHelper>();
            values.RemoveAt(0);
            foreach (var row in values)
            {
                var curRow = row.ToList();
                if (String.IsNullOrEmpty(curRow[emailColumn].ToString()))
                    continue;


                var contact = new ContactHelper();

                try
                {
                    contact.FirstName = curRow[firstNameColumn].ToString();
                    contact.LastName = curRow[lastNameColumn].ToString();
                    contact.Email = curRow[emailColumn].ToString();
                    contact.Company = curRow[companyColumn].ToString();
                    contact.Title = curRow[titleColumn].ToString();
                    contact.Country = curRow[countryColumn].ToString();
                    contact.Prooflink = curRow[prooflinkColumn].ToString();
                    contact.Employees = curRow[employeesColumn].ToString();
                    contact.EmpployeesProoflink = curRow[employeesProoflinkColumn].ToString();
                    contact.Revenue = curRow[revenueColumn].ToString();
                    contact.RevenueProoflink = curRow[revenueProoflinkColumn].ToString();
                    contact.Industry = curRow[industryColumn].ToString();
                }
                catch (Exception)
                {
                    //
                }

                contacts.Add(contact);

            }

            return contacts;
        }


        public class ContactHelper
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Company { get; set; }
            public string Title { get; set; }
            public string Country { get; set; }
            public string Employees { get; set; }
            public string EmpployeesProoflink { get; set; }
            public string Revenue { get; set; }
            public string RevenueProoflink { get; set; }
            public string Prooflink { get; set; }
            public string Industry { get; set; }
        }
    }
}