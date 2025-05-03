using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.Utils;

public class Yunyun : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("yunyun", "Yunyun")
			.SetSprites("Yunyun.png", "Item_BG.png")
			.SetStats(5, 2, 4)
			.AddPool("GeneralUnitPool")
			.WithCardType("Friendly")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("MultiHit", 1),
				};
				data.createScripts = new CardScript[] { LeaderExt.GiveCharacterEffect("Yunyun") };
			})
			.AddToAsset(this);
	}

	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnTurn>("On Turn Count Down Ally Behind")
		.WithText("Count down <keyword=counter> an ally behind by <{a}>")
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
			{
				data.effectToApply = TryGet<StatusEffectData>("Reduce Counter");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.AllyBehind;
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnTurn>("On Turn Apply Spice To Ally Behind")
		.WithText("Apply <{a}><keyword=spice> to an ally behind")
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
			{
				data.effectToApply = TryGet<StatusEffectData>("Spice");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.AllyBehind;
			})
		.AddToAsset(this);
	}
}
