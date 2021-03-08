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
        private static Timer _growingTimer;
        private static Timer _onUpdate;
        
        // On Resource-Start
        public override void OnStart()
        {
            // Initialize _GrowingTimer after Resource-Start.
            // Standard : 60sek.
            _growingTimer = new Timer(Sync.OnGrowingTimerCall, null, 60 * 1000, 60 * 1000);
            _onUpdate = new Timer(Sync.OnUpdate, null, 50, 50);
        }
        // On Resource-Stop
        public override void OnStop() { }
        
        // Init-Entity factory.
        public override IEntityFactory<IPlayer> GetPlayerFactory() { return new FarmerPlayerFactory(); }
    }
}