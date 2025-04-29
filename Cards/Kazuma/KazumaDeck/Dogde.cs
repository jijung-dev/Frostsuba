using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Dogde : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("dogde", "Dogde")
			.SetSprites("Dogde.png", "Item_BG.png")
			.SetStats(null, 1, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Reduce Counter", 2) };
			})
			.AddToAsset(this);
	}
}