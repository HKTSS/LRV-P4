using System;
using System.Collections.Generic;
using OpenBveApi.Hosts;
using OpenBveApi.Interface;
using OpenBveApi.Math;
using OpenBveApi.Trains;

public class OpenBVEScript {

	private double ElapsedTime;
	private int CurrentWiperMode;
	private bool isZAxis;
	private bool isVertical;

	public OpenBVEScript(Dictionary<string, string> args) {
		isVertical = args["vertical"] == "true";
		isZAxis = args["isZ"] == "true";
	}

	public double ExecuteScript(AbstractTrain Train, int CarIndex, Vector3 Position, double TrackPosition, 
		int SectionIndex, bool IsPartOfTrain, double TimeElapsed, int CurrentState) {
		ElapsedTime += TimeElapsed;
		
		double wiperProgress = GetWiperProgress();
		
		if(CurrentWiperMode != CurrentState && wiperProgress <= 0.02) {
			CurrentWiperMode = CurrentState;
			ElapsedTime = TimeElapsed;
		}
		
		return GetWiperValue(wiperProgress);
	}
	
	private double GetWiperProgress() {
		switch(CurrentWiperMode) {
			case 1:
				return Math.Pow(Math.Sin(ElapsedTime / 2), 100);
			case 2:
				return Math.Pow(Math.Sin(ElapsedTime / 1.5), 50);
			case 3:
				return Math.Pow(Math.Sin(ElapsedTime), 26);
			case 4:
				return Math.Pow(Math.Sin(ElapsedTime/0.5), 4);
			default:
				return 0;
		}
	}
	
	private double GetWiperValue(double wiperProgress) {
		if(isVertical) {
			if(isZAxis) {
				return wiperProgress;
			} else {
				return 0.05 * wiperProgress;
			}
		} else {
			if(isZAxis) {
				return 1.5 * wiperProgress;
			} else {
				return 0.5 - (wiperProgress / 2);
			}
		}
	}
}