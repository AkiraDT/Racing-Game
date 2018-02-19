using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFrameRate : MonoBehaviour {

	public int Target = 15;

	void Update () {
		Application.targetFrameRate = Target;
	}
}
