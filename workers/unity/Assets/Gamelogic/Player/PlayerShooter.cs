using UnityEngine;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;

namespace Assets.Gamelogic.Player
{
    [WorkerType(WorkerPlatform.UnityClient)]
    public class PlayerShooter : MonoBehaviour
    {
        [Require] public ClientConnection.Writer ClientConnectionWriter;
        [Require] private PlayerControls.Writer playerControlsWriter; //TODO: ??

        public ParticleSystem projectilePrefab;
        public float projectileVelocity = 1.0f;

        private Vector3 myCloudOffset = new Vector3(1, -4, 0);

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonUp("Fire1"))
            {
                ShootProjectile();
            }
        }

        private void ShootProjectile()
        {
            var projectileParticles = Instantiate(projectilePrefab, transform.position + myCloudOffset, transform.rotation);

            Debug.Log("ShootProjectile() - getting projectile game object");
            var projectileGameObject = projectileParticles.GetComponent<IgniteTreeOnCollision>();

            Debug.Log("ShootProjectile() - setting parent game object");
            projectileGameObject.ParentGameObject = gameObject;

            Debug.Log("ShootProjectile() - sending shoot to playercontrols");
            playerControlsWriter.Send(new PlayerControls.Update().AddShoot(new Shoot()));

            Destroy(projectileParticles.gameObject, 2.5f);
        }
    }
}