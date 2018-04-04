using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour {

	public GameObject AktifParticle;
	public GameObject[] particleList = new GameObject[0];
	RaycastHit hit;
	public Vector3 particlePos;
	public float particleDist;

	void Update(){
		if(particleList == null){
			return;
		}
		particlePos = new Vector3(hit.point.x, hit.point.y + particleDist, hit.point.z /*+ particleDist*/);
		if(Physics.Raycast(transform.position, -Vector3.up, out hit, 3f)){
			particleList[0].gameObject.transform.position = particlePos;
			particleList[1].gameObject.transform.position = particlePos;
		}
	}

	public void GroundCheck(){
		//RaycastHit hit;
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, 25f))//make sure this cast is long enough to reach the ground
		{
			if (hit.collider.CompareTag("Ground"))
			{
				if(Physics.Raycast(transform.position, -Vector3.up, out hit, 3f)){
					AktifParticle = particleList[0];
					//particleList[0].gameObject.GetComponent<ParticleSystem>().maxParticles = 1000;
					//particleList[1].gameObject.GetComponent<ParticleSystem>().maxParticles = 0;
					particleList[1].gameObject.GetComponent<ParticleSystem>().enableEmission = false;
					particleList[0].gameObject.GetComponent<ParticleSystem>().enableEmission = true;
				}else{
					//particleList[0].gameObject.GetComponent<ParticleSystem>().maxParticles = 0;
					//particleList[1].gameObject.GetComponent<ParticleSystem>().maxParticles = 0;
					particleList[0].gameObject.GetComponent<ParticleSystem>().enableEmission = false;
					particleList[1].gameObject.GetComponent<ParticleSystem>().enableEmission = false;
					return;
				}
			}
			else if (hit.collider.CompareTag("Water"))
			{
				if(Physics.Raycast(transform.position, -Vector3.up, out hit, 3f)){
					AktifParticle = particleList[1];
					//particleList[0].gameObject.GetComponent<ParticleSystem>().maxParticles = 0;
					//particleList[1].gameObject.GetComponent<ParticleSystem>().maxParticles = 1000;
					particleList[0].gameObject.GetComponent<ParticleSystem>().enableEmission = false;
					particleList[1].gameObject.GetComponent<ParticleSystem>().enableEmission = true;
				}else{
					//particleList[0].gameObject.GetComponent<ParticleSystem>().maxParticles = 0;
					//particleList[1].gameObject.GetComponent<ParticleSystem>().maxParticles = 0;
					particleList[0].gameObject.GetComponent<ParticleSystem>().enableEmission = false;
					particleList[1].gameObject.GetComponent<ParticleSystem>().enableEmission = false;
					return;
				}
			}
		}
	}
	public void ShotFire(){
		//particleList[2].gameObject.GetComponent<ParticleSystem>().maxParticles = 2;
		//particleList[2].gameObject.GetComponent<ParticleSystem>().emission;
		particleList[2].gameObject.GetComponent<ParticleSystem>().enableEmission = true;
	}
	public void StopFire(){
		//particleList[2].gameObject.GetComponent<ParticleSystem>().maxParticles = 0;
		particleList[2].gameObject.GetComponent<ParticleSystem>().enableEmission = false;
	}
}
