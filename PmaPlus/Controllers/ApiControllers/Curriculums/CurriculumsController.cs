﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using PmaPlus.Data;
using PmaPlus.Model.Models;
using PmaPlus.Model.ViewModels.Curriculum;
using PmaPlus.Model.ViewModels.Physio;
using PmaPlus.Services;

namespace PmaPlus.Controllers.ApiControllers.ClubAdminApi
{
    public class CurriculumsController : ApiController
    {
        private readonly CurriculumServices _curriculumServices;
        private readonly UserServices _userServices;

        public CurriculumsController(CurriculumServices curriculumServices, UserServices userServices)
        {
            _curriculumServices = curriculumServices;
            _userServices = userServices;
        }

        [Route("api/Curriculums/List")]
        public IEnumerable<CurriculumsList> GetCurriculumsLists()
        {
            var clubId = _userServices.GetClubByUserName(User.Identity.Name).Id;
            return _curriculumServices.GetClubCurriculumsList(clubId);
        }

        public CurriculumViewModel Get(int id)
        {
            return Mapper.Map<Curriculum, CurriculumViewModel>(_curriculumServices.GetCurriculumById(id));
        }

        [Route("api/Curriculums/{pageSize:int}/{pageNumber:int}/{orderBy:alpha?}/{direction:bool?}")]
        public CurriculumPage Get(int pageSize, int pageNumber, string orderBy = "", bool direction = false)
        {

            var clubId = _userServices.GetClubByUserName(User.Identity.Name).Id;
          

            var count = _curriculumServices.GetClubCurriculums(clubId).Count();
            var pages = (int)Math.Ceiling((double)count / pageSize);
            var curriculums = _curriculumServices.GetClubCurriculums(clubId);
            var items = Mapper.Map<IEnumerable<Curriculum>, IEnumerable<CurriculumTableViewModel>>(curriculums).OrderQuery(orderBy, x => x.Id, direction).Paged(pageNumber, pageSize);

            return new CurriculumPage()
            {
                Count = count,
                Pages = pages,
                Items = items
            };

        }

        public IEnumerable<CurriculumViewModel> Get()
        {
            var clubId = _userServices.GetClubByUserName(User.Identity.Name).Id;
            return Mapper.Map<IEnumerable<Curriculum>, IEnumerable<CurriculumViewModel>>(_curriculumServices.GetClubCurriculums(clubId));
        }


        [Route("api/Curriculums/ToLive/{id:int}")]
        public IHttpActionResult PutLiveMode(int id, [FromBody]bool isLive)
        {
            if (!_curriculumServices.CurriculumExist(id))
                return NotFound();

            _curriculumServices.SetCurriculumToLive(id,isLive);
            return Ok();

        }

        public IHttpActionResult Post([FromBody] CurriculumViewModel curriculumViewModel)
        {
            var curriculum = Mapper.Map<CurriculumViewModel, Curriculum>(curriculumViewModel);
            var clubId = _userServices.GetClubByUserName(User.Identity.Name).Id;
            _curriculumServices.AddCurriculum(curriculum,clubId);
            return Ok();
        }

        public IHttpActionResult PutClubCurriculum(int id, [FromBody]CurriculumViewModel curriculumViewModel)
        {
            if (!_curriculumServices.CurriculumExist(id))
                return NotFound();
            _curriculumServices.UpdateCurriculum(Mapper.Map<CurriculumViewModel, Curriculum>(curriculumViewModel),id);
            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            if (!_curriculumServices.CurriculumExist(id))
                return NotFound();
            _curriculumServices.DeleteCurriculum(id);
            return Ok();
        }

    }
}
