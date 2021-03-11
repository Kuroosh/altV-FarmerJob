using System;
using System.Collections.Generic;
using System.IO;
using AltV.Net;
using FarmerJob.Models;
using Newtonsoft.Json;

namespace FarmerJob
{
    public static class Constants
    {
        public enum PlantTypes : uint
        {
            Cereal = 3911344281,
            Potatoe = 3048255985,
            Tomatoe = 649223100,
            Cab = 3660027849,
            Cucumbe = 3529729772
        }

        private static List<DurtyMapModel> _seedMap = new List<DurtyMapModel>();
        public static void OnResourceStart()
        {
            try
            {
                if (File.Exists(Alt.Server.Resource.Path + "/maps/seedarea.json"))
                    _seedMap = JsonConvert.DeserializeObject<List<DurtyMapModel>>(File.ReadAllText(Alt.Server.Resource.Path + "/maps/seedarea.json"));
                else
                    Console.WriteLine("[CRITICAL-WARNING] : Map couldn't be found!");
                
                
                List<PlantTypes> plantTypes = new List<PlantTypes>();
                foreach (DurtyMapModel mapObj in _seedMap)
                {
                    Main.CurrentSeedAreas.Add(new SeedAreaModel{ Id = mapObj.Id, Position = mapObj.PositionRotation.Position, Rotation = mapObj.PositionRotation.Rotation, Radius = 2, Types = (PlantTypes)mapObj.Model });
                    if (plantTypes.Contains((PlantTypes) mapObj.Model)) continue;
                    Console.WriteLine("PlantTypes : " + (PlantTypes)mapObj.Model);
                    plantTypes.Add((PlantTypes)mapObj.Model);
                }
                foreach(var seedarea in Main.CurrentSeedAreas)                
                    Main.CreatePlant(null, seedarea);

            }
            catch (Exception ex)
            {
                Console.WriteLine("[EXCEPTION] : " +ex.Message);
                Console.WriteLine("[EXCEPTION] : " +ex.StackTrace);
            }
            
        }
        
    }
}