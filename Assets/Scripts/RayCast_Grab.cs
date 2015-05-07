using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class RayCast_Grab : MonoBehaviour {

	public GameObject head;
	public float mGrabForwardOffset, mGrabUpOffset;
	[SerializeField] float mRotationSensitivity;
	
	private FirstPersonController fpControl;
	private SimpleCarController carControl;
	private Camera fpCam;
	private Camera carCam;
	private RaycastHit hit;
	private float sightDistance = 10.0f;
	private bool hasObject = false;

	void Start () {
		// References to the First Person Controller script and main camera. //
		fpControl = gameObject.GetComponent<FirstPersonController> ();
		fpCam = GameObject.Find ("FirstPersonCharacter").GetComponent<Camera>();
		///////////////////////////////////////////////////////////////////////
		// References to the Car Controller and main camera. //
		carControl = GameObject.Find ("Stretcher01").GetComponent <SimpleCarController>();
		// Note that the below may not work... //
		carCam = GameObject.Find ("Stretcher01").GetComponent("Main Camera").GetComponent<Camera>();
		// You control the character by default... // 
		carControl.enabled = false; // check later for stupidity...
		carCam.enabled = false;
		fpControl.enabled = true;
		fpCam.enabled = true;
		/////////////////////////////////////////////
		// Eventually, we need to get a specific reference to the RAGDOLL, and set his parts //
		// to NOT be kinematic whenever we release the mouse button. //
	}
	

	void Update () 
	{
		Quaternion moveRotation = head.transform.rotation;
		float horizontal = CrossPlatformInputManager.GetAxis ("Mouse X");
		float vertical = CrossPlatformInputManager.GetAxis ("Mouse Y");
		GameObject.Find ("TargetPosition").transform.RotateAround (head.transform.position, head.transform.right, vertical);
		if (Input.GetMouseButton (0)) {
			if (Physics.Raycast (head.transform.position, head.transform.forward, out hit, sightDistance)) {
				print (horizontal);
				GameObject other = hit.transform.gameObject;
				if (other.tag == "Grabbable") {
					// PHYSICS GRABBING STUFF //
					hasObject = true;
					if (other.GetComponent<Rigidbody> () != null) {
						other.GetComponent<Rigidbody> ().isKinematic = true;
						other.transform.parent = gameObject.transform;
						other.transform.position = Vector3.MoveTowards(other.transform.position, GameObject.Find ("TargetPosition").transform.position, 1.0f);
						other.transform.rotation = moveRotation;
					} else {
						print ("No RIGiDBOdY LOL BitCH");
					}
				} else if (other.tag == "CarController" && !hasObject) {
					// SWITCH CONTROL TO CAR //
					fpControl.enabled = false;
					fpCam.enabled = false;
					carControl.enabled = true;
					carCam.enabled = true;
				} else {
					print ("Watch your trigger finger, nothing to grab here!");
				}
			}
		} else {
			hasObject = false;
			GameObject.Find ("Bip008 Spine").transform.parent = null;
			GameObject.Find ("Bip008 Spine").GetComponent<Rigidbody>().isKinematic = false;
			GameObject.Find ("Cube").transform.parent = null;
			GameObject.Find("Cube").GetComponent<Rigidbody>().isKinematic = false;
		}
	}
}