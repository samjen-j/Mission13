using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mission13.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mission13.Controllers
{
    public class HomeController : Controller
    {

        private IBowlerRepository repo {get;set;}
        public HomeController(IBowlerRepository temp)
        {
            repo = temp;
            
        }

        public IActionResult Index(string team)
        {
            var bowlers = repo.Bowlers.Where(x => x.Team.TeamName == team || team == null).ToList();
            ViewBag.Teams = repo.Teams.ToList();
            ViewBag.TeamName = team;

            return View(bowlers);
        }

        [HttpGet]
        public IActionResult AddBowler(int id)
        {
            Bowler bowler = new Bowler();

            ViewBag.BowlerID = id;

            ViewBag.Teams = repo.Teams.ToList();

            return View("AddBowler", bowler);
        }

        [HttpPost]
        public IActionResult AddBowler(Bowler b)
        {
            if (ModelState.IsValid)
            {
                repo.AddBowler(b);
                repo.SaveBowler(b);

                return View("Confirmation", b);
            }
            else
            {
                return View("Index");
            }


        }

        [HttpGet]
        public IActionResult Edit(int bowlerid)
        {

            var bowler = repo.Bowlers.Single(x => x.BowlerID == bowlerid);

            ViewBag.Teams = repo.Teams.ToList();

            return View("AddBowler", bowler);

        }

        [HttpPost]
        public IActionResult Edit(Bowler b)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Teams = repo.Teams.ToList();
                return View("AddBowler", b);
               
            }
            repo.SaveBowler(b);
            return RedirectToAction("Index");

        }

        public IActionResult DeleteBowler(Bowler b, int id)
        {
            ViewBag.BowlerID = id;
            repo.DeleteBowler(b);

            return RedirectToAction("Index");
        }

    }
}
