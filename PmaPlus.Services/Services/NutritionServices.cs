﻿using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PmaPlus.Data.Repository.Iterfaces;
using PmaPlus.Model;
using PmaPlus.Model.Models;
using PmaPlus.Model.ViewModels;

namespace PmaPlus.Services.Services
{
    public class NutritionServices
    {
        private readonly INutritionFoodTypeRepository _nutritionFoodTypeRepository;
        private readonly INutritionAlternativeRepository _nutritionAlternativeRepository;
        private readonly INutritionRecipeRepository _nutritionRecipeRepository;
        private readonly IFoodTypeToWhenRepository _foodTypeToWhenRepository;

        public NutritionServices(INutritionFoodTypeRepository nutritionFoodTypeRepository, INutritionAlternativeRepository nutritionAlternativeRepository, INutritionRecipeRepository nutritionRecipeRepository, IFoodTypeToWhenRepository foodTypeToWhenRepository)
        {
            _nutritionFoodTypeRepository = nutritionFoodTypeRepository;
            _nutritionAlternativeRepository = nutritionAlternativeRepository;
            _nutritionRecipeRepository = nutritionRecipeRepository;
            _foodTypeToWhenRepository = foodTypeToWhenRepository;
        }

        #region Nutrition Food Type 

        public bool FoodTypeExist(int id)
        {
            return _nutritionFoodTypeRepository.GetMany(f=>f.Id ==id).Any();
        }

        public IQueryable<NutritionFoodType> GetFoodTypes()
        {
            return _nutritionFoodTypeRepository.GetAll();
        }

        public NutritionFoodType AddFoodType(NutritionFoodType foodType,IList<MealTime> types)
        {
           
             var newFoodType =_nutritionFoodTypeRepository.Add(foodType);
            foreach (var type in types)
            {
                _foodTypeToWhenRepository.Add(new FoodTypeToWhen()
                {
                    FoodType = newFoodType,
                    Type = type
                });
            }

            return newFoodType;
        }

        public void UpdateFoodType(NutritionFoodType foodType, int id, IList<MealTime> when)
        {
            if (id != 0)
            {
                foodType.Id = id;
                _nutritionFoodTypeRepository.Update(foodType,id);

                var tempFood = _nutritionFoodTypeRepository.GetById(id);
                foreach (var type in when)
                {

                    if (!_foodTypeToWhenRepository.GetMany(f => f.FoodType.Id == tempFood.Id && f.Type == type).Any())
                    {
                        _foodTypeToWhenRepository.Add(new FoodTypeToWhen()
                        {
                            FoodType = tempFood,
                            Type = type
                        });
                    }
                }
                _foodTypeToWhenRepository.Delete(f => f.FoodType.Id == tempFood.Id && !when.Contains(f.Type));

            }
        }
        public void UpdateFoodType(NutritionFoodType foodType, int id)
        {
            foodType.Id = id;
            _nutritionFoodTypeRepository.Update(foodType,foodType.Id);
        }

        public void DeleteFoodType(int id)
        {
            _foodTypeToWhenRepository.Delete(f => f.FoodType.Id == id);
            _nutritionFoodTypeRepository.Delete(f=>f.Id == id);
        }

        public NutritionFoodType GetFoodTypeById(int id)
        {
            return _nutritionFoodTypeRepository.GetById(id);
        }
        public NutritionFoodType GetLastFoodType()
        {
            return _nutritionFoodTypeRepository.GetAll().LastOrDefault();
        }
        #endregion


        #region Nutritioan Alternatives
        public bool AlternativeExist(int id)
        {
            return _nutritionAlternativeRepository.GetMany(a => a.Id == id).Any();
        }

        public IQueryable<NutritionAlternative> GetAlternatives()
        {
            return _nutritionAlternativeRepository.GetAll();
        }

        public NutritionAlternative AddAlternative(NutritionAlternative alternative)
        {
            if (alternative == null)
            {
                return null;
            }

            return _nutritionAlternativeRepository.Add(alternative);
        }

        public void UpdateAlternative(NutritionAlternative alternative, int id)
        {
            if (id != 0)
            {
                alternative.Id = id;
                _nutritionAlternativeRepository.Update(alternative, id);
            }
        }

        public void DeleteAlternative(int id)
        {
            _nutritionAlternativeRepository.Delete(f => f.Id == id);
        }

        public NutritionAlternative GetAlternativeById(int id)
        {
            return _nutritionAlternativeRepository.GetById(id);
        }
        #endregion

        #region Nutrition Recipes
        public bool RecipeExist(int id)
        {
            return _nutritionRecipeRepository.GetMany(a => a.Id == id).Any();
        }

        public IQueryable<NutritionRecipe> GetRecipes()
        {
            return _nutritionRecipeRepository.GetAll();
        }

        public NutritionRecipe AddRecipe(NutritionRecipe recipe)
        {
            if (recipe == null)
            {
                return null;
            }

            return _nutritionRecipeRepository.Add(recipe);
        }

        public void UpdateRecipe(NutritionRecipe recipe, int id)
        {
            if (id != 0)
            {
                recipe.Id = id;
                _nutritionRecipeRepository.Update(recipe, id);
            }
        }

        public void DeleteRecipe(int id)
        {
            _nutritionRecipeRepository.Delete(f => f.Id == id);
        }

        public NutritionRecipe GetRecipeById(int id)
        {
            return _nutritionRecipeRepository.GetById(id);
        }



        public NutritionRecipe GetLastRecipe()
        {
            return _nutritionRecipeRepository.GetAll().LastOrDefault();
        }
        #endregion
    }
}
