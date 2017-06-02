using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
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

        public GameObject Projectile;
        public ParticleSystem ProjectileAsParticleSystem;
       
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
            //var projectileParticles = Instantiate(projectile, transform.position + myCloudOffset, transform.rotation);
            Debug.Log("Setting projectile active");
            Projectile.SetActive(true);

            var projectileGameObject = Projectile.GetComponent<IgniteTreeOnCollision>();
            projectileGameObject.ParentGameObject = gameObject;

            playerControlsWriter.Send(new PlayerControls.Update().AddShoot(new Shoot()));

            StartCoroutine(TimerUtils.WaitAndPerform(ProjectileAsParticleSystem.main.duration, () =>
            {
                Debug.Log("Setting projectile no longer active");
                Projectile.SetActive(false);
            }));
        }
    }
}