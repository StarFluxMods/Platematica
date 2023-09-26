using KitchenData;
using Unity.Collections;
using Unity.Entities;

namespace KitchenMyMod.Components
{
    public struct CHologramProjector : IApplianceProperty, IAttachableProperty, IComponentData
    {
        public FixedString128 Filename;
        public bool InvertX;
        public bool InvertZ;

        public CHologramProjector()
        {
            Filename = "";
            InvertX = false;
            InvertZ = false;
        }
    }
}