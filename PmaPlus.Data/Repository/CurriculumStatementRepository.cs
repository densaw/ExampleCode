﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PmaPlus.Data.Repository.Iterfaces;
using PmaPlus.Model.Models;

namespace PmaPlus.Data.Repository
{
    public class CurriculumStatementRepository : RepositoryBase<CurriculumStatement>, ICurriculumStatementRepository
    {
        public CurriculumStatementRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
