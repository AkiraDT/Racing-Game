using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	public Camera target;
	public float objectScale = 0.1f;
	private Vector3 initialScale; 

	// set the initial scale, and setup reference camera
	void Start ()
	{
		// record initial scale, use this as a basis
		initialScale = transform.localScale; 

		// if no specific camera, grab the default camera
		if (target == null)
			target = Camera.main;
	}

	void Update() 
	{
		transform.LookAt(target.transform.position, Vector3.up);

		Plane plane = new Plane(target.transform.forward, target.transform.position); 
		float dist = plane.GetDistanceToPoint(transform.position); 
		transform.localScale = initialScale * dist * objectScale; 
	}
		

	// scale object relative to distance from camera plane
}
