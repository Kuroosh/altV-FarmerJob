using System;
using System.Linq;
using System.Numerics;
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
        public static void PlantSeed(FarmerPlayer player)
        {
            // remove item from inventory : 
            // player.RemoveItem ( or what ever.. ) 
            if (player.Seeds <= 0) return;
            player.Seeds--;
            Console.WriteLine("My Seeds are now - : " + player.Seeds);

            // if player is near other plant - return.
            if (Main.CurrentPlants.FirstOrDefault(x => player.Position.Distance(x.Position) < Main.MaxDistanceToOtherPlants) is not null) {
                Console.WriteLine("You can't plant this here... ");
                return;
            }
            // if player is near not near a seed-area - return.
            SeedAreaModel currentSeedArea = Main.CurrentSeedAreas.FirstOrDefault(x => player.Position.Distance(x.Position) <= x.Radius && !x.InUse);
            if( currentSeedArea is null)
                Console.WriteLine("You are not in a Seed-Area.");
            
            else Main.CreatePlant(player, currentSeedArea);
        }

        [ClientEvent("Farmer:PickUpPlant")]
        public static void PickUpPlant(FarmerPlayer player)
        {
            if (!player.IsInVehicle || player.FarmerVehicle is null || player.FarmerTrailer is null) return;
            IVehicle attachedVeh = player.FarmerTrailer.AttachedTo;
            if (attachedVeh is null)
            {
                Console.WriteLine("You need your Trailer to pickup items!");
                return;
            }
            else if (attachedVeh != player.Vehicle)
            {
                Console.WriteLine("You need your Trailer on your car!");
                return;
            }
            Console.WriteLine("Called Pickup");
            PlantModel nearbyPlant = Main.CurrentPlants.FirstOrDefault(x => player.FarmerTrailer.Position.Distance(x.Position) < Main.MaxDistanceToOtherPlants);
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

        // gets called when a job got accepted.
        [ClientEvent(("Farmer:AcceptJob"))]
        public static void AcceptJob(FarmerPlayer player)
        {
            player.FarmerVehicle?.Remove();
            player.FarmerTrailer?.Remove();
            player.FarmerVehicle = Alt.CreateVehicle(VehicleModel.Tractor2, new Vector3(2232.224f, 5164.747f, 59.373657f), new Vector3(0,0,30f));
            player.FarmerTrailer = Alt.CreateVehicle(VehicleModel.RakeTrailer, new Vector3(2228.8616f, 5160f, 58.59851f), new Vector3(0,0,30f));
            // give him the items you want.
            player.Seeds = 30;
            player.EmitLocked("Farmer:ShowAreas", true);
        }
        
        // gets called when a job got stopped.
        [ClientEvent("Farmer:StopJob")]
        public static void StopJob(FarmerPlayer player)
        {
            // remove items ? it depends on you.
            player.FarmerVehicle?.Remove();
            player.FarmerTrailer?.Remove();
            player.Seeds = 0;
            player.EmitLocked("Farmer:ShowAreas", false);
        }

        // gets called when a player enters a car.
        [ScriptEvent(ScriptEventType.PlayerEnterVehicle)]
        public void PlayerEnterVehicle(Vehicle vehicle, FarmerPlayer player, byte seat)
        {
            player.EmitLocked("Farmer:IsCollecting", true);
        }
        
        // gets called when a player leaves a car.
        [ScriptEvent(ScriptEventType.PlayerLeaveVehicle)]
        public void PlayerLeaveVehicle(Vehicle vehicle, FarmerPlayer player, byte seat)
        {
            player.EmitLocked("Farmer:IsCollecting", false);
        }
        
        // gets called when a player disconnect.
        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public void OnPlayerDiconnect(FarmerPlayer player, string reason)
        {
            player.FarmerVehicle?.Remove();
            player.FarmerTrailer?.Remove();
        }


        
        
        
        // Debug
        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public static void OnPlayerConnect(FarmerPlayer player, string reason)
        {
            player.Position = Main.CurrentSeedAreas[0].Position;
            player.Model = (uint)PedModel.Franklin;
            Console.WriteLine("Counted : " + Main.CurrentSeedAreas.Count);
            foreach (SeedAreaModel _Area in Main.CurrentSeedAreas)
                player.EmitLocked("Farmer:CreateArea", _Area.Id, _Area.Position, _Area.Rotation, 0, _Area.Radius);
        }

        [Command("pos")]
        public void GetPos(FarmerPlayer player)
        {
            Console.WriteLine(player.Position.X + ", " + player.Position.Y + ", " + player.Position.Z);
        }

        [Command("createarea")]
        public void CreateArea(FarmerPlayer player, int rot, int radius)
        {
            Main.CurrentSeedAreas.Add(new SeedAreaModel{ Position = player.Position, InUse = false, Radius = radius});
            player.EmitLocked("Farmer:CreateArea", 0, player.Position, rot, radius);
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
            //PickUpPlant(player);
        } 
        [Command("acceptjob")]
        public void AcceptJobCMD(FarmerPlayer player)
        {
            AcceptJob(player);
        } 
        [Command("stopjob")]
        public void StopJobCMD(FarmerPlayer player)
        {
            StopJob(player);
        } 
    }
}