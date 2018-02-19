using UnityEngine;
using System.Collections;

public class RestartApplication : MonoBehaviour {
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Joystick1Button6) || Input.GetKeyDown(KeyCode.Joystick2Button6)){
			Application.LoadLevel(0);
		}
	}
}
