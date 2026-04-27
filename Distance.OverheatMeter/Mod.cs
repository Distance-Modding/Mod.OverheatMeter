using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Distance.Overheatmeter.Enums;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Distance.Overheatmeter
{
	//"com.github.Seeker14491/Heat"
	//Authors: vddCore + Seekr + pigpenguin
	[BepInPlugin(modGUID, modName, modVersion)]
	public sealed class Mod : BaseUnityPlugin
	{
		//Mod Details
		private const string modGUID = "Distance.OverheatMeter";
		private const string modName = "Overheat Meter";
		private const string modVersion = "1.0.0";

		//Config Entries
		public static ConfigEntry<KeyboardShortcut> ToggleHotkey { get; set; }
		public static ConfigEntry<ActivationMode> ActivationModeConfig { get; set; }
		public static ConfigEntry<DisplayMode> DisplayModeConfig { get; set; }
		public static ConfigEntry<float> WarningThreshold { get; set; }

		//Public Variables

		//Private Variables

		//Other
		private static readonly Harmony harmony = new Harmony(modGUID);
		public static ManualLogSource Log = new ManualLogSource(modName);
		public static Mod Instance;	

		void Awake()
		{
			DontDestroyOnLoad(this);
			Flags.SubscribeEvents();

			if (Instance == null)
			{
				Instance = this;
			}

			Log = BepInEx.Logging.Logger.CreateLogSource(modGUID);
			Logger.LogInfo("Initializing Overheat Meter...");

			//Config Setup
			ToggleHotkey = Config.Bind("General", "SET TOGGLE HOTKEY", new KeyboardShortcut(KeyCode.H, new KeyCode[] { KeyCode.LeftControl }),
				new ConfigDescription("Set the shortcut to toggle the Overheat Meter UI"));

			ActivationModeConfig = Config.Bind("General", "ACTIVATION MODE", ActivationMode.Always, new ConfigDescription("Control how the UI activates"));

			DisplayModeConfig = Config.Bind("General", "DISPLAY MODE", DisplayMode.Hud, new ConfigDescription("Control how the UI displays"));

			WarningThreshold = Config.Bind("General", "WARNING THRESHOLD", 0.8f, new ConfigDescription("Adjust the warning threshold", new AcceptableValueRange<float>(0.0f, 1.0f)));

			//Apply Patches
			Logger.LogInfo("Loading...");
			harmony.PatchAll();
			Logger.LogInfo("Loaded!");
		}

		#region Utilities
		private Dictionary<string, T> MapEnumToListBox<T>() where T : Enum
		{
			var result = new Dictionary<string, T>();

			var keys = Enum.GetNames(typeof(T));
			var values = (T[])Enum.GetValues(typeof(T));

			for (int index = 0; index < keys.Length; index++)
			{
				result.Add(SplitCamelCase(keys[index]), values[index]);
			}

			return result;
		}

		private string SplitCamelCase(string str)
		{
			// https://stackoverflow.com/a/5796793
			return Regex.Replace(
				Regex.Replace(
					str,
					@"(\P{Ll})(\P{Ll}\p{Ll})",
					"$1 $2"
				),
				@"(\p{Ll})(\P{Ll})",
				"$1 $2"
			);
		}
		#endregion

		#region Data
		public bool DisplayCondition => ActivationModeConfig.Value == ActivationMode.Always ||
			(ActivationModeConfig.Value == ActivationMode.Warning && Vehicle.HeatLevel >= WarningThreshold.Value) ||
			(ActivationModeConfig.Value == ActivationMode.Toggle && Toggled);

		public bool Toggled { get; set; }

		public string Text => $"{GetHeatLevel()}\n{GetSpeed()}";

		public string GetHeatLevel()
		{
			if (G.Sys.GameManager_.IsModeStarted_)
			{
				string percent = Mathf.RoundToInt(100 * Mathf.Clamp(Vehicle.HeatLevel, 0, 1)).ToString();
				while (percent.Length < 3)
				{
					percent = $"0{percent}";
				}

				return $"{percent} %";
			}
			else
            {
				return "0%";
            }
		}

		private string GetSpeed()
		{
			if (G.Sys.GameManager_.IsModeStarted_)
			{
				switch (Options.General.Units)
				{
					case Units.Metric:
						return $"{Mathf.RoundToInt( Vehicle.VelocityKPH)} KM/H";
					case Units.Imperial:
						return $"{Mathf.RoundToInt(Vehicle.VelocityMPH)} MPH";
					default:
						return string.Empty;
				}
			}
			else
			{
				return string.Empty;
			}
		}
		#endregion

		#region Settings Changed
		public void OnConfigChanged(object sender, EventArgs e)
		{
			SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

			if (settingChangedEventArgs == null) return;

			if (sender == ToggleHotkey)
			{
				Toggled = !Toggled;

				if (!Toggled)
				{
					foreach (TrickyTextLogic trickText in FindObjectsOfType<TrickyTextLogic>())
					{
						trickText.Clear();
					}
				}
			}
		}
		#endregion
	}
}