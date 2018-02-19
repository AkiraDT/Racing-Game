using UnityEngine;
using System.Collections;

public class DroneAnimator_Edited : MonoBehaviour {
	public Transform[] Props;
	public float propSpeed = 0;
	Transform trans;
	public float bobSpeed;
	public float bobHeight;
	public float wobble;
	public float wobbleSpeed;

	void Start () {
		trans = transform;
	}

	void Update () {
		foreach (Transform prop in Props) {
			prop.Rotate (0, 0, propSpeed * Time.deltaTime);
		}
		Vector3 pos = trans.localPosition;
		pos.y = Mathf.Sin (Time.time * bobSpeed) * bobHeight;
		trans.localPosition = pos;

		Vector3 rot = trans.localEulerAngles;
		rot.x = 270 +  Mathf.Sin (Time.time * wobbleSpeed) * wobble;
		rot.z = Mathf.Cos (Time.time * wobbleSpeed) * wobble;
		rot.y = 0;
		trans.localEulerAngles = rot;
	}
}
