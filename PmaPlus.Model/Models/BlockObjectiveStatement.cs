﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PmaPlus.Model.Models
{
    public class BlockObjectiveStatement
    {
        public int Id { get; set; }
        public int BlockObjectiveId { get; set; }
        public string Statement { get; set; }

        public bool Achieved { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual PlayerBlockObjective BlockObjective { get; set; }
    }
}
