using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class AdamantiteArmor : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("adamantiteArmor", "Adamantite Armor")
			.SetSprites("AdamantiteArmor.png", "Item_BG.png")
			.SetStats(null, 1, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Shell", 3) };
			})
			.AddToAsset(this);
	}
}