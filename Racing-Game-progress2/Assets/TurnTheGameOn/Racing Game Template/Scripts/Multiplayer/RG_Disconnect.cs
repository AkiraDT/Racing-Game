using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;

public class RG_Disconnect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject lobbyManager;
		lobbyManager = GameObject.Find ("LobbyManager");
		if (lobbyManager != null)
			Destroy(lobbyManager);
		NetworkManager.Shutdown ();
	}
	
	// Update is called once per frame
	void Update () {
		SceneManager.LoadScene ("Garage");
	}
}
