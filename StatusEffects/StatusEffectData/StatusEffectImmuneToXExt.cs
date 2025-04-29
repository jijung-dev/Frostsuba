using System.Collections;
using Konosuba;
using UnityEngine;

public class StatusEffectImmuneToXExt : StatusEffectData
{
	public string[] immunityType = new[] { "snow" };
	public bool isNegative;
	public string[] allType = new[] { "Block", "Demonize", "MultiHit", "Frost", "Haze", "Null", "Overload", "Shell", "Shroom", "Snow", "Spice", "Teeth", "Weakness" };

	public override void Init()
	{
		base.OnBegin += Begin;
	}

	public IEnumerator Begin()
	{
		if (isNegative)
		{
			foreach (var item in allType)
			{
				if (Frostsuba.instance.TryGet<StatusEffectData>(item).IsNegativeStatusEffect())
				{
					StatusEffectData statusEffectData = target.FindStatus(Frostsuba.instance.TryGet<StatusEffectData>(item));
					if ((bool)statusEffectData && statusEffectData.count > 0)
					{
						yield return statusEffectData.RemoveStacks(statusEffectData.count, removeTemporary: false);
					}
				}
			}
			yield break;
		}
		foreach (var types in immunityType)
		{
			StatusEffectData statusEffectData = target.FindStatus(types);
			if ((bool)statusEffectData && statusEffectData.count > 0)
			{
				yield return statusEffectData.RemoveStacks(statusEffectData.count, removeTemporary: false);
			}
		}
	}

	public override bool RunApplyStatusEvent(StatusEffectApply apply)
	{
		if(isNegative && apply.effectData.IsNegativeStatusEffect())
		{
			apply.effectData = null;
			apply.count = 0;
			return false;
		}
		if (apply.target == target && (bool)apply.effectData && immunityType.Contains(apply.effectData.type))
		{
			apply.effectData = null;
			apply.count = 0;
		}

		return false;
	}
}