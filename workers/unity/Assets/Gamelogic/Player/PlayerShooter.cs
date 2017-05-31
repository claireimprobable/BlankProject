using UnityEngine;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;


namespace Assets.Gamelogic.Player
{
    [WorkerType(WorkerPlatform.UnityClient)]
    public class PlayerShooter : MonoBehaviour
    {
        //[Require] private ClientConnection.Writer ClientConnectionWriter;
        [Require] private PlayerControls.Writer playerControlsWriter; //TODO: ??

        public ParticleSystem projectilePrefab;
        public float projectileVelocity = 1.0f;

        // Update is called once per frame
        void Update()
        {
            Debug.Log("PlayerShooter Update()");
            if (Input.GetButtonUp("Fire1"))
            {
                Debug.Log("Shooting!");
                ShootProjectile();
            }
        }

        private void ShootProjectile()
        {
            ParticleSystem projectile = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation);
            //projectile.velocity = transform.forward * projectileVelocity;
            //projectile.AddForce(-transform.up * projectileVelocity);
            playerControlsWriter.Send(new PlayerControls.Update().AddShoot(new Shoot()));

            
            Destroy(projectile.gameObject, 2.5f);
        }
    }
}