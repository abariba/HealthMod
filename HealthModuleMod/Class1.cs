using System.Collections.Generic;
using System.Reflection;
using System;
//using HModFabricator;
using SMLHelper;
using SMLHelper.Patchers;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UnityEngine;
using Harmony;

namespace HealthModuleMod
{

    //using System.Collections.Generic;
    //using SMLHelper.V2.Crafting;
    //using SMLHelper.V2.Assets;
    //using UnityEngine;
    class AddHealthModule
    {
        public static TechType FlatHealthModule = TechTypeHandler.AddTechType("FHM", "Subdurmal Body Armor", "this is a subdurmal body armor upgrade.", SpriteManager.Get(TechType.FirstAidKit),true);
        

        public static void Patch()
        {
            Console.WriteLine("[healthchip]harmony started initaializing");
            CraftDataHandler.SetEquipmentType(FlatHealthModule, EquipmentType.Chip);

            var techData = new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>()
            {
                        new Ingredient(TechType.FiberMesh, 4),
                        new Ingredient(TechType.Titanium, 4)
            },
                LinkedItems = new List<TechType>()
            {
                        TechType.Titanium,
                        TechType.Titanium
            }
            };
            CraftDataHandler.SetTechData(FlatHealthModule, techData);
            CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, FlatHealthModule, "Resources", "AdvancedMaterials");



            Console.WriteLine("[healthchip] Initialized");
            var harmony = HarmonyInstance.Create("abariba.HealthModuleMod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Console.WriteLine("[healthchip]harmony Initialized");
        }
    }
}
