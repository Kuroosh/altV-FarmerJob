using System.Numerics;

namespace FarmerJob.Models
{
    public class SeedAreaModel
    {
        public string Id { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public bool InUse { get; set; }
        public int Radius { get; set; }
        public Constants.PlantTypes Types { get; set; }
    }
}