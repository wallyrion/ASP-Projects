using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBPlatform_v1._0.Models;
using DBPlatform_v1._0.Models.Alias;
using WebGrease.Css.Extensions;

namespace DBPlatform_v1._0.Helpers
{
    public static class Initializer
    {
        public static List<string> GetCountries()
        {
            var countries = new List<string>
            {
                "USA",
                "UK",
                "Afghanistan",
                "Albania",
                "Algeria",
                "Andorra",
                "Angola",
                "Anguilla",
                "Antigua & Barbuda",
                "Argentina",
                "Armenia",
                "Australia",
                "Austria",
                "Azerbaijan",
                "Bahamas",
                "Bahrain",
                "Bangladesh",
                "Barbados",
                "Belarus",
                "Belgium",
                "Belize",
                "Benin",
                "Bermuda",
                "Bhutan",
                "Bolivia",
                "Bosnia & Herzegovina",
                "Botswana",
                "Brazil",
                "Brunei Darussalam",
                "Bulgaria",
                "Burkina Faso",
                "Myanmar/Burma",
                "Burundi",
                "Cambodia",
                "Cameroon",
                "Canada",
                "Cape Verde",
                "Cayman Islands",
                "Central African Republic",
                "Chad",
                "Chile",
                "China",
                "Colombia",
                "Comoros",
                "Congo",
                "Costa Rica",
                "Croatia",
                "Cuba",
                "Cyprus",
                "Czech Republic",
                "Democratic Republic of the Congo",
                "Denmark",
                "Djibouti",
                "Dominica",
                "Dominican Republic",
                "Ecuador",
                "Egypt",
                "El Salvador",
                "Equatorial Guinea",
                "Eritrea",
                "Estonia",
                "Ethiopia",
                "Fiji",
                "Finland",
                "France",
                "French Guiana",
                "Gabon",
                "Gambia",
                "Georgia",
                "Germany",
                "Ghana",
                "Great Britain",
                "Greece",
                "Grenada",
                "Guadeloupe",
                "Guatemala",
                "Guinea",
                "Guinea-Bissau",
                "Guyana",
                "Haiti",
                "Honduras",
                "Hungary",
                "Iceland",
                "India",
                "Indonesia",
                "Iran",
                "Iraq",
                "Israel and the Occupied Territories",
                "Italy",
                "Ivory Coast (Cote d'Ivoire)",
                "Jamaica",
                "Japan",
                "Jordan",
                "Kazakhstan",
                "Kenya",
                "Kosovo",
                "Kuwait",
                "Kyrgyz Republic (Kyrgyzstan)",
                "Laos",
                "Latvia",
                "Lebanon",
                "Lesotho",
                "Liberia",
                "Libya",
                "Liechtenstein",
                "Lithuania",
                "Luxembourg",
                "Republic of Macedonia",
                "Madagascar",
                "Malawi",
                "Malaysia",
                "Maldives",
                "Mali",
                "Malta",
                "Martinique",
                "Mauritania",
                "Mauritius",
                "Mayotte",
                "Mexico",
                "Moldova, Republic of",
                "Monaco",
                "Mongolia",
                "Montenegro",
                "Montserrat",
                "Morocco",
                "Mozambique",
                "Namibia",
                "Nepal",
                "Netherlands",
                "New Zealand",
                "Nicaragua",
                "Niger",
                "Nigeria",
                "Korea, Democratic Republic of (North Korea)",
                "Norway",
                "Oman",
                "Pacific Islands",
                "Pakistan",
                "Panama",
                "Papua New Guinea",
                "Paraguay",
                "Peru",
                "Philippines",
                "Poland",
                "Portugal",
                "Puerto Rico",
                "Qatar",
                "Reunion",
                "Romania",
                "Russian Federation",
                "Rwanda",
                "Saint Kitts and Nevis",
                "Saint Lucia",
                "Saint Vincent's & Grenadines",
                "Samoa",
                "Sao Tome and Principe",
                "Saudi Arabia",
                "Senegal",
                "Serbia",
                "Seychelles",
                "Sierra Leone",
                "Singapore",
                "Slovak Republic (Slovakia)",
                "Slovenia",
                "Solomon Islands",
                "Somalia",
                "South Africa",
                "Korea, Republic of (South Korea)",
                "South Sudan",
                "Spain",
                "Sri Lanka",
                "Sudan",
                "Suriname",
                "Swaziland",
                "Sweden",
                "Switzerland",
                "Syria",
                "Tajikistan",
                "Tanzania",
                "Thailand",
                "Timor Leste",
                "Togo",
                "Trinidad & Tobago",
                "Tunisia",
                "Turkey",
                "Turkmenistan",
                "Turks & Caicos Islands",
                "Uganda",
                "Ukraine",
                "United Arab Emirates",
                "United States of America (USA)",
                "Uruguay",
                "Uzbekistan",
                "Venezuela",
                "Vietnam",
                "Virgin Islands (UK)",
                "Virgin Islands (US)",
                "Yemen",
                "Zambia",
                "Zimbabwe"
            };
            return countries;
        }

        public static List<string> GetIndustries()
        {
            var industries = new List<string>
            {
                "Advertisement / Marketing",
                "Aerospace / Aviation",
                "Agriculture",
                "Automotive",
                "Biotech and Pharmaceuticals",
                "Computers and Technology",
                "Construction",
                "Corporate Services",
                "Education",
                "Finance",
                "Government",
                "Healthcare / Medical",
                "Insurance",
                "Legal",
                "Manufacturing",
                "Media",
                "Non-Profit / Organizations",
                "Real Estate",
                "Retail and Consumer Goods",
                "Service Industry",
                "Telecommunication",
                "Transportation and Logistics",
                "Travel / Hospitality / Entertainment",
                "Utility / Energy"
            };
            return industries;
        }
        public static List<Industry> GetIndustriesWithAlias()
        {
            List<Industry> industries = new List<Industry>();
            industries.Add(CreateIndustryAlias("Finance", "Financial Services", "Accounting", "Banking"));
            industries.Add(CreateIndustryAlias("Computers and Technology", "Consumer Electronics", "High Technology Software"));
            industries.Add(CreateIndustryAlias("Manufacturing", "Electrical / Electronic Manufacturing", "Mechanical / Industrial Engineering", "Consumer Goods", "Consumer Electronics"));
            industries.Add(CreateIndustryAlias("Retail and Consumer Goods", "Manufacturing Retail", "Retail", "Wholesale"));

            return industries;
        }

        /*public static List<Contact> GetContacts()
        {
            List<Contact> contacts = new List<Contact>
            {
                new Contact{}
            }
        }*/

        private static Industry CreateIndustryAlias(string name, params string[] aliases)
        {
            var ind = new Industry(name);
            var newAlias = new List<IndustryAlias>();
            aliases.ForEach(a => newAlias.Add(new IndustryAlias
            {
                Name = a,
                Industry = ind
            }));
            ind.Aliases = newAlias;
            return ind;
        }

        public static List<Company> GetCompanies()
        {
            var companies = new List<Company>
            {
                new Company
                {
                    name = "Apple",
                    domain = "apple.com",
                    website = "www.apple.com",
                    employees_min = 100000,
                    employees_max = 100000,
                    employees_prooflink = "https://finance.yahoo.com/quote/AAPL/profile",
                    revenue_min = 229234,
                    revenue_max = 229234,
                    revenue_prooflink = "https://finance.yahoo.com/quote/AAPL/financials"
                },
                new Company
                {
                    name = "Microsoft Corporation",
                    domain = "microsoft.com",
                    website = "http://www.microsoft.com",
                    employees_min = 124000,
                    employees_max = 124000,
                    employees_prooflink = "https://finance.yahoo.com/quote/MSFT/profile",
                    revenue_min = 89950,
                    revenue_max = 89950,
                    revenue_prooflink = "http://finance.yahoo.com/quote/MSFT/financials"
                },
                new Company
                {
                    name = "Koninklijke Philips Electronics Nv",
                    domain = "philips.com",
                    website = "www.philips.com",
                    employees_min = 73951,
                    employees_max = 73951,
                    employees_prooflink = "https://finance.yahoo.com/quote/PHG/profile",
                    revenue_min = 26418,
                    revenue_max = 26418,
                    revenue_prooflink = "https://finance.yahoo.com/quote/PHG/financials"
                },
                new Company
                {
                    name = "Samsung Electronics Company Ltd",
                    domain = "samsung.com",
                    website = "www.samsung.com",
                    employees_min = 10001,
                    employees_max = 0,
                    employees_prooflink = "https://www.linkedin.com/company/samsung-electronics",
                    revenue_min = 225205,
                    revenue_max = 225205,
                    revenue_prooflink = "https://finance.yahoo.com/quote/005930.KS/financials"
                }

            };
            return companies;
        }
        

    

    public static List<string> GetJobAreas()
    {
        var jobAreas = new List<string>
            {
                "AGRICULTURE",
                "BANKING / MORTGAGE",
                "BIOTECH",
                "BUILDING CONSTRUCTION",
                "BUSINESS",
                "CREATIVE / DESIGN",
                "CUSTOMER SUPPORT / CLIENT SERVICES",
                "EDUCATION",
                "ENGINEERING",
                "EXECUTIVES",
                "FINANCE / ACCOUNTING",
                "FOOD SERVICES / HOSPITALITY",
                "GOVERNMENT",
                "HUMAN RESOURCES",
                "INSTALLATION / MAINTENANCE / REPAIR",
                "INSURANCE",
                "IT / COMPUTERS / ELECTRONICS",
                "JOURNALISM / MEDIA / ENTERTAINMENT",
                "LEGAL",
                "LOGISTICS / TRANSPORTATION",
                "MANUFACTURING / PRODUCTION / OPERATIONS",
                "MARKETING",
                "MEDICAL & HEALTH",
                "QUALITY ASSURANCE / SAFETY",
                "REAL ESTATE",
                "RELIGIOUS",
                "RETAIL",
                "SALES",
                "SECURITY SERVICES"
            };
        return jobAreas;
    }

    public static List<string> GetJobLevels()
    {
        var jobLevels = new List<string>
            {
                "C-Level",
                "Owner",
                "Managing Director",
                "Executive VP",
                "Senior VP",
                "Vice President",
                "Head",
                "General Manager",
                "Senior Director",
                "Director",
                "Senior Manager",
                "Manager",
                "Supervisor",
                "Senior Employee",
                "Developer",
                "Individual Contributor",
                "Specialist",
                "Analyst",
                "Contractor",
                "Consultant",
                "Intern",
                "Student",
                "Professor"
            };
        return jobLevels;
    }
}

}