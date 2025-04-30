using System.Collections;
using System.Linq;
using HarmonyLib;
using Konosuba;
using UnityEngine;
using static Deadpan.Enums.Engine.Components.Modding.WildfrostMod;

[HarmonyPatch(typeof(SelectStartingPet), nameof(SelectStartingPet.Routine))]
class PatchSelectStartingPet
{
	static void Prefix(ref Entity leader, SelectStartingPet __instance)
	{
		if(__instance.running) return;
		CheckLeaderPet(leader, __instance, "aqua", "aquaPet");
		CheckLeaderPet(leader, __instance, "kazuma", "kazumaPet");
		CheckLeaderPet(leader, __instance, "megumin", "meguminPet");
		CheckLeaderPet(leader, __instance, "darkness", "darknessPet");
	}
	public static void CheckLeaderPet(Entity leader, SelectStartingPet __instance, string leaderName, string petName)
	{
		if (leader.data.name == Frostsuba.instance.TryGet<CardData>(leaderName).name)
		{
			Entity pet = __instance.pets.First(r => r.data.name == Frostsuba.instance.TryGet<CardData>(petName).name);
			__instance.group.Remove(pet);
			__instance.pets.Remove(pet);
			CardManager.ReturnToPool(pet);
		}
	}
}
[HarmonyPatch(typeof(SelectStartingPet), nameof(SelectStartingPet.Cancel))]
class PatchSelectStartingPet2
{
	static void Prefix(SelectStartingPet __instance)
	{
		if (__instance.leader.data.name == Frostsuba.instance.TryGet<CardData>("aqua").name)
		{
			__instance.StartCoroutine(CreateCard("aquaPet", __instance));
			return;
		}
		if (__instance.leader.data.name == Frostsuba.instance.TryGet<CardData>("megumin").name)
		{
			__instance.StartCoroutine(CreateCard("meguminPet", __instance));
			return;
		}
		if (__instance.leader.data.name == Frostsuba.instance.TryGet<CardData>("kazuma").name)
		{
			__instance.StartCoroutine(CreateCard("kazumaPet", __instance));
			return;
		}
		if (__instance.leader.data.name == Frostsuba.instance.TryGet<CardData>("darkness").name)
		{
			__instance.StartCoroutine(CreateCard("darknessPet", __instance));
			return;
		}
	}
	public static IEnumerator CreateCard(string cardDataName, SelectStartingPet __instance)
	{
		Card card = CardManager.Get(Frostsuba.instance.TryGet<CardData>(cardDataName).Clone(), __instance.cardController, null, inPlay: false, isPlayerCard: true);
		__instance.group.Add(card.entity);
		__instance.pets.Add(card.entity);
		card.transform.localScale = __instance.group.GetChildScale(card.entity);
		card.transform.localPosition = __instance.startPos;
		card.hover.SetHoverable(value: false);
		yield return card.UpdateData();
	}
}
