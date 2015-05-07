using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor; // is this wheel attached to motor?
	public bool steering; // does this wheel apply steer angle?
}
public class SimpleCarController : MonoBehaviour {
	public float curAngle = 0.0f;
	public List<AxleInfo> axleInfos; // the information about each individua axle
	public float maxMotorTorque; // maximum torque the motor can apply to wheel
	public float maxSteeringAngle; // maximum steer angle the wheel can have

	private FirstPersonController fpControl;
	private SimpleCarController thisController;
	private Camera fpCam;
	private Camera carCam;

	void Start(){
		// References to the First Person Controller script and main camera. //
		fpControl = GameObject.Find ("FPSController").GetComponent<FirstPersonController>();
		fpCam = GameObject.Find ("FirstPersonCharacter").GetComponent<Camera>();
		thisController = this; // change if broken
		carCam = gameObject.GetComponent("Main Camera").GetComponent<Camera>();
		// Note that gameObject.enabled will disable this script, and gameObject.GetComponent("Main Camera").GetComponent<Camera>() will
		// get this thing's camera...
	}

	public void Update()
	{
		if (Input.GetKey (KeyCode.P)) {
			fpControl.enabled = true;
			GameObject.Find ("FPSController").transform.position = gameObject.transform.position;
			fpCam.enabled = true;
			thisController.enabled = false;
			carCam.enabled = false;
		}
	}

	public void FixedUpdate()
	{
		int force = 0;
		if (Input.GetKey (KeyCode.UpArrow)) {
			force = 1;
		}
		else if(Input.GetKey (KeyCode.DownArrow)){
			force = -1;
		}
		float motor = maxMotorTorque * force;

		float angle = 0;
		if (Input.GetKey (KeyCode.RightArrow)) {
			angle += .3f;
			if(angle >=1 ){
				angle = 1;
			}
		}
		else if(Input.GetKey (KeyCode.LeftArrow)){
			angle -= .3f;
			if (angle <= -1){
				angle = -1;
			}
		}
		float steering = maxSteeringAngle * angle;
		curAngle = steering;

		foreach (AxleInfo axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if (axleInfo.motor) {
				axleInfo.leftWheel.motorTorque = motor;
				axleInfo.rightWheel.motorTorque = motor;
			}
		}
	}
	public float getAngle()
	{
		return curAngle;
	}
}
