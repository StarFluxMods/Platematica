using KitchenLib.Utils;
using UnityEngine;

namespace Platematica.Menus.IMGUI
{
    public class ExportMenu : MonoBehaviour
    {
        private Rect windowRect = new Rect(IMGUIMenuVars.windowX, IMGUIMenuVars.windowY, IMGUIMenuVars.windowWidth, IMGUIMenuVars.windowHeight);
        public bool enabled = false;

        private string exportText = "";
        private void OnGUI()
        {
            IMGUIMenuVars.SetupStyles();
            if (!enabled) return;
            windowRect = GUI.Window(VariousUtils.GetID("PlatematicaExportMenu"), windowRect, WindowFunction, "", IMGUIMenuVars.mainStyle);
        }

        public void Setup(string _exportText)
        {
            exportText = _exportText;
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
            GUILayout.Label("Export Menu", IMGUIMenuVars.headerStyle, GUILayout.Width(IMGUIMenuVars.windowWidth), GUILayout.Height(IMGUIMenuVars.windowHeight / 6));
            GUILayout.EndArea();
            
            GUILayout.BeginArea(new Rect(0, IMGUIMenuVars.windowHeight / 6, IMGUIMenuVars.windowWidth, (IMGUIMenuVars.windowHeight / 6) * 4));
            GUILayout.TextArea(exportText, GUILayout.Width(IMGUIMenuVars.windowWidth), GUILayout.Height((IMGUIMenuVars.windowHeight / 6) * 4));
            GUILayout.EndArea();
            
            GUILayout.BeginArea(new Rect(0, ((IMGUIMenuVars.windowHeight / 6) * 5) + (((IMGUIMenuVars.windowHeight / 6) / 2) - 10), IMGUIMenuVars.windowWidth, IMGUIMenuVars.windowHeight / 6));
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Copy to Clipboard", GUILayout.Width(IMGUIMenuVars.windowWidth / 2)))
            {
                GUIUtility.systemCopyBuffer = exportText;
            }
            
            if (GUILayout.Button("Close", GUILayout.Width(IMGUIMenuVars.windowWidth / 2)))
            {
                Close();
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
}