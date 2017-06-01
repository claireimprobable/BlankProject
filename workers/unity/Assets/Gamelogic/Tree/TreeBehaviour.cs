using System.Collections.Generic;
using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Improbable;
using Improbable.Entity.Component;
using Improbable.Tree;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Gamelogic.Tree
{
    [WorkerType(WorkerPlatform.UnityWorker)]
    public class TreeBehaviour : MonoBehaviour
    {
        [Require] private TreeState.Writer treeStateWriter;

        private void OnEnable()
        {
            treeStateWriter.CommandReceiver.OnIgnite.RegisterResponse(OnIgnite);
        }

        void FixedUpdate()
        {
            if (treeStateWriter.Data.status == TreeStatus.HEALTHY)
            {
                SpontaneousCombustion();
            }

            if (treeStateWriter.Data.status == TreeStatus.BURNY)
            {
                StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.FireSpreadInterval, SpreadFire));

                StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.TreeBurningTimeSecs, () =>
                {
                    ChangeStatus(TreeStatus.BALDY);
                }));
            }
        }

        public void SpontaneousCombustion()
        {
            var probability = SimulationSettings.TreeSpontaneouslyCombustChance;
            var randomRange = Random.Range(0f, 1f);
            if (randomRange < probability)
            {
                treeStateWriter.Send(new TreeState.Update().SetStatus(TreeStatus.BURNY));
            }
        }

        public void SpreadFire()
        {
            var probabilityToSpread = SimulationSettings.FireSpreadProbability;
            if (Random.Range(0f, 1f) < probabilityToSpread)
            {
                var neighbors = FindNeighbors(SimulationSettings.FireSpreadRadius);
                Debug.Log("Gonna spread some fire! Found " + neighbors.Length + " neighbors.");
                if (neighbors.Length != 0)
                {
                    var randomNeighbor = neighbors[Random.Range(0, neighbors.Length)];
                    SpreadToNeighbor(randomNeighbor.gameObject.EntityId());
                }
            }
        }

        private Collider[] FindNeighbors(float radius)
        {
            var neighbors = new List<Collider>(Physics.OverlapSphere(transform.position, radius));

            // Remove this tree so we only consider neighbors.
            for (var i = 0; i < neighbors.Count; i++)
            {
                if (neighbors[i].gameObject.Equals(gameObject))
                {
                    neighbors.RemoveAt(i);
                    break;
                }
            }
            return neighbors.ToArray();
        }

        private void SpreadToNeighbor(EntityId neighborEntityId)
        {
           SpatialOS.Commands.SendCommand(treeStateWriter, TreeState.Commands.Ignite.Descriptor, new IgniteRequest(), neighborEntityId);
        }

        private IgniteResponse OnIgnite(IgniteRequest request, ICommandCallerInfo callerInfo)
        {
            if (treeStateWriter.Data.status == TreeStatus.HEALTHY)
            {
                treeStateWriter.Send(new TreeState.Update().SetStatus(TreeStatus.BURNY));
            }
            return new IgniteResponse();
        }

        private void ChangeStatus(TreeStatus newStatus)
        {
            // Since coroutines do not stop when a component is disabled, we need this check.
            if (treeStateWriter == null || !treeStateWriter.HasAuthority)
            {
                Debug.Log("Worker lost authority during state wait time.");
                return;
            }
            treeStateWriter.Send(new TreeState.Update().SetStatus(newStatus));
        }
    }
}