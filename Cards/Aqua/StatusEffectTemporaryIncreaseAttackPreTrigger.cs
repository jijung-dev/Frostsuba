using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StatusEffectTemporaryIncreaseAttackPreTrigger : StatusEffectApplyX
{
	public bool mustHaveTarget;

	public bool oncePerTurn = true;

	public bool running;

	public bool hasRunThisTurn;

	public List<Entity> runAgainst;
	int amountIncrease = 0;
	Entity reduceTarget;
	public override void Init()
	{
		base.PreTrigger += PTrigger;
		base.PostAttack += Attack;
	}

	private IEnumerator PTrigger(Trigger trigger)
	{
		if (oncePerTurn)
		{
			hasRunThisTurn = true;
		}

		running = true;
		yield return Run(runAgainst);
		if (effectToApply is StatusEffectInstantIncreaseAttack increaseEffect)
		{
			increaseEffect.scriptableAmount = new Scriptable<ScriptableFixedAmount>(r => r.amount = -amountIncrease);
			yield return Run(new List<Entity>() { reduceTarget });
		}
		runAgainst = null;
		running = false;
	}

	public override bool RunPreTriggerEvent(Trigger trigger)
	{
		if (trigger.entity != target || running || trigger.targets == null)
			return false;

		if (effectToApply is StatusEffectInstantIncreaseAttack increaseEffect)
		{
			TargetConstraint constraint = new Scriptable<TargetConstraintDoesDamage>();
			if (target != null && constraint != null)
			{
				var allies = target.GetAllAllies();
				if (allies != null)
				{
					var candidates = allies
						.Where(r => r != null && constraint.Check(r))
						.ToList();

					if (candidates.Count > 0)
					{
						reduceTarget = candidates.RandomItem();
					}
				}
			}
			if (reduceTarget == null) return false;
			amountIncrease = reduceTarget.damage.current >= reduceTarget.damage.max ? reduceTarget.damage.current : reduceTarget.damage.max;
			increaseEffect.scriptableAmount = new Scriptable<ScriptableFixedAmount>(r => r.amount = amountIncrease);
		}

		return CheckTrigger(trigger);
	}

	public bool CheckTrigger(Trigger trigger)
	{
		if (hasRunThisTurn || running || !target.enabled || trigger.entity != target)
		{
			return false;
		}

		runAgainst = GetTargets();
		if (mustHaveTarget && (runAgainst == null || runAgainst.Count <= 0))
		{
			trigger.nullified = true;
			return false;
		}

		return true;
	}

	public override bool RunTurnEndEvent(Entity entity)
	{
		if (hasRunThisTurn && entity == target)
		{
			hasRunThisTurn = false;
		}

		return false;
	}
	public override bool RunPostAttackEvent(Hit hit)
	{
		if (hit.attacker != null && hit.attacker == target)
		{
			return true;
		}
		return false;
	}
	private IEnumerator Attack(Hit hit)
	{
		if (effectToApply is StatusEffectInstantIncreaseAttack effect)
		{
			effect.scriptableAmount = new Scriptable<ScriptableFixedAmount>(r => r.amount = -amountIncrease);
			yield return Run(GetTargets());

			if (reduceTarget == null) yield break;

			effect.scriptableAmount = new Scriptable<ScriptableFixedAmount>(r => r.amount = amountIncrease);
			yield return Run(new List<Entity>() { reduceTarget });
			reduceTarget = null;
			amountIncrease = 0;
		}
	}
}