﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PmaPlus.Data.Repository.Iterfaces;
using PmaPlus.Model;
using PmaPlus.Model.Models;

namespace PmaPlus.Data.Repository
{
    class FACourseRepository : RepositoryBase<FACourse>, IFACourseRepository
    {
        public FACourseRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
