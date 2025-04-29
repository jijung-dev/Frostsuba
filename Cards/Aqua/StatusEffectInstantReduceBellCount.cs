using System.Collections;
using UnityEngine;

public class StatusEffectInstantReduceBellCounter : StatusEffectInstant
{
	public bool toZero;
	public override IEnumerator Process()
	{
		RedrawBellSystem bellSystem = Object.FindObjectOfType<RedrawBellSystem>();

		bellSystem.SetCounter(toZero ? 0 : bellSystem.counter.current - GetAmount());
		return base.Process();
	}
}