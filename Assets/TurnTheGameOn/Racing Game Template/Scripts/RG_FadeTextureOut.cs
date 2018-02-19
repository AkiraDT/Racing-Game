using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RG_FadeTextureOut : MonoBehaviour {

	public float loadingTextUpdate = 0.25f;
	public string[] loadingTextString;
	public Text loadingText;
	public Image loadingBar;
	public Image backgroundImage;
	public GameObject loadingBarBackground;
	public float startDelay = 3.0f;
	//The higher the value the faster the texture will be faded out
	[Range(0.001f,1f)]public float fadeSpeed = 0.05f;
	private bool alphaWait = true;
	private float alpha = 1.0f;
	private float loadingTextUpdateTimer;
	private int index;
	Color fadeColor;

	void Start () {
		//Unable to Invoke in WebGL due to multithreaded limitation
		//Invoke ("fadeTextureOut", startDelay);
	}

	void Update () {
		loadingTextUpdateTimer += 1 * Time.deltaTime;
		if(Time.timeSinceLevelLoad <= startDelay){
			loadingBar.fillAmount = Time.timeSinceLevelLoad / startDelay * 1;
		}
		if (alphaWait == false) {
			alpha += -1 * fadeSpeed * Time.deltaTime;
			fadeColor = backgroundImage.color;
			fadeColor.a = alpha;
			backgroundImage.color = fadeColor;
		}
		if(loadingText != null){
			if (Time.timeSinceLevelLoad >= startDelay) {
				FadeTextureOut ();
			}else if (loadingTextUpdateTimer >= loadingTextUpdate) {
				loadingTextUpdateTimer = 0.0f;
				index += 1;
				if (index < loadingTextString.Length) {				
					loadingText.text = loadingTextString [index];
				} else {
					index = 0;
					loadingText.text = loadingTextString [index];
				}
			}
		}
		if(alpha <= 0){
			Destroy (gameObject);
		}
	}

	void FadeTextureOut(){
		alphaWait = false;
		Destroy (loadingText);
		Destroy (loadingBarBackground);
	}

}
