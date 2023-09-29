using Kitchen;
using UnityEngine;
using Kitchen.Modules;
using KitchenLib;

namespace Platematica.Menus
{
    public class HologramMenu<T> : KLMenu<T>
    {
        public HologramMenu(Transform container, ModuleList module_list) : base(container, module_list)
        {
        }
        public override void Setup(int player_id)
        {
            PlayerManager pm = Unity.Entities.World.DefaultGameObjectInjectionWorld.GetExistingSystem<PlayerManager>();
            AddLabel("Platematica");
            AddButton("Hologram Preferences", async delegate (int i)
            {
                RequestSubMenu(typeof(HologramPreferences<T>));
            }, 0, 1f, 0.2f);
            if (pm != null)
            {
                AddButton("Hologram Generator", async delegate (int i)
                {
                    RequestSubMenu(typeof(HologramLoader));
                }, 0, 1f, 0.2f);
                AddButton("Hologram Builder", async delegate (int i)
                {
                    RequestSubMenu(typeof(HologramBuilder));
                }, 0, 1f, 0.2f);
            }
            
            New<SpacerElement>(true);
            New<SpacerElement>(true);
            
            AddButton("Back", delegate (int i)
            {
                RequestPreviousMenu();
            }, 0, 1f, 0.2f);
        }
    }
}