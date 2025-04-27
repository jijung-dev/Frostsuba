using System.Collections;
using UnityEngine;

public class StatusEffectImmuneToXExt : StatusEffectData
{
	public string immunityType = "snow";

	public override void Init()
	{
		base.OnBegin += Begin;
	}

	public IEnumerator Begin()
	{
		StatusEffectData statusEffectData = target.FindStatus(immunityType);
		if ((bool)statusEffectData && statusEffectData.count > 0)
		{
			yield return statusEffectData.RemoveStacks(statusEffectData.count, removeTemporary: false);
		}
	}

	public override bool RunApplyStatusEvent(StatusEffectApply apply)
	{
		if (apply.target == target && (bool)apply.effectData && apply.effectData.type == immunityType)
		{
			apply.count = 0;
		}

		return false;
	}
}