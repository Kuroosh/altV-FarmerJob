using System;
using System.Collections.Generic;
using System.Linq;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using FarmerJob.Factory;
using FarmerJob.Models;

namespace FarmerJob
{
    public static class Main
    {
        // Settings
        public const float MaxDistanceToOtherPlants = 2f; // Max Distance to other plants.
        
        // Variables
        public static readonly List<SeedAreaModel> CurrentSeedAreas = new List<SeedAreaModel>();
        public static readonly List<PlantModel> CurrentPlants = new List<PlantModel>();

        // Creates our plant .. should be understandable..
        // in that case.. we make debug : 
        private static int _plantId = 0;
        public static void CreatePlant(FarmerPlayer player, SeedAreaModel seedArea)
        {
            try
            {
                string name = player?.Name;
                // Get LastInsertedId from Database - need Db Add..
                PlantModel plant = new PlantModel
                {
                    Id = _plantId, Hash = (uint)seedArea.Types, Position = seedArea.Position, Rotation = seedArea.Rotation, Created = DateTime.Now, GrowState = 0, Text = seedArea.Types + "\n Owner : " + name + "\n[" + _plantId+"]\n"+ 0 +" %"
                };
                seedArea.InUse = true;
                CurrentPlants.Add(plant);
                _plantId++;
                // Force plant sync update.
                Sync.ForcePlantSyncUpdate();
            }
            catch
            {
                // Catch Exception here.
            }
        }
        
        // Removes our plant.. 
        public static void RemovePlant(PlantModel plant)
        {
            try
            {
                PlantModel plantObj = CurrentPlants.FirstOrDefault(x => x.Id == plant.Id);
                if (plantObj is null) return;
                CurrentPlants.Remove(plantObj);
                // Force plant sync update.
                foreach (var player in Alt.GetAllPlayers().ToList().Cast<FarmerPlayer>().Where(player =>
                    player.NearbyPlants.FirstOrDefault(x => x.Id == plantObj.Id) is not null))
                    player.EmitLocked("Farmer:DeleteObject", plantObj.Id);
            }
            catch
            {
                // Catch Exception here.
            }
        }
    }
}