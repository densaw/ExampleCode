﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PmaPlus.Data.Repository.Iterfaces;
using PmaPlus.Model.Models;

namespace PmaPlus.Data.Repository
{
    class MessageGroupRepository : RepositoryBase<MessageGroup>, IMessageGroupRepository
    {
        public MessageGroupRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
