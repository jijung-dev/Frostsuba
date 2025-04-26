using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class Darkness : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("darkness", "Darkness")
			.SetSprites("Darkness.png", "Darkness_BG.png")
			.SetStats(12, 1, 5)
			.WithCardType("Leader")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade(), LeaderExt.GiveCharacterEffect("Darkness") };
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
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
		.Create<StatusEffectTaunt>("TauntBlock")
		.WithText("<keyword=frostsuba.taunt>".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectTaunt>(data =>
			{
				data.blockEffect = SStack("Block", 3);
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXWhenHealthLost>("When Health Lost Gain Taunt Block")
		.WithText("<keyword=frostsuba.pervertedcrusaderblock>".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHealthLost>(data =>
			{
				data.effectToApply = TryGet<StatusEffectData>("TauntBlock");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
				data.hasThreshold = true;
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectTaunt>("TauntShell")
		.WithText("<keyword=frostsuba.taunt>".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectTaunt>(data =>
			{
				data.blockEffect = SStack("Shell", 10);
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXWhenHealthLost>("When Health Lost Gain Taunt Shell")
		.WithText("<keyword=frostsuba.pervertedcrusadershell>".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHealthLost>(data =>
			{
				data.effectToApply = TryGet<StatusEffectData>("TauntShell");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
				data.hasThreshold = true;
			})
		.AddToAsset(this);
	}

	protected override void CreateKeyword()
	{
		new KeywordDataBuilder(mod)
			.Create("pervertedcrusaderblock")
			.WithTitle("Perverted Crusader")
			.WithShowName(true)
			.WithDescription("After 8<keyword=health> is lost, gain <keyword=frostsuba.taunt> and 3<keyword=block>".Process())
			.WithTitleColour(new Color(1.00f, 0.79f, 0.34f))
			.WithBodyColour(new Color(1f, 1f, 1f))
			.WithCanStack(false)
			.AddToAsset(this);

		new KeywordDataBuilder(mod)
			.Create("pervertedcrusadershell")
			.WithTitle("Perverted Crusader")
			.WithShowName(true)
			.WithDescription("After 8<keyword=health> is lost, gain <keyword=frostsuba.taunt> and 10<keyword=shell>".Process())
			.WithTitleColour(new Color(1.00f, 0.79f, 0.34f))
			.WithBodyColour(new Color(1f, 1f, 1f))
			.WithCanStack(false)
			.AddToAsset(this);

		new KeywordDataBuilder(mod)
			.Create("taunt")
			.WithTitle("Taunt")
			.WithShowName(true)
			.WithDescription("All enemies targeting allies will target this card instead".Process())
			.WithTitleColour(new Color(1.00f, 0.79f, 0.34f))
			.WithBodyColour(new Color(1f, 1f, 1f))
			.WithCanStack(false)
			.AddToAsset(this);
	}
}