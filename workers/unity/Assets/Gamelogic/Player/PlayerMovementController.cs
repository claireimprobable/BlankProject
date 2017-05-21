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
        private int floorMask;

        void Awake()
        {
            playerRigidbody = GetComponent<Rigidbody>();
        }

        void Start()
        {
            // Create a layer mask for the floor layer.
            floorMask = LayerMask.GetMask("Floor");
        }
        void FixedUpdate()
        {
            // Store the input axes.
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            // Move the player around the scene.
            UpdatePosition(h, v);

            // Turn the player to face the mouse cursor.
            //UpdateRotation();
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

        void UpdateRotation()
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                playerRigidbody.MoveRotation(newRotation);
            }
        }
    }
}
