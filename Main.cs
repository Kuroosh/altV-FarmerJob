using System;
using System.Collections.Generic;
using System.Linq;
using AltV.Net;
using AltV.Net.Elements.Entities;
using FarmerJob.Factory;
using FarmerJob.Models;

namespace FarmerJob
{
    public static class Main
    {
        
        // Variables
        public static List<SeedAreaModel> CurrentSeedAreas = new List<SeedAreaModel>();
        public static readonly List<PlantModel> CurrentPlants = new List<PlantModel>();
        
        
        // Creates our plant .. should be understandable..
        public static void CreatePlant(FarmerPlayer player)
        {
            try
            {
                // Get LastInsertedId from Database - need Db Add..
                int plantId = 0;
                PlantModel plant = new PlantModel
                {
                    Id = plantId, Position = player.Position, Created = DateTime.Now, GrowState = 0, Text = "Plant[" + plantId+"]\n"+ 0 +" %"
                };
                CurrentPlants.Add(plant);
                Sync.CreatePlantObj(plant);
            }
            catch
            {
                // Catch Exception here.
            }
        }
        
        
        // OnTimerTick. - 60000 MS.
        public static void OnGrowingTimerCall(object unused)
        {
            foreach (var seeds in CurrentPlants.ToList().Where(seeds => seeds.GrowState < 100))
                seeds.GrowState++;
            
            foreach(FarmerPlayer player in Alt.GetAllPlayers().ToList())
                Sync.UpdatePlants(player);
        }
    }
}