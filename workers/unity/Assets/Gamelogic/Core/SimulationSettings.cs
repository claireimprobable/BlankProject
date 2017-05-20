using UnityEngine;

namespace Assets.Gamelogic.Core
{
    public static class SimulationSettings
    {
        public static readonly string PlayerPrefabName = "Player";
        public static readonly string PlayerCreatorPrefabName = "PlayerCreator";
        public static readonly string CubePrefabName = "Cube";
        public static readonly string TreePrefabName = "Tree";

        public static readonly int TargetClientFramerate = 60;
        public static readonly int TargetServerFramerate = 60;
        public static readonly int FixedFramerate = 20;

        public static readonly float HeartbeatCheckIntervalSecs = 3;
        public static readonly uint TotalHeartbeatsBeforeTimeout = 3;
        public static readonly float HeartbeatSendingIntervalSecs = 3;

        public static readonly float ClientConnectionTimeoutSecs = 7;

        public static readonly float PlayerCreatorQueryRetrySecs = 4;
        public static readonly float PlayerEntityCreationRetrySecs = 4;

        public static float ClaireIsImpatient = 1f;

        // Fire
        public static float FireSpreadInterval = 1f;
        public static float FireSpreadRadius = 6f;
        public static float FireSpreadProbability = 0.5f;
        public static float DefaultFireDamageInterval = 1f;
        public static int FireDamagePerTick = 1;
        public static float OnFireMovementSpeedIncreaseFactor = 3f;

        // Tree
        public static int TreeMaxHealth = 3;
        public static int TreeBurningTimeSecs = 10;
        public static int TreeStumpRegrowthTimeSecs = 300;
        public static int BurntTreeRegrowthTimeSecs = 600;
        public static float TreeIgnitionTimeBuffer = 0.4f;
        public static float TreeExtinguishTimeBuffer = 1f;
        public static float TreeCutDownTimeBuffer = 1f;
        public static float TreeSpontaneouslyCombustChance = 0.00001f;

        // Resource Prefab Paths
        public static string FireEffectPrefabPath = "Particles/Fire";

        // Component Updates
        public static int TransformUpdatesToSkipBetweenSends = 5;
        public static float AngleQuantisationFactor = 2f;

        public static readonly string DefaultSnapshotPath = Application.dataPath + "/../../../snapshots/default.snapshot";
    }
}