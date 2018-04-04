using System;
using UnityEngine;

[ExecuteInEditMode]
public class RG_MobileController : MonoBehaviour{

	public GameObject turnLeftButton;
	public GameObject turnRightButton;
	public GameObject steeringJoystick;
	public GameObject tiltInput;
	InputData inputData;

	void Update (){
		if (inputData == null)
			inputData = Resources.Load ("InputData") as InputData;
		if(inputData.useMobileController){
			EnableControlRig (true);
		}else{
			EnableControlRig (false);
		}
	}

	void EnableControlRig (bool enabled){
		foreach (Transform t in transform){
			t.gameObject.SetActive(enabled);
		}
		float mobileType = PlayerPrefs.GetInt ("Mobile Steerint Type");
		if(mobileType == 0){
			//Using Button Input
			steeringJoystick.SetActive (false);
			turnLeftButton.SetActive (true);
			turnRightButton.SetActive (true);
			tiltInput.SetActive (false);
		}else if(mobileType == 1){
			//Using Tilt input
			steeringJoystick.SetActive (false);
			turnLeftButton.SetActive (false);
			turnRightButton.SetActive (false);
			tiltInput.SetActive (true);
		}else if(mobileType == 2){
			//Using Joystick Input
			steeringJoystick.SetActive (true);
			turnLeftButton.SetActive (false);
			turnRightButton.SetActive (false);
			tiltInput.SetActive (false);
		}
	}

}