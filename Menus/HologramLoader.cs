using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Kitchen;
using UnityEngine;
using Kitchen.Modules;
using KitchenLib;
using KitchenMyMod.Components;
using KitchenMyMod.Systems;
using Newtonsoft.Json;
using Steamworks;
using Application = UnityEngine.Application;

namespace KitchenMyMod.Menus
{
    public class HologramLoader<T> : KLMenu<T>
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
                
                AddButton("Load Schematic", delegate (int i)
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

            if (!SteamUtils.IsSteamInBigPictureMode)
            {
                AddLabel("Import New Schematic");
                AddButton("Import New", delegate (int i)
                {
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        openFileDialog.Filter = "platematica files (*.platematica)|*.platematica";
                        openFileDialog.Title = "Load Your Schematic";
                        openFileDialog.RestoreDirectory = true;

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            File.Copy(openFileDialog.FileName, Path.Combine(storagePath, Path.GetFileName(openFileDialog.FileName)));
                            LoadFiles(true, player_id);
                            RequestSubMenu(typeof(HologramLoader<T>), true);
                        }
                    }
                }, 0, 1f, 0.2f);
            }
            else
            {
                AddLabel("Import New Schematic");
                AddInfo("Importing is not supported on Steam Deck");
                AddInfo("We are working on a solution");
            }

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