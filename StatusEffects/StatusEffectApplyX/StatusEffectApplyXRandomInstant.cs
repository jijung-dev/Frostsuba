using System;
using System.Collections;
using Konosuba;

public class StatusEffectApplyXRandomInstant : StatusEffectApplyX
{
	public override bool Instant => true;
	public bool isNegative;
	public bool isAll = true;
	public string[] statuses = new[] { "Block", "Demonize", "MultiHit", "Frost", "Haze", "Null", "Overload", "Shell", "Shroom", "Snow", "Spice", "Teeth", "Weakness" };
	public override void Init()
	{
		base.OnBegin += Begin;
	}
	public override int GetAmount()
	{
		return 1;
	}
	public override bool TargetSilenced()
	{
		return false;
	}

	private IEnumerator Begin()
	{
		for (int i = 0; i < count; i++)
		{
			var effect = Frostsuba.instance.TryGet<StatusEffectData>(statuses.RandomItem());
			if (isNegative)
			{
				while (!effect.IsNegativeStatusEffect())
				{
					effect = Frostsuba.instance.TryGet<StatusEffectData>(statuses.RandomItem());
				}
			}
			else
			{
				while (effect.IsNegativeStatusEffect())
				{
					effect = Frostsuba.instance.TryGet<StatusEffectData>(statuses.RandomItem());
				}
			}
			effectToApply = effect;
			yield return Run(GetTargets(), 1);
		}
		yield return Remove();
	}
}