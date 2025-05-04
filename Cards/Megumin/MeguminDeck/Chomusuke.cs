using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class Chomusuke : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("chomusuke", "Chomusuke")
			.SetSprites("Chomusuke.png", "Megumin_BG.png")
			.SetStats(1, 1, 5)
			.WithCardType("Friendly")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.traits = new List<CardData.TraitStacks>() { TStack("Fragile", 1) };
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("On Turn Count Down AllyInFront", 1),
				};
			})
			.AddToAsset(this);
	}
	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnTurn>("On Turn Count Down AllyInFront")
		.WithText("Count down <keyword=counter> of ally in front by <{a}>")
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
			{
				data.canBeBoosted = true;
				data.effectToApply = TryGet<StatusEffectData>("Reduce Counter");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.AllyInFrontOf;
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnTurn>("On Turn Count Down Allies")
		.WithText("Count down <keyword=counter> of allies by <{a}>")
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
			{
				data.canBeBoosted = true;
				data.effectToApply = TryGet<StatusEffectData>("Reduce Counter");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
			})
		.AddToAsset(this);
	}
	protected override void CreateFinalSwapAsset()
    {
        var scripts = new CardScript[]
        {
            new Scriptable<CardScriptRemovePassiveEffect>( r => r.toRemove = new StatusEffectData[]
            {
                TryGet<StatusEffectData>("On Turn Count Down AllyInFront"),
            }),
            new Scriptable<CardScriptAddPassiveEffect>( r =>
                {
                    r.effect = TryGet<StatusEffectData>("On Turn Count Down Allies");
                    r.countRange = new Vector2Int(1, 1);
                }
            ),
        };
        finalSwapAsset = (TryGet<CardData>("chomusuke"), scripts, null);

    }
}