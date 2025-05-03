using System;
using System.Collections;

public class StatusEffectApplyXWhenYAppliedToSelfExt : StatusEffectApplyX
{
	public string whenAppliedType = "spice";

	public string[] whenAppliedTypes = new string[1] { "spice" };
	public bool instead;
	public bool isAdded;

	public override void Init()
	{
		base.PostApplyStatus += Check;
	}

	public override bool RunApplyStatusEvent(StatusEffectApply apply)
	{
		if (apply.target == target && instead && target.enabled && !TargetSilenced() && (target.alive || !targetMustBeAlive) && (bool)apply.effectData && apply.count > 0 && whenAppliedTypes.Contains(apply.effectData.type))
		{
			if (instead)
			{
				isAdded = true;
				apply.effectData = effectToApply;
			}
		}

		return false;
	}

	public override bool RunPostApplyStatusEvent(StatusEffectApply apply)
	{
		if (target.enabled && apply.target == target && (bool)apply.effectData && apply.count > 0 && !isAdded)
		{
			isAdded = false;
			return whenAppliedTypes.Contains(apply.effectData.type);
		}

		return false;
	}

	public IEnumerator Check(StatusEffectApply apply)
	{
		return Run(GetTargets(), apply.count);
	}
}