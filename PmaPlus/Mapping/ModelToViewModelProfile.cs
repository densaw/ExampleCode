﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PmaPlus.Model.Models;
using PmaPlus.Model.ViewModels;
using PmaPlus.Model.ViewModels.Curriculum;
using PmaPlus.Model.ViewModels.Nutrition;
using PmaPlus.Model.ViewModels.Physio;
using PmaPlus.Model.ViewModels.PlayerAttribute;
using PmaPlus.Model.ViewModels.SiteSettings;
using PmaPlus.Model.ViewModels.Skill;
using PmaPlus.Model.ViewModels.SportsScience;
using PmaPlus.Model.ViewModels.ToDo;
using PmaPlus.Model.ViewModels.TrainingTeamMember;

namespace PmaPlus.Mapping
{
    class ModelToViewModelProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ModelToViewModelProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<CurriculumType, CurriculumTypeViewModel>();

            Mapper.CreateMap<SkillLevel, SkillLevelViewModel>();
            Mapper.CreateMap<SkillLevel, SkillLevelTableViewModel>();

            Mapper.CreateMap<SkillVideo, SkillVideoViewModel>();
            Mapper.CreateMap<SkillVideo, SkillVideoTableViewModel>()
                .ForMember(dest => dest.TrainingItemName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Duration));

            Mapper.CreateMap<BodyPart, PhysioBodyPartTableViewModel>();
            Mapper.CreateMap<BodyPart, PhysioBodyPartViewModel>();

            Mapper.CreateMap<PhysiotherapyExercise, PhysiotherapyExerciseTableViewModel>();
            Mapper.CreateMap<PhysiotherapyExercise, PhysiotherapyExerciseViewModel>();

            Mapper.CreateMap<NutritionFoodType, NutritionFoodTypeViewModel>()
                .ForMember(d => d.FoodTypes,o => o.MapFrom(s=> s.Types.Select(t=>t.Type).AsEnumerable()));
            Mapper.CreateMap<NutritionFoodType, NutritionFoodTypeTableViewModel>();

            Mapper.CreateMap<NutritionAlternative, NutritionAlternativeViewModel>();
            Mapper.CreateMap<NutritionAlternative, NutritionAlternativeTableViewModel>();

            Mapper.CreateMap<NutritionRecipe, NutritionRecipeViewModel>();
            Mapper.CreateMap<NutritionRecipe, NutritionRecipeTableViewModel>();

            Mapper.CreateMap<SportsScienceTest, SportsScienceTestViewModel>();
            Mapper.CreateMap<SportsScienceTest, SportsScienceTestTableViewModel>();

            Mapper.CreateMap<SportsScienceExercise, SportsScienceExerciseViewModel>();
            Mapper.CreateMap<SportsScienceExercise, SportsScienceExerciseTableViewModel>();

            Mapper.CreateMap<TargetHistory, TargetHistoryTableViewModel>();
            Mapper.CreateMap<TargetHistory, TargetViewModel>();

            Mapper.CreateMap<PasswordHistory, PasswordHistoryTableViewModel>();

            Mapper.CreateMap<Scenario, ScenarioViewModel>();
            Mapper.CreateMap<Scenario, ScenarioTableViewModel>();

            //club admin


            Mapper.CreateMap<PlayerAttribute, PlayerAttributeTableViewModel>();
            Mapper.CreateMap<PlayerAttribute, PlayerAttributeViewModel>();

            Mapper.CreateMap<User, TrainingTeamMemberViewModel>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.UserDetail.FirstName + s.UserDetail.LastName))
                .ForMember(d => d.TownCity, o => o.MapFrom(s => s.UserDetail.Address.TownCity))
                .ForMember(d => d.BirthDay, o => o.MapFrom(s => s.UserDetail.Birthday))
                .ForMember(d => d.Age, o => o.MapFrom(s => DateTime.Now.Year - s.UserDetail.Birthday.Value.Year))
                .ForMember(d => d.Mobile, o => o.MapFrom(s => s.UserDetail.Address.Mobile))
                .ForMember(d => d.CrbDbsExpiry, o => o.MapFrom(s => s.UserDetail.CrbDbsExpiry))
                .ForMember(d => d.FirstAidExpiry, o => o.MapFrom(s => s.UserDetail.FirstAidExpiry))
                .ForMember(d => d.LastLogin, o => o.MapFrom(s => s.LoggedAt))
                .ForMember(d => d.ProfilePicture, o => o.MapFrom(s => s.UserDetail.ProfilePicture));

            Mapper.CreateMap<ToDo, ToDoViewModel>();

        }
    }
}
