﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using PmaPlus.Data;
using PmaPlus.Model;
using PmaPlus.Model.Models;
using PmaPlus.Model.ViewModels.Skill;
using PmaPlus.Model.ViewModels.TalentIdentifications;
using PmaPlus.Services;
using PmaPlus.Services.Services;

namespace PmaPlus.Controllers.ApiControllers
{
    public class TalentIdentificationController : ApiController
    {
        private readonly TalentServices _talentServices;
        private readonly UserServices _userServices;

        public TalentIdentificationController(TalentServices talentServices, UserServices userServices)
        {
            _talentServices = talentServices;
            _userServices = userServices;
        }


        [Route("api/TalentIdentification/{pageSize:int}/{pageNumber:int}/{orderBy:alpha?}/{direction:bool?}")]
        public TalentIdentificationPage Get(int pageSize, int pageNumber, string orderBy = "", bool direction = false)
        {
            var clubId = _userServices.GetClubByUserName(User.Identity.Name).Id;

            var count = _talentServices.GetTalentIdentifications(clubId).Count();
            var pages = (int)Math.Ceiling((double)count / pageSize);
            var bodyParts = _talentServices.GetTalentIdentifications(clubId);
            var items = Mapper.Map<IEnumerable<TalentIdentification>, IEnumerable<TalentIdentificationTableViewModel>>(bodyParts).OrderQuery(orderBy, x => x.Id, direction).Paged(pageNumber, pageSize);

            return new TalentIdentificationPage()
            {
                Count = count,
                Pages = pages,
                Items = items
            };
        }



        public TalentIdentificationViewModel Get(int id)
        {
            return
                Mapper.Map<TalentIdentification, TalentIdentificationViewModel>(
                    _talentServices.GetTalentIdentificationById(id));
        }

        [Route("api/TalentIdentification/Detail/{id:int}")]
        public TalentIdentificationDetailViewModel GetDetail(int id)
        {

            var detail = Mapper.Map<TalentIdentification, TalentIdentificationDetailViewModel>(
                    _talentServices.GetTalentIdentificationById(id));

            
            return detail;

        }

        public IHttpActionResult Post(TalentIdentificationViewModel identificationViewModel)
        {
            var clubId = _userServices.GetClubByUserName(User.Identity.Name).Id;
            var user = _userServices.GetUserByEmail(User.Identity.Name);
            if (user != null && user.Role == Role.Scout)
            {
                var scout = _userServices.GetClubScouts(clubId).FirstOrDefault(s => s.User.Id == user.Id);
                if (scout != null)
                    identificationViewModel.ScoutId = scout.Id;
            }

            var talent = Mapper.Map<TalentIdentificationViewModel, TalentIdentification>(identificationViewModel);

            _talentServices.AddTalentIdentification(talent, clubId);
            return Ok();
        }

        public IHttpActionResult Put(int id, [FromBody]TalentIdentificationViewModel identificationViewModel)
        {
            if (!_talentServices.TalentExist(id))
            {
                return NotFound();
            }
            var talent = Mapper.Map<TalentIdentificationViewModel, TalentIdentification>(identificationViewModel);

            var clubId = _userServices.GetClubByUserName(User.Identity.Name).Id;
            _talentServices.UpdateTalentIdentification(talent,id,clubId);
            return Ok();
        }


        [Route("api/TalentIdentification/Invite/{id:int}")]
        public IHttpActionResult PutInvite(int id, [FromBody]TalentInviteViewModel talentInviteViewModel)
        {
            if (!_talentServices.TalentExist(id))
            {
                return NotFound();
            }
            _talentServices.InviteTalent(id,talentInviteViewModel);
            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            if (!_talentServices.TalentExist(id))
            {
                return NotFound();
            }

            _talentServices.DeleteIdentification(id);
            return Ok();
        }

    }
}
