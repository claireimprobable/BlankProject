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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Tree"))
            {
                // TODO: can we do this better? This is a bit of a hack (projectile can issue commands..)
                var clientConnectionWriter = GameObject.FindGameObjectWithTag("Player").GetComponent<SendClientConnection>().ClientConnectionWriter;

                Debug.Log("Attempting to ignite entity: " + other.gameObject.EntityId());
                SpatialOS.Commands.SendCommand(clientConnectionWriter, TreeState.Commands.Ignite.Descriptor, new IgniteRequest(), other.gameObject.EntityId())
                         .OnSuccess(response => Debug.Log("Successful ignition: " + response))
                         .OnFailure(failure => Debug.Log("Failed ignition: " + failure.ErrorMessage));
            }
        }
    }
}