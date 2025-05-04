using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using static FinalBossGenerationSettings;

public class Manatite : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("manatite", "Manatite")
			.SetSprites("Manatite.png", "Item_BG.png")
			.WithText("<card=frostsuba.meguminDown>".Process())
			.SetStats(null, null, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.needsTarget = false;
				data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
				data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("On Card Played Reduce Counter Megumin Down", 5) };
			})
			.AddToAsset(this);
	}
	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnCardPlayed>("On Card Played Reduce Counter Megumin Down")
		.WithText("Count down <card=frostsuba.meguminDown>'s <keyword=counter> by 5".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(data =>
			{
				data.canBeBoosted = true;
				data.effectToApply = TryGet<StatusEffectData>("Reduce Counter");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
				data.applyConstraints = new TargetConstraint[]
				{
					new Scriptable<TargetConstraintIsSpecificCard>(r => r.allowedCards = new CardData[]
					{
						TryGet<CardData>("meguminDown")
					}),
				};
			})
		.AddToAsset(this);
	}
	protected override void CreateFinalSwapAsset()
	{
		var cards = mod.DataList<CardData>("Madness", "Joob");
		finalSwapAsset = (TryGet<CardData>("manatite"), null, cards);
	}
}