using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Deadpan.Enums.Engine.Components.Modding;

public class DrainTouch : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("drainTouch", "Drain Touch")
			.SetSprites("DrainTouch.png", "Item_BG.png")
			.SetStats(null, null, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Take Health", 2) };
				data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("On Card Played Increase Health Kazuma", 2)
				};
			})
			.AddToAsset(this);
	}
	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnCardPlayed>("On Card Played Increase Health Kazuma")
		.WithText("Take <{a}><keyword=health> increase <card=frostsuba.kazuma>'s <keyword=health> the same amount".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(data =>
			{
				data.effectToApply = TryGet<StatusEffectData>("Increase Max Health");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
				data.applyConstraints = new TargetConstraint[]
				{
					new Scriptable<TargetConstraintIsSpecificCard>(r => r.allowedCards = new CardData[]
					{
						TryGet<CardData>("kazuma")
					}),
				};
			})
		.AddToAsset(this);
	}
}