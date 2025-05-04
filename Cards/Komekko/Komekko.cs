using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.Utils;

public class Komekko : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("komekko", "Komekko")
			.SetSprites("Komekko.png", "Item_BG.png")
			.SetStats(2, 1, 5)
			.WithCardType("Friendly")
			.AddPool("GeneralUnitPool")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.createScripts = new CardScript[] { LeaderExt.GiveCharacterEffect("Komekko") };
			})
			.AddToAsset(this);

		new CardDataBuilder(mod)
			.CreateUnit("hostHealth", "Host")
			.SetSprites("Host.png", "Item_BG.png")
			.SetStats(10, 3, 0)
			.WithCardType("Friendly")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("Trigger When Komekko Attacks", 1),
				};
			})
			.AddToAsset(this);

		new CardDataBuilder(mod)
			.CreateUnit("hostDamage", "Host")
			.SetSprites("Host.png", "Item_BG.png")
			.SetStats(3, 10, 0)
			.WithCardType("Friendly")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("Trigger When Komekko Attacks", 1),
				};
			})
			.AddToAsset(this);
	}

	protected override void CreateStatusEffect()
	{

		new StatusEffectDataBuilder(mod)
			.Create<StatusEffectTriggerWhenCertainAllyAttacks>("Trigger When Komekko Attacks")
			.WithText("Trigger when <card=frostsuba.komekko> attacks".Process())
			.SubscribeToAfterAllBuildEvent<StatusEffectTriggerWhenCertainAllyAttacks>(data =>
			{
				data.isReaction = true;
				data.stackable = false;
				data.canBeBoosted = false;
				data.allyInRow = false;
				data.ally = TryGet<CardData>("komekko");
			})
			.AddToAsset(this);

		StatusCopy("Summon Fallow", "Summon Host Health")
			.WithText("Summon <card=frostsuba.hostHealth>".Process())
			.SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
			{
				data.summonCard = TryGet<CardData>("hostHealth");
			})
			.AddToAsset(this);

		StatusCopy("Instant Summon Fallow", "Instant Summon Host Health")
			.WithText("Summon <card=frostsuba.hostHealth>".Process())
			.SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
			{
				data.summonPosition = StatusEffectInstantSummon.Position.InFrontOf;
				data.targetSummon = TryGet<StatusEffectSummon>("Summon Host Health");
			})
			.AddToAsset(this);

		StatusCopy("When Deployed Summon Wowee", "When Deployed Summon Host Health")
			.WithText("When deployed, summon <card=frostsuba.hostHealth>".Process())
			.SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
			{
				data.effectToApply = TryGet<StatusEffectData>("Instant Summon Host Health");
			})
			.AddToAsset(this);

		StatusCopy("Summon Fallow", "Summon Host Damage")
			.WithText("Summon <card=frostsuba.hostDamage>".Process())
			.SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
			{
				data.summonCard = TryGet<CardData>("hostDamage");
			})
			.AddToAsset(this);

		StatusCopy("Instant Summon Fallow", "Instant Summon Host Damage")
			.WithText("Summon <card=frostsuba.hostDamage>".Process())
			.SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
			{
				data.summonPosition = StatusEffectInstantSummon.Position.InFrontOf;
				data.targetSummon = TryGet<StatusEffectSummon>("Summon Host Damage");
			})
			.AddToAsset(this);

		StatusCopy("When Deployed Summon Wowee", "When Deployed Summon Host Damage")
			.WithText("When deployed, summon <card=frostsuba.hostDamage>".Process())
			.SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
			{
				data.effectToApply = TryGet<StatusEffectData>("Instant Summon Host Damage");
			})
			.AddToAsset(this);
	}
}
