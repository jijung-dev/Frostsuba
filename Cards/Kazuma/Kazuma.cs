using System.Collections.Generic;
using System.Data.Common;
using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class Kazuma : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("kazuma", "Kazuma")
			.SetSprites("Kazuma.png", "Kazuma_BG.png")
			.SetStats(8, 3, 4)
			.WithCardType("Leader")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade() };
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Frost", 1) };
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("On Kill Apply Attack To Self", 2),
				};
			})
			.AddToAsset(this);

		new CardDataBuilder(mod)
			.CreateItem("steal", "S T E A L")
			.SetSprites("Steal.png", "Item_BG.png")
			.SetStats(null, null, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
				data.targetConstraints = new TargetConstraint[]
				{
					new Scriptable<TargetConstraintDoesDamage>()
				};
				data.createScripts = new CardScript[] { LeaderExt.GiveCharacterEffect("Kazuma") };
			})
			.AddToAsset(this);
	}
	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectSteal>("StealHeal")
		.WithText("Steal all <keyword=attack> and apply same amount to <card=frostsuba.kazuma> and restore <{a}><keyword=health> to allies".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectSteal>(data =>
			{
				data.effect = TryGet<StatusEffectData>("Heal");
				data.increase = TryGet<StatusEffectInstantIncreaseAttack>("Double Attack");
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectSteal>("StealCounter")
		.WithText("Steal all <keyword=attack> and apply same amount to <card=frostsuba.kazuma> and count down <{a}><keyword=counter> to allies".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectSteal>(data =>
			{
				data.effect = TryGet<StatusEffectData>("Reduce Counter");
				data.increase = TryGet<StatusEffectInstantIncreaseAttack>("Double Attack");
			})
		.AddToAsset(this);
	}
}