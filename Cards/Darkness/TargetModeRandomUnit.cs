using System.Collections.Generic;
using System.Linq;

public class TargetModeRandomUnit : TargetMode
{
	public TargetConstraint[] constraints;
	public override bool Random => true;
	public override bool NeedsTarget => false;

	public override Entity[] GetPotentialTargets(Entity entity, Entity target, CardContainer targetContainer)
	{
		HashSet<Entity> hashSet = new HashSet<Entity>();
		hashSet.AddRange(from e in Battle.GetAllUnits()
						 where (bool)e && e.enabled && e.alive && e.canBeHit && CheckConstraints(e)
						 select e);
		if (hashSet.Count <= 0)
		{
			return null;
		}

		return hashSet.ToArray();
	}

	public override Entity[] GetTargets(Entity entity, Entity target, CardContainer targetContainer)
	{
		Entity[] potentialTargets = GetPotentialTargets(entity, target, targetContainer);
		if (potentialTargets == null)
		{
			return null;
		}
		HashSet<Entity> hashSet = new HashSet<Entity>();
		hashSet.Add(potentialTargets.RandomItem());

		if (hashSet.Count <= 0)
		{
			return null;
		}

		return hashSet.ToArray();
	}

	public override Entity[] GetSubsequentTargets(Entity entity, Entity target, CardContainer targetContainer)
	{
		return GetTargets(entity, target, targetContainer);
	}
	public override CardSlot[] GetTargetSlots(CardSlotLane row)
	{
		return new CardSlot[1] { row.slots.RandomItem() };
	}

	public bool CheckConstraints(Entity target)
	{
		TargetConstraint[] array = constraints;
		if (array != null && array.Length > 0)
		{
			return constraints.All((TargetConstraint c) => c.Check(target));
		}

		return true;
	}
}