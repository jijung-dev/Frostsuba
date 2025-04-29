using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class IcePotion : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("icePotion", "Ice Potion")
			.SetSprites("IcePotion.png", "Item_BG.png")
			.SetStats(null, 2, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Snow", 2) };
			})
			.AddToAsset(this);
	}
}