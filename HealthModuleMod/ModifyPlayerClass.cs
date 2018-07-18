namespace HealthModuleMod
{
    using System.Collections.Generic;
    using System.Reflection;
    using SMLHelper.V2.Assets;
    using SMLHelper.V2.Crafting;
    using SMLHelper.V2.Handlers;
    using SMLHelper.V2.Utility;
    using UnityEngine;

    public class HealthFabricatorModule
    {
        public static CraftTree.Type HModTreeType { get; private set; }
        public static TechType HModFabTechType { get; private set; }

        // This name will be used as both the new TechType of the buildable fabricator and the CraftTree Type for the custom crafting tree.
        public const string CustomFabAndTreeID = "HealthFabricator";

        // The text you'll see in-game when you mouseover over it.
        public const string FriendlyName = "Ancient Module Recreator";

        public const string HandOverText = "Use this device";

        public static void Patch()
        {
            // Create new Craft Tree Type
            CreateCustomTree(out CraftTree.Type craftType);
            HModTreeType = craftType;

            // Create a new TechType for new fabricator
            HModFabTechType = TechTypeHandler.AddTechType(CustomFabAndTreeID,
                                                          FriendlyName,
                                                          "Construct Player upgrade modules So you can survive more deadly woonds, or even deliver them yourself.",
                                                          ImageUtils.LoadSpriteFromFile(@"./QMods/HealthModuleMod/Assets/MissingFabricator.png"),
                                                          true);

            // Create a Recipie for the new TechType
            var customFabRecipe = new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[3]//State you are using 3 ingredients
                             {
                                 new Ingredient(TechType.Titanium, 1),
                                 new Ingredient(TechType.ComputerChip, 1),
                                 new Ingredient(TechType.FiberMesh, 2),
                                 //State what ingredients you are using.
                             })
            };

            // Add the new TechType to the buildables
            CraftDataHandler.AddBuildable(HModFabTechType);

            // Add the new TechType to the group of Interior Module buildables
            CraftDataHandler.AddToGroup(TechGroup.InteriorModules, TechCategory.InteriorModule, HModFabTechType);

            LanguageHandler.SetLanguageLine(HandOverText, "Use Vehicle Module Fabricator");

            // Set the buildable prefab
            PrefabHandler.RegisterPrefab(new HModFabricatorModulePrefab(CustomFabAndTreeID, HModFabTechType));

            // Associate the recipie to the new TechType
            CraftDataHandler.SetTechData(HModFabTechType, customFabRecipe);

            string unlockMessage = $"{FriendlyName} blueprint discovered!";

            SMLHelper.CustomSpriteHandler.customSprites.Add(new SMLHelper.CustomSprite(TechType.Terraformer, ImageUtils.LoadSpriteFromFile(@"./QMods/HealthModuleMod/Assets/TerraFormer.png")));

            var toUnlock = new TechType[1] { TechType.DiamondBlade };
   
            KnownTechHandler.SetAnalysisTechEntry(TechType.Diamond, toUnlock, unlockMessage);
            var toUnlock1 = new TechType[1] { HModFabTechType };
            KnownTechHandler.SetAnalysisTechEntry(TechType.FiberMesh, toUnlock1, unlockMessage);
            var toUnlock2 = new TechType[1] { TechType.Terraformer };
            KnownTechHandler.SetAnalysisTechEntry(TechType.Melon, toUnlock2, unlockMessage);
            var toUnlock3 = new TechType[1] { TechType.BaseUpgradeConsole };
            KnownTechHandler.SetAnalysisTechEntry(TechType.Terraformer, toUnlock3, unlockMessage);
            //KnownTechHandler.SetAnalysisTechEntry(TechType.Cyclops, toUnlock, unlockMessage);
        }

        private static void CreateCustomTree(out CraftTree.Type craftType)
        {
            ModCraftTreeRoot rootNode = CraftTreeHandler.CreateCustomCraftTreeAndType(CustomFabAndTreeID, out craftType);

            var cyclopsTab = rootNode.AddTabNode("RestorationModules", "Restouration Modules", SpriteManager.Get(SpriteManager.Group.Category, "Fabricator_Personal"));
            cyclopsTab.AddCraftingNode(AddHealthModule.FlatHealthModule,
                           TechType.DiamondBlade,
                           TechType.Terraformer
                           );
        }

        internal class HModFabricatorModulePrefab : ModPrefab
        {
            internal HModFabricatorModulePrefab(string classId, TechType techType) : base(classId, $"{classId}PreFab", techType)
            {
            }

            public override GameObject GetGameObject()
            {
                // Instantiate CyclopsFabricator object
                GameObject MedPrefab = GameObject.Instantiate(Resources.Load<GameObject>("Submarine/Build/CyclopsFabricator"));//"Submarine/Build/CyclopsFabricator"

                // Retrieve sub game objects
                GameObject cyclopsFabLight = MedPrefab.FindChild("fabricatorLight");
                GameObject cyclopsFabModel = MedPrefab.FindChild("submarine_fabricator_03");

                // Update prefab name
                MedPrefab.name = CustomFabAndTreeID;

                // Add prefab ID
                var prefabId = MedPrefab.AddComponent<PrefabIdentifier>();
                prefabId.ClassId = CustomFabAndTreeID;
                prefabId.name = FriendlyName;

                // Add tech tag
                var techTag = MedPrefab.AddComponent<TechTag>();
                techTag.type = HModFabTechType;

                // Translate CyclopsFabricator model and light
                MedPrefab.transform.localPosition = new Vector3(
                                                            cyclopsFabModel.transform.localPosition.x, // Same X position
                                                            cyclopsFabModel.transform.localPosition.y - 0.8f, // Push towards the wall slightly
                                                            cyclopsFabModel.transform.localPosition.z); // Same Z position
                cyclopsFabLight.transform.localPosition = new Vector3(
                                                            cyclopsFabLight.transform.localPosition.x, // Same X position
                                                            cyclopsFabLight.transform.localPosition.y - 0.8f, // Push towards the wall slightly
                                                            cyclopsFabLight.transform.localPosition.z); // Same Z position

                // Update sky applier
                var skyApplier = MedPrefab.GetComponent<SkyApplier>();
                skyApplier.renderers = MedPrefab.GetComponentsInChildren<Renderer>();
                skyApplier.anchorSky = Skies.Auto;

                // Associate custom craft tree to the fabricator
                var fabricator = MedPrefab.GetComponent<Fabricator>();
                fabricator.craftTree = HModTreeType;
                fabricator.handOverText = HandOverText;

                // Associate power relay
                var ghost = fabricator.GetComponent<GhostCrafter>();
                var powerRelay = new PowerRelay();

                //fabricator.SetPrivateField("powerRelay", powerRelay, BindingFlags.FlattenHierarchy);

                // Add constructable
                var constructible = MedPrefab.AddComponent<Constructable>();
                constructible.allowedInBase = true;
                constructible.allowedInSub = true;
                constructible.allowedOutside = true;
                constructible.allowedOnCeiling = true;
                constructible.allowedOnGround = false;
                constructible.allowedOnWall = true;
                constructible.allowedOnConstructables = true;
                constructible.controlModelState = true;
                constructible.rotationEnabled = false;
                constructible.techType = HModFabTechType; // This was necessary to correctly associate the recipe at building time
                constructible.model = cyclopsFabModel;

                return MedPrefab;
            }
        }
    }
}
