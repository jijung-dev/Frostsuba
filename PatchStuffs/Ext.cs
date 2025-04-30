using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

//https://konosuba-fda.netlify.app/cards
public static class Ext
{
    public static CardDataBuilder AddToAsset(this CardDataBuilder data, DataBase database)
    {
        database.assets.Add(data);
        return data;
    }

    public static StatusEffectDataBuilder AddToAsset(
        this StatusEffectDataBuilder data,
        DataBase database
    )
    {
        database.assets.Add(data);
        return data;
    }

    public static KeywordDataBuilder AddToAsset(this KeywordDataBuilder data, DataBase database)
    {
        database.assets.Add(data);
        return data;
    }

    public static TraitDataBuilder AddToAsset(this TraitDataBuilder data, DataBase database)
    {
        database.assets.Add(data);
        return data;
    }

    public static string Process(this string text)
    {
        return Regex.Replace(
            text,
            @"<(card|keyword|hiddencard|hiddenkeyword)=frostsuba\.(.*?)>",
            match =>
            {
                string prefix = match.Groups[1].Value;
                string name = match.Groups[2].Value;

                return $"<{prefix}=tgestudio.wildfrost.frostsuba.{name}>";
            }
        );
    }

    public static string RemoveHidden(string text)
    {
        StringBuilder sb = new StringBuilder(text);
        string[] hiddenTags = { "<hiddencard=", "<hiddenkeyword=" };
        int start;

        foreach (var tag in hiddenTags)
        {
            while ((start = sb.ToString().IndexOf(tag)) != -1)
            {
                int end = sb.ToString().IndexOf(">", start);
                if (end == -1)
                    break; // Safety check

                sb.Remove(start, end - start + 1);
            }
        }

        return string.Join(
            "\n",
            sb.ToString().Split('\n').Where(line => !string.IsNullOrWhiteSpace(line))
        );
    }

}

public class Scriptable<T>
    where T : ScriptableObject, new()
{
    readonly Action<T> modifier;

    public Scriptable() { }

    public Scriptable(Action<T> modifier)
    {
        this.modifier = modifier;
    }

    public static implicit operator T(Scriptable<T> scriptable)
    {
        T result = ScriptableObject.CreateInstance<T>();
        scriptable.modifier?.Invoke(result);
        return result;
    }
}
