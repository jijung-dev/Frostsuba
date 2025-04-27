using System.Collections;
using System.Linq;
using Konosuba;
using UnityEngine;

public class StatusEffectGodBlow : StatusEffectData
{
	public override bool Instant => true;
	public override void Init()
	{
		base.OnBegin += Process;
		base.PostAttack += Attack;
		base.OnTurnEnd += TurnEnd;
	}

	private IEnumerator Process()
	{
		var amountIncrease = target.GetAllAllies()
		.SelectMany(ally => ally.statusEffects)
		.Where(effect => effect.isStatus)
		.Count();
		target.damage.max = amountIncrease;
		target.damage.current = amountIncrease;
		ChangePhaseAnimationSystem animationSystem =
			Object.FindObjectOfType<ChangePhaseAnimationSystem>();
		if ((bool)animationSystem)
		{
			animationSystem.slowmo = 0.8f;
			yield return animationSystem.Focus(target);
			yield return target.display.UpdateDisplay(true);
			yield return Sequences.Wait(0.2f);
			VFXHelper.SFX.TryPlaySound("GodBlow");
			yield return animationSystem.UnFocus();
			animationSystem.slowmo = 0.1f;
			yield return StatusEffectSystem.Apply(
			target,
			target,
			Frostsuba.instance.TryGet<StatusEffectData>("Trigger"),
			1
			);
		}
	}
	public override bool RunTurnEndEvent(Entity entity)
	{
		if (entity == target && entity.damage.max > 0)
		{
			return true;
		}
		return false;
	}
	public override bool RunPostAttackEvent(Hit hit)
	{
		if (hit.attacker != null && hit.attacker == target)
		{
			return true;
		}
		return false;
	}
	private IEnumerator TurnEnd(Entity entity)
	{
		target.display.promptUpdateDescription = true;
		target.PromptUpdate();
		target.damage.max = 0;
		target.damage.current = 0;
		target.display.damageIcon?.Destroy();
		yield return target.display.UpdateDisplay(true);
		yield return Remove();
	}
	private IEnumerator Attack(Hit hit)
	{
		target.display.promptUpdateDescription = true;
		target.PromptUpdate();
		target.damage.max = 0;
		target.damage.current = 0;
		target.display.damageIcon?.Destroy();
		yield return target.display.UpdateDisplay(true);
		yield return Remove();
	}
}