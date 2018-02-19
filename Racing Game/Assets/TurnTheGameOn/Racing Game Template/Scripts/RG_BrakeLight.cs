using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RG_BrakeLight : MonoBehaviour{
	public Color _colorReverse;
	public Color _colorBrakeOn;
	public Color _colorBrakeOff;
	private RG_CarController carController;

	public bool localPlayer;
	public GameObject meshRendObject;
	public Material brakeMaterial;
	public RG_SyncData syncData;
	public bool opponentCar;

	private string gameMode;
	public int materialIndex = 8;

	private void Start(){
		carController = transform.root.GetComponent<RG_CarController>();
		gameMode = PlayerPrefs.GetString ("Game Mode");
		if (gameMode == "MULTIPLAYER") {
			localPlayer = GetComponent<NetworkIdentity> ().isLocalPlayer;
		} else {
			localPlayer = true;
		}
		if (opponentCar) {
			brakeMaterial = GetComponent<MeshRenderer> ().materials [materialIndex];
		} else {
			brakeMaterial = meshRendObject.GetComponent<MeshRenderer> ().materials [materialIndex];
		}
		brakeMaterial.EnableKeyword ("_EMISSION");
	}

	private void Update(){		
		if (localPlayer) {	
			if (carController.BrakeInput > 0f) {
				if (carController.reversing) {
					brakeMaterial.SetColor ("_EmissionColor", _colorReverse);
				} else {
					brakeMaterial.SetColor ("_EmissionColor", _colorBrakeOn);
				}
			} else {
				Invoke("TurnOff", 0.5f);
			}
		} else {
			if (syncData.verticalInput < 0f) {
				if (syncData.gearString == "R") {
					brakeMaterial.SetColor ("_EmissionColor", _colorReverse);
				} else {
					brakeMaterial.SetColor ("_EmissionColor", _colorBrakeOn);
				}
			} else {
				Invoke("TurnOff", 0.5f);
			}
		}
	}

	void TurnOff(){
		brakeMaterial.SetColor ("_EmissionColor", _colorBrakeOff);
	}

}