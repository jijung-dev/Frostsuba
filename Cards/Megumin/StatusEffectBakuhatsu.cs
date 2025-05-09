using System;
using System.Collections;
using Konosuba;
using UnityEngine;

public class StatusEffectBakuhatsu : StatusEffectData
{
    public bool triggered;
    public bool isNoDown;

    public override void Init()
    {
        Events.OnEntityTrigger += EntityTrigger;
        base.PostAttack += Attack;
    }

    public void OnDestroy()
    {
        Events.OnEntityTrigger -= EntityTrigger;
    }

    public void EntityTrigger(ref Trigger trigger)
    {
        if (trigger.entity == target && CanTrigger() && trigger.type == "basic")
        {
            trigger = new TriggerBakuhatsu(
                trigger.entity,
                trigger.triggeredBy,
                "bakuhatsu",
                trigger.targets
            );
            triggered = true;
        }
    }

    public override bool RunPostAttackEvent(Hit hit)
    {
        if (hit.attacker != null && hit.attacker == target && triggered)
        {
            return true;
        }
        return false;
    }

    public IEnumerator Attack(Hit hit)
    {
        triggered = false;
        if (!isNoDown)
            yield return StatusEffectSystem.Apply(
                target,
                target,
                Frostsuba.instance.TryGet<StatusEffectData>("Megumin Down"),
                1
            );
    }
}
