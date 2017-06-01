using Improbable.Player;
using Improbable.Tree;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Assets.Gamelogic.Player
{
    [WorkerType(WorkerPlatform.UnityClient)]
    public class IgniteTreeOnCollision : MonoBehaviour
    {
        [Require] private TreeState.Reader treeStateReader;

        public GameObject ParentGameObject { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("OnTriggerEnter");
            if (other.CompareTag("Tree"))
            {
                Debug.Log("OnTriggerEnter, tag is Tree");
                //var clientConnectionWriter = ParentGameObject.ClientConnectionWriter;
                var clientConnectionWriter = ParentGameObject.GetComponent<PlayerShooter>().ClientConnectionWriter;
                Debug.Log("Attempting to ignite entity: " + other.gameObject.EntityId());
                SpatialOS.Commands.SendCommand(clientConnectionWriter, TreeState.Commands.Ignite.Descriptor, new IgniteRequest(), other.gameObject.EntityId())
                         .OnSuccess(response => Debug.Log("Successful ignition: " + response))
                         .OnFailure(failure => Debug.Log("Failed ignition: " + failure.ErrorMessage));
            }
        }
    }
}