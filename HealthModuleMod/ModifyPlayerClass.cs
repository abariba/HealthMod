using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace HealthModuleMod
{

        [HarmonyPatch(typeof(Player))]//Surivial
        [HarmonyPatch("EquipmentChanged")]
        class CheckIfChipEquipped
        {
            public static bool Postfix(Player __instance)
            {
                //Equipment equipment = __instance.;
                //float extrahealth = 0f;
                return Inventory.main.equipment.GetTechTypeInSlot("Chip") == FlatHealthModule;
                bool healthchipEquipped = true;//__instance.GetComponent("healthchip");

                //__instance.food = HCPSettings.Instance.FoodStart;
                //__instance.water = HCPSettings.Instance.WaterStart;

                return false;
            }
        }
        //    return Inventory.main.equipment.GetTechTypeInSlot("Chip") == TechType.FlatHealthModule;
        
    
}
