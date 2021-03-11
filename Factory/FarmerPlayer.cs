using System;
 using System.Collections.Generic;
 using System.Linq;
 using AltV.Net;
 using AltV.Net.Elements.Entities;
 using FarmerJob.Models;
 
 namespace FarmerJob.Factory
 {
     public class FarmerPlayer : Player
     {
 
         public readonly List<PlantModel> NearbyPlants = new List<PlantModel>();
         public int Seeds { get; set; }
         public IVehicle FarmerVehicle { get; set; }
         public IVehicle FarmerTrailer { get; set; }
         public void SyncNearbyPlants(PlantModel plant)
         {
             PlantModel localPlant = this.NearbyPlants.FirstOrDefault(x => x.Id == plant.Id);
             if (localPlant is null) return;
             localPlant = plant;
         }
         public FarmerPlayer(IntPtr nativePointer, ushort id) : base(nativePointer, id)
         {
             Seeds = 10;
         }
     }
     
     public class FarmerPlayerFactory : IEntityFactory<IPlayer>
     {
         public IPlayer Create(IntPtr playerPointer, ushort id)
         {
             return new FarmerPlayer(playerPointer, id);
         }
     }
 }