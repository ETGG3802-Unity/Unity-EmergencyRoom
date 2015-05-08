using UnityEngine;
using System.Collections;

public class miniMapCam : MonoBehaviour {

	// Use this for initialization
	GameObject[] character;
	void Start () {
		character = GameObject.FindGameObjectsWithTag ("FPSController");
		//print (character.name);
	}
	
	// Update is called once per frame
	void Update () {
		//transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
		//transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
		//character = GameObject.FindGameObjectsWithTag ("FirstPersonCharacter")[0];
		if (character.Length != 0) {
			Vector3 charPos = character[0].transform.position;
			transform.position = new Vector3 (charPos.x, charPos.y + 50, charPos.z);
			transform.LookAt (charPos);
		} else
			print ("no characters");
	}
}
