﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PmaPlus.Model.ViewModels.Player;

namespace PmaPlus.Model.ViewModels
{
    public class PlayersPage : Page
    {
        public IEnumerable<PlayerTableViewModel> Items { get; set; }
    }
}
