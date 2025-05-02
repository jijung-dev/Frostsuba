using Deadpan.Enums.Engine.Components.Modding;

public class AquaPet : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("aquaPet", "Aqua")
			.IsPet("", true)
			.SetSprites("Aqua_Pet.png", "Item_BG.png")
			.SetStats(3, null, 6)
			.WithCardType("Friendly")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("ImmuneToFrenzy", 1),
					SStack("On Turn Apply Random Positive Status To Random AllyInRow", 2),
					SStack("On Turn Apply Random Negative Status To Random AllyInRow", 1),
				};
			})
			.AddToAsset(this);
	}
	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnTurn>("On Turn Apply Random Positive Status To Random AllyInRow")
		.WithText("Apply <{a}> <keyword=frostsuba.positivestatus>".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
			{
				data.canBeBoosted = true;
				data.effectToApply = TryGet<StatusEffectData>("Apply Random Positive Status");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomAllyInRow;
			})
		.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
		.Create<StatusEffectApplyXOnTurn>("On Turn Apply Random Negative Status To Random AllyInRow")
		.WithText("and <{a}> <keyword=frostsuba.negativestatus> to random ally in row".Process())
		.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
			{
				data.canBeBoosted = true;
				data.effectToApply = TryGet<StatusEffectData>("Apply Random Negative Status");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomAllyInRow;
			})
		.AddToAsset(this);
	}
}