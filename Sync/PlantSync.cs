using System;
using System.Linq;
using System.Numerics;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using AltV.Net.Events;
using FarmerJob.Factory;
using FarmerJob.Models;

namespace FarmerJob
{
    public static class Sync
    {
        // Settings : 
        private const int MaxDistance = 300;
        private const int SyncTickObj = 5;           // Time in MS for updating nearby objects.
        
        // Variables : 
        private static DateTime _lastObjectsSynced = DateTime.Now.AddSeconds(SyncTickObj);
        
        
        private static void CreatePlantObj(FarmerPlayer player, PlantModel plant)
        {
            try
            {
                player.EmitLocked("Farmer:CreateObj", plant.Id, plant.Hash, plant.Position, plant.Rotation, plant.Text, plant.GrowState);
            }
            catch
            {
                // Catch exception here.
            }
        }
        
        private static void DeletePlantObjByIdTemp(FarmerPlayer player, int plantId)
        {
            try
            {
                player.EmitLocked("Farmer:DeleteObject", plantId);
            }
            catch
            {
                // Catch Exception here.
            }
        }
        
        // Update grow-state for players.
        private static void UpdatePlants(FarmerPlayer player)
        {
            try
            {
                foreach(PlantModel plants in player.NearbyPlants.ToList())
                    player.EmitLocked("Farmer:UpdateObj", plants.Id, plants.Text, plants.GrowState);
            }
            catch
            {
                // Catch exception here.
            }
        }
        
        public static void OnUpdate(object unused)
        {
            if (_lastObjectsSynced > DateTime.Now) return;
            foreach (FarmerPlayer players in Alt.GetAllPlayers().ToList())
            {
                foreach (PlantModel plants in Main.CurrentPlants.ToList())
                {
                    // if Position is over max distance = continue;
                    if (players.Position.Distance(plants.Position) < Sync.MaxDistance)
                    {
                        // Find nearby plant... if it don't exist.. add it!
                        PlantModel nearbyPlant = players.NearbyPlants.FirstOrDefault(x => x.Id == plants.Id);
                        
                        // Sync it with your nearby plant if needed.
                        // if nearbyPlant is not null = continue .. 
                        if (nearbyPlant is not null) { players.SyncNearbyPlants(nearbyPlant); continue; }
                        // Sync our Obj.
                        Sync.CreatePlantObj(players, plants);
                        players.NearbyPlants.Add(plants);
                    }
                    else
                    {
                        // Find nearby plant... if it exist.. remove it!
                        PlantModel nearbyPlant = players.NearbyPlants.FirstOrDefault(x => x.Id == plants.Id);
                        // if nearbyPlant is null = continue .. 
                        if (nearbyPlant is null) continue;
                        Sync.DeletePlantObjByIdTemp(players, plants.Id);
                        players.NearbyPlants.Remove(nearbyPlant);
                        
                    }
                }
            }
            _lastObjectsSynced = DateTime.Now.AddSeconds(SyncTickObj);
        }

        public static void ForcePlantSyncUpdate()
        {
            _lastObjectsSynced = DateTime.Now;
        }
        
        
        // OnTimerTick. - 60000 MS.
        public static void OnGrowingTimerCall(object unused)
        {
            foreach (var seeds in Main.CurrentPlants.ToList().Where(seeds => seeds.GrowState < 100)) {
                seeds.GrowState++;
                seeds.Text = "Plant[" + seeds.Id + "]\n" + seeds.GrowState + " %";
            }
            
            foreach(FarmerPlayer player in Alt.GetAllPlayers().ToList())
                Sync.UpdatePlants(player);
        }
    }
}