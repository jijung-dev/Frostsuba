using Deadpan.Enums.Engine.Components.Modding;

public class Furiezu : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("furiezu", "Freeze")
			.SetSprites("Furiezu.png", "Item_BG.png")
			.SetStats(null, 0, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Frost", 1), SStack("Snow", 2) };
			})
			.AddToAsset(this);
	}
}