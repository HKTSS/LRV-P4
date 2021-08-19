using OpenBveApi.Runtime;
using OpenBveApi.Colors;
using System;
using System.Windows.Forms;

namespace Plugin {
    /// <summary>The interface to be implemented by the plugin.</summary>
    public partial class Plugin : IRuntime {
        internal static int[] Panel = null;
        internal int speedLimit = 60;
        internal static bool crashed;
        internal static int carNum1 = 1127;
        internal static int carNum2 = 1120;
        internal static bool doorOpened;
        internal static bool doorOpened2 = true;
        internal static bool approachingStation;
        private bool iSPSDoorLock;
        private bool doorBrake;
        internal static int powerNotches;
        internal DirLight directionLight = DirLight.None;
        internal static bool doorApplyBrake;
        internal static bool doorlockEnabled;
        internal static Misc.LRVType LRVGeneration;
        internal static int totalCar;
        internal static int B67Notch;
        internal static bool iSPSEnabled;
        internal static bool crashEnabled;
        internal static bool AllowReverseAtStation;
        internal static bool mtrBeeping;
        internal static string language = "en-US";
        internal static string version;
        internal static SpeedMode currentSpeedMode;
        internal static CameraViewMode cameraMode;
        internal static int trainStatus;

        /// <summary>Is called when the plugin is loaded.</summary>
        public bool Load(LoadProperties prop) {
            /* Get LRV Generation with GetLRVGen method/function in Misc.cs */
            LRVGeneration = Misc.GetLRVGen(prop.TrainFolder);
            /* Set SpeedMode to normal, used for the "Turtle/Elephant switch" in LRV Panel */
            currentSpeedMode = SpeedMode.Normal;
            /* Initialize MessageManager, so we can print message on the top-left screen later. */
            MessageManager.Initialise(prop.AddMessage);
            /* Initialize an empty array with 256 elements, used for Panel Indicator. */
            Panel = new int[256];
            /* Initialize Sound, used to play sound later on. */
            SoundManager.Initialise(prop.PlaySound, prop.PlayCarSound, 256);
            prop.Panel = Panel;
            /* Set failure reason, this will be printed if the Plugin did not load for some reasons. However the failure reason should always override this default FailureReason */
            prop.FailureReason = "LRV plugin failed to load, some functions will be unavailable.";
            /* We need to access OpenBVE's built in A.I. */
            prop.AISupport = AISupport.Basic;
            /* We have to return true for OpenBVE to actually load the plugin. If this is false, OpenBVE will not load the plugin and the reason above will be used in the logs */
            /* In the LoadConfig Method, we returns true if we can find the configuration file. If we can't find it, we return false to not load the plugin as this is likely to cause problems later on */
            return ConfigManager.LoadConfig(prop, LRVGeneration);
        }

        public void Unload() {
        }

        /// <summary>Is called after loading to inform the plugin about the specifications of the train.</summary>
        public void SetVehicleSpecs(VehicleSpecs specs) {
            /* Train Information can only be accessed in the SetVehicleSpecs Method, so we created some variable (TotalCar and B67Notch) earlier on. */
            /* Then we put the information into the variable we created earlier, so we can access it later if needed. */
            /* Get the total amount of car this train has, used to dynamically disable/enable elements in the configuration form later */
            totalCar = specs.Cars;
            /* Get the total amount of power notch, used to clamp the maximum value on the elephant button in the cab */
            powerNotches = specs.PowerNotches;
            /* Get 67% of brake notch, used to apply brake while in the stations */
            B67Notch = specs.B67Notch;
            /* Set the panel indicator for displaying the car number */
            Panel[PanelIndices.FirstCarNumber] = Misc.CarNumPanel(carNum1);
            Panel[PanelIndices.SecondCarNumber] = Misc.CarNumPanel(carNum2);
        }

        /// <summary>Is called when the plugin should initialize, reinitialize or jumping stations.</summary>
        public void Initialize(InitializationModes mode) {
            ResetLRV(ResetType.JumpStation);
        }

        /// <summary>This is called every frame. If you have 60fps, then this method is called 60 times in 1 second</summary>
        public void Elapse(ElapseData data) {

            /* Update data for StationManager. */
            StationManager.Update(data);
            /* Get system language, used for displaying train settings dialog later. */
            language = data.CurrentLanguageCode;
            cameraMode = data.CameraViewMode;

            /* Lock the door above 2 km/h */
            if (data.Vehicle.Speed.KilometersPerHour > 2 && doorlockEnabled) {
                data.DoorInterlockState = DoorInterlockStates.Locked;
            } else {
                data.DoorInterlockState = DoorInterlockStates.Unlocked;
            }

            if (data.Vehicle.Speed.KilometersPerHour > speedLimit - 5) {
                Panel[PanelIndices.iSPSOverSpeed] = 1;
            } else {
                Panel[PanelIndices.iSPSOverSpeed] = 0;
            }

            if ((doorBrake && doorApplyBrake) || (iSPSDoorLock && iSPSEnabled)) {
                data.Handles.PowerNotch = 0;
                data.Handles.BrakeNotch = B67Notch;
            }

            if (directionLight == DirLight.Left) {
                Panel[PanelIndices.DirRight] = 0;
                Panel[PanelIndices.DirLeft] = 1;
            } else if (directionLight == DirLight.Right) {
                Panel[PanelIndices.DirRight] = 1;
                Panel[PanelIndices.DirLeft] = 0;
            } else if (directionLight == DirLight.Both) {
                Panel[PanelIndices.DirRight] = 1;
                Panel[PanelIndices.DirLeft] = 1;
            } else {
                Panel[PanelIndices.DirRight] = 0;
                Panel[PanelIndices.DirLeft] = 0;
            }

            /* Clamp the power notch to P1 on slow mode. */
            if (currentSpeedMode == SpeedMode.Slow && data.Handles.PowerNotch > 1) {
                data.Handles.PowerNotch = 1;
            } else if (currentSpeedMode != SpeedMode.Fast && data.Handles.PowerNotch == powerNotches) {
                /* We reserve the last notch for the fast (aka "Elephant") mode. If the current speed mode is not fast and current power notch is the last notch: Clamp it to last notch - 1 */
                data.Handles.PowerNotch = powerNotches - 1;
            }

            if (data.PrecedingVehicle != null) {
                if (data.PrecedingVehicle.Distance < 0.1 && data.PrecedingVehicle.Distance > -4 && crashed == false && crashEnabled) {
                    /* Crash Sounds */
                    SoundManager.Play(SoundIndices.Crash, 1.0, 1.0, false);

                    if (Math.Abs(data.PrecedingVehicle.Speed.KilometersPerHour - data.Vehicle.Speed.KilometersPerHour) > 10) {
                        Panel[213] = 1;
                        Panel[PanelIndices.HeadLight] = 1;

                        if (Math.Abs(data.PrecedingVehicle.Speed.KilometersPerHour - data.Vehicle.Speed.KilometersPerHour) > 17) {
                            Panel[PanelIndices.SpeedoLight] = 1;
                            directionLight = DirLight.None;
                            Panel[PanelIndices.DirLeft] = 0;
                            Panel[PanelIndices.DirRight] = 0;
                        }
                    }
                    crashed = true;
                }
            }

            if (approachingStation && data.Vehicle.Speed.KilometersPerHour < 0.1 && doorOpened2 == false) {
                Panel[202] = 1;
                /* If the reverser is Forward */
                if (data.Handles.Reverser == 1) {
                    iSPSDoorLock = true;
                } else {
                    if(AllowReverseAtStation) iSPSDoorLock = false;
                }
            }

            if (data.Vehicle.Speed.KilometersPerHour > 10 && Panel[202] == 1) Panel[202] = 0;

            /* Turn signal sound in cab */
            if (directionLight != DirLight.None) {
                if (cameraMode == CameraViewMode.Interior || cameraMode == CameraViewMode.InteriorLookAhead) {
                    SoundManager.Play(SoundIndices.CabDirIndicator, 1.0, 1.0, true);
                } else {
                    SoundManager.Stop(SoundIndices.CabDirIndicator);
                }
            } else {
                SoundManager.Stop(SoundIndices.CabDirIndicator);
            }

            Panel[PanelIndices.TrainStatus] = trainStatus;
        }

        public void SetReverser(int reverser) {
        }

        public void SetPower(int powerNotch) {
        }

        public void SetBrake(int brakeNotch) {
        }

        /// <summary>Is called when a virtual key is pressed.</summary>
        public void KeyDown(VirtualKeys key) {
            VirtualKeys virtualKey = key;
            switch (virtualKey) {
                /* GearDown = Ctrl + G */
                case VirtualKeys.GearDown:
                    ConfigForm.LaunchForm();
                    break;
                case VirtualKeys.S:
                    Panel[PanelIndices.CabDoor] ^= 1;
                    break;
                case VirtualKeys.A1:
                    ResetLRV(0);
                    break;
                case VirtualKeys.A2:
                    if (cameraMode == CameraViewMode.Interior || cameraMode == CameraViewMode.InteriorLookAhead) SoundManager.Play(SoundIndices.Click, 1.0, 1.0, false);
                    if ((int)currentSpeedMode == 2) currentSpeedMode = SpeedMode.Normal;
                    else currentSpeedMode++; Panel[PanelIndices.SpeedModeSwitch] = (int)currentSpeedMode;
                    break;
                case VirtualKeys.D:
                    ToggleDirLight(DirLight.Left);
                    break;
                case VirtualKeys.E:
                    ToggleDirLight(DirLight.Right);
                    break;
                case VirtualKeys.F:
                    Panel[PanelIndices.LED_Dest] ^= 1;
                    Panel[PanelIndices.Dest_Override]++;
                    break;
                case VirtualKeys.G:
                    Panel[PanelIndices.Digit1] ^= 1;
                    break;
                case VirtualKeys.H:
                    Panel[PanelIndices.Digit2] ^= 1;
                    break;
                case VirtualKeys.I:
                    Panel[PanelIndices.Digit3] ^= 1;
                    break;
                case VirtualKeys.J:
                    Panel[PanelIndices.LightToggle] ^= 1;
                    break;
                case VirtualKeys.K:
                    Panel[PanelIndices.HeadLight] ^= 1;
                    break;
                case VirtualKeys.L:
                    Panel[PanelIndices.SpeedoLight] ^= 1;
                    break;
                case VirtualKeys.LeftDoors:
                    if (doorOpened && mtrBeeping) {
                        if (SoundManager.IsPlaying(SoundIndices.MTRBeep)) {
                            SoundManager.Stop(SoundIndices.MTRBeep);
                        } else {
                            SoundManager.PlayCarriage(SoundIndices.MTRBeep, 2.0, 1.0, false, 0);
                            if (totalCar == 2) SoundManager.PlayCarriage(SoundIndices.MTRBeep, 2.0, 1.0, false, 1);
                        }
                    }
                    break;
                case VirtualKeys.MainBreaker:
                    ToggleDirLight(DirLight.Both);
                    break;
            }
        }

        /// <summary>Is called when a virtual key is released.</summary>
        public void KeyUp(VirtualKeys key) {
        }

        public void HornBlow(HornTypes type) {
        }

        /// <summary>Is called when the state of the doors changes.</summary>
        public void DoorChange(DoorStates oldState, DoorStates newState) {
            /* Door is opened */
            if (oldState == DoorStates.None & newState != DoorStates.None) {
                doorOpened = true;
                doorOpened2 = true;
                doorBrake = true;
                /* Door is closed */
            } else if (oldState != DoorStates.None & newState == DoorStates.None) {
                doorOpened = false;
                Panel[204] = 0;
                approachingStation = false;
                iSPSDoorLock = false;
                doorBrake = false;
            }
        }
        public void SetSignal(SignalData[] signal) {
        }

        /// <summary>Is called when the train passes a beacon.</summary>
        /// <param name="beacon">The beacon data.</param>
        public void SetBeacon(BeaconData beacon) {
            switch (beacon.Type) {
                case 12:
                    if (beacon.Optional > 0) speedLimit = beacon.Optional;
                    break;
                case 105:
                    if (StationManager.AIEnabled)
                        if (beacon.Optional == 1) {
                            if (directionLight != DirLight.Left) KeyDown(VirtualKeys.D);
                        } else {
                            if (directionLight != DirLight.None) KeyDown(VirtualKeys.D);
                        }
                    break;
                case 106:
	                if (StationManager.AIEnabled) {
		                if (beacon.Optional == 1 && directionLight != DirLight.Right) {
			                KeyDown(VirtualKeys.E);
		                } else if (directionLight != DirLight.None) {
			                KeyDown(VirtualKeys.E);
		                }
	                }
	                break;
            }
        }

        public void PerformAI(AIData data) {
            StationManager.ResetAITimer();
        }

        internal void ToggleDirLight(DirLight direction) {
            if (direction == DirLight.Left) {
                if (directionLight == direction) {
                    directionLight = DirLight.None;
                } else {
                    directionLight = DirLight.Left;
                }
            } else if (direction == DirLight.Right) {
                if (directionLight == direction) {
                    directionLight = DirLight.None;
                } else {
                    directionLight = DirLight.Right;
                }
            } else if (direction == DirLight.Both) {
                if (directionLight == direction) {
                    directionLight = DirLight.None;
                    Panel[PanelIndices.DirBoth] = 0;
                } else {
                    if (directionLight == DirLight.Left || directionLight == DirLight.Right) {
                        /* Hardcoded translation, I am too lazy. */
                        if (language.StartsWith("zh")) {
                            MessageManager.PrintMessage("開啟死火燈前, 請先關閉指揮燈", MessageColor.Orange, 6.0);
                        } else {
                            MessageManager.PrintMessage("Please switch off the turn signal before activating the hazard warning light.", MessageColor.Orange, 5.0);
                        }
                    } else {
	                    Panel[PanelIndices.DirBoth] = 1;
	                    directionLight = DirLight.Both;
                    }
                }

            }
            /* If our current camera mode is in cab(F1), play the click sound as it should only be heard in cab. */
            if (cameraMode == CameraViewMode.Interior || cameraMode == CameraViewMode.InteriorLookAhead) SoundManager.Play(SoundIndices.Click, 1.0, 1.0, false);
        }

        public void ResetLRV(ResetType mode) {
            if (mode == ResetType.JumpStation) {
                doorOpened2 = true;
                doorBrake = true;
                iSPSDoorLock = false;
                if (crashed) {
                    crashed = false;
                    Panel[PanelIndices.SpeedoLight] = 0;
                    Panel[PanelIndices.HeadLight] = 0;
                    Panel[213] = 0;
                }
            }
            StationManager.AIEnabled = false;
            iSPSDoorLock = false;
            approachingStation = false;
            doorBrake = false;
            Panel[202] = 0;
            Panel[203] = 0;
        }

        internal static void ChangeTrainNumber(int car, int states) {
            if (car == 1) {
                Panel[PanelIndices.FirstCarNumber] = states;
            } else {
                Panel[PanelIndices.SecondCarNumber] = states;
            }
        }
    }

    public enum ResetType {
        JumpStation,
        ManualReset,
    }

    internal enum DirLight {
        Left,
        Right,
        Both,
        None
    }

    internal enum SpeedMode {
        Normal,
        Fast,
        Slow
    }
}