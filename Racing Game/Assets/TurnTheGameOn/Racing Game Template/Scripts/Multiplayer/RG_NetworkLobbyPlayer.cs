using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class RG_NetworkLobbyPlayer : NetworkBehaviour {

	new public bool isLocalPlayer;
	public byte slot;
	[SyncVar(hook = "OnVehicleNumberChanged")] public int vehicleNumber;
	public Button.ButtonClickedEvent buttonCallback;
	new public NetworkIdentity netId;
	RG_NetworkManagerHUD netManagerHUD;
	RG_NetworkLobbyManager netLobbyManager;

	// Use this for initialization
	void Start () {
		netManagerHUD = GameObject.Find ("LobbyManager").GetComponent<RG_NetworkManagerHUD> ();
		netLobbyManager = GameObject.Find ("LobbyManager").GetComponent<RG_NetworkLobbyManager> ();
		netId = GetComponent<NetworkIdentity> ();
		slot = GetComponent<NetworkLobbyPlayer> ().slot;
		vehicleNumber = PlayerPrefs.GetInt ("Vehicle Number", 0);

		if (netId.isLocalPlayer) {
			CmdVehicleNumber (vehicleNumber);
			netLobbyManager.SetPlayerTypeLobby (slot, vehicleNumber);
			isLocalPlayer = true;
			PlayerPrefs.SetInt ("Slot", slot);
		} else {
			isLocalPlayer = false;
		}
		if(slot == 0 && netId.isLocalPlayer){
			//This player is host
			GameObject.Find("LobbyManager").GetComponent<RG_NetworkManagerHUD>().EnableStartLobbyGameButton();
			GameObject.Find ("Start Lobby Game Button").GetComponent<Button> ().onClick = buttonCallback;
		}
		if (slot == 0) {
			transform.name = "Player " + slot.ToString () + (" (HOST)");
		} else {
			if(isLocalPlayer)
				SetStartFlag ();
			transform.name = "Player " + slot.ToString ();
			//
		}

	}
	
	public void EnableLoadingImage(){
		//netManagerHUD.networkLobbyManager.matchMaker.ma
	//	for(int i = 0; i < netManagerHUD.networkLobbyManager.lobbySlots.Length - 1; i++){
	//		if(netManagerHUD.networkLobbyManager.lobbySlots[i] == null){
	//			netManagerHUD.networkLobbyManager.maxPlayersPerConnection += 1;
	//			netManagerHUD.networkLobbyManager.TryToAddPlayer ();
		//netw
	//		}
	//	}
		RpcEnableLoading ();
		netManagerHUD.loadingMultiplayerScene = true;
		netManagerHUD.lobbyHUDReference.lobbyWindow.SetActive (false);
		Invoke ("SetStartFlag", 0.1f);
	}

	[ClientRpc]
	public void RpcEnableLoading(){
		netManagerHUD.lobbyHUDReference.loadingImage.SetActive (true);
	}

	public void SetStartFlag(){
		GetComponent<NetworkLobbyPlayer> ().SendReadyToBeginMessage ();
	}

	/// <summary>
	/// Commands for SyncVars
	/// </summary>
	[Command]
	void CmdVehicleNumber(int value){
		SetVehicleNumber (value);
	}

	/// <summary>
	/// Client Methods to process SyncVar changes
	/// </summary>
	public void SetVehicleNumber(int value){
		vehicleNumber = value;
	}

	/// <summary>
	/// SyncVar hooks
	/// </summary>
	void OnVehicleNumberChanged(int value){
		vehicleNumber = value;
	}

}