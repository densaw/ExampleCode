﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PmaPlus.Model.Models;

namespace PmaPlus.Model.ViewModels.Diary
{
    public class DiaryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool AllDay { get; set; }
        public string Url { get; set; }
    }
}
