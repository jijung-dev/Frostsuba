using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChangePhaseExt : PlayAction
{
    public readonly Entity entity;

    public readonly CardData newPhase;

    public readonly CardAnimation animation;

    public List<Entity> newCards;

    public bool loadingNewCards;

    public ActionChangePhaseExt(Entity entity, CardData newPhase, CardAnimation animation)
    {
        this.entity = entity;
        this.newPhase = newPhase;
        this.animation = animation;
    }

    public override IEnumerator Run()
    {
        if (!entity.IsAliveAndExists())
        {
            yield break;
        }

        Events.InvokeEntityChangePhase(entity);

        PauseMenu.Block();
        DeckpackBlocker.Block();
        if (
            Deckpack.IsOpen && References.Player.entity.display is CharacterDisplay characterDisplay
        )
        {
            characterDisplay.CloseInventory();
        }

        if ((bool)animation)
        {
            yield return animation.Routine(entity);
        }

        PlayAction[] actions = ActionQueue.GetActions();
        foreach (PlayAction playAction in actions)
        {
            if (!(playAction is ActionTrigger actionTrigger))
            {
                if (playAction is ActionEffectApply actionEffectApply)
                {
                    actionEffectApply.TryRemoveEntity(entity);
                }
            }
            else if (actionTrigger.entity == entity)
            {
                ActionQueue.Remove(playAction);
            }
        }

        ActionQueue.Stack(
            new ActionSequence(Change(entity, newPhase))
            {
                note = "Change boss phase",
                priority = 10,
            },
            fixedPosition: true
        );
    }

    public static IEnumerator Change(Entity entity, CardData newData)
    {
        entity.alive = false;
        yield return entity.ClearStatuses();
        entity.data = newData;
        yield return entity.display.UpdateData(doPing: true);
        entity.alive = true;
        yield return StatusEffectSystem.EntityEnableEvent(entity);

        if (entity.hp.max <= 0)
            entity.display.healthIcon?.Destroy();
        if (entity.damage.max <= 0)
            entity.display.damageIcon?.Destroy();
        if (entity.counter.max <= 0)
            entity.display.counterIcon?.Destroy();

        PauseMenu.Unblock();
        DeckpackBlocker.Unblock();
    }

    public static IEnumerator EnableBehaviour(Behaviour system)
    {
        system.enabled = true;
        yield return null;
    }
}
