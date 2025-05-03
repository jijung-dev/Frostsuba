using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.Utils;

public class Chris : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("chris", "Chris")
			.SetSprites("Chris.png", "Item_BG.png")
			.SetStats(6, 3, 4)
			.AddPool("GeneralUnitPool")
			.WithCardType("Friendly")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Null", 2) };
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("Pre Attack Increase Attack To Self", 1),
				};
			})
			.AddToAsset(this);
	}

	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXWhenTargetCertainCard>("Pre Attack Increase Attack To Self")
		.WithText("Increase <keyword=attack> by <{a}> when targeting <keyword=null>'d unit")
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenTargetCertainCard>(data =>
			{
				data.constraints = new TargetConstraint[]
				{
					new Scriptable<TargetConstraintHasStatus>(r => r.status = TryGet<StatusEffectData>("Null"))
				};
				data.effectToApply = TryGet<StatusEffectData>("Increase Attack");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
			})
		.AddToAsset(this);
	}
}
