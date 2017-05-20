using Assets.Gamelogic.Core;
using UnityEngine;

namespace Assets.Gamelogic.UI
{
    public static class ResourceRegistry
    {
        private static GameObject firePrefab;
        public static GameObject FirePrefab { get { return firePrefab ?? (firePrefab = Resources.Load<GameObject>(SimulationSettings.FireEffectPrefabPath)); } }
    }
}
