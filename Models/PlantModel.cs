using System;
using System.Numerics;

namespace FarmerJob.Models
{
    public class PlantModel
    {
        public int Id { get; set; }
        public uint Hash { get; set; }
        public string Text { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public DateTime Created { get; set; }
        public Constants.PlantTypes Type { get; set; }
        public int GrowState { get; set; }
    }
}