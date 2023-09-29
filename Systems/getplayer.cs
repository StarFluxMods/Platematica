using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Platematica.Systems
{
    public class getplayer : GameSystemBase, IModSystem
    {
        private EntityQuery players;
        public static Vector3 x;
        protected override void Initialise()
        {
            players = GetEntityQuery(typeof(CPlayer));
        }

        protected override void OnUpdate()
        {
            NativeArray<Entity> playerArray = players.ToEntityArray(Allocator.TempJob);
            if (playerArray.Length > 0)
            {
                if (Require(playerArray[0], out CPosition pos))
                {
                    x = pos.Position;
                }
            }
        }
    }
}