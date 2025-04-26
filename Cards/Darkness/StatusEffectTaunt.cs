using System.Collections;
using UnityEngine;

public class StatusEffectTaunt : StatusEffectData
{
	public CardData.StatusEffectStacks blockEffect;
	public override void Init()
	{
		Events.OnEntityTrigger += EntityTrigger;
		base.OnBegin += Begin;
	}

	private IEnumerator Begin()
	{
		ChangePhaseAnimationSystem animationSystem =
			Object.FindObjectOfType<ChangePhaseAnimationSystem>();
		if ((bool)animationSystem)
		{
			animationSystem.slowmo = 0.8f;
			yield return animationSystem.Focus(target);
			VFXHelper.SFX.TryPlaySound("Darkness");
			yield return Sequences.Wait(0.6f);
			yield return StatusEffectSystem.Apply(
			target,
			target,
			blockEffect.data,
			blockEffect.count
			);
			yield return Sequences.Wait(0.2f);
			yield return animationSystem.UnFocus();
			animationSystem.slowmo = 0.1f;
		}
		target.PromptUpdate();
		target.display.promptUpdateDescription = true;
	}

	public void OnDestroy()
	{
		Events.OnEntityTrigger -= EntityTrigger;
	}
	private void EntityTrigger(ref Trigger trigger)
	{
		if (trigger.type == "basic" && trigger.entity.owner != target.owner)
		{
			trigger.targets = new Entity[] { target };
		}
	}
}