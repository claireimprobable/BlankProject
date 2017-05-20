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
        [SerializeField] private GameObject BurntTree;
        [SerializeField] private GameObject FireEffectPrefab;

        private GameObject fireEffect;

        private void OnEnable()
        {
            Debug.Log(string.Format("CLAIRESLOG: TreeVisualiser OnEnable()"));
            treeState.StatusUpdated.AddAndInvoke(OnStatusUpdated);
        }

        private void OnDisable()
        {
            treeState.StatusUpdated.Remove(OnStatusUpdated);
        }

        private void OnStatusUpdated(TreeStatus status)
        {
            Debug.Log(string.Format("CLAIRESLOG: TreeVisualiser status updated to: {0}", status));
            SetFireEffect(status == TreeStatus.BURNY);
            switch (status)
            {
                /*case TreeStatus.HEALTHY:
                    TransitionTo(HealthyTree);
                    break;*/
                case TreeStatus.BURNY:
                    //TransitionTo(BurntTree);
                    HideAllModels();
                    BurntTree.SetActive(true);
                    break;
            }
        }

        private void TransitionTo(GameObject newModel)
        {
            HideAllModels();
            newModel.SetActive(true);
        }

        private void HideAllModels()
        {
            HealthyTree.SetActive(false);
            BurntTree.SetActive(false);
        }



        private void SetFireEffect(bool isOnFire)
        {
            MaybeInitFire();
            fireEffect.SetActive(isOnFire);
        }

        private void MaybeInitFire()
        {
            if (fireEffect == null)
            {
                fireEffect = Instantiate(FireEffectPrefab, transform.position, transform.rotation, transform);
                fireEffect.SetActive(false);
            }
        }
    }
}