using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RG_Waypoint : MonoBehaviour {

	private RG_SceneManager sceneManager;
	public int waypointNumber;

	void Start () {
		sceneManager = this.transform.parent.GetComponent<RG_SceneManager>();
	}

	void OnTriggerEnter (Collider col) {
		for(int i = 0; i < sceneManager.raceData.numberOfRacers[sceneManager.raceData.raceNumber]; i++){
			if (i == 0) {
				if (col.transform.root.name == sceneManager.playableVehicles.playerName) {
					sceneManager.ChangeTarget (0, waypointNumber);
				}
			} else if (col.transform.root.name == sceneManager.opponentVehicles.opponentNames[i - 1]) {
				sceneManager.ChangeTarget (i, waypointNumber);
			}				
		}
	}

}