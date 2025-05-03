using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.Utils;

public class Wiz : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("wiz", "Wiz")
			.SetSprites("Wiz.png", "Item_BG.png")
			.SetStats(6, 0, 4)
			.WithCardType("Friendly")
			.AddPool("GeneralUnitPool")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.traits = new List<CardData.TraitStacks>() { TStack("Barrage", 1) };
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("Resist To All Negative Status", 2),
				};
				data.createScripts = new CardScript[] { LeaderExt.GiveCharacterEffect("Wiz") };
			})
			.AddToAsset(this);
	}

	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
			.Create<StatusEffectResistXExt>("Resist To All Negative Status")
			.WithText(
				"Resist <{a}> <keyword=frostsuba.negativestatus>".Process()
			)
			.AddToAsset(this);
	}
}
