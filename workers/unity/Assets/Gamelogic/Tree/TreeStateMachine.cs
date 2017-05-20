using System.Collections.Generic;
using Assets.Gamelogic.Fire;
using Assets.Gamelogic.FSM;
using UnityEngine;
using Improbable.Fire;
using Improbable.Tree;

namespace Assets.Gamelogic.Tree
{
    public class TreeStateMachine
    {
        public TreeFSMState CurrentState { get; private set; }

        private IDictionary<TreeFSMState, IList<TreeFSMState>> transitions;
        private IDictionary<TreeFSMState, IFsmState> states;

        private readonly TreeState.Writer tree;
        public TreeStateData Data;

        public TreeStateMachine(
            TreeState.Writer inTree,
            FlammableBehaviour flammableInterface,
            Flammable.Writer flammable
        )
        {
            Debug.Log("CLAIRESLOG: TreeStateMachine! ctor()");
            tree = inTree;

            var healthyState = new TreeHealthyState(this, flammable);
            var burningState = new TreeBurnyState(this, flammable);

            var stateList = new Dictionary<TreeFSMState, IFsmState>();
            stateList.Add(TreeFSMState.HEALTHY, healthyState);
            stateList.Add(TreeFSMState.BURNY, burningState);

            SetStates(stateList);

            var allowedTransitions = new Dictionary<TreeFSMState, IList<TreeFSMState>>();

            allowedTransitions.Add(TreeFSMState.HEALTHY, new List<TreeFSMState>()
            {
                TreeFSMState.BURNY
            });

            allowedTransitions.Add(TreeFSMState.BURNY, new List<TreeFSMState>()
            {
                TreeFSMState.HEALTHY
            });

            SetTransitions(allowedTransitions);
        }

        protected void SetStates(IDictionary<TreeFSMState, IFsmState> inStates)
        {
            states = inStates;
        }

        protected void SetTransitions(IDictionary<TreeFSMState, IList<TreeFSMState>> inTransitions)
        {
            transitions = inTransitions;
        }

        public void Tick()
        {
            states[CurrentState].Tick();
        }

        public void OnEnable(TreeFSMState initialState)
        {
            Debug.Log("CLAIRESLOG: TreeStateMachine! OnEnable()");
            Data = tree.Data.DeepCopy();
            CurrentState = initialState;
            states[CurrentState].Enter();
        }

        public void OnDisable()
        {
            states[CurrentState].Exit(true);
        }

        public void TransitionTo(TreeFSMState nextState)
        {
            Debug.Log(string.Format("CLAIRESLOG: TreeStateMachine TransitionTo({0})", nextState));
            if (IsValidTransition(nextState))
            {
                Debug.Log(string.Format("CLAIRESLOG: TreeStateMachine Changing state from {0} to {1}!", CurrentState, nextState));
                states[CurrentState].Exit(false);
                CurrentState = nextState;
                states[CurrentState].Enter();
            }
            else
            {
                Debug.LogErrorFormat("Invalid transition from {0} to {1} detected.", CurrentState, nextState);
            }
        }

        public bool IsValidTransition(TreeFSMState nextState)
        {
            return transitions[CurrentState].Contains(nextState);
        }

        public void TriggerTransition(TreeFSMState newState)
        {
            if (tree == null)
            {
                Debug.LogError("Trying to change state without authority.");
                return;
            }

            if (IsValidTransition(newState))
            {
                Debug.Log(string.Format("CLAIRESLOG: TreeStateMachine TriggerTransition({0}) is valid!", newState));

                Data.currentState = newState;

                var update = new TreeState.Update();
                update.SetCurrentState(Data.currentState);
                tree.Send(update);

                TransitionTo(newState);
            }
            else
            {
                Debug.LogErrorFormat("Tree: Invalid transition from {0} to {1} detected.", Data.currentState, newState);
            }
        }
    }
}