﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PmaPlus.Model.ViewModels.Curriculum
{
    public class SessionPage : Page
    {
        public IEnumerable<SessionTableViewModel> Items { get; set; }
    }
}
