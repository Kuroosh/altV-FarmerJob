using System.Numerics;

namespace FarmerJob.Models
{
    public class Properties
    {
        public bool IsDynamic { get; set; }
        public string TextureVariation { get; set; }
        public int LodDistance { get; set; }
        public bool IsPositionFrozen { get; set; }
        public bool IsOnFire { get; set; }
        public bool IsVisible { get; set; }
        public string LightColor { get; set; }
        public bool IsLocked { get; set; }
        public bool IsCollisionless { get; set; }
        public bool IsDoor { get; set; }
    }
    public class Creator
    {
        public string Name { get; set; }
        public uint SocialClubId { get; set; }
    }
    public class Meta
    {
        public Creator Creator { get; set; }
        public Creator LastEditor { get; set; }
        public string CreatedTime { get; set; }
        public string LastEditTime { get; set; }
    }
    public class PositionRotation
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
    }
    public class DurtyMapModel
    {
        public uint Model { get; set; }
        public Properties Properties { get; set; }
        public string Id { get; set; }
        public Meta Meta { get; set; }
        public PositionRotation PositionRotation { get; set; }
    }
}