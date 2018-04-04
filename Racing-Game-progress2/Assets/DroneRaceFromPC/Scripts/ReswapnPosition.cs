using UnityEngine;
using System.Collections;

public class ReswapnPosition : MonoBehaviour {

	public Transform respawnPos;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" || other.tag == "Player2") {
			/*other.transform.position = respawnPos.transform.position;
			other.transform.rotation = Quaternion.identity;
			other.GetComponent<DroneMovementScript> ().velocity = 0;
			other.GetComponent<DroneMovementScript> ().upForce = 0;*/
		}
	}
}
