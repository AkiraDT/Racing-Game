using UnityEngine;

namespace TurnTheGameOn.RacingGameTemplate.CrossPlatformInput.PlatformSpecific{
	public class RG_InputAxisScrollbar : MonoBehaviour{
		public string axis;

		public void HandleInput(float value){
			RG_CrossPlatformInputManager.SetAxis(axis, (value*2f) - 1f);
		}
	}

}