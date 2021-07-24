using Distance.Overheatmeter.Scripts;
using HarmonyLib;
using System;

namespace Distance.Overheatmeter.Harmony
{
	[HarmonyPatch(typeof(SpeedrunTimerLogic), "OnEnable")]
	internal static class SpeedrunTimerLogic__OnEnable
	{
		[HarmonyPostfix]
		internal static void Postfix(SpeedrunTimerLogic __instance)
		{
			try
			{
				HeatTextLogic.Create(__instance.gameObject);
			}
			catch (Exception e)
			{
				Mod.Instance.Logger.Exception(e);
			}
		}
	}
}