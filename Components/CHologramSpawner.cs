using KitchenData;
using Unity.Collections;
using Unity.Entities;

namespace KitchenMyMod.Components
{
    public struct CHologramSpawner : IApplianceProperty, IAttachableProperty, IComponentData
    {
        public int playerID;
        public FixedString128 fileName;
        public bool InvertX;
        public bool InvertZ;
    }
}