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
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("Pre Trigger Steal Random Ally Attack", 1),
					SStack("On Kill Draw", 1),
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

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectTemporaryIncreaseAttackPreTrigger>("Pre Trigger Steal Random Ally Attack")
		.WithText("Panty-Stealing Devil<hiddenkeyword=frostsuba.pantystealingdevil>".Process())
		.WithOrder(0)
		.SubscribeToAfterAllBuildEvent<StatusEffectTemporaryIncreaseAttackPreTrigger>(data =>
			{
				data.oncePerTurn = false;
                data.descColorHex = new Color(0.3216f, 0.6118f, 0.6392f).ToHexRGB();
				data.effectToApply = TryGet<StatusEffectData>("Double Attack");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
			})
		.AddToAsset(this);
	}
	protected override void CreateKeyword()
    {
        new KeywordDataBuilder(mod)
            .Create("pantystealingdevil")
            .WithTitle("Panty-Stealing Devil")
            .WithShowName(true)
            .WithDescription("Before attacking temporary steal <keyword=attack> of a random ally".Process())
            .WithTitleColour(new Color(0.3216f, 0.6118f, 0.6392f))
            .WithBodyColour(new Color(1f, 1f, 1f))
            .WithCanStack(false)
            .AddToAsset(this);
    }
}