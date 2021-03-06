﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PmaPlus.Data.Repository.Iterfaces;
using PmaPlus.Model;
using PmaPlus.Model.Models;

namespace PmaPlus.Services.Services
{
    public class SiteSettingsServices
    {
        private readonly IPasswordHistoryRepository _passwordHistoryRepository;
        private readonly ITargetHistoryRepository _targetHistoryRepository;
        private readonly UserServices _userServices;

        public SiteSettingsServices(IPasswordHistoryRepository passwordHistoryRepository, ITargetHistoryRepository targetHistoryRepository, UserServices userServices)
        {
            _passwordHistoryRepository = passwordHistoryRepository;
            _targetHistoryRepository = targetHistoryRepository;
            _userServices = userServices;
        }

        #region Target

        public IQueryable<TargetHistory> GetTargetHistories()
        {
            return _targetHistoryRepository.GetAll().OrderByDescending(t => t.Id);
        }

        public void UpdateTarget(TargetHistory target)
        {
            if (target != null)
            {
                target.UpdateDate = DateTime.Now;
                _targetHistoryRepository.Add(target);
            }
        }

        public TargetHistory GetAtulalTareget()
        {
             
            
            return _targetHistoryRepository.GetAll().OrderByDescending(t=>t.Id).AsEnumerable().FirstOrDefault();
        }

        #endregion

        #region Pass History

        public IQueryable<PasswordHistory> GetPasswordHistory(User currentUser)
        {
            return _passwordHistoryRepository.GetMany(h => h.User.Email == currentUser.Email).OrderByDescending(p=>p.Id);
        }

        public void UpdatePassword(PasswordHistory password, string userEmail)
        {

            var user = _userServices.GetUserByEmail(userEmail);
            if (user != null)
            {
                password.ChangeAt = DateTime.Now;
                password.User = user;
                password.LoggedAt = user.LoggedAt;
                user.Password = password.Password;
                user.UpdateAt = DateTime.Now;
                _userServices.UpdateUser(user);
                _passwordHistoryRepository.Add(password);
            }
        }
        #endregion
    }
}
