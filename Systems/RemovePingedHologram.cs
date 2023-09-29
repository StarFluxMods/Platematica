using Kitchen;
using Unity.Entities;

namespace Platematica.Systems
{
    [UpdateBefore(typeof(MakePing))]
    [UpdateBefore(typeof(ShowPingedApplianceInfo))]
    public class RemovePingedHologram : InteractionSystem
    {
        protected override InteractionType RequiredType
        {
            get
            {
                return InteractionType.Notify;
            }
        }

        protected override InteractionMode RequiredMode
        {
            get
            {
                return InteractionMode.Appliances;
            }
        }

        protected override bool IsPossible(ref InteractionData data)
        {
            return Require(data.Target, out CAppliance cAppliance) && cAppliance.ID == Mod.HologramProjector.ID;
        }

        protected override void Perform(ref InteractionData data)
        {
            EntityManager.DestroyEntity(data.Target);
        }
    }
}