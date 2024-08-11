using System.Collections.Generic;
using System.IO;
using Kitchen;
using UnityEngine;
using Kitchen.Modules;
using KitchenLib;
using KitchenLib.Preferences;
using Platematica.Components;
using Platematica.Systems;
using Newtonsoft.Json;
using Platematica.Enums;
using Platematica.Menus.IMGUI;
using Application = UnityEngine.Application;

namespace Platematica.Menus
{
    public class HologramLoader : KLMenu<MenuAction>
    {
        public HologramLoader(Transform container, ModuleList module_list) : base(container, module_list)
        {
        }

        private static List<string> fileNames = new();
        private static List<string> displayNames = new();
        
        private Option<string> schematicOptions = null;
        
        private string storagePath = Path.Combine(Application.persistentDataPath, "UserData", "Platematica");
        private string seletedSchematic = "";

        public override void Setup(int player_id)
        {
            LoadFiles();
            Redraw(player_id);
        }

        public void LoadFiles(bool redraw = false, int player_id = 0)
        {
            if (!Directory.Exists(storagePath))
                Directory.CreateDirectory(storagePath);
            
            seletedSchematic = "";
            fileNames.Clear();
            displayNames.Clear();
            
            DirectoryInfo directory = new DirectoryInfo(storagePath);

            FileInfo[] Files = directory.GetFiles("*.platematica");

            foreach(FileInfo file in Files )
            {
                try
                {
                    Schematic schematic = JsonConvert.DeserializeObject<Schematic>(File.ReadAllText(Path.Combine(storagePath, file.Name)));
                    fileNames.Add(file.Name);
                    displayNames.Add(schematic.name);
                }
                catch
                {
                    Mod.LogWarning("Failed to load schematic " + file.Name);
                }
            }
            
            if (fileNames.Count > 0)
            {
                schematicOptions = new(fileNames, fileNames[0], displayNames);
            
                schematicOptions.OnChanged += delegate (object _, string result)
                {
                    seletedSchematic = result;
                };
                seletedSchematic = fileNames[0];
            }

            if (redraw)
            {
                Redraw(player_id);
            }
        }

        public void Redraw(int player_id)
        {
            ModuleList.Clear();

            if (fileNames.Count > 0)
            {
                AddLabel("Load Schematic");
                AddSelect(schematicOptions);

                New<SpacerElement>(true);

                AddButton("Load Schematic", delegate(int i)
                {
                    HologramSpawner.Schematics.Add(new CHologramSpawner
                    {
                        fileName = seletedSchematic,
                        InvertX = false,
                        InvertZ = false,
                        playerID = player_id
                    });
                }, 0, 1f, 0.2f);

                New<SpacerElement>(true);

            }
            
            AddLabel("Import New Schematic");

            if (Mod.manager.GetPreference<PreferenceInt>("importExportSettings").Value == (int)ImportExportSettings.JSONText || !Mod.FileExplorerInstalled)
            {
                AddButton("Import New", delegate(int i)
                {
                    RequestAction(PauseMenuAction.CloseMenu);
                    GameObject gameObject = new GameObject();
                    gameObject.AddComponent<ImportMenu>().Setup((json) =>
                    {
                        Schematic schematic = JsonConvert.DeserializeObject<Schematic>(json);
                        File.WriteAllText(Path.Combine(storagePath, schematic.name + ".platematica"), json);
                    });
                }, 0, 1f, 0.2f);
            } else if (Mod.FileExplorerInstalled)
            {
                AddButton("Import New", delegate(int i)
                {
                    RequestAction(PauseMenuAction.CloseMenu);
                    FileExplorer.FileExplorer.OpenFileSelect((path, file) =>
                    {
                        File.Copy(file, Path.Combine(storagePath, Path.GetFileName(file)));
                        LoadFiles(true, player_id);
                    }, OnCancel, "*.platematica");
                }, 0, 1f, 0.2f);
            }

            New<SpacerElement>(true);
            New<SpacerElement>(true);
            
            AddButton("Back", delegate (int i)
            {
                Mod.manager.Save();
                RequestPreviousMenu();
            }, 0, 1f, 0.2f);
        }

        private void OnCancel()
        {
        }
    }
}