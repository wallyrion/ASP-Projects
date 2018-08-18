using DBPlatform_v1._0.Models.Companies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBPlatform_v1._0.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string prooflink { get; set; }
        public int? CompanyId { get; set; }
        public int? TitleId { get; set; }
        public int? CountryId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Company Company { get; set; }
        public Title Title { get; set; }
        public Country Country { get; set; }
        public Contact()
        {
            DateCreated = DateModified = DateTime.UtcNow;
        }
        public Contact(ContactCreateView editView)
        {
            DateCreated = DateModified = DateTime.UtcNow;
            first_name = editView.first_name;
            last_name = editView.last_name;
            email = editView.email;
            prooflink = editView.prooflink;
            CompanyId = editView.CompanyId;
            TitleId = editView.TitleId;
            CountryId = editView.CountryId;
        }
        public void getNewInfo(ContactEditView editView)
        {
            first_name = editView.first_name;
            last_name = editView.last_name;
            email = editView.email;
            prooflink = editView.prooflink;
            CompanyId = editView.CompanyId;
            TitleId = editView.TitleId;
            CountryId = editView.CountryId;
        }
    }
    public class ContactEditView
    {
        public int Id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string prooflink { get; set; }
        public int? CompanyId { get; set; }
        public int? TitleId { get; set; }
        public int? CountryId { get; set; }
        public Company Company { get; set; }
        public Title Title { get; set; }
        public Country Country { get; set; }
        public SelectList selectListCompanies { get; set; }
        public SelectList selectListTitles { get; set; }
        public SelectList selectListCountries { get; set; }
        public ContactEditView(Contact contact, IEnumerable<Company> dbCompanies, IEnumerable<Title> dbTitles, IEnumerable<Country> dbCountries)
        {
            this.first_name = contact.first_name;
            this.last_name = contact.last_name;
            this.email = contact.email;
            this.prooflink = contact.prooflink;
            this.CountryId = contact.CountryId;
            this.CompanyId = contact.CompanyId;
            this.TitleId = contact.TitleId;
            this.selectListCompanies = new SelectList(dbCompanies, "Id", "name", CompanyId);
            this.selectListTitles = new SelectList(dbTitles, "Id", "Name", TitleId);
            this.selectListCountries = new SelectList(dbCountries, "Id", "Name", CountryId);

        }
        public void updateInfo(IEnumerable<Company> dbCompanies, IEnumerable<Title> dbTitles, IEnumerable<Country> dbCountries)
        {
            this.selectListCompanies = new SelectList(dbCompanies, "Id", "name", CompanyId);
            this.selectListTitles = new SelectList(dbTitles, "Id", "Name", TitleId);
            this.selectListCountries = new SelectList(dbCountries, "Id", "Name", CountryId);

        }
        public ContactEditView()
        {

        }
    }
    public class ContactCreateView
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string prooflink { get; set; }
        public int? CompanyId { get; set; }
        public int? TitleId { get; set; }
        public int? CountryId { get; set; }
        public SelectList selectListCompanies { get; set; }
        public SelectList selectListTitles { get; set; }
        public SelectList selectListCountries { get; set; }

        public ContactCreateView()
        {

        }
        public ContactCreateView(IEnumerable<Company> dbCompanies, IEnumerable<Title> dbTitles, IEnumerable<Country> dbCountries)
        {
            this.selectListCompanies = new SelectList(dbCompanies, "Id", "name", CompanyId);
            this.selectListTitles = new SelectList(dbTitles, "Id", "Name", TitleId);
            this.selectListCountries = new SelectList(dbCountries, "Id", "Name", CountryId);

        }
        public void updateInfo(IEnumerable<Company> dbCompanies, IEnumerable<Title> dbTitles, IEnumerable<Country> dbCountries)
        {
            this.selectListCompanies = new SelectList(dbCompanies, "Id", "name", CompanyId);
            this.selectListTitles = new SelectList(dbTitles, "Id", "Name", TitleId);
            this.selectListCountries = new SelectList(dbCountries, "Id", "Name", CountryId);

        }

    }
}