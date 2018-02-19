using UnityEngine;
using System.Collections;

public class InputData : ScriptableObject {

	public enum Joystick{ None, JoystickButton0, JoystickButton1, JoystickButton2, JoystickButton3, JoystickButton4,
		JoystickButton5, JoystickButton6, JoystickButton7, JoystickButton8, JoystickButton9, JoystickButton10,
		JoystickButton11, JoystickButton12, JoystickButton13, JoystickButton14, JoystickButton15, JoystickButton16,
		JoystickButton17, JoystickButton18, JoystickButton19 }

	public bool useMobileController;

	public KeyCode pauseKey;
	public KeyCode cameraSwitchKey;
	public KeyCode nitroKey;

	public KeyCode pauseJoystick;
	[HideInInspector] public Joystick _pauseJoystick;
	public KeyCode cameraSwitchJoystick;
	[HideInInspector] public Joystick _cameraSwitchJoystick;
	public KeyCode nitroJoystick;
	[HideInInspector] public Joystick _nitroJoystick;

}
