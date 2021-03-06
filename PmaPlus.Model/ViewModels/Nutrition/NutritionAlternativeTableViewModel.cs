﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PmaPlus.Model.ViewModels.Nutrition
{
    public class NutritionAlternativeTableViewModel
    {
        public int Id { get; set; }
        public string BadItemName { get; set; }
        public string AlternativeName { get; set; }
        public string BadItemDescription { get; set; }
        public string AlternativeDescription { get; set; }
        public string VideoLink { get; set; }
    }
}
