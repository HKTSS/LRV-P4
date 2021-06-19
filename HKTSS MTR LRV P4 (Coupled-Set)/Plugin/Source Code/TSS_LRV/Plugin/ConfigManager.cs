using System.IO;
using OpenBveApi.Runtime;
using System.Globalization;
using System.Text;

namespace Plugin {
	internal class ConfigManager {
		internal static string cfgfile;
		private static string[] lines;
		private static string FailureReason;
		internal static bool LoadConfig(LoadProperties prop, Misc.LRVType LRVGen) {
			/* Get the Plugin Folder path, then combine it with LRVSystem.ini for the full path to the config file. */
			cfgfile = Path.Combine(prop.PluginFolder, "LRVSystem.ini");
			/* If the Configuration File exists: */
			if (File.Exists(cfgfile)) {
				try {
					lines = File.ReadAllLines(cfgfile, Encoding.UTF8);
				} catch {
					prop.FailureReason = "\n[LRV-System] Failed to read configuration file, aborting plugin initalization!";
					return false;
				}

				foreach (string line in lines) {
					if (!line.StartsWith("#")) {
						string[] cfg = line.Split('=');
						if (cfg.Length >= 2) {
							string key = cfg[0].Trim().ToLowerInvariant();
							string valstr = cfg[1].Trim().ToLowerInvariant();
							int val;
							string[] seperated = valstr.Split(',');
							switch (key) {
								case "carnum":
									if (int.TryParse(seperated[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out val)) {
										Plugin.carNum1 = Misc.CheckLRVNum(1, val, LRVGen);
									}
									if (seperated.Length > 1) {
										if (int.TryParse(seperated[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out val)) {
											Plugin.carNum2 = Misc.CheckLRVNum(2, val, LRVGen);
										}
									}
									break;
								case "doorlock":
									if (valstr == "true") Plugin.doorlockEnabled = true;
									else Plugin.doorlockEnabled = false;
									break;
								case "applybrake":
									if (valstr == "true") Plugin.doorApplyBrake = true;
									else Plugin.doorApplyBrake = false;
									break;
								case "ispsdoorlock":
									if (valstr == "true") Plugin.iSPSEnabled = true;
									else Plugin.iSPSEnabled = false;
									break;
								case "crash":
									if (valstr == "true") Plugin.crashEnabled = true;
									else Plugin.crashEnabled = false;
									break;
								case "mtrbeep":
									if (valstr == "true") Plugin.mtrBeeping = true;
									else Plugin.mtrBeeping = false;
									break;
								case "revatstation":
									if (valstr == "true") Plugin.AllowReverseAtStation = true;
									else Plugin.AllowReverseAtStation = false;
									break;
								case "trainstatus":
									if (int.TryParse(valstr, NumberStyles.Integer, CultureInfo.InvariantCulture, out val)) {
										if (val <= 3) Plugin.trainStatus = val;
									}
									break;
								case "version":
									Plugin.version = valstr;
									break;
							}
						}
					}
				}
				return true;
			} else {
				/* If GenerateConfig doesn't return false, that means we have generated a config file without any error. Now we can apply the changes instantly in-game */
				if (GenerateConfig()) {
					Plugin.carNum1 = 1127;
					Plugin.carNum2 = 1120;
					Plugin.doorApplyBrake = true;
					Plugin.doorlockEnabled = true;
					Plugin.iSPSEnabled = true;
					Plugin.trainStatus = 0;
					Plugin.version = "1.0.0";
					Plugin.crashEnabled = true;
					return true;
				} else {
					/* Otherwise, we set the failure reason to be the failure reason set on GenerateConfig method. Then return false to not load the plugin */
					prop.FailureReason = FailureReason;
					return false;
				}
			}
		}

		internal static void WriteConfig(string TargetKey, string TargetValue) {
			try {
				int linecount = 0;
				string[] line = File.ReadAllLines(cfgfile);
				foreach (string eachLine in line) {
					string[] cfg = eachLine.Split('=');
					string key = cfg[0].Trim().ToLowerInvariant();
					if (key == TargetKey.ToLowerInvariant()) {
						lines[linecount] = key + " = " + TargetValue;
					}
					linecount++;
				}
				File.WriteAllLines(cfgfile, lines);
			} catch {
				MessageManager.PrintMessage("Fail to save the configuration file!", OpenBveApi.Colors.MessageColor.Red, 5.0);
			}
		}

		internal static bool GenerateConfig() {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("CarNum = 1127,1120");
			sb.AppendLine("DoorLock = true");
			sb.AppendLine("ApplyBrake = true");
			sb.AppendLine("iSPSDoorLock = true");
			sb.AppendLine("Crash = true");
			sb.AppendLine("MTRbeep = false");
			sb.AppendLine("RevAtStation = false");
			sb.AppendLine("TrainStatus = 0");
			sb.Append("Version = 1.0.0");
			try {
				File.WriteAllText(cfgfile, sb.ToString());
				return true;
			} catch {
				FailureReason = "\n[LRV-System] Configuration file not found and failed to generate a config file.\n[LRV-System] Please check your permission.";
				return false;
			}
		}
	}
}
