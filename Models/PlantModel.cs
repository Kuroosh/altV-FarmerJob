using System;
using System.Numerics;

namespace FarmerJob.Models
{
    public class PlantModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Vector3 Position { get; set; }
        public DateTime Created { get; set; }
        public int GrowState { get; set; }
    }
}