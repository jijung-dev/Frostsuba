using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Localization.Tables;
using UnityEngine.U2D;
using UnityEngine.UI;
using WildfrostHopeMod;
using WildfrostHopeMod.SFX;
using WildfrostHopeMod.Utils;
using WildfrostHopeMod.VFX;
using Extensions = Deadpan.Enums.Engine.Components.Modding.Extensions;

namespace Konosuba
{
    public class Frostsuba : WildfrostMod
    {
        public Frostsuba(string modDir)
            : base(modDir)
        {
            HarmonyInstance.PatchAll(typeof(PatchHarmony));
        }

        public static Frostsuba instance;
        public static List<object> assets = new List<object>();
        public static bool preload = false;
        public override string GUID => "tgestudio.wildfrost.frostsuba";

        public override string[] Depends => new string[] { "hope.wildfrost.vfx" };

        public override string Title => "Frostsuba";

        public override string Description =>
            " "
            + "\n"
            + "\n"
            + "\n"
            + "If you found any bug/ overlapping/ overflow please tell me down the comment or in game discord #mod-development @nubboiz";

        public static string CatalogFolder
            => Path.Combine(instance.ModDirectory, "Windows");

        // A helpful shortcut
        public static string CatalogPath
            => Path.Combine(CatalogFolder, "catalog.json");

        public GameObject bullet;

        public override TMP_SpriteAsset SpriteAsset => spriteAsset;
        internal static TMP_SpriteAsset spriteAsset;

        private bool preLoaded = false;
        private RewardPool meguminItemPool;
        private RewardPool kazumaItemPool;
        private RewardPool aquaItemPool;
        private RewardPool darknessItemPool;
        private RewardPool meguminUnitPool;
        private RewardPool kazumaUnitPool;
        private RewardPool aquaUnitPool;
        private RewardPool darknessUnitPool;
        private RewardPool meguminCharmPool;
        private RewardPool kazumaCharmPool;
        private RewardPool aquaCharmPool;
        private RewardPool darknessCharmPool;

        private void CreateModAssets()
        {
            var subclass = DataBase.subclasses;
            foreach (Type type in subclass)
            {
                if (Activator.CreateInstance(type) is DataBase instance)
                {
                    assets.AddRange(instance.CreateAssest());
                }
            }

            #region Tribe
            //Add Tribe
            //Sprite flagSprite = DSTMod.Other.GetSprite("DSTFlag");
            meguminItemPool = CreateRewardPool("MeguminItemPool", "Items", DataList<CardData>(
                "DragonflamePepper", "FlameWater", "Peppereaper", "Peppering", "SpiceStones", "SunburstDart", "Badoo", "Recycler", "EnergyDart"
            ));
            meguminUnitPool = CreateRewardPool("MeguminUnitPool", "Units", DataList<CardData>(
                "Heartforge", "MobileCampfire", "SpiceSparklers", "Pyra", "Witch", "Gnomlings", "Madness"
            ));
            meguminCharmPool = CreateRewardPool("MeguminCharmPool", "Charms", DataList<CardUpgradeData>(
                "CardUpgradeInk", "CardUpgradeBom"
            ));
            kazumaItemPool = CreateRewardPool("KazumaItemPool", "Items", DataList<CardData>(
                "Dittostone", "Putty", "Bumblebee", "Voidstone", "FoggyBrew", "EyeDrops", "EnergyDart", "Scythe", "FlameWater"
            ));
            kazumaUnitPool = CreateRewardPool("KazumaUnitPool", "Units", DataList<CardData>(
                "Pootie", "Pyra", "Shelly", "MobileCampfire", "BloodBoy", "Egg", "Voodoo", "PomDispenser"
            ));
            kazumaCharmPool = CreateRewardPool("KazumaCharmPool", "Charms", DataList<CardUpgradeData>(
                "CardUpgradeInk", "CardUpgradeBom", "CardUpgradeMime"
            ));
            aquaItemPool = CreateRewardPool("AquaItemPool", "Items", DataList<CardData>(
                "HongosHammer", "SporePack", "BoltHarpoon", "FlashWhip", "ZapOrb", "FoggyBrew", "HazeBlaze", "LuminShard", "IceShard"
            ));
            aquaUnitPool = CreateRewardPool("AquaUnitPool", "Units", DataList<CardData>(
                "Fulbert", "Wallop", "Yuki", "ShroomLauncher", "Shroominator", "Flash", "Zula"
            ));
            aquaCharmPool = CreateRewardPool("AquaCharmPool", "Charms", DataList<CardUpgradeData>(
                "CardUpgradeConsumeOverload", "CardUpgradeOverload"
            ));
            darknessItemPool = CreateRewardPool("DarknessItemPool", "Items", DataList<CardData>(
                "SnowStick", "Snowcake", "SnowCannon", "ScrapPile", "SharkTooth", "SnowMaul", "Voidstone", "Bumblebee", "Junberry", "StormbearSpirit"
            ));
            darknessUnitPool = CreateRewardPool("DarknessUnitPool", "Units", DataList<CardData>(
                "Kernel", "Shelly", "Tusk", "Turmeep", "Chompom", "Fulbert", "Pootie", "TailsFive"
            ));
            darknessCharmPool = CreateRewardPool("DarknessCharmPool", "Charms", DataList<CardUpgradeData>(
                "CardUpgradeSpiky", "CardUpgradeTeethWhenHit", "CardUpgradeScrap"
            ));
            assets.Add(
                TribeCopy("Magic", "Konosuba")
                    .WithFlag(ImagePath("KonosubaFlag.png"))
                    //.WithSelectSfxEvent(FMODUnity.RuntimeManager.PathToEventReference("event:/sfx/card/summon"))
                    .SubscribeToAfterAllBuildEvent(
                        (data) =>
                        {
                            GameObject gameObject = data.characterPrefab.gameObject.InstantiateKeepName();
                            UnityEngine.Object.DontDestroyOnLoad(gameObject);
                            gameObject.name = "Player (konosuba.Frostsuba)";
                            data.characterPrefab = gameObject.GetComponent<Character>();
                            data.id = "konosuba.Frostsuba";

                            data.leaders = DataList<CardData>("aqua", "megumin", "kazuma", "darkness");

                            Inventory inventory = new Scriptable<Inventory>();
                            inventory.deck.list = DataList<CardData>().ToList();
                            data.startingInventory = inventory;

                            data.rewardPools = new RewardPool[]
                            {
                                Extensions.GetRewardPool("GeneralUnitPool"),
                                Extensions.GetRewardPool("GeneralItemPool"),
                                Extensions.GetRewardPool("GeneralCharmPool"),
                                Extensions.GetRewardPool("GeneralModifierPool"),
                                Extensions.GetRewardPool("BasicCharmPool"),
                            };
                        }
                    )
            );
            #endregion

            preLoaded = true;
        }
        public IEnumerator CampaignInit()
        {
            if (References.PlayerData?.classData.ModAdded != this)
                yield break;

            List<CardData> addCards = new List<CardData>();

            if (References.LeaderData.original == TryGet<CardData>("aqua"))
            {
                References.PlayerData.classData.rewardPools = References.PlayerData.classData.rewardPools.Concat(new[] { aquaItemPool, aquaUnitPool, aquaCharmPool }).ToArray();
                addCards.AddRange(DataList<CardData>("goddessStaff", "goddessStaff", "godRequiem", "godblow", "breakSpell", "refresh", "haste", "holyAura").Select(c => c.Clone()));
            }
            if (References.LeaderData.original == TryGet<CardData>("darkness"))
            {
                References.PlayerData.classData.rewardPools = References.PlayerData.classData.rewardPools.Concat(new[] { darknessItemPool, darknessUnitPool, darknessCharmPool }).ToArray();
                addCards.AddRange(DataList<CardData>("dustinessSword", "dustinessSword", "dustinessSword", "dustinessSword", "adamantiteArmor", "meatWall", "vanirMask", "decoy").Select(c => c.Clone()));
            }
            if (References.LeaderData.original == TryGet<CardData>("megumin"))
            {
                References.PlayerData.classData.rewardPools = References.PlayerData.classData.rewardPools.Concat(new[] { meguminItemPool, meguminUnitPool, meguminCharmPool }).ToArray();
                addCards.AddRange(DataList<CardData>("chomusuke", "manatiteRod", "manatite", "eyePatch", "firePotion", "firePotion", "icePotion", "icePotion", "darkPotion").Select(c => c.Clone()));
            }
            if (References.LeaderData.original == TryGet<CardData>("kazuma"))
            {
                References.PlayerData.classData.rewardPools = References.PlayerData.classData.rewardPools.Concat(new[] { kazumaItemPool, kazumaUnitPool, kazumaCharmPool }).ToArray();
                addCards.AddRange(DataList<CardData>("chunchunmaru", "chunchunmaru", "chunchunmaru", "furiezu", "furiezu", "steal", "drainTouch", "dogde").Select(c => c.Clone()));
            }
            References.PlayerData.inventory.deck.list.AddRange(addCards);
        }

        public override void Load()
        {
            instance = this;
            if (!Addressables.ResourceLocators.Any(r => r is ResourceLocationMap map && map.LocatorId == CatalogPath))
                Addressables.LoadContentCatalogAsync(CatalogPath).WaitForCompletion();

            bullet = (GameObject)Addressables.LoadAssetAsync<UnityEngine.Object>($"Assets/{GUID}/Bullet.prefab").WaitForCompletion();

            if (!preLoaded)
            {
                spriteAsset = HopeUtils.CreateSpriteAsset(
                    Title,
                    directoryWithPNGs: ImagePath("Icons")
                );
                CreateModAssets();
            }
            SpriteAsset.RegisterSpriteAsset();
            base.Load();
            CreateLocalizedStrings();

            Events.OnEntityCreated += FixImage;
            Events.OnCampaignInit += CampaignInit;

            VFXHelper.SFX = new SFXLoader(ImagePath("Sounds"));
            VFXHelper.SFX.RegisterAllSoundsToGlobal();
            // VFXHelper.VFX = new GIFLoader(this, ImagePath("Animations"));
            // VFXHelper.VFX.RegisterAllAsApplyEffect();

            GameMode gameMode = TryGet<GameMode>("GameModeNormal");
            gameMode.classes = gameMode.classes.Append(TryGet<ClassData>("Konosuba")).ToArray();
        }

        public override void Unload()
        {
            base.Unload();

            SpriteAsset.UnRegisterSpriteAsset();

            Events.OnEntityCreated -= FixImage;
            Events.OnCampaignInit -= CampaignInit;

            GameMode gameMode = TryGet<GameMode>("GameModeNormal");
            gameMode.classes = RemoveNulls(gameMode.classes);
            UnloadFromClasses();
        }

        public void UnloadFromClasses()
        {
            List<ClassData> tribes = AddressableLoader.GetGroup<ClassData>("ClassData");
            foreach (ClassData tribe in tribes)
            {
                if (tribe == null || tribe.rewardPools == null)
                {
                    continue;
                } //This isn't even a tribe; skip it.

                foreach (RewardPool pool in tribe.rewardPools)
                {
                    if (pool == null)
                    {
                        continue;
                    }
                    ; //This isn't even a reward pool; skip it.

                    pool.list.RemoveAllWhere((item) => item == null || item.ModAdded == this); //Find and remove everything that needs to be removed.
                }
            }
        }

        internal T[] RemoveNulls<T>(T[] data)
            where T : DataFile
        {
            List<T> list = data.ToList();
            list.RemoveAll(x => x == null || x.ModAdded == this);
            return list.ToArray();
        }

        private RewardPool CreateRewardPool(string name, string type, DataFile[] list)
        {
            RewardPool pool = new Scriptable<RewardPool>();
            pool.name = name;
            pool.type = type;
            pool.list = list.ToList();
            return pool;
        }

        private void FixImage(Entity entity)
        {
            if (entity.display is Card card && !card.hasScriptableImage) //These cards should use the static image
            {
                card.mainImage.gameObject.SetActive(true); //And this line turns them on
            }
        }

        public string TribeTitleKey => GUID + ".TribeTitle";
        public string TribeDescKey => GUID + ".TribeDesc";

        //Call this method in Load()

        private void CreateLocalizedStrings()
        {
            StringTable uiText = LocalizationHelper.GetCollection(
                "UI Text",
                SystemLanguage.English
            );
            uiText.SetString(TribeTitleKey, "Kono Subarashii Sekai ni Shukufuku wo!"); //Create the title
            uiText.SetString(
                TribeDescKey,
                "After dying a laughable and pathetic death on his way back from buying a game, high school student and recluse Kazuma Satou finds himself sitting before a beautiful but obnoxious goddess named Aqua. She provides the NEET with two options: continue on to heaven or reincarnate in every gamer's dream—a real fantasy world! Choosing to start a new life, Kazuma is quickly tasked with defeating a Demon King who is terrorizing villages. But before he goes, he can choose one item of any kind to aid him in his quest, and the future hero selects Aqua. But Kazuma has made a grave mistake—Aqua is completely useless!"
            ); //Create the description.
        }

        public override List<T> AddAssets<T, Y>()
        {
            if (assets.OfType<T>().Any())
                Debug.LogWarning(
                    $"[{Title}] adding {typeof(Y).Name}s: {assets.OfType<T>().Select(a => a._data.name).Join()}"
                );
            return assets.OfType<T>().ToList();
        }

        // public TargetConstraint TryGetConstraint(string name)
        // {
        //     if (!allConstraint.Keys.Contains(name))
        //         throw new Exception($"TryGetConstraint Error: Could not find a [{typeof(TargetConstraint).Name}] with the name [{name}]");
        //     return allConstraint[name];
        // }

        public T[] DataList<T>(params string[] names)
            where T : DataFile => names.Select((s) => TryGet<T>(s)).ToArray();

        public T TryGet<T>(string name)
            where T : DataFile
        {
            T data;
            if (typeof(StatusEffectData).IsAssignableFrom(typeof(T)))
                data = base.Get<StatusEffectData>(name) as T;
            else if (typeof(KeywordData).IsAssignableFrom(typeof(T)))
                data = base.Get<KeywordData>(name.ToLower()) as T;
            else
                data = base.Get<T>(name);

            if (data == null)
                throw new Exception(
                    $"TryGet Error: Could not find a [{typeof(T).Name}] with the name [{name}] or [{Extensions.PrefixGUID(name, this)}]"
                );
            return data;
        }

        public CampaignNodeTypeBuilder NodeCopy(string oldName, string newName) =>
            DataCopy<CampaignNodeType, CampaignNodeTypeBuilder>(oldName, newName);

        public ClassDataBuilder TribeCopy(string oldName, string newName) =>
            DataCopy<ClassData, ClassDataBuilder>(oldName, newName);

        public CardData.TraitStacks TStack(string name, int amount) =>
            new CardData.TraitStacks(TryGet<TraitData>(name), amount);

        public CardData.StatusEffectStacks SStack(string name, int amount) =>
            new CardData.StatusEffectStacks(TryGet<StatusEffectData>(name), amount);

        public StatusEffectDataBuilder StatusCopy(string oldName, string newName)
        {
            StatusEffectData data = TryGet<StatusEffectData>(oldName).InstantiateKeepName();
            data.name = GUID + "." + newName;
            data.targetConstraints = new TargetConstraint[0];
            StatusEffectDataBuilder builder = data.Edit<
                StatusEffectData,
                StatusEffectDataBuilder
            >();
            builder.Mod = this;
            return builder;
        }

        public T DataCopy<Y, T>(string oldName, string newName)
            where Y : DataFile
            where T : DataFileBuilder<Y, T>, new()
        {
            Y data = Get<Y>(oldName).InstantiateKeepName();
            data.name = GUID + "." + newName;
            T builder = data.Edit<Y, T>();
            builder.Mod = this;
            return builder;
        }
    }
}
