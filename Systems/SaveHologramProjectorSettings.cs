using System.Collections.Generic;
using Kitchen;
using KitchenMods;
using Platematica.Components;
using Unity.Collections;
using Unity.Entities;

namespace Platematica.Systems
{
    public class SaveHologramProjectorSettings : GameSystemBase, IModSystem
    {
        protected override void Initialise()
        {
            base.Initialise();

            query = GetEntityQuery(new ComponentType[]
            {
                typeof(CHologramProjector),
                typeof(CPosition)
            });
        }

        protected override void OnUpdate()
        {
            if (Has<SPracticeMode>())
            {
                using (NativeArray<CHologramProjector> nativeArray = query.ToComponentDataArray<CHologramProjector>(Allocator.Temp))
                {
                    using (NativeArray<CPosition> nativeArray2 = query.ToComponentDataArray<CPosition>(Allocator.Temp))
                    {
                        CUniversalProviders.Clear();
                        CPositions.Clear();
                        foreach (CHologramProjector item in nativeArray)
                        {
                            CUniversalProviders.Add(item);
                        }
                        foreach (CPosition item2 in nativeArray2)
                        {
                            CPositions.Add(item2);
                        }
                    }
                }
            }
        }

        public override void AfterLoading(SaveSystemType system_type)
        {
            base.AfterLoading(system_type);
            if (CUniversalProviders != null)
            {
                NativeArray<Entity> nativeArray = query.ToEntityArray(Allocator.Temp);
                NativeArray<CHologramProjector> nativeArray2 = query.ToComponentDataArray<CHologramProjector>(Allocator.Temp);
                NativeArray<CPosition> nativeArray3 = query.ToComponentDataArray<CPosition>(Allocator.Temp);

                for (int i = 0; i < nativeArray.Length; i++)
                {
                    for (int j = 0; j < CUniversalProviders.Count; j++)
                    {
                        bool flag2 = (nativeArray3[i].Position - CPositions[j].Position).Chebyshev() < 0.1f;
                        if (flag2)
                        {
                            CHologramProjector component = nativeArray2[i];
                            component.Filename = CUniversalProviders[j].Filename;
                            component.InvertX = CUniversalProviders[j].InvertX;
                            component.InvertZ = CUniversalProviders[j].InvertZ;
                            base.SetComponent<CHologramProjector>(nativeArray[i], component);
                            break;
                        }
                    }
                }

                CUniversalProviders.Clear();
                nativeArray.Dispose();
                nativeArray2.Dispose();
            }
        }

        private EntityQuery query;
        private static List<CHologramProjector> CUniversalProviders = new List<CHologramProjector>();
        private static List<CPosition> CPositions = new List<CPosition>();
    }
}