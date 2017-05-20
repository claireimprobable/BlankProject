using Assets.Gamelogic.Core;
using Improbable.Tree;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Gamelogic.Tree
{
    [WorkerType(WorkerPlatform.UnityWorker)]
    public class TreeBehaviour : MonoBehaviour
    {
        [Require] private TreeState.Writer treeStateWriter;

        // for spontaneous combustion
        void FixedUpdate()
        {
            Debug.Log(string.Format("TreeBehaviour: FixedUpdate(), my current status is {0}", treeStateWriter.Data.status));

            var probability = SimulationSettings.TreeSpontaneouslyCombustChance;
            var randomRange = Random.Range(0f, 1f);
            if (randomRange < probability && treeStateWriter.Data.status == TreeStatus.HEALTHY)
            {
                Debug.Log(string.Format("Random number {0} is less than probability {1}. Setting TreeStatus to BURNY!", randomRange, probability));
                treeStateWriter.Send(new TreeState.Update().SetStatus(TreeStatus.BURNY));
            }
        }
    }
}