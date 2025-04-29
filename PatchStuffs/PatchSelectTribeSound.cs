using HarmonyLib;
using Konosuba;
using static Deadpan.Enums.Engine.Components.Modding.WildfrostMod;

[HarmonyPatch(typeof(SelectTribe), nameof(SelectTribe.SelectRoutine))]
class PatchSelectTribeSound
{
	static void Prefix(ref ClassData classData)
	{
		if (classData.ModAdded == Frostsuba.instance)
			VFXHelper.SFX.TryPlaySound("Konosuba_Select");
	}
}
