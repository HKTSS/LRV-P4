using System;
using System.Collections.Generic;
using OpenBveApi.Hosts;
using OpenBveApi.Interface;
using OpenBveApi.Math;
using OpenBveApi.Trains;

public class OpenBVEScript {

	private int targetDestination;
    private double progress;
    private bool willHideReporter;

	public OpenBVEScript(Dictionary<string, string> args) {
		val = int.Parse(args["destination"]);
	}

	public double ExecuteScript(AbstractTrain Train, int CarIndex, Vector3 Position, double TrackPosition, 
		int SectionIndex, bool IsPartOfTrain, double TimeElapsed, int CurrentState) {
		return val;
	}
}
