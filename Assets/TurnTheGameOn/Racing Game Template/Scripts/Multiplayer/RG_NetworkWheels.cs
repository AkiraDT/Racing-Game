using UnityEngine;
using System.Collections;

namespace TurnTheGameOn{
	[ExecuteInEditMode]
	public class RG_NetworkWheels : MonoBehaviour {

		public Transform[] frontWheels;
		public Transform[] frontWheelsChild;
		public Transform[] rearWheels;
		//The default rotation values of the wheels
		public Vector3[] fWRotation;
		public Vector3[] rWRotation;
		public RG_SyncData syncVars;
		private float yVelocity;
		public float maxWheelRotation = 45;
		float xRotation;

		// Use this for initialization
		void Start () {
			if (Application.isPlaying) {
				for (int i = 0; i < frontWheels.Length; i++) {
					fWRotation [i] = frontWheels [i].localEulerAngles;
				}

				for (int i = 0; i < rearWheels.Length; i++) {
					rWRotation [i] = rearWheels [i].localEulerAngles;
				}
			}
		}

		// Update is called once per frame
		void Update () {
			if (Application.isPlaying) {
				for (int i = 0; i < frontWheels.Length; i++) {
					Vector3 temp2;
					temp2 = new Vector3 (frontWheels [i].localEulerAngles.x, fWRotation [i].y + (syncVars.horizontalInput * maxWheelRotation), fWRotation [i].z);
					float yAngle = Mathf.SmoothDampAngle (frontWheels [i].localEulerAngles.y, temp2.y, ref yVelocity, 0.07f);
					//float xAngle = Mathf.SmoothDampAngle (frontWheels[i].localEulerAngles.x, temp2.x, ref yVelocity, 0.01f);
					frontWheels [i].localEulerAngles = new Vector3 (temp2.z, yAngle, temp2.z);
					frontWheelsChild [i].Rotate (Vector3.right * (Time.deltaTime * syncVars.wheelRPM * 5));
					//		temp2 = new Vector3 (frontWheels[i].localEulerAngles.x - (syncVars.wheelRPM), frontWheels [i].localEulerAngles.y, frontWheels [i].localEulerAngles.z);
					//		//float yAngle = Mathf.SmoothDampAngle (frontWheels[i].localEulerAngles.y, temp2.y, ref yVelocity, 0.07f);
					//		float xAngle = Mathf.SmoothDampAngle (frontWheels[i].localEulerAngles.x, temp2.x, ref yVelocity, 0.01f);
					//		frontWheels [i].localEulerAngles = new Vector3 (temp2.x, temp2.y, temp2.z);
				}
				for (int i2 = 0; i2 < rearWheels.Length; i2++) {
					rearWheels [i2].Rotate (Vector3.right * (Time.deltaTime * syncVars.wheelRPM * 5));
				}
			}
		}

		public void UpdateWheelSettings(){
			RG_CarController carController = GetComponent<RG_CarController> ();
			//remove old wheel references from front wheels
			frontWheels [0].GetChild(0).transform.parent = frontWheels [0].parent;
			frontWheels [1].GetChild(0).transform.parent = frontWheels [1].parent;
			//setup front right wheel
			frontWheels [0].parent = carController.m_WheelMeshes [0].transform;
			frontWheels [0].position = carController.m_WheelMeshes [0].transform.position;
			frontWheels [0].rotation = carController.m_WheelMeshes [0].transform.rotation;
			frontWheels [0].parent = carController.m_WheelMeshes [0].transform.parent;
			carController.m_WheelMeshes [0].transform.parent = frontWheels [0];
			//setup front left wheel
			frontWheels [1].parent = carController.m_WheelMeshes [1].transform;
			frontWheels [1].position = carController.m_WheelMeshes [1].transform.position;
			frontWheels [1].rotation = carController.m_WheelMeshes [1].transform.rotation;
			frontWheels [1].parent = carController.m_WheelMeshes [1].transform.parent;
			carController.m_WheelMeshes [1].transform.parent = frontWheels [1];
			//assign the new wheels to this scripts references
			frontWheelsChild [0] = carController.m_WheelMeshes [0].transform;
			frontWheelsChild [1] = carController.m_WheelMeshes [1].transform;
			rearWheels [0] = carController.m_WheelMeshes [2].transform;
			rearWheels [1] = carController.m_WheelMeshes [3].transform;

			// make a reference to the colliders original parent
			Transform defaultColliderParent = carController.m_WheelColliders [0].transform.parent;
			// move colliders to the reference positions
			carController.m_WheelColliders [0].transform.parent = carController.m_WheelMeshes [0].transform;
			carController.m_WheelColliders [1].transform.parent = carController.m_WheelMeshes [1].transform;
			carController.m_WheelColliders [2].transform.parent = carController.m_WheelMeshes [2].transform;
			carController.m_WheelColliders [3].transform.parent = carController.m_WheelMeshes [3].transform;
			//adjust the wheel collider positions on x and z axis to match the new wheel position
			carController.m_WheelColliders [0].transform.position = new Vector3 (carController.m_WheelMeshes [0].transform.position.x, 
				carController.m_WheelColliders [0].transform.position.y, carController.m_WheelMeshes [0].transform.position.z);
			carController.m_WheelColliders [1].transform.position = new Vector3 (carController.m_WheelMeshes [1].transform.position.x, 
				carController.m_WheelColliders [1].transform.position.y, carController.m_WheelMeshes [1].transform.position.z);
			carController.m_WheelColliders [2].transform.position = new Vector3 (carController.m_WheelMeshes [2].transform.position.x, 
				carController.m_WheelColliders [2].transform.position.y, carController.m_WheelMeshes [2].transform.position.z);
			carController.m_WheelColliders [3].transform.position = new Vector3 (carController.m_WheelMeshes [3].transform.position.x, 
				carController.m_WheelColliders [3].transform.position.y, carController.m_WheelMeshes [3].transform.position.z);
			// move colliders back to the original parent
			carController.m_WheelColliders [0].transform.parent = defaultColliderParent;
			carController.m_WheelColliders [1].transform.parent = defaultColliderParent;
			carController.m_WheelColliders [2].transform.parent = defaultColliderParent;
			carController.m_WheelColliders [3].transform.parent = defaultColliderParent;
		}

	}
}