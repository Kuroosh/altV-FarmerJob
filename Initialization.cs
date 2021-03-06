using System;
using System.Threading;
using AltV.Net;
using AltV.Net.Elements.Entities;
using FarmerJob.Factory;

namespace FarmerJob
{
    public class FarmerResource : Resource
    {
        // Creating Async Timer - so it will not affect our in-game sync.
        private static Timer _GrowingTimer;
        private static Timer _OnUpdate;
        public override void OnStart()
        {
            // Initialize _GrowingTimer after Resource-Start.
            // Standard : 60sek.
            _GrowingTimer = new Timer(Main.OnGrowingTimerCall, null, 60 * 1000, 60 * 1000);
            _OnUpdate = new Timer(Constants.OnUpdate, null, 50, 50);
        }
        public override void OnStop()
        {
        }
        
        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new FarmerPlayerFactory();
        }
    }
}