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
using PmaPlus.Services;
using PmaPlus.Tools;

namespace PmaPlus.Controllers.ApiControllers.Curriculums
{
    public class SessionsController : ApiController
    {
        private readonly CurriculumServices _curriculumServices;
        private readonly IPhotoManager _photoManager;

        public SessionsController(CurriculumServices curriculumServices, IPhotoManager photoManager)
        {
            _curriculumServices = curriculumServices;
            _photoManager = photoManager;
        }

        [Route("api/Sessions/{curriculumId:int}/{pageSize:int}/{pageNumber:int}/{orderBy:alpha?}/{direction:bool?}")]
        public SessionPage Get(int curriculumId, int pageSize, int pageNumber, string orderBy = "",bool direction = false)
        {
            var count = _curriculumServices.GetSessions(curriculumId).Count();
            var pages = (int)Math.Ceiling((double)count / pageSize);
            var sessions = _curriculumServices.GetSessions(curriculumId);

            var items = Mapper.Map<IEnumerable<Session>, IEnumerable<SessionTableViewModel>>(sessions).OrderQuery(orderBy, x => x.Number, direction).Paged(pageNumber, pageSize);

            return new SessionPage()
            {
                Count = count,
                Pages = pages,
                Items = items
            };
        }

        [Route("api/Sessions/{id:int}")]
        public SessionViewModel Get(int id)
        {
            var sess = _curriculumServices.GetSessionById(id);
            var scens = sess.Scenarios.ToList();
            return Mapper.Map<Session, SessionViewModel>(sess);
        }

        [Route("api/Sessions/{curriculumId:int}")]
        public IHttpActionResult Post(int curriculumId, [FromBody]SessionViewModel sessionViewModel)
        {
            var session = Mapper.Map<SessionViewModel, Session>(sessionViewModel);
            var newSession = _curriculumServices.AddSession(session, curriculumId, sessionViewModel.Scenarios);

            if (newSession != null)
            {
                if (_photoManager.FileExists(newSession.CoachPicture))
                {
                    newSession.CoachPicture = _photoManager.MoveFromTemp(newSession.CoachPicture,
                        FileStorageTypes.Sessions, newSession.Id, Guid.NewGuid().ToString());
                }

                if (_photoManager.FileExists(newSession.PlayerPicture))
                {
                    newSession.PlayerPicture = _photoManager.MoveFromTemp(newSession.PlayerPicture,
                        FileStorageTypes.Sessions, newSession.Id, Guid.NewGuid().ToString());
                }

                _curriculumServices.UpdateSession(newSession,newSession.Id);

            return Ok();
            }

            return Ok();

        }

        [Route("api/Sessions/{id:int}")]
        public IHttpActionResult Put(int id, [FromBody] SessionViewModel sessionViewModel)
        {
            if (!_curriculumServices.SessionExist(id))
            {
                return NotFound();
            }

            if (_photoManager.FileExists(sessionViewModel.CoachPicture))
            {
                sessionViewModel.CoachPicture = _photoManager.MoveFromTemp(sessionViewModel.CoachPicture,
                    FileStorageTypes.Sessions, id, Guid.NewGuid().ToString());
            }

            if (_photoManager.FileExists(sessionViewModel.PlayerPicture))
            {
                sessionViewModel.PlayerPicture = _photoManager.MoveFromTemp(sessionViewModel.PlayerPicture,
                    FileStorageTypes.Sessions, id, Guid.NewGuid().ToString());
            }



            var session = Mapper.Map<SessionViewModel, Session>(sessionViewModel);
            _curriculumServices.UpdateSession(session,id,sessionViewModel.Scenarios);
            return Ok();
        }

        [Route("api/Sessions/{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            if (!_curriculumServices.SessionExist(id))
            {
                return NotFound();
            }

            _curriculumServices.DeleteSession(id);
            return Ok();
        }



    }
}
