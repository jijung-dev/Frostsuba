using System;
using System.Collections.Generic;
using HarmonyLib;
using Konosuba;
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
