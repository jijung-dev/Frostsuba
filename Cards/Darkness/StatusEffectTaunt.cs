using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectTaunt : StatusEffectData
{
	public override void Init()
	{
		Events.OnEntityTrigger += EntityTrigger;
	}

	public void OnDestroy()
	{
		Events.OnEntityTrigger -= EntityTrigger;
	}
	private void EntityTrigger(ref Trigger trigger)
	{
		if (trigger.type == "basic" && trigger.entity.owner != target.owner && trigger.targets?.Length > 0)
		{
			var allTarget = new Entity[trigger.targets.Length];
			for (int i = 0; i < trigger.targets.Length; i++)
			{
				allTarget[i] = target;
			}
			trigger.targets = allTarget;
		}
	}
}