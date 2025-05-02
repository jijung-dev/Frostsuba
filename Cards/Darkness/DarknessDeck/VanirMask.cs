using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class VanirMask : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("vanirMask", "Vanir Mask")
			.SetSprites("VanirMask.png", "Item_BG.png")
			.SetStats(null, 1, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Block", 1), SStack("Null", 3) };
			})
			.AddToAsset(this);
	}
}