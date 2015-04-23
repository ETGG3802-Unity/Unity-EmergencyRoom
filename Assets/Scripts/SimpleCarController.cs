using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
