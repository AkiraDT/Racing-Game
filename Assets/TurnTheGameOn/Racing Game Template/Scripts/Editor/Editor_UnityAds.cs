using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

namespace TurnTheGameOn{

	[CustomEditor(typeof(UnityAds))]
	public class Editor_UnityAds : Editor {

		int adType;
		int view;
		GameObject currenctSelection;
		static bool showReferences;
		static bool showInfo;

		public override void OnInspectorGUI(){
			GUISkin editorSkin = Resources.Load("EditorSkin") as GUISkin;
			UnityAds unityAds = (UnityAds)target;

			GUI.skin = editorSkin;
			EditorGUILayout.BeginVertical ("window");


			EditorGUILayout.BeginVertical ("box");
			EditorGUILayout.LabelField ("Rewarded Unity Video Ad", editorSkin.customStyles [3]);
			EditorGUILayout.EndVertical ();

			unityAds.adRewards.rewardCurrency = EditorGUILayout.IntField("Reward Currency 1", unityAds.adRewards.rewardCurrency);
			unityAds.adRewards.adCooldown = (UnityAds.AdRewards.AdCooldown)  EditorGUILayout.EnumPopup("Ad Cooldown", unityAds.adRewards.adCooldown);

			EditorGUILayout.BeginVertical ("box");
			showReferences = EditorGUI.Foldout (EditorGUILayout.GetControlRect(), showReferences, "   > Scene References", true, editorSkin.customStyles [0]);						
			EditorGUILayout.EndVertical();
			if (showReferences) {
				unityAds.adRewards.rewardAd  = (GameObject)EditorGUILayout.ObjectField ("Reward Ad Button", unityAds.adRewards.rewardAd , typeof(GameObject), true);
				unityAds.adRewards.rewardMessage  = (GameObject)EditorGUILayout.ObjectField ("Reward Message", unityAds.adRewards.rewardMessage , typeof(GameObject), true);
				unityAds.adRewards.rewardText  = (Text)EditorGUILayout.ObjectField ("Reward Text", unityAds.adRewards.rewardText , typeof(Text), true);
				unityAds.garageManager = (RG_GarageManager)EditorGUILayout.ObjectField ("Menu Scene Manager", unityAds.garageManager, typeof(RG_GarageManager), true);
				SerializedProperty disableForAds = serializedObject.FindProperty ("disableForAds");
				EditorGUI.BeginChangeCheck ();
				EditorGUILayout.PropertyField (disableForAds, true);
				if (EditorGUI.EndChangeCheck ())
					serializedObject.ApplyModifiedProperties ();
			}


			EditorGUILayout.BeginVertical ("box");


			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Last Ad Time", GUILayout.MaxWidth(Screen.width * 0.4f));
			EditorGUILayout.LabelField (unityAds.adRewards.lastAdTime);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.EndVertical ();

			EditorGUILayout.EndVertical ();

		}
	}
}