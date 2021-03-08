using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks.Sources;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using FarmerJob.Factory;
using FarmerJob.Models;

namespace FarmerJob
{
    public class Events : IScript
    {
        [ClientEvent(("Farmer:PlantSeed"))]
        private static void PlantSeed(FarmerPlayer player)
        {
            // remove item from inventory : 
            // player.RemoveItem ( or what ever.. ) 
            if (player.Seeds <= 0) return;
            player.Seeds--;
            Console.WriteLine("My Seeds are now - : " + player.Seeds);
            
            if (Main.CurrentPlants.FirstOrDefault(x => player.Position.Distance(x.Position) < Main.MaxDistanceToOtherPlants) is not null)
                Console.WriteLine("You can't plant this here... ");
            
            else if(Main.CurrentSeedAreas.FirstOrDefault(x => player.Position.Distance(x.Position) <= x.Radius) is null)
                Console.WriteLine("You are not in a Seed-Area.");
            
            else Main.CreatePlant(player);
        }

        [ClientEvent("Farmer:PickUpPlant")]
        public static void PickUpPlant(FarmerPlayer player)
        {
            PlantModel nearbyPlant = Main.CurrentPlants.FirstOrDefault(x => player.Position.Distance(x.Position) < Main.MaxDistanceToOtherPlants);
            if (nearbyPlant is null) Console.WriteLine("You can't pick this up here...");
            else
            {
                // add item to inventory : player.bla
                player.Seeds++;
                Console.WriteLine("My Seeds are now + : " + player.Seeds);
                Console.WriteLine("Removed Plant Id : " + nearbyPlant.Id);
                Main.RemovePlant(nearbyPlant);
            }
        }

        // Debug
        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public static void OnPlayerConnect(FarmerPlayer player, string reason)
        {
            player.Position = new Vector3(0,0,72);
            player.Model = (uint)PedModel.Franklin;
        }

        [Command("pos")]
        public void GetPos(FarmerPlayer player)
        {
            Console.WriteLine(player.Position.X + ", " + player.Position.Y + ", " + player.Position.Z);
        }

        [Command("createarea")]
        public void CreateArea(FarmerPlayer player, int rot, int radius)
        {
            Main.CurrentSeedAreas.Add(new SeedAreaModel{ Position = player.Position, PlantCount = 0, Radius = radius});
            player.EmitLocked("Farmer:CreateArea", player.Position, rot, radius);
        }

        [Command("veh")]
        public void CreateVehicle(FarmerPlayer player, string vehicleName)
        {
            IVehicle veh = Alt.CreateVehicle(Alt.Hash(vehicleName), player.Position, player.Rotation);
        }

        [Command("plant")]
        public void PlantSeedCmd(FarmerPlayer player)
        {
            PlantSeed(player);
        } 
        [Command("pickup")]
        public void PickupSeedCmd(FarmerPlayer player)
        {
            PickUpPlant(player);
        } 
    }
}