using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class FirePotion : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("firePotion", "Fire Potion")
			.SetSprites("FirePotion.png", "Item_BG.png")
			.SetStats(null, 2, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Spice", 3) };
			})
			.AddToAsset(this);
	}
}