﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PmaPlus.Data.Repository.Iterfaces;
using PmaPlus.Model.Models;

namespace PmaPlus.Services.Services
{
    public class ToDoServices
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserServices _userServices;
        public ToDoServices(IToDoRepository toDoRepository, IUserRepository userRepository, UserServices userServices)
        {
            _toDoRepository = toDoRepository;
            _userRepository = userRepository;
            _userServices = userServices;
        }

        public bool ToDoExist(int id)
        {
            return _toDoRepository.GetMany(t => t.Id == id).Any();
        }

        public IEnumerable<ToDo> GetToDos(string userEmail)
        {
            return _toDoRepository.GetMany(t => t.UserDetail.User.Email.ToLower() == userEmail.ToLower());
        } 

        public ToDo AddToDo(ToDo toDo, string userEmail)
        {
            if (_userServices.UserExist(userEmail))
            {
                toDo.UserDetail = _userServices.GetUserByEmail(userEmail).UserDetail;
                return _toDoRepository.Add(toDo);
            }
            return null;
        }

        public void UpdateToDo(ToDo toDo, int id)
        {
            if (id != 0)
            {
                toDo.Id = id;
                toDo.UserDetail = _toDoRepository.GetById(id).UserDetail;
                _toDoRepository.Update(toDo,toDo.Id);
            }
        }

        public void DeleteToDo(int id)
        {
            if (id != 0)
            {
                _userRepository.Delete(t => t.Id == id);
            }
        }
    }
}
