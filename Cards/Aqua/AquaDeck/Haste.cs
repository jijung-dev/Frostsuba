using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Haste : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("haste", "Haste")
			.SetSprites("Haste.png", "Item_BG.png")
			.SetStats(null, 0, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Reduce Counter", 2) };
			})
			.AddToAsset(this);
	}
}