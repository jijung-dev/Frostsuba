using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBakuhatsu : Trigger
{
    public TriggerBakuhatsu(Entity entity, Entity triggeredBy, string type, Entity[] targets)
        : base(entity, triggeredBy, type, targets) { }

    public override IEnumerator PreProcess()
    {
        List<Entity> list = new List<Entity>();
        list.AddRange(entity.GetAllEnemies());

        targets = list.ToArray();
        yield return StatusEffectSystem.PreCardPlayedEvent(entity, targets);
        TriggerBakuhatsu triggerExplode = this;
        Entity[] array2 = targets;
        triggerExplode.hits = new Hit[(array2 != null) ? array2.Length : 0];
        if (targets != null)
        {
            for (int j = 0; j < targets.Length; j++)
            {
                Hit hit = new Hit(entity, targets[j]);
                hit.AddAttackerStatuses();
                hit.trigger = this;
                hits[j] = hit;
            }
        }

        Hit[] array3 = hits;
        foreach (Hit hit2 in array3)
        {
            if ((bool)hit2.target)
            {
                yield return StatusEffectSystem.PreAttackEvent(hit2);
            }
        }
    }

    public override IEnumerator Animate()
    {
        var bombardAnimation =
            AssetLoader.Lookup<CardAnimation>("CardAnimations", "BombardRocketShoot")
            as CardAnimationBombardRocketShoot;
        CardAnimationExplodeShoot animation = new Scriptable<CardAnimationExplodeShoot>();
        int isEnemy = entity.owner == Battle.instance.player ? 1 : -1;

        animation.shootAngle = new Vector3(0f, 0f, isEnemy * -135f);
        animation.shootFxOffset = new Vector3(isEnemy * 1.2f, 1.25f, 0f);
        animation.recoilOffset = new Vector3(isEnemy * -1f, -1f, 0f);
        animation.shootFxPrefab = bombardAnimation.shootFxPrefab;
        animation.recoilCurve = bombardAnimation.recoilCurve;

        ChangePhaseAnimationSystem animationSystem =
            Object.FindObjectOfType<ChangePhaseAnimationSystem>();
        if ((bool)animationSystem)
        {
            animationSystem.slowmo = 1f;
            yield return animationSystem.Focus(entity);
            VFXHelper.SFX.TryPlaySound("Explode");
            yield return Sequences.Wait(1.8f);
            yield return new Routine(animation.Routine(entity));
            yield return Sequences.Wait(0.2f);
            yield return animationSystem.UnFocus();
            animationSystem.slowmo = 0.1f;
        }
    }

    public override IEnumerator ProcessHits()
    {
        yield return EXPLODE();
    }

    public IEnumerator EXPLODE()
    {
        if (hits.Length <= 0)
            yield break;
        Routine.Clump clump = new Routine.Clump();
        int isEnemy = entity.owner == Battle.instance.player ? 1 : -1;

        CardAnimationBombardRocket rocketAnimation =
            AssetLoader.Lookup<CardAnimation>("CardAnimations", "BombardRocket")
            as CardAnimationBombardRocket;
        CardAnimationBombardRocket animation = new Scriptable<CardAnimationBombardRocket>();
        animation.rocketDuration = 0.3f;
        animation.rocketPrefab = rocketAnimation.rocketPrefab;
        animation.rocketPrefab.transform.localScale *= 7;
        animation.rocketMoveCurve = rocketAnimation.rocketMoveCurve;
        clump.Add(animation.Routine(new Vector3(isEnemy * 3.5f, 1f, 0f)));
        yield return Sequences.Wait(0.3f);

        for (int i = 0; i < hits.Length; i++)
        {
            Hit hit = hits[i];
            CardContainer slot = hit.target.actualContainers[0];
            clump.Add(Fire(hit, slot));
        }
        // CameraShakerSystem animationSystem = Object.FindObjectOfType<CameraShakerSystem>();
        // if ((bool)animationSystem)
        // {
        // 	animationSystem.Shake(2f);
        // }
        animation.rocketPrefab.transform.localScale /= 7;
        yield return clump.WaitForEnd();
    }

    public static IEnumerator Fire(Hit hit, CardContainer slot)
    {
        if ((bool)hit.target)
        {
            yield return Trigger.ProcessHit(hit);
        }
    }
}
