using Improbable.Core;
using Improbable.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;

public class TransformSender : MonoBehaviour {

    [Require]
    private WorldTransform.Writer WorldTransform;
    private WorldTransform.Update WorldTransformUpdate;
    private Coordinates position;
    private Rigidbody currentRigidBody;

    private void Awake()
    {
        currentRigidBody = gameObject.GetComponent<Rigidbody>();
        WorldTransformUpdate = new WorldTransform.Update();
    }

    void FixedUpdate()
    {
        if (WorldTransform.HasAuthority)
        {
            position.X = (double)currentRigidBody.transform.position.x;
            position.Y = (double)currentRigidBody.transform.position.y;
            position.Z = (double)currentRigidBody.transform.position.z;

            WorldTransformUpdate.SetPosition(position);
            WorldTransform.Send(WorldTransformUpdate);
        }
    }
}