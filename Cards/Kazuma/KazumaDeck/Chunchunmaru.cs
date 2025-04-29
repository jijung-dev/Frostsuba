using Deadpan.Enums.Engine.Components.Modding;

public class Chunchunmaru : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("chunchunmaru", "Chunchunmaru")
			.SetSprites("Chunchunmaru.png", "Item_BG.png")
			.SetStats(null, 2, 0)
			.WithCardType("Item")
			.AddToAsset(this);
	}
}