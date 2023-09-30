using KitchenLib.Utils;
using UnityEngine;

namespace Platematica.Menus.IMGUI
{
    public class ImportMenu : MonoBehaviour
    {
        private Rect windowRect = new Rect(IMGUIMenuVars.windowX, IMGUIMenuVars.windowY, IMGUIMenuVars.windowWidth, IMGUIMenuVars.windowHeight);
        public bool enabled = false;

        private string importText = "";
        private IMGUIMenuVars.Load load;

        private void OnGUI()
        {
            IMGUIMenuVars.SetupStyles();
            if (!enabled) return;
            windowRect = GUI.Window(VariousUtils.GetID("PlatematicaImportMenu"), windowRect, WindowFunction, "", IMGUIMenuVars.mainStyle);
        }

        public void Setup(IMGUIMenuVars.Load _load)
        {
            load = _load;
            enabled = true;
        }

        public void Close()
        {
            enabled = false;
            Destroy(gameObject);
        }

        private void WindowFunction(int id)
        {
            GUILayout.BeginArea(new Rect(0, 0, IMGUIMenuVars.windowWidth, IMGUIMenuVars.windowHeight / 6));
            GUILayout.Label("Import Menu", IMGUIMenuVars.headerStyle, GUILayout.Width(IMGUIMenuVars.windowWidth), GUILayout.Height(IMGUIMenuVars.windowHeight / 6));
            GUILayout.EndArea();
            
            GUILayout.BeginArea(new Rect(0, IMGUIMenuVars.windowHeight / 6, IMGUIMenuVars.windowWidth, (IMGUIMenuVars.windowHeight / 6) * 4));
            importText = GUILayout.TextArea(importText, GUILayout.Width(IMGUIMenuVars.windowWidth), GUILayout.Height((IMGUIMenuVars.windowHeight / 6) * 4));
            GUILayout.EndArea();
            
            GUILayout.BeginArea(new Rect(0, ((IMGUIMenuVars.windowHeight / 6) * 5) + (((IMGUIMenuVars.windowHeight / 6) / 2) - 10), IMGUIMenuVars.windowWidth, IMGUIMenuVars.windowHeight / 6));
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Close", GUILayout.Width(IMGUIMenuVars.windowWidth / 2)))
            {
                Close();
            }
            
            if (GUILayout.Button("Import", GUILayout.Width(IMGUIMenuVars.windowWidth / 2)))
            {
                load(importText);
                Close();
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
}