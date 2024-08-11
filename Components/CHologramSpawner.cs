using KitchenData;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace Platematica.Components
{
    public struct CHologramSpawner : IApplianceProperty, IModComponent, IAttachableProperty, IComponentData
    {
        public int playerID;
        public FixedString128 fileName;
        public bool InvertX;
        public bool InvertZ;
    }
}