using UnityEngine;
using System.Collections;

public class UI_FrameRateCounter : MonoBehaviour {

	public float updateInterval = 0.5f;
	public UnityEngine.UI.Text uiText;
	private float accum = 0.0f;
	private int frames = 0;
	private float timeleft;

	void Start () {
		timeleft = updateInterval;  
	}

	void Update () {
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;

		if( timeleft <= 0.0 ){
			uiText.text = (accum/frames).ToString("F2") + " - FPS";
			timeleft = updateInterval;
			accum = 0.0f;
			frames = 0;
		}
	}
}