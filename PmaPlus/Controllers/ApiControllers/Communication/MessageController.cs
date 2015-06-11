﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PmaPlus.Model;
using PmaPlus.Model.ViewModels;
using PmaPlus.Model.ViewModels.MessagePrivate;
using PmaPlus.Model.ViewModels.MessageWall;
using PmaPlus.Services;

namespace PmaPlus.Controllers.ApiControllers.Communication
{
    public class MessageController : ApiController
    {
        private readonly MessageServices _messageServices;
        private readonly UserServices _userServices;
        private readonly MessagePrivateServices _messagePrivateServices;

        public MessageController(MessageServices messageServices, UserServices userServices, MessagePrivateServices messagePrivateServices)
        {
            _messageServices = messageServices;
            _userServices = userServices;
            _messagePrivateServices = messagePrivateServices;
        }

        public IHttpActionResult Post(MessageViewModel message)
        {
            message.UserId = _userServices.GetUserByEmail(User.Identity.Name).Id;
            var newMessage = _messageServices.AddMessage(message);
            return Ok(newMessage.MessageId);
        }

        public MessageWallPage Get(int page)
        {
            var messageList = _messageServices.GetAllWallMessage(page).ToList();
            foreach (var message in messageList)
            {
                var tmp = _messageServices.GetMessageComments(message.Id);
                message.Comments = tmp.ToList() ;
                message.RatingPositive = _messageServices.GetMessageRatings(message.Id, true).ToList();
                message.RatingNegative = _messageServices.GetMessageRatings(message.Id, false).ToList();
            }
            var count = _messageServices.GetAllMessageCount();
            var pages = (int)Math.Ceiling((double)count / 20);
            var items = messageList;
            return new MessageWallPage()
            {
                Count = count,
                Pages = pages,
                Items = items
            };
        }
        [Route("api/Message/Comment/{messageId:int}")]
        public IHttpActionResult PostComment(int messageId, [FromBody] MessageCommentViewModel comment)
        {
            var currentUser = _userServices.GetUserByEmail(User.Identity.Name);
            comment.UserId = currentUser.Id;
            comment.UserName = currentUser.UserDetail.FirstName;
            comment.UserAva = currentUser.UserDetail.ProfilePicture;
            var newComment = _messageServices.AddComment(messageId, comment);
            return Ok(newComment.Id);
        }
        [Route("api/Message/Rating/{messageId:int}")]
        public IHttpActionResult PostRating(int messageId, [FromBody] MessageRatingViewModel rating)
        {
            var currentUser = _userServices.GetUserByEmail(User.Identity.Name);
            rating.UserId = currentUser.Id;
            rating.UserName = currentUser.UserDetail.FirstName;
            rating.UserAva = currentUser.UserDetail.ProfilePicture;
            var newReting = _messageServices.AddMessageRating(messageId, rating);
            return Ok(newReting.Id);
        }
    }
}