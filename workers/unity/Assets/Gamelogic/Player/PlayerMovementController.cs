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

        private float movementSpeed = 10.0f;
        private Vector3 movementDirection;
        private Rigidbody playerRigidbody;
        private float camRayLength = 100f;

        void Awake()
        {
            playerRigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            // Store the input axes.
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            // Move the player around the scene.
            UpdatePosition(h, v);
        }

        void UpdatePosition(float h, float v)
        {
            // Set the movement vector based on the axis input.
            movementDirection.Set(h, 0f, v);

            // Normalise the movement vector and make it proportional to the speed per second.
            movementDirection = movementDirection.normalized * movementSpeed * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            playerRigidbody.MovePosition(transform.position + movementDirection);
        }
    }
}
