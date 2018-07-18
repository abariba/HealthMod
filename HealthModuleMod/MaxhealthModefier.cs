using Harmony;

namespace HealthModuleMod
{










    [HarmonyPatch(typeof(Player))]//Surivial
    [HarmonyPatch("EquipmentChanged")]
    class CheckIfChipEquipped
    {
        public static int EquippedHealthModulesAmount = 0;

        public static bool Prefix(Player __instance)
        {
            //Equipment equipment = __instance.;
            //float extrahealth = 0f;
            // return Inventory.main.equipment.GetTechTypeInSlot("Chip") == FlatHealthModule;

            Inventory inventory = Inventory.main;

            //if (inventory == null || inventory.equipment == null)
            //    return true;

            var newAmount = inventory.equipment.GetCount(AddHealthModule.FlatHealthModule);

            if (newAmount > EquippedHealthModulesAmount)
            {
                IncreasePlayerHealth(__instance, (1+newAmount) * 100f);
                EquippedHealthModulesAmount = newAmount;
            }
            else if (newAmount < EquippedHealthModulesAmount)
            {
                DecreasePlayerHealth(__instance, (1+newAmount) * 100f);
                EquippedHealthModulesAmount = newAmount;
            }

            //bool healthchipEquipped = true;//__instance.GetComponent("healthchip");

            //__instance.food = HCPSettings.Instance.FoodStart;
            //__instance.water = HCPSettings.Instance.WaterStart;

            return true;
        }

        public static void IncreasePlayerHealth(Player player, float amount)
        {
            player.liveMixin.data.maxHealth = amount;
            player.liveMixin.health += 100; 
        }

        public static void DecreasePlayerHealth(Player player, float amount)
        {
            player.liveMixin.data.maxHealth = amount;
            player.liveMixin.health -= 100;
        }

    }
}