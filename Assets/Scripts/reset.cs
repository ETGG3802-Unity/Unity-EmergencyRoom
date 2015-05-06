using UnityEngine;
using System.Collections;

public class reset : MonoBehaviour {
	float x, y, z;
	float rot_x, rot_y, rot_z;
	// Use this for initialization
	void Start () {
		x = transform.position.x;
		y = transform.position.y;
		z = transform.position.z;
		rot_x = transform.rotation.x;
		rot_y = transform.rotation.y;
		rot_z = transform.rotation.z;

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("r")){
			resetObject();
		}
	}
	void resetObject(){
		transform.position = new Vector3 (x, y, z);
		transform.rotation = new Quaternion(rot_x, rot_y, rot_z,0);
	}
}
