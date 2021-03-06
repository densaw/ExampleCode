﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Web;
using System.Web.Http;
using PmaPlus.Data;
using PmaPlus.Data.Repository.Iterfaces;
using PmaPlus.Filters;
using PmaPlus.Model;
using PmaPlus.Model.ViewModels.Club;
using PmaPlus.Model.ViewModels.Curriculum;
using PmaPlus.Services;
using PmaPlus.Tools;

namespace PmaPlus.Controllers.ApiControllers
{
    public class ClubsController : ApiController
    {
        private readonly ClubServices _clubServices;
        private readonly IPhotoManager _photoManager;
        private readonly UserServices _userServices;
        private readonly ICoachRepository _coachRepository;


        public ClubsController(ClubServices clubServices, IPhotoManager photoManager, UserServices userServices)
        {
            _clubServices = clubServices;
            _photoManager = photoManager;
            _userServices = userServices;
            //_photoManager = new LocalPhotoManager(HttpContext.Current.Server.MapPath(@"~/App_Data/temp"));
        }

        [Route("api/Clubs/{pageSize:int}/{pageNumber:int}/{orderBy:alpha?}/{direction:bool?}")]
        public ClubPage Get(int pageSize, int pageNumber, string orderBy = "", bool direction = false)
        {
            var count = _clubServices.GetClubsTableViewModels().Count();
            var pages = (int)Math.Ceiling((double)count / pageSize);
            var items = _clubServices.GetClubsTableViewModels().OrderQuery(orderBy, x => x.Id, direction).Paged(pageNumber, pageSize);

            return new ClubPage()
            {
                Count = count,
                Pages = pages,
                Items = items
            };

        }

        // GET: api/Clubs
        public IEnumerable<ClubTableViewModel> Get()
        {
            return _clubServices.GetClubsTableViewModels();
        }

        [Route("api/Clubs/Current")]
        public AddClubViewModel GetCurrentClub()
        {

            return _clubServices.GetClubById(_userServices.GetClubByUserName(User.Identity.Name).Id);
        }


        // GET: api/Clubs/5
        public AddClubViewModel Get(int id)
        {
            return _clubServices.GetClubById(id);
        }

        [Route("api/Clubs/logo")]
        public HttpResponseMessage GetLogo()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            HttpResponseMessage result;
            FileStream _fileStream = _photoManager.GetFileStream(club.Logo, FileStorageTypes.Clubs, club.Id);
            if (_fileStream == null)
            {
                result = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(_fileStream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(Path.GetExtension(club.Logo)));
            }
            return result;
        }

        [Route("api/Clubs/color")]
        public string GetColor()
        {
            return _userServices.GetClubColorByUser(User.Identity.Name);
        }

        [Route("api/Clubs/background")]
        public HttpResponseMessage GetBackground()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            HttpResponseMessage result;
            FileStream _fileStream = _photoManager.GetFileStream(club.Background, FileStorageTypes.Clubs, club.Id);
            if (_fileStream == null)
            {
                result = Request.CreateResponse(HttpStatusCode.NotFound);
                //result.Content = new StreamContent(new FileStream(HttpContext.Current.Server.MapPath(@"~/App_Data/FileStorage"), FileMode.Open, FileAccess.Read));
                //result.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(Path.GetExtension(club.Club.Background)));
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(_fileStream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(Path.GetExtension(club.Background)));
            }
            return result;

        }

        // POST: api/Clubs
        public IHttpActionResult Post([FromBody]AddClubViewModel clubViewModel)
        {
            var newClub = _clubServices.AddClub(clubViewModel);

            if (_userServices.UserExist(clubViewModel.ClubAdminEmail))
            {
                return Conflict();
            }

            if (newClub != null)
            {
                if (_photoManager.FileExists(clubViewModel.Logo))
                {
                    newClub.Logo = _photoManager.MoveFromTemp(newClub.Logo, FileStorageTypes.Clubs, newClub.Id, "logo");
                }
                if (_photoManager.FileExists(clubViewModel.Background))
                {
                    newClub.Background = _photoManager.MoveFromTemp(newClub.Background, FileStorageTypes.Clubs,
                        newClub.Id, "Background");
                }
                _clubServices.UpdateClub(newClub, newClub.Id);
                return Ok();
            }
            return BadRequest();
        }

        // PUT: api/Clubs/5
        public IHttpActionResult Put(int id, [FromBody]AddClubViewModel clubViewModel)
        {
            if (!_clubServices.ClubIsExist(id))
            {
                return NotFound();
            }
            if (_photoManager.FileExists(clubViewModel.Logo))
            {
                clubViewModel.Logo = _photoManager.MoveFromTemp(clubViewModel.Logo, FileStorageTypes.Clubs, clubViewModel.Id, "logo");
            }
            if (_photoManager.FileExists(clubViewModel.Background))
            {
                clubViewModel.Background = _photoManager.MoveFromTemp(clubViewModel.Background, FileStorageTypes.Clubs,
                    clubViewModel.Id, "Background");
            }
            _clubServices.UpdateClub(clubViewModel, id);
            return Ok();
        }

        // DELETE: api/Clubs/5
        public IHttpActionResult Delete(int id)
        {
            if (!_clubServices.ClubIsExist(id))
            {
                return NotFound();
            }
            _clubServices.DeleteClub(id);
            _photoManager.Delete(FileStorageTypes.Clubs, id);
            return Ok();

        }
    }
}
