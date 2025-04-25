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

        // public static string CatalogFolder
        //     => Path.Combine(instance.ModDirectory, "Windows");

        // // A helpful shortcut
        // public static string CatalogPath
        //     => Path.Combine(CatalogFolder, "catalog.json");

        public override TMP_SpriteAsset SpriteAsset => spriteAsset;
        internal static TMP_SpriteAsset spriteAsset;

        private bool preLoaded = false;

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
            // itemWithResource = CreateRewardPool("DSTItemPoolR", "Items", FrostsubaExt.DataList<DataFile>());
            // unitWithResource = CreateRewardPool("DSTUnitPoolR", "Units", DataList<DataFile>());
            // charmWithResource = CreateRewardPool("DSTCharmPoolR", "Charms", DataList<DataFile>());
            // itemWithoutResource = CreateRewardPool("DSTItemPool", "Items", DataList<DataFile>());
            // unitWithoutResource = CreateRewardPool("DSTUnitPool", "Units", DataList<DataFile>());
            // charmWithoutResource = CreateRewardPool("DSTCharmPool", "Charms", DataList<DataFile>());

            // assets.Add(
            //     TribeCopy("Magic", "DST")
            //         .WithFlag(flagSprite)
            //         .WithSelectSfxEvent(FMODUnity.RuntimeManager.PathToEventReference("event:/sfx/card/summon"))
            //         .SubscribeToAfterAllBuildEvent(
            //             (data) =>
            //             {
            //                 GameObject gameObject = data.characterPrefab.gameObject.InstantiateKeepName();
            //                 UnityEngine.Object.DontDestroyOnLoad(gameObject);
            //                 gameObject.name = "Player (dstmod.DST)";
            //                 data.characterPrefab = gameObject.GetComponent<Character>();
            //                 data.id = "dstmod.DST";

            //                 data.leaders = DataList<CardData>("wendy", "wortox", "winona", "wolfgang", "wormwood");

            //                 Inventory inventory = new Scriptable<Inventory>();
            //                 inventory.deck.list = DataList<CardData>("spear", "spear", "hamBat", "iceStaff", "boosterShot", "walkingCane").ToList();
            //                 data.startingInventory = inventory;

            //                 data.rewardPools = new RewardPool[]
            //                 {
            //                     Extensions.GetRewardPool("GeneralUnitPool"),
            //                     Extensions.GetRewardPool("GeneralItemPool"),
            //                     Extensions.GetRewardPool("GeneralCharmPool"),
            //                     Extensions.GetRewardPool("GeneralModifierPool"),
            //                 };
            //             }
            //         )
            // );
            #endregion

            preLoaded = true;
        }

        public override void Load()
        {
            instance = this;
            // if (!Addressables.ResourceLocators.Any(r => r is ResourceLocationMap map && map.LocatorId == CatalogPath))
            //     Addressables.LoadContentCatalogAsync(CatalogPath).WaitForCompletion();

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

            VFXHelper.SFX = new SFXLoader(ImagePath("Sounds"));
            VFXHelper.SFX.RegisterAllSoundsToGlobal();
            // VFXHelper.VFX = new GIFLoader(this, ImagePath("Animations"));
            // VFXHelper.VFX.RegisterAllAsApplyEffect();

            // GameMode gameMode = TryGet<GameMode>("GameModeNormal");
            // gameMode.classes = gameMode.classes.Append(TryGet<ClassData>("Konosuba")).ToArray();
        }

        public override void Unload()
        {
            base.Unload();

            SpriteAsset.UnRegisterSpriteAsset();

            Events.OnEntityCreated -= FixImage;

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
            uiText.SetString(TribeTitleKey, "The Survivor"); //Create the title
            uiText.SetString(
                TribeDescKey,
                "The night devours the unprepared. The earth offers life but demands struggle. We gather, hunt, and build to see another day. "
                    + "Beasts lurk, trees whisper, and the sky brings fire and frost. Fire is safety; darkness is death. "
                    + "We craft tools, weave armor, and wield spears. Kin and foes walk this land, but hunger is our greatest enemy."
                    + "To survive, we must be wise. To falter is to be forgotten."
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
