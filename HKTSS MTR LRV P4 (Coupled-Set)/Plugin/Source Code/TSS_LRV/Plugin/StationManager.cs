using OpenBveApi.Runtime;

namespace Plugin {
	internal static class StationManager {
		private static double LastAICall;
		internal static double currentTime;
		internal static bool AIEnabled;
		private static int p;
		private static double LastPos;
		internal static void ResetAITimer() {
			LastAICall = currentTime;
			AIEnabled = true;
		}

		internal static void Update(ElapseData data) {
			currentTime = data.TotalTime.Seconds;
			while (p < data.Stations.Count - 1 && data.Stations[p].StopPosition < data.Vehicle.Location) p++;
			while (p > 1 && data.Stations[p - 1].StopPosition > data.Vehicle.Location) p--;
			var nextStation = data.Stations[p];

			if (currentTime > LastAICall + 10) {
				AIEnabled = false;
			}

			if (LastPos < nextStation.DefaultTrackPosition && data.Vehicle.Location > nextStation.DefaultTrackPosition)
			{
				Plugin.doorOpened2 = false;
				Plugin.approachingStation = true;
			}

			LastPos = data.Vehicle.Location;

		}
	}
}
