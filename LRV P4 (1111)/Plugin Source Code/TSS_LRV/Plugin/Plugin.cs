using OpenBveApi.Runtime;
using OpenBveApi.Colors;

namespace Plugin
{
	/// <summary>The interface to be implemented by the plugin.</summary>
	public partial class Plugin : IRuntime
	{

		private int[] Panel = null;
		private int SpeedLimit = 60;
		private bool crashed = false;
		private bool DoorOpened = true;
		private bool ApproachingStation = false;
		private bool iSPSDoorLock = false;
		private bool DoorBrake = false;
		private bool SysBypass = false;
		private bool inCab = false;
		/// <summary>Is called when the plugin is loaded.</summary>
		public bool Load(LoadProperties properties) {
			MessageManager.Initialise(properties.AddMessage);
			Panel = new int[256];
			Sound = new SoundHelper(properties.PlaySound, 256);
			properties.Panel = Panel;
			properties.FailureReason = "LRV plugin failed to load, some functions will be unavailable.";
			return true;
		}

		public void Unload() {
		}

		/// <summary>Is called after loading to inform the plugin about the specifications of the train.</summary>
		public void SetVehicleSpecs(VehicleSpecs specs) {
		}

		/// <summary>Is called when the plugin should initialize, reinitialize or jumping stations.</summary>
		public void Initialize(InitializationModes mode)
		{
			ResetLRV(1);
		}

		/// <summary>Is called every frame.</summary>
		public void Elapse(ElapseData data)
		{
			if (data.CameraViewMode == CameraViewMode.Interior || data.CameraViewMode == CameraViewMode.InteriorLookAhead) {
				inCab = true;
			} else {
				inCab = false;
			}

			if (SysBypass == false)
			{
				if (data.Vehicle.Speed.KilometersPerHour > 2)
				{
					data.DoorInterlockState = DoorInterlockStates.Locked;
				} else {
					data.DoorInterlockState = DoorInterlockStates.Unlocked;
				}

				//iSPS Overspeed indication
				if (data.Vehicle.Speed.KilometersPerHour > SpeedLimit - 5)
				{
					Panel[201] = 1;
				} else {
					Panel[201] = 0;
				}

				if (DoorBrake) {
					data.Handles.PowerNotch = 0;
					data.Handles.BrakeNotch = 6;
				}

				if (iSPSDoorLock) {
					data.Handles.PowerNotch = 0;
					data.Handles.BrakeNotch = 6;
				}
			} else {
				//If System bypass is false, keep the door unlocked
				data.DoorInterlockState = DoorInterlockStates.Unlocked;
			}

			if (data.Vehicle.Speed.KilometersPerHour < 0.2 && DoorOpened == false) Panel[204] = 1;

			//Crash Animation - Check vehicle distances
			if (data.PrecedingVehicle != null)
			{
				if (data.PrecedingVehicle.Distance < 0.1 && data.PrecedingVehicle.Distance > -1 && crashed == false)
				{
						Sound[223] = SoundInstructions.PlayOnce; //Crash Sound
						Panel[105] = 0;
						Panel[106] = 0;
						Panel[101] = 1;
						Panel[213] = 1;
						Panel[100] = 1;
						crashed = true;
				}

			}

			if (ApproachingStation && data.Vehicle.Speed.KilometersPerHour < 0.1 && DoorOpened == false)
			{
				Panel[202] = 1;
				iSPSDoorLock = true;
			}

			if (data.Vehicle.Speed.KilometersPerHour > 10 && Panel[202] == 1) Panel[202] = 0;

			// Direction light sound in cab
			if (Panel[105] == 1 || Panel[106] == 1)
			{
				// Play the sound only when the camera is in F1
				if (inCab)
				{
					Sound[222] = SoundInstructions.PlayLooping;
				} else {
					Sound[222] = SoundInstructions.Stop;
				}
			} else {
				Sound[222] = SoundInstructions.Stop;
			}

			Sound.Update();
		}

		public void SetReverser(int reverser)
		{
		}

		public void SetPower(int powerNotch)
		{
		}

		public void SetBrake(int brakeNotch)
		{
		}

		/// <summary>Is called when a virtual key is pressed.</summary>
		public void KeyDown(VirtualKeys key)
		{
			VirtualKeys virtualKey = key;
			switch (virtualKey)
			{
				case VirtualKeys.S:
					Panel[250] ^= 1;
					break;
				case VirtualKeys.A1:
					ResetLRV(0);
					MessageManager.PrintMessage("Protection system reset", (MessageColor)5, 3.0);
					break;
				case VirtualKeys.A2:
					Panel[60] = 1;
					SysBypass = true;
					MessageManager.PrintMessage("Bypassing protection system", (MessageColor)5, 4.0);
					break;
				case VirtualKeys.B1:
					Panel[61] ^= 1;
					break;
				case VirtualKeys.B2:
					Panel[62] ^= 1;
					break;
				case VirtualKeys.C1:
					Panel[63] ^= 1;
					break;
				case VirtualKeys.C2:
					Panel[64] ^= 1;
					break;
				case VirtualKeys.D:
					Panel[105] ^= 1;
					if(inCab) Sound[221] = SoundInstructions.PlayOnce;
					break;
				case VirtualKeys.E:
					Panel[106] ^= 1;
					if (inCab) Sound[221] = SoundInstructions.PlayOnce;
					break;
				case VirtualKeys.F:
					Panel[107] ^= 1;
					break;
				case VirtualKeys.G:
					Panel[102] ^= 1;
					break;
				case VirtualKeys.H:
					Panel[103] ^= 1;
					break;
				case VirtualKeys.I:
					Panel[104] ^= 1;
					break;
				case VirtualKeys.J:
					Panel[99] ^= 1;
					break;
				case VirtualKeys.K:
					Panel[100] ^= 1;
					break;
				case VirtualKeys.L:
					Panel[101] ^= 1;
					break;
				default:
					break;
			}
		}

		/// <summary>Is called when a virtual key is released.</summary>
		public void KeyUp(VirtualKeys key)
		{
			VirtualKeys virtualKey = key;
			switch (virtualKey) {
				case VirtualKeys.A2:
					Panel[60] = 0;
					break;
				default:
					break;
			}
		}

		public void HornBlow(HornTypes type)
		{
		}

		/// <summary>Is called when the state of the doors changes.</summary>
		public void DoorChange(DoorStates oldState, DoorStates newState)
		{
			if (oldState == DoorStates.None & newState != DoorStates.None) //Door is opened
			{
				DoorOpened = true;
				DoorBrake = true;
			}
			else if (oldState != DoorStates.None & newState == DoorStates.None) //Door is closed
			{
				Panel[204] = 0;
				ApproachingStation = false;
				iSPSDoorLock = false;
				DoorBrake = false;
			}
		}
		public void SetSignal(SignalData[] signal)
		{
		}

		/// <summary>Is called when the train passes a beacon.</summary>
		/// <param name="beacon">The beacon data.</param>
		public void SetBeacon(BeaconData beacon)
		{
			switch (beacon.Type)
			{
				case 12:
					//Set speed limit
					if (beacon.Optional > 0.1) SpeedLimit = beacon.Optional;
					break;
				case 13:
					//Define a station
					ApproachingStation = true;
					DoorOpened = false;
					break;
				case 14:
					//Set Route Display
					if (beacon.Optional > 0 && beacon.Optional < 24)
						Panel[110] = beacon.Optional;
					break;
				default:
					break;
			}
		}

		public void PerformAI(AIData data)
		{
		}

		public void ResetLRV(int Mode)
		{
			if (Mode == 1)
			{
				DoorOpened = true;
				DoorBrake = true;
				iSPSDoorLock = false;
				if (crashed)
				{
					crashed = false;
					Panel[101] = 0;
					Panel[100] = 0;
					Panel[213] = 0;
				}
			}
			//Reset system
			iSPSDoorLock = false;
			ApproachingStation = false;
			DoorBrake = false;
			Panel[202] = 0;
			Panel[203] = 0;
		}
	}
}