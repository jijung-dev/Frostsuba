using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class GodRequiem : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("godRequiem", "God Requiem")
			.SetSprites("GodRequiem.png", "Item_BG.png")
			.SetStats(null, 3, 0)
			.WithCardType("Item")
			.AddToAsset(this);
	}
}