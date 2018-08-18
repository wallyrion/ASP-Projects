using DBPlatform_v1._0.Models;
using DBPlatform_v1._0.Models.Soccer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace DBPlatform_v1._0.Controllers
{
    public class SoccerController : Controller
    {
        private SoccerContext dbSoccer = new SoccerContext();
        public ActionResult SoccerInfo()
        {
            var pl = dbSoccer.Players;
            var players = dbSoccer.Players.Include(p => p.Team);
            return View(players.ToList());
        }
        //[HttpGet]
        //public ActionResult Index()
        //{
        //    var players = dbSoccer.Players.Include(p => p.Team);
        //    return View(players.ToList());
        //}
        [HttpGet]
        public ActionResult Create()
        {
            // Формируем список команд для передачи в представление
            SelectList teams = new SelectList(dbSoccer.Teams, "Id", "Name");
            ViewBag.Teams = teams;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Player player)
        {
            //Добавляем игрока в таблицу
            dbSoccer.Players.Add(player);
            dbSoccer.SaveChanges();
            // перенаправляем на главную страницу
            return RedirectToAction("Index");
        }
        public ActionResult TeamDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Team team = dbSoccer.Teams.Include(t => t.Players).FirstOrDefault(t => t.Id == id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            // Находим в бд футболиста
            Player player = dbSoccer.Players.Find(id);
            if (player != null)
            {
                // Создаем список команд для передачи в представление
                SelectList teams = new SelectList(dbSoccer.Teams, "Id", "Name", player.TeamId);
                ViewBag.Teams = teams;
                return View(player);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Player player)
        {
            dbSoccer.Entry(player).State = EntityState.Modified;
            dbSoccer.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ListTeams()
        {
            var teams = dbSoccer.Teams.Include(t=> t.Players);
            return View(teams.ToList());
        }

        public ActionResult Index(int? team, string position)
        {
            IQueryable<Player> players = dbSoccer.Players.Include(p => p.Team);
            if (team != null && team != 0)
            {
                players = players.Where(p => p.TeamId == team);
            }
            if (!String.IsNullOrEmpty(position) && !position.Equals("Все"))
            {
                players = players.Where(p => p.Position == position);
            }

            List<Team> teams = dbSoccer.Teams.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            teams.Insert(0, new Team { Name = "Все", Id = 0 });

            PlayersListViewModel plvm = new PlayersListViewModel
            {
                Players = players.ToList(),
                Teams = new SelectList(teams, "Id", "Name"),
                Positions = new SelectList(new List<string>()
            {
                "Все",
                "Нападающий",
                "Полузащитник",
                "Защитник",
                "Вратарь"
            })
            };
            return View(plvm);
        }
    }
}