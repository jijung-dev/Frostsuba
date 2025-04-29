using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class DarkPotion : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("darkPotion", "Dark Potion")
			.SetSprites("DarkPotion.png", "Item_BG.png")
			.SetStats(null, 1, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Frost", 2) };
				data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("Hit All Enemies", 1)
				};
			})
			.AddToAsset(this);
	}
}