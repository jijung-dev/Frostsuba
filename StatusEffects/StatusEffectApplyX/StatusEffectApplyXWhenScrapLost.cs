using System.Collections;
using UnityEngine;

public class StatusEffectApplyXWhenScrapLost : StatusEffectApplyX
{
	public bool hasThreshold;

	public bool active;

	public int currentScrap;
	public int maxScrap;

	public override void Init()
	{
		Events.OnEntityDisplayUpdated += EntityDisplayUpdated;
	}

	public void OnDestroy()
	{
		Events.OnEntityDisplayUpdated -= EntityDisplayUpdated;
	}

	public override bool RunBeginEvent()
	{
		active = true;
		maxScrap = target.FindStatus("scrap").count;
		currentScrap = target.FindStatus("scrap").count;
		return false;
	}

	public void EntityDisplayUpdated(Entity entity)
	{
		if (active && target.FindStatus("scrap").count != currentScrap && entity == target)
		{
			int num = target.FindStatus("scrap").count - currentScrap;
			currentScrap = target.FindStatus("scrap").count;
			if (num < 0 && target.enabled && !target.silenced && CheckThreshold() && (!targetMustBeAlive || (target.alive && Battle.IsOnBoard(target))))
			{
				ActionQueue.Stack(new ActionSequence(ScrapLost(-num))
				{
					note = base.name,
					priority = eventPriority
				}, fixedPosition: true);
			}
		}
	}

	public bool CheckThreshold()
	{
		if (hasThreshold)
		{
			return target.FindStatus("scrap").count <= maxScrap - GetAmount();
		}

		return true;
	}

	public IEnumerator ScrapLost(int amount)
	{
		if ((bool)this && target.IsAliveAndExists())
		{
			ChangePhaseAnimationSystem animationSystem =
			Object.FindObjectOfType<ChangePhaseAnimationSystem>();
			if ((bool)animationSystem)
			{
				animationSystem.slowmo = 0.8f;
				yield return animationSystem.Focus(target);
				VFXHelper.SFX.TryPlaySound("Darkness");
				yield return Sequences.Wait(0.6f);
				yield return Run(GetTargets(), amount);
				yield return Sequences.Wait(0.2f);
				yield return animationSystem.UnFocus();
				animationSystem.slowmo = 0.1f;
			}
			target.PromptUpdate();
			target.display.promptUpdateDescription = true;
		}
	}
}