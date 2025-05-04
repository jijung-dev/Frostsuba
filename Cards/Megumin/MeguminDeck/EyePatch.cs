using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class EyePatch : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("eyePatch", "Eye Patch")
			.SetSprites("EyePatch.png", "Item_BG.png")
			.SetStats(null, null, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.needsTarget = false;
				data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
				data.startWithEffects = new CardData.StatusEffectStacks[] 
				{ 
					SStack("On Card Played Increase Attack To Random Ally", 2),
					SStack("MultiHit", 1),
				};
			})
			.AddToAsset(this);
	}
	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnCardPlayed>("On Card Played Increase Attack To Random Ally")
		.WithText("Increase <{a}><keyword=attack> to a random ally")
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(data =>
			{
				data.canBeBoosted = true;
				data.effectToApply = TryGet<StatusEffectData>("Increase Attack");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomAlly;
				data.applyConstraints = new TargetConstraint[]
				{
					new Scriptable<TargetConstraintDoesDamage>(),
				};
			})
		.AddToAsset(this);
	}
}