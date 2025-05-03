using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.Utils;

public class Iris : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("iris", "Iris")
			.SetSprites("Iris.png", "Item_BG.png")
			.SetStats(8, 2, 4)
			.WithCardType("Friendly")
			.AddPool("GeneralUnitPool")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("On Hit Equal Heal To Allies", 1),
				};
			})
			.AddToAsset(this);
	}

	protected override void CreateStatusEffect()
	{
		StatusCopy("On Hit Equal Heal To FrontAlly", "On Hit Equal Heal To Allies")
			.WithText(
				"Restore <keyword=health> to allies equal to damage dealt".Process()
			)
			.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnHit>(data =>
			{
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
			})
			.AddToAsset(this);
	}
}
