using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.Utils;

public class Vanir : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("vanir", "Vanir")
			.SetSprites("Vanir.png", "Item_BG.png")
			.SetStats(5, 1, 5)
			.WithCardType("Friendly")
			.AddPool("GeneralUnitPool")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Frost", 1) };
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("On Turn Summon A Copy Of Self", 1),
					SStack("When Heal Take Damage Instead", 1),
				};
			})
			.AddToAsset(this);
	}

	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
			.Create<StatusEffectInstantSummon>("Instant Summon Vanir")
			.SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
			{
				data.canSummonMultiple = false;
				data.summonCopy = true;
				data.canBeBoosted = false;
				data.summonPosition = StatusEffectInstantSummon.Position.InFrontOf;
				data.targetSummon = TryGet<StatusEffectSummon>("Summon Plep");
			})
			.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnTurn>("On Turn Summon A Copy Of Self")
		.WithText("Summon a copy of self".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
			{
				data.canBeBoosted = false;
				data.effectToApply = TryGet<StatusEffectData>("Instant Summon Vanir");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXWhenYAppliedToSelfExt>("When Heal Take Damage Instead")
		.WithText("When <keyword=health>'d take damage instead".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenYAppliedToSelfExt>(data =>
			{
				data.effectToApply = TryGet<StatusEffectData>("Instant Damage");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
				data.whenAppliedTypes = new string[] { "heal" };
				data.instead = true;
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectInstantDamage>("Instant Damage")
		.AddToAsset(this);
	}
}
