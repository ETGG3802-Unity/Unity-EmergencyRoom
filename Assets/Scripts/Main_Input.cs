using UnityEngine;
using System.Collections;

public class Main_Input : MonoBehaviour {

	public KeyCode forward, backward;
	public Vector3 pz;
	public float maxVelocity;
	public GameObject head;


	private float bufferSpacePos, bufferSpaceNeg, deltaRotation, maxPitch;
	private Rigidbody charRigidBody;
	private bool moving;

	// Use this for initialization
	void Start () {
		charRigidBody = gameObject.GetComponent<Rigidbody> ();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void FixedUpdate(){
		moving = false;

		// Cap player speed! I hate making new Vector3's during the physics loop, though...//
		if (charRigidBody.velocity.z >= maxVelocity)
			charRigidBody.velocity = new Vector3(charRigidBody.velocity.x, charRigidBody.velocity.y, maxVelocity);
		else if (charRigidBody.velocity.z <= -maxVelocity) {
			charRigidBody.velocity = new Vector3(charRigidBody.velocity.x, charRigidBody.velocity.y, -maxVelocity);
		}
		///////////////////////
		/// 
		/// Movement Code! //
		if (Input.GetKey(forward)) {
			charRigidBody.AddForce(transform.forward * 0.6f, ForceMode.VelocityChange);
			moving = true;
		}
		if (Input.GetKey (backward)) {
			charRigidBody.AddForce(-transform.forward * 0.6f, ForceMode.VelocityChange);
			moving = true;
		}
		if (!moving) {
			charRigidBody.velocity = Vector3.zero; //stop all movement.
		}
		/////////////////////////
	}
	
	// Update is called once per frame
	void Update () {
	/*
	 * I think what we want to do here is this: simple movement for our character should be easy, but that rotation...
	 * we need to take the x-component of our mouse's movement and map that to the x-rotation of our camera, and the same 
	 * deal for "y".
	*/
		// Broken feature in Unity 5.1! We have to force-update the state of the cursor... and whether or not
		// it works completely depends on outside influences (state of machine, data corruption, etc.)
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		//////////////////////////////////////////////////////////////////////////////////////////////////////

		maxPitch = 60.0f; // to cap vertical rotation, to be implemented later!
		/**
		 * Current issues:
		 * * WTF is with the unyeilding momentum?
		 * 
		 * Sample rotation code from: http://answers.unity3d.com/questions/288948/rotate-around-local-y-axis.html
		 * transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);
		 **/
		// Vertical Rotation Rotates the CAMERA //
		if (Input.GetAxis("Mouse Y") > 0 || Input.GetAxis ("Mouse Y") < 0) {
			head.transform.RotateAround (head.transform.position, head.transform.right, -Input.GetAxis ("Mouse Y"));
		}
		// Horizontal Rotation Rotates the PLAYER //
		if (Input.GetAxis("Mouse X") > 0 || Input.GetAxis ("Mouse X") < 0) {
			transform.RotateAround (transform.position, Vector3.up, Input.GetAxis("Mouse X"));
			//head.transform.RotateAround (head.transform.position, head.transform.up, (pz.x - 0.5f) * deltaRotation);
		}
	}

}
