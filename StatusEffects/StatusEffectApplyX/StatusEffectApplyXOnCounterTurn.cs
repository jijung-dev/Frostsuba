using System.Collections;
using UnityEngine;

namespace DSTMod_WildFrost
{
    public class StatusEffectApplyXOnCounterTurn : StatusEffectApplyXOnTurn
    {
        public bool canApplyMultiple;
        public bool isApplied;

        public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
        {
            if (entity.counter.current <= 0 && entity == target)
            {
                if (!canApplyMultiple)
                {
                    if (!isApplied)
                    {
                        isApplied = true;
                        return base.RunCardPlayedEvent(entity, targets);
                    }
                }
                else
                {
                    return base.RunCardPlayedEvent(entity, targets);
                }
            }

            return false;
        }

        public override bool RunTurnEndEvent(Entity entity)
        {
            isApplied = false;
            return base.RunTurnEndEvent(entity);
        }

        public override bool RunTurnEvent(Entity entity)
        {
            if (entity.counter.current <= 0 && entity == target)
            {
                return base.RunTurnEvent(entity);
            }
            return false;
        }
    }
}
