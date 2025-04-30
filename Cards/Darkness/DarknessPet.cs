using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class DarknessPet : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("darknessPet", "Darkness")
			.IsPet("", true)
			.SetSprites("Darkness_Pet.png", "Item_BG.png")
			.SetStats(8, 1, 5)
			.WithCardType("Friendly")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("When Health Lost Gain Immune To All Negative Status", 6),
					SStack("Hit Random Unit", 1),
					SStack("When Hit Gain Attack To Self (No Ping)", 1),
				};
			})
			.AddToAsset(this);
	}
	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXWhenHealthLost>("When Health Lost Gain Immune To All Negative Status")
		.WithText("Perverted Crusader<hiddenkeyword=frostsuba.pervertedcrusaderpet>".Process())
		.WithOrder(0)
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHealthLost>(data =>
			{
				data.descColorHex = new Color(1.00f, 0.79f, 0.34f).ToHexRGB();
				data.effectToApply = TryGet<StatusEffectData>("Immune To All Negative Status");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
				data.hasThreshold = true;
			})
		.AddToAsset(this);
	}
	protected override void CreateKeyword()
	{
		new KeywordDataBuilder(mod)
			.Create("pervertedcrusaderpet")
			.WithTitle("Perverted Crusader")
			.WithShowName(true)
			.WithDescription("After 6<keyword=health> is lost, gain immunity to all <keyword=frostsuba.negativestatus>".Process())
			.WithTitleColour(new Color(1.00f, 0.79f, 0.34f))
			.WithBodyColour(new Color(1f, 1f, 1f))
			.WithCanStack(false)
			.AddToAsset(this);
	}
}