using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TurnTheGameOn.RacingGameTemplate.CrossPlatformInput.PlatformSpecific{
	public class NitroScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
		// designed to work in a pair with another axis touch button
		// (typically with one having -1 and one having 1 axisValues)
		private string axisName = "Vertical"; // The name of the axis
		public float boostValue = 60; // The axis that the value has
		private float responseSpeed = 999; // The speed at which the axis touch button responds
		private float returnToCentreSpeed = 3; // The speed at which the button will return to its centre
		RG_AxisTouchButton m_PairedWith; // Which button this one is paired with
		RG_CrossPlatformInputManager.VirtualAxis m_Axis; // A reference to the virtual axis as it is in the cross platform input
		bool buttonPressed;
		float startTime;
		float time = 0;
		private GameObject nitroFX;
		public AudioClip nitroAudio;
		private AudioSource audioSou;

		void Start(){
			audioSou = GetComponent<AudioSource> ();
			startTime = Time.time;
			OnEnable (); 
		}

		void OnEnable(){
			if (!RG_CrossPlatformInputManager.AxisExists(axisName)){
				// if the axis doesnt exist create a new one in cross platform input
				m_Axis = new RG_CrossPlatformInputManager.VirtualAxis(axisName);
				RG_CrossPlatformInputManager.RegisterVirtualAxis(m_Axis);
			}
			else{
				m_Axis = RG_CrossPlatformInputManager.VirtualAxisReference(axisName);
			}
			FindPairedButton();
		}

		void FindPairedButton(){
			// find the other button witch which this button should be paired
			// (it should have the same axisName)
			var otherAxisButtons = FindObjectsOfType(typeof(RG_AxisTouchButton)) as RG_AxisTouchButton[];

			if (otherAxisButtons != null){
				for (int i = 0; i < otherAxisButtons.Length; i++){
					if (otherAxisButtons[i].axisName == axisName && otherAxisButtons[i] != this){
						m_PairedWith = otherAxisButtons[i];
					}
				}
			}
		}

		void Update(){
			if (nitroFX == null) {
				nitroFX = GameObject.Find ("Player/Car Nitro FX");
                if(nitroFX != null )
				    nitroFX.SetActive (false);
			}
			if (buttonPressed) {
				time -= Time.deltaTime;
				m_Axis.Update (Mathf.MoveTowards (m_Axis.GetValue, boostValue, responseSpeed * Time.deltaTime));
				if (time <= 0) {
					buttonPressed = false;
					m_Axis.Update(Mathf.MoveTowards(m_Axis.GetValue, 0, responseSpeed * Time.deltaTime));
                    if (nitroFX != null)
                        nitroFX.SetActive (false);
				}
			}
			print (time);
		}

		public void nitroGo(){
			if (!buttonPressed) {
				buttonPressed = true;
				time = 5;
				nitroFX.SetActive (true);
				audioSou.PlayOneShot (nitroAudio);
			}
		}

		public void OnPointerDown(PointerEventData data){
//			if (m_PairedWith == null){
//				FindPairedButton();
//			}
//			// update the axis and record that the button has been pressed this frame
//			buttonPressed = true;
		}

		public void OnPointerUp(PointerEventData data){
//			buttonPressed = false;
//			m_Axis.Update(Mathf.MoveTowards(m_Axis.GetValue, 0, responseSpeed * Time.deltaTime));
		}
	}
}
