public class StatusEffectResistXExt : StatusEffectData
{
	public string resistType = "snow";
	public bool isNegative;

	public override bool RunApplyStatusEvent(StatusEffectApply apply)
	{
		var effectData = apply.effectData;

		if (apply.target == target && (effectData?.type == resistType || (isNegative && effectData != null && effectData.IsNegativeStatusEffect())))
		{
			apply.count -= count;
			if (apply.count <= 0)
			{
				apply.effectData = null;
				apply.count = 0;
			}
		}

		return false;
	}
}