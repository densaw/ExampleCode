﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PmaPlus.Model.ViewModels.Curriculum
{
    public class CurriculumStatementViewModel
    {
        public int Id { get; set; }
        public virtual ICollection<int> Roles { get; set; }
        public bool ChooseBlock { get; set; }
        public bool ChooseWeek { get; set; }
        public bool ChooseSession { get; set; }
        public string Statement { get; set; }
    }
}
