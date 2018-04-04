using System;
using UnityEngine;
using TurnTheGameOn.RacingGameTemplate.CrossPlatformInput.PlatformSpecific;

[RequireComponent(typeof (RG_CarController))]
public class RG_CarUserControl : MonoBehaviour{
	public InputData inputData;
	private RG_CarController m_Car;

	private void Awake(){
		m_Car = GetComponent<RG_CarController>();
	}


	private void FixedUpdate()	{
		// pass the input to the car!
		float h = RG_CrossPlatformInputManager.GetAxis("Horizontal");
		float v = RG_CrossPlatformInputManager.GetAxis("Vertical");

		if(!inputData.useMobileController){
			float handbrake = RG_CrossPlatformInputManager.GetAxis("Emergency Brake");
			m_Car.Move(h, v, v, handbrake);
		} else{
			m_Car.Move(h, v, v, 0f);
		}

	}
}