public class StatusEffectTriggerWhenCertainAllyAttacks : StatusEffectTriggerWhenAllyAttacks
{
    public CardData ally;

    public override bool RunHitEvent(Hit hit)
    {
        if (hit.attacker?.name == ally.name)
        {
            return base.RunHitEvent(hit);
        }

        return false;
    }
}

