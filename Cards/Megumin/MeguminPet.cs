using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class MeguminPet : DataBase
{
	public override void CreateCard()
	{
		new CardDataBuilder(mod)
			.CreateUnit("meguminPet", "Megumin")
			.IsPet("", true)
			.SetSprites("Megumin_Pet.png", "Item_BG.png")
			.SetStats(3, 4, 5)
			.WithCardType("Friendly")
			.SubscribeToAfterAllBuildEvent<CardData>(data =>
			{
				data.startWithEffects = new CardData.StatusEffectStacks[]
				{
					SStack("On Turn Apply Megumin Pet Down", 1),
					SStack("Hit All Enemies", 1),
				};
			})
			.AddToAsset(this);

		new CardDataBuilder(mod)
            .CreateUnit("meguminPetDown", "Megumin Down")
            .SetSprites("Megumin_Down.png", "Item_BG.png")
            .SetStats(1, null, 7)
            .WithCardType("Friendly")
            .SubscribeToAfterAllBuildEvent<CardData>(data =>
            {
                data.traits = new List<CardData.TraitStacks>() { TStack("Fragile", 1) };
                data.startWithEffects = new CardData.StatusEffectStacks[]
                {
                    SStack("On Counter Turn Apply Megumin Pet Up To Self", 1),
                    SStack("Block", 1),
                };
            })
            .AddToAsset(this);
	}
	protected override void CreateStatusEffect()
	{
		new StatusEffectDataBuilder(mod)
			.Create<StatusEffectApplyXOnTurn>("On Turn Apply Megumin Pet Down")
			.WithText(
                "Explosion Maniac<hiddencard=frostsuba.meguminPetDown><hiddenkeyword=frostsuba.explosionmaniac>".Process()
            )
			.WithOrder(0)
			.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
			{
                data.descColorHex = new Color(0.94f, 0.58f, 0.24f).ToHexRGB();
				data.effectToApply = TryGet<StatusEffectData>("Megumin Pet Down");
				data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
			})
			.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
			.Create<StatusEffectNextPhaseExt>("Megumin Pet Down")
			.SubscribeToAfterAllBuildEvent<StatusEffectNextPhaseExt>(data =>
			{
				data.preventDeath = true;
				data.nextPhase = TryGet<CardData>("meguminPetDown");
				data.killSelfWhenApplied = true;
			})
			.AddToAsset(this);

		new StatusEffectDataBuilder(mod)
			.Create<StatusEffectNextPhaseExt>("Megumin Pet Up")
			.SubscribeToAfterAllBuildEvent<StatusEffectNextPhaseExt>(data =>
			{
				data.preventDeath = true;
				data.nextPhase = TryGet<CardData>("meguminPet");
				data.killSelfWhenApplied = true;
			})
			.AddToAsset(this);
			
		new StatusEffectDataBuilder(mod)
            .Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Apply Megumin Pet Up To Self")
            .WithText("When <keyword=counter> reaches 0 becomes <card=frostsuba.meguminPet>".Process())
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(data =>
            {
                data.effectToApply = TryGet<StatusEffectData>("Megumin Pet Up");
                data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
            })
            .AddToAsset(this);
	}
}