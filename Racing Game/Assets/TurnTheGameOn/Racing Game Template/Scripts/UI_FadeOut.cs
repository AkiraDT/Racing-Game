using UnityEngine;
using UnityEngine.UI;

public class UI_FadeOut : MonoBehaviour {

	public CanvasGroup canvasGroup;
	[Range(0.05f,1f)]public float fadeSpeed = 0.05f;

	void OnEnable(){
		if(canvasGroup == null)
			canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.alpha = 1;
	}

	public void Update(){
		if (canvasGroup.alpha >= 0.001f) {
			canvasGroup.alpha -= 1 * fadeSpeed * Time.deltaTime;
		} else {
			gameObject.SetActive (false);
		}
	}


}

