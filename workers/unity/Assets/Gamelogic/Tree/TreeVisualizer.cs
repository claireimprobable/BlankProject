using Improbable.Tree;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Assets.Gamelogic.Tree
{
    [WorkerType(WorkerPlatform.UnityClient)]
    public class TreeVisualizer : MonoBehaviour
    {
        [Require] private TreeState.Reader treeState;

        [SerializeField] private GameObject HealthyTree;
        [SerializeField] private GameObject BaldyTree;
        [SerializeField] private GameObject FireEffectPrefab;

        private GameObject _fireEffect;

        private void OnEnable()
        {
            treeState.StatusUpdated.AddAndInvoke(OnStatusUpdated);
        }

        private void OnDisable()
        {
            treeState.StatusUpdated.Remove(OnStatusUpdated);
        }

        private void OnStatusUpdated(TreeStatus status)
        {
            SetFireEffect(status == TreeStatus.BURNY);
            switch (status)
            {
                case TreeStatus.BURNY:
                    SetFireEffect(true);
                    break;
                case TreeStatus.BALDY:
                    SetFireEffect(false);
                    HideAllModels();
                    BaldyTree.SetActive(true);
                    break;
            }
        }

        private void HideAllModels()
        {
            HealthyTree.SetActive(false);
            BaldyTree.SetActive(false);
        }

        private void SetFireEffect(bool isOnFire)
        {
            MaybeInitFire();
            _fireEffect.SetActive(isOnFire);
        }

        private void MaybeInitFire()
        {
            if (_fireEffect == null)
            {
                _fireEffect = Instantiate(FireEffectPrefab, transform.position, transform.rotation, transform);
                _fireEffect.SetActive(false);
            }
        }
    }
}