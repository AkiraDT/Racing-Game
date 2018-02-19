using UnityEngine;
using System.Collections;

public class PlayableVehicles : ScriptableObject {

	public enum CarPrefabType { Default, Edys }

	public CarPrefabType carPrefabType;

	public int startingCurrency;
	public int paintPrice;
	public int brakeColorPrice;
	public int rimColorPrice;
	public int glassColorPrice;
	public int glowPrice;
	public int upgradeSpeedPrice;
	public int upgradeAccelerationPrice;
	public int upgradeBrakesPrice;
	public int upgradeTiresPrice;
	public int upgradeSteeringPrice;

	public int currentVehicleNumber;
	public int numberOfCars;
	public string playerName = "Player";
	public GameObject[] vehicles;
	public string[] vehicleNames;
	public int[] price;
	public Material[] carMaterial;
	public Material[] brakeMaterial;
	public Material[] glassMaterial;
	public Material[] rimMaterial;

	public Color[] defaultBodyColors;
	public Color[] defaultBrakeColors;
	public Color[] defaultGlassColors;
	public Color[] defaultRimColors;
	public Color[] defaultNeonColors;

	public ParticleSystem[] carGlowLight;
	public bool[] carUnlocked;
	[Range(0,9)] public int[] topSpeedLevel;
	[Range(0,9)] public int[] torqueLevel;
	[Range(0,9)] public int[] brakeTorqueLevel;
	[Range(0,9)] public int[] tireTractionLevel;
	[Range(0,9)] public int[] steerSensitivityLevel;
	public Color[] carBodyColorPreset;
	public Color[] carGlassColorPreset;
	public Color[] carBrakeColorPreset;
	public Color[] carRimColorPreset;
	public Color[] carNeonColorPreset;
}