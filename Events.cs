using AltV.Net;
using AltV.Net.Elements.Entities;
using FarmerJob.Factory;

namespace FarmerJob
{
    public class Events
    {
        [ClientEvent(("Farmer:PlantSeed"))]
        public static void PlantSeed(FarmerPlayer player)
        {
            // remove item from inventory : 
            // player.RemoveItem ( or what ever.. ) 
            Main.CreatePlant(player);
        }
    }
}