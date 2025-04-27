using System.Collections;
using UnityEngine;

public class StatusEffectInstantFocus : StatusEffectInstant
{
	public string audioKey;
	public float unfocusDelay;
	public override IEnumerator Process()
	{
		ChangePhaseAnimationSystem animationSystem =
			Object.FindObjectOfType<ChangePhaseAnimationSystem>();
		if ((bool)animationSystem)
		{
			animationSystem.slowmo = 0.8f;
			yield return animationSystem.Focus(target);
			VFXHelper.SFX.TryPlaySound(audioKey);
			yield return Sequences.Wait(unfocusDelay);
			yield return animationSystem.UnFocus();
			animationSystem.slowmo = 0.1f;
		}
		yield return base.Process();
	}
}