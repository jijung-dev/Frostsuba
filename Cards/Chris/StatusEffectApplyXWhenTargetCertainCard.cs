using System;
using System.Collections;
using System.Linq;

public class StatusEffectApplyXWhenTargetCertainCard : StatusEffectApplyX
{
	public TargetConstraint[] constraints;
	public override void Init()
	{
		base.PreAttack += EntityPreAttack;
	}

	private IEnumerator EntityPreAttack(Hit hit)
	{
		yield return Run(GetTargets());
	}

	public override bool RunPreAttackEvent(Hit hit)
	{
		if (constraints.Any(r => r.Check(hit.target)) && hit.attacker != null && hit.attacker == target)
		{
			return true;
		}
		return false;
	}

}