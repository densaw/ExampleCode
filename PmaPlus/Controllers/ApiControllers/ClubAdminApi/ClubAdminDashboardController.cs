﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.OData.Edm.Library;
using PmaPlus.Model.ViewModels;
using PmaPlus.Model.ViewModels.DashboardContent;
using PmaPlus.Services;

namespace PmaPlus.Controllers.ApiControllers.ClubAdminApi
{
    public class ClubAdminDashboardController : ApiController
    {
        private readonly UserServices _userServices;

        public ClubAdminDashboardController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [Route("api/ClubAdminDashboard/ClubName")]
        public string GetClubName()
        {
            var clubAdmin = _userServices.GetClubByUserName(User.Identity.Name);
            if (clubAdmin == null)
            {
                return "";
            }
            return clubAdmin.Name;

        }



        [Route("api/ClubAdminDashboard/Players/ScoreGraph")]
        public IEnumerable<GraphBoxViewModel> GetPlayersScoreGraphs()
        {
            var temp = new List<GraphBoxViewModel>();

            var today = DateTime.Now.Month;

            for (int i = 0; i < 11; i++)
            {
                if (today + 1 > 12)
                {
                    today = 1;
                }
                temp.Add(new GraphBoxViewModel(){Month = today,PlayersScore = 0});
                today++;
            }

            return temp; //TODO: Players score graph
        }


        [Route("api/ClubAdminDashboard/Players/Attendance/Week")]
        public InfoBoxViewModel GetWeekAttendance()
        {
            return null; //TODO: Week attendance players for club
        }

        [Route("api/ClubAdminDashboard/Players/Attendance/Quarter")]
        public InfoBoxViewModel GetQuarterAttendance()
        {
            return null; //TODO: quarter attendance players for club
        }

        [Route("api/ClubAdminDashboard/Players/Ijuries")]
        public InfoBoxViewModel GetPlayersIjuries()
        {
            return null; //TODO:  players with active injuries for club
        }






    }
}
