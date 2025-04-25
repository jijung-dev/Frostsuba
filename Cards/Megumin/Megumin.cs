using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.Utils;

public class Megumin : DataBase
{
    public override void CreateCard()
    {
        new CardDataBuilder(mod)
            .CreateUnit("megumin", "Megumin")
            .WithText(
                "<keyword=frostsuba.explosionmaniac>\nHit all enemies<hiddencard=frostsuba.meguminDown>".Process()
            )
            .SetSprites("Megumin.png", "Megumin_BG.png")
            .SetStats(5, 0, 0)
            .WithCardType("Leader")
            .SubscribeToAfterAllBuildEvent<CardData>(data =>
            {
                data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade() };
                data.startWithEffects = new CardData.StatusEffectStacks[]
                {
                    SStack("Bakuhatsu", 1),
                    SStack("On Every Turn Gain Attack", 2),
                };
            })
            .AddToAsset(this);

        new CardDataBuilder(mod)
            .CreateUnit("meguminDown", "Megumin Down")
            .SetSprites("Megumin_Down.png", "Megumin_BG.png")
            .SetStats(1, null, 8)
            .WithCardType("Leader")
            .SubscribeToAfterAllBuildEvent<CardData>(data =>
            {
                data.traits = new List<CardData.TraitStacks>() { TStack("Fragile", 1) };
                data.startWithEffects = new CardData.StatusEffectStacks[]
                {
                    SStack("On Counter Turn Apply Megumin Up To Self", 1),
                };
            })
            .AddToAsset(this);

        new CardDataBuilder(mod)
            .CreateItem("manatiteRod", "Manatite Rod")
            .SetSprites("Megumin_Rod.png", "Megumin_BG.png")
            .WithText("Trigger <card=frostsuba.megumin>".Process())
            .SetStats(null, null, 0)
            .WithCardType("Item")
            .SubscribeToAfterAllBuildEvent<CardData>(data =>
            {
                //data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
                data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Trigger", 1) };
                data.targetConstraints = new TargetConstraint[]
                {
                    new Scriptable<TargetConstraintIsSpecificCard>(r =>
                        r.allowedCards = new CardData[] { TryGet<CardData>("megumin") }
                    ),
                };
                data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade("CrownCursed") };
            })
            .AddToAsset(this);
    }

    protected override void CreateStatusEffect()
    {
        new StatusEffectDataBuilder(mod)
            .Create<StatusEffectBakuhatsu>("Bakuhatsu")
            .WithIsReaction(true)
            .AddToAsset(this);

        new StatusEffectDataBuilder(mod)
            .Create<StatusEffectApplyXEveryTurn>("On Every Turn Gain Attack")
            .WithText("Gain <{a}><keyword=attack> every turn")
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXEveryTurn>(data =>
            {
                data.mode = StatusEffectApplyXEveryTurn.Mode.AfterTurn;
                data.effectToApply = TryGet<StatusEffectData>("Increase Attack");
                data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
            })
            .AddToAsset(this);

        new StatusEffectDataBuilder(mod)
            .Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Apply Megumin Up To Self")
            .WithText("When <keyword=counter> reaches 0 becomes <card=frostsuba.megumin>".Process())
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(data =>
            {
                data.effectToApply = TryGet<StatusEffectData>("Megumin Up");
                data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
            })
            .AddToAsset(this);

        new StatusEffectDataBuilder(mod)
            .Create<StatusEffectNextPhaseExt>("Megumin Down")
            .SubscribeToAfterAllBuildEvent<StatusEffectNextPhaseExt>(data =>
            {
                data.preventDeath = true;
                data.nextPhase = TryGet<CardData>("meguminDown");
                data.killSelfWhenApplied = true;
            })
            .AddToAsset(this);

        new StatusEffectDataBuilder(mod)
            .Create<StatusEffectNextPhaseExt>("Megumin Up")
            .SubscribeToAfterAllBuildEvent<StatusEffectNextPhaseExt>(data =>
            {
                data.preventDeath = true;
                data.nextPhase = TryGet<CardData>("megumin");
                data.killSelfWhenApplied = true;
            })
            .AddToAsset(this);
    }

    protected override void CreateKeyword()
    {
        new KeywordDataBuilder(mod)
            .Create("explosionmaniac")
            .WithTitle("Explosion Maniac")
            .WithShowName(true)
            .WithDescription("After triggering becomes <card=frostsuba.meguminDown>".Process())
            .WithTitleColour(new Color(0.94f, 0.58f, 0.24f))
            .WithBodyColour(new Color(1f, 1f, 1f))
            .WithCanStack(false)
            .AddToAsset(this);
    }
}
