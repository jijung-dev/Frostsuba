using System.Collections;

public class StatusEffectInstantDamage : StatusEffectInstant
{
	public override IEnumerator Process()
	{
		Routine.Clump clump = new Routine.Clump();
		Hit hit = new Hit(target, target, count);
		clump.Add(hit.Process());
		yield return clump.WaitForEnd();
		yield return base.Process();
	}
}