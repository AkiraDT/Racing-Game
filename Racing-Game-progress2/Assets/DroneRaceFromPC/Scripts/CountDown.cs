using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

	public string countDown;
	public bool shoowCountDown = false;
	public float spaceCountTime = 1.5f;
	public Text countText;
	public AudioClip Ready;
	public AudioClip Steady;
	public AudioClip Go;
	public AudioClip ring01;
	public AudioClip ring02;
	public AudioSource audioSrc;

	void Start(){
		
		countText.enabled = true;
		StartCoroutine("GetReady");
	}

	IEnumerator GetReady(){
		shoowCountDown = true;    

		audioSrc.PlayOneShot(Ready, 2.0f);
		audioSrc.PlayOneShot(ring01, 1.5f);

		countDown = "GAMEREADY'";
		yield return new WaitForSeconds(spaceCountTime);  

		audioSrc.PlayOneShot(ring01, 1.5f);

		countDown = "3'";    
		yield return new WaitForSeconds(spaceCountTime);  

		audioSrc.PlayOneShot(ring01, 1.5f);
		audioSrc.PlayOneShot(Steady, 2.0f);

		countDown = "2'";    
		yield return new WaitForSeconds(spaceCountTime);

		audioSrc.PlayOneShot(ring01, 1.5f);

		countDown = "1'";    
		yield return new WaitForSeconds(spaceCountTime);

		audioSrc.PlayOneShot(ring02, 1.5f);
		audioSrc.PlayOneShot(Go, 2.0f);

		countDown = "GO!!";    
		yield return new WaitForSeconds(spaceCountTime);

		countText.enabled = false;
		shoowCountDown = false;
	}

	void Update(){
		if (shoowCountDown)
		{    
			countText.color = Color.white;
			countText.text = countDown;
			countText.alignment = TextAnchor.MiddleCenter;
		}
	}
}
