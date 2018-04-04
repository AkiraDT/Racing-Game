using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RG_GarageManager))]
public class Editor_RG_GarageManager : Editor {

	bool showRaceImages;
	bool showNeonParticles;
	bool showCarModels;

	public override void OnInspectorGUI(){
		RG_GarageManager rg_gameManager = (RG_GarageManager)target;
		if(rg_gameManager.raceImage.Length != rg_gameManager.raceData.numberOfRaces){
			System.Array.Resize (ref rg_gameManager.raceImage, rg_gameManager.raceData.numberOfRaces);
		}
		if (rg_gameManager.sceneCarModel.Length != rg_gameManager.playableVehicles.numberOfCars) {
			System.Array.Resize (ref rg_gameManager.sceneCarModel, rg_gameManager.playableVehicles.numberOfCars);
		}
		if (rg_gameManager.sceneCarGlowLight.Length != rg_gameManager.playableVehicles.numberOfCars) {
			System.Array.Resize (ref rg_gameManager.sceneCarGlowLight, rg_gameManager.playableVehicles.numberOfCars);
		}

		showRaceImages = EditorGUILayout.Foldout (showRaceImages, "Race Images");
		if (showRaceImages) {
			for (int i = 0; i < rg_gameManager.raceImage.Length; i++) {
				rg_gameManager.raceImage [i] = (GameObject) EditorGUILayout.ObjectField ("Race " + i, rg_gameManager.raceImage [i], typeof(GameObject), true);
			}
		}
		showNeonParticles = EditorGUILayout.Foldout (showNeonParticles, "Neon Particle Systems");
		if (showNeonParticles) {
			for (int i = 0; i < rg_gameManager.sceneCarGlowLight.Length; i++) {
				rg_gameManager.sceneCarGlowLight [i] = (ParticleSystem) EditorGUILayout.ObjectField ("Neon Particle " + i, rg_gameManager.sceneCarGlowLight [i], typeof(ParticleSystem), true);
			}
		}
		showCarModels = EditorGUILayout.Foldout (showCarModels, "Car Models");
		if (showCarModels) {
			for (int i = 0; i < rg_gameManager.sceneCarModel.Length; i++) {
				rg_gameManager.sceneCarModel [i] = (GameObject) EditorGUILayout.ObjectField ("Car " + i, rg_gameManager.sceneCarModel [i], typeof(GameObject), true);
			}
		}

		SerializedProperty uI = serializedObject.FindProperty ("uI");
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (uI, true);
		if (EditorGUI.EndChangeCheck ())
			serializedObject.ApplyModifiedProperties ();

	}

}