﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PmaPlus.Services;

namespace PmaPlus.Areas.Player.Controllers
{
    [Authorize(Roles = "Player")]
    public class HomeController : Controller
    {
        private readonly UserServices _userServices;

        public HomeController(UserServices userServices)
        {
            _userServices = userServices;
        }
        // GET: Player/Home
        public ActionResult Dashboard()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult Communications()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult ClubDiary()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult Documents()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult Nutrition()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult Fitness()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult Tracker()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult Wellbeing()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult Injuries()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult MatchReports()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult Objectives()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult Profile()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult SkillAndKnowledge()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult ReviewReports()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult Exercise()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult Testing()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
        public ActionResult Training()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            ViewBag.them = club != null ? club.ColorTheme : "#3276b1";
            return View();
        }
    }
}