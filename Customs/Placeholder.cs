using System.Collections.Generic;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using UnityEngine;

namespace Platematica.Customs
{
    public class Placeholder : CustomAppliance
    {
        public override string UniqueNameID => "Placeholder";
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Placeholder");
        public override OccupancyLayer Layer => OccupancyLayer.Floor;

        public override List<(Locale, ApplianceInfo)> InfoList => new List<(Locale, ApplianceInfo)>
        {
            (Locale.English, new ApplianceInfo
            {
                Name = "Placeholder",
                Description = "This Appliance won't be added to schematics"
            })
        };

        public override void OnRegister(Appliance gameDataObject)
        {
            MaterialUtils.AssignMaterialsByNames(gameDataObject.Prefab);
        }
    }
}