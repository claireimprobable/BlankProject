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
            }
        }

        public void SpontaneousCombustion()
        {
            var probability = SimulationSettings.TreeSpontaneouslyCombustChance;
            var randomRange = Random.Range(0f, 1f);
            if (randomRange < probability)
            {
                Debug.Log(string.Format("Random number {0} is less than probability {1}. Setting TreeStatus to BURNY!", randomRange, probability));
                treeStateWriter.Send(new TreeState.Update().SetStatus(TreeStatus.BURNY));
            }
        }

        public void SpreadFire()
        {
            var probabilityToSpread = SimulationSettings.FireSpreadProbability;
            if (Random.Range(0f, 1f) < probabilityToSpread)
            {
                Collider[] neighbors = FindNeighbors(SimulationSettings.FireSpreadRadius);
                Debug.Log("Gonna spread some fire! Found " + neighbors.Length + " neighbors.");
                if (neighbors.Length != 0)
                {
                    Collider randomNeighbor = neighbors[Random.Range(0, neighbors.Length)];
                    SpreadToNeighbor(randomNeighbor.gameObject.EntityId());
                }
            }
        }

        private Collider[] FindNeighbors(float radius)
        {
            List<Collider> neighbors = new List<Collider>(
                Physics.OverlapSphere(transform.position, radius));

            // Remove this tree so we only consider neighbors.
            for (int i = 0; i < neighbors.Count; i++)
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
            Debug.Log("Gonna spread some fire!");
            SpatialOS.Commands.SendCommand(treeStateWriter, TreeState.Commands.Ignite.Descriptor, new IgniteRequest(), neighborEntityId);
        }

        private IgniteResponse OnIgnite(IgniteRequest request, ICommandCallerInfo callerInfo)
        {
            Debug.Log("Processed ignite request.");
            if (treeStateWriter.Data.status == TreeStatus.HEALTHY)
            {
                treeStateWriter.Send(new TreeState.Update().SetStatus(TreeStatus.BURNY));
            }
            return new IgniteResponse();
        }
    }
}