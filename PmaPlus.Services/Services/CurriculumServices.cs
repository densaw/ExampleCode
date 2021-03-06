﻿using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PmaPlus.Data.Repository.Iterfaces;
using PmaPlus.Model.Models;
using PmaPlus.Model.ViewModels.Curriculum;
using PmaPlus.Model;

namespace PmaPlus.Services
{
    public class CurriculumServices
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IStatementRolesRepository _statementRolesRepository;
        private readonly ICurriculumStatementRepository _curriculumStatementRepository;
        private readonly IScenarioRepository _scenarioRepository;

        public CurriculumServices(ICurriculumRepository curriculumRepository, ISessionRepository curriculumSessionRepository, IClubRepository clubRepository, ICurriculumStatementRepository curriculumStatementRepository, IStatementRolesRepository statementRolesRepository, IScenarioRepository scenarioRepository)
        {
            _curriculumRepository = curriculumRepository;
            _sessionRepository = curriculumSessionRepository;
            _clubRepository = clubRepository;
            _curriculumStatementRepository = curriculumStatementRepository;
            _statementRolesRepository = statementRolesRepository;
            _scenarioRepository = scenarioRepository;
        }



        #region Curriculums
        //TODO:Curriculum Progress
        public decimal GetProgress(int curriculumId)
        {
            var curriclum = _curriculumRepository.GetById(curriculumId);
            return 0;

        }


        public void SetCurriculumToLive(int curriculumId,bool isLive)
        {
            var curriclum = _curriculumRepository.GetById(curriculumId);
            curriclum.IsLive = isLive;
            _curriculumRepository.Update(curriclum,curriclum.Id);
        }



        public bool CurriculumExist(int id)
        {
            return _curriculumRepository.GetMany(c => c.Id == id).Any();
        }

        public Curriculum GetCurriculumById(int id)
        {
            return _curriculumRepository.GetById(id);
        }

        public IEnumerable<CurriculumsList> GetClubCurriculumsList(int clubId)
        {
            return from curri in _curriculumRepository.GetMany(c => c.Club.Id == clubId)
                   select new CurriculumsList()
                   {
                       Id = curri.Id,
                       Name = curri.Name
                   };
        }

        public IQueryable<Curriculum> GetClubCurriculums(int clubId)
        {
            return _curriculumRepository.GetMany(c => c.Club.Id == clubId);
        }

        public Curriculum AddCurriculum(Curriculum curriculum, int clubId)
        {
            var club = _clubRepository.GetById(clubId);

            if (club == null)
            {
                return null;
            }

            curriculum.Club = club;

            var newCurriculum = _curriculumRepository.Add(curriculum);

            return newCurriculum;

        }

        public void UpdateCurriculum(Curriculum curriculum, int id)
        {
            curriculum.Id = id;
            _curriculumRepository.Update(curriculum, curriculum.Id);
        }

        public void DeleteCurriculum(int id)
        {
            _sessionRepository.Delete(s => s.Curriculum.Id == id);
            _curriculumRepository.Delete(c => c.Id == id);
        }


        #endregion

        #region Sessions

        public bool SessionExist(int id)
        {
            return _sessionRepository.GetMany(s => s.Id == id).Any();
        }



        public IQueryable<Session> GetSessions(int curriculumId)
        {
            return _sessionRepository.GetMany(s => s.Curriculum.Id == curriculumId);
        }

        public Session GetSessionById(int id)
        {
            return _sessionRepository.GetById(id);
        }

        public Session AddSession(Session session, int curriculumId, IList<int> scenariosList)
        {
            var curriculum = _curriculumRepository.GetById(curriculumId);
            if (curriculum != null)
            {
                session.Curriculum = curriculum;
                var newSession = _sessionRepository.Add(session);
                if (newSession != null)
                {
                    foreach (var scenario in scenariosList)
                    {

                        if (_scenarioRepository.GetMany(s => s.Id == scenario).Any())
                        {
                           newSession.Scenarios.Add(_scenarioRepository.GetById(scenario));

                        }

                    }

                    _sessionRepository.Update(newSession,newSession.Id);
                }


                return null;
            }

            return null;
        }

        public void UpdateSession(Session session, int id, IList<int> scenariosList)
        {
            session.Id = id;

            
            var tempSession = _sessionRepository.GetById(id);


            if (tempSession == null)
            {
                return;
            }

            session.Scenarios = tempSession.Scenarios;
            
            
            foreach (var scenario in scenariosList)
            {
                if (!tempSession.Scenarios.Any(s => s.Id == scenario))
                {
                    tempSession.Scenarios.Add(_scenarioRepository.GetById(scenario));
                }
            }

          

            foreach (var item in tempSession.Scenarios.Where(scenario => !scenariosList.Contains(scenario == null ? 0 : scenario.Id)).ToList())
            {
                tempSession.Scenarios.Remove(item);
            }
            
           


            
            _sessionRepository.Update(session, session.Id);

        }

        public void UpdateSession(Session session, int id)
        {
            _sessionRepository.Update(session, id);
        }
        public void DeleteSession(int id)
        {
            _sessionRepository.Delete(s => s.Id == id);
        }

        #endregion


        #region Statements

        public bool StatementExist(int id)
        {
            return _curriculumStatementRepository.GetMany(s => s.Id == id).Any();
        }

        public IQueryable<CurriculumStatement> GetCurriculumStatements(int clubId)
        {
            return _curriculumStatementRepository.GetMany(s => s.Club.Id == clubId);
        }


        public CurriculumStatement GetCurriculumStatementById(int id)
        {
            return _curriculumStatementRepository.GetById(id);
        }

        public void AddCurricululmStatment(CurriculumStatement statement, IList<Role> rolesList, int clubId)
        {
            statement.Club = _clubRepository.GetById(clubId);
            var newStatment = _curriculumStatementRepository.Add(statement);

            if (newStatment != null)
            {
                foreach (var role in rolesList)
                {
                    _statementRolesRepository.Add(new StatementRoles()
                    {
                        Role = role,
                        Statement = newStatment,
                        CurriculumStatementId = newStatment.Id
                    });
                }
            }
        }

        public void UpdateCurriculumStatment(CurriculumStatement statement, IList<Role> rolesList, int id)
        {
            if (id != 0)
            {
                statement.Id = id;
                _curriculumStatementRepository.Update(statement, id);

                var tempStatement = _curriculumStatementRepository.GetById(id);

                if (tempStatement == null)
                {
                    return;
                }

                foreach (var rolee in rolesList)
                {
                    if (!_statementRolesRepository.GetMany(s => s.CurriculumStatementId == tempStatement.Id && s.Role == rolee).Any())
                    {
                        _statementRolesRepository.Add(new StatementRoles()
                        {
                            Role = rolee,
                            Statement = tempStatement,
                            CurriculumStatementId = tempStatement.Id
                        });
                    }
                }
                _statementRolesRepository.Delete(s => s.CurriculumStatementId == tempStatement.Id && !rolesList.Contains(s.Role));
            }
        }


        public void DeleteCurriculumStatement(int id)
        {
            if (id != 0)
            {
                _statementRolesRepository.Delete(sr => sr.CurriculumStatementId == id);
                _curriculumStatementRepository.Delete(s => s.Id == id);
            }
        }

        #endregion

    }
}
