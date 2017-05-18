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
        //[SerializeField] private Mesh[] meshes;

        private void OnEnable()
        {
            Debug.Log("CLAIRESLOG: I am a tree! OnEnable()");
            //SetupTreeModel();
            treeState.ComponentUpdated.Add(UpdateVisualization);
            ShowTreeModel(treeState.Data.currentState);

        }

        private void OnDisable()
        {
            Debug.Log("CLAIRESLOG: I am a tree! OnDisable()");
            treeState.ComponentUpdated.Remove(UpdateVisualization);
        }

        /*private void SetupTreeModel()
        {
            //var treeModel = meshes[(int)treeState.Data.treeType];
            var treeModel = meshes[0];
            HealthyTree.GetComponent<MeshFilter>().mesh = treeModel;
            //HealthyTree = Instantiate(HealthyTree, );
        }*/

        private void UpdateVisualization(TreeState.Update newState)
        {
            ShowTreeModel(newState.currentState.Value);
        }

        private void ShowTreeModel(TreeFSMState currentState)
        {
            Debug.Log("CLAIRESLOG: I am a tree! ShowTreeModel()");
            switch (currentState)
            {
                case TreeFSMState.HEALTHY:
                    StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.TreeExtinguishTimeBuffer, () =>
                    {
                        TransitionTo(HealthyTree);
                    }));
                    break;
                case TreeFSMState.BURNY:
                    StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.TreeIgnitionTimeBuffer, () =>
                    {
                        TransitionTo(HealthyTree);
                    }));
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
    }
}