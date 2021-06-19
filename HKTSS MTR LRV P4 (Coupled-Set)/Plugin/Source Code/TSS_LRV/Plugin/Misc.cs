using System.IO;

namespace Plugin {
	static class Misc {
		internal static int CarNumPanel(int CarNum) {
			switch (CarNum) {
				case 1111:
					return 1;
				case 1112:
					return 2;
				case 1113:
					return 3;
				case 1114:
					return 4;
				case 1115:
					return 5;
				case 1116:
					return 6;
				case 1117:
					return 7;
				case 1118:
					return 8;
				case 1119:
					return 9;
				case 1120:
					return 10;
				case 1121:
					return 11;
				case 1122:
					return 12;
				case 1123:
					return 13;
				case 1124:
					return 14;
				case 1125:
					return 15;
				case 1126:
					return 16;
				case 1127:
					return 17;
				case 1128:
					return 18;
				case 1129:
					return 19;
				case 1130:
					return 20;
				case 1131:
					return 21;
				case 1132:
					return 22;
				default:
					return 0;
			}
		}

		internal enum LRVType {
			P7,
			P6,
			P5,
			P4,
			P3,
			P1R,
			P1,
			Invalid,
		}

		internal static LRVType GetLRVGen(string TrainFolder) {
			var P4 = Path.Combine(TrainFolder, "P4_1");
			var P3 = Path.Combine(TrainFolder, "1106");
			var P1R = Path.Combine(TrainFolder, "P1_1");
			if (Directory.Exists(P4)) {
				return LRVType.P4;
			}

			if (Directory.Exists(P3)) {
				return LRVType.P3;
			}
			
			if (Directory.Exists(P1R)) {
				return LRVType.P1R;
			} else {
				return LRVType.Invalid;
			}
		}

		/* This method are called for each car, which validates the car number depending on what phase the train is. This will also reset to the default value if the car number is invalid */
		internal static int CheckLRVNum(int CarIndex, int val, LRVType LRVGen) {
			if (LRVGen == LRVType.P4) {
				if (val >= 1111 && val <= 1132) return val;

				if (CarIndex == 1) return 1127;
				else return 1120;
			}

			if (LRVGen == LRVType.P3) {
				if (val >= 1091 && val <= 1110) return val;
				if (CarIndex == 1) return 1106;
				else return 1091;
			}

			if (LRVGen == LRVType.P1 || LRVGen == LRVType.P1R) {
				if (val >= 1001 && val <= 1070) return val;
				if (CarIndex == 1) return 1043;
				else return 1033;
			}
			return 1001;
		}
	}
}
