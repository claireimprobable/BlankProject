using UnityEngine;

namespace Assets.Gamelogic.Core
{
    public static class SimulationSettings
    {
        public static readonly string PlayerPrefabName = "Player";
        public static readonly string PlayerCreatorPrefabName = "PlayerCreator";
        public static readonly string TreePrefabName = "Tree";

        public static readonly int TargetClientFramerate = 60;
        public static readonly int TargetServerFramerate = 60;
        public static readonly int FixedFramerate = 20;

        public static readonly float PlayerCreatorQueryRetrySecs = 4;
        public static readonly float PlayerEntityCreationRetrySecs = 4;

        // Heartbeat
        public static readonly float HeartbeatCheckIntervalSecs = 3;
        public static readonly uint TotalHeartbeatsBeforeTimeout = 3;
        public static readonly float HeartbeatSendingIntervalSecs = 3;
        public static readonly float ClientConnectionTimeoutSecs = 7;

        // Player
        public static float MovementSpeed = 10.0f;

        // Fire
        public static float FireSpreadInterval = 5f;
        public static float FireSpreadRadius = 6f;
        public static float FireSpreadProbability = 0.04f;

        // Tree
        public static int TreeBurningTimeSecs = 10;
        public static int TreeStumpRegrowthTimeSecs = 300;
        public static int BurntTreeRegrowthTimeSecs = 600;
        public static float TreeSpontaneouslyCombustChance = 0.0000001f;
        public static float TreeDensity = 0.2f;

        // Resource Prefab Paths
        public static string FireEffectPrefabPath = "Particles/Fire";

        // Component Updates
        public static int TransformUpdatesToSkipBetweenSends = 5;
        public static float AngleQuantisationFactor = 2f;

        public static readonly string DefaultSnapshotPath = Application.dataPath + "/../../../snapshots/default.snapshot";
    }
}