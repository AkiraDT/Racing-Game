using System;
using UnityEngine;

namespace TurnTheGameOn.RacingGameTemplate.CrossPlatformInput.PlatformSpecific{
	public class RG_ButtonHandler : MonoBehaviour{

		public string Name;

		public void SetDownState(){
			RG_CrossPlatformInputManager.SetButtonDown(Name);
		}

		public void SetUpState(){
			RG_CrossPlatformInputManager.SetButtonUp(Name);
		}

		public void SetAxisPositiveState(){
			RG_CrossPlatformInputManager.SetAxisPositive(Name);
		}

		public void SetAxisNeutralState(){
			RG_CrossPlatformInputManager.SetAxisZero(Name);
		}

		public void SetAxisNegativeState(){
			RG_CrossPlatformInputManager.SetAxisNegative(Name);
		}
	}

}