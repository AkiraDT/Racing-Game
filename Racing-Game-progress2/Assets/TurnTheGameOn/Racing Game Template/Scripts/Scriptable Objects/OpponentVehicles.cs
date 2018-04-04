using UnityEngine;
using System.Collections;

public class OpponentVehicles : ScriptableObject {
	public string[] opponentNames;
	public GameObject[] vehicles;

	public Material[] opponentBodyMaterials;
	public Color[] opponentBodyColors;

	public Material[] opponentGlassMaterials;
	public Color[] opponentGlassColors;
}