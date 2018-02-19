using UnityEngine;
using System.Collections;

public class ElisaScript : MonoBehaviour {

	public GameObject[] elisa;

	private float currentYRotation;
	private float rotationSpeed = 1000;
	public float idleRotationSpeed = 1000;
	public float movingRotationSpeed = 2000;
	public float elisaAngle;
	public bool spinDifference = true;
	void Update(){
		if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
			Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.K)){
			rotationSpeed = movingRotationSpeed;
		}
		else{
			rotationSpeed = idleRotationSpeed;
		}


		currentYRotation += Time.deltaTime * rotationSpeed;
		for(int i = 0; i< elisa.Length; i++){

			if(spinDifference == true){
				if(i % 2 == 0)	elisa[i].transform.localRotation = Quaternion.Euler(new Vector3(elisaAngle,currentYRotation,transform.rotation.z));
				else	elisa[i].transform.localRotation = Quaternion.Euler(new Vector3(elisaAngle,currentYRotation + 90,transform.rotation.z));
			}
			else{

				elisa[i].transform.localRotation = Quaternion.Euler(new Vector3(elisaAngle,currentYRotation,transform.rotation.z));
			}

		}
	}
}
