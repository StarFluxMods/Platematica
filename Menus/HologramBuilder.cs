using Kitchen;
using UnityEngine;
using Kitchen.Modules;
using KitchenLib;
using KitchenLib.Utils;
using Platematica.Systems;

namespace Platematica.Menus
{
    public class HologramBuilder : KLMenu<MenuAction>
    {
        public HologramBuilder(Transform container, ModuleList module_list) : base(container, module_list)
        {
        }
        
        public override void Setup(int player_id)
        {
            AddLabel("Create Hologram");

            AddInfo("After starting the builder, use your PING button to select the first and second corners of the schematic");
            AddInfo("*You can use placeholders to help you with the schematic*");
            
            AddButton("Start Builder", delegate (int i)
            {
                base.RequestAction(PauseMenuAction.CloseMenu);
                CreateHologram.StartHologramBuilder();
            }, 0, 1f, 0.2f);
            
            New<SpacerElement>(true);
            
            AddButton("Spawn Placeholder", delegate (int i)
            {
                SpawnUtils.SpawnApplianceBlueprintAtPlayer(Mod.Placeholder.ID, 0, 0, false);
            }, 0, 1f, 0.2f);
            
            New<SpacerElement>(true);
            New<SpacerElement>(true);
            
            AddButton("Back", delegate (int i)
            {
                Mod.manager.Save();
                RequestPreviousMenu();
            }, 0, 1f, 0.2f);
        }
    }
}