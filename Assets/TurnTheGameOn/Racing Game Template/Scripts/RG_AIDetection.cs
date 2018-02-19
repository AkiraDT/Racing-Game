using UnityEngine;
using System.Collections;

[System.Serializable]
public class SensorHitTag{
	public string forward1HitTag;
	public string forward2HitTag;
	public string forward3HitTag;
	public string forwardRight1HitTag;
	public string forwardRight2HitTag;
	public string forwardRight3HitTag;
	public string forwardLeft1HitTag;
	public string forwardLeft2HitTag;
	public string forwardLeft3HitTag;
	public string right1HitTag;
	public string right2HitTag;
	public string right3HitTag;
	public string left1HitTag;
	public string left2HitTag;
	public string left3HitTag;
	public string rear1HitTag;
	public string rear2HitTag;
	public string rear3HitTag;
	public string rearRight1HitTag;
	public string rearRight2HitTag;
	public string rearRight3HitTag;
	public string rearLeft1HitTag;
	public string rearLeft2HitTag;
	public string rearLeft3HitTag;
}

[System.Serializable]
public class Sensors{
	public Sensor forward1;
	public Sensor forward2;
	public Sensor forward3;
	public Sensor forwardRight1;
	public Sensor forwardRight2;
	public Sensor forwardRight3;
	public Sensor forwardLeft1;
	public Sensor forwardLeft2;
	public Sensor forwardLeft3;
	public Sensor right1;
	public Sensor right2;
	public Sensor right3;
	public Sensor left1;
	public Sensor left2;
	public Sensor left3;
	public Sensor rear1;
	public Sensor rear2;
	public Sensor rear3;
	public Sensor rearRight1;
	public Sensor rearRight2;
	public Sensor rearRight3;
	public Sensor rearLeft1;
	public Sensor rearLeft2;
	public Sensor rearLeft3;
}

[System.Serializable]
public class SensorHits{
	public bool forward1Hit;
	public bool forward2Hit;
	public bool forward3Hit;
	public bool forwardRight1Hit;
	public bool forwardRight2Hit;
	public bool forwardRight3Hit;
	public bool forwardLeft1Hit;
	public bool forwardLeft2Hit;
	public bool forwardLeft3Hit;
	public bool right1Hit;
	public bool right2Hit;
	public bool right3Hit;
	public bool left1Hit;
	public bool left2Hit;
	public bool left3Hit;
	public bool rear1Hit;
	public bool rear2Hit;
	public bool rear3Hit;
	public bool rearRight1Hit;
	public bool rearRight2Hit;
	public bool rearRight3Hit;
	public bool rearLeft1Hit;
	public bool rearLeft2Hit;
	public bool rearLeft3Hit;
}

[System.Serializable]
public class SensorHitLayer{
	public string forward1HitLayer;
	public string forward2HitLayer;
	public string forward3HitLayer;
	public string forwardRight1HitLayer;
	public string forwardRight2HitLayer;
	public string forwardRight3HitLayer;
	public string forwardLeft1HitLayer;
	public string forwardLeft2HitLayer;
	public string forwardLeft3HitLayer;
	public string right1HitLayer;
	public string right2HitLayer;
	public string right3HitLayer;
	public string left1HitLayer;
	public string left2HitLayer;
	public string left3HitLayer;
	public string rear1HitLayer;
	public string rear2HitLayer;
	public string rear3HitLayer;
	public string rearRight1HitLayer;
	public string rearRight2HitLayer;
	public string rearRight3HitLayer;
	public string rearLeft1HitLayer;
	public string rearLeft2HitLayer;
	public string rearLeft3HitLayer;
}

[System.Serializable]
public class SensorHitDistance{
	public float forward1HitDistance;
	public float forward2HitDistance;
	public float forward3HitDistance;
	public float forwardRight1HitDistance;
	public float forwardRight2HitDistance;
	public float forwardRight3HitDistance;
	public float forwardLeft1HitDistance;
	public float forwardLeft2HitDistance;
	public float forwardLeft3HitDistance;
	public float right1HitDistance;
	public float right2HitDistance;
	public float right3HitDistance;
	public float left1HitDistance;
	public float left2HitDistance;
	public float left3HitDistance;
	public float rear1HitDistance;
	public float rear2HitDistance;
	public float rear3HitDistance;
	public float rearRight1HitDistance;
	public float rearRight2HitDistance;
	public float rearRight3HitDistance;
	public float rearLeft1HitDistance;
	public float rearLeft2HitDistance;
	public float rearLeft3HitDistance;
}

[System.Serializable]
public class SensorLength{
	public float forward1;
	public float forward2;
	public float forward3;
	public float forwardRight1;
	public float forwardRight2;
	public float forwardRight3;
	public float forwardLeft1;
	public float forwardLeft2;
	public float forwardLeft3;
	public float right1;
	public float right2;
	public float right3;
	public float left1;
	public float left2;
	public float left3;
	public float rear1;
	public float rear2;
	public float rear3;
	public float rearRight1;
	public float rearRight2;
	public float rearRight3;
	public float rearLeft1;
	public float rearLeft2;
	public float rearLeft3;
}

[ExecuteInEditMode][System.Serializable]
public class RG_AIDetection : MonoBehaviour {
	
	public enum Switch{ off, on }

	public float updateInterval = 0.1f;

	public Switch enableSensors;
	private bool enabledSensors;
	public Switch showGizmos;
	[HideInInspector] public bool showGizmo;
	public Switch showDebugs;
	[HideInInspector] public bool showDebug;
	public LayerMask detectMask;
	public Sensors sensors;
	public SensorHits sensorHits;
	public SensorHitDistance sensorHitDistance;
	public SensorHitTag sensorHitTag;
	public SensorHitLayer sensorHitLayer;
	public SensorLength sensorLength;

	void OnEnable(){
		if (enableSensors == Switch.off) {
			enabledSensors = true;
		} else {
			enabledSensors = false;
		}
	}

	void Update(){
		if (enableSensors == Switch.off) {
			if (enabledSensors) {
				sensors.forward1.enabled = false;
				sensors.forward2.enabled = false;
				sensors.forward3.enabled = false;
				sensors.forwardRight1.enabled = false;
				sensors.forwardRight2.enabled = false;
				sensors.forwardRight3.enabled = false;
				sensors.forwardLeft1.enabled = false;
				sensors.forwardLeft2.enabled = false;
				sensors.forwardLeft3.enabled = false;
				sensors.right1.enabled = false;
				sensors.right2.enabled = false;
				sensors.right3.enabled = false;
				sensors.left1.enabled = false;
				sensors.left2.enabled = false;
				sensors.left3.enabled = false;
				sensors.rear1.enabled = false;
				sensors.rear2.enabled = false;
				sensors.rear3.enabled = false;
				sensors.rearRight1.enabled = false;
				sensors.rearRight2.enabled = false;
				sensors.rearRight3.enabled = false;
				sensors.rearLeft1.enabled = false;
				sensors.rearLeft2.enabled = false;
				sensors.rearLeft3.enabled = false;
				enabledSensors = false;
			}			
		} else {
			if(!enabledSensors){
				sensors.forward1.enabled = true;
				sensors.forward2.enabled = true;
				sensors.forward3.enabled = true;
				sensors.forwardRight1.enabled = true;
				sensors.forwardRight2.enabled = true;
				sensors.forwardRight3.enabled = true;
				sensors.forwardLeft1.enabled = true;
				sensors.forwardLeft2.enabled = true;
				sensors.forwardLeft3.enabled = true;
				sensors.right1.enabled = true;
				sensors.right2.enabled = true;
				sensors.right3.enabled = true;
				sensors.left1.enabled = true;
				sensors.left2.enabled = true;
				sensors.left3.enabled = true;
				sensors.rear1.enabled = true;
				sensors.rear2.enabled = true;
				sensors.rear3.enabled = true;
				sensors.rearRight1.enabled = true;
				sensors.rearRight2.enabled = true;
				sensors.rearRight3.enabled = true;
				sensors.rearLeft1.enabled = true;
				sensors.rearLeft2.enabled = true;
				sensors.rearLeft3.enabled = true;
				enabledSensors = true;
			}
		}
		sensors.forward1.updateInterval = updateInterval;
		sensors.forward2.updateInterval = updateInterval;
		sensors.forward3.updateInterval = updateInterval;
		sensors.forwardRight1.updateInterval = updateInterval;
		sensors.forwardRight2.updateInterval = updateInterval;
		sensors.forwardRight3.updateInterval = updateInterval;
		sensors.forwardLeft1.updateInterval = updateInterval;
		sensors.forwardLeft2.updateInterval = updateInterval;
		sensors.forwardLeft3.updateInterval = updateInterval;
		sensors.right1.updateInterval = updateInterval;
		sensors.right2.updateInterval = updateInterval;
		sensors.right3.updateInterval = updateInterval;
		sensors.left1.updateInterval = updateInterval;
		sensors.left2.updateInterval = updateInterval;
		sensors.left3.updateInterval = updateInterval;
		sensors.rear1.updateInterval = updateInterval;
		sensors.rear2.updateInterval = updateInterval;
		sensors.rear3.updateInterval = updateInterval;
		sensors.rearRight1.updateInterval = updateInterval;
		sensors.rearRight2.updateInterval = updateInterval;
		sensors.rearRight3.updateInterval = updateInterval;
		sensors.rearLeft1.updateInterval = updateInterval;
		sensors.rearLeft2.updateInterval = updateInterval;
		sensors.rearLeft3.updateInterval = updateInterval;

		sensors.forward1.showGizmos = showGizmo;
		sensors.forward2.showGizmos = showGizmo;
		sensors.forward3.showGizmos = showGizmo;
		sensors.forwardRight1.showGizmos = showGizmo;
		sensors.forwardRight2.showGizmos = showGizmo;
		sensors.forwardRight3.showGizmos = showGizmo;
		sensors.forwardLeft1.showGizmos = showGizmo;
		sensors.forwardLeft2.showGizmos = showGizmo;
		sensors.forwardLeft3.showGizmos = showGizmo;
		sensors.right1.showGizmos = showGizmo;
		sensors.right2.showGizmos = showGizmo;
		sensors.right3.showGizmos = showGizmo;
		sensors.left1.showGizmos = showGizmo;
		sensors.left2.showGizmos = showGizmo;
		sensors.left3.showGizmos = showGizmo;
		sensors.rear1.showGizmos = showGizmo;
		sensors.rear2.showGizmos = showGizmo;
		sensors.rear3.showGizmos = showGizmo;
		sensors.rearRight1.showGizmos = showGizmo;
		sensors.rearRight2.showGizmos = showGizmo;
		sensors.rearRight3.showGizmos = showGizmo;
		sensors.rearLeft1.showGizmos = showGizmo;
		sensors.rearLeft2.showGizmos = showGizmo;
		sensors.rearLeft3.showGizmos = showGizmo;

		sensors.forward1.showDebugs = showDebug;
		sensors.forward2.showDebugs = showDebug;
		sensors.forward3.showDebugs = showDebug;
		sensors.forwardRight1.showDebugs = showDebug;
		sensors.forwardRight2.showDebugs = showDebug;
		sensors.forwardRight3.showDebugs = showDebug;
		sensors.forwardLeft1.showDebugs = showDebug;
		sensors.forwardLeft2.showDebugs = showDebug;
		sensors.forwardLeft3.showDebugs = showDebug;
		sensors.right1.showDebugs = showDebug;
		sensors.right2.showDebugs = showDebug;
		sensors.right3.showDebugs = showDebug;
		sensors.left1.showDebugs = showDebug;
		sensors.left2.showDebugs = showDebug;
		sensors.left3.showDebugs = showDebug;
		sensors.rear1.showDebugs = showDebug;
		sensors.rear2.showDebugs = showDebug;
		sensors.rear3.showDebugs = showDebug;
		sensors.rearRight1.showDebugs = showDebug;
		sensors.rearRight2.showDebugs = showDebug;
		sensors.rearRight3.showDebugs = showDebug;
		sensors.rearLeft1.showDebugs = showDebug;
		sensors.rearLeft2.showDebugs = showDebug;
		sensors.rearLeft3.showDebugs = showDebug;

		sensors.forward1.detectMask = detectMask;
		sensors.forward2.detectMask = detectMask;
		sensors.forward3.detectMask = detectMask;
		sensors.forwardRight1.detectMask = detectMask;
		sensors.forwardRight2.detectMask = detectMask;
		sensors.forwardRight3.detectMask = detectMask;
		sensors.forwardLeft1.detectMask = detectMask;
		sensors.forwardLeft2.detectMask = detectMask;
		sensors.forwardLeft3.detectMask = detectMask;
		sensors.right1.detectMask = detectMask;
		sensors.right2.detectMask = detectMask;
		sensors.right3.detectMask = detectMask;
		sensors.left1.detectMask = detectMask;
		sensors.left2.detectMask = detectMask;
		sensors.left3.detectMask = detectMask;
		sensors.rear1.detectMask = detectMask;
		sensors.rear2.detectMask = detectMask;
		sensors.rear3.detectMask = detectMask;
		sensors.rearRight1.detectMask = detectMask;
		sensors.rearRight2.detectMask = detectMask;
		sensors.rearRight3.detectMask = detectMask;
		sensors.rearLeft1.detectMask = detectMask;
		sensors.rearLeft2.detectMask = detectMask;
		sensors.rearLeft3.detectMask = detectMask;


		sensorHits.forward1Hit = sensors.forward1.sensorHit;
		sensorHits.forward2Hit = sensors.forward2.sensorHit;
		sensorHits.forward3Hit = sensors.forward3.sensorHit;
		sensorHits.forwardRight1Hit = sensors.forwardRight1.sensorHit;
		sensorHits.forwardRight2Hit = sensors.forwardRight2.sensorHit;
		sensorHits.forwardRight3Hit = sensors.forwardRight3.sensorHit;
		sensorHits.forwardLeft1Hit = sensors.forwardLeft1.sensorHit;
		sensorHits.forwardLeft2Hit = sensors.forwardLeft2.sensorHit;
		sensorHits.forwardLeft3Hit = sensors.forwardLeft3.sensorHit;
		sensorHits.right1Hit = sensors.right1.sensorHit;
		sensorHits.right2Hit = sensors.right2.sensorHit;
		sensorHits.right3Hit = sensors.right3.sensorHit;
		sensorHits.left1Hit = sensors.left1.sensorHit;
		sensorHits.left2Hit = sensors.left2.sensorHit;
		sensorHits.left3Hit = sensors.left3.sensorHit;
		sensorHits.rear1Hit = sensors.rear1.sensorHit;
		sensorHits.rear2Hit = sensors.rear2.sensorHit;
		sensorHits.rear3Hit = sensors.rear3.sensorHit;
		sensorHits.rearRight1Hit = sensors.rearRight1.sensorHit;
		sensorHits.rearRight2Hit = sensors.rearRight2.sensorHit;
		sensorHits.rearRight3Hit = sensors.rearRight3.sensorHit;
		sensorHits.rearLeft1Hit = sensors.rearLeft1.sensorHit;
		sensorHits.rearLeft2Hit = sensors.rearLeft2.sensorHit;
		sensorHits.rearLeft3Hit = sensors.rearLeft3.sensorHit;

		sensorHitDistance.forward1HitDistance = sensors.forward1.sensorHitDistance;
		sensorHitDistance.forward2HitDistance = sensors.forward2.sensorHitDistance;
		sensorHitDistance.forward3HitDistance = sensors.forward3.sensorHitDistance;
		sensorHitDistance.forwardRight1HitDistance = sensors.forwardRight1.sensorHitDistance;
		sensorHitDistance.forwardRight2HitDistance = sensors.forwardRight2.sensorHitDistance;
		sensorHitDistance.forwardRight3HitDistance = sensors.forwardRight3.sensorHitDistance;
		sensorHitDistance.forwardLeft1HitDistance = sensors.forwardLeft1.sensorHitDistance;
		sensorHitDistance.forwardLeft2HitDistance = sensors.forwardLeft2.sensorHitDistance;
		sensorHitDistance.forwardLeft3HitDistance = sensors.forwardLeft3.sensorHitDistance;
		sensorHitDistance.right1HitDistance = sensors.right1.sensorHitDistance;
		sensorHitDistance.right2HitDistance = sensors.right2.sensorHitDistance;
		sensorHitDistance.right3HitDistance = sensors.right3.sensorHitDistance;
		sensorHitDistance.left1HitDistance = sensors.left1.sensorHitDistance;
		sensorHitDistance.left2HitDistance = sensors.left2.sensorHitDistance;
		sensorHitDistance.left3HitDistance = sensors.left3.sensorHitDistance;
		sensorHitDistance.rear1HitDistance = sensors.rear1.sensorHitDistance;
		sensorHitDistance.rear2HitDistance = sensors.rear2.sensorHitDistance;
		sensorHitDistance.rear3HitDistance = sensors.rear3.sensorHitDistance;
		sensorHitDistance.rearRight1HitDistance = sensors.rearRight1.sensorHitDistance;
		sensorHitDistance.rearRight2HitDistance = sensors.rearRight2.sensorHitDistance;
		sensorHitDistance.rearRight3HitDistance = sensors.rearRight3.sensorHitDistance;
		sensorHitDistance.rearLeft1HitDistance = sensors.rearLeft1.sensorHitDistance;
		sensorHitDistance.rearLeft2HitDistance = sensors.rearLeft2.sensorHitDistance;
		sensorHitDistance.rearLeft3HitDistance = sensors.rearLeft3.sensorHitDistance;

		sensorHitTag.forward1HitTag = sensors.forward1.sensorHitTag;
		sensorHitTag.forward2HitTag = sensors.forward2.sensorHitTag;
		sensorHitTag.forward3HitTag = sensors.forward3.sensorHitTag;
		sensorHitTag.forwardRight1HitTag = sensors.forwardRight1.sensorHitTag;
		sensorHitTag.forwardRight2HitTag = sensors.forwardRight2.sensorHitTag;
		sensorHitTag.forwardRight3HitTag = sensors.forwardRight3.sensorHitTag;
		sensorHitTag.forwardLeft1HitTag = sensors.forwardLeft1.sensorHitTag;
		sensorHitTag.forwardLeft2HitTag = sensors.forwardLeft2.sensorHitTag;
		sensorHitTag.forwardLeft3HitTag = sensors.forwardLeft3.sensorHitTag;
		sensorHitTag.right1HitTag = sensors.right1.sensorHitTag;
		sensorHitTag.right2HitTag = sensors.right2.sensorHitTag;
		sensorHitTag.right3HitTag = sensors.right3.sensorHitTag;
		sensorHitTag.left1HitTag = sensors.left1.sensorHitTag;
		sensorHitTag.left2HitTag = sensors.left2.sensorHitTag;
		sensorHitTag.left3HitTag = sensors.left3.sensorHitTag;
		sensorHitTag.rear1HitTag = sensors.rear1.sensorHitTag;
		sensorHitTag.rear2HitTag = sensors.rear2.sensorHitTag;
		sensorHitTag.rear3HitTag = sensors.rear3.sensorHitTag;
		sensorHitTag.rearRight1HitTag = sensors.rearRight1.sensorHitTag;
		sensorHitTag.rearRight2HitTag = sensors.rearRight2.sensorHitTag;
		sensorHitTag.rearRight3HitTag = sensors.rearRight3.sensorHitTag;
		sensorHitTag.rearLeft1HitTag = sensors.rearLeft1.sensorHitTag;
		sensorHitTag.rearLeft2HitTag = sensors.rearLeft2.sensorHitTag;
		sensorHitTag.rearLeft3HitTag = sensors.rearLeft3.sensorHitTag;


		sensorHitLayer.forward1HitLayer = sensors.forward1.sensorHitLayer;
		sensorHitLayer.forward2HitLayer = sensors.forward2.sensorHitLayer;
		sensorHitLayer.forward3HitLayer = sensors.forward3.sensorHitLayer;
		sensorHitLayer.forwardRight1HitLayer = sensors.forwardRight1.sensorHitLayer;
		sensorHitLayer.forwardRight2HitLayer = sensors.forwardRight2.sensorHitLayer;
		sensorHitLayer.forwardRight3HitLayer = sensors.forwardRight3.sensorHitLayer;
		sensorHitLayer.forwardLeft1HitLayer = sensors.forwardLeft1.sensorHitLayer;
		sensorHitLayer.forwardLeft2HitLayer = sensors.forwardLeft2.sensorHitLayer;
		sensorHitLayer.forwardLeft3HitLayer = sensors.forwardLeft3.sensorHitLayer;
		sensorHitLayer.right1HitLayer = sensors.right1.sensorHitLayer;
		sensorHitLayer.right2HitLayer = sensors.right2.sensorHitLayer;
		sensorHitLayer.right3HitLayer = sensors.right3.sensorHitLayer;
		sensorHitLayer.left1HitLayer = sensors.left1.sensorHitLayer;
		sensorHitLayer.left2HitLayer = sensors.left2.sensorHitLayer;
		sensorHitLayer.left3HitLayer = sensors.left3.sensorHitLayer;
		sensorHitLayer.rear1HitLayer = sensors.rear1.sensorHitLayer;
		sensorHitLayer.rear2HitLayer = sensors.rear2.sensorHitLayer;
		sensorHitLayer.rear3HitLayer = sensors.rear3.sensorHitLayer;
		sensorHitLayer.rearRight1HitLayer = sensors.rearRight1.sensorHitLayer;
		sensorHitLayer.rearRight2HitLayer = sensors.rearRight2.sensorHitLayer;
		sensorHitLayer.rearRight3HitLayer = sensors.rearRight3.sensorHitLayer;
		sensorHitLayer.rearLeft1HitLayer = sensors.rearLeft1.sensorHitLayer;
		sensorHitLayer.rearLeft2HitLayer = sensors.rearLeft2.sensorHitLayer;
		sensorHitLayer.rearLeft3HitLayer = sensors.rearLeft3.sensorHitLayer;

		sensors.forward1.sensorLength = sensorLength.forward1;
		sensors.forward2.sensorLength = sensorLength.forward2;
		sensors.forward3.sensorLength = sensorLength.forward3;
		sensors.forwardRight1.sensorLength = sensorLength.forwardRight1;
		sensors.forwardRight2.sensorLength = sensorLength.forwardRight2;
		sensors.forwardRight3.sensorLength = sensorLength.forwardRight3;
		sensors.forwardLeft1.sensorLength = sensorLength.forwardLeft1;
		sensors.forwardLeft2.sensorLength = sensorLength.forwardLeft2;
		sensors.forwardLeft3.sensorLength = sensorLength.forwardLeft3;
		sensors.right1.sensorLength = sensorLength.right1;
		sensors.right2.sensorLength = sensorLength.right2;
		sensors.right3.sensorLength = sensorLength.right3;
		sensors.left1.sensorLength = sensorLength.left1;
		sensors.left2.sensorLength = sensorLength.left2;
		sensors.left3.sensorLength = sensorLength.left3;
		sensors.rear1.sensorLength = sensorLength.rear1;
		sensors.rear2.sensorLength = sensorLength.rear2;
		sensors.rear3.sensorLength = sensorLength.rear3;
		sensors.rearRight1.sensorLength = sensorLength.rearRight1;
		sensors.rearRight2.sensorLength = sensorLength.rearRight2;
		sensors.rearRight3.sensorLength = sensorLength.rearRight3;
		sensors.rearLeft1.sensorLength = sensorLength.rearLeft1;
		sensors.rearLeft2.sensorLength = sensorLength.rearLeft2;
		sensors.rearLeft3.sensorLength = sensorLength.rearLeft3;
	}

}