using Controllers;
using Kitchen;
using KitchenMods;
using Platematica.Components;

namespace Platematica.Systems
{
    public class HologramProjectorToggle : ItemInteractionSystem, IModSystem
    {
        protected override void Initialise()
        {
        }

        protected override bool IsPossible(ref InteractionData data)
        {
            return Require(data.Interactor, out CInputData input) && Require(data.Target, out CHologramProjector cHologramProjector);
        }

        protected override void Perform(ref InteractionData data)
        {
            if (Require(data.Interactor, out CInputData input) && Require(data.Target, out CHologramProjector cHologramProjector))
            {
                if ((input.State.StopMoving == ButtonState.Held))
                {
                    cHologramProjector.InvertX = !cHologramProjector.InvertX;
                }
                else
                {
                    cHologramProjector.InvertZ = !cHologramProjector.InvertZ;
                }
                EntityManager.SetComponentData<CHologramProjector>(data.Target, cHologramProjector);
            }
        }
    }
}