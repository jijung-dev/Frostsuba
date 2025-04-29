using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class HolyAura : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateItem("holyAura", "Holy Aura")
			.SetSprites("HolyAura.png", "Item_BG.png")
			.SetStats(null, 1, 0)
			.WithCardType("Item")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("Hit All Enemies", 1),
					SStack("On Card Played Apply Shroom To Aqua", 2),
				};
			})
			.AddToAsset(this);
	}
	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnCardPlayed>("On Card Played Apply Shroom To Aqua")
		.WithText("On card played apply <{a}><keyword=shroom> to <card=frostsuba.aqua>".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(data =>
			{
				data.effectToApply = TryGet<StatusEffectData>("Shroom");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
				data.applyConstraints = new TargetConstraint[]
				{
					new Scriptable<TargetConstraintIsSpecificCard>(r => r.allowedCards = new CardData[]
					{
						TryGet<CardData>("aqua")
					}),
				};
			})
		.AddToAsset(this);
	}
}