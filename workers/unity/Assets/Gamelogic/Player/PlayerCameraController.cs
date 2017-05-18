using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

[WorkerType(WorkerPlatform.UnityClient)]
public class PlayerCameraController : MonoBehaviour {

    [Require] private ClientConnection.Writer ClientConnectionWriter;

    private Camera playerCamera;
    private Vector3 playerCameraOffset;

    void OnEnable()
    {
        playerCamera = Camera.main;
        playerCameraOffset = playerCamera.transform.position - gameObject.transform.position;
    }

    void OnDisable()
    {
        playerCamera = null;
    }

    void FixedUpdate()
    {
        if (playerCamera != null)
        {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, gameObject.transform.position + playerCameraOffset, 0.5f * Time.deltaTime);
        }
    }
}