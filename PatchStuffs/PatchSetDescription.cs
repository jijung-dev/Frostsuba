using System;
using System.Collections.Generic;
using HarmonyLib;
using Konosuba;
using UnityEngine;
using static Text;

[HarmonyPatch(typeof(Text), nameof(Text.GetMentionedCards), typeof(string))]
internal static class PatchSetDescription
{
    internal static bool Prefix(ref HashSet<CardData> __result, string text)
    {
        HashSet<CardData> hashSet = new HashSet<CardData>();
        for (int i = 0; i < text.Length; i++)
        {
            if (!text[i].Equals('<'))
            {
                continue;
            }

            string text2 = FindTag(text, i);
            if (text2.Length > 0 && text2.Contains("="))
            {
                string[] array = text2.Split('=');
                if (
                    array.Length == 2
                    && (array[0].Trim() == "card" || array[0].Trim() == "hiddencard")
                )
                {
                    CardData item = Frostsuba.instance.TryGet<CardData>(array[1].Trim());
                    hashSet.Add(item);
                }
            }
        }

        __result = hashSet;
        return false;
    }
}
[HarmonyPatch(typeof(Text), nameof(Text.GetKeywords), typeof(string))]
internal static class PatchSetDescription3
{
    internal static bool Prefix(ref HashSet<KeywordData> __result, string text)
    {
        HashSet<KeywordData> hashSet = new HashSet<KeywordData>();
        for (int i = 0; i < text.Length; i++)
        {
            if (!text[i].Equals('<'))
            {
                continue;
            }

            string text2 = FindTag(text, i);
            if (text2.Length <= 0 || !text2.Contains("="))
            {
                continue;
            }

            string[] array = text2.Split('=');
            if (array.Length == 2 && (array[0].Trim() == "keyword" || array[0].Trim() == "hiddenkeyword"))
            {
                KeywordData keywordData = ToKeywordExt(array[1].Trim());
                if (keywordData.show)
                {
                    hashSet.Add(keywordData);
                }
            }
        }

        __result = hashSet;
        return false;
    }
    public static KeywordData ToKeywordExt(string text)
	{
		int num = text.IndexOf(' ');
		if (num > 0)
		{
			text = text.Remove(num, text.Length - num);
		}
		KeywordData keywordData = Frostsuba.instance.TryGet<KeywordData>(text);
		Debug.Log((keywordData != null) ? ("Keyword for \"" + text + "\" = [" + keywordData.name + "]") : ("Keyword for \"" + text + "\" = NULL"));
		if (!keywordData)
		{
			Debug.LogError("Keyword \"" + text + "\" not found!");
		}

		return keywordData;
	}
}

[HarmonyPatch(
    typeof(Text),
    nameof(Text.Process),
    new Type[] { typeof(string), typeof(int), typeof(float), typeof(ColourProfileHex) }
)]
internal static class PatchSetDescription2
{
    static void Prefix(ref string original)
    {
        original = Ext.RemoveHidden(original).Trim();
    }
}
