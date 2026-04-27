using Distance.Overheatmeter.Enums;
using HarmonyLib;

namespace Distance.Overheatmeter.Patches
{
	[HarmonyPatch(typeof(TrickyTextLogic), "Update")]
	internal static class TrickyTextLogic__Update
	{
		[HarmonyPostfix]
		internal static void Postfix(TrickyTextLogic __instance)
		{
			if (Mod.Instance.DisplayCondition && Mod.DisplayModeConfig.Value == DisplayMode.Hud)
			{
				__instance.SetAlpha(1);
				__instance.textMesh_.text = Mod.Instance.Text;
			}
		}
	}
}
