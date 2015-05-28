﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PmaPlus.Data.Repository.Iterfaces;
using PmaPlus.Model.Models;

namespace PmaPlus.Data.Repository
{
    public class ScenarioSessionRepository : RepositoryBase<ScenarioSession>, IScenarioSessionRepository
    {
        public ScenarioSessionRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}