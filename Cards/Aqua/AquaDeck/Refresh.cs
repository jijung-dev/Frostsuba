using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Refresh : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("refresh", "Refresh")
			.SetSprites("Refresh.png", "Item_BG.png")
			.SetStats(null, null, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.needsTarget = false;
				data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("On Card Played Charge Bell", 1) };
				data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1), TStack("Noomlin", 1) };
			})
			.AddToAsset(this);
	}
	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnCardPlayed>("On Card Played Charge Bell")
		.WithText("Charge <keyword=redrawbell>".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(data =>
			{
				data.effectToApply = TryGet<StatusEffectData>("Instant Charge Bell");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectInstantReduceBellCounter>("Instant Charge Bell")
		.SubscribeToAfterAllBuildEvent<StatusEffectInstantReduceBellCounter>(data =>
			{
				data.toZero = true;
			})
		.AddToAsset(this);
	}
}