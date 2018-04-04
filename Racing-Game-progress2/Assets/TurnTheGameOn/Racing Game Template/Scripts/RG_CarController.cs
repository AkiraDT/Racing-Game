﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

internal enum CarDriveType
{
	FrontWheelDrive,
	RearWheelDrive,
	FourWheelDrive
}

internal enum SpeedType
{
	MPH,
	KPH
}

public class RG_CarController : MonoBehaviour
{

	public int vehicleNumber;
	public bool playerCar;
	public float nitroTopSpeed;
	public float nitroFullTorque;
	public float nitroDuration;
	public float nitroSpendRate;
	public float nitroRefillRate;
	public GameObject nitroFX;
	Slider nitroSlider;
	public EventTrigger.Entry nitroON;
	public EventTrigger.Entry nitroOFF;
	public bool nitroOn;
	float nitroAmount;
	public int levelTopSpeed;
	public int levelAcceleration;
	public int levelBrakePower;
	public int levelTireTraction;
	public int levelSteerSensitivity;
	public float levelBonusTopSpeed = 5;
	public float levelBonusAcceleration = 75;
	public float levelBonusBrakePower = 100;
	public float levelBonusTireTraction = 0.04f;
	public float levelBonusSteerSensitivity = 0.02f;
	private bool isAudioMuted;

	[ContextMenu("NiroON")]
	public void NitroOn() {
		if (!nitroOn && nitroAmount > 2.0f) {
			if (!isAudioMuted) {
				GameObject tempObject = Instantiate (Resources.Load ("Audio Clip - Nitro")) as GameObject;
				tempObject.name = "Audio Clip - Nitro";
				tempObject = null;
			}
			nitroFX.SetActive (true);
			m_Topspeed = m_Topspeed + nitroTopSpeed;
			m_FullTorqueOverAllWheels = m_FullTorqueOverAllWheels + nitroFullTorque;
			nitroOn = true;
		}
	}

	[ContextMenu("NiroOFF")]
	public void NitroOff() {
		if (nitroOn) {
			nitroFX.SetActive(false);
			m_Topspeed = m_Topspeed - nitroTopSpeed;
			m_FullTorqueOverAllWheels = m_FullTorqueOverAllWheels - nitroFullTorque;
			nitroOn = false;
		}
	}



	public bool autoFindSpeedText = true;
	public bool autoFindGearText = true;
	public bool autoFindRPMSlider = true;
	public bool autoFindDistanceMetricText = true;
	Text distanceMetricText;
	Text speedText;
	Text gearText;
	Slider RPMSlider;

	[SerializeField] private CarDriveType m_CarDriveType = CarDriveType.FourWheelDrive;
	[SerializeField] private SpeedType m_SpeedType;
	[SerializeField] public float m_Topspeed = 200;		
	[SerializeField] private float m_FullTorqueOverAllWheels;
	[SerializeField] private float m_ReverseTorque;
	[SerializeField] private float m_BrakeTorque;
	[SerializeField] private float m_MaxHandbrakeTorque;
	[SerializeField] private float m_Downforce = 100f;
	[SerializeField] private float m_RevRangeBoundary = 1f;
	[SerializeField] private float m_SlipLimit;
	[SerializeField] private Vector3 m_CentreOfMassOffset;	
	[SerializeField] private static int NoOfGears = 5;

	[SerializeField] private float m_SteerSensitivity;
	[Range(0, 90)] [SerializeField] private float m_MaximumSteerAngle;
	[Range(0, 90)] [SerializeField] private float m_SteerAngleAtMaxSpeed;
	[Tooltip("0 is raw physics , 1 the car will grip in the direction it is facing")] [Range(0, 1)] [SerializeField] private float m_SteerHelper;
	[Tooltip("0 is no traction control, 1 is full interference")] [Range(0, 1)] [SerializeField] private float m_TractionControl;

	public WheelCollider[] m_WheelColliders = new WheelCollider[4];
	public GameObject[] m_WheelMeshes = new GameObject[4];
	[SerializeField] private RG_WheelEffects[] m_WheelEffects = new RG_WheelEffects[4];
	private Quaternion[] m_WheelMeshLocalRotations;
	private Vector3 m_Prevpos, m_Pos;
	private float m_SteerAngle;
	private int m_GearNum;
	private float m_GearFactor;
	private float m_OldRotation;
	private float m_CurrentTorque;
	private Rigidbody m_Rigidbody;
	private const float k_ReversingThreshold = 0.01f;

	public bool Skidding { get; private set; }
	public float BrakeInput { get; private set; }
	public float CurrentSteerAngle{ get { return m_SteerAngle; }}
	public float CurrentSpeed{ get { return m_Rigidbody.velocity.magnitude*2.23693629f; }}
	public float MaxSpeed{get { return m_Topspeed; }}
	public float Revs { get; private set; }
	public float AccelInput { get; private set; }
	public float speed;
	public bool reversing;

	public bool overrideBrake;
	public float overrideBrakePower;//overrides the brake input value, used to force ai to brake
	public bool overrideAcceleration;
	public float overrideAccelerationPower;//overrides the brake input value, used to force ai to brake

	// Use this for initialization
	private void Start()
	{
		if (playerCar)
		{
			if (PlayerPrefs.GetString ("Audio", "ON") == "OFF") {
				isAudioMuted = true;
			} else {
				isAudioMuted = false;
			}
			GameObject lobbyImage = GameObject.Find ("Lobby Loading Image");
			if(lobbyImage != null)
				lobbyImage.SetActive(false);
			vehicleNumber = PlayerPrefs.GetInt("Vehicle Number", 0);
			levelTopSpeed = PlayerPrefs.GetInt("CarTopSpeedLevel" + vehicleNumber.ToString(), 0);
			levelAcceleration = PlayerPrefs.GetInt("CarTorqueLevel" + vehicleNumber.ToString(), 0);
			levelBrakePower = PlayerPrefs.GetInt("CarBrakeLevel" + vehicleNumber.ToString(), 0);
			levelTireTraction = PlayerPrefs.GetInt("CarTireTractionLevel" + vehicleNumber.ToString(), 0);
			levelSteerSensitivity = PlayerPrefs.GetInt("CarSteerSensitivityLevel" + vehicleNumber.ToString(), 0);
			m_Topspeed += levelTopSpeed * levelBonusTopSpeed;
			m_FullTorqueOverAllWheels += levelAcceleration * levelBonusAcceleration;
			m_BrakeTorque += levelBrakePower * levelBonusBrakePower;
			m_TractionControl += levelTireTraction * levelBonusTireTraction;
			m_SteerSensitivity += levelSteerSensitivity * levelBonusSteerSensitivity;
			if (GameObject.Find("Car Input"))
			{
				EventTrigger nitroButton = GameObject.Find("Nitro Button").GetComponent<EventTrigger>();
				EventTrigger.Entry entry = nitroON;
				nitroButton.triggers.Add(entry);
				entry = nitroOFF;
				nitroButton.triggers.Add(entry);
			}
			nitroSlider = GameObject.Find("Nitro Slider").GetComponent<Slider>();
			nitroAmount = nitroDuration;
		}
		if (autoFindDistanceMetricText) {
			distanceMetricText = GameObject.Find("MPH Text").GetComponent<Text>();
			switch (m_SpeedType) {
			case SpeedType.KPH:
				distanceMetricText.text = "KPH";
				break;
			case SpeedType.MPH:
				distanceMetricText.text = "MPH";
				break;
			}
		}
		if (autoFindSpeedText) {
			speedText = GameObject.Find("Speed Text").GetComponent<Text>();
		}
		if (autoFindGearText) {
			gearText = GameObject.Find("Gear Text").GetComponent<Text>();
		}
		if(autoFindRPMSlider){
			RPMSlider = GameObject.Find("RPM Slider").GetComponent<Slider>();
		}
		m_WheelMeshLocalRotations = new Quaternion[4];
		for (int i = 0; i < 4; i++)
		{
			m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
		}
		//m_WheelColliders[0].attachedRigidbody.centerOfMass = m_CentreOfMassOffset;

		m_MaxHandbrakeTorque = float.MaxValue;

		m_Rigidbody = GetComponent<Rigidbody>();
		m_CurrentTorque = m_FullTorqueOverAllWheels - (m_TractionControl*m_FullTorqueOverAllWheels);
	}

	void Update() {
		if (playerCar) {
			//  if (Input.GetKeyDown("n")) {
			//    NitroOn();
			// }
			// if (Input.GetKeyUp("n")){
			//    NitroOff();
			//}
			nitroSlider.value = nitroAmount;
			if (!nitroOn && nitroAmount < nitroDuration) {
				nitroAmount += nitroRefillRate * Time.deltaTime;
				if (nitroAmount > nitroDuration)
					nitroAmount = nitroDuration;
			}
			else {
				nitroAmount -= nitroSpendRate * Time.deltaTime;
				if (nitroAmount < 0) { 
					nitroAmount = 0;
					NitroOff();
				}
			}
		}
	}

	private void GearChanging()
	{
		float f = Mathf.Abs(CurrentSpeed/MaxSpeed);
		float upgearlimit = (1/(float) NoOfGears)*(m_GearNum + 1);
		float downgearlimit = (1/(float) NoOfGears)*m_GearNum;

		if (m_GearNum > 0 && f < downgearlimit)
		{
			m_GearNum--;
		}

		if (f > upgearlimit && (m_GearNum < (NoOfGears - 1)))
		{
			m_GearNum++;
		}
		if(gearText){
			if(BrakeInput > 0f && reversing){
				gearText.text = "R";
			}else{
				if(m_GearNum == 0)
					gearText.text = "N";
			}
			if(AccelInput > 0f){
				gearText.text = (m_GearNum + 1f).ToString();
			}
		}
	}


	// simple function to add a curved bias towards 1 for a value in the 0-1 range
	private static float CurveFactor(float factor)
	{
		return 1 - (1 - factor)*(1 - factor);
	}


	// unclamped version of Lerp, to allow value to exceed the from-to range
	private static float ULerp(float from, float to, float value)
	{
		return (1.0f - value)*from + value*to;
	}


	private void CalculateGearFactor()
	{
		float f = (1/(float) NoOfGears);
		// gear factor is a normalised representation of the current speed within the current gear's range of speeds.
		// We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
		var targetGearFactor = Mathf.InverseLerp(f*m_GearNum, f*(m_GearNum + 1), Mathf.Abs(CurrentSpeed/MaxSpeed));
		m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime*5f);
		if (RPMSlider) {
			if(m_SpeedType == SpeedType.MPH)
				RPMSlider.value = m_GearFactor;
			if(m_SpeedType == SpeedType.KPH && m_GearNum != 3)
				RPMSlider.value = m_GearFactor;
			if(m_SpeedType == SpeedType.KPH && m_GearNum == 3)
				RPMSlider.value = 0.9f - m_GearFactor;
		}
	}


	private void CalculateRevs()
	{
		// calculate engine revs (for display / sound)
		// (this is done in retrospect - revs are not used in force/power calculations)
		CalculateGearFactor();
		var gearNumFactor = m_GearNum/(float) NoOfGears;
		var revsRangeMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearNumFactor));
		var revsRangeMax = ULerp(m_RevRangeBoundary, 1f, gearNumFactor);
		Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
	}


	public void Move(float steering, float accel, float footbrake, float handbrake)
	{
		for (int i = 0; i < 4; i++)
		{
			Quaternion quat;
			Vector3 position;
			m_WheelColliders[i].GetWorldPose(out position, out quat);
			m_WheelMeshes[i].transform.position = position;
			m_WheelMeshes[i].transform.rotation = quat;
		}

		//clamp input values
		steering = Mathf.Clamp(steering, -1, 1);
		AccelInput = accel = Mathf.Clamp(accel, 0, 1);
		BrakeInput = footbrake = -1*Mathf.Clamp(footbrake, -1, 0);
		handbrake = Mathf.Clamp(handbrake, 0, 1);

		//Set the steer on the front wheels.
		//Assuming that wheels 0 and 1 are the front wheels.
		float speedFactor = m_SteerSensitivity * CurrentSpeed * 1.609344f / MaxSpeed;

		m_SteerAngle = Mathf.Lerp(m_MaximumSteerAngle,m_SteerAngleAtMaxSpeed, speedFactor);
		m_SteerAngle *= steering;
		m_WheelColliders[0].steerAngle = m_SteerAngle;
		m_WheelColliders[1].steerAngle = m_SteerAngle;

		if(overrideBrake){
			footbrake = overrideBrakePower;
		}
		if(overrideAcceleration){
			accel = overrideAccelerationPower;
			ApplyDrive(accel, footbrake);
			return;
		}

		SteerHelper();
		ApplyDrive(accel, footbrake);
		CapSpeed();

		//Set the handbrake.
		//Assuming that wheels 2 and 3 are the rear wheels.
		if (handbrake > 0f)
		{
			var hbTorque = handbrake*m_MaxHandbrakeTorque;
			m_WheelColliders[2].brakeTorque = hbTorque;
			m_WheelColliders[3].brakeTorque = hbTorque;
		}


		CalculateRevs();
		GearChanging();

		AddDownForce();
		CheckForWheelSpin();
		TractionControl();
	}


	private void CapSpeed()
	{
		speed = m_Rigidbody.velocity.magnitude;





		switch (m_SpeedType)
		{
		case SpeedType.MPH:

			speed *= 2.23693629f;
			if(speedText){
				speedText.text = speed.ToString("F0");
			}
			if (speed > m_Topspeed)
				m_Rigidbody.velocity = (m_Topspeed/2.23693629f) * m_Rigidbody.velocity.normalized;
			break;

		case SpeedType.KPH:
			speed *= 3.6f;
			if(speedText){
				speedText.text = speed.ToString("F0");
			}
			if (speed > m_Topspeed)
				m_Rigidbody.velocity = (m_Topspeed/3.6f) * m_Rigidbody.velocity.normalized;
			break;
		}
	}


	private void ApplyDrive(float accel, float footbrake)
	{

		float thrustTorque;
		switch (m_CarDriveType)
		{
		case CarDriveType.FourWheelDrive:
			thrustTorque = accel * (m_CurrentTorque / 4f);
			for (int i = 0; i < 4; i++)
			{
				m_WheelColliders[i].motorTorque = thrustTorque;
			}
			break;

		case CarDriveType.FrontWheelDrive:
			thrustTorque = accel * (m_CurrentTorque / 2f);
			m_WheelColliders[0].motorTorque = m_WheelColliders[1].motorTorque = thrustTorque;
			break;

		case CarDriveType.RearWheelDrive:
			thrustTorque = accel * (m_CurrentTorque / 2f);
			m_WheelColliders[2].motorTorque = m_WheelColliders[3].motorTorque = thrustTorque;
			break;

		}

		if(overrideBrake){
			footbrake = overrideBrakePower;
		}
		if(overrideAcceleration){
			accel = overrideAccelerationPower;
		}

		for (int i = 0; i < 4; i++)
		{
			if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, m_Rigidbody.velocity) < 50f)
			{
				reversing = false;
				m_WheelColliders[i].brakeTorque = m_BrakeTorque*footbrake;
			}
			else if (footbrake > 0)
			{
				reversing = true;
				m_WheelColliders[i].brakeTorque = 0f;
				m_WheelColliders[i].motorTorque = -m_ReverseTorque*footbrake;
			}
		}
	}


	private void SteerHelper()
	{
		for (int i = 0; i < 4; i++)
		{
			WheelHit wheelhit;
			m_WheelColliders[i].GetGroundHit(out wheelhit);
			if (wheelhit.normal == Vector3.zero)
				return; // wheels arent on the ground so dont realign the rigidbody velocity
		}

		// this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
		if (Mathf.Abs(m_OldRotation - transform.eulerAngles.y) < 10f)
		{
			var turnadjust = (transform.eulerAngles.y - m_OldRotation) * m_SteerHelper;
			Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
			m_Rigidbody.velocity = velRotation * m_Rigidbody.velocity;
		}
		m_OldRotation = transform.eulerAngles.y;
	}


	// this is used to add more grip in relation to speed
	private void AddDownForce()
	{
		//m_WheelColliders[0].attachedRigidbody.AddForce(-transform.up*m_Downforce*
		//	m_WheelColliders[0].attachedRigidbody.velocity.magnitude);
	}


	// checks if the wheels are spinning and is so does three things
	// 1) emits particles
	// 2) plays tiure skidding sounds
	// 3) leaves skidmarks on the ground
	// these effects are controlled through the WheelEffects class
	private void CheckForWheelSpin()
	{
		// loop through all wheels
		for (int i = 0; i < 4; i++)
		{
			WheelHit wheelHit;
			m_WheelColliders[i].GetGroundHit(out wheelHit);

			// is the tire slipping above the given threshhold
			if (Mathf.Abs(wheelHit.forwardSlip) >= m_SlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= m_SlipLimit)
			{
				m_WheelEffects[i].EmitTyreSmoke();

				// avoiding all four tires screeching at the same time
				// if they do it can lead to some strange audio artefacts
				if (!AnySkidSoundPlaying())
				{
					if(m_WheelEffects[i].enabled)
						m_WheelEffects[i].PlayAudio();
				}
				continue;
			}

			// if it wasnt slipping stop all the audio
			if (m_WheelEffects[i].PlayingAudio)
			{
				m_WheelEffects[i].StopAudio();
			}
			// end the trail generation
			m_WheelEffects[i].EndSkidTrail();
		}
	}

	// crude traction control that reduces the power to wheel if the car is wheel spinning too much
	private void TractionControl()
	{
		WheelHit wheelHit;
		switch (m_CarDriveType)
		{
		case CarDriveType.FourWheelDrive:
			// loop through all wheels
			for (int i = 0; i < 4; i++)
			{
				m_WheelColliders[i].GetGroundHit(out wheelHit);

				AdjustTorque(wheelHit.forwardSlip);
			}
			break;

		case CarDriveType.RearWheelDrive:
			m_WheelColliders[2].GetGroundHit(out wheelHit);
			AdjustTorque(wheelHit.forwardSlip);

			m_WheelColliders[3].GetGroundHit(out wheelHit);
			AdjustTorque(wheelHit.forwardSlip);
			break;

		case CarDriveType.FrontWheelDrive:
			m_WheelColliders[0].GetGroundHit(out wheelHit);
			AdjustTorque(wheelHit.forwardSlip);

			m_WheelColliders[1].GetGroundHit(out wheelHit);
			AdjustTorque(wheelHit.forwardSlip);
			break;
		}
	}


	private void AdjustTorque(float forwardSlip)
	{
		if (forwardSlip >= m_SlipLimit && m_CurrentTorque >= 0)
		{
			m_CurrentTorque -= 10 * m_TractionControl;
		}
		else
		{
			m_CurrentTorque += 10 * m_TractionControl;
			if (m_CurrentTorque > m_FullTorqueOverAllWheels)
			{
				m_CurrentTorque = m_FullTorqueOverAllWheels;
			}
		}
	}


	private bool AnySkidSoundPlaying()
	{
		for (int i = 0; i < 4; i++)
		{
			if (m_WheelEffects[i].PlayingAudio)
			{
				return true;
			}
		}
		return false;
	}
}