using System;
using System.Collections.Generic;
using System.Linq;
using KitchenLib;
using KitchenLib.Event;
using KitchenMods;
using System.Reflection;
using Kitchen;
using KitchenLib.Preferences;
using Platematica.Customs;
using Platematica.Menus;
using MessagePack;
using UnityEngine;

namespace Platematica
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "com.starfluxgames.platematica";
        public const string MOD_NAME = "Platematica (Beta)";
        public const string MOD_VERSION = "0.1.3";
        public const string MOD_AUTHOR = "StarFluxGames";
        public const string MOD_GAMEVERSION = ">=1.1.7";

        public static HologramProjector HologramProjector;
        public static Placeholder Placeholder;
        public static PreferenceManager manager;

        public static bool FileExplorerInstalled = false;
        
#if DEBUG
        public const bool DEBUG_MODE = true;
#else
        public const bool DEBUG_MODE = false;
#endif

        public static AssetBundle Bundle;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }
        
        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
            
            AppDomain currentDomain = AppDomain.CurrentDomain;
            Assembly[] assems = currentDomain.GetAssemblies();
            foreach (Assembly assembly in assems)
            {
                if (assembly.ToString().Contains("FileExplorer-Workshop"))
                {
                    LogInfo("------------------------------------------ FOUND FILE EXPLORER ------------------------------------------");
                    FileExplorerInstalled = true;
                }
            }
        }

        private void AddGameData()
        {
            LogInfo("Attempting to register game data...");

            HologramProjector = AddGameDataObject<HologramProjector>();
            Placeholder = AddGameDataObject<Placeholder>();

            LogInfo("Done loading game data.");
        }
        
        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).First();
            AddGameData();

            manager = new PreferenceManager(MOD_GUID);
            manager.RegisterPreference(new PreferenceBool("directionalAssistance", false));
            manager.RegisterPreference(new PreferenceFloat("ghostOpacity", 0.7f));
            manager.RegisterPreference(new PreferenceFloat("redTint", 0.0f));
            manager.RegisterPreference(new PreferenceFloat("greenTint", 0.1f));
            manager.RegisterPreference(new PreferenceFloat("blueTint", 0.0f));
            
            ModsPreferencesMenu<PauseMenuAction>.RegisterMenu("Platematica", typeof(HologramMenu<PauseMenuAction>), typeof(PauseMenuAction));

            Events.PreferenceMenu_PauseMenu_CreateSubmenusEvent += (s, args) => {
                args.Menus.Add(typeof(HologramMenu<PauseMenuAction>), new HologramMenu<PauseMenuAction>(args.Container, args.Module_list));
                args.Menus.Add(typeof(HologramPreferences<PauseMenuAction>), new HologramPreferences<PauseMenuAction>(args.Container, args.Module_list));
                args.Menus.Add(typeof(HologramLoader), new HologramLoader(args.Container, args.Module_list));
                args.Menus.Add(typeof(HologramBuilder), new HologramBuilder(args.Container, args.Module_list));
            };;
        }
        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
    [MessagePackObject]
    public class Schematic
    {
        [Key(0)]public string name = "";
        [Key(1)]public int xSize = 0;
        [Key(2)]public int zSize = 0;
        [Key(3)]public List<SchematicComponent> components = new();
    }
    
    [MessagePackObject]
    public struct SchematicComponent
    {
        [Key(0)]public int applianceID;
        [Key(1)]public int xOffset;
        [Key(2)]public int zOffset;
        [Key(3)]public int rotationOffset;
        [Key(4)]public bool isDirectional;
        [Key(5)]public int rotatedGrabberDirection;
    }
}
