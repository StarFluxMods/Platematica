using UnityEngine;

namespace Platematica.Menus.IMGUI
{
    public class IMGUIMenuVars : MonoBehaviour
    {
        public static float windowHeight = Screen.height / 3;
        public static float windowWidth = windowHeight * 1.778f;
        
        public static float windowX = Screen.width / 2 - windowWidth / 2;
        public static float windowY = Screen.height / 2 - windowHeight / 2;
        
        public delegate void Close();
        public delegate void Save(string path, string file);
        public delegate void Load(string json);

        public static GUIStyle mainStyle = null;
        public static GUIStyle headerStyle = null;

        public static void SetupStyles()
        {
            if (mainStyle == null || headerStyle == null)
            {
                mainStyle = new GUIStyle(GUI.skin.window);
                mainStyle.normal.background = Mod.Bundle.LoadAsset<Texture2D>("Menu Bean");
                
                
                headerStyle = new GUIStyle(GUI.skin.label);
                headerStyle.alignment = TextAnchor.MiddleCenter;
                headerStyle.fontSize = (int)windowWidth / 32;
            }
        }
    }
}