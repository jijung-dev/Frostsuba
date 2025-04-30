using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class Darkness : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("darkness", "Darkness")
			.SetSprites("Darkness.png", "Darkness_BG.png")
			.SetStats(null, 1, 5)
			.WithCardType("Leader")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade() };
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("Scrap", 8),
					SStack("When Scrap Lost Gain Immune To All Negative Status", 6),
					SStack("Taunt", 1),
					SStack("Hit Random Unit", 1),
					SStack("When Hit Gain Attack To Self (No Ping)", 1),
				};
			})
			.AddToAsset(this);
	}
	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectChangeTargetMode>("Hit Random Unit")
		.WithText("Hit a random unit")
		.SubscribeToAfterAllBuildEvent<StatusEffectChangeTargetMode>(data =>
			{
				data.targetMode = new Scriptable<TargetModeRandomUnit>();
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectTaunt>("Taunt")
		.WithText("<keyword=frostsuba.taunt>".Process())
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXWhenScrapLost>("When Scrap Lost Gain Immune To All Negative Status")
		.WithText("Perverted Crusader<hiddenkeyword=frostsuba.pervertedcrusader>".Process())
		.WithOrder(0)
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenScrapLost>(data =>
			{
				data.descColorHex = new Color(1.00f, 0.79f, 0.34f).ToHexRGB();
				data.effectToApply = TryGet<StatusEffectData>("Immune To All Negative Status");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
				data.hasThreshold = true;
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectImmuneToXExt>("Immune To All Negative Status")
		.WithText("Immune to all <keyword=frostsuba.negativestatus>".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectImmuneToXExt>(data =>
			{
				data.isNegative = true;
			})
		.AddToAsset(this);
	}

	protected override void CreateKeyword()
	{
		new KeywordDataBuilder(mod)
			.Create("pervertedcrusader")
			.WithTitle("Perverted Crusader")
			.WithShowName(true)
			.WithDescription("After 6<keyword=scrap> is lost, gain immunity to all <keyword=frostsuba.negativestatus>".Process())
			.WithTitleColour(new Color(1.00f, 0.79f, 0.34f))
			.WithBodyColour(new Color(1f, 1f, 1f))
			.WithCanStack(false)
			.AddToAsset(this);

		new KeywordDataBuilder(mod)
			.Create("taunt")
			.WithTitle("Taunt")
			.WithShowName(true)
			.WithDescription("All enemies targeting allies will target this card instead|Make all target change like Barrage or Aimless only aim this card")
			.WithTitleColour(new Color(1.00f, 0.79f, 0.34f))
			.WithBodyColour(new Color(1f, 1f, 1f))
			.WithCanStack(false)
			.AddToAsset(this);
	}
}