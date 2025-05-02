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
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Reduce Counter", 5) };
				data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
				data.targetConstraints = new TargetConstraint[]
				{
					new Scriptable<TargetConstraintIsSpecificCard>(r => r.allowedCards = new CardData[]
					{
						TryGet<CardData>("meguminDown")
					})
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