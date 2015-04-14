using UnityEngine;
using System.Collections;

public class Main_Input : MonoBehaviour {

	public KeyCode forward, left, right, backward;
	public Vector3 pz;
	public float maxVelocity;
	public GameObject head;


	private float bufferSpacePos, bufferSpaceNeg, deltaRotation, maxPitch;
	private Rigidbody charRigidBody;
	private Vector3 maxVelVector;
	private bool moving;

	// Use this for initialization
	void Start () {
		charRigidBody = gameObject.GetComponent<Rigidbody> ();
		maxVelVector = new Vector3 (charRigidBody.velocity.x, charRigidBody.velocity.y, maxVelocity);
	}

	void FixedUpdate(){
		moving = false;
		if (charRigidBody.velocity.z >= maxVelocity)
			charRigidBody.velocity = maxVelVector;
		if (Input.GetKey(forward)) {
			charRigidBody.AddForce(transform.forward * 0.3f, ForceMode.VelocityChange);
			moving = true;
		}
		if (Input.GetKey (backward)) {
			charRigidBody.AddForce(-transform.forward * 0.3f, ForceMode.VelocityChange);
			moving = true;
		}
		if (Input.GetKey (left)) {
			charRigidBody.AddForce(-transform.right * 0.3f, ForceMode.VelocityChange);
			moving = true;
		}
		if (Input.GetKey (right)) {
			charRigidBody.AddForce(transform.right * 0.3f, ForceMode.VelocityChange);
			moving = true;
		}
		if (!moving) {
			charRigidBody.velocity = Vector3.zero; //stop all movement.
		}
	}
	
	// Update is called once per frame
	void Update () {
	/*
	 * I think what we want to do here is this: simple movement for our character should be easy, but that rotation...
	 * we need to take the x-component of our mouse's movement and map that to the x-rotation of our camera, and the same 
	 * deal for "y".
	*/
		// Stupid simple version:
		pz = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		bufferSpacePos = 0.6f; // the viewport point at which rotation increased along an axis.
		bufferSpaceNeg = 0.4f; // the viewport point at which rotation increased along an axis.
		deltaRotation = 1.7f; // how quickly we rotate around an axis
		maxPitch = 60.0f; // to cap vertical rotation, to be implemented later!

		// "0" Position on ViewPort Space is (0.5, 0.5)
		/**
		 * Current issues:
		 * * This isn't how first-person camera's work!!!
		 * 
		 * Sample rotation code from: http://answers.unity3d.com/questions/288948/rotate-around-local-y-axis.html
		 * transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);
		 **/
		// Vertical Rotation Rotates the CAMERA //
		if (pz.y > bufferSpacePos) {
			head.transform.RotateAround (head.transform.position, head.transform.right, (pz.y - 0.5f) * -deltaRotation);
		}
		if (pz.y < bufferSpaceNeg) {
			head.transform.RotateAround (head.transform.position, head.transform.right, (0.5f - pz.y) * deltaRotation);
		} 

		// Horizontal Rotation Rotates the PLAYER //
		if (pz.x > bufferSpacePos) {
			transform.RotateAround (transform.position, Vector3.up, (pz.x - 0.5f) * deltaRotation);
			//head.transform.RotateAround (head.transform.position, head.transform.up, (pz.x - 0.5f) * deltaRotation);
		}
		if (pz.x < bufferSpaceNeg) {
			transform.RotateAround (transform.position, Vector3.up, (0.5f - pz.x) * -deltaRotation);
			//head.transform.RotateAround (head.transform.position, head.transform.up, (0.5f - pz.x) * -deltaRotation);
		} 
	}

}
