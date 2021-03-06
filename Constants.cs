using System;
using System.Linq;
using AltV.Net;
using AltV.Net.Elements.Entities;
using FarmerJob.Factory;
using FarmerJob.Models;

namespace FarmerJob
{
    public static class Constants
    {
        // Settings : 
        private const int SyncTickObj = 5;           // Time in MS for updating nearby objects.
        
        // Variables : 
        private static DateTime LastObjectsSynced = DateTime.Now.AddSeconds(SyncTickObj);
        public static void OnUpdate(object unused)
        {
            if (LastObjectsSynced > DateTime.Now) return;
            foreach (FarmerPlayer players in Alt.GetAllPlayers().ToList())
            {
                LastObjectsSynced = DateTime.Now.AddSeconds(SyncTickObj);
                foreach (PlantModel plants in Main.CurrentPlants.ToList())
                {
                    // if Position is over max distance = continue;
                    if (players.Position.Distance(plants.Position) < Sync.MaxDistance)
                    {
                        // Find nearby plant... if it don't exist.. add it!
                        PlantModel nearbyPlant = players.NearbyPlants.FirstOrDefault(x => x.Id == plants.Id);
                        // if nearbyPlant is not null = continue .. 
                        if (nearbyPlant is not null) continue;
                        // Sync our Obj.
                        Sync.CreatePlantObj(plants);
                        players.NearbyPlants.Add(plants);
                    }
                    else
                    {
                        // Find nearby plant... if it don't exist.. add it!
                        PlantModel nearbyPlant = players.NearbyPlants.FirstOrDefault(x => x.Id == plants.Id);
                        // if nearbyPlant is null = continue .. 
                        if (nearbyPlant is null) continue;
                        Sync.DeletePlantObjById(plants.Id);
                    }
                }
            }
        }
    }
}