using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBPlatform_v1._0.Models;
using System.Data.Entity;
using System.Text;
using DBPlatform_v1._0.Models.Companies;

namespace DBPlatform_v1._0.Helpers
{
    public static class PlatformHelper
    {
        public static bool FindExactByParts(string inputStr, string searchStr)
        {
            inputStr = inputStr.ToLower();
            searchStr = searchStr.ToLower();
            string[] parts = inputStr.Split(' ', ',');
            foreach (var part in parts)
            {
                if (part.Length < 2) continue;
                if (!searchStr.Contains(part)) return false;
            }

            return true;
        }
        public static bool FindByParts(string inputStr, string searchStr)
        {
            inputStr = inputStr.ToLower();
            searchStr = searchStr.ToLower();
            string[] parts = inputStr.Split(' ', ',');
            foreach (var part in parts)
            {
                if (part.Length < 2) continue;
                if (searchStr.Contains(part)) return true;
            }

            return false;
        }

        public static void MergeCompanies(DBPlatform context, string primaryMerge, IEnumerable<int> mergedIds)
        {
            if (String.IsNullOrEmpty(primaryMerge) || !mergedIds.Any()) return;
            var primaryCompany = context.Companies.FirstOrDefault(comp => comp.name == primaryMerge);
            if (primaryCompany == null)
            {
                primaryCompany = new Company { name = primaryMerge };
                context.Companies.Add(primaryCompany);
                context.Entry(primaryCompany).State = EntityState.Added;
            }
            var resCompanies = context.Companies.Include(co => co.Contacts)
                .Where(comp => mergedIds.Contains(comp.Id)).ToList();
            foreach (var comp in resCompanies)
            {
                if (comp.Contacts.Count > 0)
                {
                    foreach (var cont in comp.Contacts.ToList())
                    {
                        cont.Company = primaryCompany;
                        context.Entry(cont).State = EntityState.Modified;
                    }

                    context.Companies.Remove(comp);

                }
            }

            context.SaveChanges();
        }

        public static List<Contact> GetContactsByIds(DBPlatform context, string[] contactsIds)
        {

            List<int> contactIdsInt = new List<int>();
            foreach (var id in contactsIds)
            {
                try
                {
                    contactIdsInt.Add(int.Parse(id));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            var contacts = context.Contacts.Include(c => c.Company).Include(c => c.Country)
                .Include(c => c.Company.Industries).Include(c => c.Title.JobLevel).Include(c => c.Company.PrimaryIndustry)
                .Where(cont => contactIdsInt.Contains(cont.Id)).ToList();
            return contacts;
        }
        public static string GenerateCSVString(List<Contact> contacts)
        {
            StringBuilder sb = new StringBuilder();
            AppendWithComma(ref sb, "first_name", "last_name", "email", "company", "title", "country", "prooflink", "industry",
                "employees", "employees_prooflink", "revenue", "revenue_prooflink");

            foreach (var cont in contacts)
            {
                try
                {
                    AppendWithComma(ref sb,
                        cont.first_name, cont.last_name, cont.email, cont.Company.name, cont.Title!=null?cont.Title.Name:"",
                        cont.Country!=null?cont.Country.Name:"", cont.prooflink, cont.Company.PrimaryIndustry==null? "" : cont.Company.PrimaryIndustry.Name,
                        cont.Company.GetEmployees(), cont.Company.employees_prooflink ?? "", cont.Company.GetRevenue(),
                        cont.Company.revenue_prooflink ?? ""
                    );
                }
                catch (Exception e)
                {

                }
            }

            return sb.ToString();
        }

        private static void AppendWithComma(ref StringBuilder sb, params string[] values)
        {
            for (int i = 0; i < values.Length - 1; i++)
            {
                string str = values[i];
                if (str != "" && str.Contains(','))
                    str = str.Replace(',', ' ');

                sb.Append(str);
                sb.Append(",");
            }

            sb.Append(values[values.Length - 1]);
            sb.AppendLine();
        }

        public static JobArea FindJobArea(string jobAreaName)
        {
            using (var context = new DBPlatform())
            {
                return context.JobAreas.FirstOrDefault(ja => ja.Name == jobAreaName);
            }
        }

    }
}