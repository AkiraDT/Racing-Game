using UnityEngine;
using System.Collections;

public class RaceData : ScriptableObject {
	public enum Switch { Off, On }

	public int numberOfRaces;
	public string[] raceNames;
	public bool[] raceLocked;
	public int[] unlockAmount;
	public bool[] unlimitedRewards;
	public int[] firstPrize;
	public int[] secondPrize;
	public int[] thirdPrize;
	public int raceNumber;
	public int[] raceLaps;
	public int[] lapLimit;
	public int[] raceRewards;
	[Range(1,64)]public int[] numberOfRacers;
	[Range(1,64)]public int[] racerLimit;
	public int vehicleNumber;
	public float bestTime;
	public int currency;
	public Switch purchaseLevelUnlock;
	public Switch autoUnlockNextRace;
	public string lockButtonText;
	public float wrongWayDelay;
	public float[] readyTime;
	public int[] extraLapRewardMultiplier;
	public int[] extraRacerRewardMultiplier;
}