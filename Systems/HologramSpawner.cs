using System;
using System.Collections.Generic;
using Kitchen;
using KitchenMods;
using Platematica.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Platematica.Systems
{
    public class HologramSpawner : GameSystemBase, IModSystem
    {
        public static List<CHologramSpawner> Schematics = new List<CHologramSpawner>();
        private EntityQuery Players;
        
        protected override void Initialise()
        {
            Players = GetEntityQuery(typeof(CPlayer), typeof(CPosition));
        }
        
        protected override void OnUpdate()
        {
            NativeArray<Entity> entities = Players.ToEntityArray(Allocator.TempJob);
            if (Schematics.Count > 0)
            {
                CHologramSpawner schematic = Schematics[0];

                foreach (Entity entity in entities)
                {
                    if (Require(entity, out CPlayer cPlayer) && Require(entity, out CPosition cPosition))
                    {
                        if (cPlayer.ID == schematic.playerID)
                        {
                            Entity newEntity = EntityManager.CreateEntity(typeof(CCreateAppliance), typeof(CPosition), typeof(CHologramSpawner));
                            EntityManager.SetComponentData(newEntity, new CCreateAppliance { ID = Mod.HologramProjector.ID });
                            EntityManager.SetComponentData(newEntity, new CPosition
                            {
                                Position = new Vector3(Convert.ToInt32(cPosition.Position.x), 0, Convert.ToInt32(cPosition.Position.z)),
                                Rotation = new quaternion(0, 0, 0, 0),
                                ForceSnap = true
                            });
                            EntityManager.SetComponentData(newEntity, schematic);
                        }
                    }
                }
                Schematics.RemoveAt(0);
            }

            entities.Dispose();
        }
    }
}