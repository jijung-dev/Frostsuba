using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class GoddessStaff : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("goddessStaff", "Goddess Staff")
			.SetSprites("GoddessStaff.png", "Item_BG.png")
			.SetStats(null, 1, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Apply Random Negative Status", 2) };
			})
			.AddToAsset(this);
	}
}