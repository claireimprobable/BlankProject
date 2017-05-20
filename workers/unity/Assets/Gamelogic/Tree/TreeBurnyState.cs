using Assets.Gamelogic.FSM;
using Improbable.Fire;
using Improbable.Tree;
using UnityEngine;

namespace Assets.Gamelogic.Tree
{
    public class TreeBurnyState : IFsmState
    {
        private readonly Flammable.Writer flammable;
        private readonly TreeStateMachine owner;

        public TreeBurnyState(TreeStateMachine inOwner, Flammable.Writer inFlammable)
        {
            flammable = inFlammable;
        }

        public void Enter()
        {
            Debug.Log("CLAIRESLOG: TreeBurnyState Enter()");
            flammable.Send(new Flammable.Update().SetCanBeIgnited(false));

            flammable.ComponentUpdated.Add(OnFlammableUpdated);
            flammable.Send(new Flammable.Update().SetIsOnFire(true));


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
            Debug.Log("CLAIRESLOG: TreeBurnyState OnFlammableUpdated()");
            if (HasBeenExtinguished(update))
            {
                owner.TriggerTransition(TreeFSMState.HEALTHY);
            }
        }

        private bool HasBeenExtinguished(Flammable.Update flammableUpdate)
        {
            return flammableUpdate.isOnFire.HasValue && !flammableUpdate.isOnFire.Value;
        }
    }
}