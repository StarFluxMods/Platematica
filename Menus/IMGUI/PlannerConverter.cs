using System.IO;
using KitchenLib.DevUI;
using Newtonsoft.Json;
using Platematica.Utils;
using UnityEngine;

namespace Platematica.Menus.IMGUI
{
    public class PlannerConverter : BaseUI
	{
		
		private string Name = "";
		private string URL = "";
		private string Converted = "";
		
		public PlannerConverter()
		{
			ButtonName = "Planner";
		}

		public override void OnInit()
		{
		}
		
		public override void Setup()
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Name");
			Name = GUILayout.TextField(Name);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("URL");
			URL = GUILayout.TextField(URL);
			GUILayout.EndHorizontal();
            
			GUILayout.TextArea(Converted);
			
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Convert"))
			{
				if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(URL))
					return;
				
				Schematic schematic = PlateUpPlannerConverter.ConvertURLToSchematic(URL, Name);
				string json = JsonConvert.SerializeObject(schematic);
				Converted = json;
			}

			if (!string.IsNullOrEmpty(Converted))
			{
				if (GUILayout.Button("Save"))
				{
					File.WriteAllText(Path.Combine(Application.persistentDataPath, "UserData/Platematica", Name + ".platematica"), Converted);
				}
			}
			GUILayout.EndHorizontal();
		}

		public override void Disable()
		{
			Name = "";
			URL = "";
			Converted = "";
		}

	}
}