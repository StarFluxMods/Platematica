using System.Collections.Generic;
using System.IO;
using Kitchen;
using KitchenData;
using KitchenLib.Preferences;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMods;
using Platematica.Components;
using Platematica.Utils;
using MessagePack;
using Newtonsoft.Json;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Platematica.Systems
{
    public class HologramProjectorView : UpdatableObjectView<HologramProjectorView.ViewData>
    {

        public Dictionary<int, int> ApplianceRotationOverrides = new Dictionary<int, int>
        {
            {ApplianceReferences.Portioner, 180},
            {ApplianceReferences.MixerPusher, 180},
        };
        
        [MessagePackObject(false)]
        public struct ViewData : ISpecificViewData, IViewData.ICheckForChanges<ViewData>
        {
            [Key(2)] public bool Display;
            [Key(3)] public bool InvertX;
            [Key(4)] public bool InvertZ;
            [Key(6)] public Schematic Schematic;

            public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<HologramProjectorView>();

            public bool IsChangedFrom(ViewData cached)
            {
                return Display != cached.Display || InvertX != cached.InvertX || InvertZ != cached.InvertZ;
            }
        }

        protected List<GameObject> CurrentGhosts = new();
        private ViewData _viewData;

        private bool displayArrows = false;
        private float ghostOpacity = 0.5f;
        private float redTint = 0.0f;
        private float greenTint = 0.0f;
        private float blueTint = 0.0f;
        
        protected override void UpdateData(ViewData view_data)
        {
            UpdateHolograms(view_data);
        }

        public static bool UpdateGhosts = false;
        void Update()
        {
            if (UpdateGhosts)
            {
                UpdateGhosts = false;
                UpdateHolograms(_viewData);
            }
        }
        private void UpdateHolograms(ViewData view_data)
        {
            displayArrows = Mod.manager.GetPreference<PreferenceBool>("directionalAssistance").Value;
            _viewData = view_data;
            
            foreach (GameObject ghost in CurrentGhosts)
            {
                Destroy(ghost);
            }
            
            CurrentGhosts.Clear();
            
            if (!view_data.Display)
                return;
            
            foreach (SchematicComponent component in view_data.Schematic.components)
            {
                if (GameData.Main.TryGet(component.applianceID, out Appliance appliance))
                {
                    GameObject prefab = Instantiate(appliance.Prefab);
                    if (prefab == null)
                        return;

                    int xOffset = component.xOffset;
                    int zOffset = component.zOffset;
                    
                    if (view_data.InvertX)
                        xOffset -= view_data.Schematic.xSize - 1;
                    if (view_data.InvertZ)
                        zOffset -= view_data.Schematic.zSize - 1;
                    
                    prefab.transform.parent = HoldPoint.transform;
                    prefab.transform.localPosition = new Vector3(xOffset, 0, zOffset);
                    prefab.transform.localScale = Vector3.one;
                    prefab.transform.localRotation = Quaternion.Euler(0, component.rotationOffset, 0);

                    
                    if (appliance.ID == ApplianceReferences.GrabberRotatable)
                    {
                        Animator animator = prefab.GetComponent<Animator>();
                        animator.SetInteger("Direction", component.rotatedGrabberDirection);
                    }
                    
                    // Directional Arrows
                    
                    if (RenderUtils.SetMaterialsTransparent(prefab))
                    {
                        RenderUtils.RemoveColliders(prefab);
                        RenderUtils.RemoveNavMeshObstacles(prefab);
                        if (component.isDirectional && displayArrows)
                        {
                            Quaternion rotation = Quaternion.Euler(0, component.rotationOffset, 0);
                            string overlayName = "Arrow Overlay";
                            if (ApplianceRotationOverrides.ContainsKey(component.applianceID))
                                rotation = Quaternion.Euler(0, component.rotationOffset + ApplianceRotationOverrides[component.applianceID], 0);
                            if (component.rotatedGrabberDirection == 1)
                                overlayName = "Arrow Overlay - Left";
                            else  if (component.rotatedGrabberDirection == 4)
                                overlayName = "Arrow Overlay - Right";
                            GameObject arrow_overlay = Instantiate(Mod.Bundle.LoadAsset<GameObject>(overlayName));
                            arrow_overlay.transform.parent = HoldPoint.transform;
                            arrow_overlay.transform.localPosition = new Vector3(xOffset, 0.0f, zOffset);
                            arrow_overlay.transform.localScale = Vector3.one;
                            arrow_overlay.transform.localRotation = rotation;
                            MaterialUtils.AssignMaterialsByNames(arrow_overlay);
                            CurrentGhosts.Add(arrow_overlay);
                        }

                        CurrentGhosts.Add(prefab);
                    }
                }
            }
        }

        public GameObject HoldPoint;


        public class UpdateView : IncrementalViewSystemBase<ViewData>, IModSystem
        {
            
            private EntityQuery Views;
            
            protected override void Initialise()
            {
                base.Initialise();
                
                Views = GetEntityQuery(new QueryHelper()
                    .All(typeof(CHologramProjector), typeof(CLinkedView)));
            }

            protected override void OnUpdate()
            {
                using var entities = Views.ToEntityArray(Allocator.Temp);
                using var views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);
                for (var i = 0; i < views.Length; i++)
                {
                    var view = views[i];
                    if (Require(entities[i], out CHologramProjector cHologramProjector))
                    {

                        string filePath = Path.Combine(Application.persistentDataPath, "UserData", "Platematica", cHologramProjector.Filename.ToString());

                        Schematic LoadedSchematic = JsonConvert.DeserializeObject<Schematic>(File.ReadAllText(filePath));

                        if (LoadedSchematic == null)
                            return;

                        ViewData data = new ViewData
                        {
                            Display = !(Has<SPracticeMode>() || Has<SIsDayTime>()),
                            InvertX = cHologramProjector.InvertX,
                            InvertZ = cHologramProjector.InvertZ,
                            Schematic = LoadedSchematic
                        };
                        SendUpdate(view, data);
                    }
                }
            }
        }
    }
}