using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBPlatform_v1._0.Models.Companies
{
    public class Title
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? JobLevelId { get; set; }
        public JobLevel JobLevel { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public virtual ICollection<JobArea> JobAreas { get; set; }
        public ICollection<Contact> Contacts { get; set; }
        
        public Title()
        {
            DateCreated = DateModified = DateTime.UtcNow;
            JobAreas = new List<JobArea>();
        }

    }
    public class TitleEditView
    {
        public Title Title { get; set; }
        public int[] selectedJobAreas { get; set; }
        public SelectList selectListJobLevels { get; set; }
        public IEnumerable<JobArea> JobAreas { get; set; }
        public TitleEditView(Title title, IEnumerable<JobArea> DBJobAreas, IEnumerable<JobLevel> DBJobLevels)
        {
            this.Title = title;
            if (title.JobLevel != null)
            {
                this.selectListJobLevels = new SelectList(DBJobLevels, "Id", "Name", title.JobLevel.Id);
            }
            else this.selectListJobLevels = new SelectList(DBJobLevels, "Id", "Name");
            this.JobAreas = DBJobAreas.ToList();
        }
        public TitleEditView()
        {
            JobAreas = new List<JobArea>();
        }
    }
    public class SearchView
    {
        public string searchType { get; set; }
        public string searchName { get; set; }
    }

    public class JobLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int level { get; set; }
        public JobLevel(string Name, int level)
        {
            this.Name = Name;
            this.level = level;
        }
        public JobLevel()
        {

        }
    }
    public class JobArea
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Title> Titles { get; set; }
        public JobArea()
        {
            Titles = new List<Title>();
        }
        public JobArea(string name)
        {
            Name = name;
            Titles = new List<Title>();

        }
    }
}