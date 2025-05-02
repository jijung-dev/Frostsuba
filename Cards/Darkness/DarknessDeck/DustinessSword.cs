using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class DustinessSword : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("dustinessSword", "Dustiness Sword")
			.SetSprites("DustinessSword.png", "Item_BG.png")
			.SetStats(null, 2, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.traits = new List<CardData.TraitStacks> { TStack("Aimless", 1) };
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("MultiHit",1)
				};
			})
			.AddToAsset(this);
	}
}