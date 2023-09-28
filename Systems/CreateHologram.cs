using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Kitchen;
using KitchenLib.References;
using Newtonsoft.Json;
using Steamworks;
using Unity.Entities;
using UnityEngine;
using Application = UnityEngine.Application;

namespace KitchenMyMod.Systems
{
    [UpdateBefore(typeof(MakePing))]
    [UpdateBefore(typeof(ShowPingedApplianceInfo))]
    public class CreateHologram : InteractionSystem
    {

        private List<int> DirectionalIDs = new List<int>
        {
            ApplianceReferences.GasLimiter,
            ApplianceReferences.GasSafetyOverride,
            ApplianceReferences.Belt,
            ApplianceReferences.Grabber,
            ApplianceReferences.GrabberSmart,
            ApplianceReferences.GrabberRotatable,
            ApplianceReferences.Combiner,
            ApplianceReferences.Portioner,
            ApplianceReferences.MixerPusher,
        };
        
        private static int HologramStep = 0;
        private static bool BuilderEnabled = false;
        
        private Vector2 FirstCorner = Vector2.zero;
        private Vector2 SecondCorner = Vector2.zero;

        private (Vector2, Vector2) SchematicLocation;

        public static void StartHologramBuilder()
        {
            HologramStep = 0;
            BuilderEnabled = true;
        }

        public static void StopHologramBuilder()
        {
            BuilderEnabled = false;
        }

        protected override InteractionType RequiredType
        {
            get
            {
                return InteractionType.Notify;
            }
        }

        protected override InteractionMode RequiredMode
        {
            get
            {
                return InteractionMode.Appliances;
            }
        }

        protected override bool IsPossible(ref InteractionData data)
        {
            return Require(data.Target, out CAppliance cAppliance) && Require(data.Target, out CPosition cPosition) && BuilderEnabled;
        }

        protected override void Perform(ref InteractionData data)
        {
            if (Require(data.Target, out CPosition cPosition))
            {
                if (HologramStep == 0) // First Corner
                {
                    FirstCorner = new Vector2(cPosition.Position.x, cPosition.Position.z);
                    HologramStep++;
                }
                else if (HologramStep == 1) // Second Corner
                {
                    SecondCorner = new Vector2(cPosition.Position.x, cPosition.Position.z);
                    StopHologramBuilder();
                    SchematicLocation = (FirstCorner, SecondCorner);
                    
                    TextInputView.RequestTextInput("Name your schematic", "", 20, BuildSchematic);
                }
            }
        }

        private void BuildSchematic(TextInputView.TextInputState result, string name)
        {
            int x1, y1, x2, y2;

            if (FirstCorner.x < SecondCorner.x)
            {
                x1 = (int)FirstCorner.x;
                x2 = (int)SecondCorner.x;
            }
            else
            {
                x1 = (int)SecondCorner.x;
                x2 = (int)FirstCorner.x;
            }

            if (FirstCorner.y < SecondCorner.y)
            {
                y1 = (int)FirstCorner.y;
                y2 = (int)SecondCorner.y;
            }
            else
            {
                y1 = (int)SecondCorner.y;
                y2 = (int)FirstCorner.y;
            }

            Vector2 corner1 = new Vector2(x1, y1);
            Vector2 corner2 = new Vector2(x2, y2);

            List<Vector2> points = GetPoints(corner1, corner2);

            Schematic schematic = new();
            schematic.name = name;
            schematic.xSize = (int)Mathf.Abs(FirstCorner.x - SecondCorner.x);
            schematic.zSize = (int)Mathf.Abs(FirstCorner.y - SecondCorner.y);

            foreach (Vector2 point in points)
            {
                Entity entity = GetOccupant(new Vector3(point.x, 0, point.y));
                if (Require(entity, out CAppliance appliance) && Require(entity, out CPosition cPosition))
                {
                    Vector3 forward = cPosition.Forward(1);
                    float rotation = -1;

                    rotation = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;

                    int grabberRotate = -1;

                    if (Require(entity, out CConveyPushRotatable cConveyPushRotatable))
                    {
                        grabberRotate = (int)cConveyPushRotatable.Target;
                    }

                    if (appliance.ID != Mod.Placeholder.ID)
                    {
                        schematic.components.Add(new SchematicComponent()
                        {
                            applianceID = appliance.ID,
                            xOffset = (int)(point.x - FirstCorner.x),
                            zOffset = (int)(point.y - FirstCorner.y),
                            rotationOffset = (int)rotation,
                            isDirectional = DirectionalIDs.Contains(appliance.ID),
                            rotatedGrabberDirection = grabberRotate
                        });
                    }
                }
            }

            string json = JsonConvert.SerializeObject(schematic);

            string storagePath = Path.Combine(Application.persistentDataPath, "UserData", "Platematica");
            if (!Directory.Exists(storagePath))
                Directory.CreateDirectory(storagePath);

            File.WriteAllText(Path.Combine(storagePath, name + ".platematica"), json);
            /*
            if (!SteamUtils.IsSteamInBigPictureMode)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                saveFileDialog1.Filter = ".platematica files (*.platematica)|*.platematica";
                saveFileDialog1.Title = "Save Your Schematic";
                saveFileDialog1.ShowDialog();
                if(saveFileDialog1.FileName != "")
                {
                    File.Copy(Path.Combine(storagePath, name + ".platematica"), saveFileDialog1.FileName);
                }
            }
            */
        }

        private List<Vector2> GetPoints(Vector2 corner1, Vector2 corner2)
        {
            List<Vector2> points = new();
            
            for (int x = (int)corner1.x; x <= (int)corner2.x; x++)
            {
                for (int z = (int)corner1.y; z <= (int)corner2.y; z++)
                {
                    points.Add(new Vector2(x, z));
                }
            }

            return points;
        }
    }
}