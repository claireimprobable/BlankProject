using Assets.Gamelogic.FSM;
using Improbable.Fire;
using Improbable.Tree;
using UnityEngine;

namespace Assets.Gamelogic.Tree
{
    public class TreeHealthyState : IFsmState
    {
        protected TreeStateMachine Owner { get { return owner; } }

        private readonly Flammable.Writer flammable;
        private readonly TreeStateMachine owner;

        public TreeHealthyState(TreeStateMachine inOwner, Flammable.Writer inFlammable)
        {
            owner = inOwner;
            flammable = inFlammable;
        }

        public void Enter()
        {
            Debug.Log("CLAIRESLOG: TreeHealthyState Enter()");

            flammable.Send(new Flammable.Update().SetCanBeIgnited(true));

            flammable.ComponentUpdated.Add(OnFlammableUpdated);
        }

        public void Tick()
        {

        }

        public void Exit(bool disabled)
        {
            flammable.ComponentUpdated.Remove(OnFlammableUpdated);
        }

        private void OnFlammableUpdated(Flammable.Update update)
        {
            Debug.Log("CLAIRESLOG: TreeHealthyState OnFlammableUpdated()");
            if (HasBeenIgnited(update))
            {
                owner.TriggerTransition(TreeFSMState.BURNY);
            }
        }

        private bool HasBeenIgnited(Flammable.Update flammableUpdate)
        {
            return flammableUpdate.isOnFire.HasValue && flammableUpdate.isOnFire.Value;
        }
    }
}