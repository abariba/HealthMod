using Harmony;

namespace HealthModuleMod
{
    [HarmonyPatch(typeof(Survival))]//Surivial
    [HarmonyPatch("Reset")]
    class AddMaxhealth
    {
        public static bool Prefix(Survival __instance)
        {
            //Equipment equipment = __instance.;
            float extrahealth = 0f;
            bool healthchipEquipped = true;//__instance.GetComponent("healthchip");
            if (healthchipEquipped)
            {
                extrahealth = 50f;
            }
            __instance.GetComponent<Player>().liveMixin.data.maxHealth = 100f + extrahealth;
            //__instance.GetComponent<Player>().liveMixin.health = 100F * 0f;

            __instance.food = 100f;
            __instance.water = 100f;

            return false;
        }
    }
}