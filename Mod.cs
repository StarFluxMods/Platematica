using System;
using System.Collections.Generic;
using System.Linq;
using KitchenLib;
using KitchenLib.Event;
using KitchenMods;
using System.Reflection;
using Kitchen;
using KitchenLib.Preferences;
using MessagePack;
using Platematica.Customs;
using Platematica.Enums;
using Platematica.Menus;
using Platematica.Menus.IMGUI;
using UnityEngine;

namespace Platematica
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "com.starfluxgames.platematica";
        public const string MOD_NAME = "Platematica";
        public const string MOD_VERSION = "0.1.6";
        public const string MOD_AUTHOR = "StarFluxGames";
        public const string MOD_GAMEVERSION = ">=1.2.0";

        public static HologramProjector HologramProjector;
        public static Placeholder Placeholder;
        public static PreferenceManager manager;

        public static bool FileExplorerInstalled = false;

        public static AssetBundle Bundle;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        private string decoded = "https://plateupplanner.github.io/workspace#G4JgBAbAHhYF4HMCuiXILYEcBOALADtgMYBSSArACZIDqApkgMwBqlB2qpF1LlXcALTRIuAOQDiSLMTJZKnMiwA2XVWTXCAqgHZKmAEJ7DBykqRIwAWms3bdu0A";
        
        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
            
            AppDomain currentDomain = AppDomain.CurrentDomain;
            Assembly[] assems = currentDomain.GetAssemblies();
            foreach (Assembly assembly in assems)
            {
                if (assembly.ToString().Contains("FileExplorer-Workshop"))
                {
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
            
            RegisterMenu<PlannerConverter>();

            manager = new PreferenceManager(MOD_GUID);
            manager.RegisterPreference(new PreferenceBool("directionalAssistance", false));
            manager.RegisterPreference(new PreferenceFloat("ghostOpacity", 0.7f));
            manager.RegisterPreference(new PreferenceFloat("redTint", 0.0f));
            manager.RegisterPreference(new PreferenceFloat("greenTint", 0.1f));
            manager.RegisterPreference(new PreferenceFloat("blueTint", 0.0f));
            manager.RegisterPreference(new PreferenceInt("importExportSettings", (int)ImportExportSettings.FileExplorer));
            
            manager.Load();
            
            ModsPreferencesMenu<MenuAction>.RegisterMenu("Platematica", typeof(HologramMenu<MenuAction>), typeof(MenuAction));

            Events.PlayerPauseView_SetupMenusEvent += (s, args) =>
            {
                args.addMenu.Invoke(args.instance, new object[] { typeof(HologramMenu<MenuAction>), new HologramMenu<MenuAction>(args.instance.ButtonContainer, args.module_list) });
                args.addMenu.Invoke(args.instance, new object[] { typeof(HologramPreferences<MenuAction>), new HologramPreferences<MenuAction>(args.instance.ButtonContainer, args.module_list) });
                args.addMenu.Invoke(args.instance, new object[] { typeof(HologramLoader), new HologramLoader(args.instance.ButtonContainer, args.module_list) });
                args.addMenu.Invoke(args.instance, new object[] { typeof(HologramBuilder), new HologramBuilder(args.instance.ButtonContainer, args.module_list) });
            };
            
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
