using System.Collections;
using Konosuba;
using UnityEngine;

public class StatusEffectSteal : StatusEffectInstant
{
	public StatusEffectData effect;
	public StatusEffectInstantIncreaseAttack increase;
	public override IEnumerator Process()
	{
		var units = Battle.GetAllUnits();
		Entity kazuma;
		foreach (var unit in units)
		{
			if (unit.data.name == Frostsuba.instance.TryGet<CardData>("kazuma").name)
			{
				kazuma = unit;
				var stealAmount = target.damage.max >= target.damage.current ? target.damage.max : target.damage.current;
				ChangePhaseAnimationSystem animationSystem =
				Object.FindObjectOfType<ChangePhaseAnimationSystem>();
				if ((bool)animationSystem)
				{
					animationSystem.slowmo = 0.8f;
					yield return animationSystem.Focus(target);
					VFXHelper.SFX.TryPlaySound("Steal");
					yield return Sequences.Wait(0.8f);

					increase.scriptableAmount = new Scriptable<ScriptableFixedAmount>(r => r.amount = -stealAmount);
					yield return StatusEffectSystem.Apply(target,kazuma,increase, 1);

					yield return Sequences.Wait(0.2f);
					yield return animationSystem.UnFocus();
					animationSystem.slowmo = 0.1f;
				}
				increase.scriptableAmount = new Scriptable<ScriptableFixedAmount>(r => r.amount = stealAmount);
				yield return StatusEffectSystem.Apply(kazuma,kazuma,increase, 1);

				foreach (var item in kazuma.GetAllAllies())
				{
					yield return StatusEffectSystem.Apply(
					item,
					kazuma,
					effect,
					GetAmount()
					);
				}

				break;
			}
		}

		yield return base.Process();
	}
}