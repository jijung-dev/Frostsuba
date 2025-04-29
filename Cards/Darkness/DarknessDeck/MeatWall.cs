using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class MeatWall : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("meatWall", "Meat Wall")
			.SetSprites("MeatWall.png", "Item_BG.png")
			.WithText("<card=frostsuba.darkness>".Process())
			.SetStats(null, 0, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Scrap", 2), SStack("Shell", 5) };
				data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
				data.targetConstraints = new TargetConstraint[]
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