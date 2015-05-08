using UnityEngine;
using System.Collections;

public class mapIcon : MonoBehaviour {

	// Use this for initialization
	Vector3 originalOri;

	void Start () {
		originalOri = transform.eulerAngles	;
	}
	
	// Update is called once per frame
	void Update () {
		transform.eulerAngles = new Vector3(originalOri.x, transform.eulerAngles.y, transform.eulerAngles.z);
	}
}
