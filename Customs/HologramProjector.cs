using System.Collections.Generic;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using KitchenMyMod.Components;
using KitchenMyMod.Systems;
using UnityEngine;

namespace KitchenMyMod.Customs
{
    public class HologramProjector : CustomAppliance
    {
        public override string UniqueNameID => "HologramProjector";
        public override List<IApplianceProperty> Properties => new List<IApplianceProperty>
        {
            new CHologramProjector ()
        };
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("HologramProjector");
        // public override bool IsNonInteractive => true;
        public override OccupancyLayer Layer => OccupancyLayer.Floor;

        public override List<(Locale, ApplianceInfo)> InfoList => new List<(Locale, ApplianceInfo)>
        {
            (Locale.English, new ApplianceInfo
            {
                Name = "Hologram Projector"
            })
        };

        public override void OnRegister(Appliance gameDataObject)
        {
            HologramProjectorView view = gameDataObject.Prefab.AddComponent<HologramProjectorView>();
            GameObject holder = new GameObject();
            holder.transform.parent = gameDataObject.Prefab.transform;
            view.HoldPoint = holder;

            MaterialUtils.AssignMaterialsByNames(gameDataObject.Prefab);
        }
    }
}