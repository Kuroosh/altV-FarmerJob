using System.Linq;
using System.Numerics;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using FarmerJob.Factory;
using FarmerJob.Models;

namespace FarmerJob
{
    public static class Sync
    {
        // Settings : 
        public const int MaxDistance = 30;
        
        
        public static void CreatePlantObj(PlantModel plant)
        {
            try
            {
                // Need to implement for streaming range & custom obj streamer... but for now i will do just to see how it works.
                foreach (IPlayer player in Alt.GetAllPlayers())
                    player.EmitLocked("Farmer:CreateObj", plant.Id, plant.Text, plant.GrowState);
            }
            catch
            {
                // Catch exception here.
            }
        }
        
        public static void DeletePlantObjById(int Id)
        {
            try
            {
                PlantModel deletion = Main.CurrentPlants.FirstOrDefault(x => x.Id == Id);
                if (deletion is null) return;
                Main.CurrentPlants.Remove(deletion);
            }
            catch
            {
                // Catch exception here.
            }
        }

        // Update grow-state for players.
        public static void UpdatePlants(FarmerPlayer player)
        {
            try
            {
                foreach(PlantModel plants in player.NearbyPlants.ToList())
                    player.EmitLocked("Farmer:UpdateObj", plants.Id, plants.GrowState);
            }
            catch
            {
                // Catch exception here.
            }
        }
    }
}