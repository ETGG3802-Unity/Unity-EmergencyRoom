using UnityEngine;
using System.Collections;

public class QuickNDirtyRightJoystickControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float angH = Input.GetAxis ("RightH");
		float angV = Input.GetAxis ("RightV");
		transform.localEulerAngles = new Vector3(angV, angH, 0);
	}
}
