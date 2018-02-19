using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

[NetworkSettings(channel=1,sendInterval=0.2f)]
public class RG_SyncData : NetworkBehaviour {

	[SyncVar(hook = "OnHorizontalChanged")] public float horizontalInput;
	[SyncVar(hook = "OnVerticalChanged")] public float verticalInput;
	[SyncVar(hook = "OnGearChanged")] public string gearString;
	[SyncVar(hook = "OnWheelRPMChanged")] public float wheelRPM;
	public Text gearText;
	public WheelCollider wheelCollider;
	public bool mobile;
	[HideInInspector] public bool singleLocalPlayer;

	private float lastHorizontalInput;
	private float lastVerticalInput;
	private float lastWheelRPM;
	private string lastGearString;

	void Start () {
		if (gearText == null) {
			gearText = GameObject.Find ("Gear Text").GetComponent<Text>();
		}
		if(GameObject.Find("Car Input") != null){
			mobile = true;
		}
	}

	void Update () {
		if(isLocalPlayer){
			GetInputs ();
		}
		else if(singleLocalPlayer){
			wheelRPM = wheelCollider.rpm;
		}
	}

	public void GetInputs(){
		wheelRPM = wheelCollider.rpm;
		if (mobile) {
			horizontalInput = TurnTheGameOn.RacingGameTemplate.CrossPlatformInput.PlatformSpecific.RG_CrossPlatformInputManager.GetAxis ("Horizontal");
			verticalInput = TurnTheGameOn.RacingGameTemplate.CrossPlatformInput.PlatformSpecific.RG_CrossPlatformInputManager.GetAxis ("Vertical");
		} else {
			horizontalInput = Input.GetAxis ("Horizontal");
			verticalInput = Input.GetAxis ("Vertical");
		}
		if (gearText != null) {
			gearString = gearText.text;
		}
		if(horizontalInput != lastHorizontalInput){
			CmdHorizontalValue (horizontalInput);
			lastHorizontalInput = horizontalInput;
		}
		if (verticalInput != lastVerticalInput) {
			CmdVerticalValue (verticalInput);
			lastVerticalInput = verticalInput;
		}
		if (gearString != lastGearString) {
			CmdGearStringValue (gearString);
			lastGearString = gearString;
		}
		if (wheelRPM != lastWheelRPM) {
			CmdWheelRPMValue (wheelRPM);
			lastWheelRPM = wheelRPM;
		}
	}

	/// <summary>
	/// Commands for SyncVars
	/// </summary>
	[Command(channel=1)]
	void CmdHorizontalValue(float value){
		SetHorizontalInput (value);
	}
	[Command(channel=1)]
	void CmdVerticalValue(float value){
		SetVerticalInput (value);
	}
	[Command(channel=1)]
	void CmdGearStringValue(string value){
		SetGearString (value);
	}
	[Command(channel=1)]
	void CmdWheelRPMValue(float value){
		SetWheelRPM (value);
	}

	/// <summary>
	/// Client Methods to process SyncVar changes
	/// </summary>
	public void SetHorizontalInput(float value){
		horizontalInput = value;
	}
	public void SetVerticalInput(float value){
		verticalInput = value;
	}
	public void SetGearString(string value){
		gearString = value;
	}
	public void SetWheelRPM(float value){
		wheelRPM = value;
	}

	/// <summary>
	/// SyncVar hooks
	/// </summary>
	void OnHorizontalChanged(float value){
		horizontalInput = value;
	}
	void OnVerticalChanged(float value){
		verticalInput = value;
	}
	void OnGearChanged(string value){
		gearString = value;
	}
	void OnWheelRPMChanged(float value){
		wheelRPM = value;
	}
}
