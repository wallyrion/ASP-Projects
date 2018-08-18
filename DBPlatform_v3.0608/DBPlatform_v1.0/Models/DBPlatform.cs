using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DBPlatform_v1._0.Helpers;
using DBPlatform_v1._0.Models.Alias;
using DBPlatform_v1._0.Models.Companies;
using WebGrease.Css.Extensions;
using ListExtensions = WebGrease.Css.Extensions.ListExtensions;

namespace DBPlatform_v1._0.Models
{
    public class DBPlatform : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<JobArea> JobAreas { get; set; }
        public DbSet<JobLevel> JobLevels { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<RetrieveList> RetrieveLists { get; set; }

        public DbSet<IndustryAlias> IndustryAliases { get; set; }
        public DBPlatform() { }

        static DBPlatform()
        {
            System.Data.Entity.Database.SetInitializer(new DbPlatformInitializer());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasMany<Contact>(c=>c.Contacts)
                .WithOptional(x=>x.Company)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Industry>()
                .HasMany<IndustryAlias>(ind=>ind.Aliases)
                .WithOptional(x=>x.Industry)
                .WillCascadeOnDelete(true);
                

            modelBuilder.Entity<Company>()
                .HasMany(c => c.Industries)
                .WithMany(s => s.Companies)
                .Map(t => t.MapLeftKey("CompanyId")
                    .MapRightKey("IndustryId")
                    .ToTable("CompanyIndustry"));

            modelBuilder.Entity<Title>().HasMany(c => c.JobAreas)
                .WithMany(s => s.Titles)
                .Map(t => t.MapLeftKey("TitleId")
                    .MapRightKey("JobAreaId")
                    .ToTable("TitleJobArea"));
        }
        public class DbPlatformInitializer : DropCreateDatabaseIfModelChanges<DBPlatform>
        {
            protected override void Seed(DBPlatform context)
            {
                var users = new List<User>
                {
                    new User
                    {
                        Name = "Admin",
                        login = "alexey-12",
                        password = "alexey-12",
                        Role = "Admin"
                    },
                    new User
                    {
                        Name = "Moder1",
                        login = "moder",
                        password = "moder",
                        Role = "Moderator"
                    },
                    new User
                    {
                        Name = "user1",
                        login = "user1",
                        password = "password1",
                        Role = "User"
                    }
                };
                users.ForEach(user => context.Users.Add(user));

                

                var listCountries = Initializer.GetCountries();
                listCountries.ForEach(strCountry=>context.Countries.Add(new Country{Name = strCountry}) );

                //var listIndustries = Initializer.GetIndustries();
                //listIndustries.ForEach(strIndustry=>context.Industries.Add(new Industry{Name = strIndustry}) );
                var listIndustriesWithAlias = Initializer.GetIndustriesWithAlias();

                listIndustriesWithAlias.ForEach(ind => context.Industries.Add(ind));

                var listJobLevels = Initializer.GetJobLevels();
                int joblevelInt = 0;
                listJobLevels.ForEach(strJobLevel=>context.JobLevels.Add(new JobLevel{Name = strJobLevel, level = joblevelInt++}) );

                var listJobAreas = Initializer.GetJobAreas();
                listJobAreas.ForEach(strJobArea=>context.JobAreas.Add(new JobArea{Name = strJobArea}) );
                context.SaveChanges();
                var companies = Initializer.GetCompanies();
                companies[0].Industries.Add(context.Industries.FirstOrDefault(i=>i.Name=="Computers and Technology"));
                companies[0].Industries.Add(context.Industries.FirstOrDefault(i=>i.Name=="Manufacturing"));
                companies[0].PrimaryIndustry = (context.Industries.FirstOrDefault(i=>i.Name=="Manufacturing"));
                companies[1].Industries.Add(context.Industries.FirstOrDefault(i=>i.Name=="Computers and Technology"));
                companies[1].PrimaryIndustry = (context.Industries.FirstOrDefault(i=>i.Name=="Computers and Technology"));
                companies[2].Industries.Add(context.Industries.FirstOrDefault(i=>i.Name=="Computers and Technology"));
                companies[2].PrimaryIndustry = (context.Industries.FirstOrDefault(i=>i.Name=="Computers and Technology"));
                companies[2].Industries.Add(context.Industries.FirstOrDefault(i=>i.Name=="Manufacturing"));
                companies[3].Industries.Add(context.Industries.FirstOrDefault(i=>i.Name=="Retail and Consumer Goods"));
                companies[3].PrimaryIndustry = (context.Industries.FirstOrDefault(i=>i.Name=="Retail and Consumer Goods"));
                context.Companies.AddRange(companies);

            }
        }


    }
}