using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Decoy : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("decoy", "Decoy")
			.SetSprites("Decoy.png", "Item_BG.png")
			.SetStats(null, null, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.needsTarget = false;
				data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("Pre Trigger Apply Block To Darkness", 3),
					SStack("On Card Played Trigger Enemies", 1),
				};
			})
			.AddToAsset(this);
	}
	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnCardPlayed>("On Card Played Trigger Enemies")
		.WithText("Trigger Enemies")
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(data =>
			{
				data.effectToApply = TryGet<StatusEffectData>("Trigger");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
			})
		.AddToAsset(this);
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXPreTrigger>("Pre Trigger Apply Block To Darkness")
		.WithText("Apply <{a}><keyword=block> to <card=frostsuba.darkness>".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXPreTrigger>(data =>
			{
				data.effectToApply = TryGet<StatusEffectData>("Block");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
				data.applyConstraints = new TargetConstraint[]
				{
					new Scriptable<TargetConstraintIsSpecificCard>(r => r.allowedCards = new []
					{
						TryGet<CardData>("darkness"),
					})
				};
			})
		.AddToAsset(this);
	}
}