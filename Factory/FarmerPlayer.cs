using System;
using System.Collections.Generic;
using AltV.Net;
using AltV.Net.Elements.Entities;
using FarmerJob.Models;

namespace FarmerJob.Factory
{
    public class FarmerPlayer : Player
    {

        public List<PlantModel> NearbyPlants = new List<PlantModel>();
        public FarmerPlayer(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
            
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