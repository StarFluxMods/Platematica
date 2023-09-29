using System.Collections.Generic;
using KitchenLib.Preferences;
using KitchenLib.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Platematica.Utils
{
    public static class RenderUtils
    {
        public static void RemoveColliders(GameObject gameObject)
        {
            foreach (Collider collider in gameObject.GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }

        public static void AssignMaterials(GameObject gameObject, Material material)
        {
            foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                List<Material> materials = new List<Material>();
                foreach (Material _material in renderer.materials)
                {
                    materials.Add(material);
                }
                renderer.materials = materials.ToArray();
            }
        }
        
        public static void RemoveNavMeshObstacles(GameObject gameObject)
        {
            foreach (NavMeshObstacle obstacle in gameObject.GetComponentsInChildren<NavMeshObstacle>())
            {
                obstacle.enabled = false;
            }
        }
        
        public static bool SetMaterialsTransparent(GameObject gameObject)
        {
            Renderer[] array = gameObject.GetComponentsInChildren<Renderer>();
            float r = Mod.manager.GetPreference<PreferenceFloat>("redTint").Value;
            float g = Mod.manager.GetPreference<PreferenceFloat>("greenTint").Value;
            float b = Mod.manager.GetPreference<PreferenceFloat>("blueTint").Value;
            float alpha = Mod.manager.GetPreference<PreferenceFloat>("ghostOpacity").Value;
            foreach (Renderer renderer in array)
            {
                List<Material> materials = new List<Material>();
                foreach (Material material in renderer.materials)
                {
                    if (material.HasProperty("_Color0"))
                    {
                        Material newMat = null;
                        Color color = material.GetColor("_Color0");
                        color.r += r;
                        color.g += g;
                        color.b += b;
                        color.a = alpha;
                        newMat = MaterialUtils.CreateTransparent("", color);
                        materials.Add(newMat);
                    }else
                    {
                        materials.Add(material);
                    }
                }
                renderer.materials = materials.ToArray();
            }

            return true;
        }
    }
}