using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;
using System;
using System.Reflection;
using System.Collections.Generic;

public class RG_NetworkLobbyManager : NetworkLobbyManager {

	new public MatchInfo matchInfo;
	private RG_GarageManager garageManager;
	public int[] matchesMaxSize;
	public Dictionary<int, int> currentPlayers;

	void Start(){	
		garageManager = GameObject.Find ("Garage Manager").GetComponent<RG_GarageManager>();
		currentPlayers = new Dictionary<int, int>();
	}

	void Update(){
        Debug.Log(garageManager.playableVehicles.currentVehicleNumber);
        Debug.Log(garageManager.playableVehicles.vehicles.Length);
		//gamePlayerPrefab = garageManager.playableVehicles.vehicles [garageManager.playableVehicles.currentVehicleNumber-2];
	}

	public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId){
		Debug.Log ("NetworkConnection:  " + conn.connectionId);
		if (!currentPlayers.ContainsKey (conn.connectionId)) {
			StartCoroutine (AddPlayerToDictionary(0.5f, conn, playerControllerId));
			//if (lobbySlots [conn.connectionId] != null) {
			//		int vehicleNumber = (int)lobbySlots [conn.connectionId].GetComponent<RG_NetworkLobbyPlayer> ().vehicleNumber;//(short)PlayerPrefs.GetInt ("Vehicle Number");
			//	currentPlayers.Add (conn.connectionId, 0);
			//}
		}
		return base.OnLobbyServerCreateLobbyPlayer (conn, playerControllerId);
	}

	IEnumerator AddPlayerToDictionary(float delay, NetworkConnection conn, short playerControllerId){
		yield return new WaitForSeconds (delay);
		if (lobbySlots [conn.connectionId] != null) {
			Debug.Log ("Asign data in CoRoutine");
			currentPlayers.Add (conn.connectionId, lobbySlots [conn.connectionId].GetComponent<RG_NetworkLobbyPlayer> ().vehicleNumber);
		}
	}

	public void SetPlayerTypeLobby(int conn, int _type){
		if (currentPlayers.ContainsKey (conn))
			currentPlayers [conn] = _type;
	}

	public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId){
		int index =  currentPlayers [conn.connectionId];
		GameObject _temp = (GameObject)GameObject.Instantiate (spawnPrefabs [index], startPositions [conn.connectionId].position, Quaternion.identity);
		return _temp;
	}

	public void SetMatchmakingServer(string matchMakerHostName){
		NetworkLobbyManager.singleton.matchHost = matchMakerHostName;
	}

	public virtual void OnMatchCreate(MatchInfo matchInfo){
		StartHost ();
		this.matchInfo = matchInfo;
		Debug.Log ("OnMatchCreate networkId: " + matchInfo.networkId);
	}


	// Used to remove a spamming debug message
	public override void OnServerConnect(NetworkConnection conn){

		base.OnServerConnect(conn);
		Type serverType = typeof(NetworkServer);
		FieldInfo info = serverType.GetField("maxPacketSize",
			BindingFlags.NonPublic | BindingFlags.Static);
		ushort maxPackets = 1500;
		info.SetValue(null, maxPackets);
	}

	//public override void OnMatchCreate(CreateMatchResponse matchInfo){
	//	if (LogFilter.logDebug) { Debug.Log ("Network Manager OnMatchCreate " + matchInfo); }
	//	if (matchInfo.success) {
	//		Utility.SetAccessTokenForNetwork (matchInfo.networkId, new NetworkAccessToken (matchInfo.accessTokenString));
	//		StartHost (new MatchInfo (matchInfo));
	//		this.matchInfo = matchInfo;
	//		Debug.Log ("OnMatchCreate networkId: " + matchInfo.networkId);
	//	} else {
	//		if (LogFilter.logError) { Debug.LogError ("Create failed: " + matchInfo); }
	//	}
	//}
/*
	public virtual void OnMatchCreate(CreateMatchResponse matchInfo){
		if (LogFilter.logDebug) { 
			Debug.Log ("Network Manager OnMatchCreate " + matchInfo);
		}
		if (matchInfo.success) {
			try{
				Utility.SetAccessTokenForNetwork (matchInfo.networkId, new NetworkAccessToken (matchInfo.accessTokenString));
			}catch (System.ArgumentException) {
				//if (LogFilter.logError) {
				//Debug.LogError (ex);
				//}
			}
			StartHost (new MatchInfo (matchInfo));
			this.matchInfo = matchInfo;
			Debug.Log ("OnMatchCreate networkId: " + matchInfo.networkId);
		} else {
			if (LogFilter.logError) { Debug.LogError ("Create failed: " + matchInfo); }
		}
	}
*/
	/*
	public override void OnMatchList(ListMatchResponse response){
		if(LogFilter.logDebug){
			Debug.Log ("NetworkManager OnMatchList");
		}
		matches = response.matches;
		System.Array.Resize (ref matchesMaxSize, response.matches.Count);
		for (int i = 0; i < response.matches.Count; ++i)
		{
			matchesMaxSize [i] = response.matches[i].maxSize;

		}

	}
	*/
	/*
	public void StartupHost(){
		SetPort ();
		NetworkManager.singleton.StartHost ();
	}

	public void JoinGame(){
		SetIPAddress ();
		SetPort ();
		NetworkManager.singleton.StartClient ();
	}

	void SetIPAddress(){
		string ipAddress = "127.0.0.1";
		NetworkManager.singleton.networkAddress = ipAddress;
	}

	void SetPort(){
		NetworkManager.singleton.networkPort = 7777;
	}

	public void InternetGame(){
		matchMaker.CreateMatch(matchName, matchSize, true, "", OnMatchCreate);
	}

	public void SetupConnectIPButtons(){
		GameObject.Find ("ButtonStartHost").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonStartHost").GetComponent<Button> ().onClick.AddListener(StartupHost);

		GameObject.Find ("ButtonJoinGame").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonJoinGame").GetComponent<Button> ().onClick.AddListener(JoinGame);
	}

	public void SetupMatchmakingButtons(){	
		GameObject.Find ("ButtonMatchmaking").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonMatchmaking").GetComponent<Button> ().onClick.AddListener(InternetGame);
	}

	public void SetupOtherSceneButtons(){
		GameObject.Find ("ButtonDisconnect").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonDisconnect").GetComponent<Button> ().onClick.AddListener(NetworkManager.singleton.StopHost);
	}
	*/

}
