using Assets.Gamelogic.Core;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Player;
namespace Assets.Gamelogic.Player
{
    [WorkerType(WorkerPlatform.UnityClient)]
    public class PlayerMovementController : MonoBehaviour
    {
        [Require]
        private ClientConnection.Writer ClientConnectionWriter;

        private Vector3 movementDirection;
        private Rigidbody playerRigidbody;

        void Awake()
        {
            playerRigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            UpdatePosition(h, v);
        }

        void UpdatePosition(float h, float v)
        {
            // Set the movement vector based on the axis input.
            movementDirection.Set(h, 0f, v);

            // Normalise the movement vector and make it proportional to the speed per second.
            movementDirection = movementDirection.normalized * SimulationSettings.MovementSpeed * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            playerRigidbody.MovePosition(transform.position + movementDirection);
        }
    }
}