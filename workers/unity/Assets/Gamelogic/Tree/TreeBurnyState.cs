using Assets.Gamelogic.FSM;
using Improbable.Fire;
using Improbable.Tree;

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
            flammable.Send(new Flammable.Update().SetCanBeIgnited(false));

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