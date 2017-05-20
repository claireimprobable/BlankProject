using Assets.Gamelogic.UI;
using Improbable.Fire;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Assets.Gamelogic.Fire
{
    [WorkerType(WorkerPlatform.UnityClient)]
    public class FlammableVisualizer : MonoBehaviour
    {
        [Require] private Flammable.Reader flammable;

        private GameObject fireEffectInstance;
        private ParticleSystem fireEffectparticleSystem;

        private void CreateFireEffectInstance()
        {
            Debug.Log("CLAIRESLOG:CreateFireEffectInstance()");
            fireEffectInstance = Instantiate(ResourceRegistry.FirePrefab, transform);
            fireEffectInstance.transform.localPosition = Vector3.zero;
            fireEffectparticleSystem = fireEffectInstance.GetComponent<ParticleSystem>();    
        }

        private void OnEnable()
        {
            if (fireEffectInstance == null)
            {
                CreateFireEffectInstance();
            }
            flammable.ComponentUpdated.Add(OnComponentUpdated);
            UpdateParticleSystem(flammable.Data.isOnFire);
        }

        private void OnDisable()
        {
            flammable.ComponentUpdated.Remove(OnComponentUpdated);
        }

        private void OnComponentUpdated(Flammable.Update update)
        {
            if (update.isOnFire.HasValue)
            {
                UpdateParticleSystem(update.isOnFire.Value);
            }
        }

        private void UpdateParticleSystem(bool enabled)
        {
            if (enabled)
            {
                fireEffectparticleSystem.Play();
            }
            else
            {
                fireEffectparticleSystem.Stop();
            }
        }
    }
}