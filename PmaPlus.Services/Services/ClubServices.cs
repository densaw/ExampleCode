﻿using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PmaPlus.Data;
using PmaPlus.Data.Infrastructure;
using PmaPlus.Data.Repository.Iterfaces;
using PmaPlus.Model;
using PmaPlus.Model.Enums;
using PmaPlus.Model.Models;
using PmaPlus.Model.ViewModels;
using PmaPlus.Model.ViewModels.Club;
using PmaPlus.Services.Services;

namespace PmaPlus.Services
{
    public class ClubServices
    {
        private readonly IClubRepository _clubRepository;
        private readonly IWelfareOfficerRepository _welfareOfficerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IChairmanRepository _chairmanRepository;
        private readonly IClubAdminRepository _clubAdminRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserDetailRepository _userDetailRepository;
        public ClubServices(IClubRepository clubRepository,
                                IWelfareOfficerRepository welfareOfficerRepository,
                                IChairmanRepository chairmanRepository,
                                IClubAdminRepository clubAdminRepository,
                                IUserDetailRepository userDetailRepository,
                                IUserRepository userRepository,
                                IAddressRepository addressRepository)
        {
            _clubRepository = clubRepository;
            _welfareOfficerRepository = welfareOfficerRepository;
            _chairmanRepository = chairmanRepository;
            _clubAdminRepository = clubAdminRepository;
            _userDetailRepository = userDetailRepository;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
        }

        public IQueryable<ClubTableViewModel> GetClubsTableViewModels()
        {
            var clubs = from club in _clubRepository.GetAll()
                        select new ClubTableViewModel()
                        {
                            Id = club.Id,
                            Name = club.Name,
                            TownCity = club.Address.TownCity,
                            Coaches = club.Coaches.Count,
                            Players = club.Players.Count,
                            PiPay = club.Players.Count(p => p.Status == UserStatus.Active),
                            LastLogin = club.ClubAdmin.User.LoggedAt,
                            Status = club.Status
                        };
            return clubs;
        }

        public bool ClubIsExist(int id)
        {
           return _clubRepository.GetMany(c => c.Id == id).Any();
        }
        public AddClubViewModel GetClubById(int id)
        {
            Club club = _clubRepository.GetById(id);
            
            if (club == null)
                return null;

            AddClubViewModel model = new AddClubViewModel()
            {
                Id = club.Id,
                Name = club.Name,
                Logo = club.Logo,
                Status = club.Status,
                ClubAdminName = club.ClubAdmin.User.UserDetail.FirstName,
                ClubAdminEmail = club.ClubAdmin.User.UserName,
                ClubAdminPassword = club.ClubAdmin.User.Password,
                Background = club.Background,
                Established = club.Established,
                Telephone = club.Address.Telephone,
                Mobile = club.Address.Mobile,
                Address1 = club.Address.Address1,
                Address2 = club.Address.Address2,
                Address3 = club.Address.Address3,
                TownCity = club.Address.TownCity,
                PostCode = club.Address.PostCode,
                Chairman = club.Chairman.Name,
                ChairmanEmail = club.Chairman.Email,
                ChairmanTelephone = club.Chairman.Telephone,
                ColorTheme = club.ColorTheme
            };
            return model;
        }

        public Club AddClub(AddClubViewModel club)
        {

            var tempClub = new Club()
            {
                Name = club.Name,
                Logo = club.Logo,
                ColorTheme = club.ColorTheme,
                Background = club.Background,
                Status = club.Status,
                Established = club.Established,
                CreateAt = DateTime.Now,
                Address = new Address()
                {
                    Telephone = club.Telephone,
                    Mobile = club.Mobile,
                    Address1 = club.Address1,
                    Address2 = club.Address2,
                    Address3 = club.Address3,
                    TownCity = club.TownCity,
                    PostCode = club.PostCode,
                },
                ClubAdmin = new ClubAdmin()
                {
                    User = new User()
                    {
                        UserName = club.ClubAdminEmail,
                        Role = Role.ClubAdmin,
                        Email = club.ClubAdminEmail,
                        Password = club.ClubAdminPassword,
                        CreateAt = DateTime.Now,
                        UpdateAt = DateTime.Now,
                        LoggedAt = DateTime.Now,
                        UserDetail = new UserDetail()
                        {
                            FirstName = club.ClubAdminName,
                        }

                    }
                },
                Chairman = new Chairman()
                {
                    Name = club.Chairman,
                    Email = club.ChairmanEmail,
                    Telephone = club.ChairmanTelephone,
                },
                
            };
            var newClub =_clubRepository.Add(tempClub);
            newClub.ClubAdmin.Club = newClub;
            newClub.ClubAdmin.User.UserDetail.User = newClub.ClubAdmin.User;
            _clubRepository.Update(newClub,newClub.Id);
            return newClub;
        }
        public void UpdateClub(Club club, int id)
        {
            if (club.Id > 0)
            {

                _clubRepository.Update(club, id);
            }

        }
        public void UpdateClub(AddClubViewModel club, int id)
        {
            if (club.Id > 0)
            {
               
                var entity = _clubRepository.GetById(id);

                entity.Id = id;

                entity.Name = club.Name;
                entity.Logo = club.Logo;
                entity.ColorTheme = club.ColorTheme;
                entity.Background = club.Background;
                entity.Status = club.Status;
                entity.Established = club.Established;
                entity.Address.Telephone = club.Telephone;
                entity.Address.Mobile = club.Mobile;
                entity.Address.Address1 = club.Address1;
                entity.Address.Address2 = club.Address2;
                entity.Address.Address3 = club.Address3;
                entity.Address.TownCity = club.TownCity;
                entity.Address.PostCode = club.PostCode;
                entity.ClubAdmin.User.UserDetail.FirstName = club.ClubAdminName;
                entity.ClubAdmin.User.UserName = club.ClubAdminEmail;
                entity.ClubAdmin.User.Email = club.ClubAdminEmail;
                entity.ClubAdmin.User.Password = club.ClubAdminPassword;
                entity.Chairman.Name = club.Chairman;
                entity.Chairman.Email = club.ChairmanEmail;
                entity.Chairman.Telephone = club.ChairmanTelephone;

                _clubRepository.Update(entity, id);
            }

        }

        public void DeleteClub(int id)
        {
            var club = _clubRepository.GetById(id);

            _addressRepository.Delete(club.Address);
            _chairmanRepository.Delete(club.Chairman);
 
            _addressRepository.Delete(club.ClubAdmin.User.UserDetail.Address);
            _userDetailRepository.Delete(club.ClubAdmin.User.UserDetail);
            _userRepository.Delete(club.ClubAdmin.User);
            //_clubAdminRepository.Delete(club.ClubAdmin);
            

            //TODO: maybe will left something
            _clubRepository.Delete(club);
        }
        public InfoBoxViewModel GetClubLoggedThisWeek()
        {
            int lastWeek = DateTool.GetThisWeek() > 1 ? DateTool.GetThisWeek() - 1 : 52;
            int thisWeek = DateTool.GetThisWeek();

            int clubsThisWeek =
                _clubRepository.GetMany(c => SqlFunctions.DatePart("week", c.CreateAt) == thisWeek).Count();
            int clubsLastWeek =
                _clubRepository.GetMany(c => SqlFunctions.DatePart("week", c.CreateAt) == lastWeek).Count();
            int percent;
            string progress = "netral";
            if (clubsLastWeek == 0)
            {
                percent = 100;
            }
            else
            {
                percent = (int)(((double)clubsThisWeek / clubsLastWeek) * 100);
                if (percent > 100)
                {
                    percent = 100;
                }
            }
            if (clubsThisWeek - clubsLastWeek > 0)
                progress = "up";
            else if (clubsThisWeek - clubsLastWeek < 0)
                progress = "down";


            return new InfoBoxViewModel()
            {
                Amount = clubsThisWeek,
                Progress = progress,
                Percentage = percent
            };
        }

        public IList<int> GetClubsLoggedForLast_Weeks(int times = 10)
        {
            List<int> usersList = new List<int>();

            int thisYear = DateTime.Now.Year;
            int thisWeek = DateTool.GetThisWeek();
            
            for (int i = 0; i < times; i++)
            {
                if (thisWeek < 1)
                {
                    thisWeek = 52;
                    thisYear--;
                }

                usersList.Add(_clubRepository.GetMany(c =>
                          SqlFunctions.DatePart("week", c.CreateAt) == thisWeek &&
                         c.CreateAt.Year == thisYear).Count());
                thisWeek--;
            }
            usersList.Reverse();
            return usersList;

        }
    }
}
