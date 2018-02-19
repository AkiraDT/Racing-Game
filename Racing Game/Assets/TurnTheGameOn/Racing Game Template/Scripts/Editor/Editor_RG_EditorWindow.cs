using UnityEngine;
using UnityEditor;


public class Editor_RG_EditorWindow : EditorWindow {
	public enum Switch { Off, On }

	bool audioSettings;
	bool inputSettings;
	bool raceSettings;
	bool opponentVehicleSettings;
	bool playableVehicleSettings;
	bool playerPrefsSettings;

	bool showOpponentNames;
	bool showOpponentPrefabs;

	Vector2 scrollPosition = Vector2.zero;

	AudioData audioData;
	public AudioClip soundClip;

	InputData inputData;

	Switch configureRaceSize;
	Switch configureCarSize;

	RaceData raceData;
	PlayableVehicles playableVehicles;
	PlayerPrefsData playerPrefsData;
	OpponentVehicles opponentVehicles;

	int raceView;
	int carView;
	int opponentCarView;
	int numberOfRaces;
	int numberOfCars;

	GUISkin defaultSkin;

	[MenuItem ("Window/RGT Settings")]
	static void Init () {
		Editor_RG_EditorWindow window = (Editor_RG_EditorWindow)EditorWindow.GetWindow (typeof (Editor_RG_EditorWindow));
		Texture icon = Resources.Load("RGTIcon") as Texture;
		GUIContent titleContent = new GUIContent ("RGT Settings", icon);
		window.titleContent = titleContent;
		window.minSize = new Vector2(300f,500f);
		window.Show();
	}

	void DisableAllTabs(){
		audioSettings = false;
		inputSettings = false;
		raceSettings = false;
		opponentVehicleSettings = false;
		playableVehicleSettings = false;
		playerPrefsSettings = false;
	}

	void OnGUI () {
		if(defaultSkin == null)
			defaultSkin = GUI.skin;
		GUISkin editorSkin = Resources.Load("EditorSkin") as GUISkin;
		GUI.skin = editorSkin;
		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.LabelField ("Racing Game Template", editorSkin.customStyles [1]);
		EditorGUILayout.LabelField ("Project Editor", editorSkin.customStyles [1]);
		EditorGUILayout.LabelField ("v1.066", editorSkin.customStyles [2]);
		EditorGUILayout.BeginHorizontal();
		editorSkin.button.active.textColor = Color.yellow;
		if (audioSettings) {
			editorSkin.button.normal.textColor = Color.yellow;
			editorSkin.button.hover.textColor = Color.yellow;
		}
		else {
			editorSkin.button.normal.textColor = Color.white;
			editorSkin.button.hover.textColor = Color.white;
		}
		if (GUILayout.Button ("Audio", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			DisableAllTabs ();
			audioSettings = true;
		}
		if (inputSettings) {
			editorSkin.button.normal.textColor = Color.yellow;
			editorSkin.button.hover.textColor = Color.yellow;
		}
		else {
			editorSkin.button.normal.textColor = Color.white;
			editorSkin.button.hover.textColor = Color.white;
		}
		if (GUILayout.Button ("Input", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			DisableAllTabs ();
			inputSettings = true;
		}
		if (raceSettings) {
			editorSkin.button.normal.textColor = Color.yellow;
			editorSkin.button.hover.textColor = Color.yellow;
		}
		else {
			editorSkin.button.normal.textColor = Color.white;
			editorSkin.button.hover.textColor = Color.white;
		}
		if (GUILayout.Button ("Races", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			DisableAllTabs ();
			raceSettings = true;
		}
		EditorGUILayout.EndHorizontal ();


		EditorGUILayout.BeginHorizontal ();
		if (playableVehicleSettings) {
			editorSkin.button.normal.textColor = Color.yellow;
			editorSkin.button.hover.textColor = Color.yellow;
		}
		else {
			editorSkin.button.normal.textColor = Color.white;
			editorSkin.button.hover.textColor = Color.white;
		}
		if (GUILayout.Button ("Player Cars", GUILayout.MaxWidth(Screen.width * 0.5f), GUILayout.MaxHeight(40) )) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			DisableAllTabs ();
			playableVehicleSettings = true;
		}

		if (opponentVehicleSettings) {
			editorSkin.button.normal.textColor = Color.yellow;
			editorSkin.button.hover.textColor = Color.yellow;
		}
		else {
			editorSkin.button.normal.textColor = Color.white;
			editorSkin.button.hover.textColor = Color.white;
		}
		if (GUILayout.Button ("AI Cars", GUILayout.MaxWidth(Screen.width * 0.5f), GUILayout.MaxHeight(40) )) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			DisableAllTabs ();
			opponentVehicleSettings = true;
		}
		EditorGUILayout.EndHorizontal ();



		EditorGUILayout.BeginHorizontal ();
		if (playerPrefsSettings) {
			editorSkin.button.normal.textColor = Color.yellow;
			editorSkin.button.hover.textColor = Color.yellow;
		}
		else {
			editorSkin.button.normal.textColor = Color.white;
			editorSkin.button.hover.textColor = Color.white;
		}
		if (GUILayout.Button ("PlayerPrefs", GUILayout.MaxWidth(Screen.width * 0.5f), GUILayout.MaxHeight(35) )) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			DisableAllTabs ();
			playerPrefsSettings = true;
		}
		editorSkin.button.normal.textColor = Color.white;
		editorSkin.button.hover.textColor = Color.white;

		EditorGUILayout.EndHorizontal ();



		editorSkin.button.normal.textColor = Color.grey;


		EditorGUILayout.LabelField ("Unity Project Settings");

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Build Settings", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorWindow.GetWindow (System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
		}
		if (GUILayout.Button ("Services", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Window/Services");
		}
		if (GUILayout.Button ("Lighting", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Window/Lighting");
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Physics", GUILayout.MaxWidth (Screen.width * 0.2f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Physics");
		}
		if (GUILayout.Button ("Quality", GUILayout.MaxWidth (Screen.width * 0.2f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Quality");
		}
		if (GUILayout.Button ("Graphics", GUILayout.MaxWidth (Screen.width * 0.2f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Graphics");
		}
		if (GUILayout.Button ("Tags and Layers", GUILayout.MaxWidth (Screen.width * 0.35f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Tags and Layers");
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Editor", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Editor");
		}
		if (GUILayout.Button ("Player", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Player");
		}
		if (GUILayout.Button ("Script Execution Order", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Script Execution Order");
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();


		if (GUILayout.Button ("Audio", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Audio");
		}
		if (GUILayout.Button ("Time", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Time");
		}
		if (GUILayout.Button ("Network", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Network");
		}
		EditorGUILayout.EndHorizontal ();
		editorSkin.button.normal.textColor = Color.white;
		EditorGUILayout.EndVertical();
		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition, false, false);
/// Audio Settings
		if(audioSettings){
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Audio Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			if(audioData == null){
				audioData = Resources.Load ("AudioData") as AudioData;
			}
			audioData.musicSelection = (AudioData.MusicSelection) EditorGUILayout.EnumPopup ("Music Selection", audioData.musicSelection);
			audioData.numberOfTracks = EditorGUILayout.IntSlider ("Number of Tracks", audioData.numberOfTracks, 0, 50);
			if(audioData.numberOfTracks != audioData.music.Length){
				System.Array.Resize (ref audioData.music, audioData.numberOfTracks);
			}
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Music Tracks", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			for(int i = 0; i < audioData.music.Length; i++){
				audioData.music[i] = (AudioClip) EditorGUILayout.ObjectField ("Track " + i, audioData.music[i], typeof(AudioClip), false);
			}
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("UI Interaction Sounds", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			audioData.menuBack = (AudioClip) EditorGUILayout.ObjectField ("Back", audioData.menuBack, typeof(AudioClip), false);
			audioData.menuSelect = (AudioClip) EditorGUILayout.ObjectField ("Confirm", audioData.menuSelect, typeof(AudioClip), false);
			EditorUtility.SetDirty (audioData);
		}
/// Input Settings
		if(inputSettings){
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Input Manager Axes Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			SerializedObject serializedObject = new SerializedObject (AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);

			SerializedProperty axisProperty = serializedObject.FindProperty ("m_Axes");
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (axisProperty, true);
			if (EditorGUI.EndChangeCheck ())
				serializedObject.ApplyModifiedProperties ();
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Keyboard Input Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			if(inputData == null){
				inputData = Resources.Load ("InputData") as InputData;
			}
			inputData.pauseKey = (KeyCode)EditorGUILayout.EnumPopup ("Pause", inputData.pauseKey);
			inputData.cameraSwitchKey = (KeyCode)EditorGUILayout.EnumPopup ("Camera Switch", inputData.cameraSwitchKey);
			inputData.nitroKey = (KeyCode)EditorGUILayout.EnumPopup ("Nitro", inputData.nitroKey);

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Joystick Input Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();

			inputData._pauseJoystick = (InputData.Joystick)EditorGUILayout.EnumPopup ("Pause", inputData._pauseJoystick);
			switch(inputData._pauseJoystick){
			case InputData.Joystick.JoystickButton0:
				inputData.pauseJoystick = KeyCode.JoystickButton0; 
				break;
			case InputData.Joystick.JoystickButton1:
				inputData.pauseJoystick = KeyCode.JoystickButton1; 
				break;
			case InputData.Joystick.JoystickButton2:
				inputData.pauseJoystick = KeyCode.JoystickButton2; 
				break;
			case InputData.Joystick.JoystickButton3:
				inputData.pauseJoystick = KeyCode.JoystickButton3; 
				break;
			case InputData.Joystick.JoystickButton4:
				inputData.pauseJoystick = KeyCode.JoystickButton4; 
				break;
			case InputData.Joystick.JoystickButton5:
				inputData.pauseJoystick = KeyCode.JoystickButton5; 
				break;
			case InputData.Joystick.JoystickButton6:
				inputData.pauseJoystick = KeyCode.JoystickButton6; 
				break;
			case InputData.Joystick.JoystickButton7:
				inputData.pauseJoystick = KeyCode.JoystickButton7; 
				break;
			case InputData.Joystick.JoystickButton8:
				inputData.pauseJoystick = KeyCode.JoystickButton8; 
				break;
			case InputData.Joystick.JoystickButton9:
				inputData.pauseJoystick = KeyCode.JoystickButton9; 
				break;
			case InputData.Joystick.JoystickButton10:
				inputData.pauseJoystick = KeyCode.JoystickButton10; 
				break;
			case InputData.Joystick.JoystickButton11:
				inputData.pauseJoystick = KeyCode.JoystickButton11; 
				break;
			case InputData.Joystick.JoystickButton12:
				inputData.pauseJoystick = KeyCode.JoystickButton12; 
				break;
			case InputData.Joystick.JoystickButton13:
				inputData.pauseJoystick = KeyCode.JoystickButton13; 
				break;
			case InputData.Joystick.JoystickButton14:
				inputData.pauseJoystick = KeyCode.JoystickButton14; 
				break;
			case InputData.Joystick.JoystickButton15:
				inputData.pauseJoystick = KeyCode.JoystickButton15; 
				break;
			case InputData.Joystick.JoystickButton16:
				inputData.pauseJoystick = KeyCode.JoystickButton16; 
				break;
			case InputData.Joystick.JoystickButton17:
				inputData.pauseJoystick = KeyCode.JoystickButton17; 
				break;
			case InputData.Joystick.JoystickButton18:
				inputData.pauseJoystick = KeyCode.JoystickButton18; 
				break;
			case InputData.Joystick.JoystickButton19:
				inputData.pauseJoystick = KeyCode.JoystickButton19;
				break;
			}
			inputData._cameraSwitchJoystick = (InputData.Joystick)EditorGUILayout.EnumPopup ("Camera Switch", inputData._cameraSwitchJoystick);
			switch(inputData._cameraSwitchJoystick){
			case InputData.Joystick.JoystickButton0:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton0; 
				break;
			case InputData.Joystick.JoystickButton1:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton1; 
				break;
			case InputData.Joystick.JoystickButton2:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton2; 
				break;
			case InputData.Joystick.JoystickButton3:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton3; 
				break;
			case InputData.Joystick.JoystickButton4:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton4; 
				break;
			case InputData.Joystick.JoystickButton5:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton5; 
				break;
			case InputData.Joystick.JoystickButton6:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton6; 
				break;
			case InputData.Joystick.JoystickButton7:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton7; 
				break;
			case InputData.Joystick.JoystickButton8:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton8; 
				break;
			case InputData.Joystick.JoystickButton9:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton9; 
				break;
			case InputData.Joystick.JoystickButton10:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton10; 
				break;
			case InputData.Joystick.JoystickButton11:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton11; 
				break;
			case InputData.Joystick.JoystickButton12:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton12; 
				break;
			case InputData.Joystick.JoystickButton13:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton13; 
				break;
			case InputData.Joystick.JoystickButton14:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton14; 
				break;
			case InputData.Joystick.JoystickButton15:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton15; 
				break;
			case InputData.Joystick.JoystickButton16:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton16; 
				break;
			case InputData.Joystick.JoystickButton17:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton17; 
				break;
			case InputData.Joystick.JoystickButton18:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton18; 
				break;
			case InputData.Joystick.JoystickButton19:
				inputData.cameraSwitchJoystick = KeyCode.JoystickButton19;
				break;
			}
			inputData._nitroJoystick = (InputData.Joystick)EditorGUILayout.EnumPopup ("Nitro", inputData._nitroJoystick);
			switch(inputData._nitroJoystick){
			case InputData.Joystick.JoystickButton0:
				inputData.nitroJoystick = KeyCode.JoystickButton0; 
				break;
			case InputData.Joystick.JoystickButton1:
				inputData.nitroJoystick = KeyCode.JoystickButton1; 
				break;
			case InputData.Joystick.JoystickButton2:
				inputData.nitroJoystick = KeyCode.JoystickButton2; 
				break;
			case InputData.Joystick.JoystickButton3:
				inputData.nitroJoystick = KeyCode.JoystickButton3; 
				break;
			case InputData.Joystick.JoystickButton4:
				inputData.nitroJoystick = KeyCode.JoystickButton4; 
				break;
			case InputData.Joystick.JoystickButton5:
				inputData.nitroJoystick = KeyCode.JoystickButton5; 
				break;
			case InputData.Joystick.JoystickButton6:
				inputData.nitroJoystick = KeyCode.JoystickButton6; 
				break;
			case InputData.Joystick.JoystickButton7:
				inputData.nitroJoystick = KeyCode.JoystickButton7; 
				break;
			case InputData.Joystick.JoystickButton8:
				inputData.nitroJoystick = KeyCode.JoystickButton8; 
				break;
			case InputData.Joystick.JoystickButton9:
				inputData.nitroJoystick = KeyCode.JoystickButton9; 
				break;
			case InputData.Joystick.JoystickButton10:
				inputData.nitroJoystick = KeyCode.JoystickButton10; 
				break;
			case InputData.Joystick.JoystickButton11:
				inputData.nitroJoystick = KeyCode.JoystickButton11; 
				break;
			case InputData.Joystick.JoystickButton12:
				inputData.nitroJoystick = KeyCode.JoystickButton12; 
				break;
			case InputData.Joystick.JoystickButton13:
				inputData.nitroJoystick = KeyCode.JoystickButton13; 
				break;
			case InputData.Joystick.JoystickButton14:
				inputData.nitroJoystick = KeyCode.JoystickButton14; 
				break;
			case InputData.Joystick.JoystickButton15:
				inputData.nitroJoystick = KeyCode.JoystickButton15; 
				break;
			case InputData.Joystick.JoystickButton16:
				inputData.nitroJoystick = KeyCode.JoystickButton16; 
				break;
			case InputData.Joystick.JoystickButton17:
				inputData.nitroJoystick = KeyCode.JoystickButton17; 
				break;
			case InputData.Joystick.JoystickButton18:
				inputData.nitroJoystick = KeyCode.JoystickButton18; 
				break;
			case InputData.Joystick.JoystickButton19:
				inputData.nitroJoystick = KeyCode.JoystickButton19;
				break;
			}

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Mobile Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			inputData.useMobileController = EditorGUILayout.Toggle("Use Mobile Controller", inputData.useMobileController);
			EditorUtility.SetDirty (inputData);
		}
/// Race Settings
		if(raceSettings){
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Race Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			if(raceData == null){
				raceData = Resources.Load ("RaceData") as RaceData;
			}
			configureRaceSize = (Switch) EditorGUILayout.EnumPopup ("Configure Race Size", configureRaceSize);
			if (configureRaceSize == Switch.On) {
				EditorGUILayout.HelpBox ("When you reduce this number the values of the affected arrays are deleted. Only reduce this number if you want fewer races.", MessageType.Warning);
				EditorGUILayout.BeginHorizontal ();
				numberOfRaces = EditorGUILayout.IntField ("Number Of Races", numberOfRaces);
				if (GUILayout.Button ("Update")) {
					GUIUtility.hotControl = 0;
					GUIUtility.keyboardControl = 0;
					raceData.numberOfRaces = numberOfRaces;
					raceView = 0;
				}
				EditorGUILayout.EndHorizontal ();
				System.Array.Resize (ref raceData.raceNames, raceData.numberOfRaces);
				System.Array.Resize (ref raceData.numberOfRacers, raceData.numberOfRaces);
				System.Array.Resize (ref raceData.raceLaps, raceData.numberOfRaces);
				System.Array.Resize (ref raceData.lapLimit, raceData.numberOfRaces);
				System.Array.Resize (ref raceData.racerLimit, raceData.numberOfRaces);
				System.Array.Resize (ref raceData.raceLocked, raceData.numberOfRaces);
				System.Array.Resize (ref raceData.unlockAmount, raceData.numberOfRaces);
				System.Array.Resize (ref raceData.firstPrize, raceData.numberOfRaces);
				System.Array.Resize (ref raceData.secondPrize, raceData.numberOfRaces);
				System.Array.Resize (ref raceData.thirdPrize, raceData.numberOfRaces);
				System.Array.Resize (ref raceData.readyTime, raceData.numberOfRaces);
				System.Array.Resize (ref raceData.extraLapRewardMultiplier, raceData.numberOfRaces);
				System.Array.Resize (ref raceData.extraRacerRewardMultiplier, raceData.numberOfRaces);
				System.Array.Resize (ref raceData.unlimitedRewards, raceData.numberOfRaces);
			} else if(raceData.numberOfRaces != numberOfRaces){
				numberOfRaces = raceData.numberOfRaces;
			}
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button ("<", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
				GUIUtility.hotControl = 0;
				GUIUtility.keyboardControl = 0;
				if (raceView > 0) {
					raceView -= 1;
				} else {
					raceView = raceData.numberOfRaces - 1;
				}
			}
			GUILayout.Label("Race\n" + raceView.ToString(), GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) );
			if (GUILayout.Button (">", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
				GUIUtility.hotControl = 0;
				GUIUtility.keyboardControl = 0;
				if (raceView < raceData.numberOfRaces - 1) {
					raceView += 1;
				} else {
					raceView = 0;
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginVertical();
			raceData.raceNames[raceView] = EditorGUILayout.TextField("Race Name", raceData.raceNames[raceView]);
			raceData.raceLocked[raceView] = EditorGUILayout.Toggle("Race Locked", raceData.raceLocked[raceView]);
			if(raceData.purchaseLevelUnlock == RaceData.Switch.On)
				raceData.unlockAmount[raceView] = EditorGUILayout.IntField("Unlock Amount", raceData.unlockAmount[raceView]);
			raceData.unlimitedRewards[raceView] = EditorGUILayout.Toggle("Unlimited Rewards", raceData.unlimitedRewards[raceView]);
			raceData.firstPrize[raceView] = EditorGUILayout.IntField("First Prize", raceData.firstPrize[raceView]);
			raceData.secondPrize[raceView] = EditorGUILayout.IntField("Second Prize", raceData.secondPrize[raceView]);
			raceData.thirdPrize[raceView] = EditorGUILayout.IntField("Third Prize", raceData.thirdPrize[raceView]);
			raceData.extraLapRewardMultiplier[raceView] = EditorGUILayout.IntField("Extra Lap Multiplier", raceData.extraLapRewardMultiplier[raceView]);
			raceData.extraRacerRewardMultiplier[raceView] = EditorGUILayout.IntField("Extra Racer Multiplier", raceData.extraRacerRewardMultiplier[raceView]);
			raceData.numberOfRacers[raceView] = EditorGUILayout.IntSlider("Number of Racers", raceData.numberOfRacers[raceView], 1, 64);
			raceData.racerLimit[raceView] = EditorGUILayout.IntSlider ("Racer Limit", raceData.racerLimit[raceView], 1, 64);
			raceData.raceLaps[raceView] = EditorGUILayout.IntField("Number of Laps", raceData.raceLaps[raceView]);
			raceData.lapLimit[raceView] = EditorGUILayout.IntField("Lap Limit", raceData.lapLimit[raceView]);
			raceData.readyTime[raceView] = EditorGUILayout.FloatField("Ready Time", raceData.readyTime[raceView]);
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("General Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			raceData.autoUnlockNextRace = (RaceData.Switch) EditorGUILayout.EnumPopup ("Autounlock Next Race", raceData.autoUnlockNextRace);
			raceData.purchaseLevelUnlock = (RaceData.Switch) EditorGUILayout.EnumPopup ("Purchase Level Unlocks", raceData.purchaseLevelUnlock);
			raceData.lockButtonText = EditorGUILayout.TextField ("Locked Button Text", raceData.lockButtonText);
			raceData.wrongWayDelay = EditorGUILayout.FloatField("Wrong Way Delay", raceData.wrongWayDelay);
			EditorUtility.SetDirty (raceData);
		}
/// Opponent Vehicles Settings
		if(opponentVehicleSettings){
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("AI Vehicle Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			if(opponentVehicles == null){
				opponentVehicles = Resources.Load ("OpponentVehicles") as OpponentVehicles;
			}
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button ("<", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
				GUIUtility.hotControl = 0;
				GUIUtility.keyboardControl = 0;
				if (opponentCarView > 0) {
					opponentCarView -= 1;
				} else {
					opponentCarView = 62;
				}
			}
			GUILayout.Label("Opponent\n" + opponentCarView.ToString(), GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) );
			if (GUILayout.Button (">", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
				GUIUtility.hotControl = 0;
				GUIUtility.keyboardControl = 0;
				if (opponentCarView < 62) {
					opponentCarView += 1;
				} else {
					opponentCarView = 0;
				}
			}
			EditorGUILayout.EndHorizontal();
			GUI.skin = defaultSkin;
			opponentVehicles.opponentNames[opponentCarView] = EditorGUILayout.TextField("Name", opponentVehicles.opponentNames[opponentCarView]);
			opponentVehicles.vehicles[opponentCarView] = (GameObject) EditorGUILayout.ObjectField("Prefab", opponentVehicles.vehicles[opponentCarView], typeof (GameObject), false );
			opponentVehicles.opponentBodyMaterials[opponentCarView] = (Material) EditorGUILayout.ObjectField("Body Material", opponentVehicles.opponentBodyMaterials[opponentCarView], typeof (Material), false );
			opponentVehicles.opponentBodyColors[opponentCarView] = EditorGUILayout.ColorField("Body Color", opponentVehicles.opponentBodyColors[opponentCarView]);
			if(opponentVehicles.opponentBodyMaterials[opponentCarView] != null){
				opponentVehicles.opponentBodyMaterials [opponentCarView].color = opponentVehicles.opponentBodyColors [opponentCarView];
			}
			EditorUtility.SetDirty (opponentVehicles);
		}
/// Player Vehicles Settings
		if(playableVehicleSettings){
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Player Vehicle Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			if(playableVehicles == null){
				playableVehicles = Resources.Load ("PlayableVehicles") as PlayableVehicles;
			}
			if(playerPrefsData == null){
				playerPrefsData = Resources.Load ("PlayerPrefsData") as PlayerPrefsData;
			}
			configureCarSize = (Switch) EditorGUILayout.EnumPopup ("Configure Car Size", configureCarSize);
			if (configureCarSize == Switch.On) {
				EditorGUILayout.HelpBox ("When you reduce this number the values of the affected arrays are deleted. Only reduce this number if you want fewer playable vehicles.", MessageType.Warning);
				EditorGUILayout.BeginHorizontal ();
				numberOfCars = EditorGUILayout.IntField ("Number Of Cars", numberOfCars);
				if (GUILayout.Button ("Update")) {
					GUIUtility.hotControl = 0;
					GUIUtility.keyboardControl = 0;
					playableVehicles.numberOfCars = numberOfCars;
					carView = 0;
				}
				EditorGUILayout.EndHorizontal ();
				System.Array.Resize (ref playableVehicles.vehicles, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.vehicleNames, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.price, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.carMaterial, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.brakeMaterial, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.glassMaterial, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.rimMaterial, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.defaultBodyColors, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.defaultBrakeColors, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.defaultGlassColors, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.defaultRimColors, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.defaultNeonColors, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.carGlowLight, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.carUnlocked, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.topSpeedLevel, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.torqueLevel, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.brakeTorqueLevel, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.tireTractionLevel, playableVehicles.numberOfCars);
				System.Array.Resize (ref playableVehicles.steerSensitivityLevel, playableVehicles.numberOfCars);

				System.Array.Resize (ref playerPrefsData.redValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.blueValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.greenValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.redGlowValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.blueGlowValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.greenGlowValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.redGlassValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.blueGlassValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.greenGlassValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.alphaGlassValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.redBrakeValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.blueBrakeValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.greenBrakeValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.redRimValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.blueRimValues, playableVehicles.numberOfCars);
				System.Array.Resize (ref playerPrefsData.greenRimValues, playableVehicles.numberOfCars);
			} else if(playableVehicles.numberOfCars != numberOfCars){
				numberOfCars = playableVehicles.numberOfCars;
			}
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button ("<", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
				GUIUtility.hotControl = 0;
				GUIUtility.keyboardControl = 0;
				if (carView > 0) {
					carView -= 1;
				} else {
					carView = playableVehicles.numberOfCars - 1;
				}
			}
			GUILayout.Label("Car\n" + carView.ToString(), GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) );
			if (GUILayout.Button (">", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
				GUIUtility.hotControl = 0;
				GUIUtility.keyboardControl = 0;
				if (carView < playableVehicles.numberOfCars - 1) {
					carView += 1;
				} else {
					carView = 0;
				}
			}
			EditorGUILayout.EndHorizontal();
			if (playableVehicles.numberOfCars > 0) {
				playableVehicles.vehicleNames [carView] = EditorGUILayout.TextField ("Name", playableVehicles.vehicleNames [carView]);
				playableVehicles.price [carView] = EditorGUILayout.IntField ("Price", playableVehicles.price [carView]);
				playableVehicles.carUnlocked [carView] = EditorGUILayout.Toggle ("Unlocked", playableVehicles.carUnlocked [carView]);
				playableVehicles.vehicles [carView] = (GameObject)EditorGUILayout.ObjectField ("Prefab", playableVehicles.vehicles [carView], typeof(GameObject), false);
				playableVehicles.carMaterial [carView] = (Material)EditorGUILayout.ObjectField ("Body Material", playableVehicles.carMaterial [carView], typeof(Material), false);
				playableVehicles.brakeMaterial [carView] = (Material)EditorGUILayout.ObjectField ("Brake Material", playableVehicles.brakeMaterial [carView], typeof(Material), false);
				playableVehicles.glassMaterial [carView] = (Material)EditorGUILayout.ObjectField ("Glass Material", playableVehicles.glassMaterial [carView], typeof(Material), false);
				playableVehicles.rimMaterial [carView] = (Material)EditorGUILayout.ObjectField ("Rim Material", playableVehicles.rimMaterial [carView], typeof(Material), false);
				playableVehicles.carGlowLight [carView] = (ParticleSystem)EditorGUILayout.ObjectField ("Neon Particle", playableVehicles.carGlowLight [carView], typeof(ParticleSystem), false);
//new colors
				GUI.skin = defaultSkin;
				playableVehicles.defaultBodyColors [carView] = EditorGUILayout.ColorField ("Default Body Color", playableVehicles.defaultBodyColors [carView]);
				playableVehicles.defaultBrakeColors [carView] = EditorGUILayout.ColorField ("Default Brake Color", playableVehicles.defaultBrakeColors [carView]);
				playableVehicles.defaultGlassColors [carView] = EditorGUILayout.ColorField ("Default Glass Color", playableVehicles.defaultGlassColors [carView]);
				playableVehicles.defaultRimColors [carView] = EditorGUILayout.ColorField ("Default Rim Color", playableVehicles.defaultRimColors [carView]);
				playableVehicles.defaultNeonColors [carView] = EditorGUILayout.ColorField ("Default Neon Color", playableVehicles.defaultNeonColors [carView]);
				GUI.skin = editorSkin;
				EditorGUILayout.BeginVertical ("Box");
				EditorGUILayout.LabelField ("Starting Currency", editorSkin.customStyles [0]);
				EditorGUILayout.EndVertical ();
				playableVehicles.startingCurrency = EditorGUILayout.IntField ("Starting Currency", playableVehicles.startingCurrency);
				EditorGUILayout.BeginVertical ("Box");
				EditorGUILayout.LabelField ("Upgrades & Customization", editorSkin.customStyles [0]);
				EditorGUILayout.EndVertical ();
				playableVehicles.paintPrice = EditorGUILayout.IntField ("Body Paint", playableVehicles.paintPrice);
				playableVehicles.brakeColorPrice = EditorGUILayout.IntField ("Brake Color", playableVehicles.brakeColorPrice);
				playableVehicles.rimColorPrice = EditorGUILayout.IntField ("Rim Color", playableVehicles.rimColorPrice);
				playableVehicles.glassColorPrice = EditorGUILayout.IntField ("Glass Tint", playableVehicles.glassColorPrice);
				playableVehicles.glowPrice = EditorGUILayout.IntField ("Neon Light", playableVehicles.glowPrice);
				playableVehicles.upgradeSpeedPrice = EditorGUILayout.IntField ("Upgrade Speed", playableVehicles.upgradeSpeedPrice);
				playableVehicles.upgradeAccelerationPrice = EditorGUILayout.IntField ("Upgrade Acceleration", playableVehicles.upgradeAccelerationPrice);
				playableVehicles.upgradeBrakesPrice = EditorGUILayout.IntField ("Upgrade Brakes", playableVehicles.upgradeBrakesPrice);
				playableVehicles.upgradeTiresPrice = EditorGUILayout.IntField ("Upgrade Tires", playableVehicles.upgradeTiresPrice);
				playableVehicles.upgradeSteeringPrice = EditorGUILayout.IntField ("Upgrade Steering", playableVehicles.upgradeSteeringPrice);
			}
			EditorUtility.SetDirty (playableVehicles);
		}
/// PlayerPrefs Settings
		if(playerPrefsSettings){
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("PlayerPrefs Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			if(playerPrefsData == null){
				playerPrefsData = Resources.Load ("PlayerPrefsData") as PlayerPrefsData;
			}
			if (GUILayout.Button ("Delete PlayerPrefs Data")) {
				GUIUtility.hotControl = 0;
				GUIUtility.keyboardControl = 0;
				DeleteAllPlayerPrefsData ();
			}
		}
		EditorGUILayout.EndScrollView ();
	}

	void DeleteAllPlayerPrefsData(){
		if (EditorUtility.DisplayDialog ("Racing Game Template", "Are you sure you want to delete all PlayerPrefs?", "Yes", "No")) {
			if(playableVehicles == null){
				playableVehicles = Resources.Load ("PlayableVehicles") as PlayableVehicles;
			}
			for(int i = 1; i < playableVehicles.numberOfCars; i ++){
				playableVehicles.carUnlocked [i] = false;
			}
			PlayerPrefs.DeleteAll ();
			Debug.Log ("Deleted PlayerPrefs Data");
		}
	}

}