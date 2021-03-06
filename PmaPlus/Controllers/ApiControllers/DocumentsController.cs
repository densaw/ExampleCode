﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using PmaPlus.Model;
using PmaPlus.Model.ViewModels.Document;
using PmaPlus.Services;
using PmaPlus.Services.Services;
using PmaPlus.Tools;
using WebGrease.Css.Extensions;

namespace PmaPlus.Controllers.ApiControllers
{
    public class DocumentsController : ApiController
    {
        private readonly IDocumentManager _documentManager;
        private readonly SharingFoldersServices _sharingFoldersServices;
        private readonly UserServices _userServices;

        public DocumentsController(IDocumentManager documentManager, SharingFoldersServices sharingFoldersServices, UserServices userServices)
        {
            _documentManager = documentManager;
            _sharingFoldersServices = sharingFoldersServices;
            _userServices = userServices;
        }

        [Route("api/Documents/Directories/Shared")]
        public IEnumerable<DirectoryViewModel> GetSharedDirectories()
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            var user = _userServices.GetUserByEmail(User.Identity.Name);

            var dirs = _documentManager.GetUserDirectories(club.ClubAdmin.User.Id);
            if (dirs == null)
                return null;


            return dirs.Select(dir => new DirectoryViewModel()
            {
                Name = dir.Name,
                Roles = _sharingFoldersServices.GetDirectoryRoles(dir.Name, club.ClubAdmin.User.Id)
            }).Where(d => d.Roles.Contains(user.Role)).ToList();
        }



        [Route("api/Documents/Directories")]
        public IEnumerable<DirectoryViewModel> GetDirectories()
        {
            var user = _userServices.GetUserByEmail(User.Identity.Name);

            var dirs = _documentManager.GetUserDirectories(user.Id);
            return dirs.Select(dir => new DirectoryViewModel()
            {
                Name = dir.Name,
                Roles = _sharingFoldersServices.GetDirectoryRoles(dir.Name, user.Id)
            }).ToList();
        }


        [Route("api/Documents/Directories")]
        public IHttpActionResult PostDirectory(DirectoryViewModel directory)
        {
            var user = _userServices.GetUserByEmail(User.Identity.Name);
            if (_documentManager.CreateDirectory(directory.Name, user.Id))
            {
                if (directory.Roles.Count > 0 && user.Role == Role.ClubAdmin)
                {
                    _sharingFoldersServices.ShareDirectory(directory.Name, user.Id, directory.Roles);
                }
                return Ok();
            }
            return Conflict();
        }

        [Route("api/Documents/Directories")]
        public IHttpActionResult PutDirectory(DirectoryViewModel directory)
        {
            var user = _userServices.GetUserByEmail(User.Identity.Name);

            _sharingFoldersServices.ShareDirectory(directory.Name, user.Id, directory.Roles);

            return Ok();
        }


        [Route("api/Documents/{folder}")]
        public IEnumerable<FileViewModel> GetFiles(string folder)
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            var user = _userServices.GetUserByEmail(User.Identity.Name);
            var files = _documentManager.GetDirectoryFiles(folder, club.ClubAdmin.User.Id);

            var fileViewModels = files as FileViewModel[] ?? files.ToArray();
            if (user.Role != Role.ClubAdmin)
            {
                foreach (var file in fileViewModels)
                {
                    file.IsMy = _sharingFoldersServices.IsUserOwner(file.Name, folder, user.Id);
                }
            }
            return fileViewModels;
        }


        [Route("api/Documents/{folder}")]
        public IHttpActionResult DeleteFolder(string folder)
        {
            var user = _userServices.GetUserByEmail(User.Identity.Name);
            if (_documentManager.DeleteDirectory(folder, user.Id))
            {
                _sharingFoldersServices.DeleteDirectory(folder, user.Id);
            }

            return Ok();
        }

        [Route("api/Documents/{folder}")]
        public async Task<IHttpActionResult> PostFile(string folder)
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            var user = _userServices.GetUserByEmail(User.Identity.Name);

            if (Request.Content.IsMimeMultipartContent())
            {
                var doc = await _documentManager.AddDocument(Request, folder, club.ClubAdmin.User.Id);

                if (user.Role != Role.ClubAdmin)
                {
                    _sharingFoldersServices.SetFileOwner(doc, folder, user.Id);
                }


                return Ok(doc);
            }
            return BadRequest("Unsuported type");
        }


        [Route("api/Documents/{folder}/{file}")]
        public HttpResponseMessage GetFile(string folder, string file)
        {
            var adminId = _userServices.GetClubByUserName(User.Identity.Name).ClubAdmin.User.Id;

            HttpResponseMessage result;
            FileStream fileStream = _documentManager.GetFileStream(file, folder, adminId);
            if (fileStream == null)
            {
                result = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(fileStream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(Path.GetExtension(file)));
            }
            return result;
        }

        [Route("api/Documents/{folder}/{file}")]
        public IHttpActionResult DeleteFile(string folder, string file)
        {
            var club = _userServices.GetClubByUserName(User.Identity.Name);
            var user = _userServices.GetUserByEmail(User.Identity.Name);
            if (_documentManager.DeleteFile(file, folder, club.ClubAdmin.User.Id))
            {
                if (user.Role != Role.ClubAdmin)
                {
                    _sharingFoldersServices.DeleteFile(file, folder, user.Id);
                }
                return Ok();
            }
            return Conflict();
        }


        [Route("api/Documents/{folder}/{file}/Thumbnail")]
        public HttpResponseMessage GetThumbnail(string folder, string file)
        {
            var adminId = _userServices.GetClubByUserName(User.Identity.Name).ClubAdmin.User.Id;
            HttpResponseMessage result;

            var FileStream = _documentManager.GetFileStream(file, folder, adminId);
            Image img = Image.FromStream(FileStream);
            Size thumSize = TimeUtils.GetThumbnailSize(img, 150);
            var thumbnail = img.GetThumbnailImage(thumSize.Width, thumSize.Height, () => false, IntPtr.Zero);
            FileStream.Dispose();


            using (var stream = new MemoryStream())
            {
                thumbnail.Save(stream, ImageFormat.Jpeg);

                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(stream.ToArray());
                result.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                return result;
            }
        }


    }
}
