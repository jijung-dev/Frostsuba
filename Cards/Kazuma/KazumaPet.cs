using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class KazumaPet : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("kazumaPet", "Kazuma")
			.IsPet("", true)
			.SetSprites("Kazuma_Pet.png", "Item_BG.png")
			.SetStats(4, 3, 4)
			.WithCardType("Friendly")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("For Keyword Kazuma Pet", 1),
					SStack("On Kill Apply Attack To Self Kazuma", 1),
				};
			})
			.AddToAsset(this);
	}
	protected override void CreateStatusEffect()
	{
		StatusCopy("On Kill Apply Attack To Self", "On Kill Apply Attack To Self Kazuma")
		.WithText("Gain <+{a}><keyword=attack> on kill".Process())
		.AddToAsset(this);
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectEmpty>("For Keyword Kazuma Pet")
		.WithText("Shut-in NEET<hiddenkeyword=frostsuba.shutinneetpet>".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectEmpty>(data =>
			{
				data.descColorHex = new Color(0.3216f, 0.6118f, 0.6392f).ToHexRGB();
			})
		.AddToAsset(this);
	}
	protected override void CreateKeyword()
	{
		new KeywordDataBuilder(mod)
			.Create("shutinneetpet")
			.WithTitle("Shut-in NEET")
			.WithShowName(true)
			.WithDescription("A sad little guy".Process())
			.WithTitleColour(new Color(0.3216f, 0.6118f, 0.6392f))
			.WithBodyColour(new Color(1f, 1f, 1f))
			.WithCanStack(false)
			.AddToAsset(this);
	}
}