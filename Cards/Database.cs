using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using Konosuba;
using UnityEngine;
using static FinalBossGenerationSettings;

public abstract class DataBase
{
    public List<object> assets = new List<object>();
    protected (CardData, CardScript[], CardData[]) finalSwapAsset;

    public static readonly List<Type> subclasses;

    protected Frostsuba mod => Frostsuba.instance;

    protected T TryGet<T>(string name)
        where T : DataFile => mod.TryGet<T>(name);

    protected CardData.TraitStacks TStack(string name, int amount) => mod.TStack(name, amount);

    protected CardData.StatusEffectStacks SStack(string name, int amount) =>
        mod.SStack(name, amount);

    protected StatusEffectDataBuilder StatusCopy(string oldName, string newName) =>
        mod.StatusCopy(oldName, newName);

    public virtual void CreateCard() { }

    protected virtual void CreateStatusEffect() { }

    protected virtual void CreateTrait() { }

    protected virtual void CreateKeyword() { }

    protected virtual void CreateIcon() { }

    protected virtual void CreateOther() { }

    protected virtual void CreateFinalSwapAsset() { }

    public List<object> CreateAssest()
    {
        CreateStatusEffect();
        CreateTrait();
        CreateKeyword();
        CreateIcon();
        CreateCard();
        CreateOther();
        return assets;
    }

    public FinalBossCardModifier CreateFinalEffectSwap()
    {
        CreateFinalSwapAsset();
        if (finalSwapAsset.Item1 == null)
            return null;

        FinalBossCardModifier cardModifier = new Scriptable<FinalBossCardModifier>(r => r.card = finalSwapAsset.Item1);
        if (finalSwapAsset.Item2 == null)
            return null;

        var scripts = finalSwapAsset.Item2;
        cardModifier.runAll = scripts;
        return cardModifier;
    }
    public ReplaceCard CreateFinalCardSwap()
    {
        if (finalSwapAsset.Item1 == null)
            return new ReplaceCard();

        ReplaceCard cardReplace = new ReplaceCard();
        cardReplace.card = finalSwapAsset.Item1;
        if (finalSwapAsset.Item3 == null)
            return new ReplaceCard();

        cardReplace.options = finalSwapAsset.Item3;
        return cardReplace;
    }

    static DataBase()
    {
        subclasses = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(DataBase)))
            .ToList();
    }
}
