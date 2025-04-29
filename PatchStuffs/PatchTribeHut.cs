using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using Konosuba;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

[HarmonyPatch(typeof(TribeHutSequence), "SetupFlags")]
static class PatchTribeHut
{
    static string TribeName = "Konosuba";

    static void Postfix(TribeHutSequence __instance)
    {
        GameObject gameObject = GameObject.Instantiate(__instance.flags[0].gameObject);
        gameObject.transform.SetParent(__instance.flags[0].gameObject.transform.parent, false);
        TribeFlagDisplay flagDisplay = gameObject.GetComponent<TribeFlagDisplay>();
        ClassData tribe = Frostsuba.instance.TryGet<ClassData>(TribeName);
        flagDisplay.flagSprite = tribe.flag;
        __instance.flags = __instance.flags.Append(flagDisplay).ToArray();
        flagDisplay.SetAvailable();
        flagDisplay.SetUnlocked();

        TribeDisplaySequence sequence2 = GameObject.FindObjectOfType<TribeDisplaySequence>(true);
        GameObject gameObject2 = GameObject.Instantiate(sequence2.displays[1].gameObject);
        gameObject2.transform.SetParent(sequence2.displays[2].gameObject.transform.parent, false);
        sequence2.tribeNames = sequence2.tribeNames.Append(TribeName).ToArray();
        sequence2.displays = sequence2.displays.Append(gameObject2).ToArray();

        Button button = flagDisplay.GetComponentInChildren<Button>();
        button.onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
        button.onClick.AddListener(() =>
        {
            sequence2.Run(TribeName);
        });

        //(SfxOneShot)
        gameObject2.GetComponent<SfxOneshot>().eventRef = FMODUnity.RuntimeManager.PathToEventReference("event:/sfx/card/draw_multi");

        //0: Flag (ImageSprite)
        gameObject2.transform.GetChild(0).GetComponent<ImageSprite>().SetSprite(tribe.flag);

        //1: Left (ImageSprite)
        Sprite aqua = Frostsuba.instance.TryGet<CardData>("aqua").mainSprite;
        gameObject2.transform.GetChild(1).GetComponent<ImageSprite>().SetSprite(aqua);

        //2: Right (ImageSprite)
        Sprite kazuma = Frostsuba.instance.TryGet<CardData>("kazuma").mainSprite;
        gameObject2.transform.GetChild(2).GetComponent<ImageSprite>().SetSprite(kazuma);

        //3: Textbox (Image)
        gameObject2.transform.GetChild(3).GetComponent<Image>().color = new Color(0.12f, 0.47f, 0.57f);

        //3-0: Text (LocalizedString)
        StringTable collection = LocalizationHelper.GetCollection("UI Text", SystemLanguage.English);
        gameObject2.transform.GetChild(3).GetChild(0).GetComponent<LocalizeStringEvent>().StringReference = collection.GetString(
            Frostsuba.instance.TribeDescKey
        );
        gameObject2.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().fontSize *= 0.8f; 

        //4:Title Ribbon (Image)
        //4-0: Text (LocalizedString)
        gameObject2.transform.GetChild(4).GetChild(0).GetComponent<LocalizeStringEvent>().StringReference = collection.GetString(
            Frostsuba.instance.TribeTitleKey
        );
        gameObject2.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().fontSize *= 0.8f; 
    }
}

