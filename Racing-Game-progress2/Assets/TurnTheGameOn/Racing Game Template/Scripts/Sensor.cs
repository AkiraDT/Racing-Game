using UnityEngine;
using System.Collections;

[ExecuteInEditMode][System.Serializable]
public class Sensor : MonoBehaviour {

	public float updateInterval = 0.5f;
	public bool sensorHit;
	public float sensorHitDistance;
	public string sensorHitTag;
	public string sensorHitLayer;
	public float sensorLength;

	public LayerMask detectMask;

	public Color gizmoColor;
	public float gizmoSize;
	public bool showGizmos;
	public bool showDebugs;
	private Vector3 pos;
	private float timeleft;

	void Update () {
		timeleft -= Time.deltaTime;

		if (timeleft <= 0.0) {
			timeleft = updateInterval;

			RaycastHit hit;
			Vector3 fwd = this.transform.forward;
			pos = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);

			if (Physics.Raycast (pos, fwd, out hit, sensorLength, detectMask)) {
				if (showDebugs)
					Debug.DrawLine (pos, hit.point, Color.red);
				sensorHitDistance = hit.distance;
				sensorHitTag = hit.transform.tag;
				sensorHitLayer = LayerMask.LayerToName (hit.transform.gameObject.layer);
				sensorHit = true;
			} else {
				if (showDebugs)
					Debug.DrawRay (pos, fwd * sensorLength, Color.white);
				sensorHitDistance = 0;
				sensorHitTag = "";
				sensorHitLayer = "";
				sensorHit = false;
			}
		}
	}

	void OnDrawGizmos(){
		if (showGizmos) {
			Gizmos.color = gizmoColor;
			Gizmos.DrawSphere (pos, gizmoSize);
		}
	}
}
