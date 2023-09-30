using Kitchen;
using KitchenMods;
using Platematica.Components;
using Unity.Collections;
using Unity.Entities;
using EntityQuery = Unity.Entities.EntityQuery;

namespace Platematica.Systems
{
    public class HologramAssignment : GameSystemBase, IModSystem
    {
        private EntityQuery Query;
        protected override void Initialise()
        {
            Query = GetEntityQuery(typeof(CHologramProjector), typeof(CHologramSpawner));
        }

        protected override void OnUpdate()
        {
            NativeArray<Entity> entities = Query.ToEntityArray(Allocator.TempJob);
            foreach (Entity entity in entities)
            {
                if (Require(entity, out CHologramProjector cHologramProjector) && Require(entity, out CHologramSpawner cHologramSpawner))
                {
                    cHologramProjector.Filename = cHologramSpawner.fileName;
                    EntityManager.SetComponentData(entity, cHologramProjector);
                    EntityManager.RemoveComponent<CHologramSpawner>(entity);
                }
            }
        }
    }
}