using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[System.Serializable]
[ExecuteInEditMode]
[RequireComponent(typeof (RG_CarController))]
public class RG_AIHelper : MonoBehaviour {

	public enum ProgressStyle { SmoothAlongRoute, PointToPoint,	}
	public enum BrakeCondition	{
		NeverBrake,                 // the car simply accelerates at full throttle all the time.
		TargetDirectionDifference,  // the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.
		TargetDistance,             // the car will brake as it approaches its target, regardless of the target's direction. Useful if you want the car to head for a stationary target and come to rest when it arrives there.
	}


	//References
	[SerializeField] private RG_WaypointCircuit circuit;	// A route of waypoints used for pathfinding
	public Transform target;														// Current waypoint to drive toward
	public RG_AIDetection detection;												// AI sensor system used for obstacle detection and avoidance
	private RG_SceneManager sceneManager;
	private RG_CarController carController;
	private Rigidbody m_Rigidbody;

	[SerializeField] private ProgressStyle progressStyle = ProgressStyle.SmoothAlongRoute;		// whether to update the position smoothly along the route (good for curved paths) or just when we reach each waypoint.
	[SerializeField] private BrakeCondition m_BrakeCondition = BrakeCondition.TargetDistance;	// what should the AI consider when accelerating/braking?


	[Range(0,1)] public float maxSpeedFactor = 1.0f;
	[SerializeField] [Range(0, 1)] public float cautiousSpeedFactor = 0.05f;               // percentage of max speed to use when being maximally cautious


	// This script provides input to the car controller in the same way that the user control script does.
	// As such, it is really 'driving' the car, with no special physics or animation tricks to make the car behave properly.
	// "wandering" is used to give the cars a more human, less robotic feel. They can waver slightly
	// in speed and direction while driving towards their target.
	[SerializeField] [Range(0, 180)] private float m_CautiousMaxAngle = 50f;                  // angle of approaching corner to treat as warranting maximum caution
	[SerializeField] private float m_CautiousMaxDistance = 100f;                              // distance at which distance-based cautiousness begins
	[SerializeField] private float m_CautiousAngularVelocityFactor = 30f;                     // how cautious the AI should be when considering its own current angular velocity (i.e. easing off acceleration if spinning!)
	[SerializeField] private float m_SteerSensitivity = 0.05f;                                // how sensitively the AI uses steering input to turn to the desired direction
	[SerializeField] private float m_AccelSensitivity = 0.04f;                                // How sensitively the AI uses the accelerator to reach the current desired speed
	[SerializeField] private float m_BrakeSensitivity = 1f;                                   // How sensitively the AI uses the brake to reach the current desired speed
	[SerializeField] [Range(0, 1)] private float m_AccelWanderAmount = 0.1f;                  // how much the cars acceleration will wander
	[SerializeField] private float m_AccelWanderSpeed = 0.1f;                                 // how fast the cars acceleration wandering will fluctuate
	[SerializeField] private bool m_Driving;                                                  // whether the AI is currently actively driving or stopped.
	[SerializeField] private bool m_StopWhenTargetReached;                                    // should we stop driving when we reach the target?
	[SerializeField] private float m_ReachTargetThreshold = 2;                                // proximity to target to consider we 'reached' it, and stop driving.
	private float m_RandomPerlin;             // A random value for the car to base its wander on (so that AI cars don't all wander in the same pattern)


	public bool isReversing;
	public float rotationSpeed;
	[Range(20.0f, 120.0f)]public float resetAfter;
	public float respawnYOffset;
	[HideInInspector] public int opponentNumber;
	[HideInInspector] public float timer;
	[HideInInspector] public float stuckTimer;
	public float stuckReset;
	private Vector3 tempPosition;
	public float stuckSpeedFactor = 0.05f;
	// WaypointProgressTracker
	[SerializeField] private float lookAheadForTargetOffset = 5;
	// The offset ahead along the route that the we will aim for
	[SerializeField] private float lookAheadForTargetFactor = .1f;
	// A multiplier adding distance ahead along the route to aim for, based on current speed
	[SerializeField] private float lookAheadForSpeedOffset = 10;
	// The offset ahead only the route for speed adjustments (applied as the rotation of the waypoint target transform)
	[SerializeField] private float lookAheadForSpeedFactor = .2f;
	// A multiplier adding distance ahead along the route for speed adjustments
	[SerializeField] private float pointToPointThreshold = 4;
	// proximity to waypoint which must be reached to switch target to next waypoint : only used in PointToPoint mode.
	// these are public, readable by other objects - i.e. for an AI to know where to head!
	public RG_WaypointCircuit.RoutePoint targetPoint { get; private set; }
	public RG_WaypointCircuit.RoutePoint speedPoint { get; private set; }
	public RG_WaypointCircuit.RoutePoint progressPoint { get; private set; }
	private float progressDistance; // The progress round the route, used in smooth mode.
	private int progressNum; // the current waypoint number, used in point-to-point mode.
	private Vector3 lastPosition; // Used to calculate current speed (since we may not have a rigidbody component)
	private float speed; // current speed of this object (calculated from delta since last frame)

	private void Awake(){
		carController = GetComponent<RG_CarController>();
		// give the random perlin a random value
		m_RandomPerlin = Random.value*1;
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Rigidbody.AddForce (Vector3.forward * 50000, ForceMode.Impulse );
	}

	void Start(){
		sceneManager = GameObject.Find ("Race Scene Manager").GetComponent<RG_SceneManager>();
		circuit = GameObject.Find("Race Scene Manager").GetComponent<RG_WaypointCircuit>();
		if (target == null)
			target = new GameObject(name + " Waypoint Target").transform;
		progressDistance = 0;
		progressNum = 0;
		if (progressStyle == ProgressStyle.PointToPoint){
			target.position = circuit.Waypoints[progressNum].position;
			target.rotation = circuit.Waypoints[progressNum].rotation;
		}
	}

	void Update () {
		if (progressStyle == ProgressStyle.SmoothAlongRoute){
			// determine the position we should currently be aiming for
			// (this is different to the current progress position, it is a a certain amount ahead along the route)
			// we use lerp as a simple way of smoothing out the speed over time.
			if (Time.deltaTime > 0)	{
				speed = Mathf.Lerp(speed, (lastPosition - transform.position).magnitude/Time.deltaTime,
					Time.deltaTime);
			}
			target.position = circuit.GetRoutePoint(progressDistance + lookAheadForTargetOffset + lookAheadForTargetFactor*speed).position;
			target.rotation = Quaternion.LookRotation(circuit.GetRoutePoint(progressDistance + lookAheadForSpeedOffset + lookAheadForSpeedFactor*speed).direction);
			// get our current progress along the route
			progressPoint = circuit.GetRoutePoint(progressDistance);
			Vector3 progressDelta = progressPoint.position - transform.position;
			if (Vector3.Dot(progressDelta, progressPoint.direction) < 0){
				progressDistance += progressDelta.magnitude*0.5f;
			}
			lastPosition = transform.position;
		}
		else{
			// point to point mode. Just increase the waypoint if we're close enough:
			Vector3 targetDelta = target.position - transform.position;
			if (targetDelta.magnitude < pointToPointThreshold)	{
				progressNum = (progressNum + 1)%circuit.Waypoints.Length;
			}
			target.position = circuit.Waypoints[progressNum].position;
			target.rotation = circuit.Waypoints[progressNum].rotation;
			// get our current progress along the route
			progressPoint = circuit.GetRoutePoint(progressDistance);
			Vector3 progressDelta = progressPoint.position - transform.position;
			if (Vector3.Dot(progressDelta, progressPoint.direction) < 0){
				progressDistance += progressDelta.magnitude;
			}
			lastPosition = transform.position;
		}
		//Stuck timer
		timer += Time.deltaTime * 1;
		if (timer >= resetAfter) {
			timer = 0;
			transform.position = target.position;
			transform.rotation = target.rotation;
		}
		if (stuckTimer >= stuckReset) {
			stuckTimer = 0;
			transform.position = target.position;
			transform.rotation = target.rotation;
		}
		if (carController.speed < 1.0f && sceneManager.countUp && sceneManager.raceTime > 0.5f) {
			if (detection.sensorHits.forward1Hit || detection.sensorHits.forward2Hit || detection.sensorHits.forward3Hit) {
				if (!detection.sensorHits.right2Hit) {
					//Rotate right
					transform.Rotate (0, Time.deltaTime * (-rotationSpeed * 20), 0, Space.World);
					cautiousSpeedFactor = stuckSpeedFactor;
				}else if (!detection.sensorHits.left2Hit) {
					//Rotate left
					transform.Rotate (0, Time.deltaTime * (rotationSpeed * 20), 0, Space.World);
					cautiousSpeedFactor = stuckSpeedFactor;
				}else{
					transform.position = target.position;
					transform.rotation = target.rotation;
				}
			}
		}

		if (sceneManager.countUp) {
			//Check if we need to turn right
			if (detection.sensorHits.forward1Hit || detection.sensorHits.forwardLeft1Hit || detection.sensorHits.forwardLeft2Hit || detection.sensorHits.forwardLeft3Hit) {

				if (!detection.sensorHits.forwardRight1Hit && !detection.sensorHits.forwardRight2Hit && !detection.sensorHits.forwardRight3Hit) { 
					transform.Rotate (0, Time.deltaTime * (rotationSpeed * 20), 0, Space.World);
				}
				cautiousSpeedFactor = 0.1f;
			}

			//Check if we need to turn left
			if (detection.sensorHits.forward3Hit || detection.sensorHits.forwardRight1Hit || detection.sensorHits.forwardRight2Hit || detection.sensorHits.forwardRight3Hit) {
				if (!detection.sensorHits.forwardLeft1Hit && !detection.sensorHits.forwardLeft2Hit && !detection.sensorHits.forwardLeft3Hit) {
					transform.Rotate (0, Time.deltaTime * (-rotationSpeed * 20), 0, Space.World);
				}
				cautiousSpeedFactor = 0.1f;
			}
		}
		//check if we detect a vehicle in front of us, if not go faster
		if (!detection.sensorHits.forward2Hit) {
			cautiousSpeedFactor = maxSpeedFactor;
		}
		if (detection.sensorHits.forward1Hit || detection.sensorHits.forward2Hit || detection.sensorHits.forward3Hit) {
			carController.overrideBrake = true;
			carController.overrideAcceleration = true;
			carController.overrideBrakePower = 1;
			carController.overrideAccelerationPower = 0;
		}else {
			carController.overrideBrake = false;
			carController.overrideAcceleration = false;
			carController.overrideBrakePower = 0;
			carController.overrideAccelerationPower = 1;
			cautiousSpeedFactor = maxSpeedFactor;
		}
		if (sceneManager.countUp && sceneManager.racerInfo [opponentNumber].nextWP != 0)
			transform.Rotate (0, Time.deltaTime * (-rotationSpeed), 0, Space.World);

		if (carController.speed < 1.0f && sceneManager.countUp && sceneManager.racerInfo [opponentNumber].nextWP != 0) {
			stuckTimer += Time.deltaTime * 1;
		} else {
			stuckTimer = 0;
		}
	}

	private void FixedUpdate() {
		if (target == null || !m_Driving) {
			// Car should not be moving,
			// use handbrake to stop
			carController.Move(0, 0, -1f, 1f);
		}
		else {
			Vector3 fwd = transform.forward;
			if (m_Rigidbody.velocity.magnitude > carController.MaxSpeed*0.1f) {
				fwd = m_Rigidbody.velocity;
			}
			float desiredSpeed = carController.MaxSpeed;
			// now it's time to decide if we should be slowing down...
			switch (m_BrakeCondition)                {
			case BrakeCondition.TargetDirectionDifference:
				{
					// the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.
					// check out the angle of our target compared to the current direction of the car
					float approachingCornerAngle = Vector3.Angle(target.forward, fwd);
					// also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
					float spinningAngle = m_Rigidbody.angularVelocity.magnitude*m_CautiousAngularVelocityFactor;
					// if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
					float cautiousnessRequired = Mathf.InverseLerp(0, m_CautiousMaxAngle,
						Mathf.Max(spinningAngle,
							approachingCornerAngle));
					desiredSpeed = Mathf.Lerp(carController.MaxSpeed, carController.MaxSpeed*cautiousSpeedFactor,
						cautiousnessRequired);
					break;
				}
			case BrakeCondition.TargetDistance:
				{
					// the car will brake as it approaches its target, regardless of the target's direction. Useful if you want the car to
					// head for a stationary target and come to rest when it arrives there.
					// check out the distance to target
					Vector3 delta = target.position - transform.position;
					float distanceCautiousFactor = Mathf.InverseLerp(m_CautiousMaxDistance, 0, delta.magnitude);
					// also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
					float spinningAngle = m_Rigidbody.angularVelocity.magnitude*m_CautiousAngularVelocityFactor;
					// if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
					float cautiousnessRequired = Mathf.Max(
						Mathf.InverseLerp(0, m_CautiousMaxAngle, spinningAngle), distanceCautiousFactor);
					desiredSpeed = Mathf.Lerp(carController.MaxSpeed, carController.MaxSpeed*cautiousSpeedFactor,
						cautiousnessRequired);
					break;
				}
			case BrakeCondition.NeverBrake:
				break;
			}
			// use different sensitivity depending on whether accelerating or braking:
			float accelBrakeSensitivity = (desiredSpeed < carController.CurrentSpeed)	? m_BrakeSensitivity : m_AccelSensitivity;
			// decide the actual amount of accel/brake input to achieve desired speed.
			float accel = Mathf.Clamp((desiredSpeed - carController.CurrentSpeed)*accelBrakeSensitivity, -1, 1);
			// add acceleration 'wander', which also prevents AI from seeming too uniform and robotic in their driving i.e. increasing the accel wander amount can introduce jostling and bumps between AI cars in a race
			accel *= (1 - m_AccelWanderAmount) + (Mathf.PerlinNoise(Time.time*m_AccelWanderSpeed, m_RandomPerlin)*m_AccelWanderAmount);
			// calculate the local-relative position of the target, to steer towards
			Vector3 localTarget = transform.InverseTransformPoint(target.position);
			// work out the local angle towards the target
			float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z)*Mathf.Rad2Deg;
			// get the amount of steering needed to aim the car towards the target
			float steer = Mathf.Clamp(targetAngle*m_SteerSensitivity, -1, 1)*Mathf.Sign(carController.CurrentSpeed);
			// feed input to the car controller.
			if (isReversing) {
				carController.Move(steer, -accel, -accel, 0f);
			} else {
				carController.Move(steer, accel, accel, 0f);
			}
			// if appropriate, stop driving when we're close enough to the target.
			if (m_StopWhenTargetReached && localTarget.magnitude < m_ReachTargetThreshold){
				m_Driving = false;
			}
		}
	}

	public void SetTarget (Transform target) {
		m_Driving = true;
	}

	public void ResetTimer(){
		timer = 0;
	}

	private void OnDrawGizmos()	{
		if (Application.isPlaying && circuit != null){
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, target.position);
			Gizmos.DrawWireSphere(circuit.GetRoutePosition(progressDistance), 1);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(target.position, target.position + target.forward);
		}
	}
	
}
//Vector3 offsetTargetPos = target.position;
//offsetTargetPos += target.right*m_AvoidPathOffset;