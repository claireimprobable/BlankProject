using System;
using Assets.Gamelogic.Core;
using Assets.Gamelogic.Fire;
using Improbable.Fire;
using Improbable.Tree;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Gamelogic.Tree
{
    [WorkerType(WorkerPlatform.UnityWorker)]
    public class TreeBehaviour : MonoBehaviour
    {
        [Require] private TreeState.Writer tree;
        [Require] private Flammable.Writer flammable;

        [SerializeField] private FlammableBehaviour flammableInterface;

        private TreeStateMachine stateMachine;
        private Random Rng = new Random();

        private void Awake()
        {
            Debug.Log("CLAIRESLOG: Tree is Awake()!");
            flammableInterface = GetComponentIfUnassigned(flammableInterface);
        }

        private void OnEnable()
        {
            Debug.Log("CLAIRESLOG: Tree OnEnable(), creating tree with state=" + tree.Data.currentState);
            stateMachine = new TreeStateMachine(tree, flammableInterface, flammable);
            stateMachine.OnEnable(tree.Data.currentState);

            InvokeRepeating("SpontaneouslyCombust", 1.0f, 1.0f);
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

        private void SpontaneouslyCombust()
        {
            //var chance = Convert.ToDouble(SimulationSettings.TreeSpontaneouslyCombustChance);
            //var random = Math.Round(Rng.NextDouble(), 3);
            //var diff = chance - random;

            if (Random.Range(0f, 1f) < SimulationSettings.TreeSpontaneouslyCombustChance)
            {
                if (stateMachine.IsValidTransition(TreeFSMState.BURNY))
                {
                    Debug.Log("CLAIRESLOG: Tree SpontaneouslyCombust()! Fire time!");
                    stateMachine.TransitionTo(TreeFSMState.BURNY);
                }
            }      
        }
    }
}