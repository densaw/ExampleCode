﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PmaPlus.Model.Enums;

namespace PmaPlus.Model.ViewModels.SportsScience
{
    public class SportsScienceTestViewModel
    {
        public int Id { get; set; }
        [Required]
        public SportsScienceType Type { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public ZScoreFormulaType ZScoreFormula { get; set; }

        public string Measure { get; set; }

        public string LowValue { get; set; }

        public string HightValue { get; set; }

        public string NationalAverage { get; set; }

        public string Video { get; set; }
    }
}
