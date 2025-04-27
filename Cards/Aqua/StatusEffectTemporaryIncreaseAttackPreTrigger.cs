using System.Collections;
using System.Linq;

public class StatusEffectTemporaryIncreaseAttackPreTrigger : StatusEffectApplyXPreTrigger
{
	int amountIncrease = 0;
	public override void Init()
	{
		base.Init();
		base.PostAttack += Attack;
	}

	public override bool RunPreTriggerEvent(Trigger trigger)
	{
		if (trigger.entity != target || running)
			return false;

		if (effectToApply is StatusEffectInstantIncreaseAttack increaseEffect)
		{
			amountIncrease = target.GetAllAllies()
			.SelectMany(ally => ally.statusEffects)
			.Where(effect => effect.IsNegativeStatusEffect())
			.Count();
			increaseEffect.scriptableAmount = new Scriptable<ScriptableFixedAmount>(r => r.amount = amountIncrease);
		}

		return base.RunPreTriggerEvent(trigger);
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
		}
		yield return Run(GetTargets());
	}
}