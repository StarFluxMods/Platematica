using System.Collections.Generic;
using Kitchen;
using UnityEngine;
using Kitchen.Modules;
using KitchenLib;
using KitchenLib.Preferences;
using Platematica.Systems;

namespace Platematica.Menus
{
    public class HologramPreferences<T> : KLMenu<T>
    {
        public HologramPreferences(Transform container, ModuleList module_list) : base(container, module_list)
        {
        }
        
        private Option<bool> directionalAssistance = new Option<bool>(new List<bool> { true, false }, Mod.manager.GetPreference<PreferenceBool>("directionalAssistance").Value, new List<string> { "Enabled", "Disabled" });
        private Option<float> ghostOpacity = new Option<float>(new List<float> { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f }, Mod.manager.GetPreference<PreferenceFloat>("ghostOpacity").Value, new List<string> { "0.0", "0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9", "1.0" });
        private Option<float> redTint = new Option<float>(new List<float> { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f }, Mod.manager.GetPreference<PreferenceFloat>("redTint").Value, new List<string> { "0.0", "0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9", "1.0" });
        private Option<float> greenTint = new Option<float>(new List<float> { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f }, Mod.manager.GetPreference<PreferenceFloat>("greenTint").Value, new List<string> { "0.0", "0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9", "1.0" });
        private Option<float> blueTint = new Option<float>(new List<float> { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f }, Mod.manager.GetPreference<PreferenceFloat>("blueTint").Value, new List<string> { "0.0", "0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9", "1.0" });
        
        public override void Setup(int player_id)
        {
            AddLabel("Directional Assistance");
            AddSelect(directionalAssistance);
            directionalAssistance.OnChanged += delegate (object _, bool result)
            {
                HologramProjectorView.UpdateGhosts = true;
                Mod.manager.GetPreference<PreferenceBool>("directionalAssistance").Set(result);
            };
            
            New<SpacerElement>(true);
            
            AddLabel("Ghost Opacity");
            AddSelect(ghostOpacity);
            ghostOpacity.OnChanged += delegate (object _, float result)
            {
                HologramProjectorView.UpdateGhosts = true;
                Mod.manager.GetPreference<PreferenceFloat>("ghostOpacity").Set(result);
            };
            
            New<SpacerElement>(true);
            
            AddLabel("Red Tint");
            AddSelect(redTint);
            redTint.OnChanged += delegate (object _, float result)
            {
                HologramProjectorView.UpdateGhosts = true;
                Mod.manager.GetPreference<PreferenceFloat>("redTint").Set(result);
            };
            
            New<SpacerElement>(true);
            
            AddLabel("Green Tint");
            AddSelect(greenTint);
            greenTint.OnChanged += delegate (object _, float result)
            {
                HologramProjectorView.UpdateGhosts = true;
                Mod.manager.GetPreference<PreferenceFloat>("greenTint").Set(result);
            };
            
            New<SpacerElement>(true);
            
            AddLabel("Blue Tint");
            AddSelect(blueTint);
            blueTint.OnChanged += delegate (object _, float result)
            {
                HologramProjectorView.UpdateGhosts = true;
                Mod.manager.GetPreference<PreferenceFloat>("blueTint").Set(result);
            };
            
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