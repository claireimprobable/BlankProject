using Assets.Gamelogic.Core;
using Assets.Gamelogic.Fire;
using Improbable.Fire;
using Improbable.Tree;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Assets.Gamelogic.Tree
{
    [WorkerType(WorkerPlatform.UnityWorker)]
    public class TreeBehaviour : MonoBehaviour
    {
        [Require] private TreeState.Writer tree;
        [Require] private Flammable.Writer flammable;

        [SerializeField] private FlammableBehaviour flammableInterface;

        private TreeStateMachine stateMachine;

        private void Awake()
        {
            flammableInterface = GetComponentIfUnassigned(flammableInterface);
        }

        private void OnEnable()
        {
            stateMachine = new TreeStateMachine(tree, flammableInterface, flammable);
            stateMachine.OnEnable(tree.Data.currentState);
        }

        private void OnDisable()
        {
            stateMachine.OnDisable();
        }

        private FlammableBehaviour GetComponentIfUnassigned(FlammableBehaviour componentReference)
        {
            if (componentReference == null)
            {
                componentReference = gameObject.GetComponent<FlammableBehaviour>();
                if (componentReference == null)
                {
                    Debug.LogError("Failed to get component reference on " + gameObject.name);
                }
                else
                {
                    Debug.LogWarning("Component reference " + componentReference + " on " + gameObject.name + " wasn't serialized but recovered via GetComponent call");
                }
            }
            return componentReference;
        }
    }
}