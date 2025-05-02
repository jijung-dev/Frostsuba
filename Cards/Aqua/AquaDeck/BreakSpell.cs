using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class BreakSpell : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("breakSpell", "Break Spell")
			.SetSprites("BreakSpell.png", "Item_BG.png")
			.WithText("<keyword=cleanse>")
			.SetStats(null, null, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Cleanse", 1), SStack("Heal", 2) };
			})
			.AddToAsset(this);
	}
}