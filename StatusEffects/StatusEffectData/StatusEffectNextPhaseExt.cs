using System;
using System.Linq;
using Konosuba;
using UnityEngine;
using static HitFlashSystem;

public class StatusEffectNextPhaseExt : StatusEffectData
{
    public CardData nextPhase;

    public CardAnimation animation;

    public bool goToNextPhase;

    public bool activated;
    public StatusEffectData byCertainEffect;
    public bool killSelfWhenApplied;

    public override void Init()
    {
        Events.OnEntityDisplayUpdated += EntityDisplayUpdated;
    }

    public void OnDestroy()
    {
        Events.OnEntityDisplayUpdated -= EntityDisplayUpdated;
    }

    public override bool RunBeginEvent()
    {
        if (killSelfWhenApplied)
        {
            var statusKeys = new[] { "Scrap" };

            foreach (var key in statusKeys)
            {
                var effect = target.FindStatus(Frostsuba.instance.TryGet<StatusEffectData>(key));
                if (effect != null)
                {
                    effect.count = 0;
                    return base.RunBeginEvent();
                }
            }

            target.hp.current = 0;
        }

        return base.RunBeginEvent();
    }

    public void EntityDisplayUpdated(Entity entity)
    {
        if (!activated && target.hp.current <= 0 && entity == target)
        {
            TryActivate();
        }
    }

    public override bool RunPostHitEvent(Hit hit)
    {
        if (!activated && hit.target == target && target.hp.current <= 0)
        {
            if (byCertainEffect != null && hit.statusEffects.Any(r => r.data == byCertainEffect))
            {
                preventDeath = true;
                return false;
            }
            TryActivate();
        }

        return false;
    }

    public void TryActivate()
    {
        bool flag = true;
        foreach (StatusEffectData statusEffect in target.statusEffects)
        {
            if (statusEffect != this && statusEffect.preventDeath)
            {
                flag = false;
                break;
            }
        }

        if (!flag)
        {
            return;
        }

        activated = true;

        if ((bool)nextPhase)
        {
            ActionQueue.Stack(
                new ActionChangePhaseExt(target, nextPhase.Clone(), animation) { priority = 10 },
                fixedPosition: true
            );
            return;
        }

        throw new ArgumentException("Next phase not given!");
    }
}
