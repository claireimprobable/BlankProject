using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Improbable.Tree;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Assets.Gamelogic.Tree
{
    [WorkerType(WorkerPlatform.UnityClient)]
    public class TreeModelVisualizer : MonoBehaviour
    {
        [Require] private TreeState.Reader treeState;

        [SerializeField] private GameObject HealthyTree;
        [SerializeField] private GameObject BurntTree;

        private void OnEnable()
        {
            Debug.Log("CLAIRESLOG: TreeModelVisualiser OnEnable()");
            treeState.ComponentUpdated.Add(UpdateVisualization);
            ShowTreeModel(treeState.Data.currentState);

        }

        private void OnDisable()
        {
            Debug.Log("CLAIRESLOG: TreeModelVisualiser OnDisable()");
            treeState.ComponentUpdated.Remove(UpdateVisualization);
        }

        private void UpdateVisualization(TreeState.Update newState)
        {
            ShowTreeModel(newState.currentState.Value);
        }

        private void ShowTreeModel(TreeFSMState currentState)
        {
            Debug.Log("CLAIRESLOG: TreeModelVisualiser ShowTreeModel() current state: " + currentState);
            switch (currentState)
            {
                case TreeFSMState.HEALTHY:
                    StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.ClaireIsImpatient, () =>
                    {
                        TransitionTo(HealthyTree);
                    }));
                    break;
                case TreeFSMState.BURNY:
                    StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.ClaireIsImpatient, () =>
                    {
                        Debug.Log("CLAIRESLOG: TreeModelVisualiser ShowTreeModel() TRANSITIONING TO BURNY!");
                        TransitionTo(BurntTree);
                    }));
                    break;
            }
        }

        private void TransitionTo(GameObject newModel)
        {
            Debug.Log("CLAIRESLOG: TreeModelVisualiser TransitionTo()");
            HideAllModels();
            newModel.SetActive(true);
        }

        private void HideAllModels()
        {
            Debug.Log("CLAIRESLOG: TreeModelVisualiser HideAllModels()");
            HealthyTree.SetActive(false);
            BurntTree.SetActive(false);
        }
    }
}