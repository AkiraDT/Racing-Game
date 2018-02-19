using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class RG_CarCamera : MonoBehaviour {

	public enum CameraType{ CarCamera, HelmetCamera}

	private RG_CarController carController;
	public InputData inputData;
	public bool mobile;
	public bool findCameraButton;
	public EventTrigger.Entry switchCameraEvent;
	public CameraType cameraType;
	public Camera carCamera;
	public Camera helmetCamera;
    public Transform car;
	public GameObject[] interiorCameraComponents;
	public float cameraFollowSpeed;
	//public float cameraShakeMultiplier;
    public float distance;
    public float height;
    public float rotationDamping;
    public float heightDamping;
    public float zoomRatio;
    public float DefaultFOV;
    private Vector3 rotationVector;
    private Vector3 position;
    private Rigidbody carBody;
	[Header("Camera Pivot")]
	public float max;
	public float value;
	public float minPivotMoveSpeed;
	public float maxPivotMoveSpeed;
	public float pivotMoveSpeed;
	public Vector3 targetPivotPosition;
	private string gameMode;
	public Transform lobbyPlayer;
	public RG_SyncData syncVars;

	public bool cameraShake;
	[Range(0,1.0f)]public float shakeStartSpeedFactor;
	private float speedFactor;
	public float shakeAmount = 1.0f;
	Vector3 camStartPos;
	public UnityStandardAssets.ImageEffects.CameraMotionBlur motionBlur;
	public float normalBlurFactor;
	public float nitroBlurFactor;
	public float normalVelocityMax;
	public float nitroVelocityMax;
	public float normalZoomRation;
	public float nitroZoomRatio;
	public float normalHeight;
	public float nitroHeightMultiplier;
	public Material glassMaterial;
	public float interiorGlassAlpha;
	public float defaultGlassAlpha;

	void Start(){
		carController = transform.root.GetComponent<RG_CarController>();
		if(GameObject.Find("Car Input") != null){
			mobile = true;
			findCameraButton = true;
		}
		if(interiorCameraComponents.Length > 0)
			interiorCameraComponents [2] = GameObject.Find ("Player Arrow");
		if(findCameraButton){
			EventTrigger findEvent = GameObject.Find ("Camera Switch Button").GetComponent<EventTrigger>();
			if (findEvent != null) {
				findEvent.triggers.Add (switchCameraEvent);
				findEvent = null;
			}
		}
		if (!car) {
			car = transform.parent.transform;
			carBody = car.GetComponent<Rigidbody> ();
		}
		camStartPos = carCamera.transform.localPosition;
	}

	void Update(){
		if (car != null) {
			if (cameraType == CameraType.CarCamera) {				
				transform.parent = null;
				carCamera.gameObject.SetActive (true);
				for (int i = 0; i < interiorCameraComponents.Length; i++) {
					if(interiorCameraComponents [i] != null)
						interiorCameraComponents [i].SetActive (false);
				}
				if (interiorCameraComponents.Length > 0 && interiorCameraComponents [2] != null)
					interiorCameraComponents [2].SetActive (true);
				helmetCamera.gameObject.SetActive (false);
			} else if (cameraType == CameraType.HelmetCamera) {	
				helmetCamera.gameObject.SetActive (true);
				if (car != null) {
					transform.parent = car;
				}

				for (int i = 0; i < interiorCameraComponents.Length; i++) {
					if(interiorCameraComponents [i] != null)
						interiorCameraComponents [i].SetActive (true);
				}
				if (interiorCameraComponents.Length > 0 && interiorCameraComponents [2] != null)
					interiorCameraComponents [2].SetActive (false);
				carCamera.gameObject.SetActive (false);
			}
			if (Input.GetKeyDown (inputData.cameraSwitchKey) || Input.GetKeyDown (inputData.cameraSwitchJoystick)) {
				CycleCamera ();
			}
			if (Input.GetKeyDown (inputData.nitroKey) || Input.GetKeyDown (inputData.nitroJoystick)) {
				carController.NitroOn ();
			}
			if(Input.GetKeyUp (inputData.nitroKey) || Input.GetKeyUp (inputData.nitroJoystick)){
				carController.NitroOff ();
			}
		}
	}

    void LateUpdate() {
		if (car != null) {
			if (cameraType == CameraType.CarCamera) {				
				var wantedAngle = rotationVector.y;
				var wantedHeight = car.position.y + height;
				var myAngle = transform.eulerAngles.y;
				var myHeight = transform.position.y;
				myAngle = Mathf.LerpAngle (myAngle, wantedAngle, rotationDamping * Time.deltaTime);
				myHeight = Mathf.Lerp (myHeight, wantedHeight, heightDamping * Time.deltaTime);
				var currentRotation = Quaternion.Euler (0, myAngle, 0);
				transform.position = car.position;
				transform.position -= currentRotation * Vector3.forward * distance;
				position = transform.position;
				position.y = myHeight;
				//add shake based on speed
				//	float tempShake = Random.Range (1.0f,2.0f);
				//	float tempRPM;
				//	tempRPM = syncVars.wheelRPM;
				//	tempShake = tempShake * cameraShakeMultiplier * (tempRPM / 25);
				//	tempShake = Random.Range (-tempShake, tempShake);
				//	position.x += tempShake;
				//	position.y += tempShake * Random.Range (0f, 1.5f);
					//Debug.Log (tempShake);
				//	temp2 = new Vector3 (defaultSteering.x, defaultSteering.y, -(horizontalInput * (rotationLimit )));
				//	float zAngle = Mathf.SmoothDampAngle (steeringWheel.localEulerAngles.z, temp2.z - tempShake, ref yVelocity, steeringRotationSpeed );
				//	steeringWheel.localEulerAngles = new Vector3 (defaultSteering.x, defaultSteering.y, zAngle );

				transform.position = Vector3.Lerp (transform.position, position, cameraFollowSpeed);
				//transform.position = position;
				transform.LookAt (car);
				//carCamera.transform.LookAt (car);
				//Vector3.Lerp (cameraComponent.transform.position, targetPivotPosition, pivotMoveSpeed * Time.deltaTime);
				if (Input.GetAxis ("Horizontal") > 0) {
					value = -max;
					pivotMoveSpeed = minPivotMoveSpeed;
				} else if (Input.GetAxis ("Horizontal") < 0) {
					value = max;
					pivotMoveSpeed = minPivotMoveSpeed;
				} else {
					value = 0;
					pivotMoveSpeed = Mathf.Lerp (pivotMoveSpeed, maxPivotMoveSpeed, 1 * Time.deltaTime);
				}
				targetPivotPosition.x = Mathf.Lerp (targetPivotPosition.x, value, pivotMoveSpeed * Time.deltaTime);
				carCamera.transform.localPosition = targetPivotPosition;

				speedFactor = carController.speed / carController.m_Topspeed;


				if (carController.nitroOn) {
					speedFactor *= 2.0f;
					motionBlur.velocityScale = Mathf.Lerp (motionBlur.velocityScale, nitroBlurFactor, Time.deltaTime);
					motionBlur.maxVelocity = Mathf.Lerp(motionBlur.maxVelocity, nitroVelocityMax, Time.deltaTime);
					zoomRatio = Mathf.Lerp(zoomRatio, nitroZoomRatio, Time.deltaTime);
					height = Mathf.Lerp(height, normalHeight + (nitroHeightMultiplier * speedFactor), Time.deltaTime);
				} else {
					motionBlur.velocityScale = Mathf.Lerp (motionBlur.velocityScale, normalBlurFactor, Time.deltaTime);
					motionBlur.maxVelocity = Mathf.Lerp(motionBlur.maxVelocity, normalVelocityMax, Time.deltaTime);
					zoomRatio = Mathf.Lerp(zoomRatio, normalZoomRation, Time.deltaTime);
					height = Mathf.Lerp(height, normalHeight, Time.deltaTime);
				}if (cameraShake &&  speedFactor > shakeStartSpeedFactor) {
					carCamera.transform.localPosition = camStartPos + Random.insideUnitSphere * (shakeAmount * speedFactor);
				}else{
					carCamera.transform.localPosition = camStartPos;
				}
				
				//shakeDuration _ = Time.deltaTime * decreaseFactor;

			} else if (cameraType == CameraType.HelmetCamera) {
			
			}
		}
    }

	public void CycleCamera(){
		if (cameraType == CameraType.CarCamera) {
			cameraType = CameraType.HelmetCamera;
			if (glassMaterial != null) {
				Color col = glassMaterial.color;
				col.a = interiorGlassAlpha;
				glassMaterial.color = col;
			}
		}else if (cameraType == CameraType.HelmetCamera){
			cameraType = CameraType.CarCamera;
			if (glassMaterial != null) {
				Color col = glassMaterial.color;
				col.a = defaultGlassAlpha;
				glassMaterial.color = col;
			}
		}
	}
		
    void FixedUpdate(){
		
		if (cameraType == CameraType.CarCamera) {
			if (car && !mobile) {
				var localVelocity = car.InverseTransformDirection (carBody.velocity);
				if (localVelocity.z < -0.5f && Input.GetAxis ("Vertical") == -1) {
					rotationVector.y = car.eulerAngles.y + 180f;
				} else {
					rotationVector.y = car.eulerAngles.y;
				}
				var acc = carBody.velocity.magnitude;
				carCamera.fieldOfView = DefaultFOV + acc * zoomRatio * Time.deltaTime;
				//cameraComponent.transform.rotation = pivotRotation;
			}else if (car && mobile){
				
				var localVelocity = car.InverseTransformDirection (carBody.velocity);
				if (localVelocity.z < -0.5f && TurnTheGameOn.RacingGameTemplate.CrossPlatformInput.PlatformSpecific.RG_CrossPlatformInputManager.GetAxis ("Vertical") == -1) {
					rotationVector.y = car.eulerAngles.y + 180f;
				} else {
					rotationVector.y = car.eulerAngles.y;
				}
				var acc = carBody.velocity.magnitude;
				carCamera.fieldOfView = DefaultFOV + acc * zoomRatio * Time.deltaTime;
			}
		}
		else if (cameraType == CameraType.HelmetCamera){
		
		}
    }
}


