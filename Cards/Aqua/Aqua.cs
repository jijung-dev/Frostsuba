using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class Aqua : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("aqua", "Aqua")
			.SetSprites("Aqua.png", "Aqua_BG.png")
			.SetStats(8, null, 4)
			.WithCardType("Leader")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade() };
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("ImmuneToFrenzy", 1),
					SStack("On Turn Apply Random Positive Status To Allies", 2),
					SStack("On Turn Apply Random Negative Status To Allies", 1),
				};
			})
			.AddToAsset(this);

		new CardDataBuilder(mod)
			.CreateItem("godblow", "God Blow")
			.SetSprites("GodBlow.png", "Item_BG.png")
			.SetStats(null, null, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.attackEffects = new CardData.StatusEffectStacks[]
				{
					SStack("God Blow", 1),
				};
				data.targetConstraints = new TargetConstraint[]
				{
					new Scriptable<TargetConstraintIsSpecificCard>(r => r.allowedCards = new CardData[]
					{
						TryGet<CardData>("aqua")
					})
				};
			})
			.AddToAsset(this);
	}
	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXRandomInstant>("Apply Random Positive Status")
		.WithText("Apply <{a}> <keyword=frostsuba.positivestatus>".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXRandomInstant>(data =>
			{
				data.canBeBoosted = true;
				data.eventPriority = -2;
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
			})
		.AddToAsset(this);
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXRandomInstant>("Apply Random Negative Status")
		.WithText("Apply <{a}> <keyword=frostsuba.negativestatus>".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXRandomInstant>(data =>
			{
				data.canBeBoosted = true;
				data.eventPriority = -1;
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
				data.isNegative = true;
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnTurn>("On Turn Apply Random Positive Status To Allies")
		.WithText("Apply <{a}> <keyword=frostsuba.positivestatus>".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
			{
				data.canBeBoosted = true;
				data.effectToApply = TryGet<StatusEffectData>("Apply Random Positive Status");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnTurn>("On Turn Apply Random Negative Status To Allies")
		.WithText("and <{a}> <keyword=frostsuba.negativestatus> to allies".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
			{
				data.canBeBoosted = true;
				data.effectToApply = TryGet<StatusEffectData>("Apply Random Negative Status");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
			})
		.AddToAsset(this);
		
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectImmuneToXExt>("ImmuneToFrenzy")
		.WithText("Useless Goddess<hiddenkeyword=frostsuba.uselessgoddess>".Process())
		.WithOrder(0)
		.SubscribeToAfterAllBuildEvent<StatusEffectImmuneToXExt>(data =>
			{
				data.descColorHex = new Color(0.62f, 0.83f, 0.90f).ToHexRGB();
				data.immunityType = new[] { TryGet<StatusEffectData>("MultiHit").type };
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectGodBlow>("God Blow")
		.WithText("Trigger <card=frostsuba.aqua> before attacking increase <keyword=attack> equal to amount of statuses of allies".Process())
		.AddToAsset(this);
	}
	protected override void CreateKeyword()
	{
		string[] statuses = new[] { "Block", "Demonize", "MultiHit", "Frost", "Haze", "Null", "Overload", "Shell", "Shroom", "Snow", "Spice", "Teeth", "Weakness" };
		StringBuilder positive = new StringBuilder();
		StringBuilder negative = new StringBuilder();

		foreach (var item in statuses)
		{
			if (TryGet<StatusEffectData>(item).IsNegativeStatusEffect())
			{
				negative.Append($", <sprite name={TryGet<StatusEffectData>(item).type}>");
			}
			else
			{
				positive.Append($", <sprite name={TryGet<StatusEffectData>(item).type}>");
			}
		}
		new KeywordDataBuilder(mod)
			.Create("positivestatus")
			.WithTitle("Positive Status")
			.WithShowName(true)
			.WithDescription($"Including {positive}")
			.WithTitleColour(new Color(0.62f, 0.83f, 0.90f))
			.WithBodyColour(new Color(1f, 1f, 1f))
			.WithCanStack(false)
			.AddToAsset(this);

		new KeywordDataBuilder(mod)
			.Create("negativestatus")
			.WithTitle("Negative Status")
			.WithShowName(true)
			.WithDescription($"Including {negative}")
			.WithTitleColour(new Color(0.62f, 0.83f, 0.90f))
			.WithBodyColour(new Color(1f, 1f, 1f))
			.WithCanStack(false)
			.AddToAsset(this);

		new KeywordDataBuilder(mod)
			.Create("uselessgoddess")
			.WithTitle("Useless Goddess")
			.WithShowName(true)
			.WithDescription("Immune to <keyword=frenzy>")
			.WithTitleColour(new Color(0.62f, 0.83f, 0.90f))
			.WithBodyColour(new Color(1f, 1f, 1f))
			.WithCanStack(false)
			.AddToAsset(this);
	}
}